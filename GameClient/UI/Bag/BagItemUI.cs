//=============================
//Author: Zack Yang 
//Created Date: 11/28/2020 21:37
//=============================
using System.Collections;
using System.Collections.Generic;
using Common.Data;
using SkillBridge.Message;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BagItemUI : BasePanel
{
    private Image itemImg;
    private Text amountTxt;

    public ItemDefine define;

    public void Init(ItemDefine define, int amount)
    {
        itemImg = GetComponent<Image>("ItemImg");
        amountTxt = GetComponent<Text>("AmountTxt");
        this.define = define;

        itemImg.sprite = ResManager.Instance.Load<Sprite>(ResManager.ResourceType.Item, define.Icon);
        amountTxt.text = amount.ToString();

        EventTrigger trigger = GetComponent<EventTrigger>();

        EventTrigger.Entry mouseDoubleClick = new EventTrigger.Entry();
        mouseDoubleClick.eventID = EventTriggerType.PointerClick;
        mouseDoubleClick.callback.AddListener(OnmouseDoubleClick);

        trigger.triggers.Add(mouseDoubleClick);
    }

    private void OnmouseDoubleClick(BaseEventData data)
    {
        //if player double click the itemUI
        if ((data as PointerEventData).clickCount == 2)
        {
            EquipManager.Instance.EquipItem(define.ID, true);
        }
    }


}
