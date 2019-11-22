using Gsafety.PTMS.Bases.Models;
using Gsafety.PTMS.Share;
using Jounce.Framework.ViewModel;
using System;
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
using System.Windows.Data;
using System.Collections.Generic;
using Gsafety.PTMS.BasicPage.Entity;
using Gsafety.Common.Controls;

namespace Gsafety.PTMS.BasicPage.ViewModels
{
    public class VideoBaseVm : BaseEntityViewModel
    {
        /// <summary>
        // Car No. can't be null
        /// </summary>
        public static string CarNoIsNotNull
        {
            get
            {
                return ApplicationContext.Instance.StringResourceReader.GetString("VDM_CarNoIsNotNull");
            }
        }

        /// <summary>
        /// Start Time can't be null
        /// </summary>
        public static string StartTimeIsNotNull
        {
            get
            {
                return ApplicationContext.Instance.StringResourceReader.GetString("VDM_StartTimeIsNotNull");
            }
        }

        /// <summary>
        /// End time can't be Null
        /// </summary>
        public static string EndTimeIsNotNull
        {
            get
            {
                return ApplicationContext.Instance.StringResourceReader.GetString("VDM_EndTimeIsNotNull");
            }
        }

        /// <summary>
        /// start time is bigger than end time
        /// </summary>
        public static string TimeSettingError
        {
            get
            {
                return ApplicationContext.Instance.StringResourceReader.GetString("VDM_TimeSettingError");
            }
        }

        /// <summary>
        /// car number doesn't exits
        /// </summary>
        public static string CarNotExitsError
        {
            get
            {
                return ApplicationContext.Instance.StringResourceReader.GetString("VDM_CarNotExitsError");
            }
        }

        /// <summary>
        /// vehile is off line 
        /// </summary>
        public static string CarOffLine
        {
            get
            {
                return ApplicationContext.Instance.StringResourceReader.GetString("VDM_CarOffLine");
            }
        }

        private bool _isQueryBusy;
        public bool IsQueryBusy
        {
            get { return _isQueryBusy; }
            set
            {
                if (_isQueryBusy != value)
                {
                    _isQueryBusy = value;
                    RaisePropertyChanged(() => this.IsQueryBusy);
                }
            }
        }

        protected bool _isOnline = false;

        public ICommand QueryCommand { get; set; }

        DateTime? _starttime = DateTime.Now.Date;
        public DateTime? StartTime
        {
            get
            {
                return _starttime;
            }
            set
            {
                if (value != _starttime)
                {
                    _starttime = value;
                    RaisePropertyChanged(() => this.StartTime);
                }
            }
        }
        DateTime? _endtime = DateTime.Now.Date.Add(new TimeSpan(23, 59, 59));
        public DateTime? EndTime
        {
            get
            {
                return _endtime;
            }
            set
            {
                if (value != _endtime)
                {
                    _endtime = value;
                    RaisePropertyChanged(() => this.EndTime);
                }
            }
        }

        private string _mdvrid;
        public string MdvrId
        {
            get
            {
                return _mdvrid;
            }
            set
            {
                if (value != _mdvrid)
                {
                    _mdvrid = value;
                    RaisePropertyChanged(() => this.MdvrId);
                }
            }
        }

        private string _carno;
        public string CarNo
        {
            get
            {
                return _carno;
            }
            set
            {
                if (value != _carno)
                {
                    _carno = value;
                    RaisePropertyChanged(() => this.CarNo);
                }
            }
        }

        VideoInfoItem _selecteditem;
        public VideoInfoItem SelectedItem
        {
            get { return _selecteditem; }
            set
            {
                _selecteditem = value;
                RaisePropertyChanged(() => this.SelectedItem);
            }
        }

        protected virtual bool IsValidQuery(bool needonline)
        {
            var msg = "";
            var isValid = true;
            var pname = "";

            if (string.IsNullOrWhiteSpace(CarNo))
            {
                pname = "CarNo";
                msg = CarNoIsNotNull;
                isValid = false;
            }
            if (isValid && !StartTime.HasValue)
            {
                pname = "StartTime";
                msg = StartTimeIsNotNull;
                isValid = false;
            }
            if (isValid && !EndTime.HasValue)
            {
                pname = "EndTime";
                msg = EndTimeIsNotNull;
                isValid = false;
            }

            if (isValid && EndTime < StartTime)
            {
                pname = "EndTime";
                msg = TimeSettingError;
                isValid = false;
            }

            if (isValid)
            {
                CarNo = CarNo.Trim();
                var veh = (ApplicationContext.Instance.BufferManager.VehicleOrganizationManage.VehicleList ?? new ObservableCollection<Vehicle>()).FirstOrDefault(x => string.Compare(x.VehicleId, CarNo, StringComparison.OrdinalIgnoreCase) == 0);
                if (veh == null)
                {
                    pname = "CarNo";
                    msg = CarNotExitsError;
                    isValid = false;
                }
                else
                {
                    _isOnline = veh.IsOnLine;
                    if (needonline)
                    {
                        if (veh.IsOnLine == false)
                        {
                            pname = "CarNo";
                            msg = CarOffLine;
                            isValid = false;
                        }
                        else
                        {
                            MdvrId = veh.UniqueId;
                        }
                    }
                    else
                    {
                        MdvrId = veh.UniqueId;
                    }
                }
            }
            if (!isValid)
            {
                MessageBoxHelper.ShowDialog(msg);
            }
            else
            {
                if (StartTime == EndTime)
                {
                    EndTime = EndTime.Value.AddDays(1).AddSeconds(-1);
                }
            }

            return isValid;
        }

        protected List<VideoInfoItem> _items = new List<VideoInfoItem>();
        protected List<VideoInfoItem> Mdvritems = new List<VideoInfoItem>();

        PagedCollectionView dgi = null;

        public PagedCollectionView DataGridItems
        {
            get
            {
                return dgi;
            }
            set
            {
                if (dgi != value)
                {
                    dgi = value;
                    RaisePropertyChanged("DataGridItems");
                }
            }
        }

        protected virtual void Query()
        {

        }
    }
}
