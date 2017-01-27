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
        internal bool IsCleanState { get; private set; }
        internal readonly List<Type> WithoutComponents = new List<Type>();
        internal readonly List<object> WithComponents = new List<object>();
        internal readonly List<Type> NewComponents = new List<Type>();
        internal readonly List<Type> EnsureHas = new List<Type>();
        internal readonly List<Func<object>> ComponentCreators = new List<Func<object>>();

        private readonly HashSet<Type> types = new HashSet<Type>();

        public State SetIsCleanState(bool isCleanState = true)
        {
            IsCleanState = isCleanState;
            return this;
        }

        public State WithoutComponent<T>()
            where T : class
        {
            var t = typeof(T);
            EnsureValidWithType(t);
            WithComponents.Add(t);

            return this;
        }

        public State WithComponent(object component)
        {
            EnsureValidWithType(component.GetType());

            WithComponents.Add(component);
            return this;
        }

        public State WithComponent<T>()
            where T : class, new()
        {
            var t = typeof(T);
            EnsureValidWithType(t);

            EnsureHas.Add(t);
            return this;
        }

        public State WithNewComponent<T>()
            where T : class, new()
        {
            var t = typeof(T);
            EnsureValidWithType(t);
            NewComponents.Add(t);
            return this;
        }

        public State WithComponent<T>(Func<T> creator)
        {
            EnsureValidWithType(typeof(T));

            ComponentCreators.Add(() => creator);
            return this;
        }

        private void EnsureValidWithType(Type t)
        {
            if (types.Contains(t)) throw new Exception($"State already has a rule for type {t.Name}");
            types.Add(t);
        }
    }
}
