using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultiPlayer.Core
{
    public class StateMachine
    {
        public readonly Dictionary<string, State> States = new Dictionary<string, State>();

        public StateMachine WithState(string stateName, State state)
        {
            States.Add(stateName, state);
            return this;
        }
    }

    public class State
    {
        internal readonly List<Type> withoutComponents = new List<Type>();
        internal readonly List<object> withComponents = new List<object>();
        internal readonly List<Type> newComponents = new List<Type>();
        internal readonly List<Type> ensureHas = new List<Type>();
        internal readonly List<Func<object>> componentCreators = new List<Func<object>>();

        private readonly HashSet<Type> types = new HashSet<Type>();

        public State WithoutComponent<T>()
            where T : class
        {
            var t = typeof(T);
            EnsureValidWithType(t);
            withComponents.Add(t);

            return this;
        }

        public State WithComponent(object component)
        {
            EnsureValidWithType(component.GetType());

            withComponents.Add(component);
            return this;
        }

        public State WithComponent<T>()
            where T : class, new()
        {
            var t = typeof(T);
            EnsureValidWithType(t);

            ensureHas.Add(t);
            return this;
        }

        public State WithNewComponent<T>()
            where T : class, new()
        {
            var t = typeof(T);
            EnsureValidWithType(t);
            newComponents.Add(t);
            return this;
        }

        public State WithComponent<T>(Func<T> creator)
        {
            EnsureValidWithType(typeof(T));

            componentCreators.Add(() => creator);
            return this;
        }

        private void EnsureValidWithType(Type t)
        {
            if (types.Contains(t)) throw new Exception($"State already has a rule for type {t.Name}");
            types.Add(t);
        }
    }
}
