 using System.Collections;
using System.Collections.Generic;
 using System.Linq;
 using UnityEngine;
 using UnityEngine.EventSystems;
 using UnityEngine.UI;

 /// <summary>
 /// 面板基类
 /// 1.找到自己的所有的子控件=>GetComponentsInChildren<>方法
 /// 2.他应该提供显示和隐藏的功能
 /// </summary>
public class BasePanel : MonoBehaviour
{
    /// <summary>
    /// 将该面板的所有子控件按其名字存到一个dictionary中
    /// </summary>
    protected virtual void Awake()
    {
        FindChildControl<Button>();
        FindChildControl<Text>();
        FindChildControl<Toggle>();
        FindChildControl<Image>();
        FindChildControl<ScrollRect>();
        FindChildControl<Slider>();
        FindChildControl<InputField>();
    }

    /// <summary>
    /// 得到名为componentName的子对象的T类型的组件
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="componentName"></param>
    /// <returns></returns>
    protected T GetComponent<T>(string componentName) where T:UIBehaviour
    {
        if (componentDic.ContainsKey(componentName))
        {
            for (int i = 0; i < componentDic[componentName].Count; i++)
            {
                if (componentDic[componentName][i] is T)
                    return componentDic[componentName][i] as T;
            }
        }

        return null;
    }

   
    
    /// <summary>
    /// 找到每个子对象的所有对应控件
    /// </summary>
    /// <typeparam name="T">控件类型</typeparam>
    private void FindChildControl<T>() where T:UIBehaviour
    {
        T[] tList = this.GetComponentsInChildren<T>();
        for(int i=0;i<tList.Length;i++)
        {
            string objName = tList[i].gameObject.name;
            if (componentDic.ContainsKey(objName))
                componentDic[objName].Add(tList[i]);
            else
                componentDic.Add(objName, new List<UIBehaviour>{tList[i]});

            if (tList[i] is Button)
            {
                (tList[i] as Button).onClick.AddListener(() =>
                {
                    OnClick(objName);
                });
            }

            else if (tList[i] is Toggle)
            {
                (tList[i] as Toggle).onValueChanged.AddListener((value) =>
                {
                    OnValueChange(objName, value);
                });
            }
        }
    }

   
    
    /// <summary>
    /// 执行点击button后执行的逻辑
    /// </summary>
    /// <param name="btnName">button名</param>
    protected virtual void OnClick(string btnName)
    {
    }



    /// <summary>
    /// 执行toggle状态改变后执行的逻辑
    /// </summary>
    /// <param name="btnName">toggle名</param>
    protected virtual void OnValueChange(string toggleName, bool value)
    {
    }

    /// <summary>
    /// 用于显示当前面板的虚函数,在子类中具体实现
    /// </summary>
    public virtual void ShowMe()
    {
    }

    /// <summary>
    /// 用于隐藏当前面板的虚函数,在子类中具体实现
    /// </summary>
    public virtual void HideMe()
    {
    }


    //key:拥有此控件的游戏对象名
    //value:此游戏对象下所有此类型控件的list
    private Dictionary<string, List<UIBehaviour>> componentDic = new Dictionary<string, List<UIBehaviour>>();
}
