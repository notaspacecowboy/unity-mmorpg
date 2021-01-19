//=============================
//作者: 杨政 
//时间: 09/20/2020 19:54:04
//=============================
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class JoystickPanel : BasePanel
{
    //Enums
    #region enum of joystick type
    public enum Type
    {
        Fixed,
        Moving,
        Hiden
    }
    #endregion

    //Fields
    #region Fields of joystick panel's UI components

    private Image touchRange;
    private Image joystickBk;
    private Image joystickCtrl;

    #endregion

    #region Fields of joystick's general settings
    [Header("Maximum Distance"),
     Tooltip("The maximum distance allowed between joystickctrl and joystickBk")]
    public float maxL = 90f;

    [Header("Display Method"),
     Tooltip("The display method of the simulating joystick")]
    public Type joystickControlType = Type.Fixed;
    #endregion

    //Methods
    #region Methods to initialize/finalize panel when called to show or hide
    // Start is called before the first frame update
    public override void ShowMe()
    {
        //the event listener of mouse click, on dragging and end dragging.
        touchRange = GetComponent<Image>("joystickTouchRect");
        //the father obj of the joystick image
        joystickBk = GetComponent<Image>("joystickBk");
        //the image of the joystick
        joystickCtrl = GetComponent<Image>("joystickCtrl");

        //start listen to the event of mouse click
        UIManager.AddCustomEventListener(touchRange, EventTriggerType.PointerDown, OnPointerDown);

        //start listen to the event of mouse dragging
        UIManager.AddCustomEventListener(touchRange, EventTriggerType.Drag, OnDrag);

        //start listen to the event of mouse pointer up
        UIManager.AddCustomEventListener(touchRange, EventTriggerType.PointerUp, OnPointerUp);

        if (joystickControlType == Type.Hiden)
            joystickBk.gameObject.SetActive(false);
    }
    #endregion

    #region Methods to call when player click, drag, unclick
    /// <summary>
    /// this method is performed when the touchRange image detect an event of mouse click
    /// </summary>
    /// <param name="info"></param>
    private void OnPointerDown(BaseEventData info)
    {
        if (joystickControlType == Type.Hiden)
        {
            Vector2 localPos;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(touchRange.rectTransform,
                (info as PointerEventData).position,
                (info as PointerEventData).pressEventCamera, out localPos);

            joystickBk.transform.localPosition = localPos;
            joystickBk.gameObject.SetActive(true);
        }
    }

    /// <summary>
    /// this method is performed when the touchRange image detect an event of mouse click
    /// </summary>
    /// <param name="info"></param>
    private void OnPointerUp(BaseEventData info)
    {
        if (joystickControlType == Type.Hiden)
            joystickBk.gameObject.SetActive(false);

        joystickCtrl.transform.localPosition = Vector3.zero;
        //event trigger of a joystick movement stop
        EventCenter.Instance.EventTrigger<Vector2>("JoystickMove", Vector2.zero);
    }

    /// <summary>
    /// this method is performed when the touchRange image detect an event of mouse click
    /// </summary>
    /// <param name="info"></param>
    private void OnDrag(BaseEventData info)
    {
        Vector2 localPos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(joystickBk.rectTransform,
            (info as PointerEventData).position,
            (info as PointerEventData).pressEventCamera, out localPos);

        if (localPos.magnitude > maxL)
        {
            if (joystickControlType == Type.Moving)
                joystickBk.transform.position += (Vector3) (localPos.normalized * (localPos.magnitude- maxL));
            
            localPos = localPos.normalized * maxL;
        }

        joystickCtrl.transform.localPosition = localPos;

        //event trigger of a joystick movement
        EventCenter.Instance.EventTrigger<Vector2>("JoystickMove", localPos.normalized);
    }
    #endregion
}
