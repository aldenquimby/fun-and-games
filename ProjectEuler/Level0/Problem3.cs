using System.Collections.Generic;
using System.Linq;

namespace Level0
{
    public static class Problem3
    {
        public static int Solve()
        {
            var a = 600851475143;

            // key is prime factor, value is number of times
            var primeFactors = new Dictionary<int, int>();

            for (var b = 2; a > 1; b++)
            {
                if (a%b == 0)
                {
                    var x = 0;
                    while (a%b == 0)
                    {
                        a /= b;
                        x++;
                    }
                    primeFactors.Add(b, x);
                }
            }

            return primeFactors.Keys.Last();
        }
    }
}