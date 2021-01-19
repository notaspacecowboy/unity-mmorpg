//=============================
//Author: Zack Yang 
//Created Date: 12/24/2020 4:55
//=============================
using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using Common.Data;
using UnityEngine;
using UnityEngine.UI;

public class QuestPanel : BasePanel
{
    public GameObject ListItemPrefab;
    public ListView MainQuestList;
    public ListView SideQuestList;

    //UI controls
    private Text questTitleTxt;
    private Text descriptionTxt;
    private Text targetTxt;
    private Text goldTxt;
    private Text expTxt;
    private Image itemReword1;
    private Image itemReword2;
    private Image itemReword3;
    private Button abandonBtn;
    private Button navigateBtn;

    public override void ShowMe()
    {
        base.ShowMe();

        questTitleTxt = GetComponent<Text>("QuestTitleTxt");
        descriptionTxt = GetComponent<Text>("DescriptionTxt");
        targetTxt = GetComponent<Text>("TargetTxt");
        goldTxt = GetComponent<Text>("GoldTxt");
        expTxt = GetComponent<Text>("ExpTxt");
        itemReword1 = GetComponent<Image>("ItemReword1");
        itemReword2 = GetComponent<Image>("ItemReword2");
        itemReword3 = GetComponent<Image>("ItemReword3");
        abandonBtn = GetComponent<Button>("AbandonBtn");
        navigateBtn = GetComponent<Button>("NavigateBtn");

        MainQuestList.OnItemSelected += OnItemSelected;
        SideQuestList.OnItemSelected += OnItemSelected;
    }

    public override void HideMe()
    {
        base.HideMe();

        MainQuestList.OnItemSelected -= OnItemSelected;
        SideQuestList.OnItemSelected -= OnItemSelected;
    }

    private void RefreshUI()
    {
        //clear all the quest lists
        MainQuestList.Clear();
        SideQuestList.Clear();

        //init all the quest lists again
        foreach (var define in DataManager.Instance.Quests.Values)
        {
            GameObject obj = Instantiate(ListItemPrefab);
            QuestListItem item = obj.GetComponent<QuestListItem>();
            item.Init(define);

            if (define.Type == QuestDefine.E_QuestType.MAIN)
            {
                MainQuestList.AddItem(item);
            }
            else
            {
                SideQuestList.AddItem(item);
            }
        }
    }

    private void OnItemSelected(ListView.ListViewItem item)
    {
        QuestDefine define = ((QuestListItem) item).Define;

        questTitleTxt.text = define.Name;
        descriptionTxt.text = define.Overview;

        ItemDefine reward = DataManager.Instance.Items[define.RewardItem1];
        if (reward != null)
        {
            itemReword1.sprite = ResManager.Instance.Load<Sprite>(ResManager.ResourceType.Item, reward.Icon);

            reward = DataManager.Instance.Items[define.RewardItem2];
            if (reward != null)
            {
                itemReword2.sprite = ResManager.Instance.Load<Sprite>(ResManager.ResourceType.Item, reward.Icon);

                reward = DataManager.Instance.Items[define.RewardItem3];
                if (reward != null)
                {
                    itemReword3.sprite = ResManager.Instance.Load<Sprite>(ResManager.ResourceType.Item, reward.Icon);
                }
            }
        }

        goldTxt.text = "Gold: " + define.RewardGold.ToString();
        expTxt.text = "Exp: " + define.RewardExp.ToString();

    }
}
