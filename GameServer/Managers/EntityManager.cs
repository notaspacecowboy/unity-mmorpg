using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;
using GameServer.Entities;

namespace GameServer.Managers
{
    class EntityManager : Singleton<EntityManager>
    {
        private int index = 0;
        private List<Entity> mEntities = new List<Entity>();
        private Dictionary<int, List<Entity>> mMapEntites = new Dictionary<int, List<Entity>>();

        public void AddEntity(int mapID, Entity entity)
        {
            mEntities.Add(entity);
            entity.EntityData.Id = ++index;

            if (!mMapEntites.ContainsKey(mapID))
            {
                mMapEntites.Add(mapID, new List<Entity>());
            }
            mMapEntites[mapID].Add(entity);
            return;
        }

        public void RemoveEntity(int mapID, Entity entity)
        {
            mEntities.Remove(entity);

            if (mMapEntites.ContainsKey(mapID) && mMapEntites[mapID] != null)
            {
                mEntities.Remove(entity);
            }

            return;
        }
    }
}
