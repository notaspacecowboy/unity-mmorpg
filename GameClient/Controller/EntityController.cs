//=============================
//Author: Zack Yang 
//Created Date: 11/01/2020 0:49
//=============================
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Managers;
using SkillBridge.Message;
using UnityEngine;

/// <summary>
/// EntityController should be attached to each entity to control it
/// </summary>
public class EntityController : MonoBehaviour, IEntityNotifier
{
    /// <summary>
    /// rigidbody component associate with this entity
    /// </summary>
    private Rigidbody mRigidbody;

    /// <summary>
    /// animator component associated with this entity
    /// </summary>
    private Animator mAnimator;

    ///// <summary>
    ///// current animator state of this entity 
    ///// </summary>
    //private AnimatorStateInfo mStateInfo;

    /// <summary>
    /// the entity to be controlled by this controller
    /// </summary>
    public Entity entity;

    private Vector3 mLastPosition;

    /// <summary>
    /// current position of this entity
    /// </summary>
    private Vector3 mPosition;

    /// <summary>
    /// current direction of this entity
    /// </summary>
    private Vector3 mDirection;


    /// <summary>
    /// current rotation of this entity
    /// </summary>
    private Quaternion mRotation;

    /// <summary>
    /// rotation of this entity after last update
    /// </summary>
    private Quaternion mLastRotation;

    /// <summary>
    /// current speed of this entity
    /// </summary>
    private int mSpeed;

    public bool isPlayer = false;

    public void Start()
    {
        mRigidbody = GetComponent<Rigidbody>();
        mAnimator = GetComponentInChildren<Animator>();
        //if controller is assigned with a entity, update its fields
        if (entity != null)
        {
            UpdateTransform();
        }

        //if the entity is not player, do not let gravity affect this body
        //hence the entity is fully controlled by game logic
        if (!isPlayer)
        {
            mRigidbody.useGravity = false;
        }
    }

    /// <summary>
    /// update controller's fields
    /// </summary>
    private void UpdateTransform()
    {
        //update transform to be the same with what stored in entity
        mPosition = GameObjectTool.LogicToWorld(entity.position);
        mDirection = GameObjectTool.LogicToWorld(entity.direction);
        mSpeed = entity.speed;

        mRigidbody.MovePosition(mPosition);
        this.transform.forward = mDirection;

        mLastPosition = mPosition;
        mLastPosition = mDirection;

    }

    void OnDestroy()
    {
        if (entity != null)
            Debug.LogFormat("{0} OnDestroy :ID:{1} POS:{2} DIR:{3} SPD:{4} ", this.name, entity.entityID, entity.position, entity.direction, entity.speed);

        if (WorldUIManager.Instance != null)
        {
            WorldUIManager.Instance.RemoveNameBar(this.transform);
        }
    }


    public void FixedUpdate()
    {
        if (entity == null)
            return;

        //update the attached entity 
        entity.OnUpdate(Time.fixedDeltaTime);

        //if this entity is not controlled by player, then the character controller
        //will not update gameobject transform for this entity, for now it will just
        //stay at where it is
        if (!isPlayer)
        {
            this.UpdateTransform();
        }
    }

    public void OnEntityEvent(EntityEvent ev)
    {
        switch (ev)
        {
            case EntityEvent.Idle:
                mAnimator.SetBool("Move", false);
                mAnimator.SetTrigger("Idle");
                break;

            case EntityEvent.MoveFwd:
                mAnimator.SetBool("Move", true);
                break;

            case EntityEvent.MoveBack:
                mAnimator.SetBool("Move", true);
                break;

            case EntityEvent.Jump:
                mAnimator.SetBool("Jump", true);
                break;
        }


    }

    public void OnEntityRemove()
    {
    }

    public void OnEntityChange(Entity data)
    {
        entity = data;
    }
}
