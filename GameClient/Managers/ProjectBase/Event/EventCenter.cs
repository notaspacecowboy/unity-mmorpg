using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public interface IEventInfo{}

public class EventInfo<T>: IEventInfo
{
    public UnityAction<T> actions;
    public EventInfo(UnityAction<T> action)
    {
        actions += action;
    }
}

public class EventInfo : IEventInfo
{
    public UnityAction actions;
    public EventInfo(UnityAction action)
    {
        actions += action;
    }
}


/// <summary>
/// 事件中心 单例模式对象
/// 1.Dictionary
/// 2.委托
/// 3.观察者设计模式
/// 4.泛型
/// </summary>
public class EventCenter : Singleton<EventCenter>
{
    /// <summary>
    /// 添加事件监听(泛型)
    /// </summary>
    /// <param name="eventName">被监听的事件名</param>
    /// <param name="action">准备用来处理事件的委托函数</param>
    public void AddEventListener<T>(string eventName, UnityAction<T> action)
    {
        //如果事件中心中存在该被监听事件
        if (eventDic.ContainsKey(eventName))
            (eventDic[eventName] as EventInfo<T>).actions += action;
        //如果事件中心中不存在该被监听事件
        else
            eventDic.Add(eventName, new EventInfo<T>(action));
    }

    /// <summary>
    /// 添加事件监听(非泛型)
    /// </summary>
    /// <param name="eventName">事件名</param>
    /// <param name="action">响应函数</param>
    public void AddEventListener(string eventName, UnityAction action)
    {
        //如果事件中心中存在该被监听事件
        if (eventDic.ContainsKey(eventName))
            (eventDic[eventName] as EventInfo).actions += action;
        //如果事件中心中不存在该被监听事件
        else
            eventDic.Add(eventName, new EventInfo(action));
    }

    /// <summary>
    /// 移除事件监听(泛型)
    /// </summary>
    /// <param name="eventName">被监听的事件名</param>
    /// <param name="action">需要被移除的委托</param>
    public void RemoveEventListener<T>(string eventName, UnityAction<T> action)
    {
        //如果事件中心中存在该被监听事件
        if (eventDic.ContainsKey(eventName))
            (eventDic[eventName] as EventInfo<T>).actions -= action;
    }

    /// <summary>
    /// 移除事件监听(非泛型)
    /// </summary>
    /// <param name="eventName">事件名</param>
    /// <param name="action">需移除的响应函数</param>
    public void RemoveEventListener(string eventName, UnityAction action)
    {
        //如果事件中心中存在该被监听事件
        if (eventDic.ContainsKey(eventName))
            (eventDic[eventName] as EventInfo).actions -= action;
    }

    /// <summary>
    /// 事件触发(泛型)
    /// </summary>
    /// <param name="eventName">被触发的事件名</param>
    public void EventTrigger<T>(string eventName, T info)
    {
        if (eventDic.ContainsKey(eventName) && 
            (eventDic[eventName] as EventInfo<T>).actions != null)
            (eventDic[eventName] as EventInfo<T>).actions.Invoke(info);
    }

    /// <summary>
    /// 事件触发(非泛型)
    /// </summary>
    /// <param name="eventName">事件名</param>
    public void EventTrigger(string eventName)
    {
        if (eventDic.ContainsKey(eventName) &&
            (eventDic[eventName] as EventInfo).actions != null)
            (eventDic[eventName] as EventInfo).actions.Invoke();
    }

    /// <summary>
    /// 清空事件中心(通常用于场景切换)
    /// </summary>
    public void Clear()
    {
        eventDic.Clear();
    }

    //string->事件名(怪物死亡，任务通关...)
    //UnityAction->事件发生后需要执行的函数们的委托
    private Dictionary<string, IEventInfo> eventDic = new Dictionary<string, IEventInfo>();

}
