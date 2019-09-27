/////Copyright (C) Gsafety 2014 .All Rights Reserved.
/////======================================================================
/////                   Guid: 1b9704e8-3c9f-4627-9228-afab05e076bb      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: PC-ZHOUDD
/////                 Author: GJSY(zhoudd)
/////======================================================================
/////           Project Name: Gsafety.PTMS.Manager.ViewModels.CommandManageViewModel
/////    Project Description:    
/////             Class Name: GpsSettingViewModel
/////          Class Version: v1.0.0.0
/////            Create Time: 2014/10/22 11:33:11
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2014/10/22 11:33:11
/////            Modified by:
/////   Modified Description: 
/////======================================================================
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
using Jounce.Core.Command;
using Jounce.Core.ViewModel;
using Jounce.Framework.Command;
using Jounce.Framework.ViewModel;
using Jounce.Core.View;
using System.Collections.Generic;
using Gsafety.PTMS.Share;
using Gsafety.PTMS.ServiceReference.CommandManageService;
using Gsafety.PTMS.Bases.Models;
using Gsafety.PTMS.Bases.Enums;
using System.Collections.ObjectModel;
using System.Linq;
using Gsafety.PTMS.Manager.Views.CommandManageView;
using Gsafety.PTMS.ServiceReference.MessageService;
using System.Reflection;
using BaseLib.ViewModels;
using BaseLib.Model;





namespace Gsafety.PTMS.Manager.ViewModels.CommandManageViewModel
{
    [ExportAsViewModel(ManagerName.GpsSettingViewModel)]
    public class GpsSettingViewModel : BaseViewModel
    {
        private CommandManageServiceClient gpsSettingClient;
        #region properties and command
        private int currentIndex = 1;
        public string RuleName { get; set; }
        public short? UploadType { get; set; }
        public List<int> PageSizeList { get; set; }
        public IActionCommand AddCommand { get; private set; }
        public IActionCommand QueryCommand{get;private set;}
        public IActionCommand DeleteCommand { get; private set; }
        public IActionCommand RepairCommand { get; private set; }
        public IActionCommand DetailCommand { get; private set; }
        public IActionCommand DefaultCommand { get; private set; }
        public IActionCommand ToVehicleCommand { get; private set; }
        public PagedServerCollection<GpsSettingInfo> Gps_InfoList{get;set;}
        public List<EnumModel> UploadTypeList { get; set; }
        GpsSettingInfo defaultRule = new GpsSettingInfo();
        private int pageSizeValue;
        public int PageSizeValue
        {
            get { return pageSizeValue; }
            set
            {
                pageSizeValue = value;
                if (this.Gps_InfoList != null)
                {
                    this.Gps_InfoList.PageSize = value;
                }
                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => PageSizeValue));              
            }
        }
        private GpsSettingInfo _gpsSettingData;
        public GpsSettingInfo gpsSettingData
        {
            get 
               {
                   return _gpsSettingData;
               }
            set
              {
                  _gpsSettingData = value;               
              }
        }    
        
        private EnumModel _currentType;
        public EnumModel CurrentType 
        { 
            get 
            {
                return _currentType; 
            }
            set 
            {
                _currentType = value;
            } 
        }
        private bool _DeleteEnable;
        public bool DeleteEnable
        {
            get
            {
                return _DeleteEnable;
            }
            set
            {
                _DeleteEnable = value;          
                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => DeleteEnable));  
            }
        }
        #endregion       
        public GpsSettingViewModel()
        {
            try
            {
                gpsSettingClient = ServiceClientFactory.Create<CommandManageServiceClient>();
                gpsSettingClient.DeleteGpsSettingCompleted += gpsSettingClient_DeleteGpsSettingCompleted;
                gpsSettingClient.GpsSettingCompleted += gpsSettingClient_GpsSettingCompleted;
                AddCommand = new ActionCommand<object>(obj => Publish("Add"));
                RepairCommand = new ActionCommand<object>(obj => Modify("Modify"));
                DetailCommand = new ActionCommand<object>(obj => ToDetail("Detail"));
                QueryCommand = new ActionCommand<object>(obj => Query());
                DeleteCommand = new ActionCommand<object>(obj => Delete());
                DefaultCommand = new ActionCommand<object>(obj => Default());
                ToVehicleCommand = new ActionCommand<object>(obj => ToVehcile("ToVehicle"));
                PageSizeList = Gsafety.PTMS.Manager.ManagerCommon.PageSizeList;
                PageSizeValue = PageSizeList[0];
                InitPagedServerCollection();
                getTypeUpload();
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(), ex);
            }
        }
        private void Publish(string name)
        {
            try
            {              
                EventAggregator.Publish(new ViewNavigationArgs(ManagerName.GpsSettingAddView, new Dictionary<string, object>() { { "action", name }, { name, gpsSettingData } }));
               
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(), ex);
            }
        }

        private void ToDetail(string name)
        {
            try
            {
                EventAggregator.Publish(new ViewNavigationArgs(ManagerName.DetailGpsSettingsView, new Dictionary<string, object>() { { name, gpsSettingData } }));
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(), ex);
            }
        }

        protected override void ActivateView(string viewName, System.Collections.Generic.IDictionary<string, object> viewParameters)
        {
            try
            {
                base.ActivateView(viewName, viewParameters);
                if (viewParameters.Count != 0 && viewParameters["action"].ToString()== "return")
                {
                    Gps_InfoList.ToPage(currentIndex);
                }            
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(), ex);
            }            
        }
        private void InitPagedServerCollection()
        {
            try
            {
                Gps_InfoList = new PagedServerCollection<GpsSettingInfo>((pageIndex, pageSize) =>
                {
                    pageSize = PageSizeValue;
                    System.Threading.Interlocked.Exchange(ref currentIndex, pageIndex);

                    UploadType = (short)CurrentType.EnumValue;
                    gpsSettingClient.GpsSettingAsync(RuleName, UploadType,
                    new Gsafety.PTMS.ServiceReference.CommandManageService.PagingInfo { PageIndex = pageIndex, PageSize = pageSize });
                });
            }
            catch(Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(), ex);
            }         
        }
        //private void InvokServer(int pageIndex, int pageSize)
        //{
        //    try
        //    {
        //        pageSize = PageSizeValue;
        //        System.Threading.Interlocked.Exchange(ref currentIndex, pageIndex);
        //        PagingInfo pageInfo = new PagingInfo() { PageIndex = pageIndex, PageSize = pageSize };
        //        UploadType = (short)CurrentType.EnumValue;
        //        gpsSettingClient.GpsSettingAsync(RuleName, UploadType, pageInfo);
        //    }
        //    catch (Exception ex)
        //    {
               
        //    }
        //}
        void gpsSettingClient_GpsSettingCompleted(object c, GpsSettingCompletedEventArgs e)
        {
            try
            {
                Gps_InfoList.loader_Finished(new PagedResult<GpsSettingInfo>
                {
                    Count = e.Result.TotalRecord,
                    Items = e.Result.Result,
                    PageIndex = currentIndex,
                });  
                if (e.Result.Result.Count == 0)
                {
                    //MessageBox.Show(ApplicationContext.Instance.StringResourceReader.GetString("MAINTAIN_DataEmpty"), ApplicationContext.Instance.StringResourceReader.GetString("TRAFFIC_TIP"), MessageBoxButton.OK);
                }
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException("GpsSettingViewModel", ex);
                ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(), ex);
            }              
        }
      
        private void getTypeUpload()
        {
            try
            {
                UploadTypeList = new List<EnumModel>();
                UploadTypeList.Add(new EnumModel { ShowName = ApplicationContext.Instance.StringResourceReader.GetString("BASEINFO_All"), EnumName = string.Empty, EnumValue = -1 });
                foreach (var item in Enum.GetNames(typeof(GpsUploadType)))
                {
                    int typeValue = (int)Enum.Parse(typeof(GpsUploadType), item, true);
                    UploadTypeList.Add(new EnumModel { EnumName = item, ShowName = ApplicationContext.Instance.StringResourceReader.GetString(item), EnumValue = typeValue });
                }
                CurrentType = UploadTypeList.FirstOrDefault();
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(), ex);
            }
        }

        private void Delete()
        {
            try
            {
                if (gpsSettingData.Gps_IsDefault == 0)
                {
                    DeleteEnable = true;
                    if (gpsSettingData.Gps_VehicleCount > 0)
                    {
                        MessageBox.Show(ApplicationContext.Instance.StringResourceReader.GetString("MANAGE_Rule_Used"), ApplicationContext.Instance.StringResourceReader.GetString("Tip"), MessageBoxButton.OKCancel);
                    }
                    else
                    {
                        var result = MessageBox.Show(ApplicationContext.Instance.StringResourceReader.GetString("ConfirmDelete"), ApplicationContext.Instance.StringResourceReader.GetString("Tip"), MessageBoxButton.OKCancel);
                        if (result == MessageBoxResult.OK)
                        {
                            try
                            {
                                gpsSettingClient.DeleteGpsSettingAsync(gpsSettingData.Gps_RuleName);
                            }
                            catch (Exception ex)
                            {
                                ApplicationContext.Instance.Logger.LogException(GetType().FullName, ex);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(), ex);
            }
        }
        private void gpsSettingClient_DeleteGpsSettingCompleted(object sender, DeleteGpsSettingCompletedEventArgs e)
        {
            try
            {
                if (e.Result.IsSuccess)
                {
                    Gps_InfoList.ToPage(currentIndex);
                }
                else
                {
                    MessageBox.Show(ApplicationContext.Instance.StringResourceReader.GetString("Traffic_Oper_Faild"), ApplicationContext.Instance.StringResourceReader.GetString("Tip"), MessageBoxButton.OKCancel);
                }
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(), ex);
            }
        }
        private void Default()
        {
            try
            {
                if (MessageBox.Show(ApplicationContext.Instance.StringResourceReader.GetString("MANAGER_Rule_DelAllRelation"), ApplicationContext.Instance.StringResourceReader.GetString("TRAFFIC_TIP"), MessageBoxButton.OKCancel) == MessageBoxResult.OK)
                {
                    gpsSettingClient.GetDefaultGpsSettingAsync();
                    gpsSettingClient.GetDefaultGpsSettingCompleted += gpsSettingClient_GetDefaultGpsSettingCompleted;
                }
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(), ex);
            }
        }
        private void gpsSettingClient_GetDefaultGpsSettingCompleted(object sender, GetDefaultGpsSettingCompletedEventArgs e)
        {
            try
            {
                if (e.Result.IsSuccess)
                {
                    if (e.Result.Result != null)
                    {
                        GpsSettingInfo defaultRuleInfo = e.Result.Result as GpsSettingInfo;
                        ServiceReference.MessageService.GpsSendUpModel model = new ServiceReference.MessageService.GpsSendUpModel();
                        model.Setting = new SettingGpsSendUpCMD();
                        model.Setting.SendTime = DateTime.Now;
                        model.OperationType = RuleOperationType.Default;
                        model.Setting.DistanceValue = defaultRuleInfo.Gps_Distance;
                        model.Setting.TimeValue = e.Result.Result.Gps_Time;
                        model.Setting.SendType = (GpsSendType)defaultRuleInfo.Gps_UploadType;
                        model.Setting.IsUsing = (short)defaultRuleInfo.Gps_IfMonitor;
                        model.Setting.UserName = ApplicationContext.Instance.AuthenticationInfo.UserName;
                        model.Setting.RuleName = gpsSettingData.Gps_RuleID;
                        ApplicationContext.Instance.MessageManager.SendSettingGpsCMD(model);
                        Gps_InfoList.MoveToPage(currentIndex);
                    }
                    else
                    {
                        MessageBox.Show(ApplicationContext.Instance.StringResourceReader.GetString("FAIL"), ApplicationContext.Instance.StringResourceReader.GetString("TRAFFIC_TIP"), MessageBoxButton.OK);
                    }
                }
                else
                {
                    throw e.Error;
                }
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(), ex);
            }
        }

        private void gpsSettingClient_ModifyGpsSettingsCompleted(object sender, ModifyGpsSettingsCompletedEventArgs e)
        {
            try
            {
                if (e.Result.IsSuccess)
                {                   
                    ServiceReference.MessageService.GpsSendUpModel model = new ServiceReference.MessageService.GpsSendUpModel();
                    model.Setting = new SettingGpsSendUpCMD();
                    model.Setting.SendTime = DateTime.Now;
                    model.OperationType = RuleOperationType.Default;
                    model.Setting.DistanceValue = defaultRule.Gps_Distance;
                    model.Setting.TimeValue = defaultRule.Gps_Time;
                    model.Setting.SendType = (GpsSendType)defaultRule.Gps_UploadType;
                    model.Setting.IsUsing = (short)defaultRule.Gps_IfMonitor;
                    model.Setting.UserName = ApplicationContext.Instance.AuthenticationInfo.UserName;
                    model.Setting.RuleName = gpsSettingData.Gps_RuleID;
                    ApplicationContext.Instance.MessageManager.SendSettingGpsCMD(model);
                    Gps_InfoList.ToPage(currentIndex);
                }
            }
            catch(Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(), ex);
            }
        }

        private void ToVehcile(string name)
        {
            try
            {
                EventAggregator.Publish(new ViewNavigationArgs(ManagerName.GpsSettingToVechileView, new Dictionary<string, object>() { { name, gpsSettingData } }));
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(), ex);
            }
        }
        private void Modify(string name)
        {
            try
            {
                if (gpsSettingData.Gps_VehicleCount > 0)
                {
                    MessageBox.Show(ApplicationContext.Instance.StringResourceReader.GetString("MANAGE_Rule_Used"), ApplicationContext.Instance.StringResourceReader.GetString("Tip"), MessageBoxButton.OKCancel);
                }
                else
                {
                    EventAggregator.Publish(new ViewNavigationArgs(ManagerName.GpsSettingModifyView, new Dictionary<string, object>() { { name, gpsSettingData } }));
                }
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(), ex);
            }
        }

        private void Query()
        {
            try
            {
                Gps_InfoList.MoveToFirstPage();
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(), ex);
            }

        }
    }
}
