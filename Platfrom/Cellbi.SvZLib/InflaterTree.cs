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
	sealed class InflaterTree
	{
		// private methods...
		static int BuildHuffmanTree(int[] b, int bindex, int n, int s, int[] d, int[] e, int[] t, int[] m, int[] hp, int[] hn, int[] v)
		{
			// Given a list of code lengths and a maximum table size, make a set of
			// tables to decode that set of codes.  Return Z_OK on success, Z_BUF_ERROR
			// if the given code set is incomplete (the tables are still built in this
			// case), Z_DATA_ERROR if the input is invalid (an over-subscribed set of
			// lengths), or Z_MEM_ERROR if not enough memory.
			
			int[] c = new int[Constants.MaxWindowBits + 1]; // bit length count table
			int[] r = new int[3]; // table entry for structure assignment
			int[] u = new int[Constants.MaxWindowBits]; // table stack
			int[] x = new int[Constants.MaxWindowBits + 1]; // bit offsets, then code stack

			int a; // counter for codes of length k			
			int f; // i repeats in table every f entries
			int g; // maximum code length
			int h; // table level
			int i; // counter, current code
			int j; // counter
			int k; // number of bits in current code
			int l; // bits per table (returned in m)
			int mask; // (1 << w) - 1, to avoid cc -O bug on HP
			int p; // pointer into c[], b[], or v[]
			int q; // points to current table			

			int w; // bits before this table == (l * h)			
			int xp; // pointer into x
			int y; // number of dummy codes added
			int z; // number of entries in current table
			
			// Generate counts for each bit length
			
			p = 0; i = n;
			do 
			{
				c[b[bindex + p]]++; p++; i--; // assume all entries <= BMAX
			}
			while (i != 0);
			
			if (c[0] == n)
			{
				// null input--all zero length codes
				t[0] = - 1;
				m[0] = 0;
				return (int)ErrorCode.Ok;
			}
			
			// Find minimum and maximum length, bound *m by those
			l = m[0];
			for (j = 1; j <= Constants.MaxWindowBits; j++)
				if (c[j] != 0)
					break;
			k = j; // minimum code length
			if (l < j)
			{
				l = j;
			}
			for (i = Constants.MaxWindowBits; i != 0; i--)
			{
				if (c[i] != 0)
					break;
			}
			g = i; // maximum code length
			if (l > i)
			{
				l = i;
			}
			m[0] = l;
			
			// Adjust last length count to fill out codes, if needed
			for (y = 1 << j; j < i; j++, y <<= 1)
			{
				if ((y -= c[j]) < 0)
				{
					return (int)ErrorCode.DataError;
				}
			}
			if ((y -= c[i]) < 0)
			{
				return (int)ErrorCode.DataError;
			}
			c[i] += y;
			
			// Generate starting offsets into the value table for each length
			x[1] = j = 0;
			p = 1; xp = 2;
			while (--i != 0)
			{
				// note that i == g from above
				x[xp] = (j += c[p]);
				xp++;
				p++;
			}
			
			// Make a table of values in order of bit lengths
			i = 0; p = 0;
			do 
			{
				if ((j = b[bindex + p]) != 0)
				{
					v[x[j]++] = i;
				}
				p++;
			}
			while (++i < n);
			n = x[g]; // set n to length of v
			
			// Generate the Huffman codes and for each, make the table entries
			x[0] = i = 0; // first Huffman code is zero
			p = 0; // grab values in bit order
			h = - 1; // no tables yet--level -1
			w = - l; // bits decoded == (l * h)
			u[0] = 0; // just to keep compilers happy
			q = 0; // ditto
			z = 0; // ditto
			
			// go through the bit lengths (k already is bits in shortest code)
			for (; k <= g; k++)
			{
				a = c[k];
				while (a-- != 0)
				{
					// here i is the Huffman code of length k bits for value *p
					// make tables up to required level
					while (k > w + l)
					{
						h++;
						w += l; // previous table always l bits
						// compute minimum size table less than or equal to l bits
						z = g - w;
						z = (z > l)?l:z; // table size upper limit
						if ((f = 1 << (j = k - w)) > a + 1)
						{
							// try a k-w bit table
							// too few codes for k-w bit table
							f -= (a + 1); // deduct codes from patterns left
							xp = k;
							if (j < z)
							{
								while (++j < z)
								{
									// try smaller tables up to z bits
									if ((f <<= 1) <= c[++xp])
										break; // enough codes to use up j bits
									f -= c[xp]; // else deduct codes from patterns
								}
							}
						}
						z = 1 << j; // table entries for j-bit table
						
						// allocate new table
						if (hn[0] + z > Constants.Many)
						// (note: doesn't matter for fixed)
							return (int)ErrorCode.DataError; // overflow of MANY
						u[h] = q = hn[0]; // DEBUG
						hn[0] += z;
						
						// connect to last table, if there is one
						if (h != 0)
						{
							x[h] = i; // save pattern for backing up
							r[0] = (byte) j; // bits in this table
							r[1] = (byte) l; // bits to dump before this table
							j = Utils.ShiftRight(i, (w - l));
							r[2] = (int) (q - u[h - 1] - j); // offset to this table
							Array.Copy(r, 0, hp, (u[h - 1] + j) * 3, 3); // connect to last table
						}
						else
						{
							t[0] = q; // first table is returned result
						}
					}
					
					// set up table entry in r
					r[1] = (byte) (k - w);
					if (p >= n)
					{
						r[0] = 128 + 64; // out of values--invalid code
					}
					else if (v[p] < s)
					{
						r[0] = (byte) (v[p] < 256?0:32 + 64); // 256 is end-of-block
						r[2] = v[p++]; // simple code is just the value
					}
					else
					{
						r[0] = (byte) (e[v[p] - s] + 16 + 64); // non-simple--look up in lists
						r[2] = d[v[p++] - s];
					}
					
					// fill code-like entries with r
					f = 1 << (k - w);
					for (j = Utils.ShiftRight(i, w); j < z; j += f)
					{
						Array.Copy(r, 0, hp, (q + j) * 3, 3);
					}
					
					// backwards increment the k-bit code i
					for (j = 1 << (k - 1); (i & j) != 0; j = Utils.ShiftRight(j, 1))
					{
						i ^= j;
					}
					i ^= j;
					
					// backup over finished tables
					mask = (1 << w) - 1; // needed on HP, cc -O bug
					while ((i & mask) != x[h])
					{
						h--; // don't need to update q
						w -= l;
						mask = (1 << w) - 1;
					}
				}
			}
			// Return Z_BUF_ERROR if we were given an incomplete table
			return y != 0 && g != 1?(int)ErrorCode.BufferError : (int)ErrorCode.Ok;
		}
		
		// internal methods...
		internal static int InflateTreeBits(int[] c, int[] bb, int[] tb, int[] hp, CompressionStream z)
		{
			int r;
			int[] hn = new int[1]; // hufts used in space
			int[] v = new int[19]; // work area for huft_build 
			
			r = BuildHuffmanTree(c, 0, 19, 19, null, null, tb, bb, hp, hn, v);
			
			if (r == (int)ErrorCode.DataError)
			{
				z.ErrorMessage = "oversubscribed dynamic bit lengths tree";
			}
			else if (r == (int)ErrorCode.BufferError || bb[0] == 0)
			{
				z.ErrorMessage = "incomplete dynamic bit lengths tree";
				r = (int)ErrorCode.DataError;
			}
			return r;
		}
		internal static int InflateTreeDynamic(int nl, int nd, int[] c, int[] bl, int[] bd, int[] tl, int[] td, int[] hp, CompressionStream z)
		{
			int r;
			int[] v = new int[288]; // work area for huft_build
			int[] hn = new int[1]; // hufts used in space
			
			// build literal/length tree
			r = BuildHuffmanTree(c, 0, nl, 257, InflaterTreeConstants.CpLens, InflaterTreeConstants.CpLext, tl, bl, hp, hn, v);
			if (r != (int)ErrorCode.Ok || bl[0] == 0)
			{
				if (r == (int)ErrorCode.DataError)
				{
					z.ErrorMessage = "oversubscribed literal/length tree";
				}
				else if (r != (int)ErrorCode.MemoryError)
				{
					z.ErrorMessage = "incomplete literal/length tree";
					r = (int)ErrorCode.DataError;
				}
				return r;
			}
			
			// build distance tree
			r = BuildHuffmanTree(c, nl, nd, 0, InflaterTreeConstants.CpDist, InflaterTreeConstants.CpDext, td, bd, hp, hn, v);
			
			if (r != (int)ErrorCode.Ok || (bd[0] == 0 && nl > 257))
			{
				if (r == (int)ErrorCode.DataError)
				{
					z.ErrorMessage = "oversubscribed distance tree";
				}
				else if (r == (int)ErrorCode.BufferError)
				{
					z.ErrorMessage = "incomplete distance tree";
					r = (int)ErrorCode.DataError;
				}
				else if (r != (int)ErrorCode.MemoryError)
				{
					z.ErrorMessage = "empty distance tree with lengths";
					r = (int)ErrorCode.DataError;
				}
				return r;
			}
			
			return (int)ErrorCode.Ok;
		}
		internal static int InflateTreeFixed(int[] bl, int[] bd, int[][] tl, int[][] td, CompressionStream z)
		{
			bl[0] = InflaterTreeConstants.FixedBl;
			bd[0] = InflaterTreeConstants.FixedBd;
			tl[0] = InflaterTreeConstants.FixedTl;
			td[0] = InflaterTreeConstants.FixedTd;
			return (int)ErrorCode.Ok;
		}
	}
}