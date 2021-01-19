//=============================
//Author: Zack Yang 
//Created Date: 12/02/2020 21:44
//=============================
using System.Collections;
using System.Collections.Generic;
using Common.Data;
using UnityEngine;

public class ShopManager : Singleton<ShopManager>
{
    private NpcDefine onInteractive = null;
    public void Init()
    {
        NpcManager.Instance.AddEventListener(NpcFunction.InvokeShop, OnOpenShop);
    }

    private bool OnOpenShop(NpcDefine define)
    {
        onInteractive = define;

        UIManager.Instance.ShowPanel<ShopPanel>(typeof(ShopPanel), (panel) =>
        {
            panel.Init(define.Param);
        });

        return true;
    }

    public void StopInteractive()
    {
        Debug.Log("ShopManager() => StopInteractive()");
        NpcManager.Instance.StopInteractive(onInteractive);
        onInteractive = null;
    }


    public void BuyItem(int shopItemID, int amount)
    {
        ItemService.Instance.SendBuyItem(onInteractive.Param, shopItemID, amount);
    }
}
