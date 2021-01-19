//=============================
//作者: 杨政 
//时间: 09/18/2020 14:43:18
//=============================
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Services;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingPanel : BasePanel
{
    //Fields
    #region Fields of loadingpanel's UI components
    private Slider progressBar;
    private Text progressTxt;

    private EasyTween mAnim;
    #endregion

    #region Fields of current loading info

    /// <summary>
    /// if it is true, start loading simulation
    /// </summary>
    private bool mSimulation;

    #endregion

    //Methods
    #region Methods to initialize/finalize panel when called to show or hide
    public override void ShowMe()
    {
        base.ShowMe();
        
        //get slider bar component
        progressBar = GetComponent<Slider>("progressSlider");
        progressBar.value = 0;
        //get progress text component
        progressTxt = GetComponent<Text>("progressTxt");

        mAnim = GetComponent<EasyTween>();

        //add event listener of current loading status
        //EventCenter.Instance.AddEventListener<float>("loading update", LoadingUpdate);
        EventCenter.Instance.AddEventListener("start loading simulation", StartSimulation);

        mSimulation = true;
    }

    public override void HideMe()
    {
        //EventCenter.Instance.RemoveEventListener<float>("loading update", LoadingUpdate);
        EventCenter.Instance.RemoveEventListener("start loading simulation", StartSimulation);
        
        base.HideMe();
    }

    //void LoadingUpdate(float progress)
    //{
    //    progressBar.value = progress;
    //    progressTxt.text = "Loading..." + progress + "%";
    //}

    public void StartSimulation()
    {
        mSimulation = true;
    }

    #endregion


    #region Method of monobehaviors
    void Update()
    {
        if (mSimulation == true  && progressBar.value < 1f){
            progressBar.value += 0.05f;
            progressTxt.text = "Loading..." + Mathf.RoundToInt(progressBar.value * 100).ToString() + "%";
        }

        //if loading process finished and progressbar's value reach 1, switch scene
        else if (mSimulation == true)
        {
            mSimulation = false;
            EventCenter.Instance.EventTrigger("loading finish");
            mAnim.OpenCloseObjectAnimation();
        }
    }
    #endregion


    public void OnEndAnim()
    {
        UIManager.Instance.HidePanel(typeof(LoadingPanel));
    }
}
