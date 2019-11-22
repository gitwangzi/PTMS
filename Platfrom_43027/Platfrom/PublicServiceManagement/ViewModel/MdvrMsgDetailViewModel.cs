using BaseLib.Model;
using BaseLib.ViewModels;
using Gsafety.Common.CommMessage;
using Gsafety.PTMS.Bases.Enums;
using Gsafety.PTMS.Bases.Models;
using Gsafety.PTMS.Enums;
using Gsafety.PTMS.ServiceReference.PublicService;
using Gsafety.PTMS.Share;
using Jounce.Core.View;
using Jounce.Framework;
using Jounce.Framework.Command;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

namespace PublicServiceManagement.ViewModel
{
    public class MdvrMsgDetailViewModel : DetailViewModel<RunMdvrMessage>
    {
        public event EventHandler<SaveResultArgs> OnSaveResult;

        //public RunMdvrMessageClient client = null;

        private RunMdvrMessageClient InitServiceClient()
        {
            RunMdvrMessageClient client = ServiceClientFactory.Create<RunMdvrMessageClient>();
            client.InsertRunMdvrMessageCompleted += client_InsertRunMdvrMessageCompleted;
            client.UpdateRunMdvrMessageCompleted += client_UpdateRunMdvrMessageCompleted;
            return client;
        }

        void client_UpdateRunMdvrMessageCompleted(object sender, UpdateRunMdvrMessageCompletedEventArgs e)
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
                ApplicationContext.Instance.Logger.LogException("DriverInfoDetailViewModel.client_AddDriverInfoCompleted", ex);
            }
            finally
            {
                CloseClient(sender);
            }
        }

        void client_InsertRunMdvrMessageCompleted(object sender, InsertRunMdvrMessageCompletedEventArgs e)
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
                ApplicationContext.Instance.Logger.LogException("DriverInfoDetailViewModel.client_AddDriverInfoCompleted", ex);
            }
            finally
            {
                CloseClient(sender);
            }
        }

        private static void CloseClient(object sender)
        {
            RunMdvrMessageClient client = sender as RunMdvrMessageClient;
            if (client != null)
            {
                client.CloseAsync();
                client = null;
            }

        }



        private List<ComboBoxBasicStruct<EnumModel>> _msgTypes = new List<ComboBoxBasicStruct<EnumModel>>();
        /// <summary>
        /// 
        /// </summary>
        public List<ComboBoxBasicStruct<EnumModel>> MsgTypes
        {
            get { return _msgTypes; }
            set
            {
                _msgTypes = value;
                RaisePropertyChanged(() => MsgTypes);
            }
        }

        private ComboBoxBasicStruct<EnumModel> msgTypeSelected = new ComboBoxBasicStruct<EnumModel>();
        /// <summary>
        /// 
        /// </summary>
        public ComboBoxBasicStruct<EnumModel> MsgTypeSelected
        {
            get { return msgTypeSelected; }
            set
            {
                msgTypeSelected = value;
                RaisePropertyChanged(() => MsgTypeSelected);
            }
        }


        protected string action;
        public MdvrMsgDetailViewModel()
        {
            var adapter = new EnumAdapter<MdvrMsgTypeEnum>();
            var categorys = adapter.GetEnumInfos();

            MsgTypes.Add(new ComboBoxBasicStruct<EnumModel>()
            {
                Key = new EnumModel() { EnumValue = 0},
                Value = ApplicationContext.Instance.StringResourceReader.GetString("TrafficMessage"),
            });

            MsgTypes.Add(new ComboBoxBasicStruct<EnumModel>()
            {
                Key = new EnumModel() { EnumValue = 1 },
                Value = ApplicationContext.Instance.StringResourceReader.GetString("WeatherMessage"),
            });
            MsgTypes.Add(new ComboBoxBasicStruct<EnumModel>()
            {
                Key = new EnumModel() { EnumValue = 2 },
                Value = ApplicationContext.Instance.StringResourceReader.GetString("TimeMessage"),
            });
            
          
        }

        public new void ActivateView(string viewName, System.Collections.Generic.IDictionary<string, object> viewParameters)
        {
            //base.ActivateView(viewName, viewParameters);
            try
            {
                action = viewParameters["action"].ToString();
                switch (action)
                {
                    case "view":
                        Title = ApplicationContext.Instance.StringResourceReader.GetString("Common_View");
                        IsReadOnly = true;
                        JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => IsReadOnly));
                        ViewVisibility = Visibility.Collapsed;
                        InitialModel = viewParameters["view"] as RunMdvrMessage;
                        InitialFromInitialModel();
                        break;
                    case "update":
                        Title = ApplicationContext.Instance.StringResourceReader.GetString("Common_Update");
                        IsReadOnly = false;
                        JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => IsReadOnly));
                        ViewVisibility = Visibility.Visible;
                        InitialModel = viewParameters["view"] as RunMdvrMessage;
                        InitialFromInitialModel();
                        CurrentModel = new RunMdvrMessage();
                        break;
                    case "add":
                        Title = ApplicationContext.Instance.StringResourceReader.GetString("Common_Add");
                        IsReadOnly = false;
                        JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => IsReadOnly));
                        ViewVisibility = Visibility.Visible;
                        CurrentModel = new RunMdvrMessage();
                        Reset();
                        break;
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(), ex);
            }
        }
        protected override void Reset()
        {
            try
            {
                if (action == "update")
                {
                    InitialFromInitialModel();
                }
                else
                {
                    MessageTitle = string.Empty;
                    Content = string.Empty;
                    MsgTypeSelected = MsgTypes.FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(), ex);
            }
        }
        public void InitialFromInitialModel()
        {
            Content = InitialModel.Content;
            //Duration = InitialModel.Duration.ToString();
            MessageTitle = InitialModel.MessageTitle;
            MsgTypeSelected = MsgTypes.FirstOrDefault(n => n.Key.EnumValue == (int)InitialModel.MessageType);
        }
        protected override void ValidateAll()
        {
            ValidateContent(ExtractPropertyName(() => Content), _content);
           // ValidateDuration(ExtractPropertyName(() => Duration), _duration);
          
        }
        protected override void OnCommitted()
        {
            try
            {
                CurrentModel.ClientId = ApplicationContext.Instance.AuthenticationInfo.ClientID;
                CurrentModel.Content = Content;

                CurrentModel.MessageType = Convert.ToInt16(MsgTypeSelected.Key.EnumValue);
                CurrentModel.CreateTime = DateTime.Now.ToUniversalTime();
                CurrentModel.MessageTitle = MessageTitle;

                if (action.Equals("update"))
                {
                    CurrentModel.ID = InitialModel.ID;
                    Update();
                }
                else
                {
                    Add();
                }
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(), ex);
            }
        }
        protected override void Return()
        {

        }
        protected void Add()
        {
            RunMdvrMessageClient client= InitServiceClient();
            client.InsertRunMdvrMessageAsync(CurrentModel);
        }
        protected void Update()
        {
            RunMdvrMessageClient client = InitServiceClient();
            client.UpdateRunMdvrMessageAsync(CurrentModel);
        }



        private string _content;
        public string Content
        {
            get { return _content; }
            set
            {
                _content = value == null ? null : value.Trim();
                ValidateContent(ExtractPropertyName(() => Content), _content);
                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => Content));
            }
        }
        private void ValidateContent(string prop, string value)
        {
            ClearErrors(prop);
            if (string.IsNullOrEmpty(Content))
            {
                base.SetError(prop, ApplicationContext.Instance.StringResourceReader.GetString(LProxy.NotNull));
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


    }
}
