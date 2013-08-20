using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Candies
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
        public static void Min(int[] ratings, int totalStud)
        {
            for (int i = 1; i < ratings.Length; i++)
            {
                if (ratings[i] > ratings[i - 1])
                {
                    optimal[i] = optimal[i - 1] + 1;
                }
                if (ratings[i] == ratings[i - 1])
                {
                    optimal[i] = 1;
                }
                if (ratings[i] < ratings[i - 1])
                {
                    optimal[i] = optimal[i - 1] > 1 ? 1 : 0;
                    if (optimal[i] == 0)
                    {
                        optimal[i] = 1;
                        for (int j = i - 1; j >= 0; j--)
                        {
                            if ((ratings[j] > ratings[j + 1] && optimal[j] > optimal[j + 1]) || (ratings[j] < ratings[j + 1] && optimal[j] < optimal[j + 1]) || (ratings[j] == ratings[j + 1]))
                            {
                                break;
                            }
                            else
                            {
                                optimal[j] = optimal[j + 1] + 1;
                            }

                        }
                    }

                }
            }
        }
         */
    }
}
