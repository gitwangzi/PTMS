using System;
using System.Collections.Generic;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace BaseLib.Model
{
    public class BaseCommon
    {
        public static List<int> PageSizeList
        {
            get
            {
                List<int> pageSizeList = new List<int>();
                pageSizeList.Add(10);
                pageSizeList.Add(20);
                pageSizeList.Add(40);
                pageSizeList.Add(80);                
                return pageSizeList;
            }
        }
    }
}
