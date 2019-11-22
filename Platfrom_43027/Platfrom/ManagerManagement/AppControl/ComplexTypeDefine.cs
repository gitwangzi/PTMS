using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

namespace Gsafety.PTMS.Manager
{
  [AppConfigControl(isDefault: false, desc: "Type Define", name: "CompxTypeDefine")]
    public class ComplexTypeDefine:ItemTypeDefine 
    {
      private CompxControl _control;

      public ComplexTypeDefine()
      {
          _control = new CompxControl();
      }
        public override Control CreateControl(string tag, string value, Action<string> callBack)
        {
            var data = JsonHelper.FromJsonString<List<Employee>>(value) ?? new List<Employee>();
            data.ForEach(x => _control.EmployeeData.Add(x));
            _control.SaveDataCallBack = callBack;
            return _control;
        }
    }
}
