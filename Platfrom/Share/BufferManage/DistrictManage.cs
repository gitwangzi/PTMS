/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: 2ba31782-36a4-4217-a99d-eeb4306b9d2f      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: PC-DENGZL
/////                 Author: TEST(dengzl)
/////======================================================================
/////           Project Name: Gsafety.PTMS.Share
/////    Project Description:    
/////             Class Name: DistrictManage
/////          Class Version: v1.0.0.0
/////            Create Time: 9/2/2013 1:42:20 PM
/////      Class Description:  
/////======================================================================
/////          Modified Time: 9/2/2013 1:42:20 PM
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
using System.Collections.ObjectModel;
using System.Linq;
using Gsafety.PTMS.ServiceReference.DistrictService;
using System.Collections.Generic;
using Gsafety.PTMS.ServiceReference.VehicleService;
using Jounce.Core.Model;
using System.ComponentModel.Composition;
using Jounce.Core.Event;
using Common;
using System.IO;
using System.Runtime.Serialization;
using Gsafety.PTMS.Bases.Librarys;
using Gsafety.PTMS.ServiceReference.MessageService;
using System.Threading.Tasks;
using Jounce.Framework;
using System.ComponentModel;
using Gsafety.PTMS.Bases.Enums;
using Gsafety.PTMS.Bases.Models;
using Gsafety.PTMS.ServiceReference.OrganizationService;
using System.Reflection;
using Gsafety.PTMS.ServiceReference.RunMonitorGroupService;

namespace Gsafety.PTMS.Share
{
    public delegate void MoniterVehicle(Gsafety.PTMS.Bases.Models.Vehicle vehicle);

    public class DistrictManage : BaseNotify
    {

        private ObservableCollection<Gsafety.PTMS.Bases.Models.Province> _Provinces;
        private ObservableCollection<Gsafety.PTMS.Bases.Models.City> _Cities;

        private AsyncOperation asyncOper;

        private bool _hasLoadData = true;

        private double _OnlineRate = 0;

        public Action<Gsafety.PTMS.Bases.Models.Vehicle> Loaded_Event { get; set; }

        public ObservableCollection<Gsafety.PTMS.Bases.Models.Province> Provinces
        {
            get
            {
                if (!_hasLoadData)
                    GetDistrictInfo();
                return _Provinces;
            }
        }

        public ObservableCollection<Gsafety.PTMS.Bases.Models.City> Cities
        {
            get { return _Cities; }
        }

        public List<District> DistrictByAuthority { get; set; }

        public DistrictManage()
        {
            CompositionInitializer.SatisfyImports(this);
            asyncOper = AsyncOperationManager.CreateOperation(null);
            _Provinces = new ObservableCollection<Gsafety.PTMS.Bases.Models.Province>();
            _Cities = new ObservableCollection<Gsafety.PTMS.Bases.Models.City>();

        }

        public void DataLoading()
        {
            GetDistrictInfo();
        }

        /// <summary>
        /// No access to city information in accordance with Gsafety.PTMS.Bases.Models.City
        /// </summary>
        /// <param name="cityCode"></param>
        /// <returns></returns>
        public Gsafety.PTMS.Bases.Models.City GetCity(string cityCode)
        {
            string strcodeTemp = cityCode;
            if (cityCode != null && cityCode.Length > 5)
            {
                strcodeTemp = cityCode.Substring(0, 5);
            }
            foreach (var item in Provinces)
            {
                foreach (var cityItem in item.Citys)
                {
                    if (strcodeTemp.Equals(cityItem.Code))
                        return cityItem;
                }
            }
            return null;
        }

        /// <summary>
        /// Provincial Information obtained in accordance with Gsafety.PTMS.Bases.Models.City Code
        /// </summary>
        /// <param name="strCityCode"></param>
        /// <returns></returns>
        public Gsafety.PTMS.Bases.Models.Province GetProince(string strCityCode)
        {
            foreach (var item in Provinces)
            {
                foreach (var cityItem in item.Citys)
                {
                    if (strCityCode.Equals(cityItem.Code))
                        return item;
                }
            }
            return null;
        }

        internal void UiRefresh()
        {
            while (true)
            {
                asyncOper.Post(
                    result =>
                    {
                        try
                        {
                            JounceHelper.ExecuteOnUI(
                                () =>
                                {
                                    foreach (var province in _Provinces)
                                    {
                                        province.UiRefresh();
                                    }
                                }
                                );
                        }
                        catch
                        {
                        }
                    },
                    null);

                System.Threading.Thread.Sleep(3000);
            }
        }


        #region 准备删除代码

        private void GetDistrictInfo()
        {
            DistrictServiceClient client = ServiceClientFactory.Create<DistrictServiceClient>();
            client.GetDistrictCompleted += client_GetDistrictCompleted;
            client.GetDistrictAsync();
            ApplicationContext.Instance.Logger.Log(Jounce.Core.Application.LogSeverity.Information, "DistrictManager", "begin get Districts");
            ApplicationContext.Instance.BusyInfo.InitLoadingNum++;
        }

        void client_GetDistrictCompleted(object sender, GetDistrictCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                _hasLoadData = false;
                ApplicationContext.Instance.Logger.Log(Jounce.Core.Application.LogSeverity.Information, "DistrictManager", "get Districts error");
                ApplicationContext.Instance.BusyInfo.InitLoadingNum--;
                return;
            }

            try
            {
                ApplicationContext.Instance.Logger.Log(Jounce.Core.Application.LogSeverity.Information, "DistrictManager", "end get Districts");
                if (e.Result != null)
                {
                    _Provinces.Clear();
                    DistrictByAuthority =
                        new List<District>(e.Result.Result.Where(x => x.Code.Length == 2 || x.Code.Length == 5));
                    ObservableCollection<District> districts = e.Result.Result;
                    var provinceInfo =
                        districts.Where(item => item.Code.Length == 2).OrderBy(item => item.Name).ToList();
                    foreach (var item in provinceInfo)
                    {
                        Gsafety.PTMS.Bases.Models.Province province = new Gsafety.PTMS.Bases.Models.Province();
                        province.Code = item.Code;
                        province.Name = item.Name;
                        var cityInfo =
                            districts.Where(
                                city => city.Code.Length == 5 && city.Code.Substring(0, 2).Equals(item.Code))
                                     .OrderBy(city => city.Name)
                                     .ToList();
                        foreach (var cityItem in cityInfo)
                        {
                            Gsafety.PTMS.Bases.Models.City city = new Gsafety.PTMS.Bases.Models.City(province);
                            city.Code = cityItem.Code;
                            city.Name = cityItem.Name;
                            city.Parent = province;
                            province.Citys.Add(city);
                            _Cities.Add(city);
                        }
                        _Provinces.Add(province);
                    }
                    _Provinces.OrderBy(item => item.Name);
                    ApplicationContext.Instance.BufferManager.VehicleOrganizationManage.DataLoading();
                    _hasLoadData = true;
                }
            }
            catch
            {
                _hasLoadData = false;
            }
            finally
            {
                DistrictServiceClient client = sender as DistrictServiceClient;
                if (client != null)
                {
                    client.CloseAsync();
                }
                ApplicationContext.Instance.Logger.Log(Jounce.Core.Application.LogSeverity.Information, "DistrictManager", "get district finish");
                ApplicationContext.Instance.BusyInfo.InitLoadingNum--;
            }
        }
        #endregion
    }
}
