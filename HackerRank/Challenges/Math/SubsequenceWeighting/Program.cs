using System;
using System.Collections.Generic;
using System.Linq;

namespace SubsequenceWeighting
{
    class Element
    {
        public int Num { get; set; }

        // downloaded test case 2 output, and answer was larger than int.MaxValue, so using long here
        public long Weight { get; set; }
    }

    class Program
    {
        static string GetLine()
        {
            return Console.ReadLine();
        }

        static void Main()
        {
            var numTests = int.Parse(GetLine());

            for (var i = 0; i < numTests; i++)
            {
                GetLine(); // don't need total
                
                var a = GetLine().Split().Select(int.Parse).ToList();
                var w = GetLine().Split().Select(long.Parse).ToList();
                
                Console.WriteLine(MaxWeight(a, w));
            }
        }

        static long MaxWeight(IList<int> nums, IList<long> weights)
        {
            var elements = new List<Element> {new Element {Num = 0, Weight = 0}};

            for (var i = 0; i < nums.Count; i++)
            {
                var idx = GetPlaceForNum(elements, nums[i]);
            
                // weight of subsequence is sum of element weights
                var element = new Element
                {
                    Num = nums[i],
                    Weight = weights[i] + elements[idx - 1].Weight,
                };

                elements.Insert(idx, element);

                // remove everyone to the right with smaller weights
                while (idx < elements.Count - 1 && elements[idx + 1].Weight < elements[idx].Weight)
                {
                    elements.RemoveAt(idx + 1);
                }
            }

            return elements[elements.Count - 1].Weight;
        }

        static int GetPlaceForNum(List<Element> elements, int num)
        {
            // binary search to find range where num can go
            var lowerBoundIdx = 0;
            var upperBoundIdx = elements.Count - 1;
            while (upperBoundIdx > lowerBoundIdx + 1)
            {
                var mid = (lowerBoundIdx + upperBoundIdx) / 2;
                if (num > elements[mid].Num)
                {
                    lowerBoundIdx = mid;
                }
                else
                {
                    upperBoundIdx = mid;
                }
            }

            // find index for num from range
            var idx = upperBoundIdx + 1;
            if (num < elements[lowerBoundIdx].Num)
            {
                idx = lowerBoundIdx;
            }
            else if (num < elements[upperBoundIdx].Num)
            {
                idx = upperBoundIdx;
            }

            // move to left most equal num
            while (idx > 0 && num == elements[idx - 1].Num)
            {
                idx--;
            }

            return idx;
        }
    }
}