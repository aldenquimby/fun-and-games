using System;
using System.Collections.Generic;

namespace ConsoleApplication1
{
    public class Program
    {
        private static string GetLine()
        {
            return Console.ReadLine();
        }

        public static void Main(string[] args)
        {
            var numberJunctions = int.Parse(GetLine());
            var numberRoads = int.Parse(GetLine());

            // CONSTRAINT: 2 <= N <= 40 1 <= R <= 40
            if (numberJunctions < 2 || numberJunctions > 40 || numberRoads < 1 || numberRoads > 40)
            {
                Console.WriteLine("NO");
                return;
            }

            var roadByJunction = new Dictionary<int, int>();

            for (var i = 0; i < numberRoads; i++)
            {
                var line = GetLine().Split(' ');

                var from = int.Parse(line[0]);
                var to = int.Parse(line[1]);

                // CONSTRAINT: only 1 road can exit a junction
                if (roadByJunction.ContainsKey(from))
                {
                    Console.WriteLine("NO");
                    return;
                }

                roadByJunction[from] = to;
            }

            var visited = new HashSet<int>();

            // cross off junctions that we know won't work
            for (var junction = 0; junction < numberJunctions; junction++)
            {
                // if no roads lead out of this junction, or 
                // if the road leading out of this junction goes back to itself
                if (!roadByJunction.ContainsKey(junction) || roadByJunction[junction] == junction)
                {
                    visited.Add(junction);
                }
            }

            for (var junction = 0; junction < numberJunctions; junction++)
            {
                // if we've already been here, skip it
                if (visited.Contains(junction))
                {
                    continue;
                }

                // mark that we've been here
                visited.Add(junction);

                // now let's see if we can get back to this junction
                var currentJunction = roadByJunction[junction];
                var currentPath = new HashSet<int> { junction, currentJunction };
                while (roadByJunction.ContainsKey(currentJunction) && !visited.Contains(currentJunction))
                {
                    visited.Add(currentJunction);
                    currentJunction = roadByJunction[currentJunction];

                    if (currentPath.Contains(currentJunction))
                    {
                        Console.WriteLine("YES");
                        return;
                    }

                    currentPath.Add(currentJunction);
                }
            }

            // checked all junctions, no loops
            Console.WriteLine("NO");
        }
    }
}
