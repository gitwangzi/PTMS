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
  /// Inflater implementation.
  /// </summary>
	public sealed class Inflater
	{
    static byte[] mark = new byte[] { 0, 0, 0xff, 0xff};

    InflaterState state; // current inflate mode

    ErrorCode startError;
    ErrorCode error;
		
		// mode dependent information
		int method; // if FLAGS, method byte
		
		// if CHECK, check values to compare
		long[] was = new long[1]; // computed check value
		uint need; // stream check value
		
		// if BAD, inflateSync's marker bytes count
		int marker;
		
		// mode independent information
		int nowrap; // flag for no wrapper
		int wbits; // log2(window size)  (8..15, defaults to 15)
		
		InflaterBlocks blocks; // current inflate_blocks state
		
    // private methods...
		int Reset(CompressionStream stream)
		{
			if (stream == null)
				return (int)ErrorCode.StreamError;

			stream.InputTotal = 0;
			stream.OutputTotal = 0;
			stream.ErrorMessage = null;
			state = nowrap != 0 ? InflaterState.Blocks : InflaterState.Method;
			blocks.Reset(stream, null);
			return (int)ErrorCode.Ok;
		}
    /// <summary>
    /// Returns true if inflate is currently at the end of a block generated
    /// by Z_SYNC_FLUSH or Z_FULL_FLUSH. This function is used by one PPP
    /// implementation to provide an additional safety check. PPP uses Z_SYNC_FLUSH
    /// but removes the length bytes of the resulting empty stored block. When
    /// decompressing, PPP checks that at the end of input packet, inflate is
    /// waiting for these length bytes.
    /// </summary>
    /// <param name="stream">The zip stream.</param>
    int GetSyncPoint(CompressionStream stream)
    {
      if (stream == null || blocks == null)
        return (int)ErrorCode.StreamError;
      return blocks.GetSyncPoint();
    }

		bool HandleMethodState(CompressionStream stream, out ErrorCode result)
		{
			result = ErrorCode.Ok;
			if (stream.InputCount == 0)
			{
				result = error;
				return false;
			}

			error = startError;

			stream.InputCount--;
			stream.InputTotal++;
			if (((method = stream.Input[stream.InputIndex++]) & 0xf) != (int)CompressionMethod.Deflated)
			{
				state = InflaterState.Error;
				stream.ErrorMessage = "unknown compression method";
				marker = 5; // can't try inflateSync
				return true;
			}
			if ((method >> 4) + 8 > wbits)
			{
				state = InflaterState.Error;
				stream.ErrorMessage = "invalid window size";
				marker = 5; // can't try inflateSync
				return true;
			}
			state = InflaterState.Flag;
			return true;
		}
		bool HandleFlagState(CompressionStream stream, out ErrorCode result)
		{
			result = ErrorCode.Ok;
			if (stream.InputCount == 0)
			{
				result = error;
				return false;
			}

			error = startError;

			stream.InputCount--;
			stream.InputTotal++;
			int b = (stream.Input[stream.InputIndex++]) & 0xff;

			if ((((method << 8) + b) % 31) != 0)
			{
				state = InflaterState.Error;
				stream.ErrorMessage = "incorrect header check";
				marker = 5; // can't try inflateSync
				return true;
			}

			if ((b & Constants.PresetDictionaryMask) == 0)
			{
				state = InflaterState.Blocks;
				return true;
			}
			state = InflaterState.DictionaryFour;
			return true;
		}
    bool HandleDictionaryFourState(CompressionStream stream, out ErrorCode result)
    {
      result = ErrorCode.Ok;
      if (stream.InputCount == 0)
      {
        result = error;
        return false;
      }

      error = startError;

      stream.InputCount--;
      stream.InputTotal++;
      need = (uint)(((stream.Input[stream.InputIndex++] & 0xff) << 24) & unchecked((int)0xff000000L));
      state = InflaterState.DictionaryThree;
      return true;
    }
    bool HandleDictionaryThreeState(CompressionStream stream, out ErrorCode result)
    {
      result = ErrorCode.Ok;
      if (stream.InputCount == 0)
      {
        result = error;
        return false;
      }

      error = startError;

      stream.InputCount--;
      stream.InputTotal++;
      need += (uint)(((stream.Input[stream.InputIndex++] & 0xff) << 16) & 0xff0000L);
      state = InflaterState.DictionaryTwo;
      return true;
    }
    bool HandleDictionaryTwoState(CompressionStream stream, out ErrorCode result)
    {
      result = ErrorCode.Ok;
      if (stream.InputCount == 0)
      {
        result = error;
        return false;
      }

      error = startError;

      stream.InputCount--;
      stream.InputTotal++;
      need += (uint)(((stream.Input[stream.InputIndex++] & 0xff) << 8) & 0xff00L);
      state = InflaterState.DictionaryOne;
      return true;
    }
    bool HandleDictionaryOneState(CompressionStream stream, out ErrorCode result)
    {
      result = ErrorCode.Ok;
      if (stream.InputCount == 0)
      {
        result = error;
        return false;
      }

      error = startError;

      stream.InputCount--;
      stream.InputTotal++;
      need += (uint)(stream.Input[stream.InputIndex++] & 0xffL);
      stream.Checksum = new Adler32(need);
      state = InflaterState.DictionaryZero;
      result = ErrorCode.NeedsDictionary;
      return false;
    }
		bool HandleDictionaryZeroState(CompressionStream stream, out ErrorCode result)
		{
			state = InflaterState.Error;
			stream.ErrorMessage = "need dictionary";
			marker = 0; // can try inflateSync
			result = ErrorCode.StreamError;
			return false;
		}
    bool HandleBlocksState(CompressionStream stream, out ErrorCode result)
    {
      result = ErrorCode.Ok;

      error = (ErrorCode)blocks.Process(stream, (int)error);
      if (error == ErrorCode.DataError)
      {
        state = InflaterState.Error;
        marker = 0; // can try inflateSync
        return true;
      }

      if (error == ErrorCode.Ok)
        error = startError;

      if (error != ErrorCode.StreamEnd)
      {
        result = error;
        return false;
      }

      error = startError;
      blocks.Reset(stream, was);
      if (nowrap != 0)
      {
        state = InflaterState.Done;
        return true;
      }
      state = InflaterState.CheckFour;
      return true;
    }
    bool HandleCheckFourState(CompressionStream stream, out ErrorCode result)
    {
      result = ErrorCode.Ok;
      if (stream.InputCount == 0)
      {
        result = error;
        return false;
      }

      error = startError;

      stream.InputCount--;
      stream.InputTotal++;
      need = (uint)(((stream.Input[stream.InputIndex++] & 0xff) << 24) & unchecked((int)0xff000000L));
      state = InflaterState.CheckThree;
      return true;
    }
    bool HandleCheckThreeState(CompressionStream stream, out ErrorCode result)
    {
      result = ErrorCode.Ok;
      if (stream.InputCount == 0)
      {
        result = error;
        return false;
      }

      error = startError;

      stream.InputCount--;
      stream.InputTotal++;
      need += (uint)(((stream.Input[stream.InputIndex++] & 0xff) << 16) & 0xff0000L);
      state = InflaterState.CheckTwo;
      return true;
    }
    bool HandleCheckTwoState(CompressionStream stream, out ErrorCode result)
    {
      result = ErrorCode.Ok;
      if (stream.InputCount == 0)
      {
        result = error;
        return false;
      }

      error = startError;

      stream.InputCount--;
      stream.InputTotal++;
      need += (uint)(((stream.Input[stream.InputIndex++] & 0xff) << 8) & 0xff00L);
      state = InflaterState.CheckOne;
      return true;
    }
		bool HandleCheckOneState(CompressionStream stream, out ErrorCode result)
		{
			result = ErrorCode.Ok;
			if (stream.InputCount == 0)
			{
				result = error;
				return false;
			}

			error = startError;

			stream.InputCount--;
			stream.InputTotal++;
			need += (uint)(stream.Input[stream.InputIndex++] & 0xffL);

			if (((int)(was[0])) != ((int)(need)))
			{
				state = InflaterState.Error;
				stream.ErrorMessage = "incorrect data check";
				marker = 5; // can't try inflateSync
				return true;
			}

			state = InflaterState.Done;
			return true;
		}

    // public methods...
    /// <summary>
    /// Initializes zip inflater with the specified stream and winow size.
    /// </summary>
    /// <param name="stream">The zip stream.</param>
    /// <param name="windowSize">The window size.</param>
		public int Initialize(CompressionStream stream, int windowSize)
		{
			stream.ErrorMessage = null;
			blocks = null;
			
			// handle undocumented nowrap option (no zlib header or check)
			nowrap = 0;
			if (windowSize < 0)
			{
				windowSize = - windowSize;
				nowrap = 1;
			}
			
			// set window size
			if (windowSize < 8 || windowSize > 15)
			{
				End(stream);
				return (int)ErrorCode.StreamError;
			}
			wbits = windowSize;
			
			Inflater inflater = nowrap != 0 ? null : this;
			blocks = new InflaterBlocks(stream, inflater, 1 << windowSize);
			
			// reset state
			Reset(stream);
			return (int)ErrorCode.Ok;
		}
    /// <summary>
    /// Performs inflate process.
    /// </summary>
    /// <param name="stream">The input zip stream.</param>
    /// <param name="flush">The flush option.</param>
		public int Inflate(CompressionStream stream, int flush)
		{
			if (stream == null || stream.Input == null)
        return (int)ErrorCode.StreamError;

      startError = flush == (int)FlushOption.Finish ? ErrorCode.BufferError : ErrorCode.Ok;      
      error = ErrorCode.BufferError;
      ErrorCode result = ErrorCode.Ok;
			while (true)
			{
				switch (state)
				{
          case InflaterState.Method:
            if (!HandleMethodState(stream, out result))
              return (int)result;
            break;

          case InflaterState.Flag:
            if (!HandleFlagState(stream, out result))
              return (int)result;
            break;

          case InflaterState.DictionaryFour:
            if (!HandleDictionaryFourState(stream, out result))
              return (int)result;
            break;

          case InflaterState.DictionaryThree:
            if (!HandleDictionaryThreeState(stream, out result))
              return (int)result;
            break;

          case InflaterState.DictionaryTwo:
            if (!HandleDictionaryTwoState(stream, out result))
              return (int)result;
            break;

          case InflaterState.DictionaryOne:
            if (!HandleDictionaryOneState(stream, out result))
              return (int)result;
            break;

          case InflaterState.DictionaryZero:
            if (!HandleDictionaryZeroState(stream, out result))
              return (int)result;
            break;

          case InflaterState.Blocks:
            if (!HandleBlocksState(stream, out result))
              return (int)result;
            break;

          case InflaterState.CheckFour:
            if (!HandleCheckFourState(stream, out result))
              return (int)result;
            break;

          case InflaterState.CheckThree:
            if (!HandleCheckThreeState(stream, out result))
              return (int)result;
            break;

          case InflaterState.CheckTwo:
            if (!HandleCheckThreeState(stream, out result))
              return (int)result;
            break;

          case InflaterState.CheckOne:
            if (!HandleCheckOneState(stream, out result))
              return (int)result;
            break;						

          case InflaterState.Done:
            return (int)ErrorCode.StreamEnd;

          case InflaterState.Error:
            return (int)ErrorCode.DataError;
					
					default:
            return (int)ErrorCode.StreamError;
				}
			}
		}
    /// <summary>
    /// Sets inflater dictionary.
    /// </summary>
    /// <param name="stream">The input stream.</param>
    /// <param name="dictionary">The dictionary data.</param>
    /// <param name="dictLength">The length of the dictionary.</param>
    /// <returns></returns>
		public int SetDictionary(CompressionStream stream, byte[] dictionary, int dictLength)
		{
			int index = 0;
			int length = dictLength;
      if (stream == null || state != InflaterState.DictionaryZero)
        return (int)ErrorCode.StreamError;
			
			if (Adler32.Get(dictionary, 0, dictLength) != stream.Checksum.Get())
			{
        return (int)ErrorCode.DataError;
			}
			
			stream.Checksum = new Adler32();
			
			if (length >= (1 << wbits))
			{
				length = (1 << wbits) - 1;
				index = dictLength - length;
			}
			blocks.SetDictionary(dictionary, index, length);
      state = InflaterState.Blocks;
      return (int)ErrorCode.Ok;
		}
    /// <summary>
    /// Performs sync for the inflater.
    /// </summary>
    /// <param name="stream">The input stream.</param>
		public int Sync(CompressionStream stream)
		{
			int count; // number of bytes to look at
			int index; // pointer to bytes
			int markerCount; // number of marker bytes found in a row
      // set up
			if (stream == null)
        return (int)ErrorCode.StreamError;
      if (state != InflaterState.Error)
			{
        state = InflaterState.Error;
				marker = 0;
			}
      
      count = stream.InputCount;
			if (count == 0)
        return (int)ErrorCode.BufferError;
			index = stream.InputIndex;
			markerCount = marker;
			
			// search
			while (count != 0 && markerCount < 4)
			{
				if (stream.Input[index] == mark[markerCount])
					markerCount ++;
				else if (stream.Input[index] != 0)
					markerCount = 0;
				else
					markerCount = 4 - markerCount;
				index++;
        count--;
			}
			
			// restore
			stream.InputTotal += index - stream.InputIndex;
			stream.InputIndex = index;
			stream.InputCount = count;
			marker = markerCount;
			
			// return no joy or set up to restart on a new block
			if (markerCount != 4)
        return (int)ErrorCode.DataError;

      long inCount = stream.InputTotal;
      long outCount = stream.OutputTotal;
			Reset(stream);
      stream.InputTotal = inCount;
      stream.OutputTotal = outCount;
      state = InflaterState.Blocks;
      return (int)ErrorCode.Ok;
		}
    /// <summary>
    /// Ends inflater process.
    /// </summary>
    /// <param name="stream">The sip stream.</param>
    public int End(CompressionStream stream)
    {
      if (blocks != null)
        blocks.Free(stream);
      blocks = null;
      return (int)ErrorCode.Ok;
    }
	}
}