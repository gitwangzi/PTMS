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

namespace Cellbi.SvZLib
{
	/// <summary>
	/// Compession stream is used as input to output compression.
	/// </summary>
  public sealed class CompressionStream
	{
		/// <summary>
		/// 32K LZ77 window
		/// </summary>
		const int MaxWBits = 15;

		static readonly int DefaultWBits = MaxWBits;
    
    byte[] input;
    int inputIndex;
    int inputCount;
    long inputTotal;

    byte[] output;
    int outputIndex;
    int outputCount;
    long outputTotal;

		string errorMessage;

    Deflater deflater;
    Inflater inflater;

    int dataType;

    Adler32 checksum;

		// internal methods...
		/// <summary>
		/// Flush as much pending output as possible. All deflate output goes
		/// through this function so some applications may wish to modify it
		/// to avoid allocating a large next out buffer and copying into it.
		/// </summary>
		internal void  FlushPending()
		{
			int length = deflater.pending;
			
			if (length > outputCount)
				length = outputCount;

			if (length == 0)
				return ;
			
			Array.Copy(deflater.pendingBuffer, deflater.pendingOutput, output, outputIndex, length);
			
			outputIndex += length;
			deflater.pendingOutput += length;
			outputTotal += length;
			outputCount -= length;
			deflater.pending -= length;
			if (deflater.pending == 0)
				deflater.pendingOutput = 0;
		}		
		/// <summary>
		/// Read a new buffer from the current input stream, update the adler32
		/// and total number of bytes read.  All deflate input goes through
		/// this function so some applications may wish to modify it to avoid
		/// allocating a large next in buffer and copying from it.
		/// </summary>
		/// <param name="buffer">The buffer to read into.</param>
		/// <param name="start">The start index to read.</param>
		/// <param name="size">The number of bytes to read.</param>
		/// <returns>The number of bytes read.</returns>
		internal int Read(byte[] buffer, int start, int size)
		{
			int length = inputCount;
			
			if (length > size)
				length = size;

			if (length == 0)
				return 0;
			
			inputCount -= length;
			
			if (deflater.noheader == 0)
				checksum.Add(input, InputIndex, length);
			
			Array.Copy(input, InputIndex, buffer, start, length);
			InputIndex += length;
			inputTotal += length;
			return length;
		}

		// public methods...
		/// <summary>
		/// Initializes stream inflater with default window size.
		/// </summary>
		/// <returns></returns>
    public int InitializeInflater()
		{
			return InitializeInflater(DefaultWBits);
		}
		/// <summary>
		/// Initializes stream inflater witht the given window size.
		/// </summary>
		/// <param name="w">The size of the infalter window.</param>
		/// <returns></returns>
		public int InitializeInflater(int w)
		{
			inflater = new Inflater();
			return inflater.Initialize(this, w);
		}
		/// <summary>
		/// Performs inflate operation.
		/// </summary>
		/// <param name="f"></param>
		/// <returns></returns>
		public int Inflate(int f)
		{
			if (inflater == null)
        return (int)ErrorCode.StreamError;
			return inflater.Inflate(this, f);
		}
		/// <summary>
		/// Ends inflation process.
		/// </summary>
		/// <returns></returns>
		public int InflateEnd()
		{
			if (inflater == null)
        return (int)ErrorCode.StreamError;
			int ret = inflater.End(this);
			inflater = null;
			return ret;
		}
		/// <summary>
		/// Performs inflate sync operation.
		/// </summary>
		/// <returns></returns>
		public int InflateSync()
		{
			if (inflater == null)
        return (int)ErrorCode.StreamError;
			return inflater.Sync(this);
		}
		/// <summary>
		/// Sets the specified inflater dictionary.
		/// </summary>
		/// <param name="dictionary">The inflater dictionary.</param>
		/// <param name="dictLength">The inflater dictionary length.</param>
		/// <returns></returns>
		public int SetInflaterDictionary(byte[] dictionary, int dictLength)
		{
			if (inflater == null)
        return (int)ErrorCode.StreamError;
			return inflater.SetDictionary(this, dictionary, dictLength);
		}
		
		/// <summary>
		/// Initializes deflater with the given compression level.
		/// </summary>
		/// <param name="level">The deflater compression level.</param>
		/// <returns></returns>
		public int InitializeDeflater(int level)
		{
			return InitializeDeflater(level, MaxWBits);
		}
		/// <summary>
		/// Initializes deflater with the given compression level.
		/// </summary>
		/// <param name="level">The deflater compression level.</param>
		/// <param name="bits">The deflater window size.</param>
		/// <returns></returns>
		public int InitializeDeflater(int level, int bits)
		{
			deflater = new Deflater();
			return deflater.Initialize(this, level, bits);
		}
		/// <summary>
		/// Performs deflate operation with the spcified flush option.
		/// </summary>
		/// <param name="flush">The flush option to use.</param>
		/// <returns></returns>
		public int Deflate(int flush)
		{
			if (deflater == null)
				return (int)ErrorCode.StreamError;
			return deflater.Deflate(this, flush);
		}
		/// <summary>
		/// Ends deflater operation.
		/// </summary>
		/// <returns></returns>
		public int DeflateEnd()
		{
			if (deflater == null)
        return (int)ErrorCode.StreamError;
			int ret = deflater.End();
			deflater = null;
			return ret;
		}
		/// <summary>
		/// Sets deflater parameters.
		/// </summary>
		/// <param name="level">The compression level.</param>
		/// <param name="strategy">The deflater compression strategy.</param>
		/// <returns></returns>
		public int SetDeflaterParameters(int level, int strategy)
		{
			if (deflater == null)
        return (int)ErrorCode.StreamError;
			return deflater.SetParameters(this, level, strategy);
		}
		/// <summary>
		/// Set the specified deflater dictionary.
		/// </summary>
		/// <param name="dictionary">The deflater dictionary.</param>
		/// <param name="dictLength">The deflater dictionary length.</param>
		/// <returns></returns>
		public int SetDeflaterDictionary(byte[] dictionary, int dictLength)
		{
			if (deflater == null)
        return (int)ErrorCode.StreamError;
			return deflater.SetDictionary(this, dictionary, dictLength);
		}
		/// <summary>
		/// Frees all input and output data.
		/// </summary>
		public void Free()
		{
			input = null;
			output = null;
			ErrorMessage = null;
		}

		// internal properties...
		/// <summary>
		/// Gets or sets stream deflater.
		/// </summary>
		internal Deflater Deflater
		{
			get { return deflater; }
			set { deflater = value; }
		}
		/// <summary>
		/// Best guess about the data type: ascii or binary
		/// </summary>
		internal int DataType
		{
			get { return dataType; }
			set { dataType = value; }
		}

    // public properties...
		/// <summary>
		/// Gets or sets next input byte.
		/// </summary>
    public byte[] Input
    {
      get { return input; }
      set { input = value; }
    }
		/// <summary>
		/// Gets or sets start index of the next input data.
		/// </summary>
    public int InputIndex
    {
      get { return inputIndex; }
      set { inputIndex = value; }
    }
		/// <summary>
		/// Gets or sets the number of bytes available at next input.
		/// </summary>
    public int InputCount
    {
      get { return inputCount; }
      set { inputCount = value; }
    }
		/// <summary>
		/// Gets or sets total number of input bytes read so far.
		/// </summary>
    public long InputTotal
    {
      get { return inputTotal; }
      set { inputTotal = value; }
    }

		/// <summary>
		/// Gets or sets next output byte.
		/// </summary>
    public byte[] Output
    {
      get { return output; }
      set { output = value; }
    }
		/// <summary>
		/// Gets or sets output index.
		/// </summary>
    public int OutputIndex
    {
      get { return outputIndex; }
      set { outputIndex = value; }
    }
		/// <summary>
		/// Gets or set remaining free space at next output.
		/// </summary>
    public int OutputCount
    {
      get { return outputCount; }
      set { outputCount = value; }
    }
		/// <summary>
		/// Gets or sets total number of bytes output so far.
		/// </summary>
    public long OutputTotal
    {
      get { return outputTotal; }
      set { outputTotal = value; }
    }

		/// <summary>
		/// Gets or sets data checksum.
		/// </summary>
    public Adler32 Checksum
    {
      get { return checksum; }
      set { checksum = value; }
		}
		/// <summary>
		/// Gets or sets stream error message.
		/// </summary>
		public string ErrorMessage
		{
			get { return errorMessage; }
			set { errorMessage = value; }
		}
	}
}