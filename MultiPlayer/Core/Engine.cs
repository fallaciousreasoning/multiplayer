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

        public Engine(Scene scene)
        {
            Scene = scene;

            MessageHub = new MessageHub(this);
            FamilyManager = new FamilyManager();

            Entities = new ReadOnlyEnumerable<Entity>(entities);
            Systems = systems;
        }

        public void Start()
        {
            MessageHub.SendMessage(new StartMessage());
        }

        public void AddEntity(Entity entity)
        {
            entity.Id = nextId;
            entity.Engine = this;

            entityIds.Add(entity.Id, entity);
            entity.Node = entities.AddLast(entity);

            entity.ComponentAdded = OnComponentAdded;
            entity.ComponentRemoved = OnComponentRemoved;

            FamilyManager.OnEntityCreated(entity);
            MessageHub.SendMessage(new EntityAddedMessage(entity));

            nextId++;
        }

        private void OnComponentAdded(Entity entity, object component)
        {
            FamilyManager.OnComponentAdded(entity, component);
            MessageHub.SendMessage(new ComponentAddedMessage(entity, component));
        }

        private void OnComponentRemoved(Entity entity, object component)
        {
            FamilyManager.OnComponentRemoved(entity, component);
            MessageHub.SendMessage(new ComponentRemovedMessage(entity, component));
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

            var systemType = system.GetType();
            var messageTypes = system.HearsMessages();
            messageTypes.Foreach(t => MessageHub.Register(systemType, t));

            var requiredTypes = system.RequiredTypes();
            FamilyManager.Register(requiredTypes);

            var nodeType = system.NodeType();
            if (nodeType != null)
                FamilyManager.RegisterNodeFamily(nodeType);
            

            if (system is IKnowsEngine)
                (system as IKnowsEngine).Engine = this;
        }

        public void Update(Time time)
        {
            Updating = true;
            
            MessageHub.SendMessage(new UpdateMessage(time));
            MessageHub.SendMessage(new LateUpdateMessage(time));

            Updating = false;
        }

        public void Draw()
        {
            MessageHub.SendMessage(new DrawMessage());
        }

        public Scene Scene { get; }
        public MessageHub MessageHub { get; }
        public FamilyManager FamilyManager { get; }

        public IEnumerable<Entity> Entities { get; }
        public SystemManager Systems { get; }

        public bool Updating { get; private set; }
    }
}
