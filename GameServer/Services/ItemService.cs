using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization.Configuration;
using System.Text;
using System.Threading.Tasks;
using Common;
using GameServer.Managers;
using Network;
using SkillBridge.Message;

namespace GameServer.Services
{
    class ItemService: Singleton<ItemService>
    {
        public ItemService()
        {
            MessageDistributer<NetConnection<NetSession>>.Instance.Subscribe<ItemPurchaseRequest>(this.OnItemPurchase);
            MessageDistributer<NetConnection<NetSession>>.Instance.Subscribe<ItemEquipRequest>(this.OnItemEquip);
        }

        public void Init()
        {
        }


        public void OnItemPurchase(NetConnection<NetSession> sender, ItemPurchaseRequest request)
        {
            sender.Session.Response.itemPurchase = new ItemPurchaseResponse();
            if (ShopManager.Instance.BuyItem(sender, request))
            {
                sender.Session.Response.itemPurchase.Result = Result.Success;
            }
            else
            {
                sender.Session.Response.itemPurchase.Result = Result.Failed;
                sender.Session.Response.itemPurchase.Errormsg = "You do not have enough gold!";
            }

            sender.SendResponse();
        }

        public void OnItemEquip(NetConnection<NetSession> sender, ItemEquipRequest request)
        {
            Log.InfoFormat("ItemService->OnItemEquip: ToEquip: {0}", request.isEqup);

            sender.Session.Response.itemEquip = new ItemEquipResponse();
            //if character request to equip a item
            if (request.isEqup == true)
            {
                if (sender.Session.Character.ItemManager.EquipItem(sender, request.itemID, request.Slot))
                {
                    sender.Session.Response.itemEquip.Result = Result.Success;
                }
                else
                {
                    sender.Session.Response.itemEquip.Result = Result.Failed;
                    sender.Session.Response.itemEquip.Errormsg = "Item Invalid";
                }
            }
            //if character request to unequip a item
            else
            {
                if (sender.Session.Character.ItemManager.UnEquipItem(sender, request.itemID, request.Slot))
                {
                    sender.Session.Response.itemEquip.Result = Result.Success;
                }
                else
                {
                    sender.Session.Response.itemEquip.Result = Result.Failed;
                    sender.Session.Response.itemEquip.Errormsg = "Item Invalid";
                }
            }

            sender.SendResponse();
        }
    }
}
