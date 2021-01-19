//=============================
//Author: Zack Yang 
//Created Date: 12/28/2020 9:20
//=============================
using System.Collections;
using System.Collections.Generic;
using Common.Data;
using UnityEngine;
using UnityEngine.UI;

public class QuestStatusUI : WorldUIElement
{
    public Image[] questStatusImgs;

    private NpcQuestStatus currentStatus;

    public void Init(Transform owner, NpcQuestStatus status)
    {
        base.Owner = owner;
        SetStatus(status);
    }
    public void SetStatus(NpcQuestStatus status)
    {
        currentStatus = status;
        for (int i = 0; i < questStatusImgs.Length; i++)
        {
            questStatusImgs[i].gameObject.SetActive(i == (int)status);
        }
    }
}
