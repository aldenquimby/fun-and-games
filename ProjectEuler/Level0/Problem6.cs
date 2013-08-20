using System;
using System.Linq;

namespace Level0
{
    public static class Problem6
    {
        public static int Solve()
        {
            return SumOfSquaresDiff(100);
        }

        static int SumOfSquaresDiff(int max)
        {
            var sumOfSquare = Enumerable.Range(1, max).Select(x => x*x).Sum();
            var sum = Enumerable.Range(1, max).Sum();
            var squareOfSum = sum*sum;
            return squareOfSum - sumOfSquare;
        }
    }
}