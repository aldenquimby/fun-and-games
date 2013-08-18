using System;
using System.Collections.Generic;

namespace ConsoleApplication3
{
    public static class CoolProblems
    {
        public static int KnapsackProblem(int maxCapacity, List<int> weights, List<int> values = null)
        {
            values = values ?? weights;

            // result[i,j] is max sum using at most i items with weight less than or equal to j
            var result = new int[weights.Count + 1, maxCapacity + 1];

            for (var i = 1; i <= weights.Count; i++)
            {
                for (var w = 0; w <= maxCapacity; w++)
                {
                    var wi = weights[i - 1];
                    var vi = values[i - 1];

                    if (wi <= w) // item can be included
                    {
                        result[i, w] = Math.Max(result[i - 1, w], vi + result[i - 1, w - wi]);
                    }
                    else // item can't be included
                    {
                        result[i, w] = result[i - 1, w];
                    }
                }
            }

            return result[weights.Count, maxCapacity];
        }
    }
}