using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class PoolData
{
    public PoolData(GameObject poolObj, string parentName)
    {
        fatherObj = new GameObject(parentName);
        fatherObj.transform.parent = poolObj.transform;
        mQueue = new Queue<GameObject>();
    }
    /// <summary>
    /// 向抽屉里存放物体obj,使它失活并将其存放至父节点中
    /// </summary>
    /// <param name="obj"></param>
    public void PushObj(GameObject obj)
    {
        obj.SetActive(false);
        obj.transform.parent = fatherObj.transform;
        mQueue.Enqueue(obj);
    }
    public GameObject GetObj(string name)
    {
        GameObject obj = mQueue.Dequeue();
        obj.name = name;
        obj.SetActive(true);
        obj.transform.parent = null;
        return obj;
    }

    public int Count { get{return mQueue.Count;} }
    private GameObject fatherObj;
    private Queue<GameObject> mQueue;
}



/// <summary>
/// 缓存池管理器，就像一个衣柜一样，当场景中需要实时生成物体时，从缓存池该物体所对应的
/// 数组中拿出一个该物体，当该实时生成物体被销毁时，重新将它放回缓存池
/// 
/// </summary>
public class PoolManager : Singleton<PoolManager>
{
    public PoolManager()
    {
        mPool = new Dictionary<string, PoolData>();
    }

    public void GetObj(ResManager.ResourceType type, string name, UnityAction<GameObject> callBackAction)
    {
        GameObject obj = null;
        //如果池子存在且有东西
        if (mPool.ContainsKey(name) && mPool[name].Count > 0)
            callBackAction(mPool[name].GetObj(name));

        else
        {
            //如果池子里没有东西,则异步加载该资源,加载完成后执行回调函数
            ResManager.Instance.LoadAsync<GameObject>(type, name, (o =>
            {
                o.name = name;
                callBackAction(o);
            }));
        }
    }

    /// <summary>
    /// 将名为name的物体obj压入缓存池中
    /// </summary>
    /// <param name="name"></param>
    /// <param name="obj"></param>
    public void PushObj(string name, GameObject obj)
    {
        if (poolObj == null)
            poolObj = new GameObject("Pool");

        if (mPool.ContainsKey(name))
            mPool[name].PushObj(obj);

        else
        {
            mPool.Add(name, new PoolData(poolObj, name));
            mPool[name].PushObj(obj);
        }
    }

    /// <summary>
    /// 清空缓存池的方法，主要用于场景切换
    /// </summary>
    public void Clear()
    {
        mPool.Clear();
        poolObj = null;
    }

    // 缓存池容器(衣柜)
    private Dictionary<string, PoolData> mPool;
    //作为所有pool中的物体在hierarchy视图中的根节点
    private GameObject poolObj;
}
