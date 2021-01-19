//=============================
//Author: Zack Yang 
//Created Date: 11/01/2020 22:36
//=============================

using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Managers;
using Models;
using UnityEngine;
using UnityEngine.TextCore.LowLevel;

public class GameObjectManager : MonoSingleton<GameObjectManager>
{
    private Dictionary<int, GameObject> characters = new Dictionary<int, GameObject>();

    protected override void OnStart()
    {
        StartCoroutine(InitGameObject());
        CharacterManager.Instance.OnCharacterEnter += CreateCharacter;
        CharacterManager.Instance.OnCharacterLeave += RemoveCharacter;
    }


    public void OnDestroy()
    {
        CharacterManager.Instance.OnCharacterEnter -= CreateCharacter;
        CharacterManager.Instance.OnCharacterLeave -= RemoveCharacter;
    }

    private IEnumerator InitGameObject()
    {
        Debug.LogFormat("GameObjectManager->InitGameObject()");
        foreach (var character in CharacterManager.Instance.characters.Values)
        {
            CreateCharacter(character);
            yield return null;
        }
    }

    private void CreateCharacter(Character cha)
    {
        Debug.LogFormat("GameObjectManager->CreateCharacter()");
        if (!characters.ContainsKey(cha.entityID) || characters[cha.entityID] == null)
        {
            GameObject obj = ResManager.Instance.Load<GameObject>(cha.define.Resource);

            //if lack resource, print error msg
            if (obj == null)
            {
                Debug.LogErrorFormat("Character {0} cannot be found", cha.Name);
            }

            characters[cha.entityID] = obj;

            WorldUIManager.Instance.AddNameBar(obj.transform, cha);
        }


        InitCharacter(characters[cha.entityID], cha);
        //UIWorldElementManager.Instance.AddCharacterNameBar(go.transform, character);

    }


    private void InitCharacter(GameObject obj, Character cha)
    {
        Debug.LogFormat("GameObjectManager->InitCharacter()");
        obj.name = cha.Name;
        obj.transform.position = GameObjectTool.LogicToWorld(cha.position);
        obj.transform.forward = GameObjectTool.LogicToWorld(cha.direction);

        EntityController entityController = obj.GetComponent<EntityController>();

        if (entityController != null)
        {
            entityController.isPlayer = cha.IsCurrentPlayer;
            entityController.entity = cha;
            EntityManager.Instance.RegisterEntityNotifier(cha.entityID, entityController);
        }

        PlayerInputController playerController = obj.GetComponent<PlayerInputController>();
        if (entityController.isPlayer)
        {
            if (playerController != null)
            {
                playerController.character = cha;
            }

            //if (mainPlayerCamera != null)
            //{
            //    mainPlayerCamera.transform.parent = obj.transform;
            //    mainPlayerCamera.transform.localPosition = Vector3.zero;
            //    mainPlayerCamera.transform.localRotation = Quaternion.identity;
            //    mainPlayerCamera.transform.localScale = Vector3.one;
            //}

            Models.User.Instance.currentPlayerObject = obj;
        }
        else
        {
            playerController.enabled = false;
        }
    }

    private void RemoveCharacter(int id)
    {
        Debug.LogFormat("GameObjectManager->RemoveCharacter()");
        if (this.characters[id] == null)
            return;

        GameObject obj = this.characters[id];
        WorldUIManager.Instance.RemoveNameBar(obj.transform);
        
        Destroy(this.characters[id]);
        this.characters.Remove(id);
    }
}
