﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MultiPlayer.Core.Messaging;

namespace MultiPlayer.Core.Systems
{
    public abstract class CollidableSystem : BasicSystem
    {
        protected CollidableSystem(IEnumerable<Type> requires)
            : base(new[] {typeof(CollisionMessage)}, requires)
        {
        }

        protected override void Handle(IMessage message)
        {
            if (!(message is CollisionMessage)) return;

            var collisionMessage = (CollisionMessage) message;
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

        protected abstract void OnCollisionEnter(CollisionMessage message);
        protected abstract void OnCollisionExit(CollisionMessage message);

        protected abstract void OnTriggerEnter(CollisionMessage message);
        protected abstract void OnTriggerExit(CollisionMessage message);
    }
}