using System;
using System.Linq;

namespace Level0
{
    public static class Problem7
    {
        public static int Solve()
        {
            return NthPrime(10001);
        }

        static int NthPrime(int n)
        {
            var seive = Enumerable.Range(1, n*n).Select(x => true).ToList();
            seive[0] = seive[1] = false;

            var primesSoFar = 0;

            for (var i = 2; i < seive.Count; i++)
            {
                if (seive[i])
                {
                    if (++primesSoFar == n)
                    {
                        return i;
                    }

                    for (var j = i * 2; j < seive.Count; j += i)
                    {
                        seive[j] = false;
                    }
                }
            }

            throw new Exception("Ruh roh, seive wasn't large enough to find nth prime.");
        }
    }
}