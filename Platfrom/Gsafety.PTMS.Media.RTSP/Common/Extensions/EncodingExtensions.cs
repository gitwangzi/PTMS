#region Copyright
/*
This file came from Managed Media Aggregation, You can always find the latest version @ https://net7mma.codeplex.com/
  
 Julius.Friedman@gmail.com / (SR. Software Engineer ASTI Transportation Inc. http://www.asti-trans.com)

Permission is hereby granted, free of charge, 
 * to any person obtaining a copy of this software and associated documentation files (the "Software"), 
 * to deal in the Software without restriction, 
 * including without limitation the rights to :
 * use, 
 * copy, 
 * modify, 
 * merge, 
 * publish, 
 * distribute, 
 * sublicense, 
 * and/or sell copies of the Software, 
 * and to permit persons to whom the Software is furnished to do so, subject to the following conditions:
 * 
 * 
 * JuliusFriedman@gmail.com should be contacted for further details.

The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, 
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. 
 * 
 * IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, 
 * DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, 
 * TORT OR OTHERWISE, 
 * ARISING FROM, 
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
 * 
 * v//
 */
#endregion

using System;
using System.Linq;
using Gsafety.PTMS.Media.Common.Loggers;
using System.Reflection;

namespace Gsafety.PTMS.Media.RTSP.Common.Extensions.Encoding
{
    [CLSCompliant(true)]
    public static class EncodingExtensions
    {
        public static readonly char[] EmptyChar = new char[0];

        #region GetByteCount

        /// <summary>
        /// Allows a call to <see cref="Encoding.GetByteCount"/> with only 1 char, e.g. (GetByteCount('/0'))
        /// Uses the Default encoding if none was provided.
        /// </summary>
        /// <param name="encoding"></param>
        /// <param name="chars"></param>
        /// <returns></returns>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static int GetByteCount(this System.Text.Encoding encoding, params char[] chars)
        {
            if (encoding == null) encoding = System.Text.Encoding.UTF8;

            return encoding.GetByteCount(chars);
        }

        #endregion

        #region Read Delimited Data

        /// <summary>
        /// Reads the data in the stream using the given encoding until the first occurance of any of the given delimits are found, count is reached or the end of stream occurs.
        /// </summary>
        /// <param name="encoding"></param>
        /// <param name="stream"></param>
        /// <param name="delimits"></param>
        /// <param name="count"></param>
        /// <param name="result"></param>
        /// <param name="read"></param>
        /// <param name="any"></param>
        /// <param name="includeDelimits"></param>
        /// <returns></returns>
        [CLSCompliant(false)]
        public static bool ReadDelimitedDataFrom(this System.Text.Encoding encoding, System.IO.Stream stream, char[] delimits, ulong count, out string result, out ulong read, out System.Exception any, bool includeDelimits = true)
        {
            read = 0;

            any = null;

            result = string.Empty;

            //Todo, check for large delemits and use a hash or always use a hash.
            //System.Collections.Generic.HashSet<char> delimitsC = new System.Collections.Generic.HashSet<char>(delimits);

            if (delimits == null) delimits = EmptyChar;

            if (stream == null || false == stream.CanRead || count == 0)
            {
                result = null;

                return false;
            }

            long at = stream.Position;// max = stream.Length

            //Let the exception enfore the bounds for now
            if (at >= stream.Length) return false;

            //Use default..
            if (encoding == null) encoding = System.Text.Encoding.UTF8;

            System.Text.StringBuilder builder = null;

            bool sawDelimit = false;

            //Make the builder
            builder = new System.Text.StringBuilder();

            //Use the BinaryReader on the stream to ensure ReadChar reads in the correct size
            //This prevents manual conversion from byte to char and uses the encoding's code page.

            //TODO:避免流被关闭
            using (var newStream = new System.IO.MemoryStream())
            {
                stream.Position = 0;
                stream.CopyTo(newStream);
                stream.Position = at;
                newStream.Position = stream.Position;

                using (var br = new System.IO.BinaryReader(newStream, encoding))
                {
                    char cached = '0';

                    //Read a while permitted, check for EOS
                    while (read < count && newStream.CanRead)
                    {
                        try
                        {
                            //Get the char in the encoding beging used
                            try
                            {
                                cached = Convert.ToChar(br.ReadByte());
                            }
                            catch (System.Exception ex)
                            {
                                break;
                            }

                            //Determine where ReadChar advanced to (e.g. if Fallback occured)
                            read = (ulong)(newStream.Position - at);

                            //delimitsC.Contains(cached);

                            //If the char was a delemit, indicate the delimit was seen
                            if (sawDelimit = System.Array.IndexOf<char>(delimits, cached) >= 0)
                            {
                                //if the delemit should be included, include it.
                                if (includeDelimits) builder.Append(cached);
                                stream.Position = newStream.Position;
                                //Do not read further
                                break;
                            }

                            //append the char
                            builder.Append(cached);
                        }
                        catch (System.Exception ex)
                        {
                            LoggerInstance.Exception(MethodBase.GetCurrentMethod().ToString() + ex);

                            //Handle the exception
                            any = ex;

                            //Take note of the position only during exceptions
                            read = (ulong)(newStream.Position - at);
                            stream.Position = newStream.Position;

                            //Do not read further
                            //goto Done;
                            break;
                        }
                    }
                }
            }

            if (builder == null)
            {
                result = null;

                return sawDelimit;
            }

            result = builder.Length == 0 ? string.Empty : builder.ToString();

            builder = null;

            return sawDelimit;
        }

        /// <summary>
        /// Reads the data in the stream using the given encoding until the first occurance of any of the given delimits are found, count is reached or the end of stream occurs.
        /// </summary>
        /// <param name="encoding"></param>
        /// <param name="stream"></param>
        /// <param name="delimits"></param>
        /// <param name="count"></param>
        /// <param name="result"></param>
        /// <param name="includeDelimits"></param>
        /// <returns>True if a given delimit was found, otherwise false.</returns>        
        [CLSCompliant(false)]
        public static bool ReadDelimitedDataFrom(this System.Text.Encoding encoding, System.IO.Stream stream, char[] delimits, ulong count, out string result, out ulong read, bool includeDelimits = true)
        {
            System.Exception encountered;

            return ReadDelimitedDataFrom(encoding, stream, delimits, count, out result, out read, out encountered, includeDelimits);
        }

        public static bool ReadDelimitedDataFrom(this System.Text.Encoding encoding, System.IO.Stream stream, char[] delimits, long count, out string result, out long read, out System.Exception any, bool includeDelimits = true)
        {
            ulong cast;

            bool found = ReadDelimitedDataFrom(encoding, stream, delimits, (ulong)count, out result, out cast, out any, includeDelimits);

            read = (int)cast;

            return found;
        }

        public static bool ReadDelimitedDataFrom(this System.Text.Encoding encoding, System.IO.Stream stream, char[] delimits, long count, out string result, out long read, bool includeDelimits = true)
        {
            ulong cast;

            bool found = ReadDelimitedDataFrom(encoding, stream, delimits, (ulong)count, out result, out cast, includeDelimits);

            read = (long)cast;

            return found;
        }

        #endregion
    }
}