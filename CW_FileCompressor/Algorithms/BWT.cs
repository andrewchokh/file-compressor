using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CW_FileCompressor.Algorithms
{
    /// <summary>
    /// The class represents "Burrows–Wheeler transform" algorithm. 
    /// Used for file compression.
    /// </summary>
    public class BWT
    {
        public static byte[] Compress(byte[] inputBytes)
        {
            string inputString = Encoding.ASCII.GetString(inputBytes);
            int inputLength = inputString.Length;

            string[] rotations = new string[inputLength];
            for (int i = 0; i < inputLength; i++)
                rotations[i] = inputString.Substring(i) + inputString.Substring(0, i);

            Array.Sort(rotations);

            string transformedString = "";
            int originalIndex = 0;
            for (int i = 0; i < inputLength; i++)
            {
                if (rotations[i] == inputString)
                    originalIndex = i;

                transformedString += rotations[i][inputLength - 1];
            }

            byte[] transformedBytes = Encoding.ASCII.GetBytes(transformedString);

            byte[] outputBytes = new byte[inputLength + 4];
            Array.Copy(BitConverter.GetBytes(originalIndex), outputBytes, 4);
            Array.Copy(transformedBytes, 0, outputBytes, 4, inputLength);

            return outputBytes;
        }

        public static byte[] Decompress(byte[] inputBytes)
        {
            int inputLength = inputBytes.Length - 4;
            int originalIndex = BitConverter.ToInt32(inputBytes, 0);
            string transformedString = Encoding.ASCII.GetString(inputBytes, 4, inputLength);
            string[] rotations = new string[inputLength];

            for (int i = 0; i < inputLength; i++)
            {
                for (int j = 0; j < inputLength; j++)
                    rotations[j] = transformedString[j] + rotations[j];

                rotations = rotations.OrderBy(r => r).ToArray();
            }

            string originalString = rotations[originalIndex];
            byte[] originalBytes = Encoding.ASCII.GetBytes(originalString);

            return originalBytes;
        }
    }
}
