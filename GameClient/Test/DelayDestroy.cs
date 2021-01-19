//=============================
//作者: 杨政 
//时间: 09/10/2020 20:47:12
//=============================
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DelayDestroy : MonoBehaviour
{
    void Start()
    {
        Invoke("DelayDestroyIt", 3f);
    }

    void DelayDestroyIt()
    {
        PoolManager.Instance.PushObj(gameObject.name, this.gameObject);
    }
}
