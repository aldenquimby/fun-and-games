using System;
using System.Collections.Generic;
using System.Linq;

namespace ConsoleApplication3
{
    public class Program
    {
        private static string GetLine()
        {
            return read[idx++];
            return Console.ReadLine();
        }

        public static void Main(string[] args)
        {
            var firstLine = GetLine().Split(' ');
            var secondLine = GetLine().Split(' ');

            var n = int.Parse(firstLine[0]);
            var k = int.Parse(firstLine[1]);
            var m = int.Parse(firstLine[2]);

            var d = new List<int>(n);

            for (var i = 0; i < n; i++)
            {
                d.Add(int.Parse(secondLine[i]));
            }

            // TASK 1: chose k numbers from d with largest sum less than m, output that sum
            var z = SubsetSum(d, k, m);
            Console.WriteLine(z);

            // TASK 2: find the number of possible partitions of z, modulo 1000000007
            var p = PartitionFunction(z, 1000000007);
            Console.WriteLine(p);
        }

        private static int PartitionFunction(int num, int mod)
        {
            var intermediate = IntermediatePartition(num, mod);

            var p = 1;

            var kMax = Math.Floor(0.5*num);
            for (var k = 1; k <= kMax; k++)
            {
                p = (p + intermediate[k, num - k])%mod;
            }

            return p;
        }

        private static int[,] IntermediatePartition(int num, int mod)
        {
            var intermediate = new int[num + 1, num + 1];
            
            for (var i = 1; i <= num; i++)
            {
                intermediate[i, i] = 1;
            }

            for (var n = 2; n <= num; n++)
            {
                for (var k = n - 1; k > 0; k--)
                {
                    intermediate[k, n] = (intermediate[k + 1, n] + intermediate[k, n - k])%mod;
                }
            }

            return intermediate;
        }

        private static int SubsetSum(List<int> items, int numItemsToPick, int maxSum)
        {
            var sum = items.OrderByDescending(x => x).Take(numItemsToPick).Sum() + 1;
            if (maxSum > sum)
            {
                maxSum = sum;
            }

            // dp[i,j] is true if a subset of length i summing to j exists
            var dp = new bool[numItemsToPick + 1, maxSum + 1];
            dp[0, 0] = true;
            
            for (var i = 0; i < items.Count; i++)
            {
                for (var s = 0; s < numItemsToPick; s++)
                {
                    for (var j = maxSum; j >= 0; j--)
                    {
                        if (dp[s, j] && items[i] + j < maxSum)
                        {
                            dp[s + 1, j + items[i]] = true;
                        }
                    }
                }
            }

            for (var j = maxSum; j >= 0; j--)
            {
                if (dp[numItemsToPick, j])
                {
                    return j;
                }
            }

            return 0;
        }

        private static int idx;
        private static List<string> read =
            input_hard.Split('\n').Select(x => x.Trim()).Where(x => !string.IsNullOrEmpty(x)).ToList(); 
        private const string input = @"
3 2 5 
2 1 3
";
        private const string input_hard = @"
39 36 1209
10 30 45 25 35 45 45 50 15 15 15 5 45 5 45 15 15 35 5 15 45 15 35 5 15 25 45 15 25 5 45 35 35 5 15 45 45 30 35
";
    }
}
