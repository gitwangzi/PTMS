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
	/// SvZLib deflater class.
	/// </summary>
	public sealed class Deflater
	{
		static DeflaterOptions[] Options;
				
		static readonly string[] errmsg = new string[]
						{
							"need dictionary",
							"stream end",
							String.Empty,
							"file error",
							"stream error",
							"data error",
							"insufficient memory",
							"buffer error",
							"incompatible version",
							String.Empty
						};
		
		// block not completed, need more input or more output
		const int NeedMore = 0;
		
		// block flush performed
		const int BlockDone = 1;
		
		// finish started, need only more output at next deflate
		const int FinishStarted = 2;
		
		// finish done, accept no more input or output
		const int FinishDone = 3;		
		
		const int BufferSize = 8 * 2;
		
		// repeat previous bit length 3-6 times (2 bits of repeat count)
		const int Repeat3_6 = 16;
		
		// repeat a zero length 3-10 times  (3 bits of repeat count)
		const int RepeatZero3_10 = 17;
		
		// repeat a zero length 11-138 times  (7 bits of repeat count)
		const int RepeatZerp11_138 = 18;
		
		const int MinMatch = 3;
		const int MaxMatch = 258;		

		static readonly int MinLookahead = (MaxMatch + MinMatch + 1);
		
		const int MaxBits = 15;
		const int DCodes = 30;
		const int BlCodes = 19;
		const int LengthCodes = 29;
		const int Literals = 256;

		static readonly int LCodes = (Literals + 1 + LengthCodes);		
		static readonly int HeapSize = (2 * LCodes + 1);
		
		const int EndBlock = 256;
		
		CompressionStream stream; // pointer back to this zlib stream
		int status; // as the name implies
    
		int pendingBufferSize; // size of pending_buf
    
    internal byte[] pendingBuffer; // output still pending
    internal int pendingOutput; // next pending byte to output to the stream
    internal int pending; // nb of bytes in the pending buffer
		internal int noheader; // suppress zlib header and adler32

		byte dataType; // UNKNOWN, BINARY or ASCII
		byte method; // STORED (for zip only) or DEFLATED
		int lastFlush; // value of flush param for previous deflate call
		
		int wSize; // LZ77 window size (32K by default)
		int wBits; // log2(w_size)  (8..16)
		int wMask; // w_size - 1
		
		byte[] window;
		// Sliding window. Input bytes are read into the second half of the window,
		// and move to the first half later to keep a dictionary of at least wSize
		// bytes. With this organization, matches are limited to a distance of
		// wSize-MAX_MATCH bytes, but this ensures that IO is always
		// performed with a length multiple of the block size. Also, it limits
		// the window size to 64K, which is quite useful on MSDOS.
		// To do: use the user input buffer as sliding window.
		
		int windowSize;
		// Actual size of window: 2*wSize, except when the user input buffer
		// is directly used as sliding window.
		
		short[] prev;
		// Link to older string with same hash index. To limit the size of this
		// array to 64K, this link is maintained only for the last 32K strings.
		// An index in this array is thus a window index modulo 32K.
		
		short[] head; // Heads of the hash chains or NIL.
		
		int ins_h; // hash index of string to be inserted
		int hashSize; // number of elements in hash table
		int hashBits; // log2(hash_size)
		int hashMask; // hash_size-1
		
		// Number of bits by which ins_h must be shifted at each input
		// step. It must be such that after MIN_MATCH steps, the oldest
		// byte no longer takes part in the hash key, that is:
		// hash_shift * MIN_MATCH >= hash_bits
		int hashShift;
		
		// Window position at the beginning of the current output block. Gets
		// negative when the window is moved backwards.
		
		int blockStart;
		
		int matchLength; // length of best match
		int prevMatch; // previous match
		int matchAvailable; // set if previous match exists
		int strstart; // start of string to insert
		int matchStart; // start of matching string
		int lookahead; // number of valid bytes ahead in window
		
		// Length of the best match at previous step. Matches not greater than this
		// are discarded. This is used in the lazy match evaluation.
		int prevLength;
		
		// To speed up deflation, hash chains are never searched beyond this
		// length.  A higher limit improves compression ratio but degrades the speed.
		int maxChainLength;
		
		// Attempt to find a better match only when the current match is strictly
		// smaller than this value. This mechanism is used only for compression
		// levels >= 4.
		int maxLazyMatch;
		
		// Insert new strings in the hash table only if the match length is not
		// greater than this length. This saves time but degrades compression.
		// max_insert_length is used only for compression levels <= 3.
		
		int level; // compression level (1..9)
		int strategy; // favor or force Huffman coding
		
		// Use a faster search when the previous match is longer than this
		int goodMatch;
		
		// Stop searching when current match exceeds this
		int niceMatch;
		
		short[] dyn_ltree; // literal and length tree
		short[] dyn_dtree; // distance tree
		short[] bl_tree; // Huffman tree for bit lengths
		
		CompressionTree l_desc = new CompressionTree(); // desc for literal tree
		CompressionTree d_desc = new CompressionTree(); // desc for distance tree
		CompressionTree bl_desc = new CompressionTree(); // desc for bit length tree
		
		// number of codes at each bit length for an optimal tree
		internal short[] bl_count = new short[MaxBits + 1];
		
		// heap used to build the Huffman trees
		internal int[] heap = new int[2 * LCodes + 1];
		
		internal int heapLen; // number of elements in the heap
		internal int heapMax; // element of largest frequency
		// The sons of heap[n] are heap[2*n] and heap[2*n+1]. heap[0] is not used.
		// The same heap array is used to build all trees.
		
		// Depth of each subtree used as tie breaker for trees of equal frequency
		internal byte[] depth = new byte[2 * LCodes + 1];
		
		int litBuf; // index for literals or lengths */
		
		// Size of match buffer for literals/lengths.  There are 4 reasons for
		// limiting lit_bufsize to 64K:
		//   - frequencies can be kept in 16 bit counters
		//   - if compression is not successful for the first block, all input
		//     data is still in the window so we can still emit a stored block even
		//     when input comes from standard input.  (This can also be done for
		//     all blocks if lit_bufsize is not greater than 32K.)
		//   - if compression is not successful for a file smaller than 64K, we can
		//     even emit a stored file instead of a stored block (saving 5 bytes).
		//     This is applicable only for zip (not gzip or zlib).
		//   - creating new Huffman trees less frequently may not provide fast
		//     adaptation to changes in the input data statistics. (Take for
		//     example a binary file with poorly compressible code followed by
		//     a highly compressible string table.) Smaller buffer sizes give
		//     fast adaptation but have of course the overhead of transmitting
		//     trees more frequently.
		//   - I can't count above 4
		int litBufsize;
		
		int lastLit; // running index in l_buf
		
		// Buffer for distances. To simplify the code, d_buf and l_buf have
		// the same number of elements. To use different lengths, an extra flag
		// array would be necessary.
		
		int d_buf; // index of pendig_buf
		
		internal int opt_len; // bit length of current block with optimal trees
		internal int static_len; // bit length of current block with static trees
		internal int matches; // number of string matches in current block
		internal int last_eob_len; // bit length of EOB code for last block
		
		// Output buffer. bits are inserted starting at the bottom (least
		// significant bits).
		short biBuf;
		
		// Number of valid bits in bi_buf.  All bits above the last valid bit
		// are always zero.
		int biValid;

    // constructors...
    static Deflater()
    {
      Options = new DeflaterOptions[10];
      //                         good  lazy  nice  chain
      Options[0] = new DeflaterOptions(0, 0, 0, 0, DeflaterFunction.Stored);
      Options[1] = new DeflaterOptions(4, 4, 8, 4, DeflaterFunction.Fast);
      Options[2] = new DeflaterOptions(4, 5, 16, 8, DeflaterFunction.Fast);
      Options[3] = new DeflaterOptions(4, 6, 32, 32, DeflaterFunction.Fast);

      Options[4] = new DeflaterOptions(4, 4, 16, 16, DeflaterFunction.Slow);
      Options[5] = new DeflaterOptions(8, 16, 32, 32, DeflaterFunction.Slow);
      Options[6] = new DeflaterOptions(8, 16, 128, 128, DeflaterFunction.Slow);
      Options[7] = new DeflaterOptions(8, 32, 128, 256, DeflaterFunction.Slow);
      Options[8] = new DeflaterOptions(32, 128, 258, 1024, DeflaterFunction.Slow);
      Options[9] = new DeflaterOptions(32, 258, 258, 4096, DeflaterFunction.Slow);
    }
		/// <summary>
		/// Creates new instance of the Deflater class.
		/// </summary>
		public Deflater()
		{
			dyn_ltree = new short[HeapSize * 2];
			dyn_dtree = new short[(2 * DCodes + 1) * 2]; // distance tree
			bl_tree = new short[(2 * BlCodes + 1) * 2]; // Huffman tree for bit lengths
		}

    // private methods...
		void InitializeMatch()
		{
			windowSize = 2 * wSize;
			
			head[hashSize - 1] = 0;
			for (int i = 0; i < hashSize - 1; i++)
				head[i] = 0;
			
			// Set the default configuration parameters:
			maxLazyMatch = Deflater.Options[level].MaxLazy;
			goodMatch = Deflater.Options[level].GoodLength;
			niceMatch = Deflater.Options[level].NiceLength;
			maxChainLength = Deflater.Options[level].MaxChain;
			
			strstart = 0;
			blockStart = 0;
			lookahead = 0;
			matchLength = prevLength = MinMatch - 1;
			matchAvailable = 0;
			ins_h = 0;
		}
		// Initialize the tree data structures for a new zlib stream.
		void InitializeTree()
		{
			
			l_desc.DynamicTree = dyn_ltree;
			l_desc.StaticTree = CompressionStaticTree.StaticLTreeDescription;
			
			d_desc.DynamicTree = dyn_dtree;
			d_desc.StaticTree = CompressionStaticTree.StaticDTreeDescription;
			
			bl_desc.DynamicTree = bl_tree;
			bl_desc.StaticTree = CompressionStaticTree.StaticBLDescription;
			
			biBuf = 0;
			biValid = 0;
			last_eob_len = 8; // enough lookahead for inflate
			
			// Initialize the first block of the first file:
			InitializeBlock();
		}		
		void InitializeBlock()
		{
			// Initialize the trees.
			for (int i = 0; i < LCodes; i++)
				dyn_ltree[i * 2] = 0;
			for (int i = 0; i < DCodes; i++)
				dyn_dtree[i * 2] = 0;
			for (int i = 0; i < BlCodes; i++)
				bl_tree[i * 2] = 0;
			
			dyn_ltree[EndBlock * 2] = 1;
			opt_len = static_len = 0;
			lastLit = matches = 0;
		}
    // Scan a literal or distance tree to determine the frequencies of the codes
    // in the bit length tree.
    void ScanTree(short[] tree, int max_code)
    {
      int n; // iterates over all tree elements
      int prevlen = -1; // last emitted length
      int curlen; // length of current code
      int nextlen = tree[0 * 2 + 1]; // length of next code
      int count = 0; // repeat count of the current code
      int max_count = 7; // max repeat count
      int min_count = 4; // min repeat count

      if (nextlen == 0)
      {
        max_count = 138; min_count = 3;
      }
      tree[(max_code + 1) * 2 + 1] = (short)Utils.Identity(0xffff); // guard

      for (n = 0; n <= max_code; n++)
      {
        curlen = nextlen; nextlen = tree[(n + 1) * 2 + 1];
        if (++count < max_count && curlen == nextlen)
        {
          continue;
        }
        else if (count < min_count)
        {
          bl_tree[curlen * 2] = (short)(bl_tree[curlen * 2] + count);
        }
        else if (curlen != 0)
        {
          if (curlen != prevlen)
            bl_tree[curlen * 2]++;
          bl_tree[Repeat3_6 * 2]++;
        }
        else if (count <= 10)
        {
          bl_tree[RepeatZero3_10 * 2]++;
        }
        else
        {
          bl_tree[RepeatZerp11_138 * 2]++;
        }
        count = 0; prevlen = curlen;
        if (nextlen == 0)
        {
          max_count = 138; min_count = 3;
        }
        else if (curlen == nextlen)
        {
          max_count = 6; min_count = 3;
        }
        else
        {
          max_count = 7; min_count = 4;
        }
      }
    }
    // Construct the Huffman tree for the bit lengths and return the index in
    // bl_order of the last bit length code to send.
    int BuildBitLengthTree()
    {
      int max_blindex; // index of last bit length code of non zero freq

      // Determine the bit length frequencies for literal and distance trees
      ScanTree(dyn_ltree, l_desc.MaxCode);
      ScanTree(dyn_dtree, d_desc.MaxCode);

      // Build the bit length tree:
      bl_desc.BuildTree(this);
      // opt_len now includes the length of the tree representations, except
      // the lengths of the bit lengths codes and the 5+5+4 bits for the counts.

      // Determine the number of bit length codes to send. The pkzip format
      // requires that at least 4 bit length codes be sent. (appnote.txt says
      // 3 but the actual value used is 4.)
      for (max_blindex = BlCodes - 1; max_blindex >= 3; max_blindex--)
      {
        if (bl_tree[TreeConstants.BLOrder[max_blindex] * 2 + 1] != 0)
          break;
      }
      // Update opt_len to include the bit length tree and counts
      opt_len += 3 * (max_blindex + 1) + 5 + 5 + 4;

      return max_blindex;
    }
    // Send the header for a block using dynamic Huffman trees: the counts, the
    // lengths of the bit length codes, the literal tree and the distance tree.
    // IN assertion: lcodes >= 257, dcodes >= 1, blcodes >= 4.
    void SendAllTrees(int lcodes, int dcodes, int blcodes)
    {
      int rank; // index in bl_order

      SendBits(lcodes - 257, 5); // not +255 as stated in appnote.txt
      SendBits(dcodes - 1, 5);
      SendBits(blcodes - 4, 4); // not -3 as stated in appnote.txt
      for (rank = 0; rank < blcodes; rank++)
      {
        SendBits(bl_tree[TreeConstants.BLOrder[rank] * 2 + 1], 3);
      }
      SendTree(dyn_ltree, lcodes - 1); // literal tree
      SendTree(dyn_dtree, dcodes - 1); // distance tree
    }
    // Send a literal or distance tree in compressed form, using the codes in
    // bl_tree.
    void SendTree(short[] tree, int max_code)
    {
      int n; // iterates over all tree elements
      int prevlen = -1; // last emitted length
      int curlen; // length of current code
      int nextlen = tree[0 * 2 + 1]; // length of next code
      int count = 0; // repeat count of the current code
      int max_count = 7; // max repeat count
      int min_count = 4; // min repeat count

      if (nextlen == 0)
      {
        max_count = 138; min_count = 3;
      }

      for (n = 0; n <= max_code; n++)
      {
        curlen = nextlen; nextlen = tree[(n + 1) * 2 + 1];
        if (++count < max_count && curlen == nextlen)
        {
          continue;
        }
        else if (count < min_count)
        {
          do
          {
            SendCode(curlen, bl_tree);
          }
          while (--count != 0);
        }
        else if (curlen != 0)
        {
          if (curlen != prevlen)
          {
            SendCode(curlen, bl_tree); count--;
          }
          SendCode(Repeat3_6, bl_tree);
          SendBits(count - 3, 2);
        }
        else if (count <= 10)
        {
          SendCode(RepeatZero3_10, bl_tree);
          SendBits(count - 3, 3);
        }
        else
        {
          SendCode(RepeatZerp11_138, bl_tree);
          SendBits(count - 11, 7);
        }
        count = 0; prevlen = curlen;
        if (nextlen == 0)
        {
          max_count = 138; min_count = 3;
        }
        else if (curlen == nextlen)
        {
          max_count = 6; min_count = 3;
        }
        else
        {
          max_count = 7; min_count = 4;
        }
      }
    }

    // Output a byte on the stream.
    // IN assertion: there is enough room in pending_buf.
    void WriteByte(byte[] p, int start, int len)
    {
      Array.Copy(p, start, pendingBuffer, pending, len);
      pending += len;
    }
    void WriteByte(byte c)
    {
      pendingBuffer[pending++] = c;
    }
    void WriteShort(int w)
    {
      WriteByte((byte)(w));
      WriteByte((byte)(Utils.ShiftRight(w, 8)));
    }
    void WriteShortMSB(int b)
    {
      WriteByte((byte)(b >> 8));
      WriteByte((byte)(b));
    }
    void SendCode(int c, short[] tree)
    {
			int c2 = c * 2;
      SendBits((tree[c2] & 0xffff), (tree[c2 + 1] & 0xffff));
    }
		void AppendToBitBuffer(int v)
		{
			biBuf = (short)((ushort)biBuf | (ushort)((v << biValid) & 0xffff));
		}
    void SendBits(int v, int length)
    {
      int len = length;
      if (biValid > (int)BufferSize - len)
      {
				AppendToBitBuffer(v);
        WriteShort(biBuf);
        biBuf = (short)(Utils.ShiftRight(v, (BufferSize - biValid)));
        biValid += len - BufferSize;
      }
      else
      {
        AppendToBitBuffer(v);
        biValid += len;
      }
    }

    // Send one empty static block to give enough lookahead for inflate.
    // This takes 10 bits, of which 7 may remain in the bit buffer.
    // The current inflate code requires 9 bits of lookahead. If the
    // last two codes for the previous block (real code plus EOB) were coded
    // on 5 bits or less, inflate may have only 5+3 bits of lookahead to decode
    // the last real code. In this case we send two empty static blocks instead
    // of one. (There are no problems if the previous block is stored or fixed.)
    // To simplify the code, we assume the worst case of last real code encoded
    // on one bit only.
    void AlignTree()
    {
      SendBits((int)DeflaterTreeType.StaticTree << 1, 3);
      SendCode(EndBlock, TreeConstants.StaticLTree);

      FlushBitBuffer();

      // Of the 10 bits for the empty block, we have already sent
      // (10 - bi_valid) bits. The lookahead for the last real code (before
      // the EOB of the previous block) was thus at least one plus the length
      // of the EOB plus what we have just sent of the empty static block.
      if (1 + last_eob_len + 10 - biValid < 9)
      {
        SendBits((int)DeflaterTreeType.StaticTree << 1, 3);
        SendCode(EndBlock, TreeConstants.StaticLTree);
        FlushBitBuffer();
      }
      last_eob_len = 7;
    }


    // Save the match info and tally the frequency counts. Return true if
    // the current block must be flushed.
    bool TallyTree(int dist, int lc)
    {

      pendingBuffer[d_buf + lastLit * 2] = (byte)(Utils.ShiftRight(dist, 8));
      pendingBuffer[d_buf + lastLit * 2 + 1] = (byte)dist;

      pendingBuffer[litBuf + lastLit] = (byte)lc; lastLit++;

      if (dist == 0)
      {
        // lc is the unmatched char
        dyn_ltree[lc * 2]++;
      }
      else
      {
        matches++;
        // Here, lc is the match length - MIN_MATCH
        dist--; // dist = match distance - 1
        dyn_ltree[(TreeConstants.LengthCode[lc] + Literals + 1) * 2]++;
        dyn_dtree[CompressionTree.GetDistanceCode(dist) * 2]++;
      }

      if ((lastLit & 0x1fff) == 0 && level > 2)
      {
        // Compute an upper bound for the compressed length
        int out_length = lastLit * 8;
        int in_length = strstart - blockStart;
        int dcode;
        for (dcode = 0; dcode < DCodes; dcode++)
        {
          out_length = (int)(out_length + (int)dyn_dtree[dcode * 2] * (5L + TreeConstants.ExtraDBits[dcode]));
        }
        out_length = Utils.ShiftRight(out_length, 3);
        if ((matches < (lastLit / 2)) && out_length < in_length / 2)
          return true;
      }

      return (lastLit == litBufsize - 1);
      // We avoid equality with lit_bufsize because of wraparound at 64K
      // on 16 bit machines and because stored blocks are restricted to
      // 64K-1 bytes.
    }
    // Send the block data compressed using the given Huffman trees
    void CompressBlock(short[] ltree, short[] dtree)
    {
      int dist; // distance of matched string
      int lc; // match length or unmatched char (if dist == 0)
      int lx = 0; // running index in l_buf
      int code; // the code to send
      int extra; // number of extra bits to send

      if (lastLit != 0)
      {
        do
        {
          dist = ((pendingBuffer[d_buf + lx * 2] << 8) & 0xff00) | (pendingBuffer[d_buf + lx * 2 + 1] & 0xff);
          lc = (pendingBuffer[litBuf + lx]) & 0xff; lx++;

          if (dist == 0)
          {
            SendCode(lc, ltree); // send a literal byte
          }
          else
          {
            // Here, lc is the match length - MIN_MATCH
            code = TreeConstants.LengthCode[lc];

            SendCode(code + Literals + 1, ltree); // send the length code
            extra = TreeConstants.ExtraLBits[code];
            if (extra != 0)
            {
              lc -= TreeConstants.BaseLength[code];
              SendBits(lc, extra); // send the extra length bits
            }
            dist--; // dist is now the match distance - 1
            code = CompressionTree.GetDistanceCode(dist);

            SendCode(code, dtree); // send the distance code
            extra = TreeConstants.ExtraDBits[code];
            if (extra != 0)
            {
              dist -= TreeConstants.BaseDistance[code];
              SendBits(dist, extra); // send the extra distance bits
            }
          } // literal or match pair ?

          // Check that the overlay between pending_buf and d_buf+l_buf is ok:
        }
        while (lx < lastLit);
      }

      SendCode(EndBlock, ltree);
      last_eob_len = ltree[EndBlock * 2 + 1];
    }
    // Set the data type to ASCII or BINARY, using a crude approximation:
    // binary if more than 20% of the bytes are <= 6 or >= 128, ascii otherwise.
    // IN assertion: the fields freq of dyn_ltree are set and the total of all
    // frequencies does not exceed 64K (to fit in an int on 16 bit machines).
    internal void SetDataType()
    {
      int n = 0;
      int ascii_freq = 0;
      int bin_freq = 0;
      while (n < 7)
      {
        bin_freq += dyn_ltree[n * 2]; n++;
      }
      while (n < 128)
      {
        ascii_freq += dyn_ltree[n * 2]; n++;
      }
      while (n < Literals)
      {
        bin_freq += dyn_ltree[n * 2]; n++;
      }
      dataType = (byte)(bin_freq > (Utils.ShiftRight(ascii_freq, 2)) ? (int)DeflaterBlockType.Binary : (int)DeflaterBlockType.Ascii);
    }
    // Flush the bit buffer, keeping at most 7 bits in it.
    void FlushBitBuffer()
    {
      if (biValid == 16)
      {
        WriteShort(biBuf);
        biBuf = 0;
        biValid = 0;
      }
      else if (biValid >= 8)
      {
        WriteByte((byte)biBuf);
        biBuf = (short)(Utils.ShiftRight(biBuf, 8));
        biValid -= 8;
      }
    }
    // Flush the bit buffer and align the output on a byte boundary
    void WindupBitBuffer()
    {
      if (biValid > 8)
      {
        WriteShort(biBuf);
      }
      else if (biValid > 0)
      {
        WriteByte((byte)biBuf);
      }
      biBuf = 0;
      biValid = 0;
    }
    // Copy a stored block, storing first the length and its
    // one's complement if requested.
    void CopyBlock(int buf, int len, bool header)
    {
			//int index = 0;
      WindupBitBuffer(); // align on byte boundary
      last_eob_len = 8; // enough lookahead for inflate

      if (header)
      {
        WriteShort((short)len);
        WriteShort((short)~len);
      }

      //  while(len--!=0) {
      //    put_byte(window[buf+index]);
      //    index++;
      //  }
      WriteByte(window, buf, len);
    }
    void FlushBlockOnly(bool eof)
    {
      TreeFlushBlock(blockStart >= 0 ? blockStart : -1, strstart - blockStart, eof);
      blockStart = strstart;
      stream.FlushPending();
    }

    // Copy without compression as much as possible from the input stream, return
    // the current block state.
    // This function does not insert new strings in the dictionary since
    // uncompressible data is probably not useful. This function is used
    // only for the level=0 compression option.
    // NOTE: this function should be optimized to avoid extra copying from
    // window to pending_buf.
    int DeflateStored(int flush)
    {
      // Stored blocks are limited to 0xffff bytes, pending_buf is limited
      // to pending_buf_size, and each stored block has a 5 byte header:

      int max_block_size = 0xffff;
      int max_start;

      if (max_block_size > pendingBufferSize - 5)
      {
        max_block_size = pendingBufferSize - 5;
      }

      // Copy as much as possible from input to output:
      while (true)
      {
        // Fill the window as much as possible:
        if (lookahead <= 1)
        {
          FillWindow();
          if (lookahead == 0 && flush == (int)FlushOption.NoFlush)
            return NeedMore;
          if (lookahead == 0)
            break; // flush the current block
        }

        strstart += lookahead;
        lookahead = 0;

        // Emit a stored block if pending_buf will be full:
        max_start = blockStart + max_block_size;
        if (strstart == 0 || strstart >= max_start)
        {
          // strstart == 0 is possible when wraparound on 16-bit machine
          lookahead = (int)(strstart - max_start);
          strstart = (int)max_start;

          FlushBlockOnly(false);
          if (stream.OutputCount == 0)
            return NeedMore;
        }

        // Flush if we may have to slide, otherwise block_start may become
        // negative and the data will be gone:
        if (strstart - blockStart >= wSize - MinLookahead)
        {
          FlushBlockOnly(false);
          if (stream.OutputCount == 0)
            return NeedMore;
        }
      }

      FlushBlockOnly(flush == (int)FlushOption.Finish);
      if (stream.OutputCount == 0)
        return (flush == (int)FlushOption.Finish) ? FinishStarted : NeedMore;

      return flush == (int)FlushOption.Finish ? FinishDone : BlockDone;
    }
    
    // Send a stored block
    void TreeSendStoredBlock(int buf, int stored_len, bool eof)
    {
      SendBits(((int)DeflaterTreeType.StoredBlock << 1) + (eof ? 1 : 0), 3); // send block type
      CopyBlock(buf, stored_len, true); // with header
    }

    // Determine the best encoding for the current block: dynamic trees, static
    // trees or store, and output the encoded block to the zip file.
    void TreeFlushBlock(int buf, int stored_len, bool eof)
    {
      int opt_lenb, static_lenb; // opt_len and static_len in bytes
      int max_blindex = 0; // index of last bit length code of non zero freq

      // Build the Huffman trees unless a stored block is forced
      if (level > 0)
      {
        // Check if the file is ascii or binary
        if (dataType == (int)DeflaterBlockType.Unknown)
          SetDataType();

        // Construct the literal and distance trees
        l_desc.BuildTree(this);

        d_desc.BuildTree(this);

        // At this point, opt_len and static_len are the total bit lengths of
        // the compressed block data, excluding the tree representations.

        // Build the bit length tree for the above two trees, and get the index
        // in bl_order of the last bit length code to send.
        max_blindex = BuildBitLengthTree();

        // Determine the best encoding. Compute first the block length in bytes
        opt_lenb = Utils.ShiftRight((opt_len + 3 + 7), 3);
        static_lenb = Utils.ShiftRight((static_len + 3 + 7), 3);

        if (static_lenb <= opt_lenb)
          opt_lenb = static_lenb;
      }
      else
        opt_lenb = static_lenb = stored_len + 5; // force a stored block

      if (stored_len + 4 <= opt_lenb && buf != -1)
      {
        // 4: two words for the lengths
        // The test buf != NULL is only necessary if LIT_BUFSIZE > WSIZE.
        // Otherwise we can't have processed more than WSIZE input bytes since
        // the last block flush, because compression would have been
        // successful. If LIT_BUFSIZE <= WSIZE, it is never too late to
        // transform a block into a stored block.
        TreeSendStoredBlock(buf, stored_len, eof);
      }
      else if (static_lenb == opt_lenb)
      {
        SendBits(((int)DeflaterTreeType.StaticTree << 1) + (eof ? 1 : 0), 3);
        CompressBlock(TreeConstants.StaticLTree, TreeConstants.StaticDTree);
      }
      else
      {
        SendBits(((int)DeflaterTreeType.DynamicTree << 1) + (eof ? 1 : 0), 3);
        SendAllTrees(l_desc.MaxCode + 1, d_desc.MaxCode + 1, max_blindex + 1);
        CompressBlock(dyn_ltree, dyn_dtree);
      }

      // The above check is made mod 2^32, for files larger than 512 MB
      // and uLong implemented on 32 bits.

      InitializeBlock();

      if (eof)
        WindupBitBuffer();
    }

    // Fill the window when the lookahead becomes insufficient.
    // Updates strstart and lookahead.
    //
    // IN assertion: lookahead < MIN_LOOKAHEAD
    // OUT assertions: strstart <= window_size-MIN_LOOKAHEAD
    //    At least one byte has been read, or avail_in == 0; reads are
    //    performed for at least two bytes (required for the zip translate_eol
    //    option -- not supported here).
    void FillWindow()
    {
      int n, m;
      int p;
      int more; // Amount of free space at the end of the window.

      do
      {
        more = (windowSize - lookahead - strstart);

        // Deal with !@#$% 64K limit:
        if (more == 0 && strstart == 0 && lookahead == 0)
        {
          more = wSize;
        }
        else if (more == -1)
        {
          // Very unlikely, but possible on 16 bit machine if strstart == 0
          // and lookahead == 1 (input done one byte at time)
          more--;

          // If the window is almost full and there is insufficient lookahead,
          // move the upper half to the lower one to make room in the upper half.
        }
        else if (strstart >= wSize + wSize - MinLookahead)
        {
          Array.Copy(window, wSize, window, 0, wSize);
          matchStart -= wSize;
          strstart -= wSize; // we now have strstart >= MAX_DIST
          blockStart -= wSize;

          // Slide the hash table (could be avoided with 32 bit values
          // at the expense of memory usage). We slide even when level == 0
          // to keep the hash table consistent if we switch back to level > 0
          // later. (Using level 0 permanently is not an optimal usage of
          // zlib, so we don't care about this pathological case.)

          n = hashSize;
          p = n;
          do
          {
            m = (head[--p] & 0xffff);
						head[p] = (m >= wSize ? (short) (m - wSize) : (short)0);
          }
          while (--n != 0);

          n = wSize;
          p = n;
          do
          {
            m = (prev[--p] & 0xffff);
            prev[p] = (m >= wSize ? (short) (m - wSize) : (short)0);
            // If n is not on any hash chain, prev[n] is garbage but
            // its value will never be used.
          }
          while (--n != 0);
          more += wSize;
        }

        if (stream.InputCount == 0)
          return;

        // If there was no sliding:
        //    strstart <= WSIZE+MAX_DIST-1 && lookahead <= MIN_LOOKAHEAD - 1 &&
        //    more == window_size - lookahead - strstart
        // => more >= window_size - (MIN_LOOKAHEAD-1 + WSIZE + MAX_DIST-1)
        // => more >= window_size - 2*WSIZE + 2
        // In the BIG_MEM or MMAP case (not yet supported),
        //   window_size == input_size + MIN_LOOKAHEAD  &&
        //   strstart + s->lookahead <= input_size => more >= MIN_LOOKAHEAD.
        // Otherwise, window_size == 2*WSIZE so more >= 2.
        // If there was sliding, more >= WSIZE. So in all cases, more >= 2.

        n = stream.Read(window, strstart + lookahead, more);
        lookahead += n;

        // Initialize the hash value now that we have some input:
        if (lookahead >= MinMatch)
        {
          ins_h = window[strstart] & 0xff;
          ins_h = (((ins_h) << hashShift) ^ (window[strstart + 1] & 0xff)) & hashMask;
        }
        // If the whole input has less than MIN_MATCH bytes, ins_h is garbage,
        // but this is not important since only literal bytes will be emitted.
      }
      while (lookahead < MinLookahead && stream.InputCount != 0);
    }
    // Compress as much as possible from the input stream, return the current
    // block state.
    // This function does not perform lazy evaluation of matches and inserts
    // new strings in the dictionary only for unmatched strings or for short
    // matches. It is used only for the fast compression options.
    int DeflateFast(int flush)
    {
      //    short hash_head = 0; // head of the hash chain
      int hash_head = 0; // head of the hash chain
      bool bflush; // set if current block must be flushed

      while (true)
      {
        // Make sure that we always have enough lookahead, except
        // at the end of the input file. We need MAX_MATCH bytes
        // for the next match, plus MIN_MATCH bytes to insert the
        // string following the next match.
        if (lookahead < MinLookahead)
        {
          FillWindow();
          if (lookahead < MinLookahead && flush == (int)FlushOption.NoFlush)
          {
            return NeedMore;
          }
          if (lookahead == 0)
            break; // flush the current block
        }

        // Insert the string window[strstart .. strstart+2] in the
        // dictionary, and set hash_head to the head of the hash chain:
        if (lookahead >= MinMatch)
        {
          ins_h = (((ins_h) << hashShift) ^ (window[(strstart) + (MinMatch - 1)] & 0xff)) & hashMask;

          //	prev[strstart&w_mask]=hash_head=head[ins_h];
          hash_head = (head[ins_h] & 0xffff);
          prev[strstart & wMask] = head[ins_h];
          head[ins_h] = (short)strstart;
        }

        // Find the longest match, discarding those <= prev_length.
        // At this point we have always match_length < MIN_MATCH

        if (hash_head != 0L && ((strstart - hash_head) & 0xffff) <= wSize - MinLookahead)
        {
          // To simplify the code, we prevent matches with the string
          // of window index 0 (in particular we have to avoid a match
          // of the string with itself at the start of the input file).
          if (strategy != (int)CompressionStrategy.HuffmanOnly)
            matchLength = LongestMatch(hash_head);
          // longest_match() sets match_start
        }
        if (matchLength >= MinMatch)
        {
          //        check_match(strstart, match_start, match_length);

          bflush = TallyTree(strstart - matchStart, matchLength - MinMatch);

          lookahead -= matchLength;

          // Insert new strings in the hash table only if the match length
          // is not too large. This saves time but degrades compression.
          if (matchLength <= maxLazyMatch && lookahead >= MinMatch)
          {
            matchLength--; // string at strstart already in hash table
            do
            {
              strstart++;

              ins_h = ((ins_h << hashShift) ^ (window[(strstart) + (MinMatch - 1)] & 0xff)) & hashMask;
              //	    prev[strstart&w_mask]=hash_head=head[ins_h];
              hash_head = (head[ins_h] & 0xffff);
              prev[strstart & wMask] = head[ins_h];
              head[ins_h] = (short)strstart;

              // strstart never exceeds WSIZE-MAX_MATCH, so there are
              // always MIN_MATCH bytes ahead.
            }
            while (--matchLength != 0);
            strstart++;
          }
          else
          {
            strstart += matchLength;
            matchLength = 0;
            ins_h = window[strstart] & 0xff;

            ins_h = (((ins_h) << hashShift) ^ (window[strstart + 1] & 0xff)) & hashMask;
            // If lookahead < MIN_MATCH, ins_h is garbage, but it does not
            // matter since it will be recomputed at next deflate call.
          }
        }
        else
        {
          // No match, output a literal byte

          bflush = TallyTree(0, window[strstart] & 0xff);
          lookahead--;
          strstart++;
        }
        if (bflush)
        {

          FlushBlockOnly(false);
          if (stream.OutputCount == 0)
            return NeedMore;
        }
      }

      FlushBlockOnly(flush == (int)FlushOption.Finish);
      if (stream.OutputCount == 0)
      {
        if (flush == (int)FlushOption.Finish)
          return FinishStarted;
        else
          return NeedMore;
      }
      return flush == (int)FlushOption.Finish ? FinishDone : BlockDone;
    }

    // Same as above, but achieves better compression. We use a lazy
    // evaluation for matches: a match is finally adopted only if there is
    // no better match at the next window position.
    int DeflateSlow(int flush)
    {
      //    short hash_head = 0;    // head of hash chain
      int hash_head = 0; // head of hash chain
      bool bflush; // set if current block must be flushed

      // Process the input block.
      while (true)
      {
        // Make sure that we always have enough lookahead, except
        // at the end of the input file. We need MAX_MATCH bytes
        // for the next match, plus MIN_MATCH bytes to insert the
        // string following the next match.

        if (lookahead < MinLookahead)
        {
          FillWindow();
          if (lookahead < MinLookahead && flush == (int)FlushOption.NoFlush)
          {
            return NeedMore;
          }
          if (lookahead == 0)
            break; // flush the current block
        }

        // Insert the string window[strstart .. strstart+2] in the
        // dictionary, and set hash_head to the head of the hash chain:

        if (lookahead >= MinMatch)
        {
          ins_h = (((ins_h) << hashShift) ^ (window[(strstart) + (MinMatch - 1)] & 0xff)) & hashMask;
          //	prev[strstart&w_mask]=hash_head=head[ins_h];
          hash_head = (head[ins_h] & 0xffff);
          prev[strstart & wMask] = head[ins_h];
          head[ins_h] = (short)strstart;
        }

        // Find the longest match, discarding those <= prev_length.
        prevLength = matchLength; prevMatch = matchStart;
        matchLength = MinMatch - 1;

        if (hash_head != 0 && prevLength < maxLazyMatch && ((strstart - hash_head) & 0xffff) <= wSize - MinLookahead)
        {
          // To simplify the code, we prevent matches with the string
          // of window index 0 (in particular we have to avoid a match
          // of the string with itself at the start of the input file).

          if (strategy != (int)CompressionStrategy.HuffmanOnly)
            matchLength = LongestMatch(hash_head);
          // longest_match() sets match_start

          if (matchLength <= 5 && (strategy == (int)CompressionStrategy.Filtered || (matchLength == MinMatch && strstart - matchStart > 4096)))
          {

            // If prev_match is also MIN_MATCH, match_start is garbage
            // but we will ignore the current match anyway.
            matchLength = MinMatch - 1;
          }
        }

        // If there was a match at the previous step and the current
        // match is not better, output the previous match:
        if (prevLength >= MinMatch && matchLength <= prevLength)
        {
          int max_insert = strstart + lookahead - MinMatch;
          // Do not insert strings in hash table beyond this.

          //          check_match(strstart-1, prev_match, prev_length);

          bflush = TallyTree(strstart - 1 - prevMatch, prevLength - MinMatch);

          // Insert in hash table all strings up to the end of the match.
          // strstart-1 and strstart are already inserted. If there is not
          // enough lookahead, the last two strings are not inserted in
          // the hash table.
          lookahead -= (prevLength - 1);
          prevLength -= 2;
          do
          {
            if (++strstart <= max_insert)
            {
              ins_h = (((ins_h) << hashShift) ^ (window[(strstart) + (MinMatch - 1)] & 0xff)) & hashMask;
              //prev[strstart&w_mask]=hash_head=head[ins_h];
              hash_head = (head[ins_h] & 0xffff);
              prev[strstart & wMask] = head[ins_h];
              head[ins_h] = (short)strstart;
            }
          }
          while (--prevLength != 0);
          matchAvailable = 0;
          matchLength = MinMatch - 1;
          strstart++;

          if (bflush)
          {
            FlushBlockOnly(false);
            if (stream.OutputCount == 0)
              return NeedMore;
          }
        }
        else if (matchAvailable != 0)
        {

          // If there was no match at the previous position, output a
          // single literal. If there was a match but the current match
          // is longer, truncate the previous match to a single literal.

          bflush = TallyTree(0, window[strstart - 1] & 0xff);

          if (bflush)
          {
            FlushBlockOnly(false);
          }
          strstart++;
          lookahead--;
          if (stream.OutputCount == 0)
            return NeedMore;
        }
        else
        {
          // There is no previous match to compare with, wait for
          // the next step to decide.

          matchAvailable = 1;
          strstart++;
          lookahead--;
        }
      }

      if (matchAvailable != 0)
      {
        bflush = TallyTree(0, window[strstart - 1] & 0xff);
        matchAvailable = 0;
      }
      FlushBlockOnly(flush == (int)FlushOption.Finish);

      if (stream.OutputCount == 0)
      {
        if (flush == (int)FlushOption.Finish)
          return FinishStarted;
        else
          return NeedMore;
      }

      return flush == (int)FlushOption.Finish ? FinishDone : BlockDone;
    }
    int LongestMatch(int cur_match)
    {
      int chain_length = maxChainLength; // max hash chain length
      int scan = strstart; // current string
      int match; // matched string
      int len; // length of current match
      int best_len = prevLength; // best match length so far
      int limit = strstart > (wSize - MinLookahead) ? strstart - (wSize - MinLookahead) : 0;
      int nice_match = this.niceMatch;

      // Stop when cur_match becomes <= limit. To simplify the code,
      // we prevent matches with the string of window index 0.

      int wmask = wMask;

      int strend = strstart + MaxMatch;
      byte scan_end1 = window[scan + best_len - 1];
      byte scan_end = window[scan + best_len];

      // The code is optimized for HASH_BITS >= 8 and MAX_MATCH-2 multiple of 16.
      // It is easy to get rid of this optimization if necessary.

      // Do not waste too much time if we already have a good match:
      if (prevLength >= goodMatch)
      {
        chain_length >>= 2;
      }

      // Do not look for matches beyond the end of the input. This is necessary
      // to make deflate deterministic.
      if (nice_match > lookahead)
        nice_match = lookahead;

      do
      {
        match = cur_match;

        // Skip to next match if the match length cannot increase
        // or if the match length is less than 2:
        if (window[match + best_len] != scan_end || window[match + best_len - 1] != scan_end1 || window[match] != window[scan] || window[++match] != window[scan + 1])
          continue;

        // The check at best_len-1 can be removed because it will be made
        // again later. (This heuristic is not always a win.)
        // It is not necessary to compare scan[2] and match[2] since they
        // are always equal when the other bytes match, given that
        // the hash keys are equal and that HASH_BITS >= 8.
        scan += 2; match++;

        // We check for insufficient lookahead only every 8th comparison;
        // the 256th check will be made at strstart+258.
        do
        {
        }
        while (window[++scan] == window[++match] && window[++scan] == window[++match] && window[++scan] == window[++match] && window[++scan] == window[++match] && window[++scan] == window[++match] && window[++scan] == window[++match] && window[++scan] == window[++match] && window[++scan] == window[++match] && scan < strend);

        len = MaxMatch - (int)(strend - scan);
        scan = strend - MaxMatch;

        if (len > best_len)
        {
          matchStart = cur_match;
          best_len = len;
          if (len >= nice_match)
            break;
          scan_end1 = window[scan + best_len - 1];
          scan_end = window[scan + best_len];
        }
      }
      while ((cur_match = (prev[cur_match & wmask] & 0xffff)) > limit && --chain_length != 0);

      if (best_len <= lookahead)
        return best_len;
      return lookahead;
    }
		int Initialize(CompressionStream strm, int level, int method, int windowBits, int memLevel, int strategy)
		{
			int noheader = 0;
			//    byte[] my_version=ZLIB_VERSION;

			//
			//  if (version == null || version[0] != my_version[0]
			//  || stream_size != sizeof(z_stream)) {
			//  return Z_VERSION_ERROR;
			//  }

			strm.ErrorMessage = null;

			if (level == (int)CompressionLevel.DefaultCompression)
				level = 6;

			if (windowBits < 0)
			{
				// undocumented feature: suppress zlib header
				noheader = 1;
				windowBits = -windowBits;
			}

			if (memLevel < 1 || memLevel > Constants.MaxMemoryLevel || method != (int)CompressionMethod.Deflated || windowBits < 9 || windowBits > 15 || level < 0 || level > 9 || strategy < 0 || strategy > (int)CompressionStrategy.HuffmanOnly)
			{
				return (int)ErrorCode.StreamError;
			}

			strm.Deflater = (Deflater)this;

			this.noheader = noheader;
			wBits = windowBits;
			wSize = 1 << wBits;
			wMask = wSize - 1;

			hashBits = memLevel + 7;
			hashSize = 1 << hashBits;
			hashMask = hashSize - 1;
			hashShift = ((hashBits + MinMatch - 1) / MinMatch);

			window = new byte[wSize * 2];
			prev = new short[wSize];
			head = new short[hashSize];

			litBufsize = 1 << (memLevel + 6); // 16K elements by default

			// We overlay pending_buf and d_buf+l_buf. This works since the average
			// output size for (length,distance) codes is <= 24 bits.
			pendingBuffer = new byte[litBufsize * 4];
			pendingBufferSize = litBufsize * 4;

			d_buf = litBufsize;
			litBuf = (1 + 2) * litBufsize;

			this.level = level;

			//System.out.println("level="+level);

			this.strategy = strategy;
			this.method = (byte)method;

			return Reset(strm);
		}
		int Reset(CompressionStream strm)
		{
			strm.InputTotal = strm.OutputTotal = 0;
			strm.ErrorMessage = null; //
			strm.DataType = (int)DeflaterBlockType.Unknown;

			pending = 0;
			pendingOutput = 0;

			if (noheader < 0)
			{
				noheader = 0; // was set to -1 by deflate(..., Z_FINISH);
			}
			status = (noheader != 0) ? (int)DeflaterState.Busy : (int)DeflaterState.Initial;
			strm.Checksum = new Adler32();

			lastFlush = (int)FlushOption.NoFlush;

			InitializeTree();
			InitializeMatch();
			return (int)ErrorCode.Ok;
		}
    static bool IsSmaller(short[] tree, int n, int m, byte[] depth)
    {
			short tn2 = tree[n * 2];
			short tm2 = tree[m * 2];
			return (tn2 < tm2 || (tn2 == tm2 && depth[n] <= depth[m]));
    }
    
    // public methods...
		/// <summary>
		/// Restore the heap property by moving down the tree starting at node k,
		/// exchanging a node with the smallest of its two sons if necessary, stopping
		/// when the heap property is re-established (each father smaller than its
		/// two sons).
		/// </summary>
		/// <param name="tree">The tree to move down.</param>
		/// <param name="k">The start node.</param>
		public void RestoreHeapDown(short[] tree, int k)
		{
			int v = heap[k];
			int j = k << 1; // left son of k
			while (j <= heapLen)
			{
				// Set j to the smallest of the two sons:
				if (j < heapLen && IsSmaller(tree, heap[j + 1], heap[j], depth))
				{
					j++;
				}
				// Exit if v is smaller than both sons
				if (IsSmaller(tree, v, heap[j], depth))
					break;
				
				// Exchange v with the smallest son
				heap[k] = heap[j]; k = j;
				// And continue down the tree, setting j to the left son of k
				j <<= 1;
			}
			heap[k] = v;
		}		
		
		/// <summary>
		/// Initializes defalter using the given compression stream, compression level, and compression bits.
		/// </summary>
		/// <param name="strm">The compression stream.</param>
		/// <param name="level">The compression level.</param>
		/// <param name="bits">The compression bits.</param>
		/// <returns></returns>
		public int Initialize(CompressionStream strm, int level, int bits)
		{
      return Initialize(strm, level, (int)CompressionMethod.Deflated, bits, Constants.DefaultMemoryLevel, (int)CompressionStrategy.DefaultStrategy);
		}
		/// <summary>
		/// Initializes defalter using the given compression stream, compression level, and compression bits.
		/// </summary>
		/// <param name="strm">The compression stream.</param>
		/// <param name="level">The compression level.</param>
		/// <returns></returns>
		public int Initialize(CompressionStream strm, int level)
		{
			return Initialize(strm, level, Constants.MaxWindowBits);
		}
		/// <summary>
		/// Ends deflater operation.
		/// </summary>
		/// <returns></returns>
		public int End()
		{
      if (status != (int)DeflaterState.Initial && status != (int)DeflaterState.Busy && status != (int)DeflaterState.Finish)
        return (int)ErrorCode.StreamError;
			
      // Deallocate in reverse order of allocations:
			pendingBuffer = null;
			head = null;
			prev = null;
			window = null;
			// free
			// dstate=null;
      return status == (int)DeflaterState.Busy ? (int)ErrorCode.DataError : (int)ErrorCode.Ok;
		}
		/// <summary>
		/// Sets deflater parameters.
		/// </summary>
		/// <param name="strm">The deflater compression stream.</param>
		/// <param name="level">The compression level.</param>
		/// <param name="strategy">The deflater compression strategy.</param>
		/// <returns></returns>
		public int SetParameters(CompressionStream strm, int level, int strategy)
		{
      int err = (int)ErrorCode.Ok;
			
			if (level == (int)CompressionLevel.DefaultCompression)
				level = 6;
			if (level < 0 || level > 9 || strategy < 0 || strategy > (int)CompressionStrategy.HuffmanOnly)
        return (int)ErrorCode.StreamError;
			
			if (Options[this.level].Function != Options[level].Function && strm.InputTotal != 0)
			{
				// Flush the last buffer:
        err = strm.Deflate((int)FlushOption.PartialFlush);
			}
			
			if (this.level != level)
			{
				this.level = level;
				maxLazyMatch = Options[this.level].MaxLazy;
				goodMatch = Options[this.level].GoodLength;
				niceMatch = Options[this.level].NiceLength;
				maxChainLength = Options[this.level].MaxChain;
			}
			this.strategy = strategy;
			return err;
		}
		/// <summary>
		/// Sets deflater dictionary.
		/// </summary>
		/// <param name="strm">The compression stream.</param>
		/// <param name="dictionary">The deflater dictionary to set.</param>
		/// <param name="dictLength">The dictionary length.</param>
		/// <returns></returns>
		public int SetDictionary(CompressionStream strm, byte[] dictionary, int dictLength)
		{
			int length = dictLength;
			int index = 0;

      if (dictionary == null || status != (int)DeflaterState.Initial)
        return (int)ErrorCode.StreamError;
			
			strm.Checksum.Add(dictionary, 0, dictLength);
			
			if (length < MinMatch)
        return (int)ErrorCode.Ok;
			if (length > wSize - MinLookahead)
			{
				length = wSize - MinLookahead;
				index = dictLength - length; // use the tail of the dictionary
			}
			Array.Copy(dictionary, index, window, 0, length);
			strstart = length;
			blockStart = length;
			
			// Insert all strings in the hash table (except for the last two bytes).
			// s->lookahead stays null, so s->ins_h will be recomputed at the next
			// call of fill_window.
			
			ins_h = window[0] & 0xff;
			ins_h = (((ins_h) << hashShift) ^ (window[1] & 0xff)) & hashMask;
			
			for (int n = 0; n <= length - MinMatch; n++)
			{
				ins_h = (((ins_h) << hashShift) ^ (window[(n) + (MinMatch - 1)] & 0xff)) & hashMask;
				prev[n & wMask] = head[ins_h];
				head[ins_h] = (short) n;
			}
      return (int)ErrorCode.Ok;
		}
		/// <summary>
		/// Performs deflate operation on the given compression stream with the spcified flush option.
		/// </summary>
		/// <param name="strm">The compression stream.</param>
		/// <param name="flush">The flush option.</param>
		/// <returns></returns>
		public int Deflate(CompressionStream strm, int flush)
		{
			int old_flush;

			if (flush > (int)FlushOption.Finish || flush < 0)
			{
				return (int)ErrorCode.StreamError;
			}

			if (strm.Output == null || (strm.Input == null && strm.InputCount != 0) || (status == (int)DeflaterState.Finish && flush != (int)FlushOption.Finish))
			{
				strm.ErrorMessage = errmsg[(int)ErrorCode.NeedsDictionary - (int)ErrorCode.StreamError];
				return (int)ErrorCode.StreamError;
			}
			if (strm.OutputCount == 0)
			{
				strm.ErrorMessage = errmsg[(int)ErrorCode.NeedsDictionary - (int)ErrorCode.BufferError];
				return (int)ErrorCode.BufferError;
			}
			
			this.stream = strm; // just in case
			old_flush = lastFlush;
			lastFlush = flush;
			
			// Write the zlib header
			if (status == (int)DeflaterState.Initial)
			{
				int header = ((int)CompressionMethod.Deflated + ((wBits - 8) << 4)) << 8;
				int level_flags = ((level - 1) & 0xff) >> 1;
				
				if (level_flags > 3)
					level_flags = 3;
				header |= (level_flags << 6);
				if (strstart != 0)
					header |= (int)Constants.PresetDictionaryMask;
				header += 31 - (header % 31);

				status = (int)DeflaterState.Busy;
				WriteShortMSB(header);
				
				
				// Save the adler32 of the preset dictionary:
				if (strstart != 0)
				{
					WriteShortMSB((int) (Utils.ShiftRight(strm.Checksum.Get(), 16)));
					WriteShortMSB((int) (strm.Checksum.Get() & 0xffff));
				}
				strm.Checksum = new Adler32();
			}
			
			// Flush as much pending output as possible
			if (pending != 0)
			{
				strm.FlushPending();
				if (strm.OutputCount == 0)
				{
					//System.out.println("  avail_out==0");
					// Since avail_out is 0, deflate will be called again with
					// more output space, but possibly with both pending and
					// avail_in equal to zero. There won't be anything to do,
					// but this is not an error situation so make sure we
					// return OK instead of BUF_ERROR at next call of deflate:
					lastFlush = - 1;
					return (int)ErrorCode.Ok;
				}
				
				// Make sure there is something to do and avoid duplicate consecutive
				// flushes. For repeated and useless calls with Z_FINISH, we keep
				// returning Z_STREAM_END instead of Z_BUFF_ERROR.
			}
			else if (strm.InputCount == 0 && flush <= old_flush && flush != (int)FlushOption.Finish)
			{
				strm.ErrorMessage = errmsg[(int)ErrorCode.NeedsDictionary - ((int)ErrorCode.BufferError)];
				return (int)ErrorCode.BufferError;
			}
			
			// User must not provide more input after the first FINISH:
			if (status == (int)DeflaterState.Finish && strm.InputCount != 0)
			{
				strm.ErrorMessage = errmsg[(int)ErrorCode.NeedsDictionary - ((int)ErrorCode.BufferError)];
				return (int)ErrorCode.BufferError;
			}
			
			// Start a new block or continue the current one.
			if (strm.InputCount != 0 || lookahead != 0 || (flush != (int)FlushOption.NoFlush && status != (int)DeflaterState.Finish))
			{
				int bstate = - 1;
				switch (Options[level].Function)
				{
					case DeflaterFunction.Stored: 
						bstate = DeflateStored(flush);
						break;

					case DeflaterFunction.Fast: 
						bstate = DeflateFast(flush);
						break;

					case DeflaterFunction.Slow: 
						bstate = DeflateSlow(flush);
						break;
					
					default:
						break;
				}
				
				if (bstate == FinishStarted || bstate == FinishDone)
					status = (int)DeflaterState.Finish;
				
				if (bstate == NeedMore || bstate == FinishStarted)
				{
					if (strm.OutputCount == 0)
						lastFlush = - 1; // avoid BUF_ERROR next call, see above
          
					return (int)ErrorCode.Ok;
					// If flush != Z_NO_FLUSH && avail_out == 0, the next call
					// of deflate should use the same flush parameter to make sure
					// that the flush is complete. So we don't have to output an
					// empty block here, this will be done at next call. This also
					// ensures that for a very small output buffer, we emit at most
					// one empty block.
				}
				
				if (bstate == BlockDone)
				{
					if (flush == (int)FlushOption.PartialFlush)
						AlignTree();
					else
					{
						// FULL_FLUSH or SYNC_FLUSH
						TreeSendStoredBlock(0, 0, false);
						// For a full flush, this empty block will be recognized
						// as a special marker by inflate_sync().
						if (flush == (int)FlushOption.FullFlush)
						{
							//state.head[s.hash_size-1]=0;
							for (int i = 0; i < hashSize; i++)
								// forget history
								head[i] = 0;
						}
					}
					strm.FlushPending();
					if (strm.OutputCount == 0)
					{
						lastFlush = - 1; // avoid BUF_ERROR at next call, see above
						return (int)ErrorCode.Ok;
					}
				}
			}

			if (flush != (int)FlushOption.Finish)
				return (int)ErrorCode.Ok;
			if (noheader != 0)
				return (int)ErrorCode.StreamEnd;
			
			// Write the zlib trailer (adler32)
			WriteShortMSB((int) (Utils.ShiftRight(strm.Checksum.Get(), 16)));
			WriteShortMSB((int) (strm.Checksum.Get() & 0xffff));
			strm.FlushPending();
			
			// If avail_out is zero, the application will call deflate again
			// to flush the rest.
			noheader = - 1; // write the trailer only once!
			return pending != 0 ? (int)ErrorCode.Ok : (int)ErrorCode.StreamEnd;
		}
	}
}