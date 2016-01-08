using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MultiPlayer.GameComponents.Physics
{
    public interface IHearsCollision
    {
        void OnCollisionEnter(GameObject hit);
        void OnCollisionExit(GameObject separated);
    }
}
