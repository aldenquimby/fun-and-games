using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            //TODO
        }

        /*
         * 
    private static void MaxProfit(int[] sharePrice)
    {
        int firstDayPrice = sharePrice[0];
        long[] optimizedProfit = new long[sharePrice.Length];
        optimizedProfit[0] = 0;
        for (int i = 1; i < sharePrice.Length; i++)
        {
            if (sharePrice[i] > sharePrice[i - 1])
            {
                long cost = 0;
                long totalShares = 0;
                for (int j = i - 1; j >= 0; j--)
                {
                    if (sharePrice[i] < sharePrice[j])
                    {
                        optimizedProfit[i] = optimizedProfit[j];
                        break;
                    }
                    else
                    {
                        cost += sharePrice[j];
                        totalShares++;
                    }
                }
                optimizedProfit[i] += sharePrice[i] * totalShares - cost;

            }
            else
            {
                optimizedProfit[i] = optimizedProfit[i - 1];
            }
        }
        Array.Sort(optimizedProfit);
        Console.WriteLine(optimizedProfit[optimizedProfit.Length - 1]);
    }
         */
    }
}
