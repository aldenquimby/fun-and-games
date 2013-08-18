using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

public class Helper
{
    private class Key : IComparable<Key>
    {
        public Key(int value)
        {
            Value = value;
        }

        public int Value { get; set; }

        public static bool IsRemove;

        public int CompareTo(Key other)
        {
            var result = Value.CompareTo(other.Value);
            return IsRemove ? result : result == 0 ? 1 : result;
        }
    }

    private readonly SortedList<Key, int> _list;

    public Helper(int capacity)
    {
        _list = new SortedList<Key, int>(capacity);
    }

    public void Add(int element)
    {
        _list.Add(new Key(element), 0);
    }

    private int BinarySearch(int element)
    {
        var value = new Key(element);
        var num1 = 0;
        var num2 = _list.Count - 1;
        while (num1 <= num2)
        {
            var index1 = (num1 + num2 + 1)/2;
            var num3 = _list.Comparer.Compare(_list.Keys[index1], value);
            if (num3 == 0)
                return index1;
            if (num3 < 0)
                num1 = index1 + 1;
            else
                num2 = index1 - 1;
        }
        return ~num1;
    }

    public int Rank(int element)
    {
        Key.IsRemove = true;
        var idx = BinarySearch(element);
        Key.IsRemove = false;
        return _list.Count - 1 - idx;
    }
}

public class InsertionSortStats
{
    private static string GetLine()
    {
        return read[idx++];
        return Console.ReadLine();
    }

    public static void Main2()
    {
        var numTestCases = int.Parse(GetLine());

        for (var unused = 0; unused < numTestCases; unused++)
        {
            var numElements = int.Parse(GetLine());
            var elements = GetLine().Split(' ').Select(int.Parse);

            var helper = new Helper(numElements);
            
            var total = 0;
            foreach (var element in elements)
            {
                helper.Add(element);
                var rank = helper.Rank(element);
                total += rank;
            }
            Console.WriteLine(total);
        }
    }

    private static int idx;
    private static List<string> read =
        input.Split('\n').Select(x => x.Trim()).Where(x => !string.IsNullOrEmpty(x)).ToList();
    private const string input = @"
2
5
1 1 1 2 2
5
2 1 3 1 2
";
}

public class LegoBlocks2
{
    // how many ways can we build a row?
    // => f(0)=0, f(1)=1, f(2)=2, f(3)=4, ..., f(n)=f(n-1)+f(n-2)+f(n-3)+f(n-4)
    // becuase to build a row of width n, the last piece must be of length 1, 2, 3, 4
    private static readonly Dictionary<int, int> RowPerms = new Dictionary<int, int> { { 1, 1 }, { 2, 2 }, { 3, 4 }, { 4, 8 } };
    private static int RowPermutations(int width)
    {
        if (!RowPerms.ContainsKey(width))
        {
            RowPerms[width] = RowPermutations(width - 1) + RowPermutations(width - 2) +
                              RowPermutations(width - 3) + RowPermutations(width - 4);
        }
        return RowPerms[width];
    }

    // how many ways can we build a wall?
    // => (number of ways to build a row)^height
    private static int AllWalls(int height, int width)
    {
        var waysToBuildRow = RowPermutations(width);
        var waysToBuildWall = (int)Math.Pow(waysToBuildRow, height);
        return waysToBuildWall;
    }

    // logically, AllWalls(h,w) = sum for x from 1 to w of SolidWalls(h,x)*AllWalls(h,w-x)
    // shifting around, we get SolidWalls(h,w) = AllWalls(h,w) - sum for x from 1 to w-1 of SolidWalls(h,x)*AllWalls(h,w-x)
    private static readonly Dictionary<int, Dictionary<int, int>> Walls = new Dictionary<int, Dictionary<int, int>>(); 
    private static int SolidWalls(int height, int width)
    {
        if (!Walls.ContainsKey(height))
        {
            Walls[height] = new Dictionary<int, int>();
        }
        else if (Walls[height].ContainsKey(width))
        {
            return Walls[height][width];
        }

        var waysToBuildWall = AllWalls(height, width);

        var notSolidWalls = 0;
        for (var x = 1; x < width; x++)
        {
            notSolidWalls += SolidWalls(height, x)*AllWalls(height, width - x);
        }
        
        var solidWalls = waysToBuildWall - notSolidWalls;
        Walls[height][width] = solidWalls;
        return solidWalls;
    }

    private static string GetLine()
    {
        return Console.ReadLine();
    }

    private const int MOD = 1000000007;

    public static void Main2()
    {
        var numTests = int.Parse(GetLine());
        for (var i = 0; i < numTests; i++)
        {
            var line = GetLine().Split(' ');
            var height = int.Parse(line[0]);
            var width = int.Parse(line[1]);

            var solidWalls = SolidWalls(height, width);

            Console.WriteLine(solidWalls);
        }
    }
}

public class LegoBlocks
{
    private static int[] _rowPermutations;
    private static int[,] _solidWalls;

    private static void Setup(int maxHeight, int maxWidth)
    {
        _solidWalls = new int[maxHeight+1,maxWidth+1];
        _rowPermutations = new int[maxWidth + 1];

        // SETUP UP ROW PERMUTATIONS
        _rowPermutations[1] = 1;
        _rowPermutations[2] = 2;
        _rowPermutations[3] = 4;
        _rowPermutations[4] = 8;
        for (var i = 5; i <= maxWidth; i++)
        {
            _rowPermutations[i] = _rowPermutations[i - 1] + _rowPermutations[i - 2] +
                                 _rowPermutations[i - 3] + _rowPermutations[i - 4];
        }

        for (var i = 1; i <= maxHeight; i++)
        {
            _solidWalls[i, 1] = 1; // for any height, there is 1 way to do width 1
        }
    }

    private static int SolidWalls(int height, int width)
    {
        if (_solidWalls[height, width] != 0)
        {
            return _solidWalls[height, width];
        }

        for (var w = 1; w <= width; w++)
        {
            if (_solidWalls[height, w] != 0)
            {
                continue;
            }

            var waysToBuildWall = BigInteger.ModPow(_rowPermutations[w], height, MOD).CorrectMod(MOD);
            var notSolidWalls = BigInteger.Zero;

            for (var x = 1; x < w; x++)
            {
                var tmp1 = _solidWalls[height, x]; // already in mod
                var tmp = BigInteger.ModPow(_rowPermutations[w - x], height, MOD).CorrectMod(MOD);
                var toAdd = BigInteger.Multiply(tmp1, tmp).CorrectMod(MOD);
                notSolidWalls = (toAdd + notSolidWalls).CorrectMod(MOD);
            }

            var solidWalls = (waysToBuildWall - notSolidWalls).CorrectMod(MOD);
            _solidWalls[height, w] = (int) solidWalls;
        }

        return _solidWalls[height, width];
    }

    private static string GetLine()
    {
        return Console.ReadLine();
    }

    private const int MOD = 1000000007;

    public static void Main()
    {
        Setup(959, 499);
        var r = SolidWalls(959, 499);

        var numTests = int.Parse(GetLine());

        var maxHeight = 0;
        var maxWidth = 0;
        var inputs = new List<Tuple<int, int>>();
        for (var i = 0; i < numTests; i++)
        {
            var line = GetLine().Split(' ');
            var height = int.Parse(line[0]);
            var width = int.Parse(line[1]);

            if (height > maxHeight)
            {
                maxHeight = height;
            }
            if (width > maxWidth)
            {
                maxWidth = width;
            }

            inputs.Add(Tuple.Create(height, width));
        }

        Setup(maxHeight, maxWidth);

        foreach (var input in inputs)
        {
            Console.WriteLine(SolidWalls(input.Item1, input.Item2));
        }
    }
}

static class Extensions
{
    public static BigInteger CorrectMod(this BigInteger bi, int mod)
    {
        return bi.Sign >= 0 ? bi%mod : bi + mod;
    }
}