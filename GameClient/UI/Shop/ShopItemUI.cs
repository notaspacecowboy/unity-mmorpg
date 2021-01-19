//=============================
//Author: Zack Yang 
//Created Date: 12/01/2020 2:41
//=============================

using System.CodeDom;
using System.Collections;
using System.Collections.Generic;
using Common.Data;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityScript.Scripting.Pipeline;

public class ShopItemUI : BasePanel
{
    private Image mItemImg;
    private Image mSelectImg;
    private Text mAmountTxt;

    private EventTrigger trigger;
    private ShopPanel mPanel;
    private int mIndex;

    public ShopItemDefine Define;

    public void Init(ShopPanel panel, ShopItemDefine define, int index)
    {
        mPanel = panel;
        mIndex = index;
        Define = define;

        mItemImg = GetComponent<Image>("ItemImg");
        mSelectImg = GetComponent<Image>("SelectImg");
        mAmountTxt = GetComponent<Text>("AmountTxt");

        mSelectImg.gameObject.SetActive(false);
        mItemImg.sprite = ResManager.Instance.Load<Sprite>(ResManager.ResourceType.Item, DataManager.Instance.Items[define.ItemID].Icon);
        mAmountTxt.text = define.Count.ToString();

        trigger = mItemImg.gameObject.AddComponent<EventTrigger>();

        EventTrigger.Entry mouseClick = new EventTrigger.Entry();
        mouseClick.eventID = EventTriggerType.PointerClick;
        mouseClick.callback.AddListener(OnMouseClick);

        trigger.triggers.Add(mouseClick);
    }

    private void OnMouseClick(BaseEventData data)
    {
        mSelectImg.gameObject.SetActive(true);
        mPanel.Select(mIndex);
    }

    public void UnSelect()
    {
        mSelectImg.gameObject.SetActive(false);
    }
}
