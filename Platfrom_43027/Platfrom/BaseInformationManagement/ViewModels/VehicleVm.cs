/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: 9a63edf1-c587-47ef-a2b7-8df88e257d5d      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: LIN-20130409ZRS
/////                 Author: TEST(zhujf)
/////======================================================================
/////           Project Name: Gsafety.PTMS.BaseInformation.ViewModels
/////    Project Description:    
/////             Class Name: VehicleVm
/////          Class Version: v1.0.0.0
/////            Create Time: 2013/8/13 16:15:51
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2013/8/13 16:15:51
/////            Modified by:
/////   Modified Description: 
/////======================================================================
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Linq;
using System.IO;
using System.Collections;
using Gsafety.PTMS.Bases.Models;
using Gsafety.PTMS.Spreadsheet;
using Gsafety.PTMS.ServiceReference.DistrictService;
using Gsafety.PTMS.ServiceReference.VehicleService;
using Gsafety.PTMS.Share;
using Jounce.Core.Command;
using Jounce.Core.View;
using Jounce.Core.ViewModel;
using Jounce.Framework.Command;
using System.Text.RegularExpressions;
using BaseLib.ViewModels;

namespace Gsafety.PTMS.BaseInformation.ViewModels
{
    [ExportAsViewModel(BaseInformationName.VehicleVm)]
    public class VehicleVm : BaseInfoViewModelBase
    {
        #region
        private string companyId;
        private string vehicleId;
        private string owner;
        private Gsafety.PTMS.ServiceReference.VehicleService.InstallStatusType status;
        private Gsafety.PTMS.ServiceReference.VehicleService.VehicleTypeEnum? type;

        private VehicleServiceClient vehicleClient = ServiceClientFactory.Create<VehicleServiceClient>();
        public Gsafety.PTMS.ServiceReference.VehicleService.Vehicle CurrentVehicle { get; set; }
        public PagedServerCollection<Gsafety.PTMS.ServiceReference.VehicleService.Vehicle> PSC_Vehicle { get; set; }

        public List<District> ProvinceList { get; set; }
        public List<District> CityList { get; set; }


        private int pageSizeValue;
        public int PageSizeValue
        {
            get { return pageSizeValue; }
            set
            {
                pageSizeValue = value;
                if (this.PSC_Vehicle != null)
                {
                    this.PSC_Vehicle.PageSize = pageSizeValue;
                }
            }
        }

        public string VehicleId { get; set; }
        public string VehicleOwner { get; set; }

        private List<EnumModel> VehicleTypeList { get; set; }
        private List<EnumModel> VehicleSeviceTypeList { get; set; }

        private void InitialVehicleType()
        {
            VehicleTypeList = new List<EnumModel>();
            Enum.GetNames(typeof(Gsafety.PTMS.ServiceReference.VehicleService.VehicleType)).ToList().ForEach(x =>
            {
                EnumModel item = new EnumModel { EnumName = x, ShowName = ApplicationContext.Instance.StringResourceReader.GetString(x) };
                VehicleTypeList.Add(item);
            });
        }

        private void InitialVehicleSeviceType()
        {
            VehicleSeviceTypeList = new List<EnumModel>();
            Enum.GetNames(typeof(Gsafety.PTMS.ServiceReference.VehicleService.VehicleSeviceType)).ToList().ForEach(x =>
            {
                EnumModel item = new EnumModel { EnumName = x, ShowName = ApplicationContext.Instance.StringResourceReader.GetString(x) };
                VehicleSeviceTypeList.Add(item);
            });
        }

        private void InitialInstallStatus()
        {
            InstallStatusList = new List<EnumModel>();
            InstallStatusList.Add(new EnumModel { ShowName = ApplicationContext.Instance.StringResourceReader.GetString("BASEINFO_PleaseSelect"), EnumName = string.Empty });
            Enum.GetNames(typeof(Gsafety.PTMS.ServiceReference.VehicleService.InstallStatusType)).ToList().ForEach(x =>
            {
                EnumModel item = new EnumModel { EnumName = x, ShowName = ApplicationContext.Instance.StringResourceReader.GetString(x) };
                InstallStatusList.Add(item);
            });
        }

        public EnumModel CurrentType { get; set; }
        public List<EnumModel> TypeList { get; set; }
        private void InitialType()
        {
            TypeList = new List<EnumModel>();
            TypeList.Add(new EnumModel { ShowName = ApplicationContext.Instance.StringResourceReader.GetString("BASEINFO_PleaseSelect"), EnumName = string.Empty });
            Enum.GetNames(typeof(Gsafety.PTMS.ServiceReference.VehicleService.VehicleType)).ToList().ForEach(x =>
            {
                EnumModel item = new EnumModel { EnumName = x, ShowName = ApplicationContext.Instance.StringResourceReader.GetString(x) };
                TypeList.Add(item);
            });
        }


        protected override void ActivateView(string viewName, System.Collections.Generic.IDictionary<string, object> viewParameters)
        {
            base.ActivateView(viewName, viewParameters);
            if (viewParameters.Count != 0 && viewParameters["action"].ToString() == "return")
            {
                PSC_Vehicle.ToPage(currentIndex);
            }

            Url = new Uri(BaseInformationCommon.GetTemplateUrl(DownLoadType.Vehicle));
            Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => Url));
        }

        private bool ValidateEmail(string value)
        {
            if (!string.IsNullOrEmpty(value) && !Regex.IsMatch(value, "\\w{1,}@\\w{1,}\\.\\w{1,}", RegexOptions.IgnoreCase))
            {
                return true;
            }
            else return false;
        }


        #endregion
        public VehicleVm()
        {
            try
            {
                isInitialOrQuery = true;
                UploadBtnStatus = true;
                ExportBtnStatus = true;
                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => UploadBtnStatus));
                vehicleClient.BatchAddCompleted += vehicleClient_BatchAddCompleted;
                vehicleClient.DeleteVehicleCompleted += VehicleClient_DeleteVehicleCompleted;
                vehicleClient.CheckVehicleExistCompleted += vehicleClient_CheckVehicleExistCompleted;


                InitialVehicleType();
                InitialVehicleSeviceType();
                InitialInstallStatus();
                CurrentInstallStatus = InstallStatusList[0];
                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => CurrentInstallStatus));

                InitialType();
                CurrentType = TypeList[0];
                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => CurrentType));
                vehicleClient.GetVehiclesFuzzyCompleted += vehicleClient_GetVehiclesFuzzyCompleted;
                PageSizeList = BaseInformationCommon.PageSizeList;
                PageSizeValue = PageSizeList[0];
                InitPagedServerCollection();
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException("VehicleVM()", ex);
            }
        }

        protected override void InitPagedServerCollection()
        {
            try
            {
                PSC_Vehicle = new PagedServerCollection<Gsafety.PTMS.ServiceReference.VehicleService.Vehicle>((pageIndex, pageSize) =>
                {
                    pageSize = PageSizeValue;
                    System.Threading.Interlocked.Exchange(ref currentIndex, pageIndex);
                    Gsafety.PTMS.ServiceReference.VehicleService.PagingInfo pagingInfo = new Gsafety.PTMS.ServiceReference.VehicleService.PagingInfo() { PageIndex = pageIndex, PageSize = pageSize };
                    ServiceClientFactory.CreateMessageHeader(vehicleClient.InnerChannel);
                    vehicleClient.GetVehiclesFuzzyAsync(vehicleId, null, type, owner, status, pagingInfo);
                });
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException("InitPagedServerCollection()", ex);
            }
        }

        void vehicleClient_GetVehiclesFuzzyCompleted(object sender, GetVehiclesFuzzyCompletedEventArgs e)
        {

            try
            {
                PSC_Vehicle.loader_Finished(new PagedResult<Gsafety.PTMS.ServiceReference.VehicleService.Vehicle>
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
                ApplicationContext.Instance.Logger.LogException("vehicleClient_GetVehiclesFuzzyCompleted()", ex);
            }
        }

        protected override void Delete()
        {
            var result = MessageBox.Show(ApplicationContext.Instance.StringResourceReader.GetString("BASEINFO_DeleteConfirm"), ApplicationContext.Instance.StringResourceReader.GetString("TRAFFIC_TIP"), MessageBoxButton.OKCancel);
            if (result == MessageBoxResult.OK)
            {
                if (CurrentVehicle != null)
                {
                    vehicleClient.DeleteVehicleAsync(CurrentVehicle);
                }
            }
        }

        private void VehicleClient_DeleteVehicleCompleted(object sender, System.ComponentModel.AsyncCompletedEventArgs e)
        {
            PSC_Vehicle.ToPage(currentIndex);
        }

        protected override void Query()
        {
            isInitialOrQuery = true;

            companyId = string.Empty;

            vehicleId = VehicleId == null ? string.Empty : VehicleId.Trim();
            owner = VehicleOwner == null ? string.Empty : VehicleOwner.Trim();
            status = InstallStatusType.Installed;
            if (CurrentInstallStatus != null && CurrentInstallStatus.EnumName != string.Empty)
            {
                status = (Gsafety.PTMS.ServiceReference.VehicleService.InstallStatusType)Enum.Parse(typeof(Gsafety.PTMS.ServiceReference.VehicleService.InstallStatusType), CurrentInstallStatus.EnumName, true);
            }
            type = null;
            if (CurrentType != null && CurrentType.EnumName != string.Empty)
            {
                type = (Gsafety.PTMS.ServiceReference.VehicleService.VehicleTypeEnum)Enum.Parse(typeof(Gsafety.PTMS.ServiceReference.VehicleService.VehicleType), CurrentType.EnumName, true);
            }

            currentIndex = 1;
            PSC_Vehicle.MoveToFirstPage();
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
                VehicleServiceClient client = ServiceClientFactory.Create<VehicleServiceClient>();
                client.GetVehiclesFuzzyCompleted += (s, e) =>
                {
                    if (e.Result != null && e.Result.TotalRecord > 0)
                    {
                        List<string> Codes = new List<string>();
                        Codes.Add("InstallStatus");
                        Codes.Add("VehicleId");
                        //Codes.Add("Type");
                        Codes.Add("ServerType");
                        Codes.Add("ProvinceName");
                        Codes.Add("CityName");
                        Codes.Add("VehicleSn");
                        Codes.Add("EngineId");
                        Codes.Add("BrandModel");
                        Codes.Add("Region");
                        Codes.Add("OperatingLicense");
                        Codes.Add("StartYear");

                        Codes.Add("Owner");
                        Codes.Add("OwnerId");
                        Codes.Add("OwnerPhone");
                        Codes.Add("OwnerEmail");
                        Codes.Add("OwnerAddress");
                        Codes.Add("Note");

                        List<string> Names = new List<string>();
                        Names.Add(ApplicationContext.Instance.StringResourceReader.GetString("BASEINFO_InstallStatus"));
                        Names.Add(ApplicationContext.Instance.StringResourceReader.GetString("BASEINFO_VehicleNumber"));
                        Names.Add(ApplicationContext.Instance.StringResourceReader.GetString("BASEINFO_ServiceType"));
                        Names.Add(ApplicationContext.Instance.StringResourceReader.GetString("BASEINFO_Province"));
                        Names.Add(ApplicationContext.Instance.StringResourceReader.GetString("BASEINFO_City"));

                        Names.Add(ApplicationContext.Instance.StringResourceReader.GetString("BASEINFO_VehicleSN"));
                        Names.Add(ApplicationContext.Instance.StringResourceReader.GetString("BASEINFO_EngineID"));
                        Names.Add(ApplicationContext.Instance.StringResourceReader.GetString("BASEINFO_VehicleBrandModel"));
                        Names.Add(ApplicationContext.Instance.StringResourceReader.GetString("BASEINFO_Region"));
                        Names.Add(ApplicationContext.Instance.StringResourceReader.GetString("BASEINFO_OperatingLicense"));
                        Names.Add(ApplicationContext.Instance.StringResourceReader.GetString("BASEINFO_StartYear"));

                        Names.Add(ApplicationContext.Instance.StringResourceReader.GetString("BASEINFO_VehicleOwner"));
                        Names.Add(ApplicationContext.Instance.StringResourceReader.GetString("BASEINFO_IdentityID"));
                        Names.Add(ApplicationContext.Instance.StringResourceReader.GetString("BASEINFO_Phone"));
                        Names.Add(ApplicationContext.Instance.StringResourceReader.GetString("BASEINFO_EMail"));
                        Names.Add(ApplicationContext.Instance.StringResourceReader.GetString("BASEINFO_Address"));
                        Names.Add(ApplicationContext.Instance.StringResourceReader.GetString("Note"));

                        List<EnumsEx> eList = new List<EnumsEx>();

                        List<FieldEx> FieldEx2 = new List<FieldEx>();
                        Enum.GetNames(typeof(Gsafety.PTMS.ServiceReference.VehicleService.VehicleSeviceType)).ToList().ForEach(x =>
                        {
                            FieldEx item = new FieldEx { Key = x, Value = ApplicationContext.Instance.StringResourceReader.GetString(x) };
                            FieldEx2.Add(item);
                        });
                        eList.Add(new EnumsEx { Code = "ServerType", Content = FieldEx2 });

                        List<FieldEx> FieldEx3 = new List<FieldEx>();
                        Enum.GetNames(typeof(Gsafety.PTMS.ServiceReference.VehicleService.InstallStatusType)).ToList().ForEach(x =>
                        {
                            FieldEx item = new FieldEx { Key = x, Value = ApplicationContext.Instance.StringResourceReader.GetString(x) };
                            FieldEx3.Add(item);
                        });
                        eList.Add(new EnumsEx { Code = "InstallStatus", Content = FieldEx3 });

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
                ServiceClientFactory.CreateMessageHeader(client.InnerChannel);

                if (PSC_Vehicle.TotalItemCount > 10000)
                {
                    Gsafety.PTMS.ServiceReference.VehicleService.PagingInfo pagingInfo = new Gsafety.PTMS.ServiceReference.VehicleService.PagingInfo() { PageIndex = 1, PageSize = 10000 };
                    client.GetVehiclesFuzzyAsync(vehicleId, null, type, owner, status, pagingInfo);
                }
                else
                {
                    client.GetVehiclesFuzzyAsync(vehicleId, null, type, owner, status, null);
                }
            }
        }

        protected override void Publish(string name)
        {
            EventAggregator.Publish(new ViewNavigationArgs(BaseInformationName.VehicleManageV, new Dictionary<string, object>() { { "action", name }, { name, CurrentVehicle } }));
        }

        private List<Gsafety.PTMS.ServiceReference.VehicleService.Vehicle> uploadList = new List<Gsafety.PTMS.ServiceReference.VehicleService.Vehicle>();
        protected override void UploadExcel()
        {
            try
            {
                uploadList = new List<ServiceReference.VehicleService.Vehicle>();
                batchCount = 40;
                batchIndex = 1;
                errorIndex = -1;
                errorCode = string.Empty;
                System.IO.FileInfo fileInfo;
                OpenFileDialog openFileDialog = new OpenFileDialog();
                openFileDialog.Filter = "Files(*.xlsm)|*.xlsm|All files(*.*)|*.*";
                if (openFileDialog.ShowDialog() == true)
                {
                    fileInfo = openFileDialog.File;
                    if (!fileInfo.Name.EndsWith("xlsm"))
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
                            vehicleClient.CheckVehicleExistAsync(new ObservableCollection<Gsafety.PTMS.ServiceReference.VehicleService.Vehicle>(uploadList));
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
            PSC_Vehicle.ToPage(currentIndex);
        }

        private List<Gsafety.PTMS.ServiceReference.VehicleService.Vehicle> GetUploadList()
        {
            List<Gsafety.PTMS.ServiceReference.VehicleService.Vehicle> tempVehicleList = new List<Gsafety.PTMS.ServiceReference.VehicleService.Vehicle>();
            int startIndex = batchIndex;
            int endIndex = uploadContent.Length;
            for (int i = startIndex; i < endIndex; i++)
            {
                Gsafety.PTMS.ServiceReference.VehicleService.Vehicle tempVehicle = new Gsafety.PTMS.ServiceReference.VehicleService.Vehicle();

                tempVehicle.VehicleId = uploadContent[i][0].ToString().Trim();
                if (tempVehicle.VehicleId == string.Empty)
                {
                    errorIndex = i;
                    errorCode = "A" + (i + 1).ToString() + ApplicationContext.Instance.StringResourceReader.GetString("Info_isEmpty");
                    break;
                }
                if (tempVehicle.VehicleId.Trim().Length > 20)
                {
                    errorIndex = i;
                    errorCode = "A" + (i + 1).ToString() + "  " + ApplicationContext.Instance.StringResourceReader.GetString("Length_Warning");
                    break;
                }
                if (tempVehicleList.Any(t => t.VehicleId == tempVehicle.VehicleId.Trim()))
                {
                    errorIndex = i;
                    errorCode = "A" + (i + 1).ToString() + "  " + ApplicationContext.Instance.StringResourceReader.GetString("ValueTheSame");
                    break;
                }

                //tempVehicle.Type = VehicleTypeEnum.Bus;

                EnumModel VehicleSeviceType = VehicleSeviceTypeList.Where(x => x.ShowName == uploadContent[i][1].ToString().Trim()).FirstOrDefault();
                if (VehicleSeviceType == null)
                {
                    errorIndex = i;
                    errorCode = "B" + (i + 1).ToString() + ApplicationContext.Instance.StringResourceReader.GetString("Info_isEmpty");
                    break;
                }
                tempVehicle.ServiceType = (Gsafety.PTMS.ServiceReference.VehicleService.VehicleSeviceType)Enum.Parse(typeof(Gsafety.PTMS.ServiceReference.VehicleService.VehicleSeviceType), VehicleSeviceType.EnumName, true);

                tempVehicle.CityCode = ApplicationContext.Instance.BufferManager.DistrictManager.DistrictByAuthority.Where(x => x.Code.Length == 5 && x.Name == uploadContent[i][3].ToString().Trim()).Select(x => x.Code).FirstOrDefault();
                if (tempVehicle.CityCode == null)
                {
                    errorIndex = i;
                    errorCode = "D" + (i + 1).ToString() + ApplicationContext.Instance.StringResourceReader.GetString("Info_isEmpty");
                    break;

                }
                tempVehicle.VehicleSn = uploadContent[i][4].ToString().Trim();
                if (tempVehicle.VehicleSn == string.Empty)
                {
                    errorIndex = i;
                    errorCode = "E" + (i + 1).ToString() + ApplicationContext.Instance.StringResourceReader.GetString("Info_isEmpty");
                    break;
                }
                if (tempVehicleList.Any(t => t.VehicleSn == tempVehicle.VehicleSn.Trim()))
                {
                    errorIndex = i;
                    errorCode = "E" + (i + 1).ToString() + "  " + ApplicationContext.Instance.StringResourceReader.GetString("ValueTheSame");
                    break;
                }

                if (tempVehicle.VehicleSn.Trim().Length > 20)
                {
                    errorIndex = i;
                    errorCode = "E" + (i + 1).ToString() + "  " + ApplicationContext.Instance.StringResourceReader.GetString("Length_Warning");
                    break;
                }

                tempVehicle.EngineId = uploadContent[i][5].ToString().Trim();
                if (tempVehicle.EngineId.Trim().Length > 20)
                {
                    errorIndex = i;
                    errorCode = "F" + (i + 1).ToString() + "  " + ApplicationContext.Instance.StringResourceReader.GetString("Length_Warning");
                    break;
                }

                tempVehicle.BrandModel = uploadContent[i][6].ToString().Trim();
                if (tempVehicle.BrandModel == string.Empty || tempVehicle.BrandModel == null)
                {
                    errorIndex = i;
                    errorCode = "G" + (i + 1).ToString() + ApplicationContext.Instance.StringResourceReader.GetString("Info_isEmpty");
                    break;
                }
                if (tempVehicle.BrandModel.Trim().Length > 100)
                {
                    errorIndex = i;
                    errorCode = "G" + (i + 1).ToString() + "  " + ApplicationContext.Instance.StringResourceReader.GetString("Length_Warning");
                    break;
                }

                tempVehicle.Owner = uploadContent[i][10].ToString().Trim();
                if (string.IsNullOrEmpty(tempVehicle.Owner))
                {
                    errorIndex = i;
                    errorCode = "K" + (i + 1).ToString();
                    break;
                }
                if (tempVehicle.Owner.Trim().Length > 50)
                {
                    errorIndex = i;
                    errorCode = "K" + (i + 1).ToString() + "  " + ApplicationContext.Instance.StringResourceReader.GetString("Length_Warning");
                    break;
                }

                tempVehicle.Region = uploadContent[i][7].ToString().Trim();
                if (tempVehicle.Region.Trim().Length > 512)
                {
                    errorIndex = i;
                    errorCode = "H" + (i + 1).ToString() + "  " + ApplicationContext.Instance.StringResourceReader.GetString("Length_Warning");
                    break;
                }

                tempVehicle.OperationLicense = uploadContent[i][8].ToString().Trim();
                if (tempVehicle.OperationLicense.Trim().Length > 100)
                {
                    errorIndex = i;
                    errorCode = "I" + (i + 1).ToString() + "  " + ApplicationContext.Instance.StringResourceReader.GetString("Length_Warning");
                    break;
                }

                tempVehicle.StartYear = uploadContent[i][9].ToString().Trim();
                if (!string.IsNullOrEmpty(tempVehicle.StartYear))
                {
                    int year;
                    bool ret = int.TryParse(tempVehicle.StartYear, out year);
                    if (ret)
                    {
                        if (year > 2050 || year < 1950)
                        {
                            errorIndex = i;
                            errorCode = "J" + (i + 1).ToString() + "  " + ApplicationContext.Instance.StringResourceReader.GetString("BASEINFO_StartYear") + "\n" + ApplicationContext.Instance.StringResourceReader.GetString("BASEINFO_StartYearRule");
                            break;
                        }
                    }
                    else
                    {
                        errorIndex = i;
                        errorCode = "J" + (i + 1).ToString() + "  " + ApplicationContext.Instance.StringResourceReader.GetString("BASEINFO_StartYear") + "\n" + ApplicationContext.Instance.StringResourceReader.GetString("BASEINFO_StartYearRule");
                        break;

                    }

                }


                tempVehicle.Contact = uploadContent[i][11].ToString().Trim();
                ///身份证目前不验证
                //if (tempVehicle.OwnerId != string.Empty && !BaseInformationCommon.CheckIdentity(tempVehicle.OwnerId))
                //{
                //    errorIndex = i;
                //    errorCode = "L" + (i + 1).ToString() + "  " + ApplicationContext.Instance.StringResourceReader.GetString("BASEINFO_IdentityID") + "\n" + ApplicationContext.Instance.StringResourceReader.GetString("BASEINFO_FormatIllegal");
                //    break;
                //}
                if (tempVehicle.Contact.Trim().Length > 50)
                {
                    errorIndex = i;
                    errorCode = "L" + (i + 1).ToString() + "  " + ApplicationContext.Instance.StringResourceReader.GetString("Length_Warning");
                    break;
                }

                tempVehicle.ContactPhone = uploadContent[i][12].ToString().Trim();
                if (string.IsNullOrEmpty(tempVehicle.ContactPhone))
                {
                    errorIndex = i;
                    errorCode = "M" + (i + 1).ToString();
                    break;
                }
                if (tempVehicle.ContactPhone != string.Empty && !BaseInformationCommon.CheckTelephone(tempVehicle.ContactPhone))
                {
                    errorIndex = i;
                    errorCode = "M" + (i + 1).ToString() + "  " + ApplicationContext.Instance.StringResourceReader.GetString("BASEINFO_Phone") + "\n" + ApplicationContext.Instance.StringResourceReader.GetString("BASEINFO_PhoneRule");
                    break;
                }

                tempVehicle.ContactEmail = uploadContent[i][13].ToString().Trim();
                if (ValidateEmail(tempVehicle.ContactEmail))
                {
                    errorIndex = i;
                    errorCode = "N" + (i + 1).ToString() + "  " + ApplicationContext.Instance.StringResourceReader.GetString("BASEINFO_EMail") + "\n" + ApplicationContext.Instance.StringResourceReader.GetString("BASEINFO_FormatIllegal");
                    break;
                }
                if (tempVehicle.ContactEmail.Trim().Length > 100)
                {
                    errorIndex = i;
                    errorCode = "N" + (i + 1).ToString() + "  " + ApplicationContext.Instance.StringResourceReader.GetString("Length_Warning");
                    break;
                }

                tempVehicle.ContactAddress = uploadContent[i][14].ToString().Trim();
                if (tempVehicle.ContactAddress.Trim().Length > 512)
                {
                    errorIndex = i;
                    errorCode = "O" + (i + 1).ToString() + "  " + ApplicationContext.Instance.StringResourceReader.GetString("Length_Warning");
                    break;
                }

                tempVehicle.InstallStatus = Gsafety.PTMS.ServiceReference.VehicleService.InstallStatusType.UnInstall;

                tempVehicleList.Add(tempVehicle);
            }

            return tempVehicleList;
        }

        void vehicleClient_CheckVehicleExistCompleted(object sender, CheckVehicleExistCompletedEventArgs e)
        {
            try
            {
                if (e.Result.IsSuccess && e.Result.Result == false)
                {
                    vehicleClient.BatchAddAsync(new ObservableCollection<Gsafety.PTMS.ServiceReference.VehicleService.Vehicle>(uploadList));
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

        void vehicleClient_BatchAddCompleted(object sender, Gsafety.PTMS.ServiceReference.VehicleService.BatchAddCompletedEventArgs e)
        {
            try
            {
                if (e.Result.IsSuccess && e.Result.Result)
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
