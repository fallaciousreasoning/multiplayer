using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultiPlayer.Core.Systems
{
    public interface IRequiresFamily
    {
        Type FamilyType { get; }
    }
}
