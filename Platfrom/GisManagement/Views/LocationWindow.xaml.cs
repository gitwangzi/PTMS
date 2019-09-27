using Gsafety.Common.Controls;
using Gsafety.PTMS.Share;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace GisManagement.Views
{
    public partial class LocationWindow : ChildWindowWithCheck
    {
        Regex latitiuderegex = new Regex("^[-]?[0-8]?[0-9]?(\\.\\d*)?$");
        Regex longituderegex = new Regex("^[-]?([1]?[0-7]?[0-9]?(\\.\\d*)?|180)?$");
        public LocationWindow()
        {
            InitializeComponent();
            this.DataContext = this;
        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            if (ValidateLongitude("Longitude", longitude) && ValidateLongitude("Latitude", latitude))
            {
                this.DialogResult = true;
            }
            else
            {
                return;
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        string longitude = string.Empty;

        public string Longitude
        {
            get { return longitude; }
            set
            {
                longitude = value;
                ValidateLongitude("Longitude", longitude);
            }
        }

        private bool ValidateLongitude(string prop, string value)
        {
            bool isSuccess = true;
            ClearErrors(prop);
            if (!longituderegex.IsMatch(value))
            {
                SetError(prop, ApplicationContext.Instance.StringResourceReader.GetString("wrongformat"));
                isSuccess = false;
            }
            return isSuccess;
        }


        string latitude = string.Empty;

        public string Latitude
        {
            get { return latitude; }
            set
            {
                latitude = value;
                ValidateLongitude("Latitude", latitude);
            }
        }

        private bool ValidateLatitude(string prop, string value)
        {
            bool isSuccess = true;
            ClearErrors(prop);
            if (!latitiuderegex.IsMatch(value))
            {
                SetError(prop, ApplicationContext.Instance.StringResourceReader.GetString("wrongformat"));
                isSuccess = false;
            }
            return isSuccess;
        }
    }
}

