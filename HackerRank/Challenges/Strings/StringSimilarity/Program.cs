using System;

namespace StringSimilarity
{
    internal class Program
    {
        private static string ReadLine()
        {
            return Console.ReadLine();
        }

        private static void Main()
        {
            var t = int.Parse(ReadLine());
            for (var i = 0; i < t; i++)
            {
                Console.WriteLine(SimilaritiesWithSuffixes(ReadLine()));
            }
        }

        private static int SimilaritiesWithSuffixes(string input)
        {
            var sum = 0;
            for (var i = 0; i < input.Length; i++)
            {
                int j;
                for (j = 0; j + i < input.Length; j++)
                {
                    if (input[j + i] != input[j])
                    {
                        break;
                    }
                }
                sum += j;
            }
            return sum;
        }
    }
}
