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
using System.Linq;
using Gsafety.Common.Utilities;
using Gsafety.PTMS.AppConfig;
using Gsafety.PTMS.Manager.Model;

namespace Gsafety.PTMS.Manager
{
    public abstract  class ItemTypeDefine
    {
        public virtual  void ShoDesignControl(string tag, Action<string> callBack)
        {
            var att =(AppConfigControlAttribute)this.GetType().GetCustomAttributes(typeof(AppConfigControlAttribute), false).FirstOrDefault();
            if (att == null || callBack == null)
            {
                return;
            }
            var arg = new SerializableItem
            {
                Name = att.Name,
                Desc = att.Desc,
            };
            var json = JsonHelper.ToJsonString(arg);
            callBack(json);
        }

        public abstract Control CreateControl(string tag,string value, Action<string> callBack);
         
    }
}
