//=============================
//Author: Zack Yang 
//Created Date: 10/03/2020 22:18
//=============================

using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DamagePopUp : MonoBehaviour
{
    public EasyTween criticalAnimation;
    public EasyTween normalAnimation;
    public EasyTween movementAnimation;

    private TextMeshProUGUI mDmgTxt;
    private Color mDmgColor;

    [SerializeField] private Vector3 moveVector;

    [SerializeField] private float disappearSpeed = 1f;
    [SerializeField] private float disappearTime = 2f;

    private float currentTime;

    public void Init(int damage, bool isCriticalHit)
    {
        moveVector = new Vector3(1,1, 0);
        moveVector *= 30f;
        currentTime = disappearTime;
        mDmgTxt = GetComponent<TextMeshProUGUI>();

        mDmgTxt.overrideColorTags = true;

        mDmgTxt.color = new Color(1f, 0.94f, 0, 1f);
        mDmgTxt.transform.localScale = Vector3.zero;

        mDmgColor = mDmgTxt.color;
        mDmgColor.a = 1;
        mDmgTxt.color = mDmgColor;

        if (isCriticalHit)
        {
            mDmgTxt.color = new Color(1f, 0, 0, 1f);
            mDmgTxt.fontSize += 10;
            criticalAnimation.OpenCloseObjectAnimation();
        }

        else
            normalAnimation.OpenCloseObjectAnimation();


        mDmgColor = mDmgTxt.color;
        mDmgTxt.SetText(damage.ToString());
        movementAnimation.animationParts.PositionPropetiesAnim.StartPos = transform.localPosition;
        movementAnimation.animationParts.PositionPropetiesAnim.EndPos = new Vector3(transform.localPosition.x + 100f, transform.localPosition.y + 100f, 0);
        movementAnimation.OpenCloseObjectAnimation();
    }

    void Update()
    {
        transform.position += (moveVector * Time.deltaTime);
        moveVector -= (moveVector * 8f * Time.deltaTime);

        currentTime -= Time.deltaTime;
        if (currentTime < 0f)
        {
            mDmgColor.a -= disappearSpeed * Time.deltaTime;
            mDmgTxt.color = mDmgColor;
            if (mDmgColor.a < 0f)
                Destroy(gameObject);

        }
    }
}
