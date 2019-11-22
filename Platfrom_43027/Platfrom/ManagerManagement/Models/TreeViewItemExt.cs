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

namespace Gsafety.PTMS.Manager.Models
{
    public class TreeViewItemExt : TreeViewItem
    {
        /// <summary>
        /// 
        /// </summary>
        public Hyperlink Link { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string NavigateUri { get; set; }

        public string Uri { get; set; }
    }
}
