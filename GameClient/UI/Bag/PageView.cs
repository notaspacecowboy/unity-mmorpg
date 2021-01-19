//=============================
//Author: Zack Yang 
//Created Date: 11/26/2020 20:02
//=============================
using System.Collections;
using System.Collections.Generic;
using Common.Data;
using UnityEngine;
using UnityEngine.UI;

public class PageView : MonoBehaviour
{
    private bool mIsInit = false;
    private List<GameObject> items = new List<GameObject>();

    public Transform content;
    public void Show()
    {
        this.gameObject.SetActive(true);
        if (!mIsInit)
        {
            mIsInit = true;
            StartCoroutine(SetupBag());
        }
    }

    private IEnumerator SetupBag()
    {
        foreach (var obj in items)
        {
            Destroy(obj);
        }
        items.Clear();
        foreach (var item in ItemManager.Instance.items.Values)
        {
            if (item == null || item.Count <= 0 || !DataManager.Instance.Items.ContainsKey(item.ID))
                continue;

            ItemDefine define = DataManager.Instance.Items[item.ID];

            GameObject obj = ResManager.Instance.Load<GameObject>(ResManager.ResourceType.Panel, "BagItemUI");
            obj.transform.parent = content;
            obj.transform.localScale = Vector3.one;
            obj.transform.localPosition = Vector3.zero;
            BagItemUI bagItemUI = obj.GetComponent<BagItemUI>();
            bagItemUI.Init(define, item.Count);

            items.Add(obj);

        }

        yield return new WaitForEndOfFrame();
    }


    public void Hide()
    {
        this.gameObject.SetActive(false);
    }

    public void Refresh()
    {
        StartCoroutine(SetupBag());
    }
}
