using System;
using System.Linq;

namespace ConsoleApplication1
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var firstLine = Console.ReadLine() ?? "";
            var secondLine = Console.ReadLine() ?? "";

            var parts = firstLine.Split(' ');
            var numItems = int.Parse(parts[0]);
            var money = int.Parse(parts[1]);

            parts = secondLine.Split(' ');
            var itemPrices = parts.Select(int.Parse);

            var numToys = 0;
            
            foreach (var itemPrice in itemPrices.OrderBy(x => x))
            {
                if (money < itemPrice)
                {
                    break;
                }

                money -= itemPrice;
                numToys++;
            }

            Console.WriteLine(numToys);
        }
    }
}
