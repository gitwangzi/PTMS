using Gsafety.Common.Controls;
using Gsafety.Common.Converts;
using Gsafety.PTMS.ServiceReference.VehicleAlarmService;
using Gsafety.PTMS.Share;
using System;
using System.Reflection;
using System.Windows;

namespace HistoryQueryManagement.ViewModels
{
    public partial class HistoryQueryMainPageViewModel
    {
        const string _strike = "-";
        private DisplayLonConvert _lonConverter = new DisplayLonConvert();
        private DisplayLatConvert _latConverter = new DisplayLatConvert();

        // VehicleAlarmServiceClient vehicleAlarmServiceClient = null;

        #region property.....
        private string _alarmLatitude;
        public string AlarmLatitude
        {
            get { return _alarmLatitude; }
            set { _alarmLatitude = value; }
        }

        private string _AlarmGPSValid;
        /// <summary>
        /// 
        /// </summary>
        public string AlarmGPSValid
        {
            get { return _AlarmGPSValid; }
            set { _AlarmGPSValid = value; }
        }

        private string _Longitude;
        /// <summary>
        /// 
        /// </summary>
        public string AlarmLongitude
        {
            get { return _Longitude; }
            set { _Longitude = value; }
        }

        /// <summary>
        /// 安全套件编号
        /// </summary>
        private string _alarmsuiteid;
        public string AlarmSuiteID
        {
            get { return _alarmsuiteid; }
            set { _alarmsuiteid = value; }
        }


        private Visibility _appealVisible = Visibility.Collapsed;
        public Visibility AppealVisible
        {
            get { return _appealVisible; }
            set { _appealVisible = value; }
        }

        private Visibility _diposeVisible = Visibility.Collapsed;
        public Visibility DisposeVisible
        {
            get { return _diposeVisible; }
            set { _diposeVisible = value; }
        }


        private string _AlarmTime;
        public string AlarmTime
        {
            get { return _AlarmTime; }
            set { _AlarmTime = value; }
        }

        private string _INCIDENT_ID;
        /// <summary>
        /// Alarm ID
        /// </summary>
        public string INCIDENT_ID
        {
            get { return _INCIDENT_ID; }
            set { _INCIDENT_ID = value; }
        }

        private string _FORWARDED_FLAG = _strike;
        /// <summary>
        /// 
        /// </summary>
        public string FORWARDED_FLAG
        {
            get { return _FORWARDED_FLAG; }
            set
            {
                _FORWARDED_FLAG = value;
                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => FORWARDED_FLAG));
            }
        }

        private string _FORWARD_TIME;
        public string FORWARD_TIME
        {
            get { return _FORWARD_TIME; }
            set { _FORWARD_TIME = value; Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => FORWARD_TIME)); }
        }

        private string alarmsource;

        public string AlarmSource
        {
            get { return alarmsource; }
            set
            {
                alarmsource = value;
                RaisePropertyChanged("AlarmSource");
            }
        }

        private string _alarmMobile;

        public string AlarmMobile
        {
            get { return _alarmMobile; }
            set
            {
                _alarmMobile = value;
                RaisePropertyChanged("AlarmMobile");
            }
        }

        private string _keyword;

        public string Keyword
        {
            get { return _keyword; }
            set
            {
                _keyword = value;
                RaisePropertyChanged("Keyword");
            }
        }

        private string _alarmcontent;

        public string AlarmContent
        {
            get { return _alarmcontent; }
            set
            {
                _alarmcontent = value;
                RaisePropertyChanged("AlarmContent");
            }
        }

        private string _apealalarmflag;
        public string ApealAlarmFlag
        {
            get
            {
                return _apealalarmflag;
            }
            set
            {
                _apealalarmflag = value;
                RaisePropertyChanged("ApealAlarmFlag");
            }
        }

        private string _apealcontent;
        public string ApealContent
        {
            get
            {
                return _apealcontent;
            }
            set
            {
                _apealcontent = value;
                RaisePropertyChanged("ApealContent");
            }
        }

        private string _apealdisposestaff;
        public string ApealDisposeStaff
        {
            get
            {
                return _apealdisposestaff;
            }
            set
            {
                _apealdisposestaff = value;
                RaisePropertyChanged("ApealDisposeStaff");
            }
        }

        private string _apealdisposetime;
        public string ApealDisposeTime
        {
            get
            {
                return _apealdisposetime;
            }
            set
            {
                _apealdisposetime = value;
                RaisePropertyChanged("ApealDisposeTime");
            }
        }

        private string _transferalarmflag;
        public string TransferAlarmFlag
        {
            get
            {
                return _transferalarmflag;
            }
            set
            {
                _transferalarmflag = value;
                RaisePropertyChanged("TransferAlarmFlag");
            }
        }

        private string _transferdisposetime;
        public string TransferDisposeTime
        {
            get
            {
                return _transferdisposetime;
            }
            set
            {
                _transferdisposetime = value;
                RaisePropertyChanged("TransferDisposeTime");
            }
        }

        private string _transferdisposestaff;
        public string TransferDisposeStaff
        {
            get
            {
                return _transferdisposestaff;
            }
            set
            {
                _transferdisposestaff = value;
                RaisePropertyChanged("TransferDisposeStaff");
            }
        }

        private string _transfertransfercenter;
        public string TransferTransferCenter
        {
            get
            {
                return _transfertransfercenter;
            }
            set
            {
                _transfertransfercenter = value;
                RaisePropertyChanged("TransferTransferCenter");
            }
        }

        private string _transferalarmaddress;
        public string TransferAlarmAddress
        {
            get
            {
                return _transferalarmaddress;
            }
            set
            {
                _transferalarmaddress = value;
                RaisePropertyChanged("TransferAlarmAddress");
            }
        }

        private string _transferincidenttype;
        public string TransferIncidentType
        {
            get
            {
                return _transferincidenttype;
            }
            set
            {
                _transferincidenttype = value;
                RaisePropertyChanged("TransferIncidentType");
            }
        }

        private string _transferdisposecontent;
        public string TransferDisposeContent
        {
            get
            {
                return _transferdisposecontent;
            }
            set
            {
                _transferdisposecontent = value;
                RaisePropertyChanged("TransferDisposeContent");
            }
        }

        private string _disposeStatus;
        /// <summary>
        /// 反馈处置状态
        /// </summary>
        public string DisposeStatus
        {
            get
            {
                return this._disposeStatus;
            }
            set
            {
                this._disposeStatus = value;
                RaisePropertyChanged(() => DisposeStatus);
            }
        }

        private string _appealStatus;
        /// <summary>
        /// 警情处置状态
        /// </summary>

        public string ApealStatus
        {
            get
            {
                return this._appealStatus;
            }
            set
            {
                this._appealStatus = value;
                RaisePropertyChanged(() => ApealStatus);
            }
        }

        private Visibility _isAlarmNoteVisibility;
        /// <summary>
        /// 报警的处置信息是否显示
        /// </summary>
        public Visibility IsAlarmNoteVisibility
        {
            get
            {
                return this._isAlarmNoteVisibility;
            }
            set
            {
                this._isAlarmNoteVisibility = value;
                RaisePropertyChanged(() => IsAlarmNoteVisibility);
            }
        }

        private Visibility _isAlarmAnswerNoteVisibility;
        /// <summary>
        /// 报警反馈信息是否显示
        /// </summary>
        public Visibility IsAlarmAnswerNoteVisibility
        {
            get
            {
                return this._isAlarmAnswerNoteVisibility;
            }
            set
            {
                this._isAlarmAnswerNoteVisibility = value;
                RaisePropertyChanged(() => IsAlarmAnswerNoteVisibility);
            }
        }

        #endregion

        /// <summary>
        /// Constructors
        /// </summary>
        public void InitialAlarmDetail()
        {
            try
            {
                VehicleAlarmServiceClient vehicleAlarmServiceClient = InitialAlarmClient();
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException("AlarmInfoVm()", ex);
            }

        }

        void vehicleAlarmServiceClient_GetApealDisposeByAlarmIDCompleted(object sender, GetApealDisposeByAlarmIDCompletedEventArgs e)
        {
            try
            {
                if (e.Error == null)
                {
                    if (e.Result.IsSuccess)
                    {
                        ApealDisposeTime = e.Result.Result.DisposeTime.ToLocalTime().ToString();
                        ApealDisposeStaff = e.Result.Result.DisposeStaff;
                        ApealAlarmFlag = e.Result.Result.AlarmFlag.ToString();
                        ApealContent = e.Result.Result.Content;
                        if (!string.IsNullOrEmpty(ApealContent))
                        {
                            this.IsAlarmNoteVisibility = Visibility.Visible;
                        }
                        else
                        {
                            this.IsAlarmNoteVisibility = Visibility.Collapsed;
                        }
                    }
                    else
                    {
                        if (string.IsNullOrEmpty(e.Result.ErrorMsg))
                        {
                            MessageBoxHelper.ShowDialog(ApplicationContext.Instance.StringResourceReader.GetString(LProxy.Caption), ApplicationContext.Instance.StringResourceReader.GetString(LProxy.OperatorServiceError));
                        }
                        else
                        {
                            MessageBoxHelper.ShowDialog(ApplicationContext.Instance.StringResourceReader.GetString(LProxy.Caption), ApplicationContext.Instance.StringResourceReader.GetString(e.Result.ErrorMsg));
                        }
                    }
                }
                else
                {
                    MessageBoxHelper.ShowDialog(ApplicationContext.Instance.StringResourceReader.GetString(LProxy.Caption), ApplicationContext.Instance.StringResourceReader.GetString(LProxy.ServerError));
                }
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException("vehicleAlarmServiceClient_GetTransferDisposeByAlarmIDCompleted", ex);
            }
            finally
            {
                CloseClient(sender);
            }
        }

        void vehicleAlarmServiceClient_GetTransferDisposeByAlarmID_CADCompleted(object sender, GetTransferDisposeByAlarmID_CADCompletedEventArgs e)
        {
            try
            {
                if (e.Error == null)
                {
                    if (e.Result.IsSuccess)
                    {
                        this.DisposeStatus = e.Result.Result.ToString() ;                       
                       
                    }
                    else
                    {
                        if (string.IsNullOrEmpty(e.Result.ErrorMsg))
                        {
                            MessageBoxHelper.ShowDialog(ApplicationContext.Instance.StringResourceReader.GetString(LProxy.Caption), ApplicationContext.Instance.StringResourceReader.GetString(LProxy.OperatorServiceError));
                        }
                        else
                        {
                            MessageBoxHelper.ShowDialog(ApplicationContext.Instance.StringResourceReader.GetString(LProxy.Caption), ApplicationContext.Instance.StringResourceReader.GetString(e.Result.ErrorMsg));
                        }
                    }
                }

                else
                {
                    MessageBoxHelper.ShowDialog(ApplicationContext.Instance.StringResourceReader.GetString(LProxy.Caption), ApplicationContext.Instance.StringResourceReader.GetString(LProxy.ServerError));
                }
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException("vehicleAlarmServiceClient_GetTransferDisposeByAlarmID_CADCompleted", ex);

            }
            finally
            {
                CloseClient(sender);
            }
        }

        void vehicleAlarmServiceClient_GetTransferDisposeByAlarmIDCompleted(object sender, GetTransferDisposeByAlarmIDCompletedEventArgs e)
        {
            try
            {
                if (e.Error == null)
                {
                    if (e.Result.IsSuccess)
                    {
                        TransferTransferCenter = e.Result.Result.TransferCenter;
                        TransferAlarmAddress = e.Result.Result.AlarmAddress;
                        TransferAlarmFlag = e.Result.Result.AlarmFlag.ToString();
                        TransferDisposeStaff = e.Result.Result.DisposeStaff;
                        if (e.Result.Result.DisposeTime.HasValue)
                            TransferDisposeTime = e.Result.Result.DisposeTime.Value.ToLocalTime().ToString();
                        TransferIncidentType = e.Result.Result.IncidentType;
                        TransferDisposeContent = e.Result.Result.DisposeContent;
                        if (!string.IsNullOrEmpty(TransferDisposeContent))
                        {
                            this.IsAlarmAnswerNoteVisibility = Visibility.Visible;
                        }
                        else
                        {
                            this.IsAlarmAnswerNoteVisibility = Visibility.Collapsed;
                        }
                    }
                    else
                    {
                        if (string.IsNullOrEmpty(e.Result.ErrorMsg))
                        {
                            MessageBoxHelper.ShowDialog(ApplicationContext.Instance.StringResourceReader.GetString(LProxy.Caption), ApplicationContext.Instance.StringResourceReader.GetString(LProxy.OperatorServiceError));
                        }
                        else
                        {
                            MessageBoxHelper.ShowDialog(ApplicationContext.Instance.StringResourceReader.GetString(LProxy.Caption), ApplicationContext.Instance.StringResourceReader.GetString(e.Result.ErrorMsg));
                        }
                    }
                }

                else
                {
                    MessageBoxHelper.ShowDialog(ApplicationContext.Instance.StringResourceReader.GetString(LProxy.Caption), ApplicationContext.Instance.StringResourceReader.GetString(LProxy.ServerError));
                }
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException("vehicleAlarmServiceClient_GetTransferDisposeByAlarmIDCompleted", ex);

            }
            finally
            {
                CloseClient(sender);
            }
        }

        private static void CloseClient(object sender)
        {
            VehicleAlarmServiceClient client = sender as VehicleAlarmServiceClient;
            if (client != null)
            {
                client.CloseAsync();
                client = null;
            }
        }

        /// For more information on alarms
        /// </summary>
        /// <param name="alarmInfo"></param>
        public void OnAlarmSelectionChange(AlarmInfoEx alarmInfo)
        {
            try
            {
                if (alarmInfo.AppealStatus != 0)
                {
                    AppealVisible = Visibility.Visible;
                    //默认已处置
                    this.ApealStatus = ApplicationContext.Instance.StringResourceReader.GetString("Disposed");
                }
                else
                {
                    //处置状态为未处置
                    AppealVisible = Visibility.Collapsed;
                    this.ApealStatus = ApplicationContext.Instance.StringResourceReader.GetString("NoDisposed");
                }

                if (alarmInfo.TransferStatus != 0)
                {
                    DisposeVisible = Visibility.Visible;
                }
                else
                {
                    DisposeVisible = Visibility.Collapsed;
                }

                if (string.IsNullOrEmpty(alarmInfo.SuiteID))
                {
                    var client = ServiceClientFactory.Create<Gsafety.PTMS.ServiceReference.BscDevSuiteService.BscDevSuiteServiceClient>();
                    client.GetBscDevSuiteIDByVehicleSNCompleted += client_GetBscDevSuiteIDByVehicleSNCompleted;
                    client.GetBscDevSuiteIDByVehicleSNAsync(alarmInfo.VehicleId);
                }

                AlarmSuiteID = alarmInfo.AlarmGuid;
                if (alarmInfo.GpsValid != "A")
                {
                    AlarmLongitude = _strike;
                    AlarmLatitude = _strike;
                }
                else if (alarmInfo.GpsValid == "A")
                {
                    AlarmLongitude = alarmInfo.Longitude;
                    AlarmLatitude = alarmInfo.Latitude;

                    var tempLat = _latConverter.ConvertBack(AlarmLatitude, null, null, null);
                    var resultLat = _latConverter.ConvertToWESN(tempLat, null, null, null);

                    var tempLon = _lonConverter.ConvertBack(AlarmLongitude, null, null, null);
                    var resultLon = _lonConverter.ConvertToWESN(tempLon, null, null, null);

                    AlarmLongitude = resultLon as string;
                    AlarmLatitude = resultLat as string;
                }
                this.Locate = AlarmLatitude + " " + AlarmLongitude;

                AlarmTime = alarmInfo.AlarmTime.Value.ToString();
                AlarmSource = alarmInfo.Source.ToString();
                AlarmMobile = alarmInfo.AlarmMobile;
                Keyword = alarmInfo.Keyword;
                AlarmContent = alarmInfo.AlarmContent;
                this.DisposeStatus = alarmInfo.DisposalStatus.ToString();
                if (alarmInfo.AppealStatus != 0)
                {
                    VehicleAlarmServiceClient vehicleAlarmServiceClient = InitialAlarmClient();
                    vehicleAlarmServiceClient.GetApealDisposeByAlarmIDAsync(alarmInfo.ID);
                }
                if (string.IsNullOrEmpty(alarmInfo.Speed) || alarmInfo.Speed == "0.0")
                {
                    alarmInfo.Speed = "-.-";
                }
                if (string.IsNullOrEmpty(alarmInfo.Direction) || alarmInfo.Direction == "0")
                {
                    alarmInfo.Direction = "-";
                }
                if (alarmInfo.TransferStatus != 0)
                {
                    VehicleAlarmServiceClient vehicleAlarmServiceClient = InitialAlarmClient();
                   // vehicleAlarmServiceClient.GetTransferDisposeByAlarmIDAsync(alarmInfo.ID);
                     vehicleAlarmServiceClient.GetTransferDisposeByAlarmID_CADAsync(alarmInfo.ID);
                }

                if (this.client == null)
                {
                    this.InilitClient();
                }
                this.client.GetChauffeurByVehicleAsync(alarmInfo.VehicleId, ApplicationContext.Instance.AuthenticationInfo.ClientID);
                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => AlarmLatitude));
                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => AlarmLongitude));
                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => AlarmTime));
                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => AppealVisible));
                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => AlarmSource));

                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => DisposeVisible));
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(), ex);
            }
        }

        void client_GetBscDevSuiteIDByVehicleSNCompleted(object sender, Gsafety.PTMS.ServiceReference.BscDevSuiteService.GetBscDevSuiteIDByVehicleSNCompletedEventArgs e)
        {
            if (e.Cancelled)
            {
                return;
            }

            if (e.Error != null)
            {
                ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(), e.Error);
                return;
            }

            try
            {
                if (e.Result != null && e.Result.IsSuccess)
                {
                    SelectedAlarmInfo.SuiteID = e.Result.Result;
                }
            }
            catch (System.Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(), ex);
            }
            finally
            {
                var client = sender as Gsafety.PTMS.ServiceReference.BscDevSuiteService.BscDevSuiteServiceClient;
                if (client != null)
                {
                    client.CloseAsync();
                }
            }
        }
    }
}
