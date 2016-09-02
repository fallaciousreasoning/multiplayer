using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultiPlayer.Core.Families
{
    public interface IFamilyComposedOf
    {
        IList<Type> Types { get; }
    }
}
