using System;
using System.Collections.Generic;
using System.Linq;

namespace ConsoleApplication2
{
    public class Program
    {
        public static void Main(string[] args)
        {
            DoTests();
            return;

            var firstLine = Console.ReadLine() ?? "";

            var parts = firstLine.Split(' ');
            var numAstronauts = int.Parse(parts[0]);
            var numPairs = int.Parse(parts[1]);

            var astronautPairs = new List<Tuple<int, int>>();

            for (var i = 0; i < numPairs; i++)
            {
                var nextLine = Console.ReadLine() ?? "";
                parts = nextLine.Split(' ');
                var astronautPair = Tuple.Create(int.Parse(parts[0]), int.Parse(parts[1]));
                astronautPairs.Add(astronautPair);
            }

            var groups = GetAstonautGroups(astronautPairs);
            var numLegalPairs = DoMath(numAstronauts, groups);
            Console.Write(numLegalPairs);
        }

        private static int DoMath(int numAstronauts, List<HashSet<int>> astronautGroups)
        {
            var total = (numAstronauts * (numAstronauts - 1)) / 2;
            foreach (var clump in astronautGroups)
            {
                var c = clump.Count;
                total -= ((c * (c - 1)) / 2);
            }
            return total;
        }
    
        private static List<HashSet<int>> GetAstonautGroups(IEnumerable<Tuple<int, int>> astronautPairs)
        {
            var tmp = new Dictionary<int, HashSet<int>>();

            foreach (var pair in astronautPairs)
            {
                // if they both exist, merge
                if (tmp.ContainsKey(pair.Item1) && tmp.ContainsKey(pair.Item2))
                {
                    // do nothing if already the same
                    if (ReferenceEquals(tmp[pair.Item1], tmp[pair.Item2]))
                    {
                        continue;
                    }

                    var dudesToMove = new List<int>();
                    foreach (var dude in tmp[pair.Item2])
                    {
                        tmp[pair.Item1].Add(dude);
                        dudesToMove.Add(dude);
                    }
                    foreach (var dude in dudesToMove)
                    {
                        tmp[dude] = tmp[pair.Item1];
                    }
                }
                else if (tmp.ContainsKey(pair.Item1))
                {
                    tmp[pair.Item1].Add(pair.Item2);
                    tmp[pair.Item2] = tmp[pair.Item1];
                }
                else if (tmp.ContainsKey(pair.Item2))
                {
                    tmp[pair.Item2].Add(pair.Item1);
                    tmp[pair.Item1] = tmp[pair.Item2];
                }
                else if (pair.Item1 == pair.Item2) // country with 1 dude
                {
                    tmp[pair.Item1] = new HashSet<int> { pair.Item1 };
                }
                else // country with 2 dudes
                {
                    tmp[pair.Item1] = new HashSet<int> { pair.Item1, pair.Item2 };
                    tmp[pair.Item2] = tmp[pair.Item1];
                }
            }

            return tmp.Values.Distinct().ToList();
        }

        private static int NumLegalParis(int a, IEnumerable<Tuple<int, int>> b)
        {
            return DoMath(a, GetAstonautGroups(b));
        }

        private static void DoTests()
        {
            var numAstronauts = 14;
            var astronautPairs = new[]
            {
                Tuple.Create(1, 2), Tuple.Create(2, 3), Tuple.Create(1, 4), // 1, 2, 3, 4
                Tuple.Create(5, 6), // 5, 6
                Tuple.Create(7, 8), Tuple.Create(8, 9), // 7, 8, 9
                Tuple.Create(10, 11), Tuple.Create(11, 12), Tuple.Create(10, 13), Tuple.Create(13, 12), Tuple.Create(13, 0), // 10, 11, 12, 13, 0
            };
            var output = NumLegalParis(numAstronauts, astronautPairs);
            if (output != 71)
                throw new Exception();

            numAstronauts = 9;
            astronautPairs = new[]
            {
                Tuple.Create(1, 2), Tuple.Create(2, 3), // 1, 2, 3
                Tuple.Create(5, 6), Tuple.Create(5, 7), // 5, 6, 7
                Tuple.Create(0, 8), Tuple.Create(8, 9), // 0, 8, 9
            };
            output = NumLegalParis(numAstronauts, astronautPairs);
            if (output != 27)
                throw new Exception();

            numAstronauts = 6;
            astronautPairs = new[]
            {
                Tuple.Create(1, 2), Tuple.Create(2, 3), // 1, 2, 3
                Tuple.Create(5, 6), Tuple.Create(5, 7), // 5, 6, 7
            };
            output = NumLegalParis(numAstronauts, astronautPairs);
            if (output != 9)
                throw new Exception();

            numAstronauts = 6;
            astronautPairs = new[]
            {
                Tuple.Create(0, 0), // 0
                Tuple.Create(1, 2), Tuple.Create(2, 3), // 1, 2, 3
                Tuple.Create(5, 6), // 5, 6
            };
            output = NumLegalParis(numAstronauts, astronautPairs);
            if (output != 11)
                throw new Exception();

            numAstronauts = 6;
            astronautPairs = new[]
            {
                Tuple.Create(0, 0), // 0
                Tuple.Create(1, 1), Tuple.Create(1, 2), Tuple.Create(2, 2), Tuple.Create(2, 3), // 1, 2, 3
                Tuple.Create(5, 6), // 5, 6
            };
            output = NumLegalParis(numAstronauts, astronautPairs);
            if (output != 11)
                throw new Exception();

            output = NumLegalParis(0, new List<Tuple<int, int>>());
            if (output != 0)
                throw new Exception();
        }
    }
}