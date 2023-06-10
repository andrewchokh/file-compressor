using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace CW_FileCompressor.Algorithms
{

    /// <summary>
    /// The class represents "Lempel-Ziv-1977" algorithm. 
    /// Used for file compression.
    /// </summary>
    public class LZ77
    {
        public static byte[] Compress(byte[] inputBytes)
        {
            var compressedData = new List<Tuple<int, int, byte>>();

            const int searchBufferSize = 4096;
            const int lookaheadBufferSize = 8;

            int cursorPosition = 0;

            while (cursorPosition < inputBytes.Length)
            {
                int bestMatchLength = 0;
                int bestMatchPosition = -1;
                byte nextByte = inputBytes[cursorPosition];

                for (int i = Math.Max(0, cursorPosition - searchBufferSize); i < cursorPosition; i++)
                {
                    int matchLength = 0;

                    while (matchLength < lookaheadBufferSize &&
                        cursorPosition + matchLength < inputBytes.Length &&
                        inputBytes[i + matchLength] == inputBytes[cursorPosition + matchLength])
                        matchLength++;

                    if (matchLength >= bestMatchLength)
                    {
                        bestMatchLength = matchLength;
                        bestMatchPosition = i;
                    }
                }

                if (bestMatchLength > 0)
                {
                    compressedData.Add(new Tuple<int, int, byte>(cursorPosition - bestMatchPosition,
                                                                 bestMatchLength, inputBytes[cursorPosition + bestMatchLength]));
                    cursorPosition += bestMatchLength + 1;
                }
                else
                {
                    compressedData.Add(new Tuple<int, int, byte>(0, 0, nextByte));
                    cursorPosition++;
                }
            }

            var compressedBytes = new List<byte>();

            foreach (var tuple in compressedData)
            {
                compressedBytes.AddRange(Encoding.ASCII.GetBytes(tuple.Item1.ToString()));
                compressedBytes.Add(0);
                compressedBytes.AddRange(Encoding.ASCII.GetBytes(tuple.Item2.ToString()));
                compressedBytes.Add(tuple.Item3);
            }

            return compressedBytes.ToArray();
        }

        public static byte[] Decompress(byte[] inputBytes)
        {
            var decompressedBytes = new List<byte>();
            int cursorPosition = 0;

            var fragment = new List<byte>();
            int? offset = null, length = null;

            while (cursorPosition < inputBytes.Length)
            {
                fragment.Add(inputBytes[cursorPosition]);

                if (fragment.Last() == 0)
                    offset = int.Parse(Encoding.ASCII.GetString(fragment.SkipLast(1).ToArray()));

                if (char.IsLetter((char)fragment.Last()))
                    length = int.Parse(Encoding.ASCII.GetString(fragment.SkipWhile(n => n != 0).Skip(1).SkipLast(1).ToArray()));

                if (offset != null && length != null) 
                {
                    for (int i = 0; i < length; i++)
                        decompressedBytes.Add(decompressedBytes[decompressedBytes.Count - offset ?? 0]);

                    decompressedBytes.Add(fragment.Last());

                    offset = null;
                    length = null;

                    fragment.Clear();
                }
                
                cursorPosition++;
            }

            return decompressedBytes.ToArray();
        }
    }
}
