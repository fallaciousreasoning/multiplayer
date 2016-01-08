using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FarseerPhysics.Dynamics;
using Microsoft.Xna.Framework;
using MultiPlayer;
using MultiPlayer.GameComponents.Physics;

namespace Runner.Builders
{
    public static class PlayerBuilder
    {
        public static GameObjectFactory BuildPlayer()
        {
            var width = 1f;
            var height = 2f;

            var groundDetector = new TriggerDetector();
            var characterMotor = new CharacterMotor();
            characterMotor.GroundDetector = groundDetector;

            return GameObjectFactory.New()
                .WithTag("player")
                .WithTexture(TextureUtil.CreateTexture((int) (width*Transform.PIXELS_A_METRE),
                    (int) (height*Transform.PIXELS_A_METRE), Color.White))
                .With(characterMotor)
                .With(new PlayerController())
                .WithChild(GameObjectFactory.New()
                    .With(ColliderFactory.BoxTrigger(width, 0.2f))
                    .With(groundDetector)
                    .WithTexture(TextureUtil.CreateTexture(64, 64/5, Color.Red))
                    .AtPosition(new Vector2(0, 1.6f))
                    .Create())
                .With(ColliderFactory.BoxCollider(width, height, BodyType.Dynamic));
        }
    }
}
