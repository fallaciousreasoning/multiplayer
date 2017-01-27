using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using FarseerPhysics.Dynamics;
using Microsoft.Xna.Framework;
using MultiPlayer;
using MultiPlayer.Core;
using MultiPlayer.Core.Animation;
using MultiPlayer.Core.Components;
using MultiPlayer.Factories;
using Newtonsoft.Json;
using Runner.Components;
using Runner.Systems;

namespace Runner.Builders
{
    public static class CharacterBuilder
    {
        public const string CHARACTER_TAG = "character";
        public const string PLAYER_TAG = "player";

        public const string CLAMBER_STATE = "clamber";
        public const string DIVE_STATE = "dive";
        public const string MOVE_STATE = "move";
        public const string ROLL_STATE = "roll";
        public const string SLIDE_STATE = "slide";

        public static EntityBuilder Player()
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

            var roomToStandDetector = new Touching();
            var groundDetector = new Touching();
            var ceilingDetector = new Touching();
            var leftWallDetector = new Touching();
            var rightWallDetector = new Touching();
            var clamberDetector = new Touching();

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

            var diveRightAnimation = DiveAnimation()
                .Create();

            var diveLeftAnimation = DiveAnimation()
                .ReflectRotation()
                .Create();

            var animator = new AnimationContainer()
                .Add(Animations.Name(PlayerAnimation.Clamber, Direction.Right), clamberRightAnimation)
                .Add(Animations.Name(PlayerAnimation.Clamber, Direction.Left), clamberLeftAnimation)

                .Add(Animations.Name(PlayerAnimation.Roll, Direction.Left), rollLeftAnimation)
                .Add(Animations.Name(PlayerAnimation.Roll, Direction.Right), rollRightAnimation)

                .Add(Animations.Name(PlayerAnimation.SlideDown, Direction.Right), slideDownRightAnimation)
                .Add(Animations.Name(PlayerAnimation.SlideUp, Direction.Right), slideUpRightAnimation)
                .Add(Animations.Name(PlayerAnimation.SlideDown, Direction.Left), slideDownLeftAnimation)
                .Add(Animations.Name(PlayerAnimation.SlideUp, Direction.Left), slideUpLeftAnimation)
                
                .Add(Animations.Name(PlayerAnimation.Dive, Direction.Right), diveRightAnimation)
                .Add(Animations.Name(PlayerAnimation.Dive, Direction.Left), diveLeftAnimation);

            return EntityBuilder.New()
                .WithTag(PLAYER_TAG)
                .WithTag(CHARACTER_TAG)
                .WithTexture(TextureUtil.CreateTexture(widthPixels, heightPixels, Color.White))
                .With(LoadStats())
                .With(new CharacterInfo()
                {
                    CeilingDetector = ceilingDetector,
                    ClamberDetector = clamberDetector,
                    GroundDetector = groundDetector,
                    LeftWallDetector = leftWallDetector,
                    RightWallDetector = rightWallDetector,
                    RoomToStandDetector = roomToStandDetector
                })
                .With(new CharacterInput())
                .With(new Move())
                .With(animator)

                .WithChild(EntityBuilder.New(false, true, true)
                    //Add the ground detector (bar below)
                    .WithChild(EntityBuilder.New()
                        .With(ColliderBuilder.New().BoxShape(width*sensorLopOff, sensorWidth).IsTrigger().Create())
                        .With(groundDetector)
                        .WithTexture(TextureUtil.CreateTexture(widthPixels, sensorWidthPixels, Color.Red))
                        .AtPosition(new Vector2(0, height*0.5f + sensorWidth*0.5f)))

                    //Add the ceiling detector (bar above)
                    .WithChild(EntityBuilder.New()
                        .With(ColliderBuilder.New().BoxShape(width*sensorWidth, sensorWidth).IsTrigger().Create())
                        .With(ceilingDetector)
                        .WithTexture(TextureUtil.CreateTexture(widthPixels, sensorWidthPixels, Color.Red))
                        .AtPosition(-new Vector2(0, height*0.5f + sensorWidth*0.5f)))

                    //Add the room to stand detector (box above)
                    .WithChild(EntityBuilder.New()
                        .With(ColliderBuilder.New().BoxShape(width*sensorWidth, sensorWidth).IsTrigger().Create())
                        .With(ceilingDetector)
                        .WithTexture(TextureUtil.CreateTexture(widthPixels, sensorWidthPixels, Color.Red))
                        .AtPosition(-new Vector2(0, height*0.75f + sensorWidth*0.5f)))

                    //Add the left wall detector (bar down left side)
                    .WithChild(EntityBuilder.New()
                        .With(ColliderBuilder.New().BoxShape(sensorWidth, height*sensorLopOff).IsTrigger().Create())
                        .With(leftWallDetector)
                        .WithTexture(TextureUtil.CreateTexture(sensorWidthPixels, heightPixels, Color.Red))
                        .AtPosition(-new Vector2(width*0.5f + sensorWidth*0.5f, 0)))

                    //Add the right wall detector (bar down right side)
                    .WithChild(EntityBuilder.New()
                        .With(ColliderBuilder.New().BoxShape(sensorWidth, height*sensorLopOff).IsTrigger().Create())
                        .With(rightWallDetector)
                        .WithTexture(TextureUtil.CreateTexture(sensorWidthPixels, heightPixels, Color.Red))
                        .AtPosition(new Vector2(width*0.5f + sensorWidth*0.5f, 0)))

                    //Add the clamber detector (bar over top, extending out sides)
                    .WithChild(EntityBuilder.New()
                        .With(ColliderBuilder.New().BoxShape(width*3f*sensorLopOff, sensorWidth).IsTrigger().Create())
                        .With(clamberDetector)
                        .WithTexture(TextureUtil.CreateTexture(widthPixels*3, sensorWidthPixels, Color.Red))
                        .AtPosition(-new Vector2(0, (height*0.5f + sensorWidth*0.5f)*clamberSensorOffset))))
                .AtPosition(new Vector2(0, -10))
                .With(ColliderBuilder.New().BoxShape(width, height).IsDynamic().IsFixedRotation(true).Create())
                .With(new StateMachine()
                    .WithState(MOVE_STATE, new State()
                        .WithComponent<Move>()
                        .WithoutComponent<Clamber>()
                        .WithoutComponent<Dive>()
                        .WithoutComponent<Roll>()
                        .WithoutComponent<Slide>())
                     .WithState(SLIDE_STATE, new State()
                        .WithComponent<Slide>()
                        .WithoutComponent<Clamber>()
                        .WithoutComponent<Dive>()
                        .WithoutComponent<Move>()
                        .WithoutComponent<Roll>())
                    .WithState(ROLL_STATE, new State()
                        .WithComponent<Roll>()
                        .WithoutComponent<Clamber>()
                        .WithoutComponent<Dive>()
                        .WithoutComponent<Move>()
                        .WithoutComponent<Slide>())
                    .WithState(DIVE_STATE, new State()
                        .WithComponent<Dive>()
                        .WithoutComponent<Clamber>()
                        .WithoutComponent<Move>()
                        .WithoutComponent<Roll>()
                        .WithoutComponent<Slide>())
                    .WithState(CLAMBER_STATE, new State()
                        .WithComponent<Clamber>()
                        .WithoutComponent<Dive>()
                        .WithoutComponent<Move>()
                        .WithoutComponent<Roll>()
                        .WithoutComponent<Slide>()));
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

        public static AnimationBuilder DiveAnimation()
        {
            var animation = AnimationBuilder.New()
                .InsertFrame(0, new KeyFrame(0))
                .InsertFrame(0.5f, new KeyFrame(MathHelper.PiOver2 + 0.1f))
                .IsRelative(false)
                .AnimatePhysics();

            return animation;
        }
    }
}
