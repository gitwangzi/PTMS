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
	internal sealed class InflaterBlocks
	{
		// And'ing with mask[n] masks the lower n bits		
		static readonly int[] inflateMask = new int[]
					{
						0x00000000, 0x00000001, 0x00000003, 0x00000007, 0x0000000f, 0x0000001f, 0x0000003f,
						0x0000007f, 0x000000ff, 0x000001ff, 0x000003ff, 0x000007ff, 0x00000fff, 0x00001fff,
						0x00003fff, 0x00007fff, 0x0000ffff
					};
		
		// Table for deflate from PKZIP's appnote.txt.		
		static readonly int[] border = new int[]{16, 17, 18, 0, 8, 7, 9, 6, 10, 5, 11, 4, 12, 3, 13, 2, 14, 1, 15};
		
		// current mode 
		int mode;
		
		// if STORED, bytes left to copy 
		int left;
		
		// table lengths (14 bits) 
		int table;

		// index into blens (or border)
		int index;
		
		// bit lengths of codes 
		int[] blens;

		// bit length tree depth
		int[] bitLengthTreeDepth = new int[1];

		// bit length decoding tree
		int[] bitLengthDecodeTree = new int[1];
		
		// if CODES, current state
		InflaterCodes codes;
		
		// true if this block is the last block
		int last;
		
		// single malloc for tree space
		int[] hufts;
		
		// check function
		object checkfn;
		
		// check on output
		long check;

		// bits in bit buffer
		int bitBufferLength;

		// bit buffer
		int bitBuffer;

		// sliding window
		byte[] window;

		// one byte after sliding window
		int windowEnd;

		// window read pointer
		int windowRead;
		
		// window write pointer
		int windowWrite;
		
		// constructors...
		internal InflaterBlocks(CompressionStream z, object checkfn, int w)
		{
			hufts = new int[Constants.Many * 3];
			Window = new byte[w];
			WindowEnd = w;
			this.checkfn = checkfn;
			mode = (int)InflaterBlocksState.Type;
			Reset(z, null);
		}

		// private methods...
		void ProcessTypeStored(InflaterBlocksContext state)
		{
			state.ShiftRight(3);
          
			// go to byte boundary
			state.Temp = state.Bits & 7;
			state.ShiftRight(state.Temp);
          
			// get length of stored block
			mode = (int)InflaterBlocksState.Lens;
		}
		void ProcessTypeFixed(CompressionStream zip, InflaterBlocksContext state)
		{
			int[] bl = new int[1];
			int[] bd = new int[1];
			int[][] tl = new int[1][];
			int[][] td = new int[1][];
          
			InflaterTree.InflateTreeFixed(bl, bd, tl, td, zip);
			codes = new InflaterCodes(bl[0], bd[0], tl[0], td[0], zip);
          
			state.ShiftRight(3);
          
			mode = (int)InflaterBlocksState.Codes;
		}
		void ProcessTypeDynamic(InflaterBlocksContext state)
		{
			state.ShiftRight(3);
          
			mode = (int)InflaterBlocksState.Table;
		}
		void TransferBlocksState(CompressionStream zip, InflaterBlocksContext state)
		{
			BitBuffer = state.BitBuf;
			BitBufferLength = state.Bits;
			zip.InputCount = state.BytesCount;
			zip.InputTotal += state.Input - zip.InputIndex;
			zip.InputIndex = state.Input;
			WindowWrite = state.Output;
		}
		int ProcessType(CompressionStream zip, InflaterBlocksContext state, int res)
		{
			while (state.Bits < 3)
			{
				if (state.BytesCount != 0)
					res = (int)ErrorCode.Ok;
				else
				{
					TransferBlocksState(zip, state);
					state.IsReturn = true;
					return InflateFlush(zip, res);
				}
				state.BytesCount--;
				state.BitBuf |= (zip.Input[state.Input++] & 0xff) << state.Bits;
				state.Bits += 8;
			}
			state.Temp = (int)(state.BitBuf & 7);
			last = state.Temp & 1;
            
			switch (Utils.ShiftRight(state.Temp, 1))
			{
				// stored
				case 0:
				{
					ProcessTypeStored(state);
					break;
				}
            
				// fixed
				case 1:
				{
					ProcessTypeFixed(zip, state);
					break;
				}
        
				// dynamic
				case 2:
				{
					ProcessTypeDynamic(state);
					break;
				}

				// illegal
				case 3:
				{
					state.ShiftRight(3);

					mode = (int)InflaterBlocksState.Bad;
					zip.ErrorMessage = "invalid block type";
					res = (int)ErrorCode.DataError;
            
					TransferBlocksState(zip, state);
					state.IsReturn = true;
					return InflateFlush(zip, res);
				}
			}
			return 0;
		}
		int ProcessLens(CompressionStream zip, InflaterBlocksContext state, ref int res)
		{
			while (state.Bits < (32))
			{
				if (state.BytesCount != 0)
					res = (int)ErrorCode.Ok;
				else
				{
					TransferBlocksState(zip, state);
					state.IsReturn = true;
					return InflateFlush(zip, res);
				}
				state.BytesCount--;
				state.BitBuf |= (zip.Input[state.Input++] & 0xff) << state.Bits;
				state.Bits += 8;
			}
            
			if (((Utils.ShiftRight((~ state.BitBuf), 16)) & 0xffff) != (state.BitBuf & 0xffff))
			{
				mode = (int)InflaterBlocksState.Bad;
				zip.ErrorMessage = "invalid stored block lengths";
				res = (int)ErrorCode.DataError;
            
				TransferBlocksState(zip, state);
				state.IsReturn = true;
				return InflateFlush(zip, res);
			}
			left = (state.BitBuf & 0xffff);
			state.BitBuf = state.Bits = 0; // dump bits

			if (left != 0)
				mode = (int)InflaterBlocksState.Stored;
			else
			{
				if (last != 0)
					mode = (int)InflaterBlocksState.Dry;
				else
					mode = (int)InflaterBlocksState.Type;
			}
			return 0;
		}
		int GetEndBytes(InflaterBlocksContext state)
		{
			return (int)(state.Output < WindowRead ? WindowRead - state.Output - 1 : WindowEnd - state.Output);
		}
		int ProcessStored(CompressionStream zip, InflaterBlocksContext state, int res)
		{
			if (state.BytesCount == 0)
			{
				TransferBlocksState(zip, state);
				state.IsReturn = true;
				return InflateFlush(zip, res);
			}
            
			if (state.EndBytes == 0)
			{
				if (state.Output == WindowEnd && WindowRead != 0)
				{
					state.Output = 0;
					state.EndBytes = GetEndBytes(state);
				}
				if (state.EndBytes == 0)
				{
					WindowWrite = state.Output;
					res = InflateFlush(zip, res);
					state.Output = WindowWrite;
					state.EndBytes = GetEndBytes(state);
					if (state.Output == WindowEnd && WindowRead != 0)
					{
						state.Output = 0;
						state.EndBytes = GetEndBytes(state);
					}
					if (state.EndBytes == 0)
					{
						TransferBlocksState(zip, state);
						state.IsReturn = true;
						return InflateFlush(zip, res);
					}
				}
			}
			res = (int)ErrorCode.Ok;
            
			state.Temp = left;
			if (state.Temp > state.BytesCount)
				state.Temp = state.BytesCount;

			if (state.Temp > state.EndBytes)
				state.Temp = state.EndBytes;

			Array.Copy(zip.Input, state.Input, Window, state.Output, state.Temp);
			state.Input += state.Temp;
			state.BytesCount -= state.Temp;
			state.Output += state.Temp;
			state.EndBytes -= state.Temp;
			return 0;
		}
		int ProcessTable(CompressionStream zip, InflaterBlocksContext state, int res)
		{
			while (state.Bits < (14))
			{
				if (state.BytesCount != 0)
					res = (int)ErrorCode.Ok;
				else
				{
					TransferBlocksState(zip, state);
					state.IsReturn = true;
					return InflateFlush(zip, res);
				}				
				state.BytesCount--;
				state.BitBuf |= (zip.Input[state.Input++] & 0xff) << state.Bits;
				state.Bits += 8;
			}
            
			table = state.Temp = (state.BitBuf & 0x3fff);
			if ((state.Temp & 0x1f) > 29 || ((state.Temp >> 5) & 0x1f) > 29)
			{
				mode = (int)InflaterBlocksState.Bad;
				zip.ErrorMessage = "too many length or distance symbols";
				res = (int)ErrorCode.DataError;
            
				TransferBlocksState(zip, state);
				state.IsReturn = true;
				return InflateFlush(zip, res);
			}
			state.Temp = 258 + (state.Temp & 0x1f) + ((state.Temp >> 5) & 0x1f);
			blens = new int[state.Temp];

			if (blens == null || blens.Length < state.Temp)
				blens = new int[state.Temp];
			else
			{
				for (int i = 0; i < state.Temp; i++)
					blens[i] = 0;
			}

			state.ShiftRight(14);
            
			index = 0;
			mode = (int)InflaterBlocksState.BTree;
			return 0;
		}
		int ProcessBTree(CompressionStream zip, InflaterBlocksContext state, int res)
		{
			while (index < 4 + (Utils.ShiftRight(table, 10)))
			{
				while (state.Bits < 3)
				{
					if (state.BytesCount != 0)
						res = (int)ErrorCode.Ok;
					else
					{
						TransferBlocksState(zip, state);
						state.IsReturn = true;
						return InflateFlush(zip, res);
					}

					state.BytesCount--;
					state.BitBuf |= (zip.Input[state.Input++] & 0xff) << state.Bits;
					state.Bits += 8;
				}
          
				blens[border[index++]] = state.BitBuf & 7;

				state.ShiftRight(3);
			}
          
			while (index < 19)
				blens[border[index++]] = 0;
          
			bitLengthTreeDepth[0] = 7;
			state.Temp = InflaterTree.InflateTreeBits(blens, bitLengthTreeDepth, bitLengthDecodeTree, hufts, zip);
			if (state.Temp != (int)ErrorCode.Ok)
			{
				res = state.Temp;
				if (res == (int)ErrorCode.DataError)
				{
					blens = null;
					mode = (int)InflaterBlocksState.Bad;
				}
          
				TransferBlocksState(zip, state);
				state.IsReturn = true;
				return InflateFlush(zip, res);
			}
          
			index = 0;
			mode = (int)InflaterBlocksState.DTree;
			return 0;
		}
		int ProcessDTree(CompressionStream zip, InflaterBlocksContext state, int res)
		{
			while (true)
			{
				state.Temp = table;
				if (!(index < 258 + (state.Temp & 0x1f) + ((state.Temp >> 5) & 0x1f)))
					break;

				int i, j, c;
				state.Temp = bitLengthTreeDepth[0];
				while (state.Bits < (state.Temp))
				{
					if (state.BytesCount != 0)
						res = (int)ErrorCode.Ok;
					else
					{
						TransferBlocksState(zip, state);
						state.IsReturn = true;
						return InflateFlush(zip, res);
					}

					state.BytesCount--;
					state.BitBuf |= (zip.Input[state.Input++] & 0xff) << state.Bits;
					state.Bits += 8;
				}
            
				state.Temp = hufts[(bitLengthDecodeTree[0] + (state.BitBuf & inflateMask[state.Temp])) * 3 + 1];
				c = hufts[(bitLengthDecodeTree[0] + (state.BitBuf & inflateMask[state.Temp])) * 3 + 2];
            
				if (c < 16)
				{
					state.ShiftRight(state.Temp);
					blens[index++] = c;
				}
				else
				{
					// c == 16..18
					i = c == 18 ? 7 : c - 14;
					j = c == 18 ? 11 : 3;
            
					while (state.Bits < (state.Temp + i))
					{
						if (state.BytesCount != 0)
							res = (int)ErrorCode.Ok;
						else
						{
							TransferBlocksState(zip, state);
							state.IsReturn = true;
							return InflateFlush(zip, res);
						}

						state.BytesCount--;
						state.BitBuf |= (zip.Input[state.Input++] & 0xff) << state.Bits;
						state.Bits += 8;
					}
            
					state.ShiftRight(state.Temp);
            
					j += (state.BitBuf & inflateMask[i]);
            
					state.ShiftRight(i);
            
					i = index;
					state.Temp = table;
					if (i + j > 258 + (state.Temp & 0x1f) + ((state.Temp >> 5) & 0x1f) || (c == 16 && i < 1))
					{
						blens = null;
						mode = (int)InflaterBlocksState.Bad;
						zip.ErrorMessage = "invalid bit length repeat";
						res = (int)ErrorCode.DataError;
            
						TransferBlocksState(zip, state);
						state.IsReturn = true;
						return InflateFlush(zip, res);
					}
            
					c = c == 16 ? blens[i - 1] : 0;
					do
					{
						blens[i++] = c;
					}
					while (--j != 0);
					index = i;
				}
			}
            
			bitLengthDecodeTree[0] = - 1;
			int[] bl = new int[1];
			int[] bd = new int[1];
			int[] tl = new int[1];
			int[] td = new int[1];
			bl[0] = 9; // must be <= 9 for lookahead assumptions
			bd[0] = 6; // must be <= 9 for lookahead assumptions
			state.Temp = table;
			state.Temp = InflaterTree.InflateTreeDynamic(257 + (state.Temp & 0x1f), 1 + ((state.Temp >> 5) & 0x1f), blens, bl, bd, tl, td, hufts, zip);
			if (state.Temp != (int)ErrorCode.Ok)
			{
				if (state.Temp == (int)ErrorCode.DataError)
				{
					blens = null;
					mode = (int)InflaterBlocksState.Bad;
				}
				res = state.Temp;
            
				TransferBlocksState(zip, state);
				state.IsReturn = true;
				return InflateFlush(zip, res);
			}
            
			codes = new InflaterCodes(bl[0], bd[0], hufts, tl[0], hufts, td[0], zip);			
			mode = (int)InflaterBlocksState.Codes;
			return 0;
		}

		
		// internal methods...
		internal void Reset(CompressionStream z, long[] checksum)
		{
			if (checksum != null)
				checksum[0] = check;
			
			if (mode == (int)InflaterBlocksState.BTree || mode == (int)InflaterBlocksState.DTree)
				blens = null;
			
			if (mode == (int)InflaterBlocksState.Codes)
				codes.Free(z);
			
			mode = (int)InflaterBlocksState.Type;
			BitBufferLength = 0;
			BitBuffer = 0;
			WindowRead = WindowWrite = 0;

			if (checkfn != null)
			{
				z.Checksum = new Adler32();
				check = z.Checksum.Get();
			}
		}		
		
		internal int Process(CompressionStream zip, int res)
		{
			InflaterBlocksContext state = new InflaterBlocksContext();
			
			// copy input/output information to locals (UPDATE macro restores)
			state.Input = zip.InputIndex;
			state.BytesCount = zip.InputCount;
			state.BitBuf = BitBuffer;
			state.Bits = BitBufferLength;
			
			state.Output = WindowWrite;			
			state.EndBytes = GetEndBytes(state);

			int result;
			
			// process input based on current state
			while (true)
			{
				switch (mode)
				{
					case (int)InflaterBlocksState.Type: 
						
						state.IsReturn = false;
						result = ProcessType(zip, state, res);
						if (state.IsReturn)
							return result;
						break;
					
					case (int)InflaterBlocksState.Lens: 
						
						state.IsReturn = false;
						result = ProcessLens(zip, state, ref res);
						if (state.IsReturn)
							return result;

						break;
					
					case (int)InflaterBlocksState.Stored: 
						
						state.IsReturn = false;
						result = ProcessStored(zip, state, res);
						if (state.IsReturn)
							return result;
						
						if ((left -= state.Temp) != 0)
							break;
						
						if (last != 0)
							mode = (int)InflaterBlocksState.Dry;
						else
							mode = (int)InflaterBlocksState.Type;
						break;
					
					case (int)InflaterBlocksState.Table: 
						
						state.IsReturn = false;
						result = ProcessTable(zip, state, res);
						if (state.IsReturn)
							return result;

						goto case (int)InflaterBlocksState.BTree;
					
					case (int)InflaterBlocksState.BTree: 

						state.IsReturn = false;
						result = ProcessBTree(zip, state, res);
						if (state.IsReturn)
							return result;

						goto case (int)InflaterBlocksState.DTree;
					
					case (int)InflaterBlocksState.DTree: 

						state.IsReturn = false;
						result = ProcessDTree(zip, state, res);
						if (state.IsReturn)
							return result;

						goto case (int)InflaterBlocksState.Codes;
					
					case (int)InflaterBlocksState.Codes: 
						TransferBlocksState(zip, state);
						
						if ((res = codes.Process(this, zip, res)) != (int)ErrorCode.StreamEnd)
							return InflateFlush(zip, res);
						res = (int)ErrorCode.Ok;
						codes.Free(zip);
						
						state.Input = zip.InputIndex;
						state.BytesCount = zip.InputCount;
						state.BitBuf = BitBuffer;
						state.Bits = BitBufferLength;
						state.Output = WindowWrite;
						state.EndBytes = GetEndBytes(state);
						
						if (last == 0)
						{
							mode = (int)InflaterBlocksState.Type;
							break;
						}
						mode = (int)InflaterBlocksState.Dry;
						goto case (int)InflaterBlocksState.Dry;
					
					case (int)InflaterBlocksState.Dry: 
						WindowWrite = state.Output;
						res = InflateFlush(zip, res);
						state.Output = WindowWrite;
						state.EndBytes = GetEndBytes(state);
						if (WindowRead != WindowWrite)
						{
							TransferBlocksState(zip, state);
							return InflateFlush(zip, res);
						}
						mode = (int)InflaterBlocksState.Done;
						goto case (int)InflaterBlocksState.Done;
					
					case (int)InflaterBlocksState.Done: 
						res = (int)ErrorCode.StreamEnd;						
						TransferBlocksState(zip, state);
						return InflateFlush(zip, res);
					
					case (int)InflaterBlocksState.Bad: 
						res = (int)ErrorCode.DataError;						
						TransferBlocksState(zip, state);
						return InflateFlush(zip, res);					
					
					default: 
						res = (int)ErrorCode.StreamError;						
						TransferBlocksState(zip, state);
						return InflateFlush(zip, res);					
				}
			}
		}
		
		internal void Free(CompressionStream z)
		{
			Reset(z, null);
			Window = null;
			hufts = null;
		}
		
		internal void SetDictionary(byte[] d, int start, int n)
		{
			Array.Copy(d, start, Window, 0, n);
			WindowRead = WindowWrite = n;
		}
		
		/// <summary>
		/// Returns true if inflate is currently at the end of a block generated by sync flush or full flush. 
		/// </summary>
		/// <returns></returns>
		internal int GetSyncPoint()
		{
			return mode == (int)InflaterBlocksState.Lens ? 1 : 0;
		}
		
		/// <summary>
		/// Copy as much as possible from the sliding window to the output area.
		/// </summary>
		/// <param name="z"></param>
		/// <param name="r"></param>
		/// <returns></returns>
		internal int InflateFlush(CompressionStream z, int r)
		{
			int n;
			int p;
			int q;
			
			// local copies of source and destination pointers
			p = z.OutputIndex;
			q = WindowRead;
			
			// compute number of bytes to copy as far as end of window
			n = (int) ((q <= WindowWrite ? WindowWrite : WindowEnd) - q);
			if (n > z.OutputCount)
				n = z.OutputCount;
			if (n != 0 && r == (int)ErrorCode.BufferError)
				r = (int)ErrorCode.Ok;
			
			// update counters
			z.OutputCount -= n;
			z.OutputTotal += n;
			
			// update check information
			if (checkfn != null)
			{
				z.Checksum = new Adler32((uint)check);
				z.Checksum.Add(Window, q, n);
				check = z.Checksum.Get();
			}
			
			// copy as far as end of window
			Array.Copy(Window, q, z.Output, p, n);
			p += n;
			q += n;
			
			// see if more to copy at beginning of window
			if (q == WindowEnd)
			{
				// wrap pointers
				q = 0;
				if (WindowWrite == WindowEnd)
					WindowWrite = 0;
				
				// compute bytes to copy
				n = WindowWrite - q;
				if (n > z.OutputCount)
					n = z.OutputCount;
				if (n != 0 && r == (int)ErrorCode.BufferError)
					r = (int)ErrorCode.Ok;
				
				// update counters
				z.OutputCount -= n;
				z.OutputTotal += n;
				
				// update check information
				if (checkfn != null)
				{
					z.Checksum = new Adler32((uint)check);
					z.Checksum.Add(Window, q, n);
					check = z.Checksum.Get();
				}
				
				// copy
				Array.Copy(Window, q, z.Output, p, n);
				p += n;
				q += n;
			}
			
			// update pointers
			z.OutputIndex = p;
			WindowRead = q;
			
			// done
			return r;
		}

		// public properties...		
		/// <summary>
		/// Gets or sets the number of bits in bit buffer.
		/// </summary>
		public int BitBufferLength
		{
			get { return bitBufferLength; }
			set { bitBufferLength = value; }
		}
		/// <summary>
		/// Gets or sets bit buffer.
		/// </summary>
		public int BitBuffer
		{
			get { return bitBuffer; }
			set { bitBuffer = value; }
		}
		/// <summary>
		/// Gets or sets window byte array.
		/// </summary>
		public byte[] Window
		{
			get { return window; }
			set { window = value; }
		}
		/// <summary>
		/// Gets or sets one byte after windwo byte array.
		/// </summary>
		public int WindowEnd
		{
			get { return windowEnd; }
			set { windowEnd = value; }
		}
		/// <summary>
		/// Gets or sets window read pointer.
		/// </summary>
		public int WindowRead
		{
			get { return windowRead; }
			set { windowRead = value; }
		}
		/// <summary>
		/// Gets or sets window write pointer.
		/// </summary>
		public int WindowWrite
		{
			get { return windowWrite; }
			set { windowWrite = value; }
		}
	}
}