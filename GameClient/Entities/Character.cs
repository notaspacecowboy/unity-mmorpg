//=============================
//Author: Zack Yang 
//Created Date: 10/30/2020 22:44
//=============================
using System.Collections;
using System.Collections.Generic;
using Common.Data;
using SkillBridge.Message;
using UnityEngine;

public class Character : Entity
{
    /// <summary>
    /// char info sent from server
    /// </summary>
    public NCharacterInfo info;

    /// <summary>
    /// char configuration info
    /// </summary>
    public CharacterDefine define;

    /// <summary>
    /// char's name
    /// </summary>
    public string Name
    {
        get
        {
            //if this character is a player, then return its own name
            if (this.info.Type == CharacterType.Player)
                return this.info.Name;

            //if it is not player, return its name based on data configuration file
            return define.Name;
        }
    }

    public bool IsCurrentPlayer
    {
        get;
        set;
    }

    /// <summary>
    /// true if character is a player, otherwise false 
    /// </summary>
    public bool IsPlayer
    {
        get
        {
            return this.info.Type == CharacterType.Player;
        }
    }

    /// <summary>
    /// constructor method
    /// </summary>
    /// <param name="charInfo"></param>
    public Character(NCharacterInfo charInfo): base(charInfo.Entity)
    {
        this.info = charInfo;
        this.define = DataManager.Instance.Characters[charInfo.Tid];
    }

    public void MoveForward()
    {
        this.speed = this.define.Speed;
    }

    public void MoveBackward()
    {
        this.speed = -this.define.Speed;
    }

    public void Stop()
    {
        this.speed = 0;
    }

    public void SetPosition(Vector3Int position)
    {
        this.position = position;
    }


    public void SetDirection(Vector3Int direction)
    {
        this.direction = direction;
    }
}
