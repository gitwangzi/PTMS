using Gsafety.Common.Logging;
/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: 03058098-f582-415d-abba-1b047e19a042      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: PC-LIW
/////                 Author: TEST(liw)
/////======================================================================
/////           Project Name: Gsafety.PTMS.Analysis.Helper
/////    Project Description:    
/////             Class Name: GISConvertHelper
/////          Class Version: v1.0.0.0
/////            Create Time: 2013/9/9 17:41:10
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2013/9/9 17:41:10
/////            Modified by:
/////   Modified Description: 
/////======================================================================
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gsafety.Common.Util
{
    public class GISConvertHelper
    {
        /// <summary>
        /// 纬度转换
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string GetLatitude(string value)
        {
            string temp = value.ToString().Replace(".", System.Globalization.NumberFormatInfo.CurrentInfo.NumberDecimalSeparator);
            int indflag = temp.IndexOf("-");
            temp = temp.Replace("-", "");

            int du = 0;
            double fen = 0;

            int ind = temp.IndexOf(System.Globalization.NumberFormatInfo.CurrentInfo.NumberDecimalSeparator);
            if (ind == -1)
            {
                ind = temp.Length;
            }

            if ((ind - 3 + 1) >= 1)
            {
                if (temp.Substring(0, ind - 3 + 1) != "") du = int.Parse(temp.Substring(0, ind - 3 + 1));
                if ((temp.Substring(ind - 2)) != "") fen = double.Parse(temp.Substring(ind - 2));
            }
            else
            {
                fen = double.Parse(temp);
            }

            if (indflag > -1) return (-du - fen / 60).ToString();
            else return (du + fen / 60).ToString();
        }


        /// <summary>
        /// 经度转换
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string GetLongitude(string value)
        {
            string temp = value.ToString().Replace(".", System.Globalization.NumberFormatInfo.CurrentInfo.NumberDecimalSeparator);
            int indflag = temp.IndexOf("-");
            temp = temp.Replace("-", "");

            int du = 0;
            double fen = 0;

            int ind = temp.IndexOf(System.Globalization.NumberFormatInfo.CurrentInfo.NumberDecimalSeparator);
            if (ind == -1)
            {
                ind = temp.Length;
            }

            if ((ind - 3 + 1) >= 1)
            {
                if (temp.Substring(0, ind - 3 + 1) != "") du = int.Parse(temp.Substring(0, ind - 3 + 1));
                if ((temp.Substring(ind - 2)) != "") fen = double.Parse(temp.Substring(ind - 2));
            }
            else
            {
                fen = double.Parse(temp);
            }
            if (indflag > -1) return (-du - fen / 60).ToString();
            else return (du + fen / 60).ToString();
        }

        /// <summary>
        /// 方向转换
        /// </summary>
        /// <returns></returns>
        public static string GetDirection(object value, System.Globalization.CultureInfo culture)
        {
            if (value == null) return null;
            string temp = value.ToString().Replace(".", System.Globalization.NumberFormatInfo.CurrentInfo.NumberDecimalSeparator);
            return temp;
        }

        /// <summary>
        /// 速度转换
        /// </summary>
        /// <returns></returns>
        public static string GetSpeed(object value, System.Globalization.CultureInfo culture)
        {
            if (value == null) return null;
            string temp = value.ToString().Replace(".", System.Globalization.NumberFormatInfo.CurrentInfo.NumberDecimalSeparator);
            return temp;
        }
    }
}
