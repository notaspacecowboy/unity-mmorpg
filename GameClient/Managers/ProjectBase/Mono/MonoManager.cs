using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.Events;

public class MonoManager : Singleton<MonoManager>
{
    // Start is called before the first frame update
    public MonoManager()
    {
        GameObject obj = new GameObject("MonoController");
        mMonoController = obj.AddComponent<MonoController>();
    }

    public void AddUpdateListener(UnityAction action)
    {
        mMonoController.AddUpdateListener(action);
    }

    public void RemoveUpdateListener(UnityAction action)
    {
        mMonoController.RemoveUpdateListener(action);
    }

    public Coroutine StartCoroutine(string methodName)
    {
        return mMonoController.StartCoroutine(methodName);
    }

    public Coroutine StartCoroutine(string methodName, [DefaultValue("null")] object value)
    {
        return mMonoController.StartCoroutine(methodName, value);
    }

    public Coroutine StartCoroutine(IEnumerator routine)
    {
        return mMonoController.StartCoroutine(routine);
    }

    public void StopCoroutine(IEnumerator routine)
    {
        mMonoController.StopCoroutine(routine);
    }
    public void StopCoroutine(Coroutine routine)
    {
        mMonoController.StopCoroutine(routine);
    }
    private MonoController mMonoController;
}
