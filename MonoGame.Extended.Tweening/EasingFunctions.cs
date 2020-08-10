using System;
using Microsoft.Xna.Framework;

namespace MonoGame.Extended.Tweening
{
    public static class EasingFunctions
    {
        public static double Linear(double value) => value;

        public static double CubicIn(double value) => Power.In(value, 3);
        public static double CubicOut(double value) => Power.Out(value, 3);
        public static double CubicInOut(double value) => Power.InOut(value, 3);

        public static double QuadraticIn(double value) => Power.In(value, 2);
        public static double QuadraticOut(double value) => Power.Out(value, 2);
        public static double QuadraticInOut(double value) => Power.InOut(value, 2);

        public static double QuarticIn(double value) => Power.In(value, 4);
        public static double QuarticOut(double value) => Power.Out(value, 4);
        public static double QuarticInOut(double value) => Power.InOut(value, 4);

        public static double QuinticIn(double value) => Power.In(value, 5);
        public static double QuinticOut(double value) => Power.Out(value, 5);
        public static double QuinticInOut(double value) => Power.InOut(value, 5);

        public static double SineIn(double value) => (double) Math.Sin(value*MathHelper.PiOver2 - MathHelper.PiOver2) + 1;
        public static double SineOut(double value) => (double) Math.Sin(value*MathHelper.PiOver2);
        public static double SineInOut(double value) => (double) (Math.Sin(value*MathHelper.Pi - MathHelper.PiOver2) + 1)/2;

        public static double ExponentialIn(double value) => (double) Math.Pow(2, 10*(value - 1));
        public static double ExponentialOut(double value) => Out(value, ExponentialIn);
        public static double ExponentialInOut(double value) => InOut(value, ExponentialIn);

        public static double CircleIn(double value) => (double) -(Math.Sqrt(1 - value * value) - 1);
        public static double CircleOut(double value) => (double) Math.Sqrt(1 - (value - 1) * (value - 1));
        public static double CircleInOut(double value) => (double) (value <= .5 ? (Math.Sqrt(1 - value * value * 4) - 1) / -2 : (Math.Sqrt(1 - (value * 2 - 2) * (value * 2 - 2)) + 1) / 2);

        public static double ElasticIn(double value)
        {
            const int oscillations = 1;
            const double springiness = 3f;
            var e = (Math.Exp(springiness*value) - 1)/(Math.Exp(springiness) - 1);
            return (double) (e*Math.Sin((MathHelper.PiOver2 + MathHelper.TwoPi*oscillations)*value));
        }

        public static double ElasticOut(double value) => Out(value, ElasticIn);
        public static double ElasticInOut(double value) => InOut(value, ElasticIn);

        public static double BackIn(double value)
        {
            const double amplitude = 1f;
            return (double) (Math.Pow(value, 3) - value*amplitude*Math.Sin(value*MathHelper.Pi));
        }

        public static double BackOut(double value) => Out(value, BackIn);
        public static double BackInOut(double value) => InOut(value, BackIn);

        public static double BounceOut(double value) => Out(value, BounceIn);
        public static double BounceInOut(double value) => InOut(value, BounceIn);

        public static double BounceIn(double value)
        {
            const double bounceConst1 = 2.75f;
            var bounceConst2 = (double) Math.Pow(bounceConst1, 2);

            value = 1 - value; //flip x-axis

            if (value < 1/bounceConst1) // big bounce
                return 1f - bounceConst2*value*value;

            if (value < 2/bounceConst1)
                return 1 - (double) (bounceConst2*Math.Pow(value - 1.5f/bounceConst1, 2) + .75);

            if (value < 2.5/bounceConst1)
                return 1 - (double) (bounceConst2*Math.Pow(value - 2.25f/bounceConst1, 2) + .9375);

            //small bounce
            return 1f - (double) (bounceConst2*Math.Pow(value - 2.625f/bounceConst1, 2) + .984375);
        }


        private static double Out(double value, Func<double, double> function)
        {
            return 1 - function(1 - value);
        }

        private static double InOut(double value, Func<double, double> function)
        {
            if (value < 0.5f)
                return 0.5f*function(value*2);

            return 1f - 0.5f*function(2 - value*2);
        }

        private static class Power
        {
            public static double In(double value, int power)
            {
                return (double) Math.Pow(value, power);
            }

            public static double Out(double value, int power)
            {
                var sign = power%2 == 0 ? -1 : 1;
                return (double) (sign*(Math.Pow(value - 1, power) + sign));
            }

            public static double InOut(double s, int power)
            {
                s *= 2;

                if (s < 1)
                    return In(s, power)/2;

                var sign = power%2 == 0 ? -1 : 1;
                return (double) (sign/2.0*(Math.Pow(s - 2, power) + sign*2));
            }
        }
    }
}