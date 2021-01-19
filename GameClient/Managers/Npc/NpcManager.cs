//=============================
//Author: Zack Yang 
//Created Date: 11/22/2020 22:20
//=============================
using System;
using System.Collections;
using System.Collections.Generic;
using Common.Data;
using UnityEngine;
using UnityScript.Scripting.Pipeline;

public class NpcManager : Singleton<NpcManager>
{
    public delegate bool NpcEventHandler(NpcDefine npc);

    private Dictionary<NpcFunction, NpcEventHandler> NpcEvents = new Dictionary<NpcFunction, NpcEventHandler>();

    public void AddEventListener(NpcFunction function, NpcEventHandler handler)
    {
        if (NpcEvents.ContainsKey(function))
        {
            NpcEvents[function] += handler;
        }
        else
        {
            NpcEvents.Add(function, handler);
        }
    }

    public void RemoveEventListener(NpcFunction function, NpcEventHandler handler)
    {
        if (NpcEvents.ContainsKey(function))
        {
            NpcEvents[function] -= handler;
        }
    }

    public bool EventTrigger(NpcDefine npc)
    {
        switch (npc.Type)
        {
            case NpcType.Task:
                return TaskInteractive(npc);

            case NpcType.Functional:
                return FunctionalInteractive(npc);

            default:
                return false;
        }
    }

    private bool FunctionalInteractive(NpcDefine npc)
    {
        Debug.Log("interact with a functional npc");
        if (NpcEvents.ContainsKey(npc.Function))
        {
            NpcEvents[npc.Function].Invoke(npc);
            return true;
        }
        else
        {
            return false;
        }
    }

    private bool TaskInteractive(NpcDefine npc)
    {
        Debug.Log("interact with a task npc");
        return true;
    }

    public bool EventTrigger(int npcID)
    {
        NpcDefine npc = DataManager.Instance.Npcs[npcID];
        if (npc != null)
        {
            return EventTrigger(npc);
        }
        else
        {
            return false;
        }
    }

    public void Clear()
    {
        NpcEvents.Clear();
    }

    public NpcDefine GetNpcInfo(int npcID)
    {
        return DataManager.Instance.Npcs[npcID];
    }


    public void StopInteractive(NpcDefine define)
    {
        Debug.Log("NpcManager => StopInteractive");
        EventCenter.Instance.EventTrigger<NpcDefine>("StopInteractive", define);
    }
}
