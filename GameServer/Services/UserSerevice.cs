using System;
using System.Linq;
using System.ServiceModel.Configuration;
using Common;
using Common.Data;
using GameServer.Entities;
using GameServer.Managers;
using GameServer.Models;
using Network;
using SkillBridge.Message;

namespace GameServer.Services
{
    class UserService : Singleton<UserService>
    {

        public UserService()
        {
            MessageDistributer<NetConnection<NetSession>>.Instance.Subscribe<UserLoginRequest>(this.OnLogin);
            MessageDistributer<NetConnection<NetSession>>.Instance.Subscribe<UserRegisterRequest>(this.OnRegister);
            MessageDistributer<NetConnection<NetSession>>.Instance.Subscribe<UserCreateCharacterRequest>(this.OnCreateCharacter);
            MessageDistributer<NetConnection<NetSession>>.Instance.Subscribe<UserGameEnterRequest>(this.OnGameEnter);
            MessageDistributer<NetConnection<NetSession>>.Instance.Subscribe<UserGameLeaveRequest>(this.OnGameLeave);
        }

        public void Init()
        {
        }


        #region login service

        void OnLogin(NetConnection<NetSession> sender,UserLoginRequest request)
        {
            Log.InfoFormat("UserLoginRequest: User:{0}  Pass:{1}", request.User, request.Passward);

            sender.Session.Response.userLogin = new UserLoginResponse();


            TUser user = DBService.Instance.Entities.Users.Where(u => u.Username == request.User).FirstOrDefault();
            if(user==null)
            {
                sender.Session.Response.userLogin.Result = Result.Failed;
                sender.Session.Response.userLogin.Errormsg = "User does not exit";
            }
            else if(user.Password != request.Passward)
            {
                sender.Session.Response.userLogin.Result = Result.Failed;
                sender.Session.Response.userLogin.Errormsg = "Password does not match. Please re-enter.";
            }
            else
            {
                sender.Session.User = user;

                sender.Session.Response.userLogin.Result = Result.Success;
                sender.Session.Response.userLogin.Errormsg = "None";
                sender.Session.Response.userLogin.Userinfo = new NUserInfo();
                sender.Session.Response.userLogin.Userinfo.Id = (int)user.ID;
                sender.Session.Response.userLogin.Userinfo.Player = new NPlayerInfo();
                sender.Session.Response.userLogin.Userinfo.Player.Id = user.Player.ID;
                foreach(var c in user.Player.Characters)
                {
                    NCharacterInfo info = new NCharacterInfo();
                    info.Id = c.ID;
                    info.Name = c.Name;
                    info.Class = (CharacterClass)c.Class;
                    info.Type = CharacterType.Player;
                    sender.Session.Response.userLogin.Userinfo.Player.Characters.Add(info);
                }

            }
            
            sender.SendResponse();
        }

        #endregion

        #region register service

        void OnRegister(NetConnection<NetSession> sender, UserRegisterRequest request)
        {
            Log.InfoFormat("UserRegisterRequest: username: {0}  password: {1}", request.User, request.Passward );

            sender.Session.Response.userRegister = new UserRegisterResponse();

            TUser user = DBService.Instance.Entities.Users.Where((u) => u.Username == request.User).FirstOrDefault();

            //if we find user with the same username in database, registeration failed
            if (user != null)
            {
                sender.Session.Response.userRegister.Result = Result.Failed;
                sender.Session.Response.userRegister.Errormsg = "user already exist";
            }

            else
            {
                TPlayer player = DBService.Instance.Entities.Players.Add(new TPlayer());
                DBService.Instance.Entities.Users.Add(new TUser()
                {
                    Password = request.Passward,
                    Player = player,
                    Username = request.User
                });

                DBService.Instance.Entities.SaveChanges();
                sender.Session.Response.userRegister.Result = Result.Success;
                sender.Session.Response.userRegister.Errormsg = "None";
            }

            sender.SendResponse();
        }

        #endregion

        #region character creation service

        void OnCreateCharacter(NetConnection<NetSession> sender, UserCreateCharacterRequest request)
        {
            Log.InfoFormat("UserCreateCharacterRequest: Name: {0}, Class: {1}", request.Name, request.Class);

            sender.Session.Response.createChar = new UserCreateCharacterResponse();

            TCharacter character = DBService.Instance.Entities.Characters.Where((c) => c.Name == request.Name).FirstOrDefault();

            if (character != null)
            {
                sender.Session.Response.createChar.Result = Result.Failed;
                sender.Session.Response.createChar.Errormsg = "Character name already existed";
            }

            else
            {
                sender.Session.Response.createChar.Result = Result.Success;

                character = new TCharacter()
                {
                    Name = request.Name,
                    Class = (int) request.Class,
                    TID = (int) request.Class,
                    MapID = 1,          //进入游戏世界时,出现在哪张地图
                    MapPosX = 5000,     
                    MapPosY = 4000,
                    MapPosZ = 820,
                    Gold = 500,
                    Bag = new TCharacterBag(),
                    Equip = new TCharacterEquipment()
                };

                //init items
                character.Items.Add(new TCharacterItem()
                {
                    Count =  20,
                    ItemID =  1,
                    Owner = character
                });
                character.Items.Add(new TCharacterItem()
                {
                    Count = 20,
                    ItemID = 2,
                    Owner = character
                });

                //Init character bag to be empty
                TCharacterBag bag = character.Bag;
                bag.Size = 30;
                bag.Items = new byte[0];
                bag.Owner = character;

                //Init character equipments to be empty
                TCharacterEquipment equipment = character.Equip;
                equipment.Owner = character;
                equipment.Weapon = 0;
                equipment.Accessory = 0;
                equipment.Boots = 0;
                equipment.Helmet = 0;
                equipment.Chest = 0;
                equipment.Pants = 0;
                equipment.Shoulder = 0;

                sender.Session.User.Player.Characters.Add(character);   //session即内存中存储的用户信息

                DBService.Instance.Entities.TCharacterBags.Add(bag);
                DBService.Instance.Entities.Characters.Add(character);
                DBService.Instance.Entities.SaveChanges();

                foreach (var c in sender.Session.User.Player.Characters)
                {
                    NCharacterInfo info = new NCharacterInfo();
                    info.Id = c.ID;
                    info.Name = c.Name;
                    info.Class = (CharacterClass)c.Class;
                    Log.InfoFormat("{0}", info.Class.ToString());
                    info.Type = CharacterType.Player;

                    sender.Session.Response.createChar.Characters.Add(info);
                }
                sender.Session.Response.createChar.Errormsg = "None";
            }

            sender.SendResponse();
        }

        #endregion

        #region game enter service

        void OnGameEnter(NetConnection<NetSession> sender, UserGameEnterRequest request)
        {
            Log.InfoFormat("UserGameEnterRequest: id: {0}", request.characterId);

            TCharacter tCharacter = DBService.Instance.Entities.Characters.Where((c) => c.ID == request.characterId).FirstOrDefault();
            Character character = CharacterManager.Instance.AddCharacter(tCharacter);

            character.ItemManager.GetItemList(character.Info.Items);
                
            sender.Session.Response.gameEnter = new UserGameEnterResponse();
            sender.Session.Response.gameEnter.Result = Result.Success;
            sender.Session.Response.gameEnter.Errormsg = "None";
            sender.Session.Response.gameEnter.Character = character.Info;

            sender.SendResponse();
            sender.Session.Character = character;

            MapManager.Instance[character.Data.MapID].CharacterEnter(sender, character);
        }

        #endregion

        #region game leave service
        void OnGameLeave(NetConnection<NetSession> conn, UserGameLeaveRequest request)
        {
            //get the leaving character 
            Character character = conn.Session.Character;

            CharacterLeaveGame(character);
        }


        public void CharacterLeaveGame(Character character)
        {
            //save character info to database
            character.Data.MapID = character.Info.mapId;
            character.Data.MapPosX = character.Position.x;
            character.Data.MapPosY = character.Position.y;
            character.Data.MapPosZ = character.Position.z;
            DBService.Instance.Entities.SaveChanges();

            //remove character from characterManager and entityManager
            CharacterManager.Instance.RemoveCharacter(character.entityId);

            //remove character from map
            MapManager.Instance[character.Data.MapID].RemoveCharacter(character);

        }
        #endregion
    }
}
