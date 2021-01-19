//=============================
//Author: Zack Yang 
//Created Date: 10/30/2020 19:49
//=============================

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Managers;
using Models;
using SkillBridge.Message;
using UnityEngine;
using UnityEngine.Events;


/// <summary>
/// this class manages all characters in the same map with user controlled character
/// </summary>
public class CharacterManager : Singleton<CharacterManager>
{
    /// <summary>
    /// container of all characters in the game, access with its ID
    /// </summary>
    public Dictionary<int, Character> characters = new Dictionary<int, Character>();

    /// <summary>
    /// events to be called when add a new character
    /// </summary>
    public UnityAction<Character> OnCharacterEnter;

    /// <summary>
    /// events to be called when remove a character
    /// </summary>
    public UnityAction<int> OnCharacterLeave;
    public void Clear()
    {
        int[] keys = characters.Keys.ToArray();

        foreach (var id in keys)
        {
            this.RemoveCharacter(id);
        }


        characters.Clear();
    }

    public void Update()
    {
    }

    public void AddCharacter(NCharacterInfo info)
    {
        Debug.LogFormat("character {0} is added into map {1}", info.Name, info.mapId);
        Character cha = new Character(info);
        characters.Add(info.Entity.Id, cha);
        EntityManager.Instance.AddEntity(cha);



        if (User.Instance.currentCharacter.Entity != null && User.Instance.currentCharacter.Entity.Id == info.Entity.Id)
        {
            cha.IsCurrentPlayer = true;
        }
        else
        {
            cha.IsCurrentPlayer = false;
        }

        if (OnCharacterEnter != null)
            OnCharacterEnter(cha);
    }

    public void RemoveCharacter(int id)
    {
        Debug.LogFormat("character {0} is removed from map {1}", characters[id].Name, characters[id].info.mapId);
        characters.Remove(id);
        EntityManager.Instance.RemoveEntity(id);

        if (OnCharacterLeave != null)
            OnCharacterLeave(id);
    }
}
