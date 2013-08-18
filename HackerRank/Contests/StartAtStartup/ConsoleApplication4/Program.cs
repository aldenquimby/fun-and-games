using System;
using System.Collections.Generic;
using System.Linq;

namespace ConsoleApplication4
{
    public class Point
    {
        public Point(int x, int y)
        {
            X = x;
            Y = y;
        }

        public int X { get; set; }
        public int Y { get; set; }
    }

    public class Program
    {
        private static string GetLine()
        {
            return read[idx++];
            return Console.ReadLine();
        }

        public static void Main()
        {
            var firstLine = GetLine().Split(' ');

            var width = int.Parse(firstLine[0]);
            var height = int.Parse(firstLine[1]);
            var n = int.Parse(firstLine[2]);
            var q = int.Parse(firstLine[3]);

            // make sure height and width are positive
            if (width <= 0 || height <= 0)
            {
                for (var i = 0; i < q; i++)
                {
                    Console.WriteLine(0);
                }
                return;
            }

            // get all of the trees
            var trees = new List<Point>();
            for (var i = 0; i < n; i++)
            {
                var line = GetLine().Split(' ');
                var tree = new Point(int.Parse(line[0]), int.Parse(line[1]));
                tree = FixY(tree, height);
                trees.Add(tree);
            }

            for (var i = 0; i < q; i++)
            {
                var line = GetLine().Split(' ');
                var query = new Point(int.Parse(line[0]), int.Parse(line[1]));
                query = FixY(query, height);
                var output = FindLargestHouse(query, width, height, trees);
                Console.WriteLine(output);
            }
        }

        private static short[,] _largestHouse;

        private static void SetupLargestHouse(Point origin, int forestSize, IEnumerable<Point> trees)
        {
            _largestHouse = new short[forestSize, forestSize];

            // fill in left edge
            for (var i = 0; i < forestSize; i++)
            {
                _largestHouse[i, 0] = 1;
            }

            // fill in top edge
            for (var j = 0; j < forestSize; j++)
            {
                _largestHouse[0, j] = 1;
            }

            // fill in trees
            foreach (var tree in trees)
            {
                var newtree = Translate(tree, origin);
                if (newtree.Y > 0 && newtree.Y < forestSize && newtree.X > 0 && newtree.X < forestSize)
                {
                    _largestHouse[newtree.Y, newtree.X] = 1;
                }
            }

            // fill in all other cells, left to right then top to bottom
            for (var i = 1; i < forestSize; i++)
            {
                for (var j = 1; j < forestSize; j++)
                {
                    if (_largestHouse[i, j] == 0) // only if we don't already know there's a tree
                    {
                        _largestHouse[i, j] = (short) (1 + Min(_largestHouse[i - 1, j],
                                                               _largestHouse[i - 1, j - 1],
                                                               _largestHouse[i, j - 1]));
                    }
                }
            }
        }

        private static short Min(short num1, short num2, short num3)
        {
            return Math.Min(Math.Min(num1, num2), num3);
        }

        private static Point FixY(Point p, int height)
        {
            return new Point(Math.Max(p.X, 0), Math.Max(height - p.Y, 0));
        }

        private static Point Translate(Point p, Point origin)
        {
            return new Point(p.X - origin.X, p.Y - origin.Y);
        }

        private static int FindLargestHouse(Point center, int width, int height, IEnumerable<Point> trees)
        {
            var forestSize = Math.Min(width, height);
            var origin = new Point(Math.Max(center.X - height/2, 0), Math.Max(center.Y - width/2, 0));
            SetupLargestHouse(origin, forestSize, trees);

            center = Translate(center, origin);

            // don't go past the bottom right edge
            var numSqauresToCheck = Math.Min(forestSize - center.X, forestSize - center.Y);
            
            var largest = 0;
            for (var i = 0; i < numSqauresToCheck; i++)
            {
                var size = (i + 1) * 2;

                // if we can't make a bigger square, stop
                if (_largestHouse[center.Y + i, center.X + i] < size)
                {
                    break;
                }

                largest = size;
            }

            return largest;
        }

        private static int idx;
        private static List<string> read =
            input.Split('\n').Select(x => x.Trim()).Where(x => !string.IsNullOrEmpty(x)).ToList();
        private const string input = @"
9 4 3 3
1 2
3 2
8 2
2 2
5 2
5 3
";
    }
}
