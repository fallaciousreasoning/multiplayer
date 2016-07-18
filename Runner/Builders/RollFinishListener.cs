using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MultiPlayer;
using MultiPlayer.GameComponents;
using MultiPlayer.GameComponents.Animation;

namespace Runner.Builders
{
    public class RollFinishListener : IStartable, IHearsAnimationEnd, IKnowsGameObject
    {
        public CharacterMotor CharacterMotor { get; set; }

        public void OnAnimationEnd(Animation animation)
        {
            if (CharacterMotor == null) return;
            if (CharacterMotor.State != CharacterState.Rolling) return;

            if (!CharacterMotor.CanStand)
                CharacterMotor.Roll();
            else CharacterMotor.State = CharacterState.Normal;
        }

        public void Start()
        {
            if (CharacterMotor != null) return;

            CharacterMotor = GameObject.GetComponent<CharacterMotor>();
        }

        public GameObject GameObject { get; set; }
    }
}
