/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: f36b1e5e-bb0f-4ad9-a223-90ae4ex28a34      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: PC-DENGZL
/////                 Author: TEST(dengzl)
/////======================================================================
/////           Project Name: Gsafety.Common.Converts
/////    Project Description:    
/////             Class Name: VehicleStateToMinImage
/////          Class Version: v1.0.0.0
/////            Create Time: 9/24/2013 10:06:07 AM
/////      Class Description:  
/////======================================================================
/////          Modified Time: 9/24/2013 10:06:07 AM
/////            Modified by:
/////   Modified Description: 
/////======================================================================
using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Gsafety.PTMS.Share;
using Gsafety.PTMS.Bases.Enums;

namespace Gsafety.Common.Converts
{
    public class VehicleStateToMinImage : IMultiValueConverter
    {
        public Object Convert(object[] values, Type targetType, Object parameter, System.Globalization.CultureInfo culture)
        {
            if (values == null || values.Length != 2 || values[0] == null || values[1] == null)
                return string.Empty;
            VehicleType vehicleType = (VehicleType)values[0];
            bool vehicleState = false;
            if (values[1] != null)
            {
                vehicleState = (bool)values[1];
            }
            if (vehicleType == VehicleType.Bus)//bus
            {

                if (vehicleState)//online
                {
                    return "/ExternalResource;component/Images/onLineBusMin.png";
                }
                else
                {
                    return "/ExternalResource;component/Images/outLineBusMin.png";
                }

            }
            else if (vehicleType == VehicleType.Taxi)//taxi
            {
                if (vehicleState)//online 
                {
                    return "/ExternalResource;component/Images/onlineTaxiMin.png";
                }
                else
                {
                    return "/ExternalResource;component/Images/outlineTaxiMin.png";
                }
            }
            else if (vehicleType == VehicleType.Flota)//Flota
            {
                if (vehicleState)//online
                {
                    return "/ExternalResource;component/Images/onlineIntercityBusMin.png";
                }
                else
                {
                    return "/ExternalResource;component/Images/outlineIntercityBusMin.png";
                }

            }
            return string.Empty;
        }

        public object[] ConvertBack(object value, Type[] targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
