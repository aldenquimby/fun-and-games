using System;
using System.Collections.Generic;

namespace CoinOnTheTable
{
    class Program
    {
        static string ReadLine()
        {
            return Console.ReadLine();
        }

        static void Main()
        {
            var firstLine = ReadLine().Split(' ');
            _n = int.Parse(firstLine[0]);
            _m = int.Parse(firstLine[1]);
            _k = int.Parse(firstLine[2]);

            _board = new List<string>(_n);

            for (var i = 0; i < _n; i++)
            {
                _board.Add(ReadLine());
            }

            Console.WriteLine(GetOperations());
        }

        private static int _n;
        private static int _k;
        private static int _m;
        private static List<string> _board; 

        static int GetOperations()
        {
            var result = GetOperationsInternal();

            var pt = GetGoalPoint();

            // if still infinity, we can't reach the goal
            return result[pt.Item1, pt.Item2] == int.MaxValue
                ? -1
                : result[pt.Item1, pt.Item2];
        }

        static int[,] GetOperationsInternal()
        {
            var result = new int[_n, _m];

            // initialize to infinity
            for (var i = 0; i < _n; i++)
            {
                for (var j = 0; j < _m; j++)
                {
                    result[i, j] = int.MaxValue;
                }
            }

            Traverse(0, 0, 0, 0, result);

            return result;
        }

        static void Traverse(int n, int m, int k, int c, int[,] p)
        {
            if (n < 0 || n >= _n || m < 0 || m >= _m || k > _k || c >= p[n, m])
            {
                return;
            }

            p[n, m] = c;

            Traverse(n - 1, m,     k + 1, c + (_board[n][m] == 'U' ? 0 : 1), p);
            Traverse(n,     m - 1, k + 1, c + (_board[n][m] == 'L' ? 0 : 1), p);
            Traverse(n + 1, m,     k + 1, c + (_board[n][m] == 'D' ? 0 : 1), p);
            Traverse(n,     m + 1, k + 1, c + (_board[n][m] == 'R' ? 0 : 1), p);
        }

        static Tuple<int, int> GetGoalPoint()
        {
            var goali = 0;
            var goalj = 0;

            for (var i = 0; i < _n; i++)
            {
                for (var j = 0; j < _m; j++)
                {
                    if (_board[i][j] == '*')
                    {
                        goali = i;
                        goalj = j;
                    }
                }
            }

            return Tuple.Create(goali, goalj);
        }
    }
}