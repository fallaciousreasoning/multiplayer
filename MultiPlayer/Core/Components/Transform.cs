using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace MultiPlayer.Core.Components
{
    public class Transform
    {
        public const float PIXELS_A_METRE = 64;
        public const float METRES_A_PIXEL = 1/PIXELS_A_METRE;

        public Transform Parent;

        public Vector2 Position;
        public Vector2 Scale = Vector2.One;
        public float Rotation;

        public Vector2 WorldPosition
            => Parent != null
                ? Parent.WorldPosition + Vector2.Transform(Position, Matrix.CreateRotationZ(Parent.Rotation))
                : Position;

        public Vector2 WorldScale => Scale*(Parent?.WorldScale ?? Vector2.One);

        public float WorldRotation => Rotation + (Parent?.WorldRotation ?? 0);
    }
}
