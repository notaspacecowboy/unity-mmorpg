using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEditor;

public class ScriptCreateInit : UnityEditor.AssetModificationProcessor
{
    private static void OnWillCreateAsset(string path)
    {
        path = path.Replace(".meta", "");
        if (path.EndsWith(".cs"))
        {
            string addOn = "//=============================\n";
            addOn += "//Author: Zack Yang \n//Created Date: " + DateTime.Now.ToString("MM/dd/yyyy H:mm");
            addOn += "\n//=============================\n";
            string strContent = File.ReadAllText(path);
            strContent = addOn + strContent;
            File.WriteAllText(path, strContent);
            AssetDatabase.Refresh();
        }
    }
}
