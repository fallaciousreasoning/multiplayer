using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MultiPlayer.Annotations;

namespace MultiPlayer.GameComponents
{
    public abstract class KnowsParentType<T> : IKnowsParent where T : class
    {
        [EditorIgnore]
        public object ParentObject { get; set; }

        /// <summary>
        /// Casts the parent object to the expected type
        /// </summary>
        public T Parent => ParentObject as T;
    }
}
