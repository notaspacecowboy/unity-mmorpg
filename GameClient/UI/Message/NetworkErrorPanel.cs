//=============================
//Author: Zack Yang 
//Created Date: 10/13/2020 0:38
//=============================
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NetworkErrorPanel : BasePanel
{
    public override void ShowMe()
    {
        base.ShowMe();

        GetComponent<Button>("hideMeBtn").onClick.AddListener(() =>
        {
            UIManager.Instance.HidePanel(typeof(NetworkErrorPanel));
        });
    }
}
