namespace MultiPlayer.Core.Animation
{
    public class FloatCalculator : ICalculator
    {
        public object Add(object first, object second)
        {
            return (float)first + (float)second;
        }

        public object Sub(object first, object second)
        {
            return (float)first - (float)second;
        }
    }
}
