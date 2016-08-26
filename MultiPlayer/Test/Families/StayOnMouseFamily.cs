using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MultiPlayer.Core.Components;
using MultiPlayer.Core.Families;
using MultiPlayer.Test.Components;

namespace MultiPlayer.Test.Families
{
    public class StayOnMouseFamily : BasicFamily
    {
        public StayOnMouseFamily() : base(new [] {typeof(Transform), typeof(StayOnMouse)})
        {
        }
    }
}
