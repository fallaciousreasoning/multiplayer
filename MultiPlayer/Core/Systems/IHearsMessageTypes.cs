﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultiPlayer.Core.Systems
{
    public interface IHearsMessageTypes
    {
        IList<Type> HearsMessages { get; }
    }
}
