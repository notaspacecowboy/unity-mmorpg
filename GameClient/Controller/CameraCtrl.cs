//=============================
//作者: 杨政 
//时间: 09/11/2020 22:10:08
//=============================
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraCtrl : MonoBehaviour
{
    //Fields
    #region Field of camera control components 
    /// <summary>
    /// singleton
    /// </summary>
    public static CameraCtrl instance;

    /// <summary>
    /// control camera zoom in and out
    /// </summary>
    [Header("camera zoom controller")]
    public Transform mCameraZoomContainer;
    [SerializeField] private float zoomSpeed = 10f;

    /// <summary>
    /// camera container
    /// </summary>
    [Header("camera container")]
    public Transform mCameraContainer;
    #endregion

    #region Fields of camera rotation parameters
    /// <summary>
    /// the sensitivity of camera rotation in x axis
    /// </summary>
    [SerializeField]private float sensitivityX = 2f;

    #region Fields of camera rotation properties
    /// <summary>
    /// the sensitivity of camera rotation in y axis
    /// </summary>
    private float sensitivityY = 2f;

    /// <summary>
    /// minimum y value allowed when camera ratate on vertical axis
    /// </summary>
    public float minmumY = -30f;
    #endregion

    /// <summary>
    /// maximum y value allowed when camera ratate on vertical axis
    /// </summary>
    public float maxmunY = 30f;

    /// <summary>
    /// current camera rotation on vertical axis
    /// </summary>
    private float rotationY ;
    #endregion

    //Methods
    #region Methods of monobehaviors
    void Awake()
    {
        instance = this;
        rotationY = 0f;
    }
    #endregion

    #region Methods of camera rotation
    public void SetCameraRotate()
    {
        float rotationX = transform.localEulerAngles.y + Input.GetAxis("Mouse X") * sensitivityX;
        
        rotationY += Input.GetAxis("Mouse Y") * sensitivityY;
        
        rotationY = Clamp(rotationY,maxmunY,minmumY);
        
        transform.localEulerAngles = new Vector3(-rotationY,rotationX,0);
    }
    #endregion

    #region Methods of cameractrl initialization
    public void Init()
    {
    }
    #endregion

    #region Methods of camera zoom
    /// <summary>
    /// set camera zoom
    /// </summary>
    /// <param name="type">0=zoom in,1=zoom out</param>
    public void SetCameraZoom(int type)
    {
        switch (type)
        {
            case 0:
                mCameraZoomContainer.transform.Translate(0, 0, zoomSpeed * Time.deltaTime * 1);
                break;

            case 1:
                mCameraZoomContainer.transform.Translate(0, 0, zoomSpeed * Time.deltaTime * -1);
                break;

            default:
                break;
        }

        float y = mCameraZoomContainer.transform.localPosition.y;
        float z = mCameraZoomContainer.transform.localPosition.z;
        mCameraZoomContainer.transform.localPosition = new Vector3(0, y, z);
    }
    #endregion

    #region Methods of helper functions
    public float Clamp(float value, float max, float min)
    {
        if (value < min) return min;
        if (value > max) return max;
        return value;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, 15f);

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, 14f);
    }
    #endregion
}
