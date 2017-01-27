using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultiPlayer.Core
{
    public class StateMachine
    {
        internal readonly Dictionary<string, Transition> Transitions = new Dictionary<string, Transition>();

        public StateMachine WithTransition(string stateName, Transition state)
        {
            Transitions.Add(stateName, state);
            return this;
        }
    }

    public class Transition
    {
        internal bool IsCleanState { get; private set; }
        internal readonly List<Type> WithoutComponents = new List<Type>();
        internal readonly List<object> WithComponents = new List<object>();
        internal readonly List<Type> NewComponents = new List<Type>();
        internal readonly List<Type> EnsureHas = new List<Type>();
        internal readonly List<Func<object>> ComponentCreators = new List<Func<object>>();

        private readonly HashSet<Type> types = new HashSet<Type>();

        public Transition SetIsCleanState(bool isCleanState = true)
        {
            IsCleanState = isCleanState;
            return this;
        }

        public Transition WithoutComponent<T>()
            where T : class
        {
            var t = typeof(T);
            EnsureValidWithType(t);
            WithoutComponents.Add(t);

            return this;
        }

        public Transition WithComponent(object component)
        {
            EnsureValidWithType(component.GetType());

            WithComponents.Add(component);
            return this;
        }

        public Transition WithComponent<T>()
            where T : class, new()
        {
            var t = typeof(T);
            EnsureValidWithType(t);

            EnsureHas.Add(t);
            return this;
        }

        public Transition WithNewComponent<T>()
            where T : class, new()
        {
            var t = typeof(T);
            EnsureValidWithType(t);
            NewComponents.Add(t);
            return this;
        }

        public Transition WithComponent<T>(Func<T> creator)
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
