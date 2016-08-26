using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultiPlayer.Core.Families
{
    public interface INode<T>
    {
        T CreateFrom(Entity entity);
    }
}
