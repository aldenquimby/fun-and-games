using System;
using System.Collections.Generic;
using System.Linq;

namespace KDifference
{
    class Program
    {
        static string ReadLine()
        {
            return Console.ReadLine();
        }

        static void Main()
        {
            var firstLine = ReadLine().Split(' ');
            var diff = int.Parse(firstLine[1]);
            var nums = ReadLine().Split(' ').Select(int.Parse).ToList();
            Console.WriteLine(KDiff(nums, diff));
        }

        static int KDiff(List<int> input, int diff)
        {
            input.Sort();

            var count = 0;

            for (var i = input.Count - 1; i > 0; i--)
            {
                for (var j = i - 1; j >= 0; j--)
                {
                    if (input[i] - input[j] == diff)
                    {
                        count++;
                        j = 0;
                    }
                }
            }

            return count;
        }
    }
}
