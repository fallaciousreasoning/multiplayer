using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MultiPlayer.GameComponents
{
    public class ParticleEmitter : IUpdateable, IStartable, IKnowsGameObject, IDrawable
    {
        private readonly LinkedList<Particle> particles = new LinkedList<Particle>();
        private Texture2D texture = TextureUtil.CreateTexture(16, 16, Color.Orange);

        public GameObject GameObject { get; set; }

        public float EmitRate { get; set; } = 0.05f;
        private float tillEmit;

        public void Start()
        {
            tillEmit = EmitRate;
        }

        public void Update(float step)
        {
            var particleNode = particles.First;
            while (particleNode != null)
            {
                var particle = particleNode.Value;

                particle.Life -= step;

                particle.Position += step * particle.Velocity;
                particle.Rotation += step * particle.AngularVelocity;
                particle.Scale += step * particle.ScaleVelocity;

                //particle.Velocity -= particle.Velocity * particle.LinearDamping * step;
                //particle.AngularVelocity -= particle.AngularVelocity * particle.AngularDamping*step;
                //particle.ScaleVelocity -= particle.ScaleVelocity * particle.ScaleDamping*step;

                if (particleNode.Value.Life <= 0)
                    particles.Remove(particleNode);
                
                particleNode = particleNode.Next;
            }

            tillEmit -= step;
            if (tillEmit < 0)
                EmitParticle();
        }

        private void EmitParticle()
        {
            var scale = RandomUtil.RandomVector2(new Vector2(0.5f), new Vector2(2));
            var rotation = RandomUtil.RandomFloat(MathHelper.TwoPi);
            var velocity = (new Vector2(RandomUtil.RandomFloat(), RandomUtil.RandomFloat()) - new Vector2(0.5f)) * 5;
            var particle = new Particle(texture, Color.White, GameObject.Transform.Position, scale, rotation, velocity, Vector2.Zero, RandomUtil.RandomFloat()*5, 0.9f, 0, 0.5f, 1);

            particles.AddLast(particle);
        }

        public void Draw()
        {
           foreach (var p in particles) p.Draw();
        }

        class Particle
        {
            public Texture2D Texture;
            private Vector2 origin;
            public Color Tint;

            public Vector2 Position;
            public Vector2 Scale;
            public float Rotation;

            public Vector2 Velocity;
            public Vector2 ScaleVelocity;
            public float AngularVelocity;

            public float LinearDamping;
            public float ScaleDamping;
            public float AngularDamping;

            public float Life;

            public Particle(Texture2D texture, Color tint, Vector2 position, Vector2 scale, float rotation, Vector2 velocity, Vector2 scaleVelocity, float angularVelocity, float linearDamping, float scaleDamping, float angularDamping, float life)
            {
                Texture = texture;
                origin = new Vector2(texture.Width, texture.Height) * 0.5f;
                Tint = tint;

                Position = position;
                Rotation = rotation;
                Scale = scale;

                Velocity = velocity;
                ScaleVelocity = scaleVelocity;
                AngularVelocity = angularDamping;

                LinearDamping = linearDamping;
                ScaleDamping = scaleDamping;
                AngularDamping = angularDamping;

                Life = life;
            }

            public void Update(float step)
            {
            }

            public void Draw()
            {
                Scene.ActiveScene.SpriteBatch.Draw(Texture, Position*Transform.PIXELS_A_METRE, null, Tint, Rotation, origin, Scale, SpriteEffects.None, 0);
            }
        }
    }
}
