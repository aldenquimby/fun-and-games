namespace Level0
{
    public static class Problem2
    {
        public static int Solve()
        {
            const int max = 4000000;

            var sum = 0;

            var x = 1;
            var y = 2;

            while (y < max)
            {
                if (x % 2 == 0)
                {
                    sum += x;
                }

                if (y % 2 == 0)
                {
                    sum += y;
                }

                x = x + y;
                y = x + y;
            }

            return sum;
        }
    }
}