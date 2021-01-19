//=============================
//Author: Zack Yang 
//Created Date: 11/24/2020 22:09
//=============================
using System.Collections;
using System.Collections.Generic;
using SkillBridge.Message;
using UnityEngine;

public class ItemManager : Singleton<ItemManager>
{
    public Dictionary<int, Item> items = new Dictionary<int, Item>();

    public void Init(List<NItemInfo> itemInfos)
    {
        Item item = null;
        foreach (var itemInfo in itemInfos)
        {
            if(!items.ContainsKey(itemInfo.Id))
                items.Add(itemInfo.Id, new Item(itemInfo));
        }

        ItemService.Instance.OnItemNotify += OnItemNotify;
    }

    public void OnItemNotify(NStatus status)
    {
        Debug.Log(status.Type);
        Debug.Log(status.Action);
        if (status.Type == StatusType.Item && status.Action == StatusAction.Add)
        {
            this.AddItem(status.Id, status.Value);
            BagManager.Instance.UpdateBag();
        }
        else if (status.Type == StatusType.Item && status.Action == StatusAction.Remove)
        {
            this.RemoveItem(status.Id, status.Value);
            BagManager.Instance.UpdateBag();
        }
    }

    public void AddItem(int itemID, int amount = 1)
    {
        Item item = null;
        if (items.TryGetValue(itemID, out item))
        {
            item.Count += amount;
        }
        else
        {
            items.Add(itemID, new Item(itemID, amount));
        }
    }

    public void RemoveItem(int itemID, int amount = 1)
    {
        if (!items.ContainsKey(itemID))
            return;

        if (items[itemID].Count < amount)
            return;

        items[itemID].Count -= amount;
    }

    public void Speak()
    {
        foreach (var item in items.Values)
        {
            Debug.LogFormat("{0}  amount: {1}", item.define.Name, item.Count);
        }
    }
}
