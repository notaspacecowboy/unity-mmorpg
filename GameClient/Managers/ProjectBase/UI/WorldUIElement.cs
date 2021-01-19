//=============================
//Author: Zack Yang 
//Created Date: 12/28/2020 11:34
//=============================
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldUIElement : BasePanel
{
    public Transform Owner;

    public float Height = 5f;

    public void Update()
    {
        if (Owner != null)
        {
            transform.position = Owner.position + (Vector3.up * Height);
        }

        if (Camera.main != null)
        {
            transform.forward = Camera.main.transform.forward;
        }
    }
}
