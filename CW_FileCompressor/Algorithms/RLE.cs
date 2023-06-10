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
    /// The class represents "Run-Length Encoding" algorithm. 
    /// Used for file compression.
    /// </summary>
    public class RLE
    {
        public static byte[] Compress(byte[] inputBytes)
        {
            using (var compressedStream = new MemoryStream())
            {
                int count = 1;
                byte previousByte = inputBytes[0];

                for (int i = 1; i < inputBytes.Length; i++)
                {
                    byte currentByte = inputBytes[i];

                    if (currentByte == previousByte && count < 255)
                    {
                        count++;
                    }
                    else
                    {
                        compressedStream.WriteByte((byte)count);
                        compressedStream.WriteByte(previousByte);

                        count = 1;
                        previousByte = currentByte;
                    }
                }

                compressedStream.WriteByte((byte)count);
                compressedStream.WriteByte(previousByte);

                return compressedStream.ToArray();
            }
        }

        public static byte[] Decompress(byte[] inputBytes)
        {
            using (var decompressedStream = new MemoryStream())
            {
                for (int i = 0; i < inputBytes.Length; i += 2)
                {
                    int count = inputBytes[i];
                    byte value = inputBytes[i + 1];

                    for (int j = 0; j < count; j++)
                    {
                        decompressedStream.WriteByte(value);
                    }
                }

                return decompressedStream.ToArray();
            }
        }
    }
}
