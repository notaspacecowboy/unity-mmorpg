//=============================
//Author: Zack Yang 
//Created Date: 10/11/2020 0:43
//=============================
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkTest : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Network.NetClient.Instance.Init("127.0.0.1", 7878);
        Network.NetClient.Instance.Connect();

        if(Network.NetClient.Instance.Connected)
            Debug.Log("yes");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
