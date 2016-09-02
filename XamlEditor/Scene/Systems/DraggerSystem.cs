using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MultiPlayer;
using MultiPlayer.Annotations;
using MultiPlayer.Core;
using MultiPlayer.Core.Components;
using MultiPlayer.Core.Input;
using MultiPlayer.Core.Messaging;
using MultiPlayer.Core.Systems;
using XamlEditor.Extensions;
using XamlEditor.Scene.Components;
using XamlEditor.Scene.Messages;

namespace XamlEditor.Scene.Systems
{
    [HearsMessage(typeof(SelectMessage))]
    [HearsMessage(typeof(DrawMessage))]
    [HearsMessage(typeof(LateUpdateMessage))]
    public class DraggerSystem : ComponentProcessingSystem<Dragger, Transform>
    {
        private static readonly Texture2D DrawTool = TextureUtil.CreateTexture(1, 1, Color.White);
        private Dragger selectedDragger;

        protected override void OnEntityAdded(Entity entity)
        {
            Start(entity.Get<Dragger>());
            base.OnEntityAdded(entity);
        }

        protected override void Start()
        {
            base.Start();

            Family.Entities.Foreach(e => Start(e.Get<Dragger>()));
        }

        protected void Start(Dragger dragger)
        {
            dragger.XDragBounds = new AABB()
            {
                Min = new Vector2(-dragger.Length, -dragger.Thickness / 2),
                Max = new Vector2(0, dragger.Thickness/2)
            };

            dragger.YDragBounds = new AABB()
            {
                Min = new Vector2(-dragger.Thickness / 2, -dragger.Length),
                Max = new Vector2(dragger.Thickness/2, 0)
            };
        }

        public override void RecieveMessage(IMessage message)
        {
            base.RecieveMessage(message);

            if (message is DrawMessage) Draw();
        }

        protected override void Process(IMessage message, Dragger component1, Transform component2)
        {
            //Enable/disable when we need to
            if (message is SelectMessage)
            {
                var m = message as SelectMessage;

                if (selectedDragger != null) selectedDragger.Enabled = false;

                selectedDragger = m.Target.Get<Dragger>();
                selectedDragger.Enabled = true;
            }

            if (!component1.Enabled) return;

            if (message is LateUpdateMessage) Update(component1, component2);
        }

        protected void Update(Dragger dragger, Transform transform)
        {
            dragger.XDragBounds.Centre = transform.Position;
            dragger.YDragBounds.Centre = transform.Position;

            var input = Engine.Scene.Input;

            var mousePos = input.MousePosition;

            if (input.IsDown(MouseButton.Left) && !dragger.XDragging && !dragger.YDragging)
            {
                if (dragger.XDragBounds.Contains(mousePos))
                    dragger.XDragging = true;
                if (dragger.YDragBounds.Contains(mousePos))
                    dragger.YDragging = true;

                dragger.MouseStart = mousePos;
                dragger.StartPos = transform.Position;
            }

            if (input.IsUp(MouseButton.Left))
            {
                dragger.XDragging = false;
                dragger.YDragging = false;
                dragger.MouseStart = Vector2.Zero;
                dragger.StartPos = transform.Position;
            }

            var expectedPosition = dragger.StartPos + (mousePos - dragger.MouseStart);
            if (!dragger.XDragging)
                expectedPosition.X = dragger.StartPos.X;
            if (!dragger.YDragging)
                expectedPosition.Y = dragger.StartPos.Y;

            if (dragger.XDragging || dragger.YDragging)
                transform.Position = expectedPosition;
        }

        protected void Draw()
        {
            Engine.Scene.SpriteBatch.Begin(0, null, null, null, null, null, Engine.Systems.Get<CameraSystem>().CameraWorld);

            foreach (var entity in Family.Entities)
            {
                var dragger = entity.Get<Dragger>();
                if (!dragger.Enabled) continue;

                var transform = entity.Get<Transform>();

                var size = new Vector2(dragger.Thickness, dragger.Length)*Transform.PIXELS_A_METRE;

                Engine.Scene.SpriteBatch.Draw(DrawTool, transform.Position*Transform.PIXELS_A_METRE, null,
                    dragger.XAxisColor, MathHelper.PiOver2, new Vector2(0.5f, 0), size, SpriteEffects.None, 0);
                Engine.Scene.SpriteBatch.Draw(DrawTool, transform.Position*Transform.PIXELS_A_METRE, null,
                    dragger.YAxisColor, 0, new Vector2(0.5f, 1), size, SpriteEffects.None, 0);
            }

            Engine.Scene.SpriteBatch.End();
        }
    }
}
