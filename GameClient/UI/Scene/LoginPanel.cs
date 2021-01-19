//=============================
//作者: 杨政 
//时间: 09/16/2020 20:06:10
//=============================
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using Network;
using Services;
using SkillBridge.Message;
using UnityEngine;
using UnityEngine.UI;

public class LoginPanel : BasePanel
{
    /// <summary>
    /// UI动画组件
    /// </summary>
    public EasyTween ToGameAnim;
    public EasyTween ToRegisterAnim;

    private Button mSignupButton;
    private Button mLoginButton;
    private Text mErrorMsg;
    private InputField mUsernameInput;
    private InputField mPwdInput;

    public override void ShowMe()
    {
        base.ShowMe();
        if (ToRegisterAnim != null)
            ToRegisterAnim.OpenCloseObjectAnimation();

        //find ui component
        mSignupButton = GetComponent<Button>("signupBtn");
        mLoginButton = GetComponent<Button>("loginBtn");

        mUsernameInput = GetComponent<InputField>("usernameInputField");
        mPwdInput = GetComponent<InputField>("passwordInputField");

        mErrorMsg = GetComponent<Text>("errorMsgTxt");
        mErrorMsg.enabled = false;

        //add btn listeners
        mLoginButton.onClick.AddListener(OnUserClickLogin);

        mSignupButton.onClick.AddListener(OnUserClickRegister);

        //add userlogin response listener
        UserService.Instance.OnLogin += OnLogin;
    }

    public override void HideMe()
    {
        base.HideMe();
        UserService.Instance.OnLogin -= OnLogin;
    }

    private void OnUserClickLogin()
    {
        string username = mUsernameInput.text;
        string pwd = mPwdInput.text;

        UserService.Instance.SendLogin(username, pwd);
    }

    public void OnUserClickRegister()
    {
        ToRegisterAnim.OpenCloseObjectAnimation();
    }

    public void OnFinishToRegisterAnim()
    {
        UIManager.Instance.HidePanel(typeof(LoginPanel));
        UIManager.Instance.ShowPanel<SignupPanel>(typeof(SignupPanel));
    }

    public void OnFinishToGameAnim()
    {
        Invoke("SceneChange", 0.2f);
    }

    private void SceneChange()
    {
        UIManager.Instance.HidePanel(typeof(LoginPanel));
        ScenesManager.Instance.LoadSceneAsync("Scene_CharacterSelection");

    }

    /// <summary>
    /// automatically called when client receive the response of user login from server
    /// </summary>
    /// <param name="result"></param>
    /// <param name="errorMsg"></param>
    public void OnLogin(Result result, string errorMsg)
    {
        mErrorMsg.enabled = false;
        mErrorMsg.GetComponent<EasyTween>().ChangeSetState(false);

        if (result == Result.Failed)
        {
            mErrorMsg.color = Color.red;
            mErrorMsg.text = "Username and password does not match. Please try again";

            mErrorMsg.enabled = true;
            mErrorMsg.GetComponent<EasyTween>().OpenCloseObjectAnimation();
        }
        else   //switch to loading scene
        {
            if(ToGameAnim != null)
                ToGameAnim.OpenCloseObjectAnimation();
        }
    }
}
