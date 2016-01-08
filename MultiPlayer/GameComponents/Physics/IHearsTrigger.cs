using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MultiPlayer.GameComponents.Physics
{
    public interface IHearsTrigger
    {
        void OnTriggerEntered(GameObject hit);
        void OnTriggerExited(GameObject separated);
    }
}
