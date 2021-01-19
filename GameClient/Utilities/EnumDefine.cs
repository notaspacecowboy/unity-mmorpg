//=============================
//Author: Zack Yang 
//Created Date: 10/01/2020 20:07
//=============================
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#region Role Type
/// <summary>
/// role type enum
/// </summary>
public enum RoleType
{
    NoneAssign = 0,
    //player
    MainPlayer = 1,
    //monster
    Monster = 2
}
#endregion

#region Role Animation State Name

public enum E_Role_Animation_Param
{
    param_toidle,
    param_towalk,
    param_torunning,
    param_tojump,
    param_tohit01,
    param_tohit02,
    param_tohit03,
    param_toko_big,
    param_todamage,
    param_towinpose
}

public enum E_Role_Animation_Name
{
    idle,
    walk,
    running, 
    jump,
    hit01,
    hit02,
    hit03,
    ko_big,
    damage,
    winpose
}
#endregion
