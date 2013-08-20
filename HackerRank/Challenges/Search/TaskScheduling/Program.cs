using System;
using System.Collections.Generic;
using System.Linq;

namespace TaskScheduling
{
    class Program
    {
        static string ReadLine()
        {
            return lines[idxx++];
            return Console.ReadLine();
        }

        private static void Main()
        {
            // TODO this is timing out on half of the test cases

            var numTasks = int.Parse(ReadLine());
            var ms = new List<int>(numTasks);
            var ds = new List<int>(numTasks);

            var timeSpent = Enumerable.Range(1, numTasks).Select(x => 0).ToList();
            var timeOver = Enumerable.Range(1, numTasks).Select(x => 0).ToList();

            for (var i = 0; i < numTasks; i++)
            {
                var nextLine = ReadLine().Split();
                var d = int.Parse(nextLine[0]);
                var m = int.Parse(nextLine[1]);

                var idx = 0;
                while (idx < ds.Count)
                {
                    if (ds[idx] > d)
                    {
                        break;
                    }
                    idx++;
                }

                ds.Insert(idx, d);
                ms.Insert(idx, m);

                if (idx == 0)
                {
                    timeSpent[0] = m;
                    if (m > d)
                    {
                        timeOver[0] = m - d;
                    }
                    idx = 1;
                }

                while (idx <= i)
                {
                    timeSpent[idx] = timeSpent[idx - 1] + ms[idx];
                    timeOver[idx] = Math.Max(timeOver[idx - 1], timeSpent[idx] - ds[idx]);
                    idx++;
                }

                Console.WriteLine(timeOver[i]);
            }
            Console.ReadKey();
        }

        private static int idxx;
        private static List<string> lines =
            inp.Split('\n').Select(x => x.Trim()).Where(x => !string.IsNullOrEmpty(x)).ToList(); 
        private const string inp = @"
10
47 778
20 794
32 387
157 650
158 363
20 691
68 764
71 541
163 173
17 212";
    }
}