//=============================
//Author: Zack Yang 
//Created Date: 12/24/2020 4:41
//=============================
using System.Collections;
using System.Collections.Generic;
using Common.Data;
using UnityEngine;
using UnityEngine.UI;

public class QuestListItem : ListView.ListViewItem
{
    private Image onSelectedImg;
    private Text questNameTxt;

    public QuestDefine Define;

    public override void ShowMe()
    {
        base.ShowMe();
        onSelectedImg = GetComponent<Image>("OnSelectedImg");
        questNameTxt = GetComponent<Text>("QuestNameTxt");
    }

    public void Init(QuestDefine mDefine)
    {
        Define = mDefine;
        questNameTxt.text = Define.Name;
        Selected = false;
    }

    public override void OnSelected(bool selected)
    {
        onSelectedImg.gameObject.SetActive(selected);
    }
}
