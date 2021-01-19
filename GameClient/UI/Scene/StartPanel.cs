//=============================
//作者: 杨政 
//时间: 09/16/2020 19:10:11
//=============================
using System.Collections;
using System.Collections.Generic;
using System.Net.Mime;
using UnityEngine;
using UnityEngine.UI;

public class StartPanel : BasePanel
{
    //Fields
    #region Fields of startpanel's UI components
    
    public EasyTween StartAnimation;

    private EasyTween fadeTxt;
    #endregion

    //Methods
    #region Methods to initialize/finalize panel when called to show or hide
    public override void ShowMe()
    {
        base.ShowMe();
        if (StartAnimation != null)
            StartAnimation.Invoke("OpenCloseObjectAnimation", 0.5f);
    }

    public override void HideMe()
    {
        InputManager.Instance.setStatus(false);
        UIManager.Instance.GetPanel<BkPanel>(typeof(BkPanel)).ChangeBk("loginBk");
        UIManager.Instance.ShowPanel<LoginPanel>(typeof(LoginPanel));
        base.HideMe();
    }

    /// <summary>
    /// When all the ui animation finished, allow the player to tap the screen to change to login panel
    /// </summary>
    public void ReadyToTap()
    {
        InputManager.Instance.setStatus(true);
        EventCenter.Instance.AddEventListener("AnyKeydown", KeyDownListener);
    }

    public void KeyDownListener()
    {
        UIManager.Instance.HidePanel(typeof(StartPanel));
    }
    #endregion
}
