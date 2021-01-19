//=============================
//Author: Zack Yang 
//Created Date: 10/17/2020 19:32
//=============================
using System.Collections;
using System.Collections.Generic;
using Common.Data;
using SkillBridge.Message;
using UnityEngine;


namespace Models
{
    /// <summary>
    /// this class is a singleton and is used to put the current user information in client
    /// </summary>
    public class User : Singleton<User>
    {
        //fields
        #region Fields of user information
        /// <summary>
        /// current user information
        /// </summary>
        private SkillBridge.Message.NUserInfo mInfo;

        /// <summary>
        /// current user information property
        /// </summary>
        public SkillBridge.Message.NUserInfo info
        {
            get { return mInfo; }
        }

        private NCharacterInfo mCurrentCharacter;

        /// <summary>
        /// current character information
        /// </summary>
        public SkillBridge.Message.NCharacterInfo currentCharacter
        {
            get
            {
                return mCurrentCharacter;
            }
            set
            {
                mCurrentCharacter = value;
            }
        }

        public GameObject currentPlayerObject
        {
            get;
            set;
        }

        /// <summary>
        /// current map information
        /// </summary>
        public MapDefine currentMapInfo
        {
            get;
            set;
        }

        #endregion


        //methods
        #region Methods to initialize
        /// <summary>
        /// set up the current user information
        /// </summary>
        /// <param name="userInfo"></param>
        public void SetupUserInfo(SkillBridge.Message.NUserInfo userInfo)
        {
            mInfo = userInfo;
        }

        #endregion
    }
}
