using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Diagnostics;
using System.Linq;
using System.Runtime.Serialization.Configuration;
using System.Text;
using System.Threading.Tasks;
using Common;
using Common.Data;
using GameServer.Entities;
using GameServer.Services;
using Network;
using SkillBridge.Message;

namespace GameServer.Models
{
    class Map
    {
        #region MapCharacter Class

        /// <summary>
        /// this class is to store the information of characters who currently
        /// in the map
        /// </summary>
        internal class MapCharacter
        {
            /// <summary>
            /// user info of the character
            /// </summary>
            public NetConnection<NetSession> connection;

            /// <summary>
            /// character info
            /// </summary>
            public Character character;

            /// <summary>
            /// constructor function
            /// </summary>
            public MapCharacter(NetConnection<NetSession> conn, Character cha)
            {
                this.connection = conn;
                this.character = cha;
            }
        }

        #endregion

        #region Fields

        /// <summary>
        /// contains all the mapcharacters who are currently on the map
        /// </summary>
        private Dictionary<int, MapCharacter> characters = new Dictionary<int, MapCharacter>();

        /// <summary>
        /// map id, get it from its configuration info
        /// </summary>
        private int mapID
        {
            get { return this.mapDefine.ID; }
        }

        /// <summary>
        /// the configuration information of the current map
        /// </summary>
        internal MapDefine mapDefine;

        #endregion

        #region Methods

        /// <summary>
        /// constructor function
        /// </summary>
        /// <param name="defn"></param>
        internal Map(MapDefine defn)
        {
            Log.InfoFormat("Map -> Map(): Mapid: {0}, MapName: {1}", defn.ID, defn.Name);
            this.mapDefine = defn;
        }

        /// <summary>
        /// real-time update the map
        /// </summary>
        internal void Update()
        {
        }

        #endregion

        #region Methods of Character enter map

        /// <summary>
        /// it is called when a new character enter the map
        /// </summary>
        /// <param name="conn"></param>
        /// <param name="character"></param>
        internal void CharacterEnter(NetConnection<NetSession> conn, Character character)
        {
            Log.InfoFormat("Character Enter: map: {0}, charID: {1}",
                this.mapID, character.Id);

            conn.Session.Response.mapCharacterEnter = new MapCharacterEnterResponse();
            conn.Session.Response.mapCharacterEnter.mapId = this.mapID;

            foreach (var player in characters)
            {
                conn.Session.Response.mapCharacterEnter.Characters.Add(player.Value.character.Info);
                SendCharacterEnter(player.Value.connection, character.Info);
            }

            this.characters.Add(character.entityId, new MapCharacter(conn, character));

            conn.Session.Response.mapCharacterEnter.Characters.Add(character.Info);

            Log.InfoFormat("there are currently {0} characters in map",
                conn.Session.Response.mapCharacterEnter.Characters.Count);
            
            conn.SendResponse();
        }

        /// <summary>
        /// send data to an existing player on the map
        /// </summary>
        /// <param name="conn"></param>
        /// <param name="character"></param>
        private void SendCharacterEnter(NetConnection<NetSession> conn, NCharacterInfo character)
        {
            conn.Session.Response.mapCharacterEnter = new MapCharacterEnterResponse();

            conn.Session.Response.mapCharacterEnter.Characters.Add(character);
            conn.Session.Response.mapCharacterEnter.mapId = this.mapID;

            conn.SendResponse();
        }

        #endregion

        #region Methods to remove character from map

        /// <summary>
        /// when a player leaves the map, remove it from dictionary and send
        /// message to all other remainning players
        /// </summary>
        /// <param name="conn"></param>
        /// <param name="entityID"></param>
        public void RemoveCharacter(Character character)
        {
            characters.Remove(character.entityId);
            foreach (var mapCharacter in characters.Values)
            {
                SendCharacterLeave(mapCharacter.connection, character.entityId);
            }
        }

        /// <summary>
        /// send a meesage of some player leaving the game to a specefic player
        /// </summary>
        /// <param name="conn"></param>
        /// <param name="entityID"></param>
        private void SendCharacterLeave(NetConnection<NetSession> conn, int id)
        {
            conn.Session.Response.mapCharacterLeave = new MapCharacterLeaveResponse();
            conn.Session.Response.mapCharacterLeave.entityId = id;
            
            conn.SendResponse();
        }

        #endregion

        #region Methods to perform map character state synchronization

        public void UpdateMapEntity(NEntitySync entity)
        {
            foreach (var mapCharacter in characters.Values)
            {
                if (mapCharacter.character.entityId == entity.Id)
                {
                    mapCharacter.character.Position = entity.Entity.Position;
                    mapCharacter.character.Direction = entity.Entity.Direction;
                    mapCharacter.character.Speed = entity.Entity.Speed;
                }
                else
                {
                    MapService.Instance.SendEntityUpdate(mapCharacter.connection, entity);
                }
            }
        }

        #endregion
    }
}
