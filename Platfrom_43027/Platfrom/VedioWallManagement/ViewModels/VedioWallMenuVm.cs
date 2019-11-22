/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: 4d8c6066-dc2b-4d40-949c-fbe0fac82dfb      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: PC-DENGZL
/////                 Author: TEST(dengzl)
/////======================================================================
/////           Project Name: Gsafety.PTMS.VedioWall.ViewModels
/////    Project Description:    
/////             Class Name: VedioWallMenuVm
/////          Class Version: v1.0.0.0
/////            Create Time: 9/11/2013 4:42:22 PM
/////      Class Description:  
/////======================================================================
/////          Modified Time: 9/11/2013 4:42:22 PM
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
using System.Collections.ObjectModel;
using Gsafety.PTMS.Share;
using Jounce.Framework.Command;
using Gsafety.PTMS.Bases.Librarys;
using System.Linq;
using Gsafety.PTMS.VedioWall.Views;
using Jounce.Framework;
using Gsafety.PTMS.Bases.Models;
using Gsafety.PTMS.BasicPage.Views;
using Gsafety.PTMS.BasicPage.Models;
using System.Reflection;
namespace Gsafety.PTMS.VedioWall.ViewModels
{
    [ExportAsViewModel(VideoManagementName.VedioWallMenuVm)]
    public class VedioWallMenuVm : BaseViewModel
    {
        public VehicleTreeFactory VehicleTreeFactory { get; set; }

        public ICommand PlayVideoCommand { get; private set; }

        public ICommand VehicleSearchCommand { get; private set; }

        public ICommand HistoricalVideoCommand { get; private set; }

        public string FilterText
        {
            get;
            set;
        }

        public VedioWallMenuVm()
        {
            VehicleTreeFactory = new VehicleTreeFactory();

            PlayVideoCommand = new ActionCommand<object>(obj => VideoPaly(obj));
            VehicleSearchCommand = new ActionCommand<object>(obj => VehicleSearch_Event());
            HistoricalVideoCommand = new ActionCommand<object>((obj) => HistoricalVideo_Event(obj));
        }

        private void HistoricalVideo_Event(object vechileId)
        {
            var cvm = new HistoryVideoManageWindow(vechileId as string, 16);
            cvm.Closed += cvm_Closed;
            cvm.Show();
        }

        void cvm_Closed(object sender, EventArgs e)
        {
            try
            {
                var window = sender as HistoryVideoManageWindow;
                if (window.DialogResult != true)
                {
                    return;
                }

                if (window.SelectVideoInfoItems.Count > 0)
                {
                    var mediaPlayerInfo = new MediaInfoEx();
                    mediaPlayerInfo.IsHideProgressControl = true;
                    mediaPlayerInfo.Orientation = Orientation.Horizontal;
                    mediaPlayerInfo.AutoPlay = true;
                    mediaPlayerInfo.ShowHistoryLine = false;

                    foreach (var item in window.SelectVideoInfoItems)
                    {
                        var info = new MediaInfoEx.MediaInfoItem()
                        {
                            StartTime = item.Model.StartTime,
                            EndTime = item.Model.EndTime,
                            Url = item.Model.FileID,
                            Channel = (int)item.CameraInstallLocation,
                            IsRealVideo = false,
                            IsShowControlBar = true,
                            IsShowProcessBar = true,
                            ShowRemoveBtn = true
                        };
                        mediaPlayerInfo.MediaInfoItems.Add(info);
                    }

                    mediaPlayerInfo.VehicleId = window.HistoryVideoManageContentViewModel.CarNo;
                    EventAggregator.Publish<MediaInfoEx>(mediaPlayerInfo);
                }
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(), ex);
            }
        }

        private void VehicleSearch_Event()
        {
            VehicleTreeFactory.SearchVehicleTree(FilterText);
        }

        private void VideoPaly(object obj)
        {
            var window = new CameraSelectWindow(obj as string, 8);
            window.Closed += window_Closed;
            window.Show();
        }

        void window_Closed(object sender, EventArgs e)
        {
            try
            {
                var winodw = sender as CameraSelectWindow;
                if (winodw.DialogResult == true && winodw.SelectResult.Count > 0)
                {
                    var info = new MediaInfoEx()
                    {
                        MediaInfoItems = winodw.SelectResult,
                        IsHideProgressControl = true,
                        AutoPlay = true,
                        Orientation = Orientation.Horizontal,
                        ShowHistoryLine = false
                    };

                    info.MediaInfoItems.ForEach(t => t.ShowRemoveBtn = true);

                    ApplicationContext.Instance.EventAggregator.Publish<MediaInfoEx>(info);
                }
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(), ex);
            }
        }
    }
}
