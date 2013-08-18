using System;
using System.Linq;

namespace StringReduction
{
    class Program
    {
        static string ReadLine()
        {
            return Console.ReadLine();
        }

        static void Main()
        {
            var numTests = int.Parse(ReadLine());
            for (var i = 0; i < numTests; i++)
            {
                Console.WriteLine(SmallestReduction(ReadLine()));
            }
        }

        static int SmallestReduction(string input)
        {
            var charLookup = input.ToLookup(x => x);
            var aCount = charLookup['a'].Count();
            var bCount = charLookup['b'].Count();
            var cCount = charLookup['c'].Count();

            // if two of three are 0, can't reduce
            if ((aCount == 0 && bCount == 0) || 
                (aCount == 0 && cCount == 0) || 
                (bCount == 0 && cCount == 0))
            {
                return input.Length;
            }

            // if all odd or all even, can reduce to 2
            if ((aCount%2 == 0 && bCount%2 == 0 && cCount%2 == 0) || 
                (aCount%2 == 1 && bCount%2 == 1 && cCount%2 == 1))
            {
                return 2;
            }

            // else can reduce to 1
            return 1;
        }
    }
}