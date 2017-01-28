using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using MultiPlayer.Annotations;

namespace MultiPlayer.Core.Components
{
    public class Transform : IKnowsParent
    {
        public const float PIXELS_A_METRE = 64;
        public const float METRES_A_PIXEL = 1/PIXELS_A_METRE;

        public bool PositionIndependent;
        public bool RotationIndependent;
        public bool ScaleIndependent;

        public Vector2 Position;
        public Vector2 Scale = Vector2.One;
        public float Rotation;
        private Entity parent;

        [EditorIgnore]
        public Vector2 WorldPosition
        {
            get
            {
                return ParentTransform != null && !PositionIndependent
                    ? ParentTransform.WorldPosition + Vector2.Transform(Position, Matrix.CreateRotationZ(ParentTransform.Rotation))
                    : Position;
            }
            set { Position = value - (WorldPosition - Position); }
        }

        [EditorIgnore]
        public Vector2 WorldScale
        {
            get { return Scale*(ScaleIndependent ? Vector2.One : ParentTransform?.WorldScale ?? Vector2.One); }
            set { Scale = value/(ScaleIndependent ? Vector2.One : ParentTransform?.WorldScale ?? Vector2.One); }
        }

        [EditorIgnore]
        public float WorldRotation
        {
            get { return Rotation + (RotationIndependent ? 0 : ParentTransform?.WorldRotation ?? 0); }
            set { Rotation = value - (RotationIndependent ? 0 : ParentTransform?.WorldRotation ?? 0); }
        }

        [EditorIgnore]
        public Transform ParentTransform { get; set; }

        [EditorIgnore]
        public Entity Parent
        {
            get { return parent; }
            set
            {
                parent = value;
                ParentTransform = parent != null && parent.HasComponent<Transform>() ? parent.Get<Transform>() : null;
            }
        }
    }
}
