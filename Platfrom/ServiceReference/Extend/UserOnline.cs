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

namespace Gsafety.PTMS.ServiceReference.AccountService
{
    public partial class UserOnline
    {
        private string _onlinetimespan = string.Empty;

        public string OnlineTimeSpan
        {
            get { return _onlinetimespan; }
            set { _onlinetimespan = value; }
        }

    }
}
