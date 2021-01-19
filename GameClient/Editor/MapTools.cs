//=============================
//Author: Zack Yang 
//Created Date: 11/17/2020 0:46
//=============================

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Common.Data;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MapTools 
{
    [MenuItem("MapTools/Export Teleporters")]
    public static void ExportTeleporters()
    {
        DataManager.Instance.Load();

        Scene current = EditorSceneManager.GetActiveScene();
        string currentScene = current.name;
        if (current.isDirty)
        {
            EditorUtility.DisplayDialog("Tip", "Please save the current scene", "OK");
            return;
        }

        List<Teleporter> allTeleporters = new List<Teleporter>();

        foreach (var map in DataManager.Instance.Maps)
        {
            string sceneFile = "Assets/Levels/" + map.Value.Resource + ".unity";
            if (!File.Exists(sceneFile))
            {
                Debug.LogErrorFormat("scene {0} does not exist!", sceneFile);
                continue;
            }

            EditorSceneManager.OpenScene(sceneFile, OpenSceneMode.Single);

            Teleporter[] teleporters = GameObject.FindObjectsOfType<Teleporter>();
            foreach (var teleporter in teleporters)
            {
                if (!DataManager.Instance.Teleporters.ContainsKey(teleporter.ID))
                {
                    EditorUtility.DisplayDialog("Error",
                        String.Format("in map {0}, teleporter {1} does not exist!", map.Value.Name, teleporter.ID), "OK");
                    return;
                }

                TeleporterDefine define = DataManager.Instance.Teleporters[teleporter.ID];
                if (define.ID != teleporter.ID)
                {
                    EditorUtility.DisplayDialog("Error",
                        String.Format("In map {0}, teleporter[{1}], mapID: {3} does not match", map.Value.Name,
                            teleporter.ID, define.ID), "OK");
                }

                define.Position = GameObjectTool.WorldToLogicN(teleporter.transform.position);
                define.Direction = GameObjectTool.WorldToLogicN(teleporter.transform.forward);
            }
        }

        DataManager.Instance.SaveTeleporters();
        EditorSceneManager.OpenScene("Assets/Levels/" + currentScene + ".unity");
        EditorUtility.DisplayDialog("Tip", "All teleporters are successfully exported!", "OK");
    }
}
