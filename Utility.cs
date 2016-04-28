using System;

namespace ConsoleDA
{
    public static class Utility
    {
        private static Random random;

        static Utility()
        {
            random = new Random();
        }

        public static int Random()
        {
            return random.Next();
        }
        public static int Random(int maxValue)
        {
            return random.Next(maxValue);
        }
        public static int Random(int minValue, int maxValue)
        {
            return random.Next(minValue, maxValue);
        }
    }
}
