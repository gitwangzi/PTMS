#region Copyright (c) 2006-2008 Cellbi
/*
Cellbi.SvZLib Software Component Product
Copyright (c) 2006-2008 Cellbi
www.cellbi.com

Redistribution and use in source and binary forms, with or without modification,
are permitted provided that the following conditions are met:

	1.	Redistributions of source code must retain the above copyright notice,
			this list of conditions and the following disclaimer.

	2.	Redistributions in binary form must reproduce the above copyright notice,
			this list of conditions and the following disclaimer in the documentation
			and/or other materials provided with the distribution.

	3.	The names of the authors may not be used to endorse or promote products derived
			from this software without specific prior written permission.

THIS SOFTWARE IS PROVIDED “AS IS?AND ANY EXPRESSED OR IMPLIED WARRANTIES,
INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL CELLBI
OR ANY CONTRIBUTORS TO THIS SOFTWARE BE LIABLE FOR ANY DIRECT, INDIRECT,
INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT
LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA,
OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF
LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING
NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE,
EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.

*/
#endregion
#region Copyright (c) 2000,2001,2002,2003 ymnk, JCraft,Inc.
/* -*-mode:java; c-basic-offset:2; -*- */
/*
Copyright (c) 2000,2001,2002,2003 ymnk, JCraft,Inc. All rights reserved.

Redistribution and use in source and binary forms, with or without
modification, are permitted provided that the following conditions are met:

  1. Redistributions of source code must retain the above copyright notice,
     this list of conditions and the following disclaimer.

  2. Redistributions in binary form must reproduce the above copyright 
     notice, this list of conditions and the following disclaimer in 
     the documentation and/or other materials provided with the distribution.

  3. The names of the authors may not be used to endorse or promote products
     derived from this software without specific prior written permission.

THIS SOFTWARE IS PROVIDED ``AS IS'' AND ANY EXPRESSED OR IMPLIED WARRANTIES,
INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL JCRAFT,
INC. OR ANY CONTRIBUTORS TO THIS SOFTWARE BE LIABLE FOR ANY DIRECT, INDIRECT,
INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT
LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA,
OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF
LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING
NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE,
EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
 */

/*
 * This program is based on zlib-1.1.3, so all credit should go authors
 * Jean-loup Gailly(jloup@gzip.org) and Mark Adler(madler@alumni.caltech.edu)
 * and contributors of zlib.
 */
#endregion

using System;
using System.IO;

namespace Cellbi.SvZLib
{
  /// <summary>
  /// Contains useful zip utility methods...
  /// </summary>
	public sealed class Utils
	{
		/// <summary>
		/// This method returns the literal value received
		/// </summary>
		/// <param name="literal">The literal to return</param>
		/// <returns>The received value</returns>
		internal static long Identity(long literal)
		{
			return literal;
		}
		/// <summary>
		/// Performs an unsigned bitwise right shift with the specified number
		/// </summary>
		/// <param name="number">Number to operate on</param>
		/// <param name="bits">Ammount of bits to shift</param>
		/// <returns>The resulting number from the shift operation</returns>
		internal static int ShiftRight(int number, int bits)
		{
      return number >= 0 ? number >> bits : (int)((number >> bits) + (2L << ~bits));
		}
		/// <summary>
		/// Performs an unsigned bitwise right shift with the specified number
		/// </summary>
		/// <param name="number">Number to operate on</param>
		/// <param name="bits">Ammount of bits to shift</param>
		/// <returns>The resulting number from the shift operation</returns>
		internal static int ShiftRight(int number, long bits)
		{
			return ShiftRight(number, (int)bits);
		}
		/// <summary>
		/// Performs an unsigned bitwise right shift with the specified number
		/// </summary>
		/// <param name="number">Number to operate on</param>
		/// <param name="bits">Ammount of bits to shift</param>
		/// <returns>The resulting number from the shift operation</returns>
		internal static long ShiftRight(long number, int bits)
		{
      return number >= 0 ? number >> bits : (number >> bits) + (2L << ~bits);
		}
		/// <summary>
		/// Performs an unsigned bitwise right shift with the specified number
		/// </summary>
		/// <param name="number">Number to operate on</param>
		/// <param name="bits">Ammount of bits to shift</param>
		/// <returns>The resulting number from the shift operation</returns>
		internal static long ShiftRight(long number, long bits)
		{
			return ShiftRight(number, (int)bits);
		}

		/// <summary>
    /// Reads a number of characters from the current source Stream and writes 
    /// the data to the target array at the specified index.
    /// </summary>
		/// <param name="source">The source Stream to read from.</param>
		/// <param name="target">Contains the array of characteres read from the source Stream.</param>
		/// <param name="start">The starting index of the target array.</param>
		/// <param name="count">The maximum number of characters to read from the source Stream.</param>
		/// <returns>
    /// The number of characters read. The number will be less than or equal to count depending 
    /// on the data available in the source Stream. Returns -1 if the end of the stream is reached.
    /// </returns>
		public static int ReadInput(Stream source, byte[] target, int start, int count)
		{
			// Returns 0 bytes if not enough space in target
			if (target.Length == 0)
				return 0;

			byte[] receiver = new byte[target.Length];
			int bytesRead = source.Read(receiver, start, count);

			// Returns -1 if EOF
			if (bytesRead == 0)	
				return -1;
                
			for(int i = start; i < start + bytesRead; i++)
				target[i] = (byte)receiver[i];
                
			return bytesRead;
		}
    
    /// <summary>
    /// Copies one stream to the other.
    /// </summary>
    /// <param name="input">The input stream.</param>
    /// <param name="output">The output stream.</param>
		public static void CopyStream(Stream input, Stream output)
		{
			byte[] buffer = new byte[2000];
			int length;
			while ((length = input.Read(buffer, 0, 2000)) > 0)
				output.Write(buffer, 0, length);
			output.Flush();
		}
    /// <summary>
    /// Compresses the specified array of bytes.
    /// </summary>
    /// <param name="input">The data to compress.</param>
		public static byte[] Compress(byte[] input)
		{
			MemoryStream memory = new MemoryStream(input);
			MemoryStream output = new MemoryStream();
			try
			{
				Compress(memory, output);
				byte[] buffer = output.GetBuffer();
				byte[] result = new byte[output.Length];
				Array.Copy(buffer, 0, result, 0, (int)output.Length);
				return result;
			}
			finally
			{
				memory.Close();
				output.Close();
			}
		}
    /// <summary>
    /// Compresses one stream to the other.
    /// </summary>
    /// <param name="input">The input stream to compress.</param>
    /// <param name="output">The output stream.</param>
		public static void Compress(Stream input, Stream output)
		{
			OutputStream outZStream = new OutputStream(output, (int)CompressionLevel.DefaultCompression);
			try
			{
				CopyStream(input, outZStream);
			}
			finally
			{
				outZStream.Close();
			}
		}
    /// <summary>
    /// Compresses one file to the other.
    /// </summary>
    /// <param name="inFile">The input file to compress.</param>
    /// <param name="outFile">The output file.</param>
		public static void CompressFile(string inFile, string outFile)
		{
			FileStream inFileStream = new FileStream(inFile, FileMode.Open);
			FileStream outFileStream = new FileStream(outFile, FileMode.Create);
			try
			{
				Compress(inFileStream, outFileStream);
			}
			finally
			{
				outFileStream.Close();
				inFileStream.Close();
			}
		}
    /// <summary>
    /// Decompresses the specified array of bytes.
    /// </summary>
    /// <param name="input">The data to decompress.</param>
		public static byte[] Decompress(byte[] input)
		{
			MemoryStream memory = new MemoryStream(input);
			MemoryStream output = new MemoryStream();
			try
			{
				Decompress(memory, output);
				byte[] buffer = output.GetBuffer();
				byte[] result = new byte[output.Length];
				Array.Copy(buffer, 0, result, 0, (int)output.Length);
				return result;
			}
			finally
			{
				memory.Close();
				output.Close();
			}
		}
    /// <summary>
    /// Decompresses input stream and saves the result to the output stream.
    /// </summary>
    /// <param name="input">The input stream to decompress.</param>
    /// <param name="output">The output stream.</param>
		public static void Decompress(Stream input, Stream output)
		{
			OutputStream outZStream = new OutputStream(output);
			try
			{
				CopyStream(input, outZStream);
			}
			finally
			{
				outZStream.Close();
			}
		}
    /// <summary>
    /// Decompresses input file and saves the result to the output file.
    /// </summary>
    /// <param name="inFile">The input stream to decompress.</param>
    /// <param name="outFile">The output stream.</param>
		public static void DecompressFile(string inFile, string outFile)
		{
			FileStream inFileStream = new FileStream(inFile, FileMode.Open);
			FileStream outFileStream = new FileStream(outFile, FileMode.Create);
			try
			{
				Decompress(inFileStream, outFileStream);
			}
			finally
			{
				outFileStream.Close();
				inFileStream.Close();
			}
		}
	}
}
