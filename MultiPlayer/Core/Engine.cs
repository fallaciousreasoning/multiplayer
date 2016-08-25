using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MultiPlayer.Core.Families;
using MultiPlayer.Core.Messaging;
using MultiPlayer.Core.Systems;
using MultiPlayer.Extensions;

namespace MultiPlayer.Core
{
    public class Engine
    {
        private readonly SystemManager systems = new SystemManager();

        private readonly LinkedList<Entity> entities = new LinkedList<Entity>();
        private readonly Dictionary<int, Entity> entityIds = new Dictionary<int, Entity>();
        private int nextId = 1;

        public Engine()
        {
            MessageHub = new MessageHub(this);
            FamilyManager = new FamilyManager();

            Entities = new ReadOnlyEnumerable<Entity>(entities);
            Systems = systems;
        }

        public void AddEntity(Entity entity)
        {
            entity.Id = nextId;
            entity.Engine = this;

            entityIds.Add(entity.Id, entity);
            entity.Node = entities.AddLast(entity);

            entity.ComponentAdded = FamilyManager.OnComponentAdded;
            entity.ComponentRemoved = FamilyManager.OnComponentRemoved;

            FamilyManager.OnEntityCreated(entity);
            MessageHub.SendMessage(new EntityAddedMessage(entity));

            nextId++;
        }

        public void RemoveEntity(Entity entity)
        {
            entityIds.Remove(entity.Id);
            entities.Remove(entity.Node);

            entity.Node = null;

            entity.ComponentAdded = null;
            entity.ComponentRemoved = null;

            FamilyManager.OnEntityRemoved(entity);
            MessageHub.SendMessage(new EntityRemovedMessage(entity));

            entity.Engine = null;
        }

        public Entity GetEntity(int entityId)
        {
            return entityIds[entityId];
        }

        public void AddSystem(ISystem system)
        {
            systems.Add(system);

            var registrableSystem = system as IRegistrableSystem;
            if (registrableSystem != null)
            {
                var types = registrableSystem.Receives;
                var systemType = registrableSystem.GetType();
                types.Foreach(t => MessageHub.Register(systemType, t));
            }

            var requiresFamilies = system as IRequiresFamilies;
            if (requiresFamilies != null)
            {
                var requiredTypes = requiresFamilies.RequiredFamilies;
                requiredTypes.Foreach(FamilyManager.Register);
            }

            if (system is IKnowsEngine)
                (system as IKnowsEngine).Engine = this;
        }

        public void Update(Time time)
        {
            Updating = true;
            
            MessageHub.SendMessage(new UpdateMessage(time));

            Updating = false;
        }

        public MessageHub MessageHub { get; }
        public FamilyManager FamilyManager { get; }

        public IEnumerable<Entity> Entities { get; }
        public SystemManager Systems { get; }

        public bool Updating { get; private set; }
    }
}
