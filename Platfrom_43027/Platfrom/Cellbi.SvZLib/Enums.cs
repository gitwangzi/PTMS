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
	/// Compression level enumeration.
	/// </summary>
	public enum CompressionLevel : int
	{
		/// <summary>
		/// No compression will be used, exact copy.
		/// </summary>
		NoCompression = 0,

		/// <summary>
		/// Lite compression level will be used, best speed.
		/// </summary>
		BestSpeed = 1,

		/// <summary>
		/// Best compression level will be used, slower speed.
		/// </summary>
		BestCompression = 9,

		/// <summary>
		/// Default compression level will be used.
		/// </summary>
		DefaultCompression = -1
	}

	/// <summary>
	/// Enumerates all compression methods for zip contents.
	/// </summary>
	public enum CompressionMethod : int
	{
		/// <summary>
		/// No compression, instead original data is stored.
		/// </summary>
		Stored = 0,

		/// <summary>
		/// Zip compression with a dictionary up to 32Kb, using additional Huffman/Shannon-Fano trees compression.
		/// </summary>
		Deflated = 8,

		/// <summary>
		/// Deflate compression method extension with a 64KB window.
		/// </summary>
		Deflate64 = 9,

		/// <summary>
		/// BZip2 compression method. Not supported.
		/// </summary>
		BZip2 = 11,

		/// <summary>
		/// WinZip compression for AES encryption. Not supported
		/// </summary>
		WinZipAES = 99,
	}

	/// <summary>
	/// Compression strategy enumeration.
	/// </summary>
	public enum CompressionStrategy : int
	{
		/// <summary>
		/// Default compression strategy.
		/// </summary>
		DefaultStrategy = 0,

		/// <summary>
		/// Filtered compression strategy.
		/// </summary>
		Filtered = 1,

		/// <summary>
		/// Huffman compression strategy.
		/// </summary>
		HuffmanOnly = 2
	}

	/// <summary>
	/// Deflater function.
	/// </summary>
	public enum DeflaterFunction : int
	{
		/// <summary>
		/// Stored deflater function.
		/// </summary>
		Stored = 0,

		/// <summary>
		/// Fast deflater function.
		/// </summary>
		Fast = 1,

		/// <summary>
		/// Slow deflater function.
		/// </summary>
		Slow = 2
	}

	/// <summary>
	/// Deflater block types.
	/// </summary>
	internal enum DeflaterBlockType : int
	{
		/// <summary>
		/// Binary deflater block type.
		/// </summary>
		Binary = 0,

		/// <summary>
		/// Acsii text deflater block type.
		/// </summary>
		Ascii = 1,

		/// <summary>
		/// Unknown deflater block type.
		/// </summary>
		Unknown = 2
	}

	/// <summary>
	/// Defalter state
	/// </summary>
	internal enum DeflaterState : int
	{
		/// <summary>
		/// Initial deflater state.
		/// </summary>
		Initial = 42,

		/// <summary>
		/// Busy deflater state.
		/// </summary>
		Busy = 113,

		/// <summary>
		/// Finish defalter state.
		/// </summary>
		Finish = 666
	}

	/// <summary>
	/// Deflater tree type.
	/// </summary>
	internal enum DeflaterTreeType
	{
		/// <summary>
		/// Stored in memory block.
		/// </summary>
		StoredBlock = 0,

		/// <summary>
		/// Static deflater tree.
		/// </summary>
		StaticTree = 1,

		/// <summary>
		/// Dynamic deflater tree.
		/// </summary>
		DynamicTree = 2
	}

	/// <summary>
	/// Error codes enumeration.
	/// </summary>
	public enum ErrorCode : int
	{
		/// <summary>
		/// No error.
		/// </summary>
		Ok = 0,

		/// <summary>
		/// End of stream encountered.
		/// </summary>
		StreamEnd = 1,

		/// <summary>
		/// Dictionary needed error.
		/// </summary>
		NeedsDictionary = 2,

		/// <summary>
		/// Error no.
		/// </summary>
		ErrorNo = -1,

		/// <summary>
		/// Stream error.
		/// </summary>
		StreamError = -2,

		/// <summary>
		/// Data error.
		/// </summary>
		DataError = -3,

		/// <summary>
		/// Memory error.
		/// </summary>
		MemoryError = -4,

		/// <summary>
		/// Buffer error.
		/// </summary>
		BufferError = -5,

		/// <summary>
		/// Version error.
		/// </summary>
		VersionError = -6
	}

	/// <summary>
	/// Flush option to use.
	/// </summary>
	public enum FlushOption : int
	{
		/// <summary>
		/// No flushing is done.
		/// </summary>
		NoFlush = 0,

		/// <summary>
		/// Partial flushing is done.
		/// </summary>
		PartialFlush = 1,

		/// <summary>
		/// Sync flushing is done.
		/// </summary>
		SyncFlush = 2,

		/// <summary>
		///  Full flushing is done.
		/// </summary>
		FullFlush = 3,

		/// <summary>
		/// Finish flush.
		/// </summary>
		Finish = 4
	}

	/// <summary>
	/// Inflater algorithm states.
	/// </summary>
	internal enum InflaterState : int
	{
		/// <summary>
		/// Method byte waiting state.
		/// </summary>
		Method = 0,

		/// <summary>
		/// Flag byte waiting state.
		/// </summary>
		Flag = 1,

		/// <summary>
		/// Four dictionary check bytes state.
		/// </summary>
		DictionaryFour = 2,

		/// <summary>
		/// Three dictionary check bytes state.
		/// </summary>
		DictionaryThree = 3,

		/// <summary>
		/// Two dictionary check bytes state
		/// </summary>
		DictionaryTwo = 4,

		/// <summary>
		/// One dictionary check byte state
		/// </summary>
		DictionaryOne = 5,

		/// <summary>
		/// SetDictionary waiting state.
		/// </summary>
		DictionaryZero = 6,

		/// <summary>
		/// Decompressing blocks state.
		/// </summary>
		Blocks = 7,

		/// <summary>
		/// Four check bytes state.
		/// </summary>
		CheckFour = 8,

		/// <summary>
		/// Three check bytes state.
		/// </summary>
		CheckThree = 9,

		/// <summary>
		/// Two check bytes state.
		/// </summary>
		CheckTwo = 10,

		/// <summary>
		/// One check byte state.
		/// </summary>
		CheckOne = 11,

		/// <summary>
		/// Finished check, done.
		/// </summary>
		Done = 12,

		/// <summary>
		/// Error state.
		/// </summary>
		Error = 13
	}

	/// <summary>
	/// Inflater blocks state.
	/// </summary>
	internal enum InflaterBlocksState : int
	{
		/// <summary>
		/// Get type bits (3, including end bit).
		/// </summary>
		Type = 0,
		
		/// <summary>
		/// Get lengths for stored.
		/// </summary>
		Lens = 1,

		/// <summary>
		/// Processing stored block.
		/// </summary>
		Stored = 2,

		/// <summary>
		/// Get table lengths.
		/// </summary>
		Table = 3,

		/// <summary>
		/// Get bit lengths tree for a dynamic block.
		/// </summary>
		BTree = 4,

		/// <summary>
		/// Get length, distance trees for a dynamic block.
		/// </summary>
		DTree = 5,

		/// <summary>
		/// Processing fixed or dynamic block.
		/// </summary>
		Codes = 6,

		/// <summary>
		/// Output remaining window bytes.
		/// </summary>
		Dry = 7,

		/// <summary>
		/// Finished last block, done.
		/// </summary>
		Done = 8,

		/// <summary>
		/// Data error.
		/// </summary>
		Bad = 9
	}

	/// <summary>
	/// Infalter codes state.
	/// Waiting for "i:" = input, "o:" = output, "x:" = nothing
	/// </summary>
	internal enum InflaterCodesState : int
	{
		/// <summary>
		/// x: set up for LEN
		/// </summary>
		Start = 0,

		/// <summary>
		/// i: get length/literal/eob next
		/// </summary>
		Len = 1,

		/// <summary>
		/// i: getting length extra (have base)
		/// </summary>
		LenText = 2,

		/// <summary>
		/// i: get distance next
		/// </summary>
		Dist = 3,

		/// <summary>
		/// i: getting distance extra
		/// </summary>
		DistExt = 4,
		
		/// <summary>
		/// o: copying bytes in window, waiting for space
		/// </summary>
		Copy = 5,

		/// <summary>
		/// o: got literal, waiting for output space
		/// </summary>
		Lit = 6,

		/// <summary>
		/// o: got eob, possibly still output waiting
		/// </summary>
		Wash = 7,
		
		/// <summary>
		/// x: got eob and all data flushed
		/// </summary>
		End = 8,

		/// <summary>
		/// x: got error
		/// </summary>
		BadCode = 9,
	}
}