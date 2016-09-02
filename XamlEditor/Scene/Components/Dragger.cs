using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MultiPlayer;
using MultiPlayer.Annotations;
using MultiPlayer.Core;

namespace XamlEditor.Scene.Components
{
    [EditorIgnore]
    public class Dragger
    {
        public bool Enabled { get; set; }

        public float Thickness { get; set; } = 0.2f;
        public float Length { get; set; } = 2f;

        public Color XAxisColor = Color.Green;
        public Color YAxisColor = Color.Blue;

        public AABB XDragBounds;
        public AABB YDragBounds;

        public bool XDragging;
        public bool YDragging;

        public Vector2 MouseStart;
        public Vector2 StartPos;
    }
}
