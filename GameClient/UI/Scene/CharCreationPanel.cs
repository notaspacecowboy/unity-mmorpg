//=============================
//Author: Zack Yang 
//Created Date: 10/17/2020 23:52
//=============================
using System.Collections;
using System.Collections.Generic;
using Services;
using SkillBridge.Message;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class CharCreationPanel : BasePanel
{
    private Toggle       mWarriorTgl;
    private Toggle       mWizardTgl;
    private Toggle       mArcherTgl;
    private Button       mConfirmBtn;
    private InputField   mNameInputField;
    private Text         mClassTxt;
    private Text         mDescriptionTxt;
    private Text         mNameRequirementTxt;

    public EasyTween[] animations;

    #region Methods of initialization/disposal

    public override void ShowMe()
    {
        Debug.LogFormat("CharCreationPanel -> ShowMe");

        base.ShowMe();

        #region find all ui components

        mWarriorTgl = GetComponent<Toggle>("WarriorTgl");
        mWizardTgl = GetComponent<Toggle>("WizardTgl");
        mArcherTgl = GetComponent<Toggle>("ArcherTgl");
        mConfirmBtn = GetComponent<Button>("ConfirmBtn");
        mNameInputField = GetComponent<InputField>("NameInputField");
        mClassTxt = GetComponent<Text>("ClassTxt");
        mDescriptionTxt = GetComponent<Text>("DescriptionTxt");
        mNameRequirementTxt = GetComponent<Text>("NameRequirementTxt");

        mWarriorTgl.isOn = true;
        mWarriorTgl.Select();
        EventCenter.Instance.EventTrigger<CharacterClass>("ChangeChar", CharacterClass.Warrior);
        #endregion

        #region playing animations
        for (int i = 0; i < animations.Length; i++)
        {
                animations[i].OpenCloseObjectAnimation();
        }
        #endregion

        #region when class btn pressed, publish an event trigger
        mWarriorTgl.onValueChanged.AddListener((flag) =>
        {
            if (flag)
            {
                EventCenter.Instance.EventTrigger<CharacterClass>("ChangeChar", CharacterClass.Warrior);
                animations[1].OpenCloseObjectAnimation();
            }
        });

        mWizardTgl.onValueChanged.AddListener((flag) =>
        {
            if (flag)
            {
                EventCenter.Instance.EventTrigger<CharacterClass>("ChangeChar", CharacterClass.Wizard);
                animations[1].OpenCloseObjectAnimation();
            }
        });

        mArcherTgl.onValueChanged.AddListener((flag) =>
        {
            if (flag)
            {
                EventCenter.Instance.EventTrigger<CharacterClass>("ChangeChar", CharacterClass.Archer);
                animations[1].OpenCloseObjectAnimation();
            }
        });
        #endregion

        #region when create character btn pressed, first check charname availability, then send a message to server

        mConfirmBtn.onClick.AddListener(OnCreateCharBtnPressed);

        #endregion

        #region listen to server response of character creation

        UserService.Instance.OnCreateCharacter += OnCreateCharacter;

        #endregion
    }

    public override void HideMe()
    {
        base.HideMe();
        UserService.Instance.OnCreateCharacter -= OnCreateCharacter;
    }

    #endregion

    #region methods of btn press

    /// <summary>
    /// this method is automatically called when play press btn for creating a new char
    /// </summary>
    private void OnCreateCharBtnPressed()
    {
        if (!CheckCharName())
        {
            mNameRequirementTxt.GetComponent<EasyTween>().OpenCloseObjectAnimation();
            return;
        }

        UserService.Instance.SendCreateChar(mNameInputField.text, CharacterSelectionManager.Instance.currentClass);
    }

    private bool CheckCharName()
    {
        if(mNameInputField.text != null)
            return true;

        return false;
    }

    #endregion

    #region Methods of animations

    /// <summary>
    /// when class description animation finished, do animation again and replace texts
    /// </summary>
    public void OnContainerExit()
    {
        mClassTxt.text = DataManager.Instance.Characters[(int) CharacterSelectionManager.Instance.currentClass].Name;
        mDescriptionTxt.text = "  " + DataManager.Instance.Characters[(int)CharacterSelectionManager.Instance.currentClass].Description;
        animations[1].OpenCloseObjectAnimation();
    }

    #endregion

    #region Methods of reaction to server response of char creation

    void OnCreateCharacter(Result result, string errorMsg)
    {
        if (result == Result.Failed)
        {
            return;
        }

        UIManager.Instance.HidePanel(typeof(CharCreationPanel));

        UIManager.Instance.ShowPanel<CharSelectionPanel>(typeof(CharSelectionPanel));
    }

    #endregion
}
