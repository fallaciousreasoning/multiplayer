using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MultiPlayer.Core.Families;
using MultiPlayer.Core.Messaging;
using MultiPlayer.Core.Nodes;
using MultiPlayer.Core.Systems;

namespace MultiPlayer.Test.Systems
{
    public class TestCollisionListener : CollidableSystem<CollidableNode>
    {
        protected override void OnCollisionEnter(CollisionMessage message)
        {
            Debug.WriteLine($"Collision Enter {message.Target.Id}");
        }

        protected override void OnCollisionExit(CollisionMessage message)
        {
            Debug.WriteLine($"Collision Exit {message.Target.Id}");
        }

        protected override void OnTriggerEnter(CollisionMessage message)
        {
            Debug.WriteLine($"Trigger Enter {message.Target.Id}");
        }

        protected override void OnTriggerExit(CollisionMessage message)
        {
            Debug.WriteLine($"Trigger Exit {message.Target.Id}");
        }
    }
}
