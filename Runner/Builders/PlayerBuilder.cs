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
            var width = 0.5f;
            var height = 1f;

            var widthPixels = (int) (width*Transform.PIXELS_A_METRE);
            var heightPixels = (int) (height*Transform.PIXELS_A_METRE);

            var sensorWidthRatio = 0.2f;
            var sensorLopOff = 0.9f;
            var sensorWidth = width*sensorWidthRatio;
            var sensorWidthPixels = (int) (sensorWidth*Transform.PIXELS_A_METRE);

            var groundDetector = new TriggerDetector() { TriggeredBy = "Ground" };
            var leftWallDetector = new TriggerDetector() { TriggeredBy = "Ground"};
            var rightWallDetector = new TriggerDetector() { TriggeredBy = "Ground" };
            var clamberDetector = new TriggerDetector() {TriggeredBy = "Ground"};

            var characterMotor = new CharacterMotor();
            characterMotor.GroundDetector = groundDetector;
            characterMotor.LeftWallDetector = leftWallDetector;
            characterMotor.RightWallDetector = rightWallDetector;
            characterMotor.ClamberDetector = clamberDetector;

            return GameObjectFactory.New()
                .WithTag("player")
                .WithTexture(TextureUtil.CreateTexture(widthPixels,heightPixels, Color.White))
                .With(characterMotor)
                .With(new PlayerController())

                //Add the ground detector (bar below)
                .WithChild(GameObjectFactory.New()
                    .With(ColliderFactory.BoxTrigger(width*sensorLopOff, sensorWidth))
                    .With(groundDetector)
                    .WithTexture(TextureUtil.CreateTexture(widthPixels, sensorWidthPixels, Color.Red))
                    .AtPosition(new Vector2(0, height * 0.5f + 0.1f))
                    .Create())

                //Add the left wall detector (bar down left side)
                .WithChild(GameObjectFactory.New()
                    .With(ColliderFactory.BoxTrigger(sensorWidth, height*sensorLopOff))
                    .With(leftWallDetector)
                    .WithTexture(TextureUtil.CreateTexture(sensorWidthPixels, heightPixels, Color.Red))
                    .AtPosition(-new Vector2(width * 0.5f + sensorWidth*0.5f, 0))
                    .Create())

                //Add the right wall detector (bar down right side)
                .WithChild(GameObjectFactory.New()
                    .With(ColliderFactory.BoxTrigger(sensorWidth, height*sensorLopOff))
                    .With(rightWallDetector)
                    .WithTexture(TextureUtil.CreateTexture(sensorWidthPixels, heightPixels, Color.Red))
                    .AtPosition(new Vector2(width * 0.5f + sensorWidth*0.5f, 0))
                    .Create())

                //Add the clamber detector (bar over top, extending out sides)
                .WithChild(GameObjectFactory.New()
                    .With(ColliderFactory.BoxTrigger(width * 3f*sensorLopOff, sensorWidth))
                    .With(clamberDetector)
                    .WithTexture(TextureUtil.CreateTexture(widthPixels*3, sensorWidthPixels, Color.Red))
                    .AtPosition(-new Vector2(0, height * 0.5f + sensorWidth*0.5f))
                    .Create())

                .With(ColliderFactory.BoxCollider(width, height, BodyType.Dynamic, true));
        }
    }
}
