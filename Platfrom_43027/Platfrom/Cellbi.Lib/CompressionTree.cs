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
	/// Compression tree.
	/// </summary>
	sealed class CompressionTree
	{
    int maxCode;
    CompressionStaticTree staticTree;
    short[] dynamicTree;
    
    // private methods...
    /// <summary>
    /// Compute the optimal bit lengths for a tree and update the total bit length
    /// for the current block.
    /// IN assertion: the fields freq and dad are set, heap[heap_max] and
    ///    above are the tree nodes sorted by increasing frequency.
    /// OUT assertions: the field len is set to the optimal bit length, the
    ///    array bl_count contains the frequencies for each bit length.
    ///    The length opt_len is updated; static_len is also updated if stree is
    ///    not null.
    /// </summary>
    /// <param name="deflater">The deflater to use.</param>
    void GenerateBitLengths(Deflater deflater)
    {
      short[] tree = dynamicTree;
      short[] stree = staticTree.StaticTreeData;
      int[] extra = staticTree.ExtraBits;
      int extraBase = staticTree.ExtraBase;
      int maxLength = staticTree.MaxLength;
      int heapIdx; // heap index
      int n, m; // iterate over the tree elements
      int bits; // bit length
      int xbits; // extra bits
      short f; // frequency
      int overflow = 0; // number of elements with bit length too large

      for (bits = 0; bits <= TreeConstants.MaxBits; bits++)
        deflater.bl_count[bits] = 0;

      // In a first pass, compute the optimal bit lengths (which may
      // overflow in the case of the bit length tree).
      tree[deflater.heap[deflater.heapMax] * 2 + 1] = 0; // root of the heap

      for (heapIdx = deflater.heapMax + 1; heapIdx < TreeConstants.HeapSize; heapIdx++)
      {
        n = deflater.heap[heapIdx];
        bits = tree[tree[n * 2 + 1] * 2 + 1] + 1;
        if (bits > maxLength)
        {
          bits = maxLength; overflow++;
        }
        tree[n * 2 + 1] = (short)bits;
        // We overwrite tree[n*2+1] which is no longer needed

        if (n > maxCode)
          continue; // not a leaf node

        deflater.bl_count[bits]++;
        xbits = 0;
        if (n >= extraBase)
          xbits = extra[n - extraBase];
        f = tree[n * 2];
        deflater.opt_len += f * (bits + xbits);
        if (stree != null)
          deflater.static_len += f * (stree[n * 2 + 1] + xbits);
      }
      if (overflow == 0)
        return;

      // This happens for example on obj2 and pic of the Calgary corpus
      // Find the first bit length which could increase:
      do
      {
        bits = maxLength - 1;
        while (deflater.bl_count[bits] == 0)
          bits--;
        deflater.bl_count[bits]--; // move one leaf down the tree
        deflater.bl_count[bits + 1] = (short)(deflater.bl_count[bits + 1] + 2); // move one overflow item as its brother
        deflater.bl_count[maxLength]--;
        // The brother of the overflow item also moves one step up,
        // but this does not affect bl_count[max_length]
        overflow -= 2;
      }
      while (overflow > 0);

      for (bits = maxLength; bits != 0; bits--)
      {
        n = deflater.bl_count[bits];
        while (n != 0)
        {
          m = deflater.heap[--heapIdx];
          if (m > maxCode)
            continue;
          if (tree[m * 2 + 1] != bits)
          {
            deflater.opt_len = (int)(deflater.opt_len + ((long)bits - (long)tree[m * 2 + 1]) * (long)tree[m * 2]);
            tree[m * 2 + 1] = (short)bits;
          }
          n--;
        }
      }
    }
    /// <summary>
    /// Generate the codes for a given tree and bit counts (which need not be
    /// optimal).
    /// IN assertion: the array bl_count contains the bit length statistics for
    /// the given tree and the field len is set for all tree elements.
    /// OUT assertion: the field code is set for all tree elements of non zero code length.
    /// </summary>
    /// <param name="tree"></param>
    /// <param name="maxCode"></param>
    /// <param name="blCount"></param>
    static void GenerateCodes(short[] tree, int maxCode, short[] blCount)
    {
      short[] nextCode = new short[TreeConstants.MaxBits + 1]; // next code value for each bit length
      short code = 0; // running code value
      int bits; // bit index
      int n; // code index

      // The distribution counts are first used to generate the code values
      // without bit reversal.
      for (bits = 1; bits <= TreeConstants.MaxBits; bits++)
      {
        nextCode[bits] = code = (short)((code + blCount[bits - 1]) << 1);
      }

      // Check that the bit counts in blCount are consistent.
			// The last code must be all ones.
      for (n = 0; n <= maxCode; n++)
      {
        int len = tree[n * 2 + 1];
        if (len == 0)
          continue;
        // Now reverse the bits
        tree[n * 2] = (short)(ReverseBits(nextCode[len]++, len));
      }
    }
    /// <summary>
    /// Reverse the first len bits of a code, using straightforward code (a faster
    /// method would use a table)
    /// IN assertion: 1 &lt;= len &lt;= 15
    /// </summary>
    /// <param name="code"></param>
    /// <param name="len"></param>
    static int ReverseBits(int code, int len)
    {
      int res = 0;
      do
      {
        res |= code & 1;
        code = Utils.ShiftRight(code, 1);
        res <<= 1;
      }
      while (--len > 0);
      return Utils.ShiftRight(res, 1);
    }
		
    // public methods...
		/// <summary>
		/// Gets mapping from a distance to a distance code.
		/// </summary>
    /// <param name="distance">The distance parameter is the distance - 1 and must not have side effects.</param>
		public static int GetDistanceCode(int distance)
		{
      // DistanceCode[256] and DistanceCode[257] are never used.
      if (distance < 256)
        return TreeConstants.DistanceCode[distance];
      return TreeConstants.DistanceCode[256 + Utils.ShiftRight(distance, 7)];
		}
		
		/// <summary>
    /// Construct one Huffman tree and assigns the code bit strings and lengths.
    /// Update the total bit length for the current block.
    /// IN assertion: the field freq is set for all tree elements.
    /// OUT assertions: the fields len and code are set to the optimal bit length
    ///     and corresponding code. The length opt_len is updated; static_len is
    ///     also updated if stree is not null. The field max_code is set.
		/// </summary>
		/// <param name="deflater"></param>
		public void BuildTree(Deflater deflater)
		{
			short[] tree = dynamicTree;
			short[] stree = staticTree.StaticTreeData;
			int elems = staticTree.MaxElements;
			int n, m; // iterate over heap elements
			int maxCode = - 1; // largest code with non zero frequency
			int node; // new node being created
			
			// Construct the initial heap, with least frequent element in
			// heap[1]. The sons of heap[n] are heap[2*n] and heap[2*n+1].
			// heap[0] is not used.
			deflater.heapLen = 0;
      deflater.heapMax = TreeConstants.HeapSize;
			
			for (n = 0; n < elems; n++)
			{
				if (tree[n * 2] != 0)
				{
					deflater.heap[++deflater.heapLen] = maxCode = n;
					deflater.depth[n] = 0;
				}
				else
				{
					tree[n * 2 + 1] = 0;
				}
			}
			
			// The pkzip format requires that at least one distance code exists,
			// and that at least one bit should be sent even if there is only one
			// possible code. So to avoid special checks later on we force at least
			// two codes of non zero frequency.
			while (deflater.heapLen < 2)
			{
				node = deflater.heap[++deflater.heapLen] = (maxCode < 2?++maxCode:0);
				tree[node * 2] = 1;
				deflater.depth[node] = 0;
				deflater.opt_len--;
				if (stree != null)
					deflater.static_len -= stree[node * 2 + 1];
				// node is 0 or 1 so it does not have extra bits
			}
			this.maxCode = maxCode;
			
			// The elements heap[heap_len/2+1 .. heap_len] are leaves of the tree,
			// establish sub-heaps of increasing lengths:
			
			for (n = deflater.heapLen / 2; n >= 1; n--)
				deflater.RestoreHeapDown(tree, n);
			
			// Construct the Huffman tree by repeatedly combining the least two
			// frequent nodes.
			
			node = elems; // next internal node of the tree
			do 
			{
				// n = node of least frequency
				n = deflater.heap[1];
				deflater.heap[1] = deflater.heap[deflater.heapLen--];
				deflater.RestoreHeapDown(tree, 1);
				m = deflater.heap[1]; // m = node of next least frequency
				
				deflater.heap[--deflater.heapMax] = n; // keep the nodes sorted by frequency
				deflater.heap[--deflater.heapMax] = m;
				
				// Create a new node father of n and m
				tree[node * 2] = (short) (tree[n * 2] + tree[m * 2]);
				deflater.depth[node] = (byte) (System.Math.Max((byte) deflater.depth[n], (byte) deflater.depth[m]) + 1);
				tree[n * 2 + 1] = tree[m * 2 + 1] = (short) node;
				
				// and insert the new node in the heap
				deflater.heap[1] = node++;
				deflater.RestoreHeapDown(tree, 1);
			}
			while (deflater.heapLen >= 2);
			
			deflater.heap[--deflater.heapMax] = deflater.heap[1];
			
			// At this point, the fields freq and dad are set. We can now
			// generate the bit lengths.
			
			GenerateBitLengths(deflater);
			
			// The field len is now set, we can generate the bit codes
			GenerateCodes(tree, maxCode, deflater.bl_count);
		}

    // public properties...
    /// <summary>
    /// Gets or sets largest code with non zero frequency.
    /// </summary>
    public int MaxCode
    {
      get { return maxCode; }
      set { maxCode = value; }
    }
    /// <summary>
    /// Gets or sets corresponding static tree.
    /// </summary>
    public CompressionStaticTree StaticTree
    {
      get { return staticTree; }
      set { staticTree = value; }
    }
    /// <summary>
    /// Gets or sets dynamic tree.
    /// </summary>
    public short[] DynamicTree
    {
      get { return dynamicTree; }
      set { dynamicTree = value; }
    }
	}
}