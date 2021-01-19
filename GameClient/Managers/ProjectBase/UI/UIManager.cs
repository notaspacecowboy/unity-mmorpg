using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

//UI层级的枚举
public enum E_UILayer
{
    System,
    Top,
    Mid,
    Bot
};

public enum E_Canvas
{
    Main,
    World,
}


/// <summary>
/// UI管理器
/// 1.管理所有当前正在显示的面板
/// 2.提供给外部 显示 和 隐藏面板的接口
/// </summary>
public class UIManager : Singleton<UIManager>
{
    private class UIPanel
    {
        /// <summary>
        /// 
        /// </summary>
        public string ResourcePath;
        public E_UILayer Layer;
        public E_Canvas CanvasType;
        public BasePanel panel;
        public bool Cache;
    }


    #region main canvas infomation

    /// <summary>
    /// main canvas transform
    /// </summary>
    private RectTransform mMainCanvas;
    /// <summary>
    /// system layer of main canvas
    /// </summary>
    private Transform mMainSystem;
    /// <summary>
    /// top layer of main canvas
    /// </summary>
    private Transform mMainTop;
    /// <summary>
    /// mid layer of main canvas
    /// </summary>
    private Transform mMainMid;
    /// <summary>
    /// bot layer of main canvas
    /// </summary>
    private Transform mMainBot;

    #endregion

    #region world canvas information

    /// <summary>
    /// world canvas transform
    /// </summary>
    private Transform mWorldCanvas;
    /// <summary>
    /// current camera for the world canvas
    /// </summary>
    private Transform mLookAtCamera;
    /// <summary>
    /// camera cannot be used outside
    /// </summary>
    public Transform LookAtCamera
    {
        set
        {
            mLookAtCamera = value;
        }
    }

    #endregion

    private Dictionary<Type, UIPanel> mPanels = new Dictionary<Type, UIPanel>();

    public UIManager()
    {
        this.InitCanvas();
        
        //register all panels to dictionary
        mPanels.Add(typeof(BkPanel), new UIPanel(){ResourcePath = "BkPanel", Layer = E_UILayer.Bot, CanvasType = E_Canvas.Main, Cache = false});
        mPanels.Add(typeof(LoginPanel), new UIPanel() { ResourcePath = "LoginPanel", Layer = E_UILayer.Mid, CanvasType = E_Canvas.Main, Cache = false });
        mPanels.Add(typeof(SignupPanel), new UIPanel() { ResourcePath = "SignupPanel", Layer = E_UILayer.Mid, CanvasType = E_Canvas.Main, Cache = false });
        mPanels.Add(typeof(StartPanel), new UIPanel() { ResourcePath = "StartPanel", Layer = E_UILayer.Mid, CanvasType = E_Canvas.Main, Cache = false });
        mPanels.Add(typeof(LoadingPanel), new UIPanel() { ResourcePath = "LoadingPanel", Layer = E_UILayer.System, CanvasType = E_Canvas.Main, Cache = false });
        mPanels.Add(typeof(JoystickPanel), new UIPanel() { ResourcePath = "JoystickPanel", Layer = E_UILayer.Top, CanvasType = E_Canvas.Main, Cache = false });
        mPanels.Add(typeof(MiniMapPanel), new UIPanel() { ResourcePath = "MiniMapPanel", Layer = E_UILayer.Mid, CanvasType = E_Canvas.Main, Cache = false });
        //mPanels.Add(typeof(NameBar), new UIPanel() { ResourcePath = "NameBar", Layer = E_UILayer.Top, CanvasType = E_Canvas.Main, Cache = false });
        mPanels.Add(typeof(CharCreationPanel), new UIPanel() { ResourcePath = "CharCreationPanel", Layer = E_UILayer.Mid, CanvasType = E_Canvas.Main, Cache = false });
        mPanels.Add(typeof(CharSelectionPanel), new UIPanel() { ResourcePath = "CharSelectionPanel", Layer = E_UILayer.Mid, CanvasType = E_Canvas.Main, Cache = false });
        //mPanels.Add(typeof(CharTgl), new UIPanel() { ResourcePath = "CharTgl", Layer = E_UILayer.Top, CanvasType = E_Canvas.Main, Cache = false });
        mPanels.Add(typeof(MainCityPanel), new UIPanel() { ResourcePath = "MainCityPanel", Layer = E_UILayer.Mid, CanvasType = E_Canvas.Main, Cache = false });
        mPanels.Add(typeof(DamagePanel), new UIPanel() { ResourcePath = "DamagePanel", CanvasType = E_Canvas.World, Cache = true });
        
        mPanels.Add(typeof(BagPanel), new UIPanel() { ResourcePath = "BagPanel", Layer = E_UILayer.Top, CanvasType = E_Canvas.Main, Cache = true });
        
        mPanels.Add(typeof(ShopPanel), new UIPanel() { ResourcePath = "ShopPanel", Layer = E_UILayer.Top, CanvasType = E_Canvas.Main, Cache = false });
        mPanels.Add(typeof(ConfirmationPanel), new UIPanel() { ResourcePath = "ConfirmationPanel", Layer = E_UILayer.System, CanvasType = E_Canvas.Main, Cache = true });

        mPanels.Add(typeof(QuestPanel), new UIPanel() { ResourcePath = "QuestPanel", Layer = E_UILayer.Top, CanvasType = E_Canvas.Main, Cache = false });
    }


    /// <summary>
    /// this methods load and setup all canvas used in game
    /// </summary>
    private void InitCanvas()
    {
        //event system
        GameObject eventObj = ResManager.Instance.Load<GameObject>(ResManager.ResourceType.Canvas, "EventSystem");
        GameObject.DontDestroyOnLoad(eventObj);             //dont destroy when switch scenes

        //main canvas
        GameObject canvasObj = ResManager.Instance.Load<GameObject>(ResManager.ResourceType.Canvas, "MainCanvas");
        mMainCanvas = canvasObj.transform as RectTransform;
        GameObject.DontDestroyOnLoad(canvasObj);

        mMainSystem = mMainCanvas.Find("System");          //find all layers of main canvas
        mMainTop = mMainCanvas.Find("Top");
        mMainMid = mMainCanvas.Find("Mid");
        mMainBot = mMainCanvas.Find("Bot");

        //world canvas
        canvasObj = ResManager.Instance.Load<GameObject>(ResManager.ResourceType.Canvas, "WorldCanvas");
        mWorldCanvas = canvasObj.transform as RectTransform;
        GameObject.DontDestroyOnLoad(canvasObj);
    }


    /// <summary>
    /// show a UI panel based on its information stored in dictionary
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="panelType">class type of the panel to be shown</param>
    /// <param name="callBackAction"></param>
    public void ShowPanel<T>(Type panelType, UnityAction<T> callBackAction = null) where T: BasePanel
    {
        //if the panel is already loaded, refresh it and callback
        if (mPanels.ContainsKey(panelType))
        {
            if (mPanels[panelType].panel != null)
            {
                mPanels[panelType].panel.ShowMe();

                if (callBackAction != null)
                    callBackAction(mPanels[panelType].panel as T);
                return;
            }
            //if the panel is registered but not yet loaded, load it and then callback
            else
            {
                UIPanel current = mPanels[panelType];
                ResManager.Instance.LoadAsync<GameObject>(ResManager.ResourceType.Panel, current.ResourcePath,
                    (obj) =>
                    {
                        Transform father;
                        switch (current.Layer)
                        {
                            case E_UILayer.System:
                                father = mMainSystem;
                                break;
                            case E_UILayer.Top:
                                father = mMainTop;
                                break;
                            case E_UILayer.Mid:
                                father = mMainMid;
                                break;
                            default:
                                father = mMainBot;
                                break;
                        }

                        //set hierarchy
                        obj.transform.SetParent(father);
                        obj.transform.localScale = Vector3.one;
                        obj.name = current.ResourcePath;
                        (obj.transform as RectTransform).offsetMax = Vector2.zero;
                        (obj.transform as RectTransform).offsetMin = Vector2.zero;

                        //init panel
                        current.panel = obj.GetComponent<T>();
                        current.panel.ShowMe();

                        //处理面板创建完成后的逻辑
                        if (callBackAction != null)
                            callBackAction(current.panel as T);
                    });
            }
            
        }
    }

    /// <summary>
    /// hide a panel
    /// </summary>
    /// <param name="panelType"></param>
    public void HidePanel(Type panelType)
    {
        if (mPanels.ContainsKey(panelType))
        {
            if (mPanels[panelType].panel != null)
            {
                mPanels[panelType].panel.HideMe();
                GameObject.Destroy(mPanels[panelType].panel.gameObject);
                mPanels[panelType].panel = null;
            }
        }
    }


    /// <summary>
    /// return a panel if it is loaded, otherwise return null
    /// </summary>
    /// <param name="panelName"></param>
    public T GetPanel<T>(Type paneltType) where T:BasePanel
    {
        if (mPanels.ContainsKey(paneltType))
        {
            if (mPanels[paneltType].panel != null)
            {
                return mPanels[paneltType].panel as T;
            }
        }
        
        return null;
    }


    //public Transform GetLayerTransform(E_UILayer layer)
    //{
    //    switch (layer)
    //    {
    //        case E_UILayer.Bot:
    //            return mMainBot;

    //        case E_UILayer.Mid:
    //            return mMainMid;

    //        case E_UILayer.Top:
    //            return mMainTop;

    //        case E_UILayer.System:
    //            return mMainSystem;

    //        default:
    //            return null;
    //    }
    //}

    
    /// <summary>
    /// add a event listener on a ui component
    /// </summary>
    /// <param name="control">ui component</param>
    /// <param name="type">event type</param>
    /// <param name="callBackAction">call back function</param>
    public static void AddCustomEventListener(UIBehaviour control, EventTriggerType type, UnityAction<BaseEventData> callBackAction)
    {
        EventTrigger trigger = control.GetComponent<EventTrigger>();
        if (trigger == null)
            trigger = control.gameObject.AddComponent<EventTrigger>();
        EventTrigger.Entry entry = new EventTrigger.Entry();
        entry.eventID = type;
        entry.callback.AddListener(callBackAction);
        trigger.triggers.Add(entry);
    }
}
