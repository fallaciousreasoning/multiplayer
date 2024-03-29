﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MultiPlayer.Core;
using MultiPlayer.Core.Messaging;

namespace XamlEditor.Scene.Messages
{
    public class SelectMessage : ITargetedMessage
    {
        public SelectMessage(Entity target)
        {
            Target = target;
        }
        
        public Entity Target { get; }
    }
}
