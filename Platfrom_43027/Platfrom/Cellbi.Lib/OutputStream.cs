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
	/// Compression output stream.
	/// </summary>
	public class OutputStream : Stream
	{
    Stream stream;
    CompressionStream zip = new CompressionStream();
    FlushOption flushOption;
        
    byte[] buffer = new byte[1];
    int bufsize = 4096 + 2048;
        
    bool compress;

    // private methods...
    void InitializeStream(Stream stream, int level, bool compress)
    {
      flushOption = FlushOption.NoFlush;
      buffer = new byte[bufsize];

			this.stream = stream;
			this.compress = compress;

			if (compress)
				zip.InitializeDeflater(level);
			else
				zip.InitializeInflater();
    }

    // constructors...
		/// <summary>
		/// Creates new instance of the output compression stream.
		/// </summary>
		/// <param name="stream">The underlying stream to use.</param>
    public OutputStream(Stream stream)
      : base()
    {
      InitializeStream(stream, 0, false);
    }
		/// <summary>
		/// Creates new instance of the output compression stream.
		/// </summary>
		/// <param name="stream">The underlying stream to use.</param>
		/// <param name="level">The compression level to use.</param>
    public OutputStream(Stream stream, int level)
      : base()
    {
      InitializeStream(stream, level, true);
    }

    // public methods...
		/// <summary>
		/// Writes the given byte to the stream.
		/// </summary>
		/// <param name="b">The value to write.</param>
    public void WriteByte(int b)
		{
      WriteByte((byte)b);
		}
		/// <summary>
		/// Writes the given byte to the stream.
		/// </summary>
		/// <param name="b">The value to write.</param>
		public override void WriteByte(byte b)
		{
      byte[] buf = new byte[] { b };
      Write(buf, 0, 1);
		}
		/// <summary>
		/// Writes the array of bytes to the stream.
		/// </summary>
		/// <param name="data">The date to write.</param>
		/// <param name="start">The start index.</param>
		/// <param name="length">The length of data to write.</param>
		public override void Write(byte[] data, int start, int length)
		{
			if (length == 0)
				return ;
			int errorCode;
			byte[] bytes = new byte[data.Length];
			Array.Copy(data, 0, bytes, 0, data.Length);
			zip.Input = bytes;
			zip.InputIndex = start;
			zip.InputCount = length;
			do 
			{
				zip.Output = buffer;
				zip.OutputIndex = 0;
				zip.OutputCount = bufsize;

				if (compress)
					errorCode = zip.Deflate((int)flushOption);
				else
					errorCode = zip.Inflate((int)flushOption);

				if (errorCode != (int)ErrorCode.Ok && errorCode != (int)ErrorCode.StreamEnd) 
					throw new StreamException((compress?"de":"in") + "flating: " + zip.ErrorMessage);

				stream.Write(buffer, 0, bufsize - zip.OutputCount);
				if (errorCode == (int)ErrorCode.StreamEnd)
					break;
			}
			while (zip.InputCount > 0 || zip.OutputCount == 0);
		}
		/// <summary>
		/// Closes the stream.
		/// </summary>
		public override void Close()
		{
			try
			{
				try
				{
					Finish();
				}
				catch
				{
				}
			}
			finally
			{
				End();
				stream = null;
			}
		}
		/// <summary>
		/// Finishes all compression stream operations.
		/// </summary>
		public void Finish()
		{
			int err;
			do
			{
				zip.Output = buffer;
				zip.OutputIndex = 0;
				zip.OutputCount = bufsize;
				if (compress)
					err = zip.Deflate((int)FlushOption.Finish);
				else
					err = zip.Inflate((int)FlushOption.Finish);
				if (err != (int)ErrorCode.StreamEnd && err != (int)ErrorCode.Ok)
					throw new StreamException((compress ? "de" : "in") + "flating: " + zip.ErrorMessage);
				if (bufsize - zip.OutputCount > 0)
					stream.Write(buffer, 0, bufsize - zip.OutputCount);
				if (err == (int)ErrorCode.StreamEnd)
					break;
			}
			while (zip.InputCount > 0 || zip.OutputCount == 0);
			try
			{
				Flush();
			}
			catch
			{
			}
		}
		/// <summary>
		/// Ends output stream.
		/// </summary>
    public void End()
    {
      if (compress)
        zip.DeflateEnd();
      else
        zip.InflateEnd();
      zip.Free();
      zip = null;
    }
		/// <summary>
		/// Flushes the stream.
		/// </summary>
		public override void Flush()
		{
			stream.Flush();
		}
		/// <summary>
		/// Reads array of bytes from the stream.
		/// </summary>
		/// <param name="buffer">The buffer to read to.</param>
		/// <param name="offset">The start offset in the buffer to fill.</param>
		/// <param name="count">The number of bytes to read.</param>
		/// <returns></returns>
		public override int Read(byte[] buffer, int offset, int count)
		{
			return stream.Read(buffer, offset, count);
		}
		/// <summary>
		/// Sets stream length to the given value.
		/// </summary>
		/// <param name="value">The length of the stream.</param>
		public override void SetLength(long value)
		{
      stream.SetLength(value);
		}
		/// <summary>
		/// Seeks in the stream.
		/// </summary>
		/// <param name="offset">The start offset to use.</param>
		/// <param name="origin">The origin of seek operation.</param>
		/// <returns></returns>
		public override long Seek(long offset, SeekOrigin origin)
		{
      return stream.Seek(offset, origin);
		}
    
    // public properties...
		/// <summary>
		/// Gets or sets flush mode.
		/// </summary>
    public FlushOption FlushMode
    {
			get { return (flushOption); }
			set { flushOption = value; }
    }
    /// <summary>
    /// Returns the total number of bytes input so far.
    /// </summary>
    public virtual long TotalIn
    {
			get { return zip.InputTotal; }
    }
    /// <summary>
    /// Returns the total number of bytes output so far.
    /// </summary>
    public virtual long TotalOut
    {
			get { return zip.OutputTotal; }
    }
		/// <summary>
		/// Returns true if stream can be read.
		/// </summary>
    public override bool CanRead
    {
			get { return stream.CanRead; }
    }
		/// <summary>
		/// Returns true if seek can be done on the stream.
		/// </summary>
    public override bool CanSeek
    {
			get { return stream.CanSeek; }
    }
		/// <summary>
		/// Returns true if stream is writeable.
		/// </summary>
    public override bool CanWrite
    {
			get { return stream.CanWrite; }
    }
		/// <summary>
		/// Gets the stream length.
		/// </summary>
    public override long Length
    {
			get { return stream.Length; }
    }
		/// <summary>
		/// Gets stream position.
		/// </summary>
    public override long Position
    {
			get { return stream.Position; }
			set { stream.Position = value; }
    }
	}
}