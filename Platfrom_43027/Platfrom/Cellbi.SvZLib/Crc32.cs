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
	/// Crc checksum algorithm.
	/// </summary>
	public sealed class Crc32 : IChecksumAlgorithm
	{
		const uint crcMask = 0xffffffff;
		static uint[] crcTable;
		uint value;

		// private methods...
		static uint[] CalculateCrcTable()
		{
			uint[] table = new uint[256];
			uint poly = CalculatePoly();
			int count = table.Length;
			for (uint i = 0; i < count; i++)
			{
				uint j = i;
				for (uint k = 0; k < 8; k++)
					j = (j & 0x01) != 0 ? poly ^ (j >> 1) : j >> 1;
				table[i] = j;
			}
			return table;
		}
		static uint CalculatePoly()
		{
			byte[] p = new byte[]{0, 1, 2, 4, 5, 7, 8, 10, 11, 12, 16, 22, 23, 26};
			uint result = 0;
			int count = p.Length;
			for (int i = 0; i < count; i++)
				result |= (uint)(0x01 << (31 - p[i]));
			return result; // 0xedb88320
		}

		// constructors...		
		/// <summary>
		/// Initializes a new instance of the Crc32 class.
		/// </summary>
		static Crc32()
		{
			crcTable = CalculateCrcTable();			
		}
		/// <summary>
		/// Initializes a new instance of the Crc32 class.
		/// </summary>
		public Crc32()
		{
			Reset();
		}

		// public methods...
		/// <summary>
		/// Adds the specified byte data to the checksum.
		/// </summary>
		/// <param name="data">The data to use.</param>
		public void Add(byte data)
		{
			value = crcTable[(value ^ data) & 0xff] ^ (value >> 8);
		}		
		/// <summary>
		/// Adds the specified range of bytes to the checksum.
		/// </summary>
		/// <param name="data">The array of bytes to use.</param>
		public void Add(byte[] data)
		{
			Add(data, 0, data.Length);
		}
		/// <summary>
		/// Adds the specified range of bytes to the checksum.
		/// </summary>
		/// <param name="data">The array of bytes to use.</param>
		/// <param name="start">The index of the start byte.</param>
		/// <param name="length">The length of bytes.</param>
		public void Add(byte[] data, int start, int length)
		{
			for (int i = 0; i < length; i++)
				Add(data[start + i]);
		}
		/// <summary>
		/// Resets checksum.
		/// </summary>
		public void Reset()
		{
			value = crcMask;
		}
		/// <summary>
		/// Returns current checksum value.
		/// </summary>
		public long Get()
		{
			return value & crcMask;
		}

		// public static methods...
		/// <summary>
		/// Calculates Crc32 for the specified byte array.
		/// </summary>
		/// <param name="data">The array to calculate crc.</param>
		public static long Get(byte[] data)
		{
			Crc32 crc = new Crc32();
			crc.Add(data);
			return crc.Get();
		}
		/// <summary>
		/// Calculates Crc32 for the specified byte array.
		/// </summary>
		/// <param name="data">The array to calculate crc.</param>
		/// <param name="start">The index of the start byte.</param>
		/// <param name="length">The length of bytes.</param>
		public static long Get(byte[] data, int start, int length)
		{
			Crc32 crc = new Crc32();
			crc.Add(data, start, length);
			return crc.Get();
		}
	}
}