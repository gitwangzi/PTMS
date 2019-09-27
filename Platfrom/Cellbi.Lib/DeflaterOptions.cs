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
	/// Deflater options class.
	/// </summary>
  public class DeflaterOptions
  {
    int goodLength;
    int maxLazy;
    int niceLength;
    int maxChain;
    DeflaterFunction function;

    // constructors...
		/// <summary>
		/// Creates new instance of the DeflaterOptions class.
		/// </summary>
		/// <param name="goodLength">The good length option.</param>
		/// <param name="maxLazy">The maximum lazy option.</param>
		/// <param name="niceLength">The nice length option.</param>
		/// <param name="maxChain">The maximum chain option.</param>
		/// <param name="function">The deflater function option.</param>
    public DeflaterOptions(int goodLength, int maxLazy, int niceLength, int maxChain, DeflaterFunction function)
    {
      this.goodLength = goodLength;
      this.maxLazy = maxLazy;
      this.niceLength = niceLength;
      this.maxChain = maxChain;
      this.function = function;
    }

    // public properties...
		/// <summary>
		/// Gets or sets good length.
		/// Reduce lazy search above this match length.
		/// </summary>
    public int GoodLength
    {
      get { return goodLength; }
      set { goodLength = value; }
    }
		/// <summary>
		/// Gets or sets maximum lazy.
		/// Do not perform lazy search above this match length.
		/// </summary>
    public int MaxLazy
    {
      get { return maxLazy; }
      set { maxLazy = value; }
    }
		/// <summary>
		/// Gets or sets nice length.
		/// Quit search above this match length.
		/// </summary>
    public int NiceLength
    {
      get { return niceLength; }
      set { niceLength = value; }
    }
		/// <summary>
		/// Gets or sets maximum chain.
		/// </summary>
    public int MaxChain
    {
      get { return maxChain; }
      set { maxChain = value; }
    }
		/// <summary>
		/// Gets or sets deflater function.
		/// </summary>
    public DeflaterFunction Function
    {
      get { return function; }
      set { function = value; }
    }
  }
}