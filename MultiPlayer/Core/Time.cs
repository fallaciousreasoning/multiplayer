using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultiPlayer.Core
{
    public class Time
    {
        public float DeltaTime;
        public float GameSpeed = 1;

        public float Step => DeltaTime*GameSpeed;
    }
}
