//=============================
//Author: Zack Yang 
//Created Date: 11/03/2020 23:13
//=============================
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NameBar : WorldUIElement
{
    private Text mNameTxt;
    private Character mCharacter;

    // Start is called before the first frame update
    public void Init(Character character, Transform owner)
    {
        base.Owner = owner;
        
        mNameTxt = GetComponent<Text>("NameTxt");
        if (character != null)
        {
            mCharacter = character;
            mNameTxt.text = mCharacter.Name;
        }
    }

    // Update is called once per frame
    void Update()
    {
        UpdateInfo();
    }

    private void UpdateInfo()
    {
        if (mNameTxt.text != mCharacter.Name)
        {
            mNameTxt.text = mCharacter.Name;
        }
    }
}
