using System;
using System.Collections.Generic;
using System.Linq;

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
            var n = int.Parse(GetLine());

            var ratings = new List<int>(n);
            for (var i = 0; i < n; i++)
            {
                ratings.Add(int.Parse(GetLine()));
            }

            Console.WriteLine(NumCandies(ratings));
        }

        static int NumCandies(IList<int> ratings)
        {
            var candies = new int[ratings.Count]; // candies[i] is optimal # candies for student i

            // first guy gets 1 candy
            candies[0] = 1;

            for (var i = 1; i < ratings.Count; i++)
            {
                if (ratings[i] > ratings[i - 1]) // if i have higher rating than previous guy, i get 1 more
                {
                    candies[i] = candies[i - 1] + 1;
                }
                else if (ratings[i] == ratings[i - 1]) // if i'm equal to previous guy, i get 1
                {
                    candies[i] = 1;
                }
                else // if i'm lower than previous guy, i get 1 and others might change
                {
                    candies[i] = 1;

                    if (candies[i - 1] <= 1)
                    {
                        for (var j = i - 1; j >= 0; j--)
                        {
                            if ((ratings[j] > ratings[j + 1] && candies[j] > candies[j + 1]) || 
                                (ratings[j] < ratings[j + 1] && candies[j] < candies[j + 1]) || 
                                (ratings[j] == ratings[j + 1]))
                            {
                                break;
                            }
                            
                            candies[j] = candies[j + 1] + 1;
                        }
                    }
                }
            }

            return candies.Sum();
        }
    }
}