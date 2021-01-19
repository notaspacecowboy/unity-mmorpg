//=============================
//Author: Zack Yang 
//Created Date: 11/02/2020 22:13
//=============================
using System.Collections;
using System.Collections.Generic;
using Models;
using UnityEngine;

public class MainPlayerCameraController : MonoSingleton<MainPlayerCameraController>
{
    public GameObject player;

    private void LateUpdate()
    {
        if (player == null)
        {
            player = User.Instance.currentPlayerObject;
        }

        if (player == null)
            return;

        this.transform.position = player.transform.position;
        this.transform.rotation = player.transform.rotation;
    }
}
