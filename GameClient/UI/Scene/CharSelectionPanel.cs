//=============================
//Author: Zack Yang 
//Created Date: 10/22/2020 23:15
//=============================

using System;
using System.Collections;
using System.Collections.Generic;
using Models;
using Services;
using SkillBridge.Message;
using UnityEngine;
using UnityEngine.UI;

public class CharSelectionPanel : BasePanel
{
    private int i = 0;
    private List<SkillBridge.Message.NCharacterInfo> characters;
    private List<CharTgl> charList = new List<CharTgl>();
    private Button mCreateCharBtn;
    private Button mEnterGameBtn;

    public Transform content;
    public ToggleGroup tglGroup;

    public override void ShowMe()
    {
        base.ShowMe();

        characters = Models.User.Instance.info.Player.Characters;

        mCreateCharBtn = GetComponent<Button>("CreateCharBtn");
        mEnterGameBtn = GetComponent<Button>("EnterGameBtn");
        
        mCreateCharBtn.onClick.AddListener(OnCreateCharBtnClick);
        mEnterGameBtn.onClick.AddListener(OnEnterGameBtnClick);

        CreateBtns();
    }



    private void CreateBtns()
    {
        for (int i = 0; i < charList.Count; i++)
            Destroy(charList[i].gameObject);

        charList.Clear();

        GameObject character;
        CharTgl charTgl;
        for (int i = 0; i < characters.Count; i++)
        {
            character = ResManager.Instance.Load<GameObject>(ResManager.ResourceType.Panel, "CharTgl");

            character.transform.parent = content;
            character.transform.localScale = Vector3.one;
            charTgl = character.GetComponent<CharTgl>();
            charList.Add(charTgl);
            charTgl.Init(characters[i], tglGroup, characters[i].Id);
        }
    }

    #region Methods that will be automatically called when buttons clicked 

    public void OnCreateCharBtnClick()
    {
        UIManager.Instance.HidePanel(typeof(CharSelectionPanel));
        UIManager.Instance.ShowPanel<CharCreationPanel>(typeof(CharCreationPanel));
    }

    public void OnEnterGameBtnClick()
    {
        foreach (var character in User.Instance.info.Player.Characters)
        {
            if (character.Id == CharacterSelectionManager.Instance.currentID)
            {
                Models.User.Instance.currentCharacter = character;
                break;
            }
        }

        Services.UserService.Instance.SendGameEnter(CharacterSelectionManager.Instance.currentID);

        EventCenter.Instance.AddEventListener("loading finish", OnLoadingFinish);
    }

    public void OnLoadingFinish()
    {
        UIManager.Instance.HidePanel(typeof(BkPanel));
        UIManager.Instance.HidePanel(typeof(CharSelectionPanel));
        EventCenter.Instance.RemoveEventListener("loading finish", OnLoadingFinish);
    }

    #endregion
}
