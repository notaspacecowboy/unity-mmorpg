//=============================
//Author: Zack Yang 
//Created Date: 11/26/2020 19:46
//=============================
using System.Collections;
using System.Collections.Generic;
using Common.Data;
using UnityEngine;
using UnityEngine.UI;

public class BagPanel : BasePanel
{
    #region Bag variables

    public PageTgl[] pageTgles;
    public PageView[] pageViews;

    private Button quitBtn;

    private EasyTween anim;

    #endregion

    #region equip variables

    public EquipItemUI[] equips; 

    #endregion

    public override void ShowMe()
    {
        base.ShowMe();

        anim = GetComponent<EasyTween>();
        anim.OpenCloseObjectAnimation();

        for (int i = 0; i < pageViews.Length; i++)
        {
            pageTgles[i].Init(pageViews[i]);
        }

        pageTgles[0].TurnOn();

        quitBtn = GetComponent<Button>("QuitBtn");
        quitBtn.onClick.AddListener(() =>
        {
            UIManager.Instance.HidePanel(typeof(BagPanel));
        });

        for (int i = 0; i < equips.Length; i++)
        {
            int itemID = EquipManager.Instance.GetEquipments()[i];

            if (itemID != 0)
            {
                ItemDefine define = DataManager.Instance.Items[itemID];

                equips[i].gameObject.SetActive(true);
                equips[i].Init(define);
            }
        }
    }

    public void Refresh()
    {
        for (int i = 0; i < pageTgles.Length; i++)
        {
            if (pageTgles[i].Tgl.isOn)
            {
                pageViews[i].Refresh();
            }
        }

        for (int i = 0; i < equips.Length; i++)
        {
            if(equips[i] == null)
                continue;
            
            int itemID = EquipManager.Instance.GetEquipments()[i];

            if (itemID != 0)
            {
                ItemDefine define = DataManager.Instance.Items[itemID];

                equips[i].gameObject.SetActive(true);
                equips[i].Init(define);
            }
            else
            {
                equips[i].gameObject.SetActive(false);
            }
        }
    }
}
