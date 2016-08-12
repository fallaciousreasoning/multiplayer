using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultiPlayer.GameComponents
{
    public interface IHearsChildDestroyed
    {
        void OnChildDestroyed(object child);
    }
}
