//=============================
//Author: Zack Yang 
//Created Date: 12/07/2020 6:32
//=============================
using System.Collections;
using System.Collections.Generic;
using Common.Data;
using Models;
using SkillBridge.Message;
using UnityEngine;
using UnityEngine.Events;

public class EquipManager : Singleton<EquipManager>
{ 
    /// <summary>
    /// the itemID of the equipment that is waiting to be equipped
    /// </summary>
    private int pendingEquip = 0;

    /// <summary>
    /// true if the pending equipment is waiting to be equipped, false if it is waiting to be unequipped
    /// </summary>
    private bool isEquip = false;
    /// <summary>
    /// current equipment
    /// </summary>
    private int[] equipments;

    public void Init(int[] characterEquips)
    {
        ItemService.Instance.OnItemEquip += OnItemEquip;

        //Init equipment
        equipments = new int[7];
        for(int i = 0; i < 7; i++)
        {
            equipments[i] = characterEquips[i];
        }
    }

    public int[] GetEquipments()
    {
        return equipments;
    }

    /// <summary>
    /// send item equipment request to item service
    /// </summary>
    /// <param name="itemID"></param>
    /// <param name="slot"></param>
    /// <param name="isEquip"></param>
    public void EquipItem(int itemID, bool isEquip)
    {
        if (!DataManager.Instance.Items.ContainsKey(itemID) || DataManager.Instance.Items[itemID].Type != E_ItemType.EQUIP)
            return;

        EquipDefine define = null;
        if (!DataManager.Instance.Equips.TryGetValue(itemID, out define))
        {
            return;
        }

        pendingEquip = itemID;
        this.isEquip = isEquip;
        int slot = (int) define.Slot;

        ItemService.Instance.SendEquipItem(itemID, slot, isEquip);
    }

    
    /// <summary>
    /// automatically called to equip the current pending item
    /// </summary>
    /// <param name="result"></param>
    /// <param name="errormsg"></param>
    private void OnItemEquip(Result result, string errormsg)
    {
        Debug.LogFormat("EquipManager->OnItemEquip: result: {0}", result);

        if (result == Result.Failed)
        {
            Debug.LogFormat("the item {0} cannot be equipped", pendingEquip);
            return;
        }

        if (pendingEquip == 0)
        {
            Debug.LogErrorFormat("No pending item can be found", pendingEquip);
            return;
        }

        if (isEquip)
        {
            EquipDefine equip = DataManager.Instance.Equips[pendingEquip];
            equipments[(int) equip.Slot] = pendingEquip;

            ItemManager.Instance.RemoveItem(pendingEquip);
            BagManager.Instance.SortBagItem();

            //refresh UI
            BagPanel panel = UIManager.Instance.GetPanel<BagPanel>(typeof(BagPanel));
            if (panel != null)
            {
                panel.Refresh();
            }

            pendingEquip = 0;
        }
        else
        {
            EquipDefine equip = DataManager.Instance.Equips[pendingEquip];
            equipments[(int)equip.Slot] = 0;

            ItemManager.Instance.AddItem(pendingEquip);
            BagManager.Instance.SortBagItem();

            //refresh UI
            BagPanel panel = UIManager.Instance.GetPanel<BagPanel>(typeof(BagPanel));
            if (panel != null)
            {
                panel.Refresh();
            }


            pendingEquip = 0;

        }

        ItemManager.Instance.Speak();
    }
}
