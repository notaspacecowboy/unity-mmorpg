using Common.Data;
using GameServer.Core;
using SkillBridge.Message;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameServer.Managers;

namespace GameServer.Entities
{
    class CharacterBase : Entity
    {

        public int Id
        {
            get;
            set;
        }
        public NCharacterInfo Info;
        public CharacterDefine Define;

        public CharacterBase(Vector3Int pos, Vector3Int dir):base(pos,dir)
        {

        }

        public CharacterBase(CharacterType type, int tid, int level, Vector3Int pos, Vector3Int dir) :
           base(pos, dir)
        {
            this.Info = new NCharacterInfo();
            this.Info.Type = type;
            this.Info.Level = level;
            this.Info.Tid = tid;
            this.Info.Entity = this.EntityData;
            this.Define = DataManager.Instance.Characters[this.Info.Tid];
            this.Info.Name = this.Define.Name;
        }
    }
}
