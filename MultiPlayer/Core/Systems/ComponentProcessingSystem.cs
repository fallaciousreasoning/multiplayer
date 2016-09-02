using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MultiPlayer.Core.Messaging;

namespace MultiPlayer.Core.Systems
{
    public abstract class ComponentProcessingSystem<T> : EntityProcessingSystem where T : class
    {
        protected override void Process(IMessage message, Entity entity)
        {
            Process(message, entity.Get<T>());
        }

        public override IList<Type> Types { get; } = new List<Type>() {typeof(T)};
        protected abstract void Process(IMessage message, T component);
    }

    public abstract class ComponentProcessingSystem<T1, T2> : EntityProcessingSystem where T1 : class where T2 : class
    {
        protected override void Process(IMessage message, Entity entity)
        {
            Process(message, entity.Get<T1>(), entity.Get<T2>());
        }

        public override IList<Type> Types { get; } = new List<Type>() { typeof(T1), typeof(T2) };
        protected abstract void Process(IMessage message, T1 component1, T2 component2);
    }

    public abstract class ComponentProcessingSystem<T1, T2, T3> : EntityProcessingSystem where T1 : class where T2 : class where T3 : class
    {
        protected override void Process(IMessage message, Entity entity)
        {
            Process(message, entity.Get<T1>(), entity.Get<T2>(), entity.Get<T3>());
        }

        public override IList<Type> Types { get; } = new List<Type>() { typeof(T1), typeof(T2), typeof(T3) };
        protected abstract void Process(IMessage message, T1 component1, T2 component2, T3 component3);
    }

    public abstract class ComponentProcessingSystem<T1, T2, T3, T4> : EntityProcessingSystem where T1 : class where T2 : class where T3 : class where T4 : class
    {
        protected override void Process(IMessage message, Entity entity)
        {
            Process(message, entity.Get<T1>(), entity.Get<T2>(), entity.Get<T3>(), entity.Get<T4>());
        }

        public override IList<Type> Types { get; } = new List<Type>() { typeof(T1), typeof(T2), typeof(T3), typeof(T4) };
        protected abstract void Process(IMessage message, T1 component1, T2 component2, T3 component3, T4 component4);
    }
}
