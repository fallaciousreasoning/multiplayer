using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultiPlayer.Collections
{
    public class ConstituentTypes
    {
        private readonly int hashCode;

        public ImmutableHashSet<Type> Types { get; }
        public ImmutableList<Type> TypesList { get; }

        public int Count => Types.Count;

        public bool Contains(Type type)
        {
            return Types.Contains(type);
        }

        public ConstituentTypes(params Type[] types)
        {
            TypesList = types.ToImmutableList();
            Types = TypesList.ToImmutableHashSet();
            hashCode = TypesList.Sum(t => t.GetHashCode());
        }

        public ConstituentTypes(IEnumerable<Type> types)
        {
            TypesList = types.ToImmutableList();
            Types = TypesList.ToImmutableHashSet();
            hashCode = TypesList.Sum(t => t.GetHashCode());
        }

        public override int GetHashCode()
        {
            return hashCode;
        }

        public override bool Equals(object obj)
        {
            if (!(obj is ConstituentTypes)) return false;

            var c = (ConstituentTypes) obj;

            return c.Types.All(Types.Contains);
        }
    }
}
