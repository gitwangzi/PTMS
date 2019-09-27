using Gsafety.Common.CommMessage;
using Gsafety.PTMS.ServiceReference.VedioService;
using Gsafety.PTMS.Share;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace Gsafety.PTMS.BasicPage.Views
{
    public partial class EditNoteWindow : ChildWindow
    {
        private QueryServerFileListMessage _model;

        public EditNoteWindow(QueryServerFileListMessage model)
        {
            InitializeComponent();
            _model = model;

            if (model.Note != null)
            {
                txtNote.Text = model.Note;
            }
            else
            {
                txtNote.Text = string.Empty;
            }
        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            var client = ServiceClientFactory.Create<VedioServiceClient>();
            client.UpdateVideoNoteCompleted += client_UpdateVideoNoteCompleted;
            client.UpdateVideoNoteAsync(_model.UUID, txtNote.Text.Trim());
        }

        void client_UpdateVideoNoteCompleted(object sender, UpdateVideoNoteCompletedEventArgs e)
        {
            try
            {
                SaveResultArgs args = new SaveResultArgs();

                if (e.Cancelled)
                {
                    return;
                }
                if (e.Error != null)
                {
                    args.Result = false;
                    args.Message = ApplicationContext.Instance.StringResourceReader.GetString(LProxy.ServerError);
                    ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(), e.Error);
                }
                if (e.Result.IsSuccess == false)
                {
                    if (!string.IsNullOrEmpty(e.Result.ErrorMsg))
                    {
                        args.Result = false;
                        args.Message = ApplicationContext.Instance.StringResourceReader.GetString(e.Result.ErrorMsg.Trim());
                        ApplicationContext.Instance.Logger.LogError(MethodBase.GetCurrentMethod().ToString(), args.Message);
                    }
                    else
                    {
                        args.Result = false;
                        args.Message = ApplicationContext.Instance.StringResourceReader.GetString(LProxy.OperatorServiceError);
                        ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(), e.Result.ExceptionMessage);
                    }
                }
                else
                {
                    if (e.Result.Result)
                    {
                        _model.Note = txtNote.Text.Trim();
                        this.DialogResult = true;
                    }
                }
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException("DriverInfoDetailViewModel.client_AddDriverInfoCompleted", ex);
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }
    }
}

