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
    [AppConfigControl(isDefault:false,desc:"DataType",name:"DatePicker")]
    public class DataPickTypeDefine:ItemTypeDefine 
    {
        protected DatePicker _control;

        public DataPickTypeDefine()
        {
            this._control = new DatePicker();
        }

        public override Control CreateControl(string tag, string value, Action<string> callBack)
        {
            DateTime date;
            if (!DateTime.TryParse(value, out date))
            {
                date = DateTime.Now;
            }
            _control.Text = date.ToString();
            _control.SelectedDateChanged += (s, e) => { callBack(_control.Text); };

            return _control;
        }
        

        
    }
}
