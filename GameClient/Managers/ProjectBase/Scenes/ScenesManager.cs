using System;
using System.Collections;
using System.Collections.Generic;
using Services;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;


public class ScenesManager : Singleton<ScenesManager>
{
    private bool mIsInit = false;

    public void LoadScene(string sceneName, UnityAction doList = null)
    {
        SceneManager.LoadScene(sceneName);
        if(doList != null)
            doList();
    }

    public void LoadSceneAsync(string sceneName)
    {
        UIManager.Instance.ShowPanel<LoadingPanel>(typeof(LoadingPanel));
        SceneManager.LoadScene(sceneName);
        EventCenter.Instance.EventTrigger("start loading simulation");
    }
}
