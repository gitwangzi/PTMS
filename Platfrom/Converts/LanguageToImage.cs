/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: 1dd579e3-4aae-44bf-9a81-fe43049983e1      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: PC-DENGZL
/////                 Author: TEST(dengzl)
/////======================================================================
/////           Project Name: Gsafety.Common.Converters
/////    Project Description:    
/////             Class Name: VehicleTypeToImage
/////          Class Version: v1.0.0.0
/////            Create Time: 8/16/2013 4:54:34 PM
/////      Class Description:  
/////======================================================================
/////          Modified Time: 8/16/2013 4:54:34 PM
/////            Modified by:
/////   Modified Description: 
/////======================================================================
using System;
using System.Windows.Data;

namespace Gsafety.Common.Converts
{
    public class LanguageToImage : IValueConverter
    {
        readonly string ZHPath = "/InstallationManagement;component/Images/";
        readonly string PTPath = "/InstallationManagement;component/ImagesPt/";
        readonly string ESPath = "/InstallationManagement;component/ImagesEs/";
        readonly string ENPath = "/InstallationManagement;component/ImagesEn/";
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {

            string languageName = System.Threading.Thread.CurrentThread.CurrentCulture.Name;
            if(string.IsNullOrEmpty(languageName))
            {
                languageName = "es-ES";
            }

            if(value != null)
            {
                string picture = (string)value;
                if(languageName.Equals("zh-CN"))
                    return string.Format("{0}{1}", ZHPath, picture);
                else if(languageName.Equals("pt-BR"))
                    return string.Format("{0}{1}", PTPath, picture);
                else if(languageName.Equals("en-US"))
                    return string.Format("{0}{1}", ENPath, picture);
                else
                    return string.Format("{0}{1}", ESPath, picture);

            }
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }

    }
}
