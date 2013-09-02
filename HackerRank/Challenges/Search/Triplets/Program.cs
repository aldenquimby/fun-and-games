using System;
using System.Collections.Generic;
using System.Linq;

namespace Triplets
{
    class Triplet
    {
        public int One { get; set; }
        public int Two { get; set; }
        public int Three { get; set; }
    }

    class Program
    {
        static string GetLine()
        {
            return Console.ReadLine();
        }

        static void Main()
        {
            GetLine(); // don't need n
            var d = GetLine().Split(' ').Select(int.Parse).ToList();
            Console.WriteLine(Triplets(d));
        }

        static long Triplets(IList<int> d)
        {
            var triplets = new Triplet[d.Count];
            var seen = new HashSet<int>();
            for (var i = d.Count - 1; i >= 0; i--)
            {
                triplets[i] = new Triplet();
                if (!seen.Contains(d[i]))
                {
                    triplets[i].One = 1;
                    seen.Add(d[i]);
                }
            }

            // TODO n^2 is too slow, timing out on half of the cases
            for (var i = d.Count - 1; i >= 0; i--)
            {
                for (var j = i + 1; j < d.Count; j++)
                {
                    if (d[i] == d[j])
                    {
                        break;
                    }
                    
                    if (d[i] < d[j])
                    {
                        triplets[i].Two += triplets[j].One;
                        triplets[i].Three += triplets[j].Two;
                    }
                }
            }

            return triplets.Sum(x => (long) x.Three);
        }
    }
}