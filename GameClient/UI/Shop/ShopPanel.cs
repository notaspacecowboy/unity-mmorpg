//=============================
//Author: Zack Yang 
//Created Date: 12/01/2020 1:24
//=============================
using System.Collections;
using System.Collections.Generic;
using Common.Data;
using UnityEngine;
using UnityEngine.UI;

public class ShopPanel : BasePanel
{
    private int shopID;

    private Button quitBtn;

    private List<ShopItemUI> items = new List<ShopItemUI>();
    private int selectedIndex;
    private EasyTween anim;


    public Transform Content;

    #region Methods for Initialization
    public void Init(int id)
    {
        shopID = id;
        selectedIndex = -1;

        anim = GetComponent<EasyTween>();

        anim.OpenCloseObjectAnimation();

        InitShopItems();

        quitBtn = GetComponent<Button>("QuitBtn");
        quitBtn.onClick.AddListener(() =>
        {
            UIManager.Instance.HidePanel(typeof(ShopPanel));
        });
    }

    void InitShopItems()
    {
        int i = 0;
        foreach (var itemDefine in DataManager.Instance.ShopItems[shopID].Values)
        {
            GameObject obj = ResManager.Instance.Load<GameObject>(ResManager.ResourceType.Panel, "ShopItemUI");
            obj.transform.parent = Content;
            obj.transform.localScale = Vector3.one;
            obj.transform.localPosition = Vector3.zero;

            ShopItemUI itemUI = obj.GetComponent<ShopItemUI>();
            itemUI.Init(this, itemDefine, i++);
            items.Add(itemUI);
        }
    }


    public override void HideMe()
    {
        base.HideMe();
        Debug.Log("shop panel is closed");
        ShopManager.Instance.StopInteractive();
    }

    #endregion

    #region methods for purchase logics

    public void Select(int index)
    {
        if (selectedIndex != -1)
        {
            items[selectedIndex].UnSelect();
        }
        selectedIndex = index;
        ShowConfirmation();
    }
    public void ShowConfirmation()
    {
        if (selectedIndex != -1)
        {
            UIManager.Instance.ShowPanel<ConfirmationPanel>(typeof(ConfirmationPanel), (panel) =>
            {
                panel.Init(items[selectedIndex].Define, this);
            });
        }
    }

    public void ConfirmPurchase()
    {
        //ShopManager.Instance.BuyItem(items[SelectedIndex].define.ItemID, 1);
    }

    public void OnBuyItem()
    {
    }

    #endregion
}
