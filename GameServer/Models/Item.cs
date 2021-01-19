using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer.Models
{
    class Item
    {
        public int ID;
        public int Count;

        private TCharacterItem mItem;

        public Item(TCharacterItem item)
        {
            mItem = item;
            ID = mItem.ItemID;
            Count = item.Count;
        }

        public void Add(int amount = 1)
        {
            Count += amount;
            mItem.Count = Count;
        }

        public void Consume(int amount = 1)
        {
            if (Count < amount)
            {
                Count = 0;
            }
            else
            {
                Count -= amount;
            }
            mItem.Count = Count;
        }
    }
}
