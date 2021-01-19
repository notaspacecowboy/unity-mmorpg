//=============================
//Author: Zack Yang 
//Created Date: 11/05/2020 23:58
//=============================
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniMapManager : Singleton<MiniMapManager>
{
    #region fields 

    /// <summary>
    /// current minimap
    /// </summary>
    public MiniMapPanel MiniMap;

    /// <summary>
    /// current player gameobject
    /// </summary>
    public GameObject CurrentPlayer
    {
        get
        {
            return Models.User.Instance.currentPlayerObject;
        }
    }

    #endregion

    #region methods

    /// <summary>
    /// setup minimap
    /// </summary>
    /// <param name="boundingBox"></param>
    public void SetupMinimap(Collider boundingBox)
    {
        if (MiniMap == null)
        {
            UIManager.Instance.ShowPanel<MiniMapPanel>(typeof(MiniMapPanel), (panel) =>
            {
                panel.Init(boundingBox);
            });
        }
        else
        {
            MiniMap.Init(boundingBox);
        }
    }

    /// <summary>
    ///  get current minimap resource
    /// </summary>
    /// <returns></returns>
    public Sprite GetMapSprite()
    {
        if (Models.User.Instance.currentMapInfo == null)
            return null;

        return ResManager.Instance.Load<Sprite>(ResManager.ResourceType.MiniMap,
            Models.User.Instance.currentMapInfo.MiniMap);
    }


    /// <summary>
    /// clear current minimap
    /// </summary>
    public void Clear()
    {
        UIManager.Instance.HidePanel(typeof(MiniMapPanel));
        MiniMap = null;
    }


    #endregion
}
