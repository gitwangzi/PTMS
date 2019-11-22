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
	sealed class InflaterCodes
	{
		static readonly int[] inflateMask = new int[]
					{
						0x00000000, 0x00000001, 0x00000003, 0x00000007, 0x0000000f, 0x0000001f, 0x0000003f,
						0x0000007f, 0x000000ff, 0x000001ff, 0x000003ff, 0x000007ff, 0x00000fff, 0x00001fff,
						0x00003fff, 0x00007fff, 0x0000ffff
					};			
		
		int mode; // current inflate_codes mode
		
		// mode dependent information
		int len;
		
		int[] tree; // pointer into tree
		int treeIndex = 0;
		int need; // bits needed
		
		int lit;
		
		// if EXT or COPY, where and how much
		int getBits; // bits to get for extra
		int dist; // distance back to copy from
		
		byte lbits; // ltree bits decoded per branch
		byte dbits; // dtree bits decoder per branch
		int[] ltree; // literal/length/eob tree
		int ltreeIndex; // literal/length/eob tree
		int[] dtree; // distance tree
		int dtreeIndex; // distance tree
		
		// constructors...
		internal InflaterCodes(int bl, int bd, int[] tl, int tlIndex, int[] td, int tdIndex, CompressionStream z)
		{
			mode = (int)InflaterCodesState.Start;
			lbits = (byte) bl;
			dbits = (byte) bd;
			ltree = tl;
			ltreeIndex = tlIndex;
			dtree = td;
			dtreeIndex = tdIndex;
		}		
		internal InflaterCodes(int bl, int bd, int[] tl, int[] td, CompressionStream z)
		{
			mode = (int)InflaterCodesState.Start;
			lbits = (byte) bl;
			dbits = (byte) bd;
			ltree = tl;
			ltreeIndex = 0;
			dtree = td;
			dtreeIndex = 0;
		}
		
		// internal methods...
		internal int Process(InflaterBlocks s, CompressionStream z, int r)
		{
			int j; // temporary storage
			int tindex; // temporary pointer
			int e; // extra bits or operation
			int b = 0; // bit buffer
			int k = 0; // bits in bit buffer
			int p = 0; // input data pointer
			int n; // bytes available there
			int q; // output window write pointer
			int m; // bytes to end of window or read pointer
			int f; // pointer to copy strings from
			
			// copy input/output information to locals (UPDATE macro restores)
			p = z.InputIndex;
			n = z.InputCount;
			b = s.BitBuffer;
			k = s.BitBufferLength;
			q = s.WindowWrite;
			m = q < s.WindowRead ? s.WindowRead - q - 1 : s.WindowEnd - q;
			
			// process input and output based on current state
			while (true)
			{
				switch (mode)
				{
					// waiting for "i:"=input, "o:"=output, "x:"=nothing
					case (int)InflaterCodesState.Start:  // x: set up for LEN
						if (m >= 258 && n >= 10)
						{
							s.BitBuffer = b;
							s.BitBufferLength = k;
							z.InputCount = n;
							z.InputTotal += p - z.InputIndex;
							z.InputIndex = p;
							s.WindowWrite = q;

							r = InflateFast(lbits, dbits, ltree, ltreeIndex, dtree, dtreeIndex, s, z);
							
							p = z.InputIndex;
							n = z.InputCount;
							b = s.BitBuffer;
							k = s.BitBufferLength;
							q = s.WindowWrite;
							m = q < s.WindowRead ? s.WindowRead - q - 1 : s.WindowEnd - q;
							
							if (r != (int)ErrorCode.Ok)
							{
								mode = r == (int)ErrorCode.StreamEnd ? (int)InflaterCodesState.Wash : (int)InflaterCodesState.BadCode;
								break;
							}
						}
						need = lbits;
						tree = ltree;
						treeIndex = ltreeIndex;
						
						mode = (int)InflaterCodesState.Len;
						goto case (int)InflaterCodesState.Len;
					
					case (int)InflaterCodesState.Len:  // i: get length/literal/eob next
						j = need;
						
						while (k < (j))
						{
							if (n != 0)
								r = (int)ErrorCode.Ok;
							else
							{
								s.BitBuffer = b;
								s.BitBufferLength = k;
								z.InputCount = n;
								z.InputTotal += p - z.InputIndex;
								z.InputIndex = p;
								s.WindowWrite = q;

								return s.InflateFlush(z, r);
							}
							n--;
							b |= (z.Input[p++] & 0xff) << k;
							k += 8;
						}
						
						tindex = (treeIndex + (b & inflateMask[j])) * 3;
						
						b = Utils.ShiftRight(b, (tree[tindex + 1]));
						k -= (tree[tindex + 1]);
						
						e = tree[tindex];
						
						if (e == 0)
						{
							// literal
							lit = tree[tindex + 2];
							mode = (int)InflaterCodesState.Lit;
							break;
						}
						if ((e & 16) != 0)
						{
							// length
							getBits = e & 15;
							len = tree[tindex + 2];
							mode = (int)InflaterCodesState.LenText;
							break;
						}
						if ((e & 64) == 0)
						{
							// next table
							need = e;
							treeIndex = tindex / 3 + tree[tindex + 2];
							break;
						}
						if ((e & 32) != 0)
						{
							// end of block
							mode = (int)InflaterCodesState.Wash;
							break;
						}
						mode = (int)InflaterCodesState.BadCode; // invalid code
						z.ErrorMessage = "invalid literal/length code";
						r = (int)ErrorCode.DataError;
						
						s.BitBuffer = b;
						s.BitBufferLength = k;
						z.InputCount = n;
						z.InputTotal += p - z.InputIndex;
						z.InputIndex = p;
						s.WindowWrite = q;
						return s.InflateFlush(z, r);
					
					case (int)InflaterCodesState.LenText:  // i: getting length extra (have base)
						j = getBits;
						
						while (k < (j))
						{
							if (n != 0)
								r = (int)ErrorCode.Ok;
							else
							{	
								s.BitBuffer = b;
								s.BitBufferLength = k;
								z.InputCount = n;
								z.InputTotal += p - z.InputIndex;
								z.InputIndex = p;
								s.WindowWrite = q;
								return s.InflateFlush(z, r);
							}
							n--; b |= (z.Input[p++] & 0xff) << k;
							k += 8;
						}
						
						len += (b & inflateMask[j]);
						
						b >>= j;
						k -= j;
						
						need = dbits;
						tree = dtree;
						treeIndex = dtreeIndex;
						mode = (int)InflaterCodesState.Dist;
						goto case (int)InflaterCodesState.Dist;
					
					case (int)InflaterCodesState.Dist:  // i: get distance next
						j = need;
						
						while (k < (j))
						{
							if (n != 0)
								r = (int)ErrorCode.Ok;
							else
							{
								s.BitBuffer = b;
								s.BitBufferLength = k;
								z.InputCount = n;
								z.InputTotal += p - z.InputIndex;
								z.InputIndex = p;
								s.WindowWrite = q;
								return s.InflateFlush(z, r);
							}
							n--; b |= (z.Input[p++] & 0xff) << k;
							k += 8;
						}
						
						tindex = (treeIndex + (b & inflateMask[j])) * 3;
						
						b >>= tree[tindex + 1];
						k -= tree[tindex + 1];
						
						e = (tree[tindex]);
						if ((e & 16) != 0)
						{
							// distance
							getBits = e & 15;
							dist = tree[tindex + 2];
							mode = (int)InflaterCodesState.DistExt;
							break;
						}
						if ((e & 64) == 0)
						{
							// next table
							need = e;
							treeIndex = tindex / 3 + tree[tindex + 2];
							break;
						}
						mode = (int)InflaterCodesState.BadCode; // invalid code
						z.ErrorMessage = "invalid distance code";
						r = (int)ErrorCode.DataError;
						
						s.BitBuffer = b;
						s.BitBufferLength = k;
						z.InputCount = n;
						z.InputTotal += p - z.InputIndex;
						z.InputIndex = p;
						s.WindowWrite = q;
						return s.InflateFlush(z, r);
					
					
					case (int)InflaterCodesState.DistExt:  // i: getting distance extra
						j = getBits;
						
						while (k < (j))
						{
							if (n != 0)
								r = (int)ErrorCode.Ok;
							else
							{		
								s.BitBuffer = b;
								s.BitBufferLength = k;
								z.InputCount = n;
								z.InputTotal += p - z.InputIndex;
								z.InputIndex = p;
								s.WindowWrite = q;
								return s.InflateFlush(z, r);
							}
							n--; b |= (z.Input[p++] & 0xff) << k;
							k += 8;
						}
						
						dist += (b & inflateMask[j]);
						
						b >>= j;
						k -= j;
						
						mode = (int)InflaterCodesState.Copy;
						goto case (int)InflaterCodesState.Copy;
					
					case (int)InflaterCodesState.Copy:  // o: copying bytes in window, waiting for space
						f = q - dist;
						while (f < 0)
						{
							// modulo window size-"while" instead
							f += s.WindowEnd; // of "if" handles invalid distances
						}
						while (len != 0)
						{
							
							if (m == 0)
							{
								if (q == s.WindowEnd && s.WindowRead != 0)
								{
									q = 0;
									m = q < s.WindowRead ? s.WindowRead - q - 1 : s.WindowEnd - q;
								}
								if (m == 0)
								{
									s.WindowWrite = q;
									r = s.InflateFlush(z, r);
									q = s.WindowWrite;
									m = q < s.WindowRead ? s.WindowRead - q - 1 : s.WindowEnd - q;
									
									if (q == s.WindowEnd && s.WindowRead != 0)
									{
										q = 0;
										m = q < s.WindowRead ? s.WindowRead - q - 1 : s.WindowEnd - q;
									}
									
									if (m == 0)
									{
										s.BitBuffer = b;
										s.BitBufferLength = k;
										z.InputCount = n;
										z.InputTotal += p - z.InputIndex;
										z.InputIndex = p;
										s.WindowWrite = q;
										return s.InflateFlush(z, r);
									}
								}
							}
							
							s.Window[q++] = s.Window[f++]; m--;
							
							if (f == s.WindowEnd)
								f = 0;
							len--;
						}
						mode = (int)InflaterCodesState.Start;
						break;
					
					case (int)InflaterCodesState.Lit:  // o: got literal, waiting for output space
						if (m == 0)
						{
							if (q == s.WindowEnd && s.WindowRead != 0)
							{
								q = 0;
								m = q < s.WindowRead ? s.WindowRead - q - 1 : s.WindowEnd - q;
							}
							if (m == 0)
							{
								s.WindowWrite = q;
								r = s.InflateFlush(z, r);
								q = s.WindowWrite;
								m = q < s.WindowRead ? s.WindowRead - q - 1 : s.WindowEnd - q;
								
								if (q == s.WindowEnd && s.WindowRead != 0)
								{
									q = 0;
									m = q < s.WindowRead ? s.WindowRead - q - 1 : s.WindowEnd - q;
								}
								if (m == 0)
								{
									s.BitBuffer = b;
									s.BitBufferLength = k;
									z.InputCount = n;
									z.InputTotal += p - z.InputIndex;
									z.InputIndex = p;
									s.WindowWrite = q;
									return s.InflateFlush(z, r);
								}
							}
						}
						r = (int)ErrorCode.Ok;
						
						s.Window[q++] = (byte)lit;
						m--;
						
						mode = (int)InflaterCodesState.Start;
						break;
					
					case (int)InflaterCodesState.Wash:  // o: got eob, possibly more output
						if (k > 7)
						{
							// return unused byte, if any
							k -= 8;
							n++;
							p--; // can always return one
						}
						
						s.WindowWrite = q;
						r = s.InflateFlush(z, r);
						q = s.WindowWrite;
						m = q < s.WindowRead ? s.WindowRead - q - 1 : s.WindowEnd - q;
						
						if (s.WindowRead != s.WindowWrite)
						{
							s.BitBuffer = b;
							s.BitBufferLength = k;
							z.InputCount = n;
							z.InputTotal += p - z.InputIndex;
							z.InputIndex = p;
							s.WindowWrite = q;
							return s.InflateFlush(z, r);
						}
						mode = (int)InflaterCodesState.End;
						goto case (int)InflaterCodesState.End;
					
					case (int)InflaterCodesState.End: 
						r = (int)ErrorCode.StreamEnd;
						s.BitBuffer = b;
						s.BitBufferLength = k;
						z.InputCount = n;
						z.InputTotal += p - z.InputIndex;
						z.InputIndex = p;
						s.WindowWrite = q;
						return s.InflateFlush(z, r);
					
					
					case (int)InflaterCodesState.BadCode:  // x: got error
						r = (int)ErrorCode.DataError;
						
						s.BitBuffer = b;
						s.BitBufferLength = k;
						z.InputCount = n;
						z.InputTotal += p - z.InputIndex;
						z.InputIndex = p;
						s.WindowWrite = q;
						return s.InflateFlush(z, r);
					
					
					default: 
						r = (int)ErrorCode.StreamError;
						
						s.BitBuffer = b;
						s.BitBufferLength = k;
						z.InputCount = n;
						z.InputTotal += p - z.InputIndex;
						z.InputIndex = p;
						s.WindowWrite = q;
						return s.InflateFlush(z, r);
					
				}
			}
		}		
		internal void Free(CompressionStream z)
		{
			//  ZFREE(z, c);
		}
		/// <summary>
		/// Called with number of bytes left to write in window at least 258
		/// (the maximum string length) and number of input bytes available
		/// at least ten.  The ten bytes are six bytes for the longest length/
		/// distance pair plus four bytes for overloading the bit buffer.
		/// </summary>
		/// <param name="bl"></param>
		/// <param name="bd"></param>
		/// <param name="tl"></param>
		/// <param name="tlIndex"></param>
		/// <param name="td"></param>
		/// <param name="tdIndex"></param>
		/// <param name="s"></param>
		/// <param name="z"></param>
		/// <returns></returns>
		internal int InflateFast(int bl, int bd, int[] tl, int tlIndex, int[] td, int tdIndex, InflaterBlocks s, CompressionStream z)
		{
			int t; // temporary pointer
			int[] tp; // temporary pointer
			int tpIndex; // temporary pointer
			int e; // extra bits or operation
			int b; // bit buffer
			int k; // bits in bit buffer
			int p; // input data pointer
			int n; // bytes available there
			int q; // output window write pointer
			int m; // bytes to end of window or read pointer
			int ml; // mask for literal/length tree
			int md; // mask for distance tree
			int c; // bytes to copy
			int d; // distance back to copy from
			int r; // copy source pointer
			
			// load input, output, bit values
			p = z.InputIndex;
			n = z.InputCount;
			b = s.BitBuffer;
			k = s.BitBufferLength;
			q = s.WindowWrite;
			m = q < s.WindowRead ? s.WindowRead - q - 1 : s.WindowEnd - q;
			
			// initialize masks
			ml = inflateMask[bl];
			md = inflateMask[bd];
			
			// do until not enough input or output space for fast loop
			do 
			{
				// assume called with m >= 258 && n >= 10
				// get literal/length code
				while (k < (20))
				{
					// max bits for literal/length code
					n--;
					b |= (z.Input[p++] & 0xff) << k; k += 8;
				}
				
				t = b & ml;
				tp = tl;
				tpIndex = tlIndex;
				int tpIndext3 = (tpIndex + t) * 3;
				if ((e = tp[tpIndext3]) == 0)
				{
					b >>= (tp[tpIndext3 + 1]); k -= (tp[tpIndext3 + 1]);
					
					s.Window[q++] = (byte) tp[tpIndext3 + 2];
					m--;
					continue;
				}
				do 
				{
					
					b >>= (tp[tpIndext3 + 1]); k -= (tp[tpIndext3 + 1]);
					
					if ((e & 16) != 0)
					{
						e &= 15;
						c = tp[tpIndext3 + 2] + ((int) b & inflateMask[e]);
						
						b >>= e; k -= e;
						
						// decode distance base of block to copy
						while (k < (15))
						{
							// max bits for distance code
							n--;
							b |= (z.Input[p++] & 0xff) << k; k += 8;
						}
						
						t = b & md;
						tp = td;
						tpIndex = tdIndex;
						tpIndext3 = (tpIndex + t) * 3;
						e = tp[tpIndext3];						
						do 
						{
							
							b >>= (tp[tpIndext3 + 1]); k -= (tp[tpIndext3 + 1]);
							
							if ((e & 16) != 0)
							{
								// get extra bits to add to distance base
								e &= 15;
								while (k < (e))
								{
									// get extra bits (up to 13)
									n--;
									b |= (z.Input[p++] & 0xff) << k; k += 8;
								}
								
								d = tp[tpIndext3 + 2] + (b & inflateMask[e]);
								
								b >>= (e); k -= (e);
								
								// do the copy
								m -= c;
								if (q >= d)
								{
									// offset before dest
									//  just copy
									r = q - d;
									if (q - r > 0 && 2 > (q - r))
									{
										s.Window[q++] = s.Window[r++]; // minimum count is three,
										s.Window[q++] = s.Window[r++]; // so unroll loop a little
										c -= 2;
									}
									else
									{
										Array.Copy(s.Window, r, s.Window, q, 2);
										q += 2; r += 2; c -= 2;
									}
								}
								else
								{
									// else offset after destination
									r = q - d;
									do 
									{
										r += s.WindowEnd; // force pointer in window
									}
									while (r < 0); // covers invalid distances
									e = s.WindowEnd - r;
									if (c > e)
									{
										// if source crosses,
										c -= e; // wrapped copy
										if (q - r > 0 && e > (q - r))
										{
											do 
											{
												s.Window[q++] = s.Window[r++];
											}
											while (--e != 0);
										}
										else
										{
											Array.Copy(s.Window, r, s.Window, q, e);
											q += e; r += e; e = 0;
										}
										r = 0; // copy rest from start of window
									}
								}
								
								// copy all or what's left
								if (q - r > 0 && c > (q - r))
								{
									do 
									{
										s.Window[q++] = s.Window[r++];
									}
									while (--c != 0);
								}
								else
								{
									Array.Copy(s.Window, r, s.Window, q, c);
									q += c; r += c; c = 0;
								}
								break;
							}
							else if ((e & 64) == 0)
							{
								t += tp[tpIndext3 + 2];
								t += (b & inflateMask[e]);
								tpIndext3 = (tpIndex + t) * 3;
								e = tp[tpIndext3];
							}
							else
							{
								z.ErrorMessage = "invalid distance code";
								
								c = z.InputCount - n;
								c = (k >> 3) < c ? k >> 3 : c;
								n += c;
								p -= c;
								k -= (c << 3);
								
								s.BitBuffer = b;
								s.BitBufferLength = k;
								z.InputCount = n;
								z.InputTotal += p - z.InputIndex;
								z.InputIndex = p;
								s.WindowWrite = q;
								
								return (int)ErrorCode.DataError;
							}
						}
						while (true);
						break;
					}
					
					if ((e & 64) == 0)
					{
						t += tp[tpIndext3 + 2];
						t += (b & inflateMask[e]);
						tpIndext3 = (tpIndex + t) * 3;
						if ((e = tp[tpIndext3]) == 0)
						{
							
							b >>= (tp[tpIndext3 + 1]);
							k -= (tp[tpIndext3 + 1]);
							
							s.Window[q++] = (byte) tp[tpIndext3 + 2];
							m--;
							break;
						}
					}
					else if ((e & 32) != 0)
					{
						
						c = z.InputCount - n;
						c = (k >> 3) < c ? k >> 3 : c;
						n += c;
						p -= c;
						k -= (c << 3);
						
						s.BitBuffer = b;
						s.BitBufferLength = k;
						z.InputCount = n;
						z.InputTotal += p - z.InputIndex;
						z.InputIndex = p;
						s.WindowWrite = q;
						
						return (int)ErrorCode.StreamEnd;
					}
					else
					{
						z.ErrorMessage = "invalid literal/length code";
						
						c = z.InputCount - n;
						c = (k >> 3) < c ? k >> 3 : c;
						n += c;
						p -= c;
						k -= (c << 3);
						
						s.BitBuffer = b;
						s.BitBufferLength = k;
						z.InputCount = n;
						z.InputTotal += p - z.InputIndex;
						z.InputIndex = p;
						s.WindowWrite = q;
						
						return (int)ErrorCode.DataError;
					}
				}
				while (true);
			}
			while (m >= 258 && n >= 10);
			
			// not enough input or output--restore pointers and return
			c = z.InputCount - n;
			c = (k >> 3) < c ? k >> 3 : c;
			n += c;
			p -= c;
			k -= (c << 3);
			
			s.BitBuffer = b;
			s.BitBufferLength = k;
			z.InputCount = n;
			z.InputTotal += p - z.InputIndex;
			z.InputIndex = p;
			s.WindowWrite = q;
			
			return (int)ErrorCode.Ok;
		}
	}
}