//=============================
//Author: Zack Yang 
//Created Date: 12/02/2020 23:57
//=============================

using System;
using System.Collections;
using System.Collections.Generic;
using Models;
using Network;
using Services;
using SkillBridge.Message;
using UnityEngine;
using UnityEngine.Events;

public class ItemService : Singleton<ItemService>
{
    public UnityAction<NStatus> OnItemNotify = null;
    public UnityAction<Result, string> OnItemPurchase = null;
    public UnityAction<Result, string> OnItemEquip = null;
    public ItemService()
    {
        MessageDistributer.Instance.Subscribe<ItemPurchaseResponse>(OnItemPurchaseResponse);
        MessageDistributer.Instance.Subscribe<StatusNotify>(OnNotify);
        MessageDistributer.Instance.Subscribe<ItemEquipResponse>(OnItemEquipResponse);
    }
    public void Init()
    {
    }


    #region character purchase item service

    public void SendBuyItem(int shopID, int itemID, int amount)
    {
        NetMessage message = new NetMessage();
        message.Request = new NetMessageRequest();
        message.Request.itemPurchase = new ItemPurchaseRequest();
        message.Request.itemPurchase.shopID = shopID;
        message.Request.itemPurchase.shopItemID = itemID;
        message.Request.itemPurchase.Amount = amount;

        NetClient.Instance.SendMessage(message);
    }

    private void OnItemPurchaseResponse(object sender, ItemPurchaseResponse response)
    {
        Debug.LogFormat("ItemPurchaseResponse::Result :{0}", response.Result);

        if (OnItemPurchase != null)
        {
            OnItemPurchase.Invoke(response.Result, response.Errormsg);
        }
    }

    #endregion

    #region character item equipment service

    public void SendEquipItem(int itemID, int slot, bool isEquip)
    {
        Debug.Log("send equipment");
        NetMessage message = new NetMessage();
        message.Request = new NetMessageRequest();
        message.Request.itemEquip = new ItemEquipRequest();
        message.Request.itemEquip.itemID = itemID;
        message.Request.itemEquip.Slot = slot;
        message.Request.itemEquip.isEqup = isEquip;

        NetClient.Instance.SendMessage(message);
    }

    private void OnItemEquipResponse(object sender, ItemEquipResponse response)
    {
        Debug.LogFormat("IteEquipResponse::Result :{0}", response.Result);

        if (OnItemEquip != null)
        {
            OnItemEquip.Invoke(response.Result, response.Errormsg);
        }
    }

    #endregion

    #region manage notifies

    private void OnNotify(object sender, StatusNotify notifies)
    {
        Debug.LogFormat("status change notify received");
        foreach (var statusNotify in notifies.Status)
        {
            if (statusNotify.Type == StatusType.Item)
            {
                if (OnItemNotify != null)
                {
                    OnItemNotify.Invoke(statusNotify);
                }
            }
        }
    }

    #endregion
}
