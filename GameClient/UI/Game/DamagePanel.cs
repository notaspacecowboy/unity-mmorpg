//=============================
//Author: Zack Yang 
//Created Date: 10/03/2020 22:38
//=============================
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class DamagePanel : BasePanel
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        { 
            if(Random.Range(0, 100) < 30)
                Create(Input.mousePosition, 100, true);
            else
                Create(Input.mousePosition, 100, false);
        }
    }

    public void Create(Vector3 position, int damage, bool isCriticalHit)
    {
        //ResManager.Instance.LoadAsync<GameObject>(ResManager.ResourceType.Effect, GlobalInit.DamagePopUp, (obj =>
        //{
        //    obj.transform.parent = gameObject.transform;
        //    obj.transform.position = position;
        //    obj.GetComponent<DamagePopUp>().Init(damage, isCriticalHit);
        //}));
    }
}
