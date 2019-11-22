using Gsafety.PTMS.ServiceReference.EmailService;
using Gsafety.PTMS.Share;
using Jounce.Core.ViewModel;
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

namespace Gsafety.PTMS.Alert.Views
{
    public partial class ChildSendMail : ChildWindow
    {
        Email mail;
        private EmailServiceClient emailclient = ServiceClientFactory.Create<EmailServiceClient>();
        public ChildSendMail(Email email)
        {
            InitializeComponent();
            ToCompany.IsChecked = true;
            ToEdit.IsChecked = false;
            DecValue.IsEnabled = false;
            mail = email;
        }

        Regex regex = new Regex(@"^([\d\w_]+[\d\w_.]*)@([0-9a-zA-Z_])+(.[\d\w_])*", RegexOptions.IgnoreCase);
        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            
            if (ToCompany.IsChecked == false)
            {
                mail.MailToArray = new System.Collections.ObjectModel.ObservableCollection<string> { };
            }
            if (ToEdit.IsChecked == true)
            {
                if (string.IsNullOrEmpty(this.DecValue.Text.Trim()))
                {
                    MessageBox.Show(ApplicationContext.Instance.StringResourceReader.GetString("ValueNotNull"));
                    return;
                }
                string[] sArray = (DecValue.Text.Trim()).Split(new Char[] { ';'});
                foreach (string s in sArray)
                {
                    if (s.Trim() != "")
                    {
                        if (!regex.Match(s.Trim()).Success)
                        {
                            MessageBox.Show(ApplicationContext.Instance.StringResourceReader.GetString("EmailUnright"));
                            return;
                        }
                        mail.MailToArray.Add(s);
                    }
                }
            }
           
            if (ToEdit.IsChecked == true || ToCompany.IsChecked == true)
            {
                if (mail.MailToArray.Count > 0)
                {
                    emailclient.SendEmailAsync(mail);
                }
            }
            this.DialogResult = true;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        private void ToEdit_Checked(object sender, RoutedEventArgs e)
        {
            if (ToEdit.IsChecked == true)
            {
                DecValue.IsEnabled = true;
            }
            else
            {
                DecValue.IsEnabled = false;
            }

            if (ToEdit.IsChecked == false && ToCompany.IsChecked == false)
            {
                OKButton.IsEnabled = false;
            }
            else
            {
                OKButton.IsEnabled = true;
            }
        }

        private void ToCompany_Click(object sender, RoutedEventArgs e)
        {
            if (ToEdit.IsChecked == false && ToCompany.IsChecked == false)
            {
                OKButton.IsEnabled = false;
            }
            else
            {
                OKButton.IsEnabled = true;
            }
        }
    }
}

