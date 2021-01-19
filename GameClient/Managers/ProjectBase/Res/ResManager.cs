using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Object = UnityEngine.Object;

/// <summary>
/// 资源加载模块
/// </summary>      
public class ResManager : Singleton<ResManager>, IDisposable
{
    #region ResourceType resource type
    /// <summary>
    /// 资源类型
    /// </summary>
    public enum ResourceType
    {
        /// <summary>
        /// 画布
        /// </summary>
        Canvas,
        /// <summary>
        /// 面板
        /// </summary>
        Panel,
        /// <summary>
        /// 角色
        /// </summary>
        Character,
        /// <summary>
        /// 特效
        /// </summary>
        Effect,
        /// <summary>
        /// 音乐
        /// </summary>
        Music,
        /// <summary>
        /// 音效
        /// </summary>
        Sound,
        /// <summary>
        /// 通用预制体
        /// </summary>
        CommonPrefab,
        /// <summary>
        /// 背景图片
        /// </summary>
        BkImg,
        /// <summary>
        /// 小地图
        /// </summary>
        MiniMap,
        /// <summary>
        /// 物品
        /// </summary>
        Item
    }
    #endregion

    #region Load<T> load resource synchronously
    /// <summary>
    /// 同步加载资源
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="name"></param>
    /// <returns></returns>
    public T Load<T>(ResourceType type, string name) where T:Object
    {
        //根据资源类型给名称添加前缀地址
        switch (type)
        {
            case ResourceType.Canvas:
                name = "UI/Canvas/" + name;
                break;

            case ResourceType.Panel:
                name = "UI/Panels/" + name;
                break;

            case ResourceType.Character:
                name = "Character/" + name;
                break;

            case ResourceType.Effect:
                name = "Effect/" + name;
                break;

            case ResourceType.Music:
                name = "Music/" + name;
                break;

            case ResourceType.Sound:
                name = "Sound/" + name;
                break;

            case ResourceType.BkImg:
                name = "BkImg/" + name;
                break;

            case ResourceType.CommonPrefab:
                name = "CommonPrefab/" + name;
                break;

            case ResourceType.MiniMap:
                name = "MiniMap/" + name;
                break;

            case ResourceType.Item:
                name = name;
                break;
        }
        T res = Resources.Load<T>(name);
        //如果对象是一个GameObject,我们把它实例化后再返回出去，外部可以直接使用
        if (res is GameObject)
            return GameObject.Instantiate(res);
        //对于无需实例化的对象(audioclip,textasset...)直接返回出去
        else
            return res;
    }


    public T Load<T>(string name) where T : Object
    {
        T res = Resources.Load<T>(name);
        //如果对象是一个GameObject,我们把它实例化后再返回出去，外部可以直接使用
        if (res is GameObject)
            return GameObject.Instantiate(res);
        //对于无需实例化的对象(audioclip,textasset...)直接返回出去
        else
            return res;
    }
    #endregion

    #region LoadAsync<T> load resources asynchronouslu
    //异步加载资源:减少程序卡顿感
    //通过异步加载的资源无法马上使用，通常需要等待n帧(根据资源大小决定)才能加载完成
    //因此,我们的封装函数返回值为void
    public void LoadAsync<T>(ResourceType type, string name, UnityAction<T> callBack) where T:Object
    {
        //根据资源类型给名称添加前缀地址
        switch (type)
        {
            case ResourceType.Canvas:
                name = "UI/Canvas/" + name;
                break;

            case ResourceType.Panel:
                name = "UI/Panels/" + name;
                break;

            case ResourceType.Character:
                name = "Character/" + name;
                break;

            case ResourceType.BkImg:
                name = "BkImg/" + name;
                break;

            case ResourceType.Effect:
                name = "Effect/" + name;
                break;

            case ResourceType.CommonPrefab:
                name = "CommonPrefab/" + name;
                break;

            case ResourceType.Music:
                name = "Music/" + name;
                break;

            case ResourceType.Sound:
                name = "Sound/" + name;
                break;

            case ResourceType.Item:
                name = "UI/Items/" + name;
                break;
        }

        //开启异步加载的协程
        MonoManager.Instance.StartCoroutine(RealLoadAsync<T>(name,callBack));
    }

    /// <summary>
    /// 真正的协程函数，用于开启异步加载的资源
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="name"></param>
    /// <param name="callBack"></param>
    /// <returns></returns>
    private IEnumerator RealLoadAsync<T>(string name, UnityAction<T> callBack) where T : Object
    {
        ResourceRequest r = Resources.LoadAsync<T>(name);
        yield return r;

        if (r.asset is GameObject)
            callBack(GameObject.Instantiate(r.asset) as T);
        else
            callBack(r.asset as T);

    }
    #endregion

    #region Dispose release memory
    /// <summary>
    /// 释放资源
    /// </summary>
    public void Dispose()
    {
        PoolManager.Instance.Clear();
        //释放未使用的资源
        Resources.UnloadUnusedAssets();
    }
    #endregion
}
