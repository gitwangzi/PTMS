/////Copyright (C) Gsafety 2014 .All Rights Reserved.
/////======================================================================
/////                   Guid: 61a4867f-5bc8-46c0-be79-74243dac6334      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: PC-ZHANGY
/////                 Author: GJSY(zhangy)
/////======================================================================
/////           Project Name: Gsafety.PTMS.Manager
/////    Project Description:    
/////             Class Name: ManagerCommon
/////          Class Version: v1.0.0.0
/////            Create Time: 2014/11/11 13:32:26
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2014/11/11 13:32:26
/////            Modified by:
/////   Modified Description: 
/////======================================================================
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Linq;

namespace Gsafety.PTMS.Manager
{
    public class ManagerCommon
    {
        public static bool CheckIdentity(string identity)
        {
            try
            {
                var cedula = int.Parse(identity);
                //array = cedula.split( "" );
                int num = identity.Length;
                if (num == 10)
                {
                    int[] array = new int[num];
                    for (int i = 0; i < num; i++)
                    {
                        array[i] = int.Parse(identity[i].ToString());
                    }
                    int total = 0;
                    int digito = (array[9] * 1);
                    for (int i = 0; i < (num - 1); i++)
                    {
                        int mult = 0;
                        if ((i % 2) != 0)
                        {
                            total = total + (array[i] * 1);
                        }
                        else
                        {
                            mult = array[i] * 2;
                            if (mult > 9)
                                total = total + (mult - 9);
                            else
                                total = total + mult;
                        }
                    }
                    double decena = total / 10;
                    decena = Math.Floor(decena);
                    decena = (decena + 1) * 10;
                    var final = (decena - total);
                    if ((final == 10 && digito == 0) || (final == digito))
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                return false;
            }
        }

        public static bool CheckVehicleId(string vehicleId)
        {
            try
            {
                int num = vehicleId.Length;
                if (num == 7)
                {
                    string alphabet = vehicleId.Substring(0, 3);
                    if (!Regex.IsMatch(alphabet, @"^[A-Z]+$", RegexOptions.IgnoreCase))
                    {
                        return false;
                    }

                    string number = vehicleId.Substring(3, 4);
                    if (!Regex.IsMatch(number, @"^[0-9]+$"))
                    {
                        return false;
                    }

                    return true;
                }
                if (num == 8)
                {
                    string alphabet = vehicleId.Substring(0, 4);
                    if (!Regex.IsMatch(alphabet, @"^[A-Z]+$", RegexOptions.IgnoreCase))
                    {
                        return false;
                    }

                    string number = vehicleId.Substring(4, 4);
                    if (!Regex.IsMatch(number, @"^[0-9]+$"))
                    {
                        return false;
                    }

                    return true;
                }
                return false;
            }
            catch
            {
                return false;
            }
        }

        public static bool CheckTelephone(string telephone)
        {
            try
            {
                int num = telephone.Length;
                if (num == 11)
                {
                    long.Parse(telephone);
                    return true;
                }
                return false;
            }
            catch
            {
                return false;
            }
        }

        public static bool CheckDigit(string digit, int length)
        {
            try
            {
                int num = digit.Length;
                if (num == length)
                {
                    if (Regex.IsMatch(digit, @"^[0-9]+$"))
                    {
                        return true;
                    }
                }
                return false;
            }
            catch
            {
                return false;
            }
        }

        public static T Clone<T>(T t)
        {
            var ser = new System.Runtime.Serialization.DataContractSerializer(typeof(T));
            using (var ms = new MemoryStream())
            {
                ser.WriteObject(ms, t);
                ms.Seek(0, SeekOrigin.Begin);
                return (T)ser.ReadObject(ms); ;
            }
        }

        public static List<int> PageSizeList
        {
            get
            {
                List<int> pageSizeList = new List<int>();
                pageSizeList.Add(20);
                pageSizeList.Add(40);
                pageSizeList.Add(80);
                return pageSizeList;
            }
        }
    }
}
