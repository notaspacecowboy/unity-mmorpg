//=============================
//Author: Zack Yang 
//Created Date: 10/12/2020 22:45
//=============================
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class Singleton<T> where T: new()
{
    private static T instance;

    public static T Instance
    {
        get
        {
            if(instance == null)
                instance = new T();

            return instance;
        }
    }
}
