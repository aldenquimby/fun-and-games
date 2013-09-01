using System;
using System.Collections.Generic;
using System.Linq;

namespace Encryption
{
    class Square
    {
        public Square(int w, int h)
        {
            Width = w;
            Height = h;
            Size = w*h;
        }

        public int Width { get; private set; }
        public int Height { get; private set; }
        public int Size { get; private set; }
    }

    class Program
    {
        static string GetLine()
        {
            return Console.ReadLine();
        }

        static void Main()
        {
            var word = GetLine();
            var encoded = Encode(word);
            Console.WriteLine(encoded);
        }

        static List<List<char>> ChunkString(string s, int chunkSize)
        {
            var chunks = new List<List<char>>();
            var cur = new List<char>(chunkSize);
            foreach (var c in s)
            {
                cur.Add(c);
                if (cur.Count == chunkSize)
                {
                    chunks.Add(cur);
                    cur = new List<char>();
                }
            }
            if (cur.Count > 0)
            {
                chunks.Add(cur);
            }
            return chunks;
        }

        static string Encode(string word)
        {
            var possibleWidthHeights = new List<Square>();
         
            for (var h = 1; h < 10; h++)
            {
                for (var w = h; w < 10; w++)
                {
                    possibleWidthHeights.Add(new Square(w, h));
                }
            }

            var square = possibleWidthHeights
                .Where(x => x.Width - x.Height <= 1)
                .OrderBy(x => x.Size)
                .First(x => x.Size >= word.Length);

            var chunks = ChunkString(word, square.Width);

            var words = new List<string>();
            var encWord = new List<char>();
            for (var col = 0; col < square.Width; col++)
            {
                for (var row = 0; row < square.Height; row++)
                {
                    var chunk = chunks[row];
                    if (chunk.Count > col)
                    {
                        encWord.Add(chunk[col]);
                    }
                    if (row == square.Height - 1)
                    {
                        words.Add(new string(encWord.ToArray()));
                        encWord = new List<char>();
                    }
                }
            }

            return string.Join(" ", words);
        }
    }
}