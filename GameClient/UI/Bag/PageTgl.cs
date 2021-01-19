//=============================
//Author: Zack Yang 
//Created Date: 11/26/2020 20:02
//=============================
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PageTgl : BasePanel
{
    private Image mOnSelectImg;
    private Image mUnSelectedImg;
    private Text mLabel;
    private Image mHighlightImg;
    private Image mtoggleImg;

    private PageView mPageView;

    public Color color;
    public Toggle Tgl;

    // Start is called before the first frame update
    public void Init(PageView view)
    {
        mPageView = view;

        mOnSelectImg = GetComponent<Image>("OnSelectedImg");
        mUnSelectedImg = GetComponent<Image>("UnSelectedImg");
        mHighlightImg = GetComponent<Image>("HighlightImg");
        mtoggleImg = GetComponent<Image>("ToggleImg");

        Tgl = GetComponent<Toggle>();
        mLabel = GetComponent<Text>("Label");

        mHighlightImg.gameObject.SetActive(false);
        if (mLabel != null)
        {
            Tgl.graphic = mLabel;
        }

        Tgl.onValueChanged.AddListener(OnValueChange);
    }

    public void TurnOn()
    {
        Tgl.isOn = false;
        Tgl.isOn = true;
    }

    public void OnValueChange(bool selected)
    {
        if (selected)
        {
            if (mOnSelectImg != null)
            {
                mOnSelectImg.gameObject.SetActive(true);
            }

            if (mUnSelectedImg != null)
            {
                mUnSelectedImg.gameObject.SetActive(false);
            }

            mHighlightImg.gameObject.SetActive(true);

            mPageView.Show();
        }
        else
        {
            if (mOnSelectImg != null)
            {
                mOnSelectImg.gameObject.SetActive(false);
            }

            if (mUnSelectedImg != null)
            {
                mUnSelectedImg.gameObject.SetActive(true);
            }

            mHighlightImg.gameObject.SetActive(false);
            mPageView.Hide();
        }
    }
}
