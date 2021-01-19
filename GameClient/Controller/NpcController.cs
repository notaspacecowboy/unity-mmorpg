//=============================
//Author: Zack Yang 
//Created Date: 11/22/2020 22:54
//=============================
//=============================
//Author: Zack Yang 
//Created Date: 11/22/2020 22:19
//=============================
using System;
using System.Collections;
using System.Collections.Generic;
using Common.Data;
using Models;
using UnityEngine;

public class NpcController : MonoBehaviour
{
    //fields
    /// <summary>
    /// id of current npc
    /// </summary>
    public int NpcID;

    /// <summary>
    /// npc info
    /// </summary>
    private NpcDefine mNpcDefine;

    /// <summary>
    /// animator component of current npc
    /// </summary>
    private Animator mAnimator;

    /// <summary>
    /// skinned mesh renderer component of current npc
    /// </summary>
    private BoxCollider mTrigger;

    private Vector3 mForward;

    private bool isInteractive = false;

    public void Start()
    {
        //get necessary components
        mAnimator = GetComponent<Animator>();
        mTrigger = GetComponent<BoxCollider>();

        //get npcdefine
        mNpcDefine = DataManager.Instance.Npcs[NpcID];

        //get current facing direction
        mForward = new Vector3(this.transform.forward.x, this.transform.forward.y, this.transform.forward.z);

        //let the npc do some random actions
        StartCoroutine(RandomAction());
    }


    private IEnumerator RandomAction()
    {
        while (true)
        {
            //if npc is currently talking to player, stop random action
            if(isInteractive)
                yield return new WaitForSeconds(2f);
            else
            {
                yield return new WaitForSeconds(UnityEngine.Random.Range(5f, 10f));
            }
            Relax();
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        //有空做个ui, 玩家靠近他就生成worldcanvas ui提示玩家按e
    }

    public void OnTriggerLeave(Collider other)
    {
        //消除所生成的ui
    }

    public void OnTriggerStay(Collider other)
    {
        if (Input.GetKeyUp(KeyCode.E))
        {
            Interactive();
        }
    }

    #region start interaction methods

    private void Interactive()
    {
        if (!isInteractive)
        {
            isInteractive = true;
            StartCoroutine(DoInteractive());
            EventCenter.Instance.AddEventListener<NpcDefine>("StopInteractive", OnStopInteractive);
        }
    }

    private IEnumerator DoInteractive()
    {
        yield return FaceToPlayer();
        if (NpcManager.Instance.EventTrigger(mNpcDefine))
        {
            Talk();
        }
        else
        {
            Debug.LogFormat("you are trying to talk to npc {0}", mNpcDefine.Name);
        }
    }

    private IEnumerator FaceToPlayer()
    {
        Vector3 faceTo = (User.Instance.currentPlayerObject.transform.position - this.transform.position).normalized;
        while (Vector3.Angle(this.transform.forward, faceTo) > 5f)
        {
            this.transform.forward = Vector3.Lerp(this.transform.forward, faceTo, Time.deltaTime * 5f);
            yield return null;
        }
    }

    #endregion

    #region stop interaction methods

    private void OnStopInteractive(NpcDefine define)
    {
        Debug.Log("OnStopInteractive");
        if (define.ID == mNpcDefine.ID)
        {
            StartCoroutine(StopInteractive());
        }
    }

    private IEnumerator StopInteractive()
    {
        yield return FaceToFront();
        mAnimator.SetTrigger("Idle");
        isInteractive = false;

        EventCenter.Instance.RemoveEventListener<NpcDefine>("StopInteractive", OnStopInteractive);
    }

    private IEnumerator FaceToFront()
    {
        while (Vector3.Angle(mForward, this.transform.forward) > 5f)
        {
            this.transform.forward = Vector3.Lerp(this.transform.forward, mForward, Time.deltaTime * 5f);
            yield return null;
        }
    }

    #endregion

    #region utilities

    private void Relax()
    {
        mAnimator.SetTrigger("Relax");
    }

    private void Talk()
    {
        mAnimator.SetTrigger("Talk");
    }

    #endregion
}
