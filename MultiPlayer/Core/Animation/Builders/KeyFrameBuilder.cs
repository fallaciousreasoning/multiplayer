using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace MultiPlayer.Core.Animation.Components
{
    public class KeyFrameBuilder
    {
        public const string ROTATION = "rotation";
        public const string POSITION = "position";
        public const string SCALE = "scale";

        private readonly Dictionary<string, Func<object>> valueGenerators = new Dictionary<string, Func<object>>();

        public bool HasSeries(IEnumerable<string> series)
        {
            return series.All(valueGenerators.ContainsKey);
        }

        public KeyFrameBuilder WithSeries(string series, object value)
        {
            return WithSeries(series, () => value);
        }

        public KeyFrameBuilder WithSeries(string series, Func<object> getter)
        {
            valueGenerators.Add(series, getter);
            return this;
        }

        public KeyFrameBuilder WithRotation(float rotation)
        {
            return WithSeries(ROTATION, rotation);
        }

        public KeyFrameBuilder WithScale(Vector2 scale)
        {
            return WithSeries(SCALE, scale);
        }

        public KeyFrameBuilder WithPosition(Vector2 position)
        {
            return WithSeries(POSITION, position);
        }

        public KeyFrame Build()
        {
            var values = new Dictionary<string, object>();
            foreach (var key in valueGenerators.Keys)
                values.Add(key, valueGenerators[key]());

            return new KeyFrame(values);
        }

        public static KeyFrameBuilder New()
        {
            return new KeyFrameBuilder();
        }
    }
}
