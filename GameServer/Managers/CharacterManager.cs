using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;
using GameServer.Entities;
using Microsoft.Win32.SafeHandles;
using SkillBridge.Message;

namespace GameServer.Managers
{
    class CharacterManager: Singleton<CharacterManager>
    {
        private Dictionary<int, Character> characters = new Dictionary<int, Character>();

        public void Init()
        {
        }

        public void Dispose()
        {
        }

        public void Clear()
        {
            characters.Clear();
        }

        /// <summary>
        /// Create a character and adds it to the character manager 
        /// </summary>
        /// <param name="tChar"></param>
        /// <returns></returns>
        public Character AddCharacter(TCharacter tChar)
        {
            Character character = new Character(CharacterType.Player, tChar);

            //set entity id, add player to entitymanager
            EntityManager.Instance.AddEntity(tChar.MapID, character);

            characters.Add(character.entityId, character);

            return character;
        }

        public void RemoveCharacter(int id)
        {
            EntityManager.Instance.RemoveEntity(characters[id].Info.mapId, characters[id]);
            this.characters.Remove(id);
        }
    }
}
