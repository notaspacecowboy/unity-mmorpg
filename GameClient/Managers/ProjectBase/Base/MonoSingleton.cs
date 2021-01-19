//=============================
//Author: Zack Yang 
//Created Date: 10/11/2020 0:41
//=============================
using UnityEngine;


public abstract class MonoSingleton<T> : MonoBehaviour where T : MonoBehaviour
{
    public bool global = true;
    static T instance;
    public static T Instance
    {
        get
        {
            if (instance == null)
            {
                instance =(T)FindObjectOfType<T>();
            }
            return instance;
        }

    }

    void Start()
    {
        if (global)
        {
            //if there is already a monosingleton T in the game, destroy this one
            if (instance != null && instance != this.gameObject.GetComponent<T>())
            {
                Destroy(this.gameObject);
                return;
            }

            instance = this.gameObject.GetComponent<T>();
            DontDestroyOnLoad(this.gameObject);
        }

       
        this.OnStart();
    }

    protected virtual void OnStart()
    {

    }
}