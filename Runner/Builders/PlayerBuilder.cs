using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FarseerPhysics.Dynamics;
using Microsoft.Xna.Framework;
using MultiPlayer;
using MultiPlayer.GameComponents;
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
            var clamberSensorOffset = 1.5f;

            var groundDetector = new TriggerDetector() { TriggeredBy = "Ground" };
            var leftWallDetector = new TriggerDetector() { TriggeredBy = "Ground"};
            var rightWallDetector = new TriggerDetector() { TriggeredBy = "Ground" };
            var clamberDetector = new TriggerDetector() {TriggeredBy = "Ground"};

            var clamberRightAnimation = ClamberAnimation().Create();
            var rollanimation = BuildRollAnimation().Create();

            var animator = new AnimationController()
                .Add("clamber_right", clamberRightAnimation)
                .Add("roll", rollanimation);

            var characterMotor = new CharacterMotor();
            characterMotor.GroundDetector = groundDetector;
            characterMotor.LeftWallDetector = leftWallDetector;
            characterMotor.RightWallDetector = rightWallDetector;

            characterMotor.ClamberDetector = clamberDetector;
            characterMotor.Animator = animator;


            return GameObjectFactory.New()
                .WithTag("player")
                .WithTexture(TextureUtil.CreateTexture(widthPixels,heightPixels, Color.White))
                .With(characterMotor)
                .With(new PlayerController())
                .With(animator)

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
                    .AtPosition(-new Vector2(0, (height * 0.5f + sensorWidth*0.5f)*clamberSensorOffset))
                    .Create())

                .With(ColliderFactory.BoxCollider(width, height, BodyType.Dynamic, true));
        }

        public static AnimationBuilder ClamberAnimation()
        {
            var animator = AnimationBuilder.New()
                .InsertFrame(0, new KeyFrame(new Vector2(0)))
                .InsertFrame(0.2f, new KeyFrame(new Vector2(0, -1)))
                .InsertFrame(0.3f, new KeyFrame(new Vector2(0.5f, -1f)))
                .IsRelative(true);

            return animator;
        }

        public static AnimationBuilder BuildRollAnimation()
        {
            var animator = AnimationBuilder.New()
                .InsertFrame(0f, new KeyFrame(new Vector2(0f), 0))
                .InsertFrame(2.5f, new KeyFrame(new Vector2(1f, 0), MathHelper.Pi))
                .InsertFrame(5f, new KeyFrame(new Vector2(1f, 1f), MathHelper.TwoPi))
                .IsRelative(true)
                .Loops(true);

            return animator;
        }

        public static GameObject AnimationTest()
        {
            var animator = BuildRollAnimation().Create();

            var gameObject = GameObjectFactory.New()
                .WithTexture(TextureUtil.CreateTexture(64, 64, Color.Yellow))
                .With(animator)
                .Create();
            
            animator.Start();

            return gameObject;
        }
    }
}
