using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MultiPlayer.Core.Components;

namespace MultiPlayer.Core.Families
{
    public class CollidableFamily : BasicFamily
    {
        public CollidableFamily()
            : base(new [] {typeof(Transform), typeof(Collider)})
        {
        }
    }
}
