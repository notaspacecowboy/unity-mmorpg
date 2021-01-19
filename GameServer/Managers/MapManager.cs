using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;
using GameServer.Models;
using GameServer.Services;

namespace GameServer.Managers
{
    class MapManager : Singleton<MapManager>
    {
        private Dictionary<int, Map> maps = new Dictionary<int, Map>();

        public void Init()
        {
            foreach (var mapDefine in DataManager.Instance.Maps.Values)
            {
                Map map = new Map(mapDefine);

                this.maps.Add(mapDefine.ID, map);
            }
        }


        public Map this[int key]
        {
            get
            {
                return this.maps[key];
            }
        }

        public void Update()
        {
            foreach (var map in this.maps.Values)
            {
                map.Update();
            }
        }
    }
}
