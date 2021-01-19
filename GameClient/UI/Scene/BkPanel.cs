//=============================
//作者: 杨政 
//时间: 09/17/2020 21:46:47
//=============================
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

public class BkPanel : BasePanel
{
    public void ChangeBk(string path)
    {
        GetComponent<Image>("BkPic").sprite = ResManager.Instance.Load<Sprite>(ResManager.ResourceType.BkImg, path);
    }
}
