//=============================
//Author: Zack Yang 
//Created Date: 10/30/2020 20:38
//=============================
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using SkillBridge.Message;
using UnityEngine;

public class Entity
{
    private NEntity mEntityData;

    public NEntity entityData
    {
        get
        {
            this.UpdateEntityData();
            return this.mEntityData;
        }

        set
        {
            this.mEntityData = value;
            this.SetUpEntity(value);
        }
    }

    public int entityID;
    public Vector3Int position;
    public Vector3Int direction;
    public int speed;


    public Entity(NEntity nEntity)
    {
        this.entityID = nEntity.Id;     //id does not change
        this.mEntityData = nEntity;      
        this.SetUpEntity(nEntity);
    }

    /// <summary>
    /// this method is called by entitycontroller to update its current position 
    /// </summary>
    /// <param name="delta"></param>
    public virtual void OnUpdate(float delta)
    {
        if (this.speed != 0)
        {
            Vector3 dir = this.direction;
            this.position += Vector3Int.RoundToInt((this.speed * delta * dir)/100f);
        }
    }

    /// <summary>
    /// this method set up the entity info
    /// </summary>
    /// <param name="entity"></param>
    private void SetUpEntity(NEntity entity)
    {
        this.position = this.position.FromNVector3(entity.Position);
        this.direction = this.direction.FromNVector3(entity.Direction);
        this.speed = entity.Speed;
    }

    private void UpdateEntityData()
    {
        mEntityData.Position.FromVector3Int(this.position);
        mEntityData.Direction.FromVector3Int(this.direction);
        mEntityData.Speed = this.speed;
    }
}
