using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MultiPlayer.Core.Messaging;

namespace MultiPlayer.Annotations
{
    [AttributeUsage(AttributeTargets.Class|AttributeTargets.Interface, AllowMultiple = true, Inherited = true)]
    public class HearsMessageAttribute : Attribute
    {
        public IEnumerable<Type> MessageTypes { get; private set; }

        public HearsMessageAttribute(params Type[] types)
        {
            var messageType = typeof(IMessage);
            if (!types.All(t => messageType.IsAssignableFrom(t))) throw new ArgumentException($"All types must implement the message interface!");

            MessageTypes = types;
        }
    }
}
