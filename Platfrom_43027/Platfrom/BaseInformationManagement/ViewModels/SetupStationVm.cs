/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: d5860d61-8414-4244-91f0-9182d876bf3d      
/////             clrversion: 4.0.30319.17929
/////Registered organization: 
/////           Machine Name: PC-LANQ
/////                 Author: TEST(lanq)
/////======================================================================
/////           Project Name: Gsafety.PTMS.BaseInformation.ViewModels
/////    Project Description:    
/////             Class Name: SetupStationVm
/////          Class Version: v1.0.0.0
/////            Create Time: 2013/8/15 10:50:09
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2013/8/15 10:50:09
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
using Jounce.Core.Command;
using Jounce.Framework.Command;
using Jounce.Core.View;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using Gsafety.PTMS.ServiceReference.DistrictService;
using System.Linq;
using Gsafety.PTMS.Share;
using Gsafety.PTMS.ServiceReference.InstallStationService;
using Gsafety.PTMS.ServiceReference.ADGroupService;
using System.ServiceModel;
using Gsafety.PTMS.Spreadsheet;
using System.Collections;
using Gsafety.PTMS.Bases.Models;
using System.Text.RegularExpressions;
using System.IO;
using BaseLib.ViewModels;

namespace Gsafety.PTMS.BaseInformation.ViewModels
{
    [ExportAsViewModel(BaseInformationName.SetupStationVm)]
    public class SetupStationVm : BaseInfoViewModelBase
    {
        #region
        private string districtcode;
        private string stationname;
        private InstallStationServiceClient stationclient = ServiceClientFactory.Create<InstallStationServiceClient>();
        private DistrictServiceClient districtclient = ServiceClientFactory.Create<DistrictServiceClient>();
        ADAccountInfo SetUpStation = new ADAccountInfo();
        ObservableCollection<InstallStation> SetUpStationList = new ObservableCollection<InstallStation>();
        public InstallStation CurrentInstallStation { get; set; }
        GroupServiceClient StationRelated = ServiceClientFactory.Create<GroupServiceClient>();
        public PagedServerCollection<InstallStation> PSC_SetupStation { get; set; }

        public List<District> ProvinceList { get; set; }
        public string ProviceCode { get; set; }
        public List<District> CityList { get; set; }
        public string CityCode { get; set; }
        //public Uri Url { get; private set; }
        public string InstallStationName { get; set; }
        private int pageSizeValue;
        public int PageSizeValue
        {
            get { return pageSizeValue; }
            set
            {
                pageSizeValue = value;
                if (this.PSC_SetupStation != null)
                {
                    this.PSC_SetupStation.PageSize = pageSizeValue;
                }
            }
        }
        //public List<int> PageSizeList { get; set; }
        //public IActionCommand ExportCommand { get; private set; }
        //public IActionCommand UpdateCommand { get; private set; }
        //public IActionCommand DeleteCommand { get; private set; }
        //public IActionCommand QueryCommand { get; private set; }
        //public IActionCommand ViewCommand { get; private set; }
        //public IActionCommand AddCommand { get; private set; }
        //public IActionCommand UploadCommand { get; private set; }
        //public bool UploadBtnStatus { get; set; }
        //private bool isInitialOrQuery;
        //public bool ExportBtnStatus { get; set; }

        protected override void ActivateView(string viewName, System.Collections.Generic.IDictionary<string, object> viewParameters)
        {
            base.ActivateView(viewName, viewParameters);
            PSC_SetupStation.ToPage(currentIndex);
            GetSetUpStationInfo();
            Url = new Uri(BaseInformationCommon.GetTemplateUrl(DownLoadType.setupStation));
            Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => Url));
        }
        private bool ValidatePhone(string value)
        {
            if (!string.IsNullOrEmpty(value) && !BaseInformationCommon.CheckTelephone(value))
            {
                return true;
            }
            else return false;
        }
        private bool ValidateEmail(string value)
        {
            if (!string.IsNullOrEmpty(value) && !Regex.IsMatch(value, "\\w{1,}@\\w{1,}\\.\\w{1,}", RegexOptions.IgnoreCase))
            {
                return true;
            }
            else return false;
        }
        private bool _Isdelete;
        public bool Isdelete
        {
            get { return _Isdelete; }

            set
            {
                _Isdelete = value;

            }
        }
        #endregion
        public SetupStationVm()
        {
            try
            {
                isInitialOrQuery = true;
                ExportBtnStatus = true;
                UploadBtnStatus = true;
                Isdelete = false;
                stationclient.DeleteInstallStationCompleted += installStationClient_DeleteInstallStationCompleted;
                stationclient.BatchAddStationCompleted += stationclient_BatchAddStationCompleted;
                StationRelated.GetAccountInfoByGroupNameCompleted += StationRelated_GetAccountInfoByGroupNameCompleted;
                stationclient.CheckInstallStationExistCompleted += stationclient_CheckInstallStationExistCompleted;
                GetProvinceAndCity();
                PageSizeList = BaseInformationCommon.PageSizeList;
                PageSizeValue = PageSizeList[0];
                InitPagedServerCollection();
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException("SetupStationVm()", ex);
            }
        }

        private int matchnum = 0;
        protected override void InitPagedServerCollection()
        {
            stationclient.GetInstallStationsFuzzyCompleted += installStationClient_GetInstallStationsFuzzyCompleted;
            PSC_SetupStation = new PagedServerCollection<InstallStation>((pageIndex, pageSize) =>
            {
                pageSize = PageSizeValue;
                System.Threading.Interlocked.Exchange(ref currentIndex, pageIndex);
                Gsafety.PTMS.ServiceReference.InstallStationService.PagingInfo pagingInfo = new Gsafety.PTMS.ServiceReference.InstallStationService.PagingInfo() { PageIndex = pageIndex, PageSize = pageSize };
                ServiceClientFactory.CreateMessageHeader(stationclient.InnerChannel);
               // stationclient.GetInstallStationsFuzzyAsync(districtcode, stationname, pagingInfo, ApplicationContext.Instance.AuthenticationInfo.ClientID);
            });
        }

        private void GetSetUpStationInfo()
        {
            string groupName = "SiteManager";
            StationRelated.GetAccountInfoByGroupNameAsync(groupName);
        }

        void StationRelated_GetAccountInfoByGroupNameCompleted(object sender, GetAccountInfoByGroupNameCompletedEventArgs e)
        {
            try
            {
                foreach (var item in e.Result.Result)
                {
                    foreach (var setstation in SetUpStationList.ToList())
                        if (item.OrgCode == setstation.ID)
                        {
                            //setstation.DeleteFlag = false;
                        }
                }
                if (Isdelete == true)
                {
                    if (CurrentInstallStation.Valid == 1)
                    {
                        var result = MessageBox.Show(ApplicationContext.Instance.StringResourceReader.GetString("BASEINFO_DeleteConfirm"), ApplicationContext.Instance.StringResourceReader.GetString("TRAFFIC_TIP"), MessageBoxButton.OKCancel);
                        if (result == MessageBoxResult.OK)
                        {
                            if (CurrentInstallStation != null)
                            {
                                stationclient.DeleteInstallStationAsync(CurrentInstallStation.ID);
                            }
                        }
                    }
                    else
                    {
                        MessageBox.Show(ApplicationContext.Instance.StringResourceReader.GetString("BASEINFO_CanDeleteStation"));
                    }

                    Isdelete = false;
                }
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException("StationRelated_GetAccountInfoByGroupNameCompleted", ex);
            }
        }

        void installStationClient_GetInstallStationsFuzzyCompleted(object sender, GetInstallStationsFuzzyCompletedEventArgs e)
        {
            try
            {
                PSC_SetupStation.loader_Finished(new PagedResult<InstallStation>
                {
                    Count = e.Result.TotalRecord,
                    Items = e.Result.Result,
                    PageIndex = currentIndex,
                });
                SetUpStationList = e.Result.Result;
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
                    //MessageBox.Show(ApplicationContext.Instance.StringResourceReader.GetString("MAINTAIN_DataEmpty"));
                }
                if (isInitialOrQuery)
                {
                    isInitialOrQuery = false;
                }
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException("installStationClient_GetInstallStationsFuzzyCompleted", ex);
            }
        }

        void stationclient_BatchAddStationCompleted(object sender, BatchAddStationCompletedEventArgs e)
        {
            try
            {
                if (e.Result.IsSuccess)
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
            catch (Exception ex)
            {

                setUploadBtnStatus(true);
            }
        }

        protected override void Delete()
        {
            Isdelete = true;
            GetSetUpStationInfo();
        }

        void installStationClient_DeleteInstallStationCompleted(object sender, DeleteInstallStationCompletedEventArgs e)
        {
            PSC_SetupStation.ToPage(currentIndex);
        }



        protected override void Query()
        {
            try
            {
                isInitialOrQuery = true;

                districtcode = string.Empty;
                if (CurrentCity != null && CurrentCity.Code != string.Empty)
                {
                    districtcode = CurrentCity.Code;
                }
                else if (CurrentProvince != null && CurrentProvince.Code != string.Empty)
                {
                    districtcode = CurrentProvince.Code;
                }
                stationname = InstallStationName == null ? string.Empty : InstallStationName.Trim();

                currentIndex = 1;
                PSC_SetupStation.MoveToFirstPage();
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException("SetupStationVm Query", ex);
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
                InstallStationServiceClient client = ServiceClientFactory.Create<InstallStationServiceClient>();
                client.GetInstallStationsFuzzyCompleted += (s, e) =>
                {
                    if (e.Result != null && e.Result.TotalRecord > 0)
                    {
                        List<string> Codes = new List<string>();
                        Codes.Add("Name");
                        Codes.Add("ProvinceName");
                        Codes.Add("CityName");
                        Codes.Add("Address");
                        Codes.Add("Email");
                        Codes.Add("Contact");
                        Codes.Add("ContactPhone");
                        Codes.Add("Director");
                        Codes.Add("DirectorPhone");
                        Codes.Add("Note");

                        List<string> Names = new List<string>();
                        Names.Add(ApplicationContext.Instance.StringResourceReader.GetString("BASEINFO_SetupStation"));
                        Names.Add(ApplicationContext.Instance.StringResourceReader.GetString("BASEINFO_Province"));
                        Names.Add(ApplicationContext.Instance.StringResourceReader.GetString("BASEINFO_City"));
                        Names.Add(ApplicationContext.Instance.StringResourceReader.GetString("BASEINFO_Address"));
                        Names.Add(ApplicationContext.Instance.StringResourceReader.GetString("BASEINFO_EMail"));
                        Names.Add(ApplicationContext.Instance.StringResourceReader.GetString("BASEINFO_ContactPerson"));
                        Names.Add(ApplicationContext.Instance.StringResourceReader.GetString("BASEINFO_Contact"));
                        Names.Add(ApplicationContext.Instance.StringResourceReader.GetString("BASEINFO_Director"));
                        Names.Add(ApplicationContext.Instance.StringResourceReader.GetString("BASEINFO_DirecotrPhone"));
                        Names.Add(ApplicationContext.Instance.StringResourceReader.GetString("Note"));

                        XLSXExporter xlsx = new XLSXExporter();
                        xlsx.Export(e.Result.Result.ToList(), dlg.OpenFile(), Codes, Names);
                        setExportBtnStatus(true);
                        MessageBox.Show(ApplicationContext.Instance.StringResourceReader.GetString("BaseInfo_ExportSucceed"), ApplicationContext.Instance.StringResourceReader.GetString("TRAFFIC_TIP"), MessageBoxButton.OK);
                    }
                    else
                    {
                        MessageBox.Show(ApplicationContext.Instance.StringResourceReader.GetString("BaseInfo_ExportFaild"), ApplicationContext.Instance.StringResourceReader.GetString("TRAFFIC_TIP"), MessageBoxButton.OK);
                    }
                };
                ServiceClientFactory.CreateMessageHeader(client.InnerChannel);

                if (PSC_SetupStation.TotalItemCount > 10000)
                {
                    Gsafety.PTMS.ServiceReference.InstallStationService.PagingInfo pagingInfo = new Gsafety.PTMS.ServiceReference.InstallStationService.PagingInfo() { PageIndex = 1, PageSize = 10000 };
                    client.GetInstallStationsFuzzyAsync(districtcode,null,stationname, pagingInfo,ApplicationContext.Instance.AuthenticationInfo.ClientID);
                }
                else
                {
                    client.GetInstallStationsFuzzyAsync(districtcode,null, stationname, null,ApplicationContext.Instance.AuthenticationInfo.ClientID);
                }
            }
        }
        private List<InstallStation> uploadList = new List<InstallStation>();
        protected override void UploadExcel()
        {
            try
            {
                uploadList = new List<InstallStation>();
                batchCount = 40;
                batchIndex = 1;
                errorIndex = -1;
                errorCode = string.Empty;
                System.IO.FileInfo fileInfo;
                OpenFileDialog openFileDialog = new OpenFileDialog();
                openFileDialog.Filter = " Files(*.xlsm)|*.xlsm|All files(*.*)|*.*";
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
                        matchnum = 0;
                        uploadList = GetUploadList();
                        //全局变量matchnum为1时表示有重复
                        if (matchnum == 0)
                        {
                            if (errorIndex == -1)
                            {
                                setUploadBtnStatus(false);
                                stationclient.CheckInstallStationExistAsync(new ObservableCollection<InstallStation>(uploadList));
                            }
                            else
                            {
                                MessageBox.Show(ApplicationContext.Instance.StringResourceReader.GetString("BASEINFO_ErrorCell") + " " + errorCode, ApplicationContext.Instance.StringResourceReader.GetString("TRAFFIC_TIP"), MessageBoxButton.OK);
                                uploadContent = null;
                            }
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
            PSC_SetupStation.ToPage(currentIndex);
        }

        private List<InstallStation> GetUploadList()
        {
            List<InstallStation> tempStationList = new List<InstallStation>();
            int startIndex = batchIndex;
            //int endIndex = batchIndex + batchCount < uploadContent.Length ? batchIndex + batchCount : uploadContent.Length;
            int endIndex = uploadContent.Length;

            for (int i = startIndex; i < endIndex; i++)
            {
                InstallStation station = new InstallStation();
                station.Name = uploadContent[i][0];
                if (string.IsNullOrEmpty(station.Name))
                {
                    errorIndex = i;
                    errorCode = "A" + (i + 1).ToString() + ApplicationContext.Instance.StringResourceReader.GetString("Info_isEmpty"); ;
                    break;
                }
                if (tempStationList.Any(t => t.Name == station.Name))
                {
                    errorIndex = i;
                    errorCode = "A" + (i + 1).ToString() + "  " + ApplicationContext.Instance.StringResourceReader.GetString("ValueTheSame");
                    break;
                }
                if (station.Name.Trim().Length > 512)
                {
                    errorIndex = i;
                    errorCode = "A" + (i + 1).ToString() + "  " + ApplicationContext.Instance.StringResourceReader.GetString("Length_Warning");
                    break;
                }

                //station.CityCode = ApplicationContext.Instance.BufferManager.DistrictManager.DistrictByAuthority.Where(x => x.Code.Length == 5 && x.Name == uploadContent[i][2].ToString().Trim()).Select(x => x.Code).FirstOrDefault();
                //if (string.IsNullOrEmpty(station.CityCode))
                //{
                //    errorIndex = i;
                //    errorCode = "C" + (i + 1).ToString() + ApplicationContext.Instance.StringResourceReader.GetString("InfoisError"); ;
                //    break;
                //}

                station.Address = uploadContent[i][3].ToString().Trim();
                if (station.Address.Trim().Length > 512)
                {
                    errorIndex = i;
                    errorCode = "D" + (i + 1).ToString() + "  " + ApplicationContext.Instance.StringResourceReader.GetString("Length_Warning");
                    break;
                }

                station.Email = uploadContent[i][4].ToString().Trim();
                if (ValidateEmail(station.Email))
                {
                    errorIndex = i;
                    errorCode = "E" + (i + 1).ToString() + "  " + ApplicationContext.Instance.StringResourceReader.GetString("BASEINFO_EMail") + "\n" + ApplicationContext.Instance.StringResourceReader.GetString("BASEINFO_FormatIllegal");
                    break;
                }
                if (station.Email.Trim().Length > 100)
                {
                    errorIndex = i;
                    errorCode = "E" + (i + 1).ToString() + "  " + ApplicationContext.Instance.StringResourceReader.GetString("Length_Warning");
                    break;
                }

                station.Contact = uploadContent[i][5].ToString().Trim();
                if (station.Contact.Trim().Length > 512)
                {
                    errorIndex = i;
                    errorCode = "F" + (i + 1).ToString() + "  " + ApplicationContext.Instance.StringResourceReader.GetString("Length_Warning");
                    break;
                }

                station.ContactPhone = uploadContent[i][6].ToString().Trim();
                if (ValidatePhone(station.ContactPhone))
                {
                    errorIndex = i;
                    errorCode = "G" + (i + 1).ToString() + "  " + ApplicationContext.Instance.StringResourceReader.GetString("ALARM_Phone") + "\n" + ApplicationContext.Instance.StringResourceReader.GetString("BASEINFO_PhoneRule");
                    break;
                }
                if (station.ContactPhone.Trim().Length > 20)
                {
                    errorIndex = i;
                    errorCode = "G" + (i + 1).ToString() + "  " + ApplicationContext.Instance.StringResourceReader.GetString("Length_Warning");
                    break;
                }

                station.Director = uploadContent[i][7].ToString().Trim();
                if (station.Director.Trim().Length > 512)
                {
                    errorIndex = i;
                    errorCode = "H" + (i + 1).ToString() + "  " + ApplicationContext.Instance.StringResourceReader.GetString("Length_Warning");
                    break;
                }

                station.DirectorPhone = uploadContent[i][8].ToString().Trim();
                if (ValidatePhone(station.DirectorPhone))
                {
                    errorIndex = i;
                    errorCode = "I" + (i + 1).ToString() + "  " + ApplicationContext.Instance.StringResourceReader.GetString("BASEINFO_DirecotrPhone") + "\n" + ApplicationContext.Instance.StringResourceReader.GetString("BASEINFO_PhoneRule");
                    break;
                }
                if (station.DirectorPhone.Trim().Length > 50)
                {
                    errorIndex = i;
                    errorCode = "I" + (i + 1).ToString() + "  " + ApplicationContext.Instance.StringResourceReader.GetString("Length_Warning");
                    break;
                }

                station.Note = uploadContent[i][9].ToString();
                if (station.Note.Trim().Length > 2000)
                {
                    errorIndex = i;
                    errorCode = "J" + (i + 1).ToString() + "  " + ApplicationContext.Instance.StringResourceReader.GetString("Length_Warning");
                    break;
                }

                tempStationList.Add(station);
            }
            return tempStationList;
        }
        private Boolean equalmatch(InstallStation m, InstallStation n)
        {
            if (m == n)
                return true;
            //if (m == null || n == null)
            //    return false;
            //if (m.Address == n.Address && m.CityCode == n.CityCode
            //    && m.CityName == n.CityName && m.Contact == n.Contact && m.ContactPhone == n.ContactPhone
            //    && m.DeleteFlag == n.DeleteFlag && m.Director == n.Director && m.DirectorPhone == n.DirectorPhone
            //    && m.Email == n.Email)
            //    return true;
            else return false;
        }

        void stationclient_CheckInstallStationExistCompleted(object sender, CheckInstallStationExistCompletedEventArgs e)
        {
            try
            {
                if (e.Result.Result == false)
                {
                    stationclient.BatchAddStationAsync(new ObservableCollection<InstallStation>(uploadList));
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

        protected override void Publish(string name)
        {
            EventAggregator.Publish(new ViewNavigationArgs(BaseInformationName.SetupStationManageV, new Dictionary<string, object>() { { "action", name }, { name, CurrentInstallStation } }));
        }

        #region
        private void GetProvinceAndCity()
        {
            ProvinceList = new List<District>();
            ProvinceList.Add(new District { Name = ApplicationContext.Instance.StringResourceReader.GetString("BASEINFO_All"), Code = string.Empty });
            //ApplicationContext.Instance.BufferManager.DistrictManager.DistrictByAuthority.Where(x => x.Code.Length == 2).ToList().ForEach(x => ProvinceList.Add(x));
            ProvinceList.AddRange(ApplicationContext.Instance.BufferManager.DistrictManager.DistrictByAuthority.Where(x => x.Code.Length == 2).ToList().OrderBy(x => x.Name));
            Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => ProvinceList));
            CurrentProvince = ProvinceList[0];
            Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => CurrentProvince));
        }

        private District currentProvince;

        public District CurrentProvince
        {
            get { return currentProvince; }
            set
            {
                currentProvince = value;
                if (currentProvince != null)
                {
                    GetCityByProvinceCode(currentProvince.Code);
                }
            }
        }

        public void GetCityByProvinceCode(string provinceCode)
        {
            CityList = new List<District>();
            CityList.Add(new District { Name = ApplicationContext.Instance.StringResourceReader.GetString("BASEINFO_All"), Code = string.Empty });

            if (!string.IsNullOrEmpty(provinceCode))
            {
                //ApplicationContext.Instance.BufferManager.DistrictManager.DistrictByAuthority.Where(x => x.Code.Length == 5 && x.Code.StartsWith(provinceCode)).ToList().ForEach(x => CityList.Add(x));
                CityList.AddRange(ApplicationContext.Instance.BufferManager.DistrictManager.DistrictByAuthority.Where(x => x.Code.Length == 5 && x.Code.StartsWith(provinceCode)).ToList().OrderBy(x => x.Name));
            }
            Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => CityList));
            CurrentCity = CityList[0];
            Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => CurrentCity));
        }

        private District currentCity;

        public District CurrentCity
        {
            get { return currentCity; }
            set
            {
                currentCity = value;
            }
        }

        #endregion
    }
}
