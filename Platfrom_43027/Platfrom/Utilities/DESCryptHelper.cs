using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
using System.IO;

namespace Gsafety.Common.Utilities
{
    public sealed class CryptHelper
    {
        #region URLEncrypt
        /// <summary>
        ///  URLEncrypt
        /// </summary>
        /// <param name="str">value</param>
        /// <returns></returns>
        public static string URLEncrypt(string str)
        {
            string EnStr;
            byte[] bstr = System.Text.Encoding.UTF8.GetBytes(str);
            EnStr = Convert.ToBase64String(bstr);
            EnStr = EnStr.Replace("+", "@");
            return EnStr;
        }
        #endregion
    }
}
