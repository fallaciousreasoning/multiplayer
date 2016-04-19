using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace MultiPlayer.GameComponents.Animation
{
    public interface IAccessor
    {
        GameObject GameObject { get; set; }
        object Get();

        void Set(object value, object relativeTo);
    }
}
