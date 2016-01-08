using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using MultiPlayer.GameComponents;

namespace MultiPlayer
{
    public class Transform : IKnowsGameObject
    {
        public const float PIXELS_A_METRE = 64.0f;
        public const float METRES_A_PIXEL = 1/PIXELS_A_METRE;

        public Vector2 LocalPosition
        {
            get { return localPosition; }
            set
            {
                localPosition = value;
            }
        }

        public Vector2 Position
        {
            get
            {
                return GameObject.ParentObject is GameObject
                    ? (GameObject.ParentObject as GameObject).Transform.Position + Vector2.Transform(LocalPosition, Matrix.CreateRotationZ((GameObject.ParentObject as GameObject).Transform.Rotation))
                    : LocalPosition;
            }
            set
            {
                LocalPosition = value - (Position - LocalPosition);
            }
        }

        public Vector2 DrawPosition
        {
            get { return Position*PIXELS_A_METRE; }
            set { Position = value*METRES_A_PIXEL; }
        }

        public Vector2 LocalScale = Vector2.One;

        public Vector2 Scale
        {
            get { return GameObject.ParentObject is GameObject ? (GameObject.ParentObject as GameObject).Transform.Scale*LocalScale : LocalScale; }
            set
            {
                if (Scale.X == 0 || Scale.Y == 0) return;

                LocalScale = value/(Scale/LocalScale);
            }
        }

        public float LocalRotation
        {
            get { return localRotation; }
            set
            {
                localRotation = value;
            }
        }

        private Vector2 localPosition;
        private float localRotation;

        public float Rotation
        {
            get { return GameObject.ParentObject is GameObject ? (GameObject.ParentObject as GameObject).Transform.Rotation + LocalRotation : LocalRotation; }
            set { LocalRotation = value - (Rotation - LocalRotation); }
        }

        public Vector2 FacingDirection
        {
            get
            {
                return new Vector2((float)Math.Cos(Rotation - MathHelper.PiOver2), (float)Math.Sin(Rotation - MathHelper.PiOver2));
            }
        }

        public GameObject GameObject { get; set; }
    }
}
