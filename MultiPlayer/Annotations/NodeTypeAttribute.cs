using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultiPlayer.Annotations
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public class NodeTypeAttribute : Attribute
    {
        public Type Expects { get; }
        public NodeTypeAttribute(Type expects)
        {
            Expects = expects;
        }
    }
}
