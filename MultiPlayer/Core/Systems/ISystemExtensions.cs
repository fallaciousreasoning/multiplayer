using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MultiPlayer.Annotations;
using MultiPlayer.Collections;
using MultiPlayer.Core.Families;
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
                recieveAttr.MessageTypes.ForEach(t => receives.Add(t));
            }

            var hearsMessageTypes = system as IHearsMessageTypes;
            hearsMessageTypes?.HearsMessages.ForEach(m => receives.Add(m));

            return receives;
        }

        public static ConstituentTypes RequiredTypes(this ISystem system)
        {
            var type = system.GetType();
            var types= new HashSet<Type>();

            var familyComposedOf = system as IFamilyComposedOf;
            if (familyComposedOf != null)
                return new ConstituentTypes(familyComposedOf.Types);

            var requiresFamily = system as IRequiresFamily;
            if (requiresFamily != null)
                return new ConstituentTypes(requiresFamily.FamilyType.ComposingTypes());

            var attributes = type.GetAllCustomAttributes();

            foreach (var attribute in attributes)
            {
                if (!(attribute is NodeTypeAttribute)) continue;

                var nodeTypeAttr = ((NodeTypeAttribute) attribute);
                nodeTypeAttr.Expects.ComposingTypes().ForEach(t => types.Add(t));
            }

            return new ConstituentTypes(types);
        }

        public static Type NodeType(this ISystem system)
        {
            var type = system.GetType();

            var requiresFamily = system as IRequiresFamily;
            if (requiresFamily != null)
                return requiresFamily.FamilyType;

            return (type.GetAllCustomAttributes().FirstOrDefault(a => a is NodeTypeAttribute) as NodeTypeAttribute)?.Expects;
        }
    }
}
