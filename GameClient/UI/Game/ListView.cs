//=============================
//Author: Zack Yang 
//Created Date: 12/23/2020 2:24
//=============================
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class ListView : MonoBehaviour
{
    public class ListViewItem : BasePanel, IPointerClickHandler
    {
        private bool selected = false;
        public bool Selected
        {
            get
            {
                return selected;
            }

            set
            {
                selected = value;
                OnSelected(selected);
            }
        }

        private ListView owner;

        public void Init(ListView mOwner)
        {
            owner = mOwner;
        }

        /// <summary>
        /// method to be called when user selected listviewItem 
        /// </summary>
        /// <param name="selected"></param>
        public virtual void OnSelected(bool selected)
        {
        }
        public void OnPointerClick(PointerEventData eventData)
        {
            if (!selected)
            {
                Selected = true;
            }

            if (owner != null && owner.SelectedItem != this)
            {
                owner.SelectedItem = this;
            }
        }
    }

    public UnityAction<ListViewItem> OnItemSelected = null;

    private List<ListViewItem> items = new List<ListViewItem>();

    private ListViewItem selectedItem = null;
    public ListViewItem SelectedItem
    {
        get
        {
            return selectedItem;
        }
        private set
        {
            if (selectedItem != value)
            {
                selectedItem.Selected = false;
                selectedItem = value;

                if (OnItemSelected != null)
                {
                    OnItemSelected.Invoke((ListViewItem)value);
                }
            }
        }
    }

    public void AddItem(ListViewItem mItem)
    {
        mItem.Init(this);
        items.Add(mItem);
    }

    public void Clear()
    {
        foreach (var item in items)
        {
            Destroy(item.gameObject);
        }

        items.Clear();
    }
}
