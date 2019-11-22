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
using Gsafety.Common.Utilities;
using Gsafety.PTMS.Manager.Model;

namespace Gsafety.PTMS.Manager
{
    [AppConfigControl(isDefault: false, desc: "Drop Down", name: "EnumSelecter")]
    public class EnumTypeDefine : ItemTypeDefine
    {
        private char _spChar = ',';
        protected  ComboBox _control;
        

        public EnumTypeDefine()
        {
            _control = new ComboBox();
             
        }

        public override Control CreateControl(string tag,string value, Action<string> callBack)
        {
            _control.ItemsSource = (tag??"").Split(_spChar);
            if(!string.IsNullOrWhiteSpace(value))
            {
                _control.SelectedValue=value;
            }
            _control.SelectionChanged += (s, e) => { callBack(_control.SelectedValue as string); };
            return _control;
        }

        public override void ShoDesignControl(string tag,Action<string> callBack)
        {
          
            var window = new AddEnumWindow();
            var source = JsonHelper.FromJsonString<SerializableItem>(tag);
            if (source!=null && !string.IsNullOrWhiteSpace(source.Tag))
            {
                foreach (var x in source.Tag.Split(','))
                {
                    window.SourceList.Add(x);
                }
            }
            window.OKButton.Click += (s, e) =>
            {
               
                if (callBack != null)
                {
                    var arg = new SerializableItem
                    {
                        Name="EnumSelecter",
                        Desc="下拉框",
                        Tag = string.Join(_spChar.ToString(), window.SourceList),
                    };
                   var json=JsonHelper.ToJsonString(arg);
                   callBack(json);
                }
            };
            window.Show();
        }


      
    }
}