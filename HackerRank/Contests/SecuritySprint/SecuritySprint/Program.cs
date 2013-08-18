using System;
using System.CodeDom;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Policy;
using System.Text;

namespace SecuritySprint
{
    class Program
    {
        static string GetLine()
        {

            return Console.ReadLine();
        }

        static void Main()
        {
            var dictWords = File.ReadAllLines("dictionary.lst");
            var dictByLength = dictWords.ToLookup(x => x.Length).ToDictionary(x => x.Key, x => x.ToList());
            var inputWords = GetLine().Split(' ');
            var inputByLength = inputWords.ToLookup(x => x.Length).ToDictionary(x => x.Key, x => x.ToList());

            var charMap = new Dictionary<char, char>();

            foreach (var kvp in inputByLength)
            {
                foreach (var inputWord in kvp.Value)
                {
                    var maps = new List<Dictionary<char, char>>();
                    var map = new Dictionary<char, char>();
                    foreach (var dictWord in dictByLength[kvp.Key])
                    {
                        var isValid = true;
                        for (var i = 0; i < inputWord.Length; i++)
                        {
                            if (!map.ContainsKey(inputWord[i]))
                            {
                                map[inputWord[i]] = dictWord[i];
                            }
                            else if (map[inputWord[i]] != dictWord[i])
                            {
                                isValid = false;
                            }
                        }
                        if (isValid)
                        {
                            maps.Add(map);
                        }
                    }


                }
            }

        }

        static void Helper()
        {
            var numTests = int.Parse(GetLine());
            for (var i = 0; i < numTests; i++)
            {
                var keyword = GetLine();
                var cipher = GetLine();

                var keyChars = keyword.Distinct().ToList();
                var charsToSkip = new HashSet<char>(keyChars);
                var columns = keyChars.Select(x => new List<char>{x}).ToList();
                var curCol = 0;
                for (var currentChar = 'A'; currentChar <= 'Z'; currentChar++)
                {
                    if (charsToSkip.Contains(currentChar))
                    {
                        continue;
                    }
                    columns[curCol].Add(currentChar);
                    curCol = (curCol + 1)%columns.Count;
                }

                var map = new Dictionary<char, char>();
                var current = 'A';
                foreach (var col in columns.OrderBy(x => x[0]))
                {
                    foreach (var c in col)
                    {
                        map[c] = current++;
                    }
                }

                var output = cipher.Select(c => map.ContainsKey(c) ? map[c] : c).ToArray();
                Console.WriteLine(new string(output));
            }
        }
    }
} 
