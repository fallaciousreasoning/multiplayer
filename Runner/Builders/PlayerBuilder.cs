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

            var groundDetector = new TriggerDetector() { TriggeredBy = "Ground" };
            var leftWallDetector = new TriggerDetector() { TriggeredBy = "Ground"};
            var rightWallDetector = new TriggerDetector() { TriggeredBy = "Ground" };
            var clamberDetector = new TriggerDetector() {TriggeredBy = "Ground"};

            var characterMotor = new CharacterMotor();
            characterMotor.GroundDetector = groundDetector;
            characterMotor.LeftWallDetector = leftWallDetector;
            characterMotor.RightWallDetector = leftWallDetector;
            characterMotor.ClamberDetector = leftWallDetector;

            return GameObjectFactory.New()
                .WithTag("player")
                .WithTexture(TextureUtil.CreateTexture((int) (width*Transform.PIXELS_A_METRE),
                    (int) (height*Transform.PIXELS_A_METRE), Color.White))
                .With(characterMotor)
                .With(new PlayerController())

                //Add the ground detector (bar below)
                .WithChild(GameObjectFactory.New()
                    .With(ColliderFactory.BoxTrigger(width, 0.2f))
                    .With(groundDetector)
                    .WithTexture(TextureUtil.CreateTexture(64, 64/5, Color.Red))
                    .AtPosition(new Vector2(0, height * 0.5f + 0.1f))
                    .Create())

                //Add the left wall detector (bar down left side)
                .WithChild(GameObjectFactory.New()
                    .With(ColliderFactory.BoxTrigger(0.2f, height))
                    .With(leftWallDetector)
                    .WithTexture(TextureUtil.CreateTexture(64/5, 128, Color.Red))
                    .AtPosition(-new Vector2(width * 0.5f + 0.1f, 0))
                    .Create())

                //Add the right wall detector (bar down right side)
                .WithChild(GameObjectFactory.New()
                    .With(ColliderFactory.BoxTrigger(0.2f, height))
                    .With(rightWallDetector)
                    .WithTexture(TextureUtil.CreateTexture(64 / 5, 128, Color.Red))
                    .AtPosition(new Vector2(width * 0.5f + 0.1f, 0))
                    .Create())

                //Add the clamber detector (bar over top, extending out sides)
                .WithChild(GameObjectFactory.New()
                    .With(ColliderFactory.BoxTrigger(width * 3f, 0.2f))
                    .With(clamberDetector)
                    .WithTexture(TextureUtil.CreateTexture((int)(width*3*Transform.PIXELS_A_METRE), 64/5, Color.Red))
                    .AtPosition(-new Vector2(0, height * 0.5f + 0.1f))
                    .Create())

                .With(ColliderFactory.BoxCollider(width, height, BodyType.Dynamic));
        }
    }
}
