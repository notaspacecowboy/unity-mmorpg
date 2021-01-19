//=============================
//Author: Zack Yang 
//Created Date: 11/09/2020 20:30
//=============================
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SkillBridge.Message;

namespace Assets.Scripts.Managers
{
    interface IEntityNotifier
    {
        void OnEntityRemove();
        void OnEntityChange(Entity entity);
        void OnEntityEvent(EntityEvent ev);

    }
    class EntityManager : Singleton<EntityManager>
    {
        private Dictionary<int, Entity> entities = new Dictionary<int, Entity>();
        private Dictionary<int, IEntityNotifier> notifiers = new Dictionary<int, IEntityNotifier>();

        public void RegisterEntityNotifier(int id, IEntityNotifier notifier)
        {
            notifiers[id] = notifier;
        }
        
        public void AddEntity(Entity entity)
        {
            entities[entity.entityID] = entity;
        }

        public void RemoveEntity(int id)
        {
            entities.Remove(id);

            if (notifiers.ContainsKey(id))
            {
                notifiers[id].OnEntityRemove();
                notifiers.Remove(id);
            }
        }

        public void UpdateEntity(List<NEntitySync> entitySyncs)
        {
            foreach (var data in entitySyncs)
            {
                Entity entity = null;
                if (entities.TryGetValue(data.Id, out entity))
                {
                    entity.entityData = data.Entity;

                    if (notifiers.ContainsKey(data.Id))
                    {
                        notifiers[data.Id].OnEntityChange(entity);
                        notifiers[data.Id].OnEntityEvent(data.Event);
                    }
                }
            }
        }
    }
}
