using Gsafety.PTMS.Share;
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

namespace Gsafety.Common.CommMessage
{
    public class PredefinedColor
    {
        public PredefinedColor(byte a, byte r, byte g, byte b, string name)
        {
            Value = Color.FromArgb(a, r, g, b);
            Name = name;
            NameV = ApplicationContext.Instance.StringResourceReader.GetString(name);
        }
        public Color Value { get; private set; }
        public string Name { get; private set; }
        public string NameV { get; private set; }
    }
}
