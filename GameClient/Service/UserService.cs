//=============================
//Author: Zack Yang 
//Created Date: 10/12/2020 22:39
//=============================

using System;
using System.Collections;
using System.Collections.Generic;
using Common;
using JetBrains.Annotations;
using Network;
using SkillBridge.Message;
using UnityEngine;
using UnityEngine.Events;

namespace Services
{
    public class UserService : Singleton<UserService>, IDisposable
    {
        //fields

        #region UnityActions to be called
        /// <summary>
        /// onLogin actions
        /// </summary>
        public UnityAction<Result, string> OnLogin;
        
        /// <summary>
        /// onRegister actions
        /// </summary>
        public UnityAction<Result, string> OnRegister;

        /// <summary>
        /// OnCreateCharcter actions
        /// </summary>
        public UnityAction<Result, string> OnCreateCharacter;

        /// <summary>
        /// onGameEnter actions
        /// </summary>
        public UnityAction<Result, string> OnGameEnter;

        #endregion

        #region other fields to be used in userservice class

        /// <summary>
        /// the message that about to be sent
        /// </summary>
        private NetMessage pendingMessage = null;

        /// <summary>
        /// true if client is connected to server, otherwise false
        /// </summary>
        private bool connected = false;

        /// <summary>
        /// true if user have already quitted game, otherwise false
        /// </summary>
        private bool isQuitGame = false;

        #endregion

        //methods

        #region constructor and deconstructor, start and stop event listener

        /// <summary>
        /// constructor, start listen to different events
        /// </summary>
        public UserService()
        {
            NetClient.Instance.OnConnect += OnGameServerConnect;
            NetClient.Instance.OnDisconnect += OnGameServerDisconnect;
            
            MessageDistributer.Instance.Subscribe<UserLoginResponse>(this.OnUserLogin);
            MessageDistributer.Instance.Subscribe<UserRegisterResponse>(this.OnUserRegister);
            MessageDistributer.Instance.Subscribe<UserCreateCharacterResponse>(this.OnUserCreateCharacter);
            MessageDistributer.Instance.Subscribe<UserGameEnterResponse>(this.OnUserGameEnter);
        }


        /// <summary>
        /// method to be called when disconnected, the userService will then stop listen to response from server
        /// </summary>
        public void Dispose()
        {
            MessageDistributer.Instance.Unsubscribe<UserLoginResponse>(this.OnUserLogin);
            MessageDistributer.Instance.Unsubscribe<UserRegisterResponse>(this.OnUserRegister);
            MessageDistributer.Instance.Unsubscribe<UserCreateCharacterResponse>(this.OnUserCreateCharacter);
            MessageDistributer.Instance.Unsubscribe<UserGameEnterResponse>(this.OnUserGameEnter);


            NetClient.Instance.OnConnect -= OnGameServerConnect;
            NetClient.Instance.OnDisconnect -= OnGameServerDisconnect;
        }


        public void Init()
        {
        }

        #endregion

        #region Methods about server connections

        /// <summary>
        /// Use this method to connect to server
        /// </summary>
        public void ConnectToServer()
        {
            Debug.Log("ConnectToServer() start");

            Network.NetClient.Instance.Init("127.0.0.1", 7878);
            Network.NetClient.Instance.Connect();
        }

        /// <summary>
        /// this method is automatically called when client connect to server
        /// </summary>
        /// <param name="result"></param>
        /// <param name="reason"></param>
        private void OnGameServerConnect(int result, string reason)
        {
            //Log.InfoFormat("LoadingMesager::OnGameServerConnect :{0} reason:{1}", result, reason);
            if (NetClient.Instance.Connected)
            {
                this.connected = true;
                if (this.pendingMessage != null)
                {
                    NetClient.Instance.SendMessage(this.pendingMessage);
                    this.pendingMessage = null;
                }
            }
            else
            {
                if (!this.DisconnectNotify(result, reason))
                {
                    UIManager.Instance.ShowPanel<NetworkErrorPanel>(typeof(NetworkErrorPanel));
                }
            }
        }

        /// <summary>
        /// this method is automatically called when client disconnect from server
        /// </summary>
        /// <param name="result"></param>
        /// <param name="reason"></param>
        private void OnGameServerDisconnect(int result, string reason)
        {
            DisconnectNotify(result, reason);
        }

        /// <summary>
        /// id disconnect failed, execute events
        /// </summary>
        /// <param name="result"></param>
        /// <param name="reason"></param>
        /// <returns></returns>
        private bool DisconnectNotify(int result, string reason)
        {
            if (this.pendingMessage != null)
            {
                if (this.pendingMessage.Request.userLogin != null)
                {
                    if (this.OnLogin != null)
                    {
                        this.OnLogin(Result.Failed, string.Format("Disconnected from server!\n RESULT:{0} ERROR:{1}", result, reason));
                    }
                }
                else if (this.pendingMessage.Request.userRegister != null)
                {
                    if (this.OnRegister != null)
                    {
                        this.OnRegister(Result.Failed, string.Format("Disconnected from server!\n RESULT:{0} ERROR:{1}", result, reason));
                    }
                }

                return true;
            }
            return false;
        }

        #endregion

        #region user login service

        /// <summary>
        /// send the user login request to server
        /// </summary>
        /// <param name="username"></param>
        /// <param name="pwd"></param>
        public void SendLogin(string username, string pwd)
        {
            Debug.LogFormat("UserLoginRequest::user :{0} psw:{1}", username, pwd);

            NetMessage message = new NetMessage();
            message.Request = new NetMessageRequest();
            message.Request.userLogin = new UserLoginRequest();

            message.Request.userLogin.User = username;
            message.Request.userLogin.Passward = pwd;

            if (this.connected && NetClient.Instance.Connected)
            {
                this.pendingMessage = null;
                NetClient.Instance.SendMessage(message);
            }

            else
            {
                pendingMessage = message;
                this.ConnectToServer();
            }
        }

        /// <summary>
        /// this method is automatically called when client receive response from server
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="response"></param>
        private void OnUserLogin(object sender, UserLoginResponse response)
        {
            Debug.LogFormat("OnLogin:{0} [{1}]", response.Result, response.Errormsg);

            if (response.Result == Result.Success)
            {//登陆成功逻辑
                Models.User.Instance.SetupUserInfo(response.Userinfo);
            };
            if (this.OnLogin != null)
            {
                this.OnLogin(response.Result, response.Errormsg);

            }
        }

        #endregion

        #region user register service

        /// <summary>
        /// send the user register request to server
        /// </summary>
        /// <param name="username"></param>
        /// <param name="pwd"></param>
        public void SendRegister(string username, string pwd)
        {
            Debug.LogFormat("UserRegisterRequest:: User: {0}  pwd: {1}", username, pwd);

            NetMessage message = new NetMessage();
            message.Request = new NetMessageRequest();
            message.Request.userRegister = new UserRegisterRequest();

            message.Request.userRegister.User = username;
            message.Request.userRegister.Passward = pwd;

            if (this.connected && NetClient.Instance.Connected)
            {
                this.pendingMessage = null;
                NetClient.Instance.SendMessage(message);
            }
            else
            {
                this.pendingMessage = message;
                this.ConnectToServer();
            }
        }

        /// <summary>
        /// this method is automatically called when receive register response from server
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="response"></param>
        private void OnUserRegister(object sender, UserRegisterResponse response)
        {
            Debug.LogFormat("OnRegister:{0} [{1}]", response.Result, response.Errormsg);

            if (this.OnRegister != null)
            {
                this.OnRegister(response.Result, response.Errormsg);
            }
        }

        #endregion

        #region user create character service

        /// <summary>
        /// send the user create new character request to server
        /// </summary>
        /// <param name="charName"></param>
        /// <param name="charClass"></param>
        public void SendCreateChar(string charName, CharacterClass charClass)
        {
            Debug.LogFormat("UserCharacterCreateRequest::charName :{0} class:{1}", charName, charClass);

            NetMessage message = new NetMessage();
            message.Request = new NetMessageRequest();
            message.Request.createChar = new UserCreateCharacterRequest();

            message.Request.createChar.Name = charName;
            message.Request.createChar.Class = charClass;


            if (this.connected && NetClient.Instance.Connected)
            {
                this.pendingMessage = null;
                NetClient.Instance.SendMessage(message);
            }

            else
            {
                pendingMessage = message;
                this.ConnectToServer();
            }
        }

        /// <summary>
        /// this method is automatically called when client receive response from server
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="response"></param>
        private void OnUserCreateCharacter(object sender, UserCreateCharacterResponse response)
        {
            Debug.LogFormat("OnUserCreateCharacter:{0} [{1}]", response.Result, response.Errormsg);

            Models.User.Instance.info.Player.Characters.Clear();
            Models.User.Instance.info.Player.Characters.AddRange(response.Characters);

            if (this.OnCreateCharacter != null)
            {
                this.OnCreateCharacter(response.Result, response.Errormsg);
            }
        }

        #endregion

        #region user enter game service

        /// <summary>
        /// send  entergame request to server
        /// </summary>
        /// <param name="charID"></param>
        public void SendGameEnter(int charID)
        {
            Debug.LogFormat("UserGameEnterRequest::charID :{0}", charID);

            NetMessage message = new NetMessage();
            message.Request = new NetMessageRequest();
            message.Request.gameEnter = new UserGameEnterRequest();
            message.Request.gameEnter.characterId = charID;
        
            if (this.connected && NetClient.Instance.Connected)
            {
                this.pendingMessage = null;
                NetClient.Instance.SendMessage(message);
            }

            else
            {
                this.pendingMessage = message;
                this.ConnectToServer();
            }
        }

        /// <summary>
        /// this method is automatically called when client receive response for user enter game request
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="response"></param>
        public void OnUserGameEnter(object sender, UserGameEnterResponse response)
        {
            Debug.LogFormat("UserGameEnterResponse::result :{0} errormsg: {1}", response.Result, response.Errormsg);

            if (this.OnGameEnter != null)
            {
                this.OnGameEnter(response.Result, response.Errormsg);
            }

            if (response.Result == Result.Success && response.Character != null)
            {
                Models.User.Instance.currentCharacter = response.Character;

                ItemManager.Instance.Init(response.Character.Items);
                BagManager.Instance.Init(response.Character.Bag);
                EquipManager.Instance.Init(response.Character.Equip.Equips);
                ShopManager.Instance.Init();
                //QuestManager.Instance.Init(response.Character.Quests);
            }
        }

        #endregion

        #region character leave game service

        /// <summary>
        /// send a request to server for a specefic player to leave map 
        /// </summary>
        /// <param name="entityID"></param>
        public void SendLeaveGame(NCharacterInfo info)
        {
            MapService.Instance.PlayerLeaveMap();

            NetMessage message = new NetMessage();
            message.Request = new NetMessageRequest();
            message.Request.gameLeave = new UserGameLeaveRequest();
            message.Request.gameLeave.NChar = info;            

            if (this.connected && NetClient.Instance.Connected)
            {
                this.pendingMessage = null;
                NetClient.Instance.SendMessage(message);
            }

            else
            {
                this.pendingMessage = message;
                this.ConnectToServer();
            }
        }

        #endregion
    }
}
