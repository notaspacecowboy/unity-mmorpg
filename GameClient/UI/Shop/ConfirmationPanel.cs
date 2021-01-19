//=============================
//Author: Zack Yang 
//Created Date: 12/01/2020 2:17
//=============================
using System.Collections;
using System.Collections.Generic;
using Common.Data;
using SkillBridge.Message;
using UnityEngine;
using UnityEngine.UI;

public class ConfirmationPanel : BasePanel
{
    private Button yesBtn;
    private Button noBtn;
    private Text confirmTxt;

    private ShopPanel panel;
    public void Init(ShopItemDefine define, ShopPanel panel)
    {
        yesBtn = GetComponent<Button>("YesBtn");
        noBtn = GetComponent<Button>("NoBtn");
        confirmTxt = GetComponent<Text>("ConfirmTxt");

        confirmTxt.text = confirmTxt.text + DataManager.Instance.Items[define.ItemID].Name + " ?";

        yesBtn.onClick.AddListener(() =>
        {
            ShopManager.Instance.BuyItem(define.ID, 1);
            ItemService.Instance.OnItemPurchase += this.OnItemPurchaseResponse;
        });

        noBtn.onClick.AddListener(() =>
        {
            UIManager.Instance.HidePanel(typeof(ConfirmationPanel));
        });
    }

    private void OnItemPurchaseResponse(Result result, string errorMsg)
    {
        ItemService.Instance.OnItemPurchase -= this.OnItemPurchaseResponse;
        
        if (result == Result.Success)
        {
            UIManager.Instance.HidePanel(typeof(ConfirmationPanel));
        }
        else
        {
            confirmTxt.text = errorMsg;
            confirmTxt.color = Color.red;
        }
    }
}
