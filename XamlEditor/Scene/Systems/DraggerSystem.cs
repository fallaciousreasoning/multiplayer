using System;
using System.Collections.Generic;
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
using XamlEditor.Scene.Components;
using XamlEditor.Scene.Messages;

namespace XamlEditor.Scene.Systems
{
    [HearsMessage(typeof(EnableDraggerMessage))]
    [HearsMessage(typeof(DrawMessage))]
    [HearsMessage(typeof(LateUpdateMessage))]
    public class DraggerSystem : ComponentProcessingSystem<Dragger, Transform>
    {
        private static readonly Texture2D DrawTool = TextureUtil.CreateTexture(1, 1, Color.White);

        protected void Start(Dragger dragger)
        {
            dragger.XDragBounds = new AABB()
            {
                Min = new Vector2(-dragger.Length, dragger.Thickness / 2),
                Max = new Vector2(0)
            };

            dragger.YDragBounds = new AABB()
            {
                Min = new Vector2(dragger.Thickness / 2, -dragger.Length),
                Max = new Vector2(0)
            };
        }

        protected override void Process(IMessage message, Dragger component1, Transform component2)
        {
            //Enable/disable when we need to
            if (message is EnableDraggerMessage)
            {
                var m = message as EnableDraggerMessage;
                m.Target.Get<Dragger>().Enabled = m.State;
            }

            if (!component1.Enabled) return;

            if (message is LateUpdateMessage) Update(component1, component2);
            if (message is DrawMessage) Draw(component1, component2);
        }

        protected void Update(Dragger dragger, Transform transform)
        {
            Start(dragger);

            dragger.XDragBounds.Centre = transform.Position - dragger.XDragBounds.HalfSize;
            dragger.YDragBounds.Centre = transform.Position - dragger.YDragBounds.HalfSize;

            var input = Engine.Scene.Input;

            var mousePos = input.MousePosition;
            mousePos *= Transform.METRES_A_PIXEL;

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
            {
                transform.Position = expectedPosition;
                if (dragger.Drag != null)
                    dragger.Drag.Get<Transform>().Position = expectedPosition;
            }
        }

        protected void Draw(Dragger dragger, Transform transform)
        {
            var size = new Vector2(dragger.Thickness, dragger.Length) * Transform.PIXELS_A_METRE;

            Engine.Scene.SpriteBatch.Draw(DrawTool, transform.Position*Transform.PIXELS_A_METRE, null, dragger.XAxisColor, MathHelper.PiOver2, new Vector2(0.5f, 0), size, SpriteEffects.None, 0);
            Engine.Scene.SpriteBatch.Draw(DrawTool, transform.Position*Transform.PIXELS_A_METRE, null, dragger.YAxisColor, 0, new Vector2(0.5f, 1), size, SpriteEffects.None, 0);
        }
    }
}
