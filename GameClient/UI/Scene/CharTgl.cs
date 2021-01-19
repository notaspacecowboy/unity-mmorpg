//=============================
//Author: Zack Yang 
//Created Date: 10/23/2020 0:28
//=============================
using System.Collections;
using System.Collections.Generic;
using System.IO.MemoryMappedFiles;
using SkillBridge.Message;
using UnityEngine;
using UnityEngine.UI;

public class CharTgl : BasePanel
{
    private NCharacterInfo mInfo;
    private Text   mCharNameTxt;
    private Text   mClassNameTxt;
    private Text   mLevelTxt;
    private Image  mCoverImg;
    private Image  mClassImg;
    private Toggle mPressTgl;
    private int    mID;

    public void Init(SkillBridge.Message.NCharacterInfo info, ToggleGroup tglGroup, int id)
    {
        //Debug.LogFormat("CharBtn->Init() charName: {0}, className: {1}, level: {2}", info.Name, info.Class, info.Level);

        mInfo = info;
        mID = id;
        mCharNameTxt = GetComponent<Text>("CharNameTxt");
        mClassNameTxt = GetComponent<Text>("ClassNameTxt");
        mLevelTxt = GetComponent<Text>("LevelTxt");
        mPressTgl = GetComponent<Toggle>("PressTgl");
        mCoverImg = GetComponent<Image>("CoverImg");
        mClassImg = GetComponent<Image>("ClassImg");

        mCoverImg.enabled = false;

        mCharNameTxt.text = info.Name;
        mLevelTxt.text = "LV. " + info.Level.ToString();
        switch (info.Class)
        {
            case CharacterClass.Warrior:
                mClassNameTxt.text = "Warrior";
                mClassImg.sprite = ResManager.Instance.Load<Sprite>(ResManager.ResourceType.BkImg, "z");
                break;

            case CharacterClass.Archer:
                mClassNameTxt.text = "Archer";
                mClassImg.sprite = ResManager.Instance.Load<Sprite>(ResManager.ResourceType.BkImg, "g");
                break;

            case CharacterClass.Wizard:
                mClassNameTxt.text = "Wizard";
                mClassImg.sprite = ResManager.Instance.Load<Sprite>(ResManager.ResourceType.BkImg, "y");
                break;

            default:
                break;
        }

        mPressTgl.group = tglGroup;
        mPressTgl.onValueChanged.AddListener(OnToggleValueChanged);
    }

    public void OnToggleValueChanged(bool flag)
    {
        if (flag)
        {
            mCoverImg.enabled = true;
            CharacterSelectionManager.Instance.currentID = mID;
            EventCenter.Instance.EventTrigger<CharacterClass>("ChangeChar", mInfo.Class);
            return;
        }

        mCoverImg.enabled = false;
    }
}
