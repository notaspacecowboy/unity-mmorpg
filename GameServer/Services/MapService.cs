using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Common;
using Common.Data;
using GameServer.Entities;
using GameServer.Managers;
using GameServer.Models;
using Network;
using SkillBridge.Message;

namespace GameServer.Services
{
    class MapService: Singleton<MapService>
    {
        #region Initialization

        public MapService()
        {
            MessageDistributer<NetConnection<NetSession>>.Instance.Subscribe<MapEntitySyncRequest>(this.OnMapEntitySync);
            MessageDistributer<NetConnection<NetSession>>.Instance.Subscribe<MapTeleportRequest>(this.OnMapTeleport);
        }

        public void Init()
        {
            Log.InfoFormat("MapService->Init()");
            MapManager.Instance.Init();
        }

        #endregion

        #region characters state synchronization service

        void OnMapEntitySync(NetConnection<NetSession> conn, MapEntitySyncRequest request)
        {
            Log.InfoFormat("MapEntitySyncRequest: EntitySpeed: {0}", request.entitySync.Entity.Speed);

            Character character = conn.Session.Character;
            MapManager.Instance[character.Info.mapId].UpdateMapEntity(request.entitySync);
        }

        public void SendEntityUpdate(NetConnection<NetSession> sender, NEntitySync entity)
        {
            sender.Session.Response.mapEntitySync = new MapEntitySyncResponse();
            sender.Session.Response.mapEntitySync.entitySyncs.Add(entity);

            sender.SendResponse();
        }

        #endregion

        #region character map teleportation service
        void OnMapTeleport(NetConnection<NetSession> conn, MapTeleportRequest request)
        {
            Log.InfoFormat("MapTeleportRequest: TeleportID: {0}", request.teleporterId);

            TeleporterDefine from = DataManager.Instance.Teleporters[request.teleporterId];
            TeleporterDefine destination = DataManager.Instance.Teleporters[from.LinkTo];
            if (from == null)
            {
                Log.ErrorFormat("TeleportID {0} cannot be found", request.teleporterId);
            }

            if (destination == null)
            {
                Log.ErrorFormat("TeleportID {0} cannot be found", from.LinkTo);
            }

            Log.InfoFormat("character {0} requests teleportation {1}", conn.Session.Character.Info.Name, from.Descript);
            
            MapManager.Instance[from.MapID].RemoveCharacter(conn.Session.Character);

            conn.Session.Character.Position = destination.Position;
            conn.Session.Character.Direction = destination.Direction;

            MapManager.Instance[destination.MapID].CharacterEnter(conn, conn.Session.Character);
        }

        #endregion
    }
}
