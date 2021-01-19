using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// 使没有继承monobehaviour的类也可以使用update函数进行逐帧更新
/// 1.生命周期函数
/// 2.事件
/// 3.协程
/// </summary>
public class MonoController: MonoBehaviour
{
    void Start()
    {
        DontDestroyOnLoad(this.gameObject);
    }
    /// <summary>
    /// 当该帧中有需要被调用的更新函数，即invoke updateEvent
    /// </summary>
    void Update()
    {
        if (updateEvent != null)
            updateEvent();
    }

    /// <summary>
    /// 给外部提供的 添加帧更新事件的函数
    /// </summary>
    public void AddUpdateListener(UnityAction action)
    {
        updateEvent += action;
    }

    /// <summary>
    /// 给外部提供的 添加帧更新事件的函数
    /// </summary>
    public void RemoveUpdateListener(UnityAction action)
    {
        updateEvent -= action;
    }

    private event UnityAction updateEvent;
}
