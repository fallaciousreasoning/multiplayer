using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using FarseerPhysics.Dynamics;
using Microsoft.Xna.Framework;
using MultiPlayer;
using MultiPlayer.GameComponents;
using MultiPlayer.GameComponents.Animation;
using MultiPlayer.GameComponents.Physics;
using Newtonsoft.Json;

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

            var roomToStandDetector = new TriggerDetector() {TriggeredBy = "Ground"};
            var groundDetector = new TriggerDetector() { TriggeredBy = "Ground" };
            var ceilingDetector = new TriggerDetector() {TriggeredBy = "Ground"};
            var leftWallDetector = new TriggerDetector() { TriggeredBy = "Ground"};
            var rightWallDetector = new TriggerDetector() { TriggeredBy = "Ground" };
            var clamberDetector = new TriggerDetector() {TriggeredBy = "Ground"};

            var clamberRightAnimation = ClamberAnimation().Create();
            var clamberLeftAnimation = ClamberAnimation()
                .ReflectHorizontal()
                .Create();

            var rollRightAnimation = RollAnimation()
                .Create();
            var rollLeftAnimation = RollAnimation()
                .ReflectHorizontal()
                .ReflectRotation()
                .Create();
            
            var slideDownRightAnimation = SlideDownAnimation()
                .Create();

            var slideDownLeftAnimation = SlideDownAnimation()
                .ReflectHorizontal()
                .ReflectRotation()
                .Create();
            
            var slideUpRightAnimation = SlideDownAnimation()
                .Reversed()
                .Create();

            var slideUpLeftAnimation = SlideDownAnimation()
                .Reversed()
                .ReflectHorizontal()
                .ReflectRotation()
                .Create();


            var animator = new AnimationController()
                .Add(Animations.Name(PlayerAnimation.Clamber, Direction.Right), clamberRightAnimation)
                .Add(Animations.Name(PlayerAnimation.Clamber, Direction.Left), clamberLeftAnimation)

                .Add(Animations.Name(PlayerAnimation.Roll, Direction.Left), rollLeftAnimation)
                .Add(Animations.Name(PlayerAnimation.Roll, Direction.Right), rollRightAnimation)

                .Add(Animations.Name(PlayerAnimation.SlideDown, Direction.Right), slideDownRightAnimation)
                .Add(Animations.Name(PlayerAnimation.SlideUp, Direction.Right), slideUpRightAnimation)
                .Add(Animations.Name(PlayerAnimation.SlideDown, Direction.Left), slideDownLeftAnimation)
                .Add(Animations.Name(PlayerAnimation.SlideUp, Direction.Left), slideUpLeftAnimation);
            
            var characterMotor = new CharacterMotor();
            characterMotor.Stats = LoadStats();

            characterMotor.GroundDetector = groundDetector;
            characterMotor.CeilingDetector = ceilingDetector;
            characterMotor.RoomToStandDetector = roomToStandDetector;

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
                .With(new RollFinishListener())

                .WithChild(GameObjectFactory.New(false, true, true)
                    //Add the ground detector (bar below)
                    .WithChild(GameObjectFactory.New()
                        .With(ColliderFactory.BoxTrigger(width * sensorLopOff, sensorWidth))
                        .With(groundDetector)
                        .WithTexture(TextureUtil.CreateTexture(widthPixels, sensorWidthPixels, Color.Red))
                        .AtPosition(new Vector2(0, height * 0.5f + sensorWidth * 0.5f))
                        .Create())

                    //Add the ceiling detector (bar above)
                    .WithChild(GameObjectFactory.New()
                        .With(ColliderFactory.BoxTrigger(width * sensorWidth, sensorWidth))
                        .With(ceilingDetector)
                        .WithTexture(TextureUtil.CreateTexture(widthPixels, sensorWidthPixels, Color.Red))
                        .AtPosition(-new Vector2(0, height * 0.5f + sensorWidth * 0.5f))
                        .Create())

                    //Add the room to stand detector (box above)
                    .WithChild(GameObjectFactory.New()
                        .With(ColliderFactory.BoxTrigger(width * sensorWidth, sensorWidth))
                        .With(ceilingDetector)
                        .WithTexture(TextureUtil.CreateTexture(widthPixels, sensorWidthPixels, Color.Red))
                        .AtPosition(-new Vector2(0, height * 0.75f + sensorWidth * 0.5f))
                        .Create())

                    //Add the left wall detector (bar down left side)
                    .WithChild(GameObjectFactory.New()
                        .With(ColliderFactory.BoxTrigger(sensorWidth, height * sensorLopOff))
                        .With(leftWallDetector)
                        .WithTexture(TextureUtil.CreateTexture(sensorWidthPixels, heightPixels, Color.Red))
                        .AtPosition(-new Vector2(width * 0.5f + sensorWidth * 0.5f, 0))
                        .Create())

                    //Add the right wall detector (bar down right side)
                    .WithChild(GameObjectFactory.New()
                        .With(ColliderFactory.BoxTrigger(sensorWidth, height * sensorLopOff))
                        .With(rightWallDetector)
                        .WithTexture(TextureUtil.CreateTexture(sensorWidthPixels, heightPixels, Color.Red))
                        .AtPosition(new Vector2(width * 0.5f + sensorWidth * 0.5f, 0))
                        .Create())

                    //Add the clamber detector (bar over top, extending out sides)
                    .WithChild(GameObjectFactory.New()
                        .With(ColliderFactory.BoxTrigger(width * 3f * sensorLopOff, sensorWidth))
                        .With(clamberDetector)
                        .WithTexture(TextureUtil.CreateTexture(widthPixels * 3, sensorWidthPixels, Color.Red))
                        .AtPosition(-new Vector2(0, (height * 0.5f + sensorWidth * 0.5f) * clamberSensorOffset))
                        .Create())
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

        private static CharacterStats LoadStats(string statsFile = "player_config.json")
        {
            CharacterStats stats;
            if (File.Exists(statsFile))
            {
                var json = File.ReadAllText(statsFile);
                stats = JsonConvert.DeserializeObject<CharacterStats>(json);
            }
            else
            {
                stats = new CharacterStats();
                var json = JsonConvert.SerializeObject(stats);
                File.WriteAllText(statsFile, json);
            }
            return stats;
        }

        public static AnimationBuilder RollAnimation()
        {
            var animator = AnimationBuilder.New()
                .InsertFrame(0f, new KeyFrame(new Vector2(0f), 0, new Vector2(1)))
                .InsertFrame(0.2f, new KeyFrame(new Vector2(1f, 0.2f), MathHelper.Pi, new Vector2(1, 0.5f)))
                .InsertFrame(0.4f, new KeyFrame(new Vector2(2f, 0f), MathHelper.TwoPi, new Vector2(1)))
                .AnimatePhysics()
                .IsRelative(true);

            return animator;
        }

        public static AnimationBuilder SlideDownAnimation()
        {
            var animation = AnimationBuilder.New()
                .InsertFrame(0, new KeyFrame(0))
                .InsertFrame(0.2f, new KeyFrame(-MathHelper.PiOver2))
                .IsRelative(false);

            return animation;
        }

        public static GameObject AnimationTest()
        {
            var animator = RollAnimation().Create();

            var gameObject = GameObjectFactory.New()
                .WithTexture(TextureUtil.CreateTexture(64, 64, Color.Yellow))
                .With(animator)
                .Create();
            
            animator.Start();

            return gameObject;
        }
    }
}
