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
	internal class InputStream : BinaryReader
	{
		CompressionStream zip = new CompressionStream();
		int bufsize = 512;
    FlushOption flushOption;
    byte[] buffer;
		bool compress;
		
		Stream stream = null;

    bool noMoreInput = false;

    // private methods...
    void InitializeStream(Stream stream, int level, bool compress)
    {
      flushOption = FlushOption.NoFlush;
      buffer = new byte[bufsize];

			this.stream = stream;			
			this.compress = compress;
			zip.Input = buffer;
			zip.InputIndex = 0;
			zip.InputCount = 0;

			if (compress)
				zip.InitializeDeflater(level);
			else
				zip.InitializeInflater();
    }

    // constructors...
		public InputStream(Stream stream)
      : base(stream)
		{
			InitializeStream(stream, 0, false);
		}
		public InputStream(Stream stream, int level)
      : base(stream)
		{
			InitializeStream(stream, level, true);
		}

    // public methods...
		public override int Read()
		{
      byte[] buf = new byte[1];
			if (Read(buf, 0, 1) == - 1)
				return -1;
			return (buf[0] & 0xff);
		}
		public override int Read(byte[] data, int offset, int length)
		{
			if (length == 0)
				return (0);

			int errorCode;
			zip.Output = data;
			zip.OutputIndex = offset;
			zip.OutputCount = length;
			do 
			{
				if ((zip.InputCount == 0) && (!noMoreInput))
				{
					// if buffer is empty and more input is avaiable, refill it
					zip.InputIndex = 0;
					zip.InputCount = Utils.ReadInput(stream, buffer, 0, bufsize); //(bufsize<z.avail_out ? bufsize : z.avail_out));
					if (zip.InputCount == - 1)
					{
						zip.InputCount = 0;
						noMoreInput = true;
					}
				}
				
				if (compress)
					errorCode = zip.Deflate((int)flushOption);
				else
					errorCode = zip.Inflate((int)flushOption);

				if (noMoreInput && (errorCode == (int)ErrorCode.BufferError))
					return -1;
				
				if (errorCode != (int)ErrorCode.Ok && errorCode != (int)ErrorCode.StreamEnd)
					throw new StreamException((compress?"de":"in") + "flating: " + zip.ErrorMessage);

				if (noMoreInput && (zip.OutputCount == length))
					return -1;
			}
			while (zip.OutputCount == length && errorCode == (int)ErrorCode.Ok);

			return length - zip.OutputCount;
		}
		public long Skip(long n)
		{
			int len = 512;
			if (n < len)
				len = (int) n;
			byte[] tmp = new byte[len];
			return ((long) Utils.ReadInput(BaseStream, tmp, 0, tmp.Length));
		}
		public override void Close()
		{
			stream.Close();
		}

    // public properties...
		/// <summary>
		/// Gets or sets flush mode.
		/// </summary>
    public FlushOption FlushMode
    {
			get { return flushOption; }
			set { flushOption = value; }
    }
    /// <summary>
    /// Returns the total number of bytes input so far.
    /// </summary>
    public long TotalIn
    {
			get { return zip.InputTotal; }
    }
    /// <summary>
    /// Returns the total number of bytes output so far.
    /// </summary>
    public long TotalOut
    {
			get { return zip.OutputTotal; }
    }
	}
}