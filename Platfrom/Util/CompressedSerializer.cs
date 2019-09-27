using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Runtime.Serialization;

namespace Gsafety.Common.Util
{
	public static class CompressedSerializer
	{
		/// <summary> 
		/// Decompresses the specified compressed data. 
		/// </summary> 
		/// <typeparam name="T"></typeparam> 
		/// <param name="compressedData">The compressed data.</param> 
		/// <returns></returns> 
		public static T Decompress<T>(byte[] compressedData) where T : class
		{
			T result = null;
			using (MemoryStream memory = new MemoryStream())
			{
				DataContractSerializer ser = new DataContractSerializer(typeof(T));
				byte[] outArray = Cellbi.SvZLib.Utils.Decompress(compressedData);

				memory.Write(outArray, 0, outArray.Length);
				memory.Position = 0;
				result = ser.ReadObject(memory) as T;
			}
			return result;
		}
		/// <summary> 
		/// Compresses the specified data. 
		/// </summary> 
		/// <typeparam name="T"></typeparam> 
		/// <param name="data">The data.</param> 
		/// <returns></returns> 
		public static byte[] Compress<T>(T data)
		{
			byte[] result = null;
			using (MemoryStream memory = new MemoryStream())
			{
				DataContractSerializer ser = new DataContractSerializer(typeof(T));
				ser.WriteObject(memory, data);
				byte[] array = memory.ToArray();

				result = Cellbi.SvZLib.Utils.Compress(array);
			}
			return result;
		}
	} 
}
