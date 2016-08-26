using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MultiPlayer.Annotations;
using MultiPlayer.Core.Messaging;

namespace MultiPlayer.Core.Systems
{
    [HearsMessage(typeof(StartMessage), typeof(UpdateMessage), typeof(LateUpdateMessage), typeof(CollisionMessage))]
    public class GenericSystem<T> : ISystem
    {
        public void RecieveMessage(IMessage message)
        {
            Handle(message);
        }

        protected virtual void Handle(IMessage message)
        {
            if (message is StartMessage)
                Start();

            if (message is UpdateMessage)
                Update((message as UpdateMessage).Time);

            if (message is LateUpdateMessage)
                Update((message as LateUpdateMessage).Time);

            var collisionMessage = message as CollisionMessage;
            if (collisionMessage == null) return;

            if (collisionMessage.IsTrigger)
            {
                if (collisionMessage.Mode == CollisionMode.Entered)
                    OnTriggerEnter(collisionMessage);
                else OnTriggerExit(collisionMessage);
            }
            else
            {
                if (collisionMessage.Mode == CollisionMode.Entered)
                    OnCollisionEnter(collisionMessage);
                else OnCollisionExit(collisionMessage);
            }
        }

        protected virtual void Start()
        {
            
        }

        protected virtual void Update(Time time)
        {

        }

        protected virtual void LateUpdate(Time time)
        {
            
        }

        protected virtual void OnCollisionEnter(CollisionMessage collisionMessage)
        {
            
        }

        protected virtual void OnCollisionExit(CollisionMessage collisionMessage)
        {

        }

        protected virtual void OnTriggerEnter(CollisionMessage collisionMessage)
        {

        }

        protected virtual void OnTriggerExit(CollisionMessage collisionMessage)
        {

        }
    }
}
