namespace Level0
{
    public class Problem4
    {
        public int Solve()
        {
            return LargestPalindrome(100, 999);
        }

        static int LargestPalindrome(int min, int max)
        {
            var largest = 0;
            for (var i = max; i > min; i--)
            {
                for (var j = i; j >= min; j--)
                {
                    var product = i * j;
                    if (IsPalindrome(product))
                    {
                        if (product > largest)
                        {
                            largest = product;
                        }
                        break;
                    }
                }
            }
            return largest;
        }

        static bool IsPalindrome(int x)
        {
            var asString = x.ToString();
            for (var i = 0; i < asString.Length / 2; i++)
            {
                if (asString[i] != asString[asString.Length - 1 - i])
                {
                    return false;
                }
            }
            return true;
        }
    }
}
