using System;

namespace Pat.Api
{
    public static class Equality
    {
        public const double Epsilon = 0.00000001;

        public static bool AreEqual(double d1, double d2)
        {
            return Math.Abs(d1 - d2) < Epsilon;
        }

        public static bool AreLessOrEqual(double d1, double d2)
        {
            return d1 < d2 || AreEqual(d1, d2);
        }
        
        public static bool AreMoreOrEqual(double d1, double d2)
        {
            return d1 > d2 || AreEqual(d1, d2);
        }

        public static bool Less(double d1, double d2)
        {
            return d1 < d2 && !AreEqual(d1, d2);
        }
    }
}