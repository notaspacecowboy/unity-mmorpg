//=============================
//Author: Zack Yang 
//Created Date: 10/31/2020 23:22
//=============================

using System.CodeDom;
using System.Collections;
using System.Collections.Generic;
using SkillBridge.Message;
using UnityEngine;

[RequireComponent(typeof(EntityController))]
public class PlayerInputController : MonoBehaviour
{
    /// <summary>
    /// player rigidbody component
    /// </summary>
    private Rigidbody mRigidbody;
    
    /// <summary>
    /// player movement status
    /// </summary>
    private SkillBridge.Message.CharacterState mState;

    /// <summary>
    /// entity controller associated with player 
    /// </summary>
    private EntityController mEntityController;

    /// <summary>
    /// player rotate speed
    /// </summary>
    [SerializeField] private float mRotateSpeed = 2.0f;

    /// <summary>
    /// player minimum rotate angle to display rotation animation and update to cha data
    /// </summary>
    [SerializeField] private float mTurnAngle = 5f;

    private int mSpeed;

    /// <summary>
    /// if player is currently not on ground
    /// </summary>
    private bool mOnAir = false;

    /// <summary>
    /// player's position after last update
    /// </summary>
    private Vector3 mLastPos;

    private float lastAsync = 0;


    /// <summary>
    /// player's Character
    /// </summary>
    public Character character;

    // Start is called before the first frame update
    void Start()
    {
        //get and init fields
        mState = CharacterState.Idle;
        mRigidbody = GetComponentInChildren<Rigidbody>();
        mEntityController = GetComponent<EntityController>();

        //if mcharacter hasnt been assigned value, load data and create a temp cha
        if (character == null)
        {
            DataManager.Instance.LoadData();
            NCharacterInfo cinfo = new NCharacterInfo();
            cinfo.Id = 1;
            cinfo.Name = "Test";
            cinfo.Tid = 1;
            cinfo.Entity = new NEntity();
            cinfo.Entity.Position = new NVector3();
            cinfo.Entity.Direction = new NVector3();
            cinfo.Entity.Direction.X = 0;
            cinfo.Entity.Direction.Y = 100;
            cinfo.Entity.Direction.Z = 0;
            character = new Character(cinfo);
        }

        mLastPos = mRigidbody.transform.position;
    }

    /// <summary>
    /// use fixed update to deal with user inputs
    /// When receive input, update 3 things: (1)Character data, (2)rigidbody, (3)animator
    /// </summary>
    void Update()
    {
        //if controller hasnt been assigned with a character, dont read user input
        if (character == null)
            return;

        //get user input on vertical axis
        float v = Input.GetAxis("Vertical");
        //if user wants to move forward
        if (v > 0.01f)
        {
            //send synchronization message only when state changes
            if (mState != SkillBridge.Message.CharacterState.Move)
            {
                mState = CharacterState.Move;
                character.MoveForward();

                SendEntityEvent(EntityEvent.MoveFwd);
            }

            mRigidbody.velocity = this.mRigidbody.velocity.y * Vector3.up + GameObjectTool.LogicToWorld(character.direction) * (character.speed + 9.81f) / 100f;
        }

        //if user wants to move backward
        else if (v < -0.01f)
        {
            if (mState != SkillBridge.Message.CharacterState.Move)
            {
                mState = CharacterState.Move;
                character.MoveBackward();

                SendEntityEvent(EntityEvent.MoveBack);
            }

            mRigidbody.velocity = this.mRigidbody.velocity.y * Vector3.up + GameObjectTool.LogicToWorld(character.direction) * (this.character.speed + 9.81f) / 100f;
        }

        //if user is not moving
        else
        {
            character.Stop();
            if (mState != SkillBridge.Message.CharacterState.Idle)
            {
                mState = CharacterState.Idle;
                mRigidbody.velocity = Vector3.zero;

                SendEntityEvent(EntityEvent.Idle);
            }
        }

        //player jump input detection
        //jump is not considered a movement, only display animation
        if (Input.GetButtonDown("Jump"))
        {
            SendEntityEvent(EntityEvent.Jump);
        }

        //horizontal input are consider as rotation
        float h = Input.GetAxis("Horizontal");
        if (h > 0.1f || h < -0.1f)
        {
            //perform rotation on gameobject
            this.transform.Rotate(0, h * mRotateSpeed, 0);
            Vector3 dir = GameObjectTool.LogicToWorld(character.direction);
            
            //update the data stored in Character
            Quaternion rotation = new Quaternion();
            rotation.SetFromToRotation(dir, this.transform.forward);

            //if offset is too small, do not update
            if (rotation.eulerAngles.y > this.mTurnAngle && rotation.eulerAngles.y < (360 - mTurnAngle))
            {
                character.SetDirection(GameObjectTool.WorldToLogic(this.transform.forward));
                mRigidbody.transform.forward = this.transform.forward;
                SendEntityEvent(EntityEvent.None);
            }
        }
    }


    private void LateUpdate()
    {
        if (this.character == null)
            return;

        //lastPos records the position of rigidbody after last update
        Vector3 offset = this.mRigidbody.transform.position - mLastPos;
        mSpeed = (int) (offset.magnitude * 100 / Time.deltaTime);

        mLastPos = mRigidbody.transform.position;

        Vector3Int goLogicPos = GameObjectTool.WorldToLogic(this.mRigidbody.transform.position);
        float logicOffset = (goLogicPos - this.character.position).magnitude;
        if (logicOffset > 100f)
        {
            this.character.SetPosition(GameObjectTool.WorldToLogic(this.mRigidbody.transform.position));
            this.SendEntityEvent(EntityEvent.None);
        }

        //update gameobject position
        this.transform.position = mRigidbody.transform.position;
    }

    /// <summary>
    /// When player begin/stop moving, send the info to entityController to update
    /// player animation
    /// </summary>
    /// <param name="ev"></param>
    private void SendEntityEvent(EntityEvent ev)
    {
        //send state change to entity controller
        if (mEntityController != null)
        {
            mEntityController.OnEntityEvent(ev);
        }

        //send state change to server for state synchronization
        MapService.Instance.SendEntitySync(character.entityData, ev);
    }


}
