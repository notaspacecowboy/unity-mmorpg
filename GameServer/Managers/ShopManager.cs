using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;
using Common.Data;
using GameServer.Entities;
using Network;
using SkillBridge.Message;

namespace GameServer.Managers
{
    class ShopManager: Singleton<ShopManager>
    {
        public bool BuyItem(NetConnection<NetSession> sender, ItemPurchaseRequest request)
        {
            if (VerifyTransaction(sender, request))
            {
                ShopItemDefine define = DataManager.Instance.ShopItems[request.shopID][request.shopItemID];

                sender.Session.Character.ItemManager.AddItem(define.ItemID, request.Amount);

                //additem notify
                NStatus status = new NStatus();
                status.Type = StatusType.Item;
                status.Action = StatusAction.Add;
                status.Id = define.ItemID;
                status.Value = request.Amount;
                Console.WriteLine("purchase one unit of " + define.ItemID);
                if(sender.Session.Response.Notifies == null)
                    sender.Session.Response.Notifies = new StatusNotify();
                sender.Session.Response.Notifies.Status.Add(status);

                //deduct money notify
                status = new NStatus();
                status.Type = StatusType.Money;
                status.Action = StatusAction.Remove;
                status.Id = -1;
                status.Value = define.Price;
                sender.Session.Response.Notifies.Status.Add(status);

                return true;
            }

            return false;
        }

        private bool VerifyTransaction(NetConnection<NetSession> sender, ItemPurchaseRequest request)
        {
            if (!DataManager.Instance.ShopItems.ContainsKey(request.shopID))
            {
                return false;
            }

            if (!DataManager.Instance.ShopItems[request.shopID].ContainsKey(request.shopItemID))
            {
                return false;
            }

            ShopItemDefine define = DataManager.Instance.ShopItems[request.shopID][request.shopItemID];
            if (define == null)
            {
                return false;
            }

            if (!sender.Session.Character.ChangeGold(-define.Price))
            {
                return false;
            }

            return true;
        }
    }
}

