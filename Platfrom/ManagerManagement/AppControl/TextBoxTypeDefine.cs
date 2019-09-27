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

namespace Gsafety.PTMS.Manager
{
    [AppConfigControl(isDefault: true, desc: "TextBox", name: "TextBox")]
    public class TextBoxTypeDefine:ItemTypeDefine 
    {
        protected TextBox _control;

        public TextBoxTypeDefine()
        {
            _control = new TextBox();
        }

        public override Control CreateControl(string tag, string value, Action<string> callBack)
        {
            _control.Text = value ?? "";
            _control.TextChanged += (s, e) => { callBack(_control.Text); };

            return _control;
        }


    }
}
