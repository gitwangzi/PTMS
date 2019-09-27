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
	/// Static compression tree.
	/// </summary>
	sealed class CompressionStaticTree
	{	
		static CompressionStaticTree staticLTreeDescription;
		static CompressionStaticTree staticDTreeDescription;
		static CompressionStaticTree staticBLDescription;

    short[] staticTreeData;
    int[] extraBits;
    int extraBase;
    int maxElements;
    int maxLength;

    // constructors...
		/// <summary>
		/// Initializes a new instance of the CompressionStaticTree class.
		/// </summary>
    static CompressionStaticTree()
    {
      staticLTreeDescription = new CompressionStaticTree(TreeConstants.StaticLTree, TreeConstants.ExtraLBits, TreeConstants.Literals + 1, TreeConstants.LCodes, TreeConstants.MaxBits);
      staticDTreeDescription = new CompressionStaticTree(TreeConstants.StaticDTree, TreeConstants.ExtraDBits, 0, TreeConstants.DCodes, TreeConstants.MaxBits);
      staticBLDescription = new CompressionStaticTree(null, TreeConstants.ExtraBLBits, 0, TreeConstants.BLCodes, TreeConstants.MaxBLBits);
    }

		/// <summary>
		/// Initializes a new instance of the CompressionStaticTree class.
		/// </summary>
		/// <param name="staticTreeData">The static tree data to use.</param>
		/// <param name="extraBits">The extra bits for each code.</param>
		/// <param name="extraBase">The base index for extra bits.</param>
		/// <param name="elements">The max number of elements in the tree.</param>
		/// <param name="maxLength">The max bit length for the tree codes.</param>
    public CompressionStaticTree(short[] staticTreeData, int[] extraBits, int extraBase, int elements, int maxLength)
		{
			this.staticTreeData = staticTreeData;
			this.extraBits = extraBits;
			this.extraBase = extraBase;
			this.maxElements = elements;
			this.maxLength = maxLength;
		}

    // public properties...
		/// <summary>
		/// Gets static L compression tree description.
		/// </summary>
		public static CompressionStaticTree StaticLTreeDescription
		{
			get { return staticLTreeDescription; }
		}
		/// <summary>
		/// Gets static D compression tree description.
		/// </summary>
		public static CompressionStaticTree StaticDTreeDescription
		{
			get { return staticDTreeDescription; }
		}
		/// <summary>
		/// Gets static BL compression tree description.
		/// </summary>
		public static CompressionStaticTree StaticBLDescription
		{
			get { return staticBLDescription; }
		}
		/// <summary>
		/// Gets or sets static tree data.
		/// </summary>
		public short[] StaticTreeData
		{
      get { return staticTreeData; }
      set { staticTreeData = value; }
    }
		/// <summary>
		/// Gets or sets extra bits for each code or null.
		/// </summary>
    public int[] ExtraBits
    {
      get { return extraBits; }
      set { extraBits = value; }
    }
		/// <summary>
		/// Gets or sets base index for extra bits.
		/// </summary>
    public int ExtraBase
    {
      get { return extraBase; }
      set { extraBase = value; }
    }
		/// <summary>
		/// Gets or sets max number of elements in the tree.
		/// </summary>
    public int MaxElements
    {
      get { return maxElements; }
      set { maxElements = value; }
    }
		/// <summary>
		/// Gets or sets max bit length for the tree codes.
		/// </summary>
    public int MaxLength
    {
      get { return maxLength; }
      set { maxLength = value; }
    }
	}
}