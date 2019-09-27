/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: d473d641-3b78-4a23-8aba-718e759bf71a      
/////             clrversion: 4.0.30319.17929
/////Registered organization: 
/////           Machine Name: PC-LANQ
/////                 Author: TEST(lanq)
/////======================================================================
/////           Project Name: Gsafety.PTMS.BaseInformation.ViewModels
/////    Project Description:    
/////             Class Name: SuiteInfoVm
/////          Class Version: v1.0.0.0
/////            Create Time: 2013/8/12 16:07:35
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2013/8/12 16:07:35
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
using Jounce.Core.ViewModel;
using Gsafety.PTMS.ServiceReference.SecuritySuiteService;
using Jounce.Core.Command;
using Gsafety.PTMS.Share;
using Jounce.Framework.Command;
using Jounce.Core.View;
using System.Collections.Generic;
using System.Linq;
using System.Collections.ObjectModel;
using System.Collections;
using Gsafety.PTMS.Bases.Models;
using Gsafety.PTMS.Spreadsheet;
using System.IO;
using BaseLib.ViewModels;

namespace Gsafety.PTMS.BaseInformation.ViewModels
{
    [ExportAsViewModel(BaseInformationName.SuiteInfoVm)]
    public class SuiteInfoVm : BaseInfoViewModelBase
    {
        private string suiteId;
        private string vehicleId;
        private string mdvrId;
        private string mdvrCoreId;
        private InstallStatusType? status;

        private SecuritySuiteServiceClient suiteClient = ServiceClientFactory.Create<SecuritySuiteServiceClient>();
        public DeviceSuite CurrentSecuritySuite { get; set; }
        public PagedServerCollection<DeviceSuite> PSC_SecuritySuite { get; set; }

        public string SuiteId { get; set; }
        public string VehicleId { get; set; }
        public string MdvrId { get; set; }
        public string MdvrCoreId { get; set; }
        private int pageSizeValue;
        public int PageSizeValue
        {
            get { return pageSizeValue; }
            set
            {
                pageSizeValue = value;
                if (this.PSC_SecuritySuite != null)
                {
                    this.PSC_SecuritySuite.PageSize = value;
                }
            }
        }

        public List<EnumModel> DeviceTypeList { get; set; }
        private void InitialDeviceType()
        {
            DeviceTypeList = new List<EnumModel>();
            Enum.GetNames(typeof(Gsafety.PTMS.ServiceReference.SecuritySuiteService.VehicleTypeEnum)).ToList().ForEach(x =>
            {
                EnumModel item = new EnumModel { EnumName = x, ShowName = ApplicationContext.Instance.StringResourceReader.GetString(x) };
                DeviceTypeList.Add(item);
            });
        }


        private void InitialInstallStatus()
        {
            InstallStatusList = new List<EnumModel>();
            InstallStatusList.Add(new EnumModel { ShowName = ApplicationContext.Instance.StringResourceReader.GetString("BASEINFO_PleaseSelect"), EnumName = string.Empty });
            Enum.GetNames(typeof(InstallStatusType)).ToList().ForEach(x =>
            {
                EnumModel item = new EnumModel { EnumName = x, ShowName = ApplicationContext.Instance.StringResourceReader.GetString(x) };
                InstallStatusList.Add(item);
            });
        }


        protected override void ActivateView(string viewName, System.Collections.Generic.IDictionary<string, object> viewParameters)
        {
            base.ActivateView(viewName, viewParameters);
            if (viewParameters.Count != 0 && viewParameters["action"].ToString() == "return")
            {
                PSC_SecuritySuite.ToPage(currentIndex);
            }
            Url = new Uri(BaseInformationCommon.GetTemplateUrl(DownLoadType.Suite));
            Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => Url));
        }

        public SuiteInfoVm()
        {
            try
            {
                isInitialOrQuery = true;
                InitialInstallStatus();
                UploadBtnStatus = true;
                ExportBtnStatus = true;
                suiteClient.BatchAddCompleted += SecuritySuiteClient_BatchAddCompleted;
                suiteClient.DeleteSecuritySuiteCompleted += SecuritySuiteClient_DeleteSecuritySuiteCompleted;
                suiteClient.CheckSecuritySuiteExistCompleted += suiteClient_CheckSecuritySuiteExistCompleted;
                InitialDeviceType();
                CurrentInstallStatus = InstallStatusList[0];
                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => CurrentInstallStatus));

                PageSizeList = BaseInformationCommon.PageSizeList;
                PageSizeValue = PageSizeList[0];
                InitPagedServerCollection();
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException("SuiteInfoVm()", ex);
            }

        }

        protected override void InitPagedServerCollection()
        {
            suiteClient.GetSecuritySuitesFuzzyCompleted += SecuritySuiteClient_GetSecuritySuitesFuzzyCompleted;
            PSC_SecuritySuite = new PagedServerCollection<DeviceSuite>((pageIndex, pageSize) =>
            {
                pageSize = PageSizeValue;
                System.Threading.Interlocked.Exchange(ref currentIndex, pageIndex);
                PagingInfo pagingInfo = new PagingInfo() { PageIndex = pageIndex, PageSize = pageSize };
                ServiceClientFactory.CreateMessageHeader(suiteClient.InnerChannel);
                suiteClient.GetSecuritySuitesFuzzyAsync(vehicleId, suiteId, mdvrId, mdvrCoreId, status, pagingInfo);
            });
        }

        void SecuritySuiteClient_GetSecuritySuitesFuzzyCompleted(object sender, GetSecuritySuitesFuzzyCompletedEventArgs e)
        {
            try
            {
                PSC_SecuritySuite.loader_Finished(new PagedResult<DeviceSuite>
                {
                    Count = e.Result.TotalRecord,
                    Items = e.Result.Result,
                    PageIndex = currentIndex,
                });
                if (e.Result.TotalRecord == 0)
                {
                    setExportBtnStatus(false);
                }
                else
                {
                    setExportBtnStatus(true);
                }
                if (e.Result.TotalRecord == 0 && isInitialOrQuery)
                {
                    //MessageBox.Show(ApplicationContext.Instance.StringResourceReader.GetString("MAINTAIN_DataEmpty"), ApplicationContext.Instance.StringResourceReader.GetString("TRAFFIC_TIP"), MessageBoxButton.OK);
                }
                if (isInitialOrQuery)
                {
                    isInitialOrQuery = false;
                }
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException("SecuritySuiteClient_GetSecuritySuitesFuzzyCompleted()", ex);
            }
        }

        protected override void Delete()
        {
            var result = MessageBox.Show(ApplicationContext.Instance.StringResourceReader.GetString("BASEINFO_DeleteConfirm"), ApplicationContext.Instance.StringResourceReader.GetString("TRAFFIC_TIP"), MessageBoxButton.OKCancel);
            if (result == MessageBoxResult.OK)
            {
                if (CurrentSecuritySuite != null)
                {
                    suiteClient.DeleteSecuritySuiteAsync(CurrentSecuritySuite);
                }
            }
        }

        void SecuritySuiteClient_DeleteSecuritySuiteCompleted(object sender, System.ComponentModel.AsyncCompletedEventArgs e)
        {
            PSC_SecuritySuite.ToPage(currentIndex);
        }

        protected override void Query()
        {
            try
            {
                isInitialOrQuery = true;

                suiteId = SuiteId == null ? string.Empty : SuiteId.Trim();
                vehicleId = VehicleId == null ? string.Empty : VehicleId.Trim();
                mdvrCoreId = MdvrCoreId == null ? string.Empty : MdvrCoreId.Trim();
                mdvrId = MdvrId == null ? string.Empty : MdvrId.Trim();
                status = null;
                if (CurrentInstallStatus != null && CurrentInstallStatus.EnumName != string.Empty)
                {
                    status = (InstallStatusType)Enum.Parse(typeof(InstallStatusType), CurrentInstallStatus.EnumName, true);
                }

                currentIndex = 1;
                PSC_SecuritySuite.MoveToFirstPage();
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException("SuiteInfoVm Query", ex);
            }
        }

        protected override void Export()
        {
            SaveFileDialog dlg = new SaveFileDialog();
            dlg.Filter = "Excel Document (.xlsx)|*.xlsx";
            dlg.DefaultExt = ".xlsx";
            bool? dialogResult = dlg.ShowDialog();
            if (dialogResult == true)
            {
                setExportBtnStatus(false);
                SecuritySuiteServiceClient client = ServiceClientFactory.Create<SecuritySuiteServiceClient>();
                client.GetSecuritySuitesFuzzyCompleted += (s, e) =>
                {
                    if (e.Result != null && e.Result.TotalRecord > 0)
                    {
                        List<string> Codes = new List<string>();
                        Codes.Add("InstallStatus");
                        Codes.Add("SuiteId");
                        Codes.Add("MdvrId");
                        Codes.Add("MdvrCoreId");
                        Codes.Add("DeviceType");
                        Codes.Add("Camera1Id");
                        Codes.Add("Camera2Id");
                        Codes.Add("Camera3Id");
                        Codes.Add("Camera4Id");
                        Codes.Add("AlarmButton1Id");
                        Codes.Add("DoorSensorId");
                        Codes.Add("SdCardId");
                        Codes.Add("UpsId");
                        Codes.Add("status");


                        Codes.Add("MdvrSimId");
                        Codes.Add("MdvrSimPhoneNumber");

                        Codes.Add("SoftwareVersion");

                        List<string> Names = new List<string>();
                        Names.Add(ApplicationContext.Instance.StringResourceReader.GetString("BASEINFO_InstallStatus"));
                        Names.Add(ApplicationContext.Instance.StringResourceReader.GetString("BASEINFO_SuiteID"));
                        Names.Add(ApplicationContext.Instance.StringResourceReader.GetString("BASEINFO_MDVR_SN"));
                        Names.Add(ApplicationContext.Instance.StringResourceReader.GetString("BASEINFO_MDVR_CORE_SN"));
                        Names.Add(ApplicationContext.Instance.StringResourceReader.GetString("BASEINFO_DeviceType"));
                        Names.Add(ApplicationContext.Instance.StringResourceReader.GetString("BASEINFO_Camera1"));
                        Names.Add(ApplicationContext.Instance.StringResourceReader.GetString("BASEINFO_Camera2"));
                        Names.Add(ApplicationContext.Instance.StringResourceReader.GetString("BASEINFO_Camera3"));
                        Names.Add(ApplicationContext.Instance.StringResourceReader.GetString("BASEINFO_Camera4"));
                        Names.Add(ApplicationContext.Instance.StringResourceReader.GetString("BASEINFO_AlarmButton1"));
                        Names.Add(ApplicationContext.Instance.StringResourceReader.GetString("BASEINFO_DOOR_SWITCH_SENSOR"));
                        Names.Add(ApplicationContext.Instance.StringResourceReader.GetString("BASEINFO_SdCardId"));
                        Names.Add(ApplicationContext.Instance.StringResourceReader.GetString("BASEINFO_UPS"));
                        Names.Add(ApplicationContext.Instance.StringResourceReader.GetString("BASEINFO_SuiteStatus"));




                        Names.Add(ApplicationContext.Instance.StringResourceReader.GetString("BASEINFO_MdvrSimId"));
                        Names.Add(ApplicationContext.Instance.StringResourceReader.GetString("BASEINFO_MdvrSimPhoneNumber"));

                        Names.Add(ApplicationContext.Instance.StringResourceReader.GetString("BASEINFO_SoftwareVersion"));


                        List<EnumsEx> eList = new List<EnumsEx>();

                        List<FieldEx> FieldEx1 = new List<FieldEx>();
                        Enum.GetNames(typeof(Gsafety.PTMS.ServiceReference.SecuritySuiteService.VehicleTypeEnum)).ToList().ForEach(x =>
                        {
                            FieldEx item = new FieldEx { Key = x, Value = ApplicationContext.Instance.StringResourceReader.GetString(x) };
                            FieldEx1.Add(item);
                        });
                        eList.Add(new EnumsEx { Code = "DeviceType", Content = FieldEx1 });

                        List<FieldEx> FieldEx2 = new List<FieldEx>();
                        Enum.GetNames(typeof(InstallStatusType)).ToList().ForEach(x =>
                        {
                            FieldEx item = new FieldEx { Key = x, Value = ApplicationContext.Instance.StringResourceReader.GetString(x) };
                            FieldEx2.Add(item);
                        });
                        eList.Add(new EnumsEx { Code = "InstallStatus", Content = FieldEx2 });

                        List<FieldEx> FieldEx3 = new List<FieldEx>();
                        Enum.GetNames(typeof(DeviceSuiteStatus)).ToList().ForEach(x =>
                        {
                            FieldEx item = new FieldEx { Key = x, Value = ApplicationContext.Instance.StringResourceReader.GetString(x) };
                            FieldEx3.Add(item);
                        });
                        eList.Add(new EnumsEx { Code = "status", Content = FieldEx3 });

                        XLSXExporter xlsx = new XLSXExporter();
                        xlsx.Export(e.Result.Result.ToList(), dlg.OpenFile(), Codes, Names, eList);
                        setExportBtnStatus(true);
                        MessageBox.Show(ApplicationContext.Instance.StringResourceReader.GetString("BaseInfo_ExportSucceed"), ApplicationContext.Instance.StringResourceReader.GetString("TRAFFIC_TIP"), MessageBoxButton.OK);
                    }
                    else
                    {
                        MessageBox.Show(ApplicationContext.Instance.StringResourceReader.GetString("BaseInfo_ExportFaild"), ApplicationContext.Instance.StringResourceReader.GetString("TRAFFIC_TIP"), MessageBoxButton.OK);
                    }
                };

                if (PSC_SecuritySuite.TotalItemCount > 10000)
                {
                    PagingInfo pagingInfo = new PagingInfo() { PageIndex = 1, PageSize = 10000 };
                    client.GetSecuritySuitesFuzzyAsync(vehicleId, suiteId, mdvrId, mdvrCoreId, status, pagingInfo);
                }
                else
                {
                    client.GetSecuritySuitesFuzzyAsync(vehicleId, suiteId, mdvrId, mdvrCoreId, status, null);
                }
            }
        }

        protected override void Publish(string name)
        {
            EventAggregator.Publish(new ViewNavigationArgs(BaseInformationName.SuiteInfoManageV, new Dictionary<string, object>() { { "action", name }, { name, CurrentSecuritySuite } }));
        }

        private List<DeviceSuite> uploadList = new List<DeviceSuite>();
        protected override void UploadExcel()
        {
            try
            {
                uploadList = new List<DeviceSuite>();
                batchCount = 40;
                batchIndex = 1;
                errorIndex = -1;
                errorCode = string.Empty;
                System.IO.FileInfo fileInfo;
                OpenFileDialog openFileDialog = new OpenFileDialog();
                openFileDialog.Filter = " Files(*.xlsx)|*.xlsx|All files(*.*)|*.*";
                if (openFileDialog.ShowDialog() == true)
                {

                    fileInfo = openFileDialog.File;
                    if (!fileInfo.Name.EndsWith("xlsx"))
                    {
                        MessageBox.Show(ApplicationContext.Instance.StringResourceReader.GetString("BASEINFO_FormatIllegal"), ApplicationContext.Instance.StringResourceReader.GetString("TRAFFIC_TIP"), MessageBoxButton.OK);
                        return;
                    }

                    FileStream fs = null;
                    try
                    {
                        fs = fileInfo.OpenRead();
                    }
                    catch
                    {
                        MessageBox.Show(ApplicationContext.Instance.StringResourceReader.GetString("BASEINFO_CloseFile"), ApplicationContext.Instance.StringResourceReader.GetString("TRAFFIC_TIP"), MessageBoxButton.OK);
                        return;
                    }

                    try
                    {
                        XLSXReader xlsxReader = new XLSXReader(fileInfo);

                        List<string> subItems = xlsxReader.GetListSubItems();

                        IEnumerable<IDictionary> datasource = xlsxReader.GetData(subItems[0]);

                        int maxColumns = 0;
                        if (datasource.Count() == 0)
                        {
                            MessageBox.Show(ApplicationContext.Instance.StringResourceReader.GetString("MAINTAIN_DataEmpty"), ApplicationContext.Instance.StringResourceReader.GetString("TRAFFIC_TIP"), MessageBoxButton.OK);
                            return;
                        }

                        IDictionary firstRow = null;

                        firstRow = datasource.First();
                        maxColumns = firstRow.Count;

                        List<object> lines = new List<object>();


                        foreach (IDictionary currentDict in datasource)
                        {
                            List<object> cells = new List<object>();
                            foreach (DictionaryEntry pair in firstRow)
                            {
                                if (currentDict.Contains(pair.Key))
                                {
                                    cells.Add(currentDict[pair.Key]);
                                }
                                else
                                {
                                    cells.Add(string.Empty);
                                }
                            }
                            lines.Add(cells);
                        }
                        if (lines.Count < 2)
                        {
                            MessageBox.Show(ApplicationContext.Instance.StringResourceReader.GetString("MAINTAIN_DataEmpty"), ApplicationContext.Instance.StringResourceReader.GetString("TRAFFIC_TIP"), MessageBoxButton.OK);
                            return;
                        }

                        uploadContent = new String[lines.Count][];
                        for (int i = 0; i < uploadContent.Length; i++)
                        {
                            List<object> cells = (List<object>)lines[i];
                            uploadContent[i] = new String[cells.Count];
                            for (int j = 0; j < cells.Count; j++)
                            {
                                uploadContent[i][j] = cells[j].ToString();
                            }
                        }
                        uploadList = GetUploadList();
                        if (errorIndex == -1)
                        {
                            setUploadBtnStatus(false);
                            suiteClient.CheckSecuritySuiteExistAsync(new ObservableCollection<DeviceSuite>(uploadList));
                        }
                        else
                        {
                            MessageBox.Show(ApplicationContext.Instance.StringResourceReader.GetString("BASEINFO_ErrorCell") + " " + errorCode, ApplicationContext.Instance.StringResourceReader.GetString("TRAFFIC_TIP"), MessageBoxButton.OK);
                            uploadContent = null;
                        }
                    }
                    finally
                    {
                        if (fs != null)
                        {
                            fs.Close();
                        }
                    }
                }
            }
            catch
            {
                MessageBox.Show(ApplicationContext.Instance.StringResourceReader.GetString("FAIL"), ApplicationContext.Instance.StringResourceReader.GetString("TRAFFIC_TIP"), MessageBoxButton.OK);
            }
        }

        protected override void GoNextPage()
        {
            PSC_SecuritySuite.ToPage(currentIndex);
        }

        private List<DeviceSuite> GetUploadList()
        {
            List<DeviceSuite> tempSuiteList = new List<DeviceSuite>();
            int startIndex = batchIndex;
            int endIndex = uploadContent.Length;

            for (int i = startIndex; i < endIndex; i++)
            {
                DeviceSuite tempSuite = new DeviceSuite();

                tempSuite.SuiteId = uploadContent[i][0].ToString().Trim();//SuiteId
                if (tempSuite.SuiteId == string.Empty)
                {
                    errorIndex = i;
                    errorCode = "A" + (i + 1).ToString() + ApplicationContext.Instance.StringResourceReader.GetString("Info_isEmpty");
                    break;
                }
                if (tempSuite.SuiteId.Trim().Length > 25)
                {
                    errorIndex = i;
                    errorCode = "A" + (i + 1).ToString() + ApplicationContext.Instance.StringResourceReader.GetString("Length_Warning");
                    break;
                }
                if (tempSuiteList.Any(t => t.SuiteId == tempSuite.SuiteId.Trim()))
                {
                    errorIndex = i;
                    errorCode = "A" + (i + 1).ToString() + "  " + ApplicationContext.Instance.StringResourceReader.GetString("ValueTheSame");
                    break;
                }

                tempSuite.MdvrId = uploadContent[i][1].ToString().Trim();//MdvrId
                if (tempSuite.MdvrId == string.Empty)
                {
                    errorIndex = i;
                    errorCode = "B" + (i + 1).ToString() + ApplicationContext.Instance.StringResourceReader.GetString("Info_isEmpty");
                    break;
                }
                if (tempSuite.MdvrId.Trim().Length > 20)
                {
                    errorIndex = i;
                    errorCode = "B" + (i + 1).ToString() + ApplicationContext.Instance.StringResourceReader.GetString("Length_Warning");
                    break;
                }
                if (tempSuiteList.Any(t => t.MdvrId == tempSuite.MdvrId.Trim()))
                {
                    errorIndex = i;
                    errorCode = "B" + (i + 1).ToString() + "  " + ApplicationContext.Instance.StringResourceReader.GetString("ValueTheSame");
                    break;
                }

                tempSuite.MdvrCoreId = uploadContent[i][2].ToString().Trim();//CEIEC ID
                if (tempSuite.MdvrCoreId == string.Empty)
                {
                    errorIndex = i;
                    errorCode = "C" + (i + 1).ToString() + ApplicationContext.Instance.StringResourceReader.GetString("Info_isEmpty");
                    break;
                }
                if (tempSuite.MdvrCoreId.Trim().Length != 10)
                {
                    errorIndex = i;
                    errorCode = "C" + (i + 1).ToString() + ApplicationContext.Instance.StringResourceReader.GetString("Length_Short_Warning");
                    break;
                }
                if (tempSuiteList.Any(t => t.MdvrCoreId == tempSuite.MdvrCoreId.Trim()))
                {
                    errorIndex = i;
                    errorCode = "C" + (i + 1).ToString() + "  " + ApplicationContext.Instance.StringResourceReader.GetString("ValueTheSame");
                    break;
                }

                EnumModel DeviceType = DeviceTypeList.Where(x => x.ShowName == uploadContent[i][3].ToString().Trim()).FirstOrDefault();
                if (DeviceType == null)
                {
                    errorIndex = i;
                    errorCode = "D" + (i + 1).ToString() + ApplicationContext.Instance.StringResourceReader.GetString("Info_isEmpty"); ;
                    break;
                }
                tempSuite.DeviceType = (Gsafety.PTMS.ServiceReference.SecuritySuiteService.VehicleTypeEnum)Enum.Parse(typeof(Gsafety.PTMS.ServiceReference.SecuritySuiteService.VehicleTypeEnum), DeviceType.EnumName, true);

                tempSuite.Camera1Id = uploadContent[i][4].ToString().Trim();//Camera1Id
                if (tempSuite.Camera1Id.Trim().Length > 25)
                {
                    errorIndex = i;
                    errorCode = "E" + (i + 1).ToString() + ApplicationContext.Instance.StringResourceReader.GetString("Length_Warning");
                    break;
                }

                tempSuite.Camera2Id = uploadContent[i][5].ToString().Trim();//Camera2Id
                if (tempSuite.Camera2Id.Trim().Length > 25)
                {
                    errorIndex = i;
                    errorCode = "F" + (i + 1).ToString() + ApplicationContext.Instance.StringResourceReader.GetString("Length_Warning");
                    break;
                }

                tempSuite.Camera3Id = uploadContent[i][6].ToString().Trim();//Camera1Id
                if (tempSuite.Camera3Id.Trim().Length > 25)
                {
                    errorIndex = i;
                    errorCode = "G" + (i + 1).ToString() + ApplicationContext.Instance.StringResourceReader.GetString("Length_Warning");
                    break;
                }

                tempSuite.Camera4Id = uploadContent[i][7].ToString().Trim();//Camera2Id
                if (tempSuite.Camera4Id.Trim().Length > 25)
                {
                    errorIndex = i;
                    errorCode = "H" + (i + 1).ToString() + ApplicationContext.Instance.StringResourceReader.GetString("Length_Warning");
                    break;
                }

                tempSuite.AlarmButton1Id = uploadContent[i][8].ToString().Trim();//AlarmButton1Id
                if (tempSuite.AlarmButton1Id.Trim().Length > 20)
                {
                    errorIndex = i;
                    errorCode = "I" + (i + 1).ToString() + ApplicationContext.Instance.StringResourceReader.GetString("Length_Warning");
                    break;
                }

                tempSuite.DoorSensorId = uploadContent[i][11 - 2].ToString().Trim();//DoorSensorId
                if (tempSuite.DoorSensorId.Trim().Length > 50)
                {
                    errorIndex = i;
                    errorCode = "J" + (i + 1).ToString() + ApplicationContext.Instance.StringResourceReader.GetString("Length_Warning");
                    break;
                }

                tempSuite.SdCardId = uploadContent[i][12 - 2].ToString().Trim();//SdCardId
                if (tempSuite.SdCardId.Trim().Length > 50)
                {
                    errorIndex = i;
                    errorCode = "K" + (i + 1).ToString() + ApplicationContext.Instance.StringResourceReader.GetString("Length_Warning");
                    break;
                }

                tempSuite.UpsId = uploadContent[i][13 - 2].ToString().Trim();//UpsId
                if (tempSuite.UpsId.Trim().Length > 50)
                {
                    errorIndex = i;
                    errorCode = "L" + (i + 1).ToString() + ApplicationContext.Instance.StringResourceReader.GetString("Length_Warning");
                    break;
                }


                tempSuite.MdvrSimId = uploadContent[i][14 - 2].ToString().Trim();//MdvrSimId
                if (tempSuite.MdvrSimId.Trim().Length > 100)
                {
                    errorIndex = i;
                    errorCode = "M" + (i + 1).ToString() + ApplicationContext.Instance.StringResourceReader.GetString("Length_Warning");
                    break;
                }

                tempSuite.MdvrSimPhoneNumber = uploadContent[i][15 - 2].ToString().Trim();//MdvrSimPhoneNumber
                if (tempSuite.MdvrSimPhoneNumber.Trim().Length > 20)
                {
                    errorIndex = i;
                    errorCode = "N" + (i + 1).ToString() + ApplicationContext.Instance.StringResourceReader.GetString("Length_Warning");
                    break;
                }

                tempSuite.SoftwareVersion = uploadContent[i][16 - 2].ToString().Trim();//SoftwareVersion
                if (tempSuite.SoftwareVersion.Trim().Length > 100)
                {
                    errorIndex = i;
                    errorCode = "O" + (i + 1).ToString() + ApplicationContext.Instance.StringResourceReader.GetString("Length_Warning");
                    break;
                }

                tempSuite.Note = uploadContent[i][17 - 2].ToString().Trim();//Note
                if (tempSuite.Note.Trim().Length > 2000)
                {
                    errorIndex = i;
                    errorCode = "P" + (i + 1).ToString() + ApplicationContext.Instance.StringResourceReader.GetString("Length_Warning");
                    break;
                }

                tempSuite.status = DeviceSuiteStatus.Initial;

                tempSuite.InstallStatus = InstallStatusType.UnInstall;

                tempSuiteList.Add(tempSuite);

            }

            return tempSuiteList;
        }

        void suiteClient_CheckSecuritySuiteExistCompleted(object sender, CheckSecuritySuiteExistCompletedEventArgs e)
        {
            try
            {
                if (e.Result.Result == false)
                {
                    suiteClient.BatchAddAsync(new ObservableCollection<DeviceSuite>(uploadList));
                }
                else
                {
                    MessageBox.Show(ApplicationContext.Instance.StringResourceReader.GetString("BASEINFO_DataExisted"), ApplicationContext.Instance.StringResourceReader.GetString("TRAFFIC_TIP"), MessageBoxButton.OK);
                    setUploadBtnStatus(true);
                }
            }
            catch (System.Exception ex)
            {
                setUploadBtnStatus(true);
            }
        }

        void SecuritySuiteClient_BatchAddCompleted(object sender, BatchAddCompletedEventArgs e)
        {
            try
            {
                if (e.Result.Result)
                {
                    MessageBox.Show(ApplicationContext.Instance.StringResourceReader.GetString("BASEINFO_LoadSucess"), ApplicationContext.Instance.StringResourceReader.GetString("TRAFFIC_TIP"), MessageBoxButton.OK);
                    setUploadBtnStatus(true);
                }
                else
                {
                    MessageBox.Show(e.Result.ExceptionMessage.ToString());
                    setUploadBtnStatus(true);
                }
            }
            catch
            {
                setUploadBtnStatus(true);
            }
        }
    }
}
