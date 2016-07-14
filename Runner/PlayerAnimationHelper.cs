using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Runner
{
    public enum PlayerAnimation
    {
        Roll,
        SlideDown,
        SlideUp,
        Clamber,
    }

    public enum Direction
    {
        Left,
        Right
    }

    public class Animations
    {
        public static string Name(PlayerAnimation animation, Direction direction)
        {
            var a = ToLowerUnderscoreCase(animation.ToString());
            var d = ToLowerUnderscoreCase(direction.ToString());

            return $"{a}_{d}";
        }
        
        private static string ToLowerUnderscoreCase(string s)
        {
            var result = new StringBuilder();

            foreach (var c in s)
            {
                if (char.IsUpper(c) && result.Length > 0)
                    result.Append('_');
                result.Append(char.ToLower(c));
            }

            return result.ToString();
        }
    }
}
