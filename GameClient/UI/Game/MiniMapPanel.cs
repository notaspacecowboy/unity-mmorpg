//=============================
//Author: Zack Yang 
//Created Date: 11/05/2020 21:42
//=============================
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using Common.Data;
using UnityEngine;
using UnityEngine.U2D;
using UnityEngine.UI;

public class MiniMapPanel : BasePanel
{
    //fields

    #region fields of ui components

    /// <summary>
    /// minimap image
    /// </summary>
    private Image mMiniMapImg;
    /// <summary>
    /// name of current map
    /// </summary>
    private Text mMapNameTxt;
    /// <summary>
    /// arrow image that indicates current player position
    /// </summary>
    private Image mArrow;

    #endregion

    #region fields of minimap information

    /// <summary>
    /// bounding box which covers the entire map
    /// </summary>
    private Collider mBoundingBox;

    /// <summary>
    /// current character gameobject
    /// </summary>
    private GameObject mPlayerObj;

    #endregion


    //methods
    #region methods initialization and update

    public override void ShowMe()
    {
        base.ShowMe();

        //register itself on minimap manager
        MiniMapManager.Instance.MiniMap = this;

        mMiniMapImg = GetComponent<Image>("MiniMapImg");
        mMapNameTxt = GetComponent<Text>("MapNameTxt");
        mArrow = GetComponent<Image>("Arrow");
    }

    /// <summary>
    /// called by minimap manager, used to init minimap
    /// </summary>
    /// <param name="boundingBox"></param>
    public void Init(Collider boundingBox)
    {
        mBoundingBox = boundingBox;

        mMiniMapImg.sprite = MiniMapManager.Instance.GetMapSprite();

        mMapNameTxt.text = Models.User.Instance.currentMapInfo.Name;
        mMiniMapImg.SetNativeSize();
        mMiniMapImg.transform.localPosition = Vector3.zero;
    }

    // Update is called once per frame
    void Update()
    {
        if (mPlayerObj == null)
        {
            mPlayerObj = MiniMapManager.Instance.CurrentPlayer;
        }

        if (mMiniMapImg.sprite == null)
        {
            mMiniMapImg.sprite = MiniMapManager.Instance.GetMapSprite();
        }

        if (mBoundingBox != null && mPlayerObj != null)
        {
            float worldX = mBoundingBox.bounds.min.x;
            float worldY = mBoundingBox.bounds.min.z;

            float offsetX = (mPlayerObj.transform.position.x - worldX) / mBoundingBox.bounds.size.x;
            float offsetY = (mPlayerObj.transform.position.z - worldY) / mBoundingBox.bounds.size.z;

            mMiniMapImg.rectTransform.pivot = new Vector2(offsetX, offsetY);

            mArrow.transform.eulerAngles = new Vector3(0, 0, -mPlayerObj.transform.eulerAngles.y);

        }
    }

    #endregion

}
