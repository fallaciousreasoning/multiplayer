using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MultiPlayer.GameComponents
{
    public class AABB
    {
        public Vector2 Min { get; set; }
        public Vector2 Max { get; set; }

        public Vector2 Centre
        {
            get
            {
                return (Max + Min)*0.5f;
            }
            set
            {
                var width = Width;
                var height = Height;
                var halfSize = new Vector2(width, height)*0.5f;

                Min = value - halfSize;
                Max = value + halfSize;
            }
        }

        public float Width
        {
            get
            {
                return Max.X - Min.X;
            }
            set
            {
                var centre = Centre;
                Min = new Vector2(centre.X -  value* 0.5f, Min.Y);
                Max = new Vector2(centre.X + value * 0.5f, Min.Y);
            }
        }

        public float Height
        {
            get { return Max.Y - Min.Y; }
            set
            {
                var centre = Centre;
                Min = new Vector2(Min.X, centre.Y - value*0.5f);
                Max = new Vector2(Max.X, centre.Y + value * 0.5f);
            }
        }

        public float Area
        {
            get { return Width*Height; }
        }

        public Vector2 HalfSize { get { return new Vector2(Width, Height)*0.5f; } }

        public bool Contains(AABB other)
        {
            return other.Min.X > Min.X && other.Max.X < Max.X && other.Min.Y > Min.Y && other.Max.Y < Max.Y;
        }

        public bool Contains(Vector2 point)
        {
            return point.X > Min.X && point.Y > Min.Y && point.X < Max.X && point.Y < Max.Y;
        }

        public bool Intersects(AABB other)
        {
            return !(Min.X > other.Max.X || Min.Y > other.Max.Y || Max.X < other.Min.X || Max.Y < other.Min.Y);
        }

        public static AABB Scale(AABB input, float scale)
        {
            return new AABB() {Min = input.Min * scale, Max = input.Max * scale};
        }

        public static AABB Translate(AABB input, Vector2 by)
        {
            return new AABB() {Min = input.Min + by , Max = input.Max + by};
        }

        public static AABB FromTexture(Texture2D texture, float scalingFactor = Transform.METRES_A_PIXEL)
        {
            var halfSize = new Vector2(texture.Width, texture.Height)*0.5f*scalingFactor;
            return new AABB() {Min = -halfSize, Max = halfSize};
        }
    }
}
