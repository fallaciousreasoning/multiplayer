using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace MultiPlayer.GameComponents.Animation
{
    public class KeyFrame
    {
        public const string POSITION_NAME = "position";
        public const string SCALE_NAME = "scale";
        public const string ROTATION_NAME = "rotation";

        public List<string> Series { get; } = new List<string>();
        public Dictionary<string, object> Values { get; } = new Dictionary<string, object>();

        private readonly Dictionary<string, IAnimationFrame> frames = new Dictionary<string, IAnimationFrame>();

        public KeyFrame(Vector2 position, float rotation, Vector2 scale)
        {
            SetSeries(POSITION_NAME, new Vector2Frame(position));
            SetSeries(SCALE_NAME, new Vector2Frame(scale));
            SetSeries(ROTATION_NAME, new FloatFrame(rotation));
        }

        public KeyFrame(Vector2 position, float rotation)
        {
            SetSeries(POSITION_NAME, new Vector2Frame(position));
            SetSeries(ROTATION_NAME, new FloatFrame(rotation));
        }

        public KeyFrame(Vector2 position)
        {
            SetSeries(POSITION_NAME, new Vector2Frame(position));
        }

        public KeyFrame(float rotation)
        {
            SetSeries(ROTATION_NAME, new FloatFrame(rotation));
        }

        public IAnimationFrame GetFrame(string seriesName)
        {
            if (frames.ContainsKey(seriesName)) return frames[seriesName];
            return null;
        }

        public void SetSeries(string seriesName, IAnimationFrame frame)
        {
            Series.Add(seriesName);
            frames.Add(seriesName, frame);  
            Values.Add(seriesName, frame.FrameValue); 
        }

        public bool ContainsSeries(string seriesName)
        {
            return frames.ContainsKey(seriesName);
        }

        public bool ContainsAll(IEnumerable<string> seriesNames)
        {
            return seriesNames.All(Series.Contains);
        }
    }
}
