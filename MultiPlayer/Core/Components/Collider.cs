using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FarseerPhysics.Collision.Shapes;
using FarseerPhysics.Common;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Dynamics.Contacts;
using Microsoft.Xna.Framework;

namespace MultiPlayer.Core.Components
{
    public class Collider
    {
        public bool FixedRotation;
        public bool IsTrigger;
        public BodyType BodyType = BodyType.Static;

        public float Density;

        public Vertices Shape;

        internal Body Body { get; set; }
        internal bool WasTrigger { get; set; }
        internal Vector2 LastScale { get; set; } = Vector2.One;

        public Vector2 Velocity { get { return Body.LinearVelocity; } set { Body.LinearVelocity = value; } }
        public float AngularVelocity { get { return Body.AngularVelocity; } set { Body.AngularVelocity = value; } }
    }
}
