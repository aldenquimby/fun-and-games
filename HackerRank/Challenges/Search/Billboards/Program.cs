using System;
using System.Collections.Generic;
using System.Linq;

namespace Billboards
{
    class Program
    {
        static string GetLine()
        {
            return lines[idxx++];
            return Console.ReadLine();
        }

        static void Main()
        {
            // TODO this is failing a few cases and im not sure why yet

            var firstLine = GetLine().Split();
            var n = int.Parse(firstLine[0]);
            var k = int.Parse(firstLine[1]);

            var profits = new List<int>(n+1);
            for (var i = 0; i < n; i++)
            {
                profits.Add(int.Parse(GetLine()));
            }
            profits.Add(0);

            Console.WriteLine(DoWork(profits, k));
        }

        /// <summary>
        /// optimal[i] is the optimal solution using billboards 0 to i-1.
        /// Case 1. If i is less than or equal to k, we use all billboards. 
        /// Case 2. If we don't use ith billboard, optimal[i] = optimal[i-1].
        /// Case 3. Otherwise, we need to check if using billboard i with
        /// subset of other billboards is better.
        /// </summary>
        static long DoWork(List<int> profits, int k)
        {
            var optimal = Enumerable.Range(0, profits.Count).Select(x => (long)0).ToList();

            var cache = new List<long>(profits.Select(x => (long) x));

            for (var i = 1; i <= k; i++)
            {
                optimal[i] = profits[i] + optimal[i - 1];
            }

            for (var i = 2; i < k; i++)
            {
                cache[k - i + 1] += cache[k - i + 2];
            }

            for (var i = 2; i < k; i++)
            {
                cache[i] += optimal[i - 2];
            }

            for (var i = k + 1; i < profits.Count; i++)
            {
                for (var j = i - k + 1; j < i; j++)
                {
                    cache[j] += profits[i];
                }

                cache[i - 1] += i < 3 ? 0 : optimal[i - 3];

                var prevOptimal = optimal[i - 1];
                var excludingPrev = profits[i] + optimal[i - 2];
                var maxOthers = k == 1 ? 0 : cache.Skip(i - k).Take(k - 1).Max();

                optimal[i] = Math.Max(Math.Max(prevOptimal, excludingPrev), maxOthers);
            }

            return optimal[profits.Count - 1];
        }
        
        private static int idxx;
        private static List<string> lines =
            inp.Split('\n').Select(x => x.Trim()).Where(x => !string.IsNullOrEmpty(x)).ToList(); 
        private const string inp = @"
        6 1
4
7
2
0
8
9";

    }
}
