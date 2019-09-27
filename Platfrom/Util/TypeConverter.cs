using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Gsafety.Common.Util
{
    public class TypeConverter
    {
        public static T ByteToObject<T>(byte[] bytes, int length) where T : class
        {
            DataContractSerializer serializer = new DataContractSerializer(typeof(T));
            using (MemoryStream ms = new MemoryStream())
            {
                ms.Write(bytes, 0, length);
                ms.Position = 0;
                T t = serializer.ReadObject(ms) as T;
                return t;
            }
        }

        public static byte[] ObjectToByte<T>(T t)
        {
            DataContractSerializer serializer = new DataContractSerializer(typeof(T));
            using (MemoryStream ms = new MemoryStream())
            {
                serializer.WriteObject(ms, t);
                return ms.ToArray();
            }
        }
    }
}
