using System;
using System.Collections.Generic;

namespace ConsoleApplication2
{
    public class Program
    {
        private static string GetLine()
        {
            return Console.ReadLine();
        }

        public static void Main(string[] args)
        {
            const int size = 5;

            var data = new int[size, size];

            for (var row = 0; row < size; row++)
            {
                var line = GetLine().Split(' ');

                for (var col = 0; col < size; col++)
                {
                    data[row, col] = int.Parse(line[col]);
                }
            }

            // start at 1, 1
            var current = 11;
            var curRow = 0;
            var curCol = 0;

            var visited = new HashSet<int>();
            var cells = new List<int>();

            while (!visited.Contains(current))
            {
                // mark that we visited it
                visited.Add(current);
                cells.Add(current);

                // check if we found treasure
                if (current == data[curRow, curCol])
                {
                    foreach (var cell in cells)
                    {
                        Console.WriteLine(cell/10 + " " + cell%10);
                    }
                    return;
                }

                // go to the next location
                current = data[curRow, curCol];
                curRow = current/10 - 1;
                curCol = current%10 - 1;
            }

            Console.WriteLine("NO TREASURE");
        }
    }
}
