using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultiPlayer.Annotations
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public class WorksOnType : Attribute
    {
        public Type Expects { get; }
        public List<Type> Composition { get; } = new List<Type>();
        public WorksOnType(Type expects)
        {
            Expects = expects;

            ExtractTypes();
        }

        private void ExtractTypes()
        {
            var fields = Expects.GetFields();

            foreach (var field in fields)
            {
                if (field.IsLiteral || field.IsInitOnly || !field.IsPublic) continue;
                Composition.Add(field.FieldType);
            }
        }
    }
}
