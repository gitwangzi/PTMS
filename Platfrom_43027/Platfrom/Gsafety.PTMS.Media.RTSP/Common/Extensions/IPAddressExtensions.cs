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

namespace Gsafety.PTMS.Media.RTSP.Common.Extensions.IPAddress
{
    public static class IPAddressExtensions
    {
        private static System.Net.IPAddress emptyIpv4 = System.Net.IPAddress.Parse("0.0.0.0");
        private static System.Net.IPAddress intranetMask1v4 = System.Net.IPAddress.Parse("10.255.255.255");
        private static System.Net.IPAddress intranetMask2v4 = System.Net.IPAddress.Parse("172.16.0.0");
        private static System.Net.IPAddress intranetMask3v4 = System.Net.IPAddress.Parse("172.31.255.255");
        private static System.Net.IPAddress intranetMask4v4 = System.Net.IPAddress.Parse("192.168.255.255");

        //Should check if ipV6 is even supported before defining them.
        //Shoul be null and then in Static constructor should check =>
        //System.Net.Sockets.Socket.OSSupportsIPv6 or try GetIPv6Properties().Index > -999 from the networkInterface...

        private static System.Net.IPAddress emptyIpv6 = System.Net.IPAddress.IPv6Any;
        private static System.Net.IPAddress intranetMask1v6 = System.Net.IPAddress.Parse("::ffff:10.255.255.255");
        private static System.Net.IPAddress intranetMask2v6 = System.Net.IPAddress.Parse("::ffff:172.16.0.0");
        private static System.Net.IPAddress intranetMask3v6 = System.Net.IPAddress.Parse("::ffff:172.31.255.255");
        private static System.Net.IPAddress intranetMask4v6 = System.Net.IPAddress.Parse("::ffff:192.168.255.255");
    }
}
