using System;
using System.Collections.Generic;
using System.Linq;

namespace StockMaximize
{
    class Program
    {
        static string GetLine()
        {
            return Console.ReadLine();
        }

        static void Main()
        {
            var numTests = int.Parse(GetLine());

            for (var i = 0; i < numTests; i++)
            {
                GetLine(); // don't care about n
                var sharePrices = GetLine().Split(' ').Select(int.Parse).ToList();
                Console.WriteLine(GetProfit(sharePrices));
            }
        }

        static long GetProfit(IList<int> sharePrices)
        {
            var profit = new long[sharePrices.Count];
            
            for (var i = 1; i < sharePrices.Count; i++)
            {
                var price = sharePrices[i];

                // if this price is less, profit is same as last profit
                if (price <= sharePrices[i - 1])
                {
                    profit[i] = profit[i - 1];
                }
                else
                {
                    long cost = 0;
                    long totalShares = 0;
                    
                    // add share prices for all previous larger than this one
                    for (var j = i - 1; j >= 0; j--)
                    {
                        if (price < sharePrices[j])
                        {
                            profit[i] = profit[j];
                            break;
                        }
                        
                        cost += sharePrices[j];
                        totalShares++;
                    }

                    profit[i] += price * totalShares - cost;
                }
            }

            return profit.Max();
        }
    }
}
