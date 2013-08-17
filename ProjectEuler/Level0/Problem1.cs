namespace Level0
{
    public static class Problem1
    {
        public static int Solve()
        {
            const int max = 1000;

            var sum = 0;

            for (var i = 3; i < max; i += 3)
            {
                sum += i;
            }

            for (var i = 5; i < max; i += 5)
            {
                if (i%3 != 0) sum += i;
            }

            return sum;
        }
    }
}