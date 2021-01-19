using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization.Configuration;
using System.Text;
using System.Threading.Tasks;
using Common;
using GameServer.Entities;
using Network;
using SkillBridge.Message;

namespace GameServer.Services
{
    class BagService: Singleton<BagService>
    {
        public BagService()
        {
            //MessageDistributer.Instance.Subscribe<BagUpdateRequest>(OnBagUpdate);
        }
        public void Init()
        {
        }

        public void OnBagUpdate(NetConnection<NetSession> conn, BagUpdateRequest request)
        {
            Character character = conn.Session.Character;

            //update protocol bag info
            character.Info.Bag = request.Bag;

            //update database bag info
            character.Data.Bag.Size = request.Bag.Size;
            character.Data.Bag.Items = request.Bag.Items;
        }
    }
}
