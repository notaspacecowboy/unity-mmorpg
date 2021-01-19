using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 输入控制模块
/// 统一管理输入相关的逻辑
/// 暂时只针对pc段按键
/// 将游戏中输入相关的操作独立出来,降低程序耦合性
/// </summary>
public class InputManager : Singleton<InputManager>
{ 
    public InputManager()
    {
        MonoManager.Instance.AddUpdateListener(Update);
    }

    //选择是否开启玩家输入检测
    public void setStatus(bool status)
    {
        isOpen = status;
    }
    private void CheckKeyCode(KeyCode key)
    {
        //分发按键按下或抬起事件至事件中心
        if (Input.GetKeyDown(key))
        {
            EventCenter.Instance.EventTrigger<KeyCode>("Keydown", key);
        }

        if (Input.GetKey(key))
        {
            EventCenter.Instance.EventTrigger<KeyCode>("Keyhold", key);
        }

        if (Input.GetKeyUp(key))
        {
            EventCenter.Instance.EventTrigger<KeyCode>("Keyup", key);
        }
    }

    private void CheckAnyKeyDown()
    {
        if (Input.anyKeyDown)
            EventCenter.Instance.EventTrigger("AnyKeydown");
    }

    private void CheckMouseBtn()
    {
        if (Input.GetMouseButtonDown(0))
            leftMouseDown = true;

        if (Input.GetMouseButtonUp(0))
            leftMouseDown = false;
    }

    private void Update()
    {
        if (!isOpen)
            return;

        CheckKeyCode(KeyCode.W);
        CheckKeyCode(KeyCode.A);
        CheckKeyCode(KeyCode.S);
        CheckKeyCode(KeyCode.D);
        CheckKeyCode(KeyCode.LeftShift);
        CheckMouseBtn();
        CheckAnyKeyDown();
    }

    private bool isOpen = false;
    public bool leftMouseDown = false;
}
