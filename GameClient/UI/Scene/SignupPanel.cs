//=============================
//作者: 杨政 
//时间: 09/16/2020 20:06:36
//=============================
using System.Collections;
using System.Collections.Generic;
using Services;
using SkillBridge.Message;
using UnityEngine;
using UnityEngine.UI;

public class SignupPanel : BasePanel
{
    //Fields
    #region Fields of signuppanel's UI components
    private Button mSignupBtn;
    private Button mBackToLoginBtn;
    private InputField mUsernameInput;
    private InputField mPwdInput;
    private InputField mConfirmPwdInput;
    private Text mErrorMsg;

    public EasyTween anim;
    #endregion

    //Methods
    #region Methods to initialize/dispose panel when called to show or hide
    public override void ShowMe()
    {
        base.ShowMe();

        anim.OpenCloseObjectAnimation();

        //subscribe for user login response
        UserService.Instance.OnRegister += this.OnRegister;

        //get all the useful ui components
        mSignupBtn = GetComponent<Button>("signupBtn");
        mBackToLoginBtn = GetComponent<Button>("backToLoginBtn");
        mUsernameInput = GetComponent<InputField>("usernameInputField");
        mPwdInput = GetComponent<InputField>("passwordInputField");
        mConfirmPwdInput = GetComponent<InputField>("confirmPasswordInputField");
        mErrorMsg = GetComponent<Text>("errorMsgTxt");
        mErrorMsg.enabled = false;

        //change the input type of pwd into password
        mPwdInput.inputType = InputField.InputType.Password;
        mConfirmPwdInput.inputType = InputField.InputType.Password;

        //add event listener for btn click events
        mSignupBtn.onClick.AddListener(() =>
        {
            mErrorMsg.enabled = false;
            mErrorMsg.GetComponent<EasyTween>().ChangeSetState(false);
            string username = mUsernameInput.text.ToString();
            string pwd = mPwdInput.text.ToString();
            string confirmPwd = mConfirmPwdInput.text.ToString();

            bool flag = CheckUserInput(username, pwd, confirmPwd);

            if (flag)
            {
                UserService.Instance.SendRegister(username, pwd);
            }
        });

        mBackToLoginBtn.onClick.AddListener(OnBackToLoginBtnClick);
    }

    public override void HideMe()
    {
        base.HideMe();

        //unsubscribe for the user resigister response
        UserService.Instance.OnRegister -= OnRegister;
    }

    #endregion

    #region registeration method

    /// <summary>
    /// This method is automatically called when receive user register response
    /// </summary>
    /// <param name="result"></param>
    /// <param name="erroMsg"></param>
    private void OnRegister(Result result, string erroMsg)
    {
        Debug.LogFormat("SignupPanel -> OnRegister(): result: {0}, errorMsg: {1}", result, erroMsg);

        if (result == Result.Success)
        {
            mErrorMsg.color = Color.green;
            mErrorMsg.text = "All set. Now you are good to go!";
        }

        else
        {
            mErrorMsg.color = Color.red;
            mErrorMsg.text = "Registeration failed! user already exist";

        }

        mErrorMsg.enabled = true;
        mErrorMsg.GetComponent<EasyTween>().OpenCloseObjectAnimation();
    }

    #endregion

    #region btn click method

    private void OnBackToLoginBtnClick()
    {
        if(anim != null)
            anim.OpenCloseObjectAnimation();
    }


    public void OnAnimFinish()
    {
        UIManager.Instance.HidePanel(typeof(SignupPanel));
        UIManager.Instance.ShowPanel<LoginPanel>(typeof(LoginPanel));
    }
    #endregion

    #region private helper functions

    /// <summary>
    /// check if the user input is in correct format
    /// </summary>
    /// <param name="username"></param>
    /// <param name="pwd"></param>
    /// <param name="confirmPwd"></param>
    /// <returns></returns>
    private bool CheckUserInput(string username, string pwd, string confirmPwd)
    {
        if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(pwd) || string.IsNullOrEmpty(confirmPwd))
        {
            mErrorMsg.text = "Please fill in all the information.";
            mErrorMsg.enabled = true;
            mErrorMsg.GetComponent<EasyTween>().OpenCloseObjectAnimation();
            return false;
        }
        if (pwd != confirmPwd)
        {
            mErrorMsg.text = "Password do not match.";
            mErrorMsg.enabled = true;
            mErrorMsg.GetComponent<EasyTween>().OpenCloseObjectAnimation();
            return false;
        }

        if (pwd.Length < 8)
        {
            mErrorMsg.text = "Password must have at least 8 characters.";
            mErrorMsg.enabled = true;
            mErrorMsg.GetComponent<EasyTween>().OpenCloseObjectAnimation();
            return false;
        }
        return true;
    }

    #endregion
}
