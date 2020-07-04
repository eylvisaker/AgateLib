using System;
using System.Collections.Generic;
using System.Text;

namespace AgateLib.Display
{
    /// <summary>
    /// A set of easing functions.
    /// </summary>
    public static class Ease
    {
        public static float SmoothStart2(float x) => x * x;
        public static float SmoothStart3(float x) => x * x * x;
        public static float SmoothStart4(float x) => x * x * x * x;
        public static float SmoothStart5(float x) => x * x * x * x * x;
        public static float SmoothStart6(float x) => x * x * x * x * x * x;

        public static float SmoothStop2(float x) => 1 - (1 - x) * (1 - x);
        public static float SmoothStop3(float x) => 1 - (1 - x) * (1 - x) * (1 - x);
        public static float SmoothStop4(float x) => 1 - (1 - x) * (1 - x) * (1 - x) * (1 - x);
        public static float SmoothStop5(float x) => 1 - (1 - x) * (1 - x) * (1 - x) * (1 - x) * (1 - x);
        public static float SmoothStop6(float x) => 1 - (1 - x) * (1 - x) * (1 - x) * (1 - x) * (1 - x) * (1 - x);
        public static float SmoothStop7(float x) => 1 - (1 - x) * (1 - x) * (1 - x) * (1 - x) * (1 - x) * (1 - x) * (1 - x);

        /// <summary>
        /// Interpolates from the left function at x = 0 to the right function at x = 1
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static Func<float, float> Interpolate(Func<float, float> left, Func<float, float> right)
        {
            return (float x) => (1 - x) * left(x) + x * right(x);
        }
    }
}
