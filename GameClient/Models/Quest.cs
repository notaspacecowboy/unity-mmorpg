//=============================
//Author: Zack Yang 
//Created Date: 12/28/2020 11:05
//=============================
using System.Collections;
using System.Collections.Generic;
using Common.Data;
using SkillBridge.Message;
using UnityEngine;

public class Quest
{
    public NQuestInfo info;
    public QuestDefine Define;

    public Quest(NQuestInfo mInfo)
    {
        info = mInfo;
        Define = DataManager.Instance.Quests[info.QuestId];
    }

    public Quest(QuestDefine mDefine)
    {
        Define = mDefine;
    }
}
