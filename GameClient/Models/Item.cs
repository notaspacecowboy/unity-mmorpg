//=============================
//Author: Zack Yang 
//Created Date: 11/24/2020 22:03
//=============================
using System.Collections;
using System.Collections.Generic;
using Common.Data;
using SkillBridge.Message;
using UnityEngine;

public class Item
{
    public int ID;
    public int Count;
    public ItemDefine define;

    public Item(NItemInfo itemInfo)
    {
        ID = itemInfo.Id;
        Count = itemInfo.Count;
        define = DataManager.Instance.Items[ID];
    }

    public Item(int itemID, int count)
    {
        ID = itemID;
        Count = count;
        define = DataManager.Instance.Items[ID];
    }
}
