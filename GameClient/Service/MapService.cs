//=============================
//Author: Zack Yang 
//Created Date: 10/30/2020 18:29
//=============================

using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Managers;
using Common.Data;
using Models;
using Network;
using SkillBridge.Message;
using UnityEngine;
using UnityEngine.Events;

public class MapService : Singleton<MapService>, IDisposable
{
    //Fields
    #region Fields of map info

    public int CurrentMapID
    {
        get;
        private set;
    }

    #endregion

    //Methods
    #region Initializer and destructor

    /// <summary>
    /// constructor of map service
    /// </summary>
    public MapService()
    {
        MessageDistributer.Instance.Subscribe<MapCharacterEnterResponse>(OnMapCharacterEnter);
        MessageDistributer.Instance.Subscribe<MapCharacterLeaveResponse>(OnMapCharacterLeave);
        MessageDistributer.Instance.Subscribe<MapEntitySyncResponse>(OnMapEntitySync);
    }

    public void Init()
    {
        CurrentMapID = 0;
    }

    /// <summary>
    /// destructor of map service
    /// </summary>
    public void Dispose()
    {
        Debug.LogFormat("mapservice dispose function is called");
        MessageDistributer.Instance.Unsubscribe<MapCharacterEnterResponse>(OnMapCharacterEnter);
        MessageDistributer.Instance.Unsubscribe<MapCharacterLeaveResponse>(OnMapCharacterLeave);
    }

    #endregion

    #region character enter map Service

    /// <summary>
    /// this method is automatically called when map service receive a response of
    /// a character enter map
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="response"></param>
    private void OnMapCharacterEnter(object sender, MapCharacterEnterResponse response)
    {
        Debug.LogFormat("OnMapCharacterEnter:Map:{0} Count:{1}", response.mapId, response.Characters.Count);

        //update current map
        if (CurrentMapID != response.mapId)
        {
            SwitchMap(response.mapId);
            CurrentMapID = response.mapId;
        }

        foreach (var cha in response.Characters)
        {
            //if find user controlled character, update User model
            if (Models.User.Instance.currentCharacter.Id == cha.Id)
            {
                Models.User.Instance.currentCharacter = cha;
                Debug.LogFormat("the current player is {0}", cha.Name);
            }

            //add all the characters in the current map to charactermanager
            CharacterManager.Instance.AddCharacter(cha);
        }

    }

    #endregion

    #region Character leave map service

    public void PlayerLeaveMap()
    {
        CurrentMapID = 0;
        CharacterManager.Instance.Clear();
        User.Instance.currentCharacter = null;
    }


    /// <summary>
    /// switch game scene to scene no.mapID
    /// </summary>
    /// <param name="mapID"></param>
    private void SwitchMap(int mapID)
    {
        MapDefine define = DataManager.Instance.Maps[mapID];

        if (define != null)
        {
            ScenesManager.Instance.LoadSceneAsync(define.Resource);
            Models.User.Instance.currentMapInfo = define;
        }
        else
        {
            Debug.LogErrorFormat("map {0} cannot be found", mapID);
        }
    }


    private void OnMapCharacterLeave(object sender, MapCharacterLeaveResponse response)
    {
        Debug.LogFormat("MapService->OnMapCharacterLeave(): character: {0}", response.entityId);

        CharacterManager.Instance.RemoveCharacter(response.entityId);
    }

    #endregion

    #region map characters synchronization service

    public void SendEntitySync(NEntity entity, EntityEvent ev)
    {
        NetMessage message = new NetMessage();
        message.Request = new NetMessageRequest();
        message.Request.mapEntitySync = new MapEntitySyncRequest();
        message.Request.mapEntitySync.entitySync = new NEntitySync()
        {
            Id = entity.Id,
            Entity = entity,
            Event = ev
        };

        NetClient.Instance.SendMessage(message);
    }

    public void OnMapEntitySync(object conn, MapEntitySyncResponse response)
    {
        Debug.LogFormat("MapService->OnMapEntitySync()");
        EntityManager.Instance.UpdateEntity(response.entitySyncs);
    }

    #endregion

    public void SendPlayerTeleport(int teleportId)
    {
        TeleporterDefine define = DataManager.Instance.Teleporters[teleportId];
        if (define.LinkTo == 0)
        {
            return;
        }
            
        CharacterManager.Instance.Clear();

        NetMessage message = new NetMessage();
        message.Request = new NetMessageRequest();
        message.Request.mapTeleport = new MapTeleportRequest();
        message.Request.mapTeleport.teleporterId = teleportId;

        NetClient.Instance.SendMessage(message);
    }
}
