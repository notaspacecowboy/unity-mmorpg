//=============================
//作者: 杨政 
//时间: 09/16/2020 19:06:30
//=============================
using System.Collections;
using System.Collections.Generic;
using Services;
using UnityEngine;

public class main : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Application.targetFrameRate = 30;

        log4net.Config.XmlConfigurator.ConfigureAndWatch(new System.IO.FileInfo("log4net.xml"));
        UnityLogger.Init();
        Common.Log.Init("Unity");
        Common.Log.Info("Game start");

        StartCoroutine(DataManager.Instance.LoadData());

        //Init basic services
        MapService.Instance.Init();
        UserService.Instance.Init();
        ItemService.Instance.Init();

        UIManager.Instance.ShowPanel<BkPanel>(typeof(BkPanel),  (panel) =>
        {
            UIManager.Instance.GetPanel<BkPanel>(typeof(BkPanel)).ChangeBk("startBk");
            UIManager.Instance.ShowPanel<StartPanel>(typeof(StartPanel));
        });
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
