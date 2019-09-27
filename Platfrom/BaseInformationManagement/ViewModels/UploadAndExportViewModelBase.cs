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
using BaseLib.ViewModels;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Gsafety.PTMS.BaseInformation;
using Jounce.Core.Command;
using Jounce.Framework.Command;

namespace Gsafety.Ant.BaseInformation.ViewModels
{
    public class UploadAndExportViewModelBase<T> : ListViewModel<T>
    {
        public IActionCommand UploadCommand { get; protected set; }
        public IActionCommand ExportCommand { get; protected set; }
        protected int batchIndex;
        protected int errorIndex;
        protected StringBuilder errorCode;
        protected string[][] uploadContent;
        public Uri Url { get; protected set; }
        public bool UploadBtnStatus { get; protected set; }
        public bool ExportBtnStatus { get; set; }

        public UploadAndExportViewModelBase()
        {
            errorCode = new StringBuilder();
            UploadCommand = new ActionCommand<object>(obj => UploadAction());
            ExportCommand = new ActionCommand<object>(obj => ExportAction());
        }

        protected virtual void UploadAction() { }
        protected virtual void ExportAction() { }

        protected void GoNextPage()
        {
            Data.ToPage(currentIndex);
        }

        public string GetTemplateUrl(DownLoadType type)
        {
            var result = Application.Current.Host.Source.AbsoluteUri.Split('/');
            string folderUrl = string.Join("/", result.Take(result.Length - 2)) + "/DownTemplate/";
            string currentUrl = string.Empty;
            string CurrentUICulture = System.Threading.Thread.CurrentThread.CurrentUICulture.Name;
            switch (CurrentUICulture)
            {
                case "zh-CN":
                    switch (type)
                    {
                        case DownLoadType.Vehicle:
                            currentUrl = folderUrl + "车辆.xlsm";
                            break;
                        case DownLoadType.Suite:
                            currentUrl = folderUrl + "安全套件.xlsx";
                            break;
                        case DownLoadType.setupStation:
                            currentUrl = folderUrl + "安装点.xlsm";
                            break;
                        case DownLoadType.GPSDevice:
                            currentUrl = folderUrl + "定位设备.xlsx";
                            break;
                        case DownLoadType.Driver:
                            currentUrl = folderUrl + "驾驶员.xlsx";
                            break;
                        default:
                            break;
                    }
                    break;
                case "en-US":
                    switch (type)
                    {
                        case DownLoadType.Vehicle:
                            currentUrl = folderUrl + "Vehicle.xlsm";
                            break;
                        case DownLoadType.Suite:
                            currentUrl = folderUrl + "SecuritySuite.xlsx";
                            break;
                        case DownLoadType.setupStation:
                            currentUrl = folderUrl + "SetupStation.xlsm";
                            break;
                        case DownLoadType.GPSDevice:
                            currentUrl = folderUrl + "GPSDevice.xlsx";
                            break;
                        case DownLoadType.Driver:
                            currentUrl = folderUrl + "Drivers.xlsx";
                            break;
                        default:
                            break;
                    }
                    break;
                case "es-ES":
                    switch (type)
                    {
                        case DownLoadType.Vehicle:
                            currentUrl = folderUrl + "ElVehicle.xlsm";
                            break;
                        case DownLoadType.Suite:
                            currentUrl = folderUrl + "LaSuitdeSeguridad.xlsx";
                            break;
                        case DownLoadType.setupStation:
                            currentUrl = folderUrl + "Elpuntodemontaje.xlsm";
                            break;
                        case DownLoadType.GPSDevice:
                            currentUrl = folderUrl + "LocalizaciondeEquipos.xlsx";
                            break;
                        case DownLoadType.Driver:
                            currentUrl = folderUrl + "Conductor.xlsx";
                            break;
                        default:
                            break;
                    }
                    break;
            }
            return currentUrl;
        }

        protected void setUploadBtnStatus(bool Flag)
        {
            UploadBtnStatus = Flag;
            Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => UploadBtnStatus));
            if (Flag)
            {
                uploadContent = null;
                GoNextPage();
            }
        }

        protected void setExportBtnStatus(bool Flag)
        {
            ExportBtnStatus = Flag;
            Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => ExportBtnStatus));
        }

        protected bool ValidatePhone(string value)
        {
            bool flag = true;
            if (!string.IsNullOrEmpty(value))
            {
                int num = 0;
                char[] cc = value.ToCharArray();
                foreach (var item in cc)
                {
                    flag = flag && int.TryParse(item.ToString(), out num);
                }
            }

            return flag;
        }

        protected bool ValidateEmail(string value)
        {
            if (!string.IsNullOrEmpty(value) && !Regex.IsMatch(value, "\\w{1,}@\\w{1,}\\.\\w{1,}", RegexOptions.IgnoreCase))
            {
                return true;
            }
            else return false;
        }

        protected string ConvertColumnIndexToStr(int columnIndex)
        {
            var name = "";
            var aCount = columnIndex / 26;
            if (aCount > 0)
            {
                name += (char)(aCount - 1 + 65);
            }

            var left = columnIndex % 26;
            name = name + (char)(left + 65);

            return name;
        }
    }
}
