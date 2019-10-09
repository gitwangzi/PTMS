using GisManagement;
using GisManagement.Views;
using Gsafety.PTMS.ServiceReference.VehicleAlarmService;
using Gsafety.Ant.Share;
using Jounce.Core.ViewModel;
using System;
using Gsafety.PTMS.Bases.Enums;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using Gsafety.PTMS.Share;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Gsafety.Common.Controls;
using Gsafety.Ant.Monitor.ViewModels;
namespace Gsafety.PTMS.Monitor.Views
{

    public partial class ChildSendMail : ChildWindow
    {


        MonitorEmailEditViewModel viewModel;
 
        private AlarmEmailInfo _mail;
        public AlarmEmailInfo mail
        {
            get
            {
                return _mail;
            }
            set
            {
                _mail = value;
            }
        }

        public ChildSendMail(string title)
        {
            this.viewModel = new MonitorEmailEditViewModel();
            this.DataContext = this.viewModel;
            this.viewModel.Name = string.Empty;
            this.viewModel.Mail = string.Empty;
            this.Title = title;
            InitializeComponent();

        }

        public void Edit(AlarmEmailInfo email)
        {
            mail = email;

            this.viewModel.Name = email.Name;
            this.viewModel.Mail = email.Mail;
            if (mail.Level == (int)IncidentLevelEnum.Common)
            {

                this.Common.IsChecked = true;

            }
            else if (mail.Level == (int)IncidentLevelEnum.Bigger)
            {

                this.Bigger.IsChecked = true;

            }
            else if (mail.Level == (int)IncidentLevelEnum.Major)
            {

                this.Major.IsChecked = true;

            }
            else
            {

                this.EspecialMajor.IsChecked = true;

            }

            Show();
        }    
       

        Regex regex = new Regex(@"^([\d\w_]+[\d\w_.]*)@([0-9a-zA-Z_])+(.[\d\w_])*", RegexOptions.IgnoreCase);
        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(this.TitleValue.Text.Trim()))
                {
                    //MessageBoxHelper.ShowDialog(ApplicationContext.Instance.StringResourceReader.GetString("Tip"), ApplicationContext.Instance.StringResourceReader.GetString("ALERT_SendTitleNotNull"), MessageDialogButton.Ok);

                    return;
                }
                mail.Name = this.TitleValue.Text.Trim();

                if (string.IsNullOrEmpty(this.DecValue.Text.Trim()))
                {
                   // MessageBoxHelper.ShowDialog(ApplicationContext.Instance.StringResourceReader.GetString("Tip"), ApplicationContext.Instance.StringResourceReader.GetString("RecipientsValueNotNull"), MessageDialogButton.Ok);


                    return;
                }

                if (DecValue.Text.Trim() != "")
                {
                    if (!regex.Match(DecValue.Text.Trim()).Success)
                    {
                       // MessageBoxHelper.ShowDialog(ApplicationContext.Instance.StringResourceReader.GetString("Tip"), ApplicationContext.Instance.StringResourceReader.GetString("EmailUnright"), MessageDialogButton.Ok);


                        return;
                    }
                    mail.Mail = this.DecValue.Text.Trim();
                }

                if (this.Common.IsChecked==true)
                {
                    mail.Level = (int)IncidentLevelEnum.Common;
                }
                else if (this.Bigger.IsChecked == true)
                {
                    mail.Level = (int)IncidentLevelEnum.Bigger;
                }
                else if (this.Major.IsChecked == true)
                {
                    mail.Level = (int)IncidentLevelEnum.Major;
                }
                else
                {
                    mail.Level = (int)IncidentLevelEnum.EspcialMajor;
                }
              

                    
                this.DialogResult = true;
            }
            catch(Exception ex)
            {
              this.DialogResult = false;
            }
        }

      

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

       

        private void SendPicture_Click(object sender, RoutedEventArgs e)
        {

        }

        private void TitleValue_TextChanged_1(object sender, TextChangedEventArgs e)
        {
            this.viewModel.Name = this.TitleValue.Text;
          
        }

        private void DecValue_TextChanged_1(object sender, TextChangedEventArgs e)
        {
            this.viewModel.Mail = this.DecValue.Text;
          
        }
    }
}

