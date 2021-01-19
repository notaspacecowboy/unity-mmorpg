//=============================
//Author: Zack Yang 
//Created Date: 11/03/2020 23:46
//=============================
using System.Collections;
using System.Collections.Generic;
using Common.Data;
using UnityEngine;

public class WorldUIManager : MonoSingleton<WorldUIManager>
{
    public string NameBarPath = "NameBar";
    public string QuestStatusPath = "QuestStatusUI";

    //增加任务,npc的prefab信息

    public Transform worldCanvasTransform;
    public Transform lookAtCamera;

    private Dictionary<Transform, GameObject> nameBars = new Dictionary<Transform, GameObject>();
    private Dictionary<Transform, GameObject> npcQuestStatus = new Dictionary<Transform, GameObject>();

    #region player namebar UI

    public void AddNameBar(Transform owner, Character character)
    {
        if (!nameBars.ContainsKey(owner))
        {
            GameObject obj = ResManager.Instance.Load<GameObject>(ResManager.ResourceType.Panel, NameBarPath);
            obj.transform.parent = worldCanvasTransform;
            obj.transform.localScale = Vector3.one;

            NameBar nameBar = obj.GetComponent<NameBar>();
            nameBar.Init(character, owner);

            nameBars.Add(owner, obj);
        }
    }

    public void RemoveNameBar(Transform owner)
    {
        if (!nameBars.ContainsKey(owner))
            return;

        Destroy(nameBars[owner]);
        nameBars.Remove(owner);
    }

    #endregion


    #region NPC quest mark UI

    public void AddNpcQuest(Transform owner, NpcQuestStatus status)
    {
        if (!npcQuestStatus.ContainsKey(owner))
        {
            GameObject obj = ResManager.Instance.Load<GameObject>(ResManager.ResourceType.Panel, QuestStatusPath);
            obj.transform.parent = worldCanvasTransform;
            obj.transform.localScale = Vector3.one;

            QuestStatusUI statusUI = obj.GetComponent<QuestStatusUI>();
            statusUI.Init(owner, status);

            npcQuestStatus.Add(owner, obj);
        }
    }

    public void RemoveNpcQuest(Transform owner)
    {
        if (!npcQuestStatus.ContainsKey(owner))
            return;

        Destroy(npcQuestStatus[owner]);
        npcQuestStatus.Remove(owner);
    }

    #endregion
}