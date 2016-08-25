using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MultiPlayer.Core.Components;

namespace MultiPlayer.Core.Families
{
    public class SpriteFamily : BasicFamily
    {
        public SpriteFamily() : base(new []
        {
            typeof(PositionComponent),
            typeof(SpriteComponent)
        })

        {
        }
    }
}
