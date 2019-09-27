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
    public class ComboBoxBasicStruct<T>
    {
        /// <summary>
        /// 下拉列表的键
        /// </summary>
        public T Key { get; set; }
        /// <summary>
        /// 下拉列表显示内容
        /// </summary>
        public string Value { get; set; }
    }
}
