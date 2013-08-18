using System;
using System.Collections.Generic;
using System.Linq;

namespace ConsoleApplication4
{
    public class InputPoint
    {
        public int Position { get; set; }
        public int A { get; set; }
        public int B { get; set; }
    }

    public class ResultPoint
    {
        public ResultPoint(double x, double y)
        {
            X = x;
            Y = y;
        }

        public double X { get; set; }
        public double Y { get; set; }
        public double Score { get; set; }
    }

    public class Range
    {
        public int Time { get; set; }
        public int Left { get; set; }
        public int Right { get; set; }
    }

    public class Program
    {
        private static string GetLine()
        {
            return Console.ReadLine();
        }

        public static void Main(string[] args)
        {
            var firstLine = GetLine().Split(' ');
            var n = int.Parse(firstLine[0]);
            var q = int.Parse(firstLine[1]);

            var secondLine = GetLine().Split(' ');
            var thirdLine = GetLine().Split(' ');

            var points = new List<InputPoint>();

            for (var i = 0; i < n; i++)
            {
                points.Add(new InputPoint
                {
                    Position = i + 1,
                    A = int.Parse(secondLine[i]), 
                    B = int.Parse(thirdLine[i]),
                });
            }

            var ranges = new List<Range>();

            for (var i = 0; i < q; i++)
            {
                var line = GetLine().Split(' ');

                ranges.Add(new Range
                {
                    Time = i,
                    Left = int.Parse(line[0]),
                    Right = int.Parse(line[1]),
                });
            }

            foreach (var range in ranges)
            {
                var output = DoWork(points, range);
                Console.WriteLine(output);
            }
        }

        private static double DoWork(List<InputPoint> points, Range r)
        {
            var activePoints = points.Where(x => x.Position >= r.Left && x.Position <= r.Right).ToList();

            var largest = MathHelper.Largest(activePoints);
            var median = MathHelper.Median(activePoints);
            var close = new ResultPoint(1.4, 2.3);
            var pointToUse = median;
            
            pointToUse.Score = GetScore(points, pointToUse.X, pointToUse.Y);
            var result = AwesomeTest(pointToUse, activePoints);

            return Math.Round(result.Score, 4);
        }

        private const double EPSILON = 0.01;
        private const double PRECISION = 0.000001;

        private const double AWESOME_EPSILON = 0.1;
        private const double AWESOME_PRECISION = 0.0001;

        private static ResultPoint AwesomeTest(ResultPoint currResult, List<InputPoint> points, int iteration = 1)
        {
            var newResult = currResult;

            var epsilon = AWESOME_EPSILON/(iteration*10);

            foreach (var surroundingPoint in GetSurroundingPoints(currResult, epsilon))
            {
                surroundingPoint.Score = GetScore(points, surroundingPoint.X, surroundingPoint.Y);

                if (surroundingPoint.Score < newResult.Score)
                {
                    newResult = surroundingPoint;
                }
            }

            // if we got a new point, use it
            if (!ReferenceEquals(newResult, currResult))
            {
                return AwesomeTest(newResult, points, iteration);
            }

            if (epsilon < AWESOME_PRECISION)
            {
                return currResult;
            }
            else
            {
                return AwesomeTest(currResult, points, ++iteration);
            }
        }

        private static IEnumerable<ResultPoint> GetSurroundingPoints(ResultPoint point, double epsilon)
        {
            yield return new ResultPoint(point.X - epsilon, point.Y - epsilon);
            yield return new ResultPoint(point.X - epsilon, point.Y);
            yield return new ResultPoint(point.X - epsilon, point.Y + epsilon);
            yield return new ResultPoint(point.X, point.Y - epsilon);
            yield return new ResultPoint(point.X, point.Y + epsilon);
            yield return new ResultPoint(point.X + epsilon, point.Y - epsilon);
            yield return new ResultPoint(point.X + epsilon, point.Y);
            yield return new ResultPoint(point.X + epsilon, point.Y - epsilon);
        }

        private static ResultPoint Do(ResultPoint start, List<InputPoint> points)
        {
            var xOld = start.X;
            var yOld = start.Y;
            var xNew = xOld - EPSILON * MathHelper.DerivativeApprox(x => GetScore(points, x, yOld), xOld);
            var yNew = yOld - EPSILON * MathHelper.DerivativeApprox(y => GetScore(points, xOld, y), yOld);

            while (true)
            {
                var xDone = Math.Abs(xNew - xOld) <= PRECISION;
                var yDone = Math.Abs(yNew - yOld) <= PRECISION;

                if (xDone && yDone)
                {
                    break;
                }

                if (!xDone)
                {
                    var xDeriv = MathHelper.DerivativeApprox(x => GetScore(points, x, yOld), xOld);
                    xOld = xNew;
                    xNew = xOld - EPSILON*xDeriv;                    
                }

                if (!yDone)
                {
                    var yDeriv = MathHelper.DerivativeApprox(y => GetScore(points, xOld, y), yOld);
                    yOld = yNew;
                    yNew = yOld - EPSILON * yDeriv;
                }
            }

            return new ResultPoint(xNew, yNew);
        }

        private static double GetScore(IEnumerable<InputPoint> points, double resultX, double resultY)
        {
            var score = points.Sum(pt => GetDistance(pt, resultX, resultY));
            return score;
        }

        private static double GetDistance(InputPoint point, double resultX, double resultY)
        {
            // distance = max(|X-A[i]|,|Y-B[i]|)
            return Math.Max(Math.Abs(resultX - point.A), Math.Abs(resultY - point.B));
        }

        private static void Tests()
        {
            var points = new List<InputPoint>
            {
                new InputPoint {A = 1, B = 4, Position = 1},
                new InputPoint {A = 2, B = 3, Position = 2},
                new InputPoint {A = 0, B = 1, Position = 3},
                new InputPoint {A = 1, B = 1, Position = 4}
            };

            var output = DoWork(points, new Range {Left = 1, Right = 4});
            Console.WriteLine(output);
        }
    }

    public static class MathHelper
    {
        private const double H = 0.00000000000001;
        private const double H2 = H * 2;

        public static double DerivativeApprox(Func<double, double> f, double x, bool betterApprox = true)
        {
            if (betterApprox)
            {
                return (f(x - H2) - 8 * f(x - H) + 8 * f(x + H) - f(x + H2)) / (H2 * 6);
            }
            else
            {
                return (f(x + H) - f(x - H)) / H2;
            }
        }

        public static ResultPoint Centroid(List<InputPoint> vertices)
        {
            var centroid = new ResultPoint(0, 0);
            var signedArea = 0.0;
            double x0; // Current vertex X
            double y0; // Current vertex Y
            double x1; // Next vertex X
            double y1; // Next vertex Y
            double a;  // Partial signed area

            // For all vertices except last
            int i;
            for (i = 0; i < vertices.Count - 1; ++i)
            {
                x0 = vertices[i].A;
                y0 = vertices[i].B;
                x1 = vertices[i + 1].A;
                y1 = vertices[i + 1].B;
                a = x0 * y1 - x1 * y0;
                signedArea += a;
                centroid.X += (x0 + x1) * a;
                centroid.Y += (y0 + y1) * a;
            }

            // Do last vertex
            x0 = vertices[i].A;
            y0 = vertices[i].B;
            x1 = vertices[0].A;
            y1 = vertices[0].B;
            a = x0 * y1 - x1 * y0;
            signedArea += a;
            centroid.X += (x0 + x1) * a;
            centroid.Y += (y0 + y1) * a;

            signedArea *= 0.5;
            centroid.X /= (6 * signedArea);
            centroid.Y /= (6 * signedArea);

            return centroid;
        }

        public static ResultPoint Largest(List<InputPoint> points)
        {
            var result = new ResultPoint(0, 0);

            foreach (var point in points)
            {
                if (point.A > result.X)
                {
                    result.X = point.A;
                }

                if (point.B > result.Y)
                {
                    result.Y = point.B;
                }
            }

            return result;
        }

        public static ResultPoint Median(List<InputPoint> points)
        {
            var result = new ResultPoint(0, 0);

            foreach (var point in points)
            {
                result.X += point.A;
                result.Y += point.B;
            }

            result.X /= points.Count;
            result.Y /= points.Count;

            return result;
        }
    }
}
