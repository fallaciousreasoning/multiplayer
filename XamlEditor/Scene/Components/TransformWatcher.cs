using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using MultiPlayer.Annotations;

namespace XamlEditor.Scene.Components
{
    [EditorIgnore]
    public class TransformWatcher
    {
        public Vector2 Position;
        public Vector2 Scale;
        public float Rotation;
    }
}
