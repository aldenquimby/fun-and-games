using System;
using System.Collections.Generic;
using System.Linq;

namespace ConsoleApplication5
{
    public class Result
    {
        public Result()
        {
            Windows = new List<int>();
        }

        public List<int> Windows { get; set; }
        public double Price { get; set; }
        public Dictionary<string, int> WindowBacks { get; set; }
        public int MaxWindowTaken { get; set; }
    }

    public class Program
    {
        private static string GetLine()
        {
            return Console.ReadLine();
        }

        private static Dictionary<string, int> PriceByPlace = new Dictionary<string, int>(); 

        private static void Main(string[] args)
        {
            RunTests();
            return;
            var firstLine = GetLine().Split(' ');

            var numPeople = int.Parse(firstLine[0]);
            var numWindows = int.Parse(firstLine[1]);
            var numDests = int.Parse(firstLine[2]);

            var destinations = new List<string>(numPeople);

            for (var i = 0; i < numDests; i++)
            {
                var line = GetLine().Split(' ');
                var place = line[0];
                var price = int.Parse(line[1]);
                PriceByPlace[place] = price;
            }

            for (var i = 0; i < numPeople; i++)
            {
                destinations.Add(GetLine());
            }

            var result = GetResult(numWindows, destinations);

            Console.WriteLine(Math.Round(result.Price, 3));
            foreach (var window in result.Windows)
            {
                Console.WriteLine(window);
            }
        }

        private static Result DoIt(int numWindows, List<string> lineOfPeople)
        {
            if (lineOfPeople.Count == 0 || numWindows == 0)
            {
                return new Result();
            }

            var dp = new Result[lineOfPeople.Count];

            dp[0] = new Result
            {
                Price = PriceByPlace[lineOfPeople[0]],
                Windows = new List<int> {1},
                WindowBacks = new Dictionary<string, int> {{lineOfPeople[0], 1}},
                MaxWindowTaken = 1,
            };

            for (var i = 1; i < lineOfPeople.Count; i++)
            {
                double price = PriceByPlace[lineOfPeople[i]];

                var prev = dp[i - 1];

                var result = new Result
                {
                    MaxWindowTaken = prev.MaxWindowTaken,
                    WindowBacks = prev.WindowBacks.ToDictionary(x => x.Key, x => x.Value),
                    Windows = prev.Windows.ToList(),
                };
                
                int windowToPick;
                if (prev.WindowBacks.ContainsKey(lineOfPeople[i]))
                {
                    windowToPick = prev.WindowBacks[lineOfPeople[i]];
                    price *= 0.8;
                }
                else if (prev.MaxWindowTaken < numWindows)
                {
                    windowToPick = prev.MaxWindowTaken + 1;
                    result.MaxWindowTaken++;
                }
                else
                {
                    windowToPick = 0;
                }

                result.Price = prev.Price + price;
                result.WindowBacks[lineOfPeople[i]] = windowToPick;
                result.Windows.Add(windowToPick);

                dp[i] = result;
            }

            return dp[lineOfPeople.Count - 1];
        }

        private static Result GetResult(int numWindows, List<string> lineOfPeople)
        {
            var windows = Enumerable.Range(1, numWindows).ToDictionary(x => x, x => "");
            return DoWork(windows, lineOfPeople, new Result());
        }

        private static Result DoWork(Dictionary<int, string> windows, List<string> lineOfPeople, Result curResult)
        {
            // if we're at the end, return result
            if (lineOfPeople.Count == 0)
            {
                return curResult;
            }

            // now we've got a person we're looking at
            var person = lineOfPeople.First();
            var restOfLine = lineOfPeople.Skip(1).ToList();

            // GREEDY: pick window where back of line is same as person
            foreach (var window in windows)
            {
                if (string.Equals(person, window.Value))
                {
                    curResult.Windows.Add(window.Key);
                    curResult.Price += PriceByPlace[person] * 0.8;
                    return DoWork(windows, restOfLine, curResult);
                }
            }

            // for each window, get price from rest of line, choose cheapest
            var cheapest = new Result { Price = double.MaxValue };

            // make sure we don't choose the same window twice
            var triedWindowBacks = new HashSet<string>();

            foreach (var window in windows)
            {
                if (triedWindowBacks.Contains(window.Value))
                {
                    continue;
                }
                triedWindowBacks.Add(window.Value);

                // make a copy to send down to rest of line
                var restOfLineWindows = windows.ToDictionary(x => x.Key, x => x.Value);
                var restOfLineResult = new Result { Price = curResult.Price, Windows = curResult.Windows.ToList() };

                // choose this window
                restOfLineResult.Windows.Add(window.Key);
                restOfLineResult.Price += PriceByPlace[person];
                restOfLineWindows[window.Key] = person;

                // get price of children 
                var possibleResult = DoWork(restOfLineWindows, restOfLine, restOfLineResult);

                if (possibleResult.Price < cheapest.Price)
                {
                    cheapest = possibleResult;
                }    
            }

            return cheapest;
        }

        private static void RunTests()
        {
            PriceByPlace = new Dictionary<string, int>{{"CALIFORNIA", 10}, {"HAWAII", 8}, {"NEWYORK", 12}};
            var output = GetResult(2, new List<string> {"NEWYORK", "NEWYORK", "CALIFORNIA", "NEWYORK", "HAWAII"});
            if (output.Price != 49.2 || !ListEqual(output.Windows, new List<int>{1, 1, 2, 1, 1}))
                throw new Exception();

            output = GetResult(1, new List<string> {"NEWYORK"});
            if (output.Price != 12.0 || !ListEqual(output.Windows, new List<int>{1}))
                throw new Exception();
        }

        private static bool ListEqual(List<int> l1, List<int> l2)
        {
            if (l1.Count != l2.Count)
            {
                return false;
            }

            for (var i = 0; i < l1.Count; i++)
            {
                if (l1[i] != l2[i])
                {
                    return false;
                }
            }

            return true;
        }
    }
}
