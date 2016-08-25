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

        public bool IgnoreParentRotation { get; set; }
        public bool IgnoreParentTranslation { get; set; }
        public bool IgnoreParentScale { get; set; }

        private Vector2 localPosition;

        public Vector2 Position
        {
            get
            {
                return (GameObject != null && GameObject.ParentObject is GameObject && !IgnoreParentTranslation)
                    ? (GameObject.ParentObject as GameObject).Transform.Position + Vector2.Transform(localPosition, Matrix.CreateRotationZ((GameObject.ParentObject as GameObject).Transform.Rotation))
                    : localPosition;
            }
            set
            {
                localPosition = value - (Position - localPosition);
            }
        }

        public Vector2 DrawPosition => Position*PIXELS_A_METRE;

        private Vector2 localScale = Vector2.One;

        public Vector2 Scale
        {
            get
            {
                return (GameObject != null && GameObject.ParentObject is GameObject && !IgnoreParentScale)
                    ? (GameObject.ParentObject as GameObject).Transform.Scale*localScale
                    : localScale;
            }
            set
            {
                var scale = Scale;
                if (value.X != 0 && scale.X != 0) localScale.X = value.X/(scale.X/localScale.X);
                if (value.Y != 0 && scale.Y != 0) localScale.Y = value.Y/(scale.Y/localScale.Y);
            }
        }

        private float localRotation;

        public float Rotation
        {
            get
            {
                return (GameObject != null && GameObject.ParentObject is GameObject && !IgnoreParentRotation)
                    ? (GameObject.ParentObject as GameObject).Transform.Rotation + localRotation
                    : localRotation;
            }
            set { localRotation = value - (Rotation - localRotation); }
        }

        public Vector2 FacingDirection
        {
            get
            {
                return new Vector2((float)Math.Cos(Rotation - MathHelper.PiOver2), (float)Math.Sin(Rotation - MathHelper.PiOver2));
            }
        }

        public GameObject GameObject { get; set; }

        public void Set(Transform transform)
        {
            Position = transform.Position;
            Rotation = transform.Rotation;
            Scale = transform.Scale;
        }
    }
}
