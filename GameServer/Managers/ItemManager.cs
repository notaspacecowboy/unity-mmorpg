using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Data;
using GameServer.Entities;
using GameServer.Models;
using GameServer.Services;
using Network;
using SkillBridge.Message;

namespace GameServer.Managers
{
    class ItemManager
    {
        /// <summary>
        /// each itemManager is responsible for items of a specefic character
        /// </summary>
        private Character owner;

        /// <summary>
        /// dictionary of all items of character mOwner
        /// </summary>
        private Dictionary<int, Item> items = new Dictionary<int, Item>();

        public ItemManager(Character character)
        {
            owner = character;
            Init();
        }
        public void Init()
        {
            foreach (var characterItem in owner.Data.Items)
            {
                items.Add(characterItem.ItemID, new Item(characterItem));
            }
        }


        public bool ConsumeItem(int id, int amount = 1)
        {
            Item item = null;

            if (items.TryGetValue(id, out item))
            {
                if (item.Count < amount)
                {
                    return false;
                }
                else
                {
                    item.Consume(amount);

                    //save changes to database
                    DBService.Instance.Entities.SaveChanges();
                    return true;
                }
            }

            return false;
        }

        public bool ContainsItem(int id)
        {
            if (items.ContainsKey(id))
                return true;
            else
            {
                return false;
            }
        }

        public void AddItem(int id, int amount = 1)
        {
            Item item = null;
            if (items.TryGetValue(id, out item))
            {
                item.Add(amount);
            }
            //if item is not stored in the dictionary, create a new record
            else
            {
                TCharacterItem tItem = new TCharacterItem()
                {
                    ItemID = id,
                    Count = amount,
                    Owner = owner.Data
                };

                owner.Data.Items.Add(tItem);
                items.Add(id, new Item(tItem));
            }

            //save changes to database
            DBService.Instance.Entities.SaveChanges();
        }

        public Item GetItem(int itemID)
        {
            Item item = null;
            if (items.TryGetValue(itemID, out item))
                return item;

            //if not existed
            return null;
        }


        public bool EquipItem(NetConnection<NetSession> sender, int itemID, int slot)
        {
            if (items.ContainsKey(itemID) && items[itemID].Count > 0)
            {
                ItemDefine itemDefine = DataManager.Instance.Items[itemID];
                if (itemDefine == null || itemDefine.Type != E_ItemType.EQUIP)
                    return false;

                EquipDefine equipDefine = DataManager.Instance.Equips[itemID];
                if (equipDefine == null || (int)equipDefine.Slot != slot)
                {
                    return false;
                }

                #region change equipment

                switch (slot)
                {
                    case (int)EquipSlot.Weapon:
                        if (this.owner.Data.Equip.Weapon != 0)
                        {
                            this.AddItem(this.owner.Data.Equip.Weapon);
                        }
                        this.owner.Data.Equip.Weapon = itemID;
                        this.owner.Info.Equip.Equips[slot] = itemID;
                        this.ConsumeItem(itemID);
                        break;

                    case (int)EquipSlot.Accessory:
                        if (this.owner.Data.Equip.Accessory != 0)
                        {
                            this.AddItem(this.owner.Data.Equip.Accessory);
                        }
                        this.owner.Data.Equip.Accessory = itemID;
                        this.owner.Info.Equip.Equips[slot] = itemID;
                        this.ConsumeItem(itemID);
                        break;

                    case (int)EquipSlot.Helmet:
                        if (this.owner.Data.Equip.Helmet != 0)
                        {
                            this.AddItem(this.owner.Data.Equip.Helmet);
                        }
                        this.owner.Data.Equip.Helmet = itemID;
                        this.owner.Info.Equip.Equips[slot] = itemID;
                        this.ConsumeItem(itemID);
                        break;

                    case (int)EquipSlot.Chest:
                        if (this.owner.Data.Equip.Chest != 0)
                        {
                            this.AddItem(this.owner.Data.Equip.Chest);
                        }
                        this.owner.Data.Equip.Chest = itemID;
                        this.owner.Info.Equip.Equips[slot] = itemID;
                        this.ConsumeItem(itemID);
                        break;

                    case (int)EquipSlot.Shoulder:
                        if (this.owner.Data.Equip.Shoulder != 0)
                        {
                            this.AddItem(this.owner.Data.Equip.Shoulder);
                        }
                        this.owner.Data.Equip.Shoulder = itemID;
                        this.owner.Info.Equip.Equips[slot] = itemID;
                        this.ConsumeItem(itemID);
                        break;

                    case (int)EquipSlot.Pants:
                        if (this.owner.Data.Equip.Pants != 0)
                        {
                            this.AddItem(this.owner.Data.Equip.Pants);
                        }
                        this.owner.Data.Equip.Pants = itemID;
                        this.owner.Info.Equip.Equips[slot] = itemID;
                        this.ConsumeItem(itemID);
                        break;

                    case (int)EquipSlot.Boots:
                        if (this.owner.Data.Equip.Boots != 0)
                        {
                            this.AddItem(this.owner.Data.Equip.Boots);
                        }
                        this.owner.Data.Equip.Boots = itemID;
                        this.owner.Info.Equip.Equips[slot] = itemID;
                        this.ConsumeItem(itemID);
                        break;
                }

                #endregion

                return true;
            }

            return false;
        }


        public bool UnEquipItem(NetConnection<NetSession> sender, int itemID, int slot)
        {
            #region unequip equipment

            switch (slot)
            {
                case (int)EquipSlot.Weapon:
                    if (this.owner.Data.Equip.Weapon == itemID)
                    {
                        this.AddItem(this.owner.Data.Equip.Weapon);
                        this.owner.Data.Equip.Weapon = 0;
                        this.owner.Info.Equip.Equips[slot] = 0;
                        break;
                    }

                    return false;

                case (int)EquipSlot.Accessory:
                    if (this.owner.Data.Equip.Accessory == itemID)
                    {
                        this.AddItem(this.owner.Data.Equip.Accessory);
                        this.owner.Data.Equip.Accessory = 0;
                        this.owner.Info.Equip.Equips[slot] = 0;
                        break;
                    }

                    return false;

                case (int)EquipSlot.Helmet:
                    if (this.owner.Data.Equip.Helmet == itemID)
                    {
                        this.AddItem(this.owner.Data.Equip.Helmet);
                        this.owner.Data.Equip.Helmet = 0;
                        this.owner.Info.Equip.Equips[slot] = 0;
                        break;
                    }

                    return false;

                case (int)EquipSlot.Chest:
                    if (this.owner.Data.Equip.Chest == itemID)
                    {
                        this.AddItem(this.owner.Data.Equip.Chest);
                        this.owner.Data.Equip.Chest = 0;
                        this.owner.Info.Equip.Equips[slot] = 0;
                        break;
                    }

                    return false;

                case (int)EquipSlot.Shoulder:
                    if (this.owner.Data.Equip.Shoulder == itemID)
                    {
                        this.AddItem(this.owner.Data.Equip.Shoulder);
                        this.owner.Data.Equip.Shoulder = 0;
                        this.owner.Info.Equip.Equips[slot] = 0;
                        break;
                    }

                    return false;

                case (int)EquipSlot.Pants:
                    if (this.owner.Data.Equip.Pants == itemID)
                    {
                        this.AddItem(this.owner.Data.Equip.Pants);
                        this.owner.Data.Equip.Pants = 0;
                        this.owner.Info.Equip.Equips[slot] = 0;
                        break;
                    }

                    return false;

                case (int)EquipSlot.Boots:
                    if (this.owner.Data.Equip.Boots == itemID)
                    {
                        this.AddItem(this.owner.Data.Equip.Boots);
                        this.owner.Data.Equip.Boots = 0;
                        this.owner.Info.Equip.Equips[slot] = 0;
                        break;
                    }

                    return false;
            }

            #endregion

            return true;
        }

        public void GetItemList(List<NItemInfo> itemInfos)
        {
            itemInfos.Clear();

            foreach (var item in items.Values)
            {
                itemInfos.Add(new NItemInfo() {Id = item.ID, Count = item.Count});
            }
        }

    }
}
