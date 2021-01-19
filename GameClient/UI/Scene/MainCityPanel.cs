//=============================
//Author: Zack Yang 
//Created Date: 11/02/2020 23:18
//=============================
using System.Collections;
using System.Collections.Generic;
using Models;
using Services;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainCityPanel : BasePanel
{
    private Text mNameTxt;
    private Text mLevelTxt;
    private Button mLeaveGameBtn;
    private Button mBagBtn;

    public override void ShowMe()
    {
        base.ShowMe();

        mNameTxt = GetComponent<Text>("NameTxt");
        mLevelTxt = GetComponent<Text>("LevelTxt");
        mLeaveGameBtn = GetComponent<Button>("LeaveGameBtn");
        mBagBtn = GetComponent<Button>("BagBtn");

        mNameTxt.text = Models.User.Instance.currentCharacter.Name;
        mLevelTxt.text = Models.User.Instance.currentCharacter.Level.ToString();
        
        mLeaveGameBtn.onClick.AddListener(OnLeaveGameBtnPressed);
        mBagBtn.onClick.AddListener(() =>
        {
            UIManager.Instance.ShowPanel<BagPanel>(typeof(BagPanel));
        });
    }

    public void OnLeaveGameBtnPressed()
    {
        MainCityManager.Instance.LeaveMainCity();
        ScenesManager.Instance.LoadScene("Scene_CharacterSelection");
        //ScenesManager.Instance.LoadSceneAsync("Scene_CharacterSelection");
        UserService.Instance.SendLeaveGame(Models.User.Instance.currentCharacter);
    }
}
