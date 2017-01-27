using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MultiPlayer.Annotations;
using MultiPlayer.Core.Families;
using MultiPlayer.Core.Messaging;

namespace MultiPlayer.Core.Systems
{
    [HearsMessage(typeof(StateTransitionMessage))]
    public class StateManager : ISystem, IFamilyComposedOf
    {
        public void RecieveMessage(IMessage message)
        {
            var stateTransitionMessage = message as StateTransitionMessage;
            if (stateTransitionMessage == null) return;
            
            ChangeState(stateTransitionMessage.Target, stateTransitionMessage.TargetState);
        }

        private void ChangeState(Entity entity, string stateName)
        {
            var stateMachine = entity.Get<StateMachine>();

            if (!stateMachine.States.ContainsKey(stateName)) throw new ArgumentException($"Invalid state {stateName}");

            var state = stateMachine.States[stateName];

            foreach (var type in state.withoutComponents)
            {
                if (!entity.HasComponent(type)) continue;

                entity.Remove(type);
            }

            foreach (var type in state.ensureHas)
            {
                if (entity.HasComponent(type)) continue;
                entity.Add(Activate(type));
            }

            foreach (var type in state.newComponents)
            {
                if (entity.HasComponent(type)) entity.Remove(type);
                entity.Add(Activate(type));
            }

            foreach (var component in state.withComponents)
            {
                var type = component.GetType();
                if (entity.HasComponent(type))
                {
                    if (entity.Get(type) == component) continue;
                    entity.Remove(type);
                }

                entity.Add(component);
            }

            foreach (var creator in state.componentCreators)
            {
                var component = creator();
                var type = component.GetType();

                if (entity.HasComponent(type)) entity.Remove(type);

                entity.Add(component);
            }
        }

        private object Activate(Type t)
        {
            var c = t.GetConstructor(new Type[0]);
            return c.Invoke(new object[0]);
        }

        public IList<Type> Types { get; } = new List<Type> {typeof(StateMachine)};
    }
}
