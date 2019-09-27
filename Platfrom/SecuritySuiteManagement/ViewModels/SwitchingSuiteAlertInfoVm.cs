using Gsafety.PTMS.ServiceReference.DeviceAlertService;
using Gsafety.PTMS.Share;
using Jounce.Core.Command;
using Jounce.Core.ViewModel;
using Jounce.Framework.Command;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace Gsafety.PTMS.SecuritySuite.ViewModels
{
    [ExportAsViewModel(SecuritySuiteName.SwitchingSuiteAlertInfoVm)]

    public class SwitchingSuiteAlertInfoVm : BaseViewModel
    {
       public string InputSuiteID { get; set; }
       public string InputCarNumber { get; set; }
       private DateTime _inputstartime = DateTime.Now.AddDays(-1);
       private bool isFirst;
       public DateTime InputStartTime 
       {
           get
           {
               return _inputstartime;
           }
           set
           {
               _inputstartime = value;
           }
       }
       private DateTime _inputendtime = DateTime.Now;
       public DateTime InputEndTime 
       {
           get
           {
               return _inputendtime;
           }
           set
           {
               _inputendtime = value.AddHours(24);
           }
       }
       public List<AlertTypeItem> AlertTypes { get; set; }
       public List<string> ishandled { get; set; }
       public List<string> SelectedAlertTypes { get; set; }
       public string ShowAlertTypes { get; set; }

       private bool _isBusy = false;
       public bool IsBusy
       {
            get { return this._isBusy; }
            set
            {
                this._isBusy = value;
                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => IsBusy));
            }
        }

        private string _BusyContent = ApplicationContext.Instance.StringResourceReader.GetString("DateLoading");
        public string BusyContent
        {
            get
            {
                return _BusyContent;
            }
        }

       private List<int> pageSizeList;
       public List<int> PageSizeList
       {
           get 
           { 
               return pageSizeList; 
           }
           set 
           { 
               pageSizeList = value; 
           }
       }

       private int _PageSizeValue = 20;
       public int PageSizeValue
       {
           get
           {
               return _PageSizeValue;
           }
           set
           {
               _PageSizeValue = value;
               Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => PageSizeValue));
               PageChanged = true;
               PageSizeChenged();
           }
       }
       private void PageSizeChenged()
       {
           PagingInfo pageinfo = new PagingInfo
           {
               PageIndex = 0,
               PageSize = PageSizeValue
           };
           ServiceClientFactory.CreateMessageHeader(deviceAlertServiceClient.InnerChannel);
           deviceAlertServiceClient.GetDeviceAlertEx1Async(InputCarNumber, InputSuiteID, typs, InputStartTime, InputEndTime, pageinfo);
       }

       private decimal? _selecthandle;
       public decimal? selecthandle
       {
           get
           {
               return _selecthandle;
           }
           set
           {
               _selecthandle = value;
           }
       }

       private string _selecthandled;
       public string selecthandled
       {
           get
           {
               return _selecthandled;
           }
           set
           {
               _selecthandled = value;
               if (_selecthandled == ApplicationContext.Instance.StringResourceReader.GetString("MAINTAIN_Unhandled"))
               {
                   selecthandle = 1;
               }
               else if (_selecthandled == ApplicationContext.Instance.StringResourceReader.GetString("MAINTAIN_Verified"))
               {
                   selecthandle = 2;
               }
               else if (_selecthandled == ApplicationContext.Instance.StringResourceReader.GetString("MAINTAIN_handled"))
               {
                   selecthandle = 3;
               }
               else
               {
                   selecthandle = 0;
               }
               Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => selecthandled));
               QueryRepairRecord();
           }
       }
       public class AlertTypeItem : BaseViewModel
       {
           public int ID { get; set; }
           public string AlertType { get; set; }
           public Action<AlertTypeItem> IsCheckedChangeAction { get; set; }
           public Action<bool> AttributeChangeAction { get; set; }

           public bool IsRecheck;

           private bool _IsChecked;
           public bool IsChecked
           {
               get { return _IsChecked; }
               set
               {
                   if (_IsChecked != value)
                   {
                       _IsChecked = value;
                       if (IsCheckedChangeAction != null)
                       {
                           IsCheckedChangeAction(this);
                       }
                       if (AttributeChangeAction != null)
                       {
                           if (IsRecheck == true)
                               AttributeChangeAction(value);
                           else
                               IsRecheck = true;
                       }
                       Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => IsChecked));
                   }
               }
           }
       }
       public IActionCommand QueryAlertInfoCommand { get; private set; }
       public IActionCommand OperationCommand { get; private set; }

       private ObservableCollection<DeviceAlert> _AlertInfoRecords;
       public ObservableCollection<DeviceAlert> AlertInfoRecords
       {
           get { return this._AlertInfoRecords; }
           set
           {
               this._AlertInfoRecords = value;
               Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => AlertInfoRecords));
           }
       }
       private DeviceAlert _AlertInfo;
       public DeviceAlert AlertInfo
       {
           get
           {
               return _AlertInfo;
           }
           set
           {
               _AlertInfo = value;
               Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => AlertInfo));
           }
       }

       ObservableCollection<decimal?> typs = new ObservableCollection<decimal?>();

       DeviceAlertServiceClient deviceAlertServiceClient;

       public SwitchingSuiteAlertInfoVm()
       {
           try
           {
               isFirst = true;
               deviceAlertServiceClient = ServiceClientFactory.Create<DeviceAlertServiceClient>();
               deviceAlertServiceClient.GetDeviceAlertEx1Completed += deviceAlertServiceClient_GetDeviceAlertEx1Completed;
               //deviceAlertServiceClient.GetSwitchingSuiteAlertInfoAsync();
               AlertTypes = new List<AlertTypeItem>
           {
               new AlertTypeItem{ID = 0,AlertType = ApplicationContext.Instance.StringResourceReader.GetString("MAINTAIN_All"),AttributeChangeAction = checkAllAlertTypes, IsCheckedChangeAction = null, IsRecheck = true},
               new AlertTypeItem{ID = 10,AlertType = ApplicationContext.Instance.StringResourceReader.GetString("LowTemperature"),IsChecked = false, AttributeChangeAction = changeAllAlertTypes, IsCheckedChangeAction = alertTypeCheckChanged, IsRecheck = false},
               new AlertTypeItem{ID = 11,AlertType = ApplicationContext.Instance.StringResourceReader.GetString("OverTemperature"),IsChecked = false, AttributeChangeAction = changeAllAlertTypes, IsCheckedChangeAction = alertTypeCheckChanged, IsRecheck = false},
               new AlertTypeItem{ID = 12,AlertType = ApplicationContext.Instance.StringResourceReader.GetString("GpsFault"),IsChecked = false, AttributeChangeAction = changeAllAlertTypes, IsCheckedChangeAction = alertTypeCheckChanged, IsRecheck = false},
             //  new AlertTypeItem{ID = 13,AlertType = ApplicationContext.Instance.StringResourceReader.GetString("VedioShelter"),IsChecked = false, AttributeChangeAction = changeAllAlertTypes, IsCheckedChangeAction = alertTypeCheckChanged, IsRecheck = false},
               new AlertTypeItem{ID = 14,AlertType = ApplicationContext.Instance.StringResourceReader.GetString("VedioNoSignal"),IsChecked = false, AttributeChangeAction = changeAllAlertTypes, IsCheckedChangeAction = alertTypeCheckChanged, IsRecheck = false},      
               new AlertTypeItem{ID = 16,AlertType = ApplicationContext.Instance.StringResourceReader.GetString("SdFault"),IsChecked = false, AttributeChangeAction = changeAllAlertTypes, IsCheckedChangeAction = alertTypeCheckChanged, IsRecheck = false},
               //new AlertTypeItem{ID = 17,AlertType = ApplicationContext.Instance.StringResourceReader.GetString("PasswordFault"),IsChecked = false, AttributeChangeAction = changeAllAlertTypes, IsCheckedChangeAction = alertTypeCheckChanged, IsRecheck = false},
               new AlertTypeItem{ID = 18,AlertType = ApplicationContext.Instance.StringResourceReader.GetString("AbnormalValtage"),IsChecked = false, AttributeChangeAction = changeAllAlertTypes, IsCheckedChangeAction = alertTypeCheckChanged, IsRecheck = false},
           };
               ishandled = new List<string>
           {
               ApplicationContext.Instance.StringResourceReader.GetString("All"),
               ApplicationContext.Instance.StringResourceReader.GetString("MAINTAIN_Unhandled"),
               ApplicationContext.Instance.StringResourceReader.GetString("MAINTAIN_Verified"),
               ApplicationContext.Instance.StringResourceReader.GetString("MAINTAIN_handled")
           };
               PageSizeList = new List<int>
           {
               20,
               40,
               80,
           };
               SelectedAlertTypes = new List<string>();
               typs = new ObservableCollection<decimal?>();
               selecthandled = ApplicationContext.Instance.StringResourceReader.GetString("All");
               QueryAlertInfoCommand = new ActionCommand<object>(obj => QueryRepairRecord());
               OperationCommand = new ActionCommand<object>(obj => Operation());
               Jounce.Framework.JounceHelper.ExecuteOnUI(() =>
               {
                   RaisePropertyChanged(() => AlertTypes);
                   RaisePropertyChanged(() => PageSizeList);
                   RaisePropertyChanged(() => ishandled);
               });
           }
           catch (Exception ex)
           {
               ApplicationContext.Instance.Logger.LogException("SwitchingSuiteAlertInfoVm()", ex);
           }
       }

       private void Operation()
       {
           
       }

       private void checkAllAlertTypes(bool isChecked)
       {
           try
           {
               if (isChecked == true)
               {
                   foreach (AlertTypeItem item in AlertTypes)
                       item.IsChecked = isChecked;
               }
               if (isChecked == false)
               {
                   foreach (AlertTypeItem item in AlertTypes)
                       item.IsChecked = isChecked;
               }
           }
           catch (Exception ex)
           {
               ApplicationContext.Instance.Logger.LogException("checkAllAlertTypes()", ex);
           }
       }

       private void changeAllAlertTypes(bool isChecked)
       {
           try
           {
               AlertTypeItem allItem = AlertTypes[0];
               if (isChecked == true && allItem.IsChecked == false)
               {
                   bool isAllChecked = true;
                   for (int i = 1; i < AlertTypes.Count; i++)
                       isAllChecked &= AlertTypes[i].IsChecked;
                   if (isAllChecked == true)
                   {
                       allItem.IsRecheck = false;
                       allItem.IsChecked = true;
                   }
               }
               if (isChecked == false && allItem.IsChecked == true)
               {
                   allItem.IsRecheck = false;
                   allItem.IsChecked = false;
               }
           }
           catch (Exception ex)
           {
               ApplicationContext.Instance.Logger.LogException("changeAllAlertTypes()", ex);
           }
       }

       private void alertTypeCheckChanged(AlertTypeItem item)
       {
           try
           {
               if (item.IsChecked == true)
               {
                   SelectedAlertTypes.Add(item.AlertType);
                   typs.Add(item.ID);
               }
               else
               {
                   SelectedAlertTypes.Remove(item.AlertType);
                   typs.Remove(item.ID);
               }
               Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => SelectedAlertTypes));
           }
           catch (Exception ex)
           {
               ApplicationContext.Instance.Logger.LogException("alertTypeCheckChanged()", ex);
           }
      }

       void deviceAlertServiceClient_GetDeviceAlertEx1Completed(object sender, GetDeviceAlertEx1CompletedEventArgs e)
       {
           try
           {
               IsBusy = false;
               if ((totalCount != e.Result.TotalRecord) || PageChanged)
                   totalCount = e.Result.TotalRecord;
               PageChanged = false;
               AlertInfoRecords = e.Result.Result;
               if (totalCount == 0&&!isFirst)
               {
                   //MessageBox.Show(ApplicationContext.Instance.StringResourceReader.GetString("MAINTAIN_DataEmpty"), ApplicationContext.Instance.StringResourceReader.GetString("TRAFFIC_TIP"), MessageBoxButton.OK);
               }
               isFirst = false;
           }
           catch (Exception ex)
           {
               ApplicationContext.Instance.Logger.LogException("deviceAlertServiceClient_GetDeviceAlertEx1Completed()", ex);
           }
       }

 
       public bool PageChanged = false;

       private int _PageIndex = -1;
       public int PageIndex
       {
           get
           {
               return _PageIndex;
           }
           set
           {
               if (_PageIndex == -1)
               {
                   _PageIndex = value;
               }
               else
               {
                   _PageIndex = value;
                   GetInfoByPageIndex();
               }
           }
       }
       private void GetInfoByPageIndex()
       {
           try
           {
               PagingInfo pageinfo = new PagingInfo
               {
                   PageIndex = PageIndex,
                   PageSize = PageSizeValue
               };
               ServiceClientFactory.CreateMessageHeader(deviceAlertServiceClient.InnerChannel);
               deviceAlertServiceClient.GetDeviceAlertEx1Async(InputCarNumber, InputSuiteID, typs, InputStartTime, InputEndTime, pageinfo);
           }
           catch (Exception ex)
           {
               ApplicationContext.Instance.Logger.LogException("GetInfoByPageIndex()", ex);
           }
       }

       private PagedCollectionView _ItemCount;
       public PagedCollectionView ItemCount
       {
           get { return _ItemCount; }
           set
           {
               _ItemCount = value;
               Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => ItemCount));
           }
       }

       private int _totalCount;
       public int totalCount
       {
           get
           {
               return _totalCount;
           }
           set
           {
               _totalCount = value;
               initdatapager(_totalCount);
           }
       }
       
       private void initdatapager(int _totalCount)
       {
           try
           {
               List<int> itemCount = new List<int>();
               for (int i = 1; i <= _totalCount; i++)
               {
                   itemCount.Add(i);
               }
               ItemCount = new PagedCollectionView(itemCount);
           }
           catch (Exception ex)
           {
               ApplicationContext.Instance.Logger.LogException("initdatapager()", ex);
           }
       }
       
       private void QueryRepairRecord()
       {
           try
           {
               IsBusy = true;
               PagingInfo pageinfo = new PagingInfo
               {
                   PageIndex = PageIndex,
                   PageSize = PageSizeValue
               };
               ServiceClientFactory.CreateMessageHeader(deviceAlertServiceClient.InnerChannel);
               deviceAlertServiceClient.GetDeviceAlertEx1Async(InputCarNumber, InputSuiteID, typs, InputStartTime, InputEndTime, pageinfo);
           }
           catch (Exception ex)
           {
               ApplicationContext.Instance.Logger.LogException("QueryRepairRecord()", ex);
           }
       }
    }
}

