//=============================
//Author: Zack Yang 
//Created Date: 12/28/2020 10:23
//=============================
using System.Collections;
using System.Collections.Generic;
using Models;
using NUnit.Framework;
using SkillBridge.Message;
using UnityEngine;

public class QuestManager : Singleton<QuestManager>
{
    /// <summary>
    /// player's on-going quests
    /// </summary>
    public List<NQuestInfo> QuestInfo = new List<NQuestInfo>();

    /// <summary>
    /// all the quests defined in configuration file
    /// </summary>
    public Dictionary<int, Quest> Quests = new Dictionary<int, Quest>();

    /// <summary>
    /// key: npcID
    /// value: the list of all quests for a specefic npc, divided by the quest status
    /// </summary>
    public Dictionary<int, Dictionary<QuestStatus, List<Quest>>> NpcQuests = new Dictionary<int, Dictionary<QuestStatus, List<Quest>>>();

    public void Init(List<NQuestInfo> quests)
    {
        QuestInfo = quests;
        Quests.Clear();
        NpcQuests.Clear();

        InitQuests();
    }

    private void InitQuests()
    {
        //init quests that has been taken by player
        foreach (var info in QuestInfo)
        {
            Quest quest = new Quest(info);
            Quests[quest.info.QuestId] = quest;

            AddNpcQuest(quest.Define.AcceptNPC, quest);
            AddNpcQuest(quest.Define.SubmitNPC, quest);
        }

        foreach (var define in DataManager.Instance.Quests.Values)
        {
            if (define.LimitClass != (int)CharacterClass.None && define.LimitClass != (int)User.Instance.currentCharacter.Class)
            {
                continue;
            }

            if (define.LimitLevel > User.Instance.currentCharacter.Level)
            {
                continue;
            }

            if (Quests.ContainsKey(define.ID))
            {
                continue;
            }

            if (define.PreQuest > 0)
            {
                Quest preQuest;
                Quests.TryGetValue(define.PreQuest, out preQuest);

                if(preQuest.info == null || preQuest.info.Status != QuestStatus.Finished)
                    continue;
            }

            Quest quest = new Quest(define);
            Quests[quest.info.QuestId] = quest;

            AddNpcQuest(quest.Define.AcceptNPC, quest);
            AddNpcQuest(quest.Define.SubmitNPC, quest);
        }
    }

    private void AddNpcQuest(int npcID, Quest quest)
    {
        if (!NpcQuests.ContainsKey(npcID))
        {
            NpcQuests[npcID] = new Dictionary<QuestStatus, List<Quest>>();
            NpcQuests[npcID][QuestStatus.InProgress] = new List<Quest>();
            NpcQuests[npcID][QuestStatus.Complated] = new List<Quest>();
            NpcQuests[npcID][QuestStatus.Finished] = new List<Quest>();
        }

        if (quest.info != null)
        {
            NpcQuests[npcID][QuestStatus.InProgress].Add(quest);
        }
        else
        {
            NpcQuests[npcID][QuestStatus.Finished].Add(quest);
        }
    }
}
