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
    class Character : CharacterBase
    {
       
        public TCharacter Data;

        public ItemManager ItemManager;

        public Character(CharacterType type,TCharacter cha):
            base(new Core.Vector3Int(cha.MapPosX, cha.MapPosY, cha.MapPosZ),new Core.Vector3Int(100,0,0))
        {
            this.Data = cha;
            this.Id = cha.ID;

            //init character info
            this.Info = new NCharacterInfo();
            this.Info.Type = type;
            this.Info.Id = cha.ID;
            this.Info.Name = cha.Name;
            this.Info.Level = 1;//cha.Level;
            this.Info.Tid = cha.TID;
            this.Info.Class = (CharacterClass)cha.Class;
            this.Info.mapId = cha.MapID;
            this.Info.Entity = this.EntityData;
            this.Info.Gold = (int)cha.Gold;
            //Init bag
            this.Info.Bag = new NBagInfo();
            this.Info.Bag.Size = this.Data.Bag.Size;
            this.Info.Bag.Items = this.Data.Bag.Items;
            //Init equipments
            this.Info.Equip = new NEquipInfo();
            this.Info.Equip.Equips = new int[7];
            this.Info.Equip.Equips[0] = this.Data.Equip.Weapon;
            this.Info.Equip.Equips[1] = this.Data.Equip.Accessory;
            this.Info.Equip.Equips[2] = this.Data.Equip.Helmet;
            this.Info.Equip.Equips[3] = this.Data.Equip.Chest;
            this.Info.Equip.Equips[4] = this.Data.Equip.Shoulder;
            this.Info.Equip.Equips[5] = this.Data.Equip.Pants;
            this.Info.Equip.Equips[6] = this.Data.Equip.Boots;

            //get character define from json configuration file
            this.Define = DataManager.Instance.Characters[this.Info.Tid];

            //init item system
            ItemManager = new ItemManager(this);
            ItemManager.GetItemList(Info.Items);
        }

        public bool ChangeGold(int amount)
        {
            if (this.Data.Gold + amount >= 0)
            {
                this.Data.Gold += amount;
                this.Info.Gold = (int)this.Data.Gold;
                return true;
            }
                return false;
        }

    }
}
