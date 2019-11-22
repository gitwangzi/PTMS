using BaseLib.Model;
using BaseLib.ViewModels;
using Gsafety.PTMS.Bases.Models;
using Gsafety.PTMS.ServiceReference.PublicService;
using Gsafety.PTMS.Share;
using System;
using System.Collections.Generic;
using System.Net;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Linq;
using Gsafety.Common.Controls;
using Gsafety.Common.CommMessage;
using System.Collections.ObjectModel;

namespace Gsafety.PTMS.Monitor.ViewModels
{
    public class SendPhoneMsgViewModel : DetailViewModel<RunAppMessage>
    {
        public event EventHandler<SaveResultArgs> OnSaveResult;
        public string vehicleId = string.Empty;
        private string msgId = Guid.NewGuid().ToString();
        public SendPhoneMsgViewModel(string sendSehicleId)
        {
            CurrentModel = new RunAppMessage();
            vehicleId = sendSehicleId;
            ZMessageType.Add(new ComboBoxBasicStruct<EnumModel>()
            {
                Key = new EnumModel() { EnumValue = 1 },
                Value = ApplicationContext.Instance.StringResourceReader.GetString("WeatherMessage"),
            });
            ZMessageType.Add(new ComboBoxBasicStruct<EnumModel>()
            {
                Key = new EnumModel() { EnumValue = 2 },
                Value = ApplicationContext.Instance.StringResourceReader.GetString("TrafficMessage"),
            });
            ZMessageType.Add(new ComboBoxBasicStruct<EnumModel>()
            {
                Key = new EnumModel() { EnumValue = 3 },
                Value = ApplicationContext.Instance.StringResourceReader.GetString("MaintainMessage"),
            });
            ZMessageType.Add(new ComboBoxBasicStruct<EnumModel>()
            {
                Key = new EnumModel() { EnumValue = 4 },
                Value = ApplicationContext.Instance.StringResourceReader.GetString("Security"),
            });
            ZMessageType.Add(new ComboBoxBasicStruct<EnumModel>()
            {
                Key = new EnumModel() { EnumValue = 5 },
                Value = ApplicationContext.Instance.StringResourceReader.GetString("Notice"),
            });

            VMessageType = ZMessageType.FirstOrDefault();
        }

        private List<ComboBoxBasicStruct<EnumModel>> _messageType = new List<ComboBoxBasicStruct<EnumModel>>();
        public List<ComboBoxBasicStruct<EnumModel>> ZMessageType
        {
            get { return _messageType; }
            set
            {
                _messageType = value;
                RaisePropertyChanged(() => ZMessageType);
            }
        }
        private ComboBoxBasicStruct<EnumModel> vMessageType = new ComboBoxBasicStruct<EnumModel>();
        public ComboBoxBasicStruct<EnumModel> VMessageType
        {
            get { return vMessageType; }
            set
            {
                vMessageType = value;
                RaisePropertyChanged(() => VMessageType);
            }
        }
        private string _messageTitle;
        public string MessageTitle
        {
            get { return _messageTitle; }
            set
            {
                _messageTitle = value == null ? null : value.Trim();
                ValidateMessageTitle(ExtractPropertyName(() => MessageTitle), _messageTitle);
                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => MessageTitle));
            }
        }
        private void ValidateMessageTitle(string prop, string value)
        {
            ClearErrors(prop);
            if (string.IsNullOrEmpty(MessageTitle))
            {
                base.SetError(prop, ApplicationContext.Instance.StringResourceReader.GetString(LProxy.NotNull));
            }
        }
        private string _message;
        public string Message
        {
            get { return _message; }
            set
            {
                _message = value == null ? null : value.Trim();
                ValidateMessage(ExtractPropertyName(() => Message), _message);
                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => Message));
            }
        }
        private void ValidateMessage(string prop, string value)
        {
            ClearErrors(prop);
            if (string.IsNullOrEmpty(Message))
            {
                base.SetError(prop, ApplicationContext.Instance.StringResourceReader.GetString(LProxy.NotNull));
            }
        }

        protected override void ValidateAll()
        {
            ValidateMessage(ExtractPropertyName(() => Message), _message);
            ValidateMessageTitle(ExtractPropertyName(() => MessageTitle), _messageTitle);
        }

        protected override void OnCommitted()
        {
            try
            {
                CurrentModel.ClientId = ApplicationContext.Instance.AuthenticationInfo.ClientID;
                CurrentModel.Message = Message;
                CurrentModel.MessageTitle = MessageTitle;
                CurrentModel.Creator = ApplicationContext.Instance.AuthenticationInfo.UserID;
                //CurrentModel.VehicleId = VehicleId;
                CurrentModel.MessageType = Convert.ToInt16(VMessageType.Key.EnumValue);
                CurrentModel.CreateTime = DateTime.Now.ToUniversalTime();
                CurrentModel.ID = msgId;

                RunAppMessageClient client = InitServiceClient();
                client.SendRunAppMessageAsync(CurrentModel, vehicleId);
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(), ex);
            }
        }

        protected override void Reset()
        {
            MessageTitle = string.Empty;
            Message = string.Empty;
            VMessageType = ZMessageType.FirstOrDefault();
        }

        private RunAppMessageClient InitServiceClient()
        {
            RunAppMessageClient client = ServiceClientFactory.Create<RunAppMessageClient>();
            client.SendRunAppMessageCompleted += client_SendRunAppMessageCompleted;
            return client;
        }
        private void CloseClient(RunAppMessageClient client)
        {
            if (client != null)
            {
                client.CloseAsync();
                client = null;
            }
        }

        void client_SendRunAppMessageCompleted(object sender, SendRunAppMessageCompletedEventArgs e)
        {
            try
            {
                if (e.Error != null)
                {
                    ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(), e.Error);
                    MessageBoxHelper.ShowDialog(ApplicationContext.Instance.StringResourceReader.GetString(LProxy.ServerError));
                }
                else
                {
                    if (e.Result.IsSuccess == false)
                    {
                        if (!string.IsNullOrEmpty(e.Result.ErrorMsg))
                        {
                            ApplicationContext.Instance.Logger.LogError(MethodBase.GetCurrentMethod().ToString(),
                                e.Result.ErrorMsg);
                            MessageBoxHelper.ShowDialog(ApplicationContext.Instance.StringResourceReader.GetString(e.Result.ErrorMsg));
                        }
                        else
                        {
                            MessageBoxHelper.ShowDialog(ApplicationContext.Instance.StringResourceReader.GetString(LProxy.OperatorServiceError));
                            ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(),
                                e.Result.ExceptionMessage);
                        }
                    }
                    else
                    {
                        SaveResultArgs args = new SaveResultArgs();
                        args.Result = true;

                        if (OnSaveResult != null)
                        {
                            OnSaveResult(this, args);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException("_client_DeliverSpeedLimitToVehicle", ex);
            }
            finally
            {
                RunAppMessageClient client = sender as RunAppMessageClient;
                CloseClient(client);
            }
        }
    }
}
