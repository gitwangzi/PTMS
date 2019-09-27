using System;
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
    public class NameValueModel<T>
    {
        string name = string.Empty;

        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        T innervalue;

        public T Value
        {
            get { return innervalue; }
            set { innervalue = value; }
        }
    }
}
