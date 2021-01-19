//=============================
//Author: Zack Yang 
//Created Date: 11/27/2020 1:11
//=============================
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Common.Data;
using SkillBridge.Message;
using UnityEngine;
using UnityScript.Scripting.Pipeline;

public class BagManager : Singleton<BagManager>
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct BagItem
    {
        public ushort ItemID;
        public ushort Count;

        public static BagItem Zero = new BagItem() {ItemID = 0, Count = 0};

        public static bool operator==(BagItem a, BagItem b)
        {
            if (a.ItemID == b.ItemID && a.Count == b.Count)
            {
                return true;
            }

            return false;
        }

        public static bool operator!=(BagItem a, BagItem b)
        {
            if (a.ItemID == b.ItemID && a.Count == b.Count)
            {
                return false;
            }

            return true;
        }

        public override bool Equals(object other)
        {
            if (other is BagItem)
            {
                return Equals((BagItem) other);
            }

            return false;
        }

        public bool Equals(BagItem other)
        {
            return this == other;
        }

        public override int GetHashCode()
        {
            return this.Count.GetHashCode() ^ this.ItemID.GetHashCode();
        }
    }


    private int mSize;
    private int count;
    public BagItem[] Items;
    private NBagInfo info;

    unsafe public void Init(NBagInfo bag)
    {
        mSize = bag.Size;
        this.info = bag;

        Items = new BagItem[mSize];

        if (bag.Items != null && bag.Items.Length > mSize)
        {
            UnpackBagItem();
        }
        else
        {
            info.Items = new byte[sizeof(BagItem) * mSize];
            SortBagItem();
        }
    }


    public void SortBagItem()
    {
        int i = 0;
        foreach (var item in ItemManager.Instance.items.Values)
        {
            if (i >= mSize)
                break;
            if (item.Count <= item.define.StackLimit)
            {
                Items[i].ItemID = (ushort) item.ID;
                Items[i].Count = (ushort) item.Count;
                i++;
            }
            else
            {
                int acc = item.Count;
                while (acc > item.define.StackLimit)
                {
                    if (i >= mSize)
                        break;

                    Items[i].ItemID = (ushort) item.ID;
                    Items[i].Count = (ushort) item.define.StackLimit;
                    acc -= item.define.StackLimit;
                    i++;
                }
            }
        }
    }


    private unsafe void UnpackBagItem()
    {
        fixed (byte* p = info.Items)
        {
            for (int i = 0; i < mSize; i++)
            {
                BagItem* tmp = (BagItem*) (p + i * sizeof(BagItem));
                Items[i] = *tmp;
            }
        }
    }

    private unsafe NBagInfo PackBagItem()
    {
        fixed (byte* p = info.Items)
        {
            for (int i = 0; i < mSize; i++)
            {
                BagItem* tmp = (BagItem*) (p + i * sizeof(BagItem));
                *tmp = Items[i];
            }
        }

        return this.info;
    }


    public void UpdateBag()
    {
        SortBagItem();
    }

}
