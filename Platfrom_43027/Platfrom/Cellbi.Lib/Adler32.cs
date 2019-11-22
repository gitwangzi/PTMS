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
	/// Adler32 checksum algorithm.
	/// </summary>
	public sealed class Adler32 : IChecksumAlgorithm
	{
		const int crcBase = 0xfff1;
		const int crcMax = 0x15b0;
		uint value;

		// constructors...
		/// <summary>
		/// Initializes a new instance of the Adler32 class.
		/// </summary>
		public Adler32()
      : this(1)
		{
		}
    /// <summary>
    /// Initializes a new instance of the Adler32 class.
    /// </summary>
    /// <param name="value">The initial checksum value.</param>
    public Adler32(uint value)
    {
     this.value = value;
    }

		// public methods...
		/// <summary>
		/// Adds the specified byte data to the checksum.
		/// </summary>
		/// <param name="data">The data to use.</param>
		public void Add(byte data)
		{
			uint s1 = value & 0xffff;
			uint s2 = (value >> 0x10) & 0xffff;

			s1 += (uint)(data & 0xff);
			s2 += s1;

			s1 = s1 % crcBase;
			s2 = s2 % crcBase;

			value = (s2 << 0x10) + s1;
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
			uint s1 = value & 0xffff;
			uint s2 = (value >> 0x10) & 0xffff;

			while (length > 0)
			{
				int k = crcMax;
				if (k > length)
					k = length;
				length -= k;
				
				for (int i = 0; i < k; i++)
				{
					s1 += (uint)(data[start++] & 0xff);
					s2 += s1;
				}

				s1 = s1 % crcBase;
				s2 = s2 % crcBase;
			}
			value = (s2 << 0x10) | s1;
		}
		/// <summary>
		/// Resets checksum.
		/// </summary>
		public void Reset()
		{
			value = 1;
		}
		/// <summary>
		/// Returns current checksum value.
		/// </summary>
		public long Get()
		{
			return value;
		}

		// public static methods...
		/// <summary>
		/// Calculates Crc32 for the specified byte array.
		/// </summary>
		/// <param name="data">The array to calculate crc.</param>
		public static long Get(byte[] data)
		{
			Adler32 crc = new Adler32();
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
			Adler32 crc = new Adler32();
			crc.Add(data, start, length);
			return crc.Get();
		}
	}
}