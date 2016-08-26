using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MultiPlayer.Annotations;
using MultiPlayer.Extensions;

namespace MultiPlayer.Core.Systems
{
    public static class ISystemExtensions
    {
        public static IEnumerable<Type> HearsMessages(this ISystem system)
        {
            var receives = new HashSet<Type>();

            var type = system.GetType();
            var attributes = type.GetAllCustomAttributes();

            foreach (var a in attributes)
            {
                if (!(a is HearsMessageAttribute)) continue;

                var recieveAttr = (HearsMessageAttribute) a;
                recieveAttr.MessageTypes.Foreach(t => receives.Add(t));
            }

            var hearsMessageTypes = system as IHearsMessageTypes;
            hearsMessageTypes?.HearsMessages.Foreach(m => receives.Add(m));

            return receives;
        }
    }
}
