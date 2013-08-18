using System;
using System.Collections.Generic;
using System.Linq;

namespace VerticalSticks
{
    class Program
    {
        static string GetLine()
        {
            return Console.ReadLine();
        }

        static void Main()
        {
            var numCases = int.Parse(GetLine());
            for (var i = 0; i < numCases; i++)
            {
                GetLine(); // don't care about n
                var input = GetLine().Split(' ').Select(int.Parse).OrderBy(x => x).ToList();

                // memoize
                var daCache = new double[input.Count];

                // sum the expected values of each index
                var expectedVal = input.Select((el, idx) => ExpectedVal(input, daCache, idx)).Sum();

                Console.WriteLine(expectedVal.ToString("0.00"));
            }
        }

        private static double ExpectedVal(IList<int> sortedInput, IList<double> cache, int index)
        {
            // an element might appear multiple times, we need the left most
            var idxToUse = 0;
            for (var idx = index; idx >= 0; idx--)
            {
                if (sortedInput[idx] < sortedInput[index])
                {
                    idxToUse = idx + 1;
                    break;
                }
            }

            // if we've already computed it, no need to do work
            if (cache[idxToUse] <= 0)
            {
                var eq = sortedInput.Count - idxToUse - 1;

                var weights = new double[sortedInput.Count + 1];
                var sums = new double[sortedInput.Count + 1];
                var values = new double[sortedInput.Count + 1];

                for (var idx = 1; idx <= sortedInput.Count; idx++)
                {
                    // everything to the right of the the index stays the same
                    if (idx > idxToUse + 1)
                    {
                        values[idx] = values[idx - 1];
                    }
                    else
                    {
                        if (idx > 1)
                        {
                            weights[idx] = weights[idx - 1]*(idxToUse - idx + 2)/(sortedInput.Count - idx + 1);
                            sums[idx] = sums[idx - 1] + weights[idx]*idx*eq/(sortedInput.Count - idx);
                        }
                        else
                        {
                            // first guy has weight 1
                            weights[idx] = 1.0;
                            sums[idx] = (double)eq/(sortedInput.Count - 1);
                        }

                        values[idx] = weights[idx]*idx + sums[idx - 1];
                    }
                }

                cache[idxToUse] = values.Skip(1).Sum()/sortedInput.Count;
            }

            return cache[idxToUse];
        }
    }
}