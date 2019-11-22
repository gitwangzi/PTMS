using Jounce.Core.ViewModel;
using System;
using System.ComponentModel.Composition;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace HistoryQueryManagement
{
    public class HistoryQueryBinding
    {
        [Export]
        public ViewModelRoute HistoryQueryMainPageBinding
        {
            get
            {
                return ViewModelRoute.Create(HistoryQueryName.HistoryQueryMainVm, HistoryQueryName.HistoryQueryMainV);
            }
        }
    }
}
