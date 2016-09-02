using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MultiPlayer.Annotations;
using MultiPlayer.Collections;
using MultiPlayer.Core.Families;
using MultiPlayer.Core.Messaging;
using MultiPlayer.Extensions;

namespace MultiPlayer.Core.Systems
{
    [HearsMessage(typeof(StartMessage))]
    public abstract class EntityProcessingSystem : IFamilyComposedOf, IKnowsEngine, ISystem
    {
        protected bool Started { get; private set; }
        protected IFamily Family;
        protected IObservableLinkedList<Entity> Entities;

        public void RecieveMessage(IMessage message)
        {
            if (!Started)
                Start();

            Entities.Foreach(entity => Process(message, entity));
        }

        private void Start()
        {
            Family = Engine.FamilyManager.Get(new ConstituentTypes(Types));
            Entities = Family.Entities;

            Entities.ItemAdded += (source, item) => OnEntityAdded((Entity) item);
            Entities.ItemRemoved += (source, item) => OnEntityRemoved((Entity)item);

            Started = true;
        }

        protected virtual void OnEntityAdded(Entity entity)
        {
        }

        protected virtual void OnEntityRemoved(Entity entity)
        {
        }

        protected abstract void Process(IMessage message, Entity entity);

        public abstract IList<Type> Types { get; }
        public Engine Engine { get; set; }
    }
}
