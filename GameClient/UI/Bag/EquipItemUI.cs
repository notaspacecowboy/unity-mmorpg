//=============================
//Author: Zack Yang 
//Created Date: 12/07/2020 19:11
//=============================
using System.Collections;
using System.Collections.Generic;
using Common.Data;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class EquipItemUI : BasePanel
{
    private Image mItemImg;

    private bool isInit = false;

    public ItemDefine define;

    public void Init(ItemDefine define)
    {
        if (isInit)
        {
            Refresh(define);
            return;
        }

        isInit = true;
        mItemImg = GetComponent<Image>();

        this.Refresh(define);

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
            EquipManager.Instance.EquipItem(define.ID, false);
        }
    }

    public void Refresh(ItemDefine define)
    {
        this.define = define;

        mItemImg.sprite = ResManager.Instance.Load<Sprite>(ResManager.ResourceType.Item, define.Icon);
    }
}
