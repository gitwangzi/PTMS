using BaseLib.ViewModels;
using Gsafety.Common.CommMessage;
using Gsafety.PTMS.BaseInformation;
using Gsafety.PTMS.Share;
using Jounce.Core.Event;
using Jounce.Core.ViewModel;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.Reflection;
using System.Windows;

namespace Gsafety.Ant.BaseInformation.ViewModels
{
    [ExportAsViewModel(BaseInformationName.AntProductBaseInfoMainPageVm)]
    public class AntProductBaseInfoMainPageViewModel : PTMSBaseViewModel
    {
        public AntProductBaseInfoMainPageViewModel()
        {

        }
        #region 属性

        ObservableCollection<MenuInfo> _manageMenus = new ObservableCollection<MenuInfo>();
        /// <summary>
        /// 运营管理
        /// </summary>
        public ObservableCollection<MenuInfo> ManageMenus
        {
            get { return this._manageMenus; }
        }

        #endregion

        private static string BSC_Vehicle = ApplicationContext.Instance.StringResourceReader.GetString("BASEINFO_Vehicle");
        private static string BSC_Driver = ApplicationContext.Instance.StringResourceReader.GetString("Churffure");
        private static string BSC_SafeDevice = ApplicationContext.Instance.StringResourceReader.GetString("MAINTAIN_SecuritySuite");
        private static string BSC_DevGps = ApplicationContext.Instance.StringResourceReader.GetString("BASEINFO_DevGpsManager");
        private static string BASEINFO_SetupStation = ApplicationContext.Instance.StringResourceReader.GetString("BASEINFO_SetupStation");

        protected override void OnInitialUI(ObservableCollection<string> FuncItems)
        {
            try
            {
                var meunVehicle = new MenuInfo("BaseInformationManagement", BSC_Vehicle, "/VehicleDepartmentV");
                var menuDriver = new MenuInfo("BaseInformationManagement", BSC_Driver, "/DriverInfoV");
                var menuSafeDevice = new MenuInfo("BaseInformationManagement", BSC_SafeDevice, "/SafeDeviceInfoV");
                var menuDevGps = new MenuInfo("BaseInformationManagement", BSC_DevGps, "/DevGpsManageViewV");
                var menuInstallStation = new MenuInfo("BaseInformationManagement", BASEINFO_SetupStation, "/InstallPlaceV");

                if (ApplicationContext.Instance.AuthenticationInfo.Role.FuncItems.Contains("02-04-01-01"))
                {
                    this._manageMenus.Add(meunVehicle);
                }
                if (ApplicationContext.Instance.AuthenticationInfo.Role.FuncItems.Contains("02-04-01-02"))
                {
                    this._manageMenus.Add(menuDriver);
                }
                if (ApplicationContext.Instance.AuthenticationInfo.Role.FuncItems.Contains("02-04-01-03"))
                {
                    this._manageMenus.Add(menuSafeDevice);
                }
                if (ApplicationContext.Instance.AuthenticationInfo.Role.FuncItems.Contains("02-04-01-04"))
                {
                    this._manageMenus.Add(menuDevGps);
                }
                if (ApplicationContext.Instance.AuthenticationInfo.Role.FuncItems.Contains("02-04-01-05"))
                {
                    this._manageMenus.Add(menuInstallStation);
                }

                Jounce.Framework.JounceHelper.ExecuteOnUI(() =>
                {
                    RaisePropertyChanged(() => this.ManageMenus);
                });
            }
            catch (System.Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(), ex);
            }
        }
    }
}
