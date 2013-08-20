namespace Level0
{
    public static class Problem5
    {
        public static int Solve()
        {
            return SmallestMultiple(20);
        }

        static int SmallestMultiple(int max)
        {
            for (var i = max; ; i += max)
            {
                if (IsDivisible(i, max))
                {
                    return i;
                }
            }
        }

        static bool IsDivisible(int num, int start)
        {
            if (start == 1)
            {
                return true;
            }

            return num%start == 0 && IsDivisible(num, start - 1);
        }
    }
}