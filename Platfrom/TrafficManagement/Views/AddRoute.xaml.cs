using BaseLib.ViewModels;
using Gsafety.Common.Controls;
using Gsafety.PTMS.Bases.Enums;
using Gsafety.PTMS.ServiceReference.TrafficManageService;
using Gsafety.PTMS.Share;
using Jounce.Core.Command;
using Jounce.Framework.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Gsafety.PTMS.Traffic.Views
{
    public delegate void AferUpdateRouteInfo(TrafficRoute e);
    public delegate void AfterAddRouteInfo(TrafficRoute e);

    public partial class AddRoute : ChildWindowWithCheck
    {
        public AddRoute(TrafficRoute csTrafficRote)
        {
            InitializeComponent();
            _trafficRoute = csTrafficRote;
            duration = _trafficRoute.OverSpeedDuration.ToString();
            maxspeed = _trafficRoute.MaxSpeed.ToString();
            routewidth = _trafficRoute.Width.ToString();
            this.DataContext = this;

            SaveRouteCommand = new ActionCommand<object>(obj => SaveRoute());
            this.MouseRightButtonDown += ChildWindow_MouseRightButtonDown;
        }

        private void ChildWindow_MouseRightButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            e.Handled = true;
        }

        private void SaveRoute()
        {
            if (ValidateName("RouteName", RouteName) && ValidateRouteWidth("RouteWidth", RouteWidth.ToString()))
            {
                bool pass = false;
                if (IsControlSpeed)
                {
                    pass = false;
                    if (ValidateSpeed("MaxSpeed", MaxSpeed.ToString()) && ValidateDuration("Duration", Duration.ToString()))
                    {
                        pass = true;
                    }
                }
                else
                {
                    pass = true;
                }
                if (IsControlTime)
                {
                    pass = false;
                    if (BValidateBeginAndEndDate("RouteStartTime", (DateTime)RouteStartTime, "RouteEndTime", (DateTime)RouteEndTime))
                    {
                        pass = true;
                    }
                }
                else
                {
                    pass = true;
                }

                if (!IsControlSpeed && !InRouteAlarm && !OutRouteAlarm)
                {
                    MessageBoxHelper.ShowDialog(ApplicationContext.Instance.StringResourceReader.GetString("PleaseGoFenceProprities"), MessageDialogButton.Ok);
                    return;
                }

                if (pass)
                {
                    this.DialogResult = true;
                    if (_bNew)//新建才需要设置以下值
                    {
                        var window = MessageBoxHelper.ShowDialog(ApplicationContext.Instance.StringResourceReader.GetString("TRAFFIC_TIP"), ApplicationContext.Instance.StringResourceReader.GetString("TRAFFIC_DrawRouteGraphics"), MessageDialogButton.Ok);
                        window.Closed += closeWindow_Closed;
                    }
                    else//修改
                    {
                        if (afterUpdateRouteInfo != null)
                        {
                            afterUpdateRouteInfo(_trafficRoute);
                        }
                        this.Title = ApplicationContext.Instance.StringResourceReader.GetString("Edit");
                    }
                }
            }

        }

        void closeWindow_Closed(object sender, EventArgs e)
        {
            var window = sender as ChildWindow;
            if (window.DialogResult == true)
            {
                if (null != afterAddRouteInfo)
                {
                    afterAddRouteInfo(_trafficRoute);
                }
                this.DialogResult = true;
            }
            else
            {
                this.DialogResult = false;
            }
            this.Title = ApplicationContext.Instance.StringResourceReader.GetString("Add");
        }


        private TrafficRoute _trafficRoute;
        public TrafficRoute TrafficRoute
        {
            get
            {
                return _trafficRoute;
            }
            set
            {
                _trafficRoute = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private bool _bNew = true;

        public bool New
        {
            get { return _bNew; }
            set
            {
                _bNew = value;
                if (_bNew)
                {
                    Title = ApplicationContext.Instance.StringResourceReader.GetString("Add");
                }
                else
                {
                    Title = ApplicationContext.Instance.StringResourceReader.GetString("Edit");
                }
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public event AferUpdateRouteInfo afterUpdateRouteInfo;
        /// <summary>
        /// 
        /// </summary>
        public event AfterAddRouteInfo afterAddRouteInfo;

        public IActionCommand SaveRouteCommand { get; private set; }


        /// <summary>
        /// cancel
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        public string RouteName
        {
            get
            {
                return _trafficRoute.Name;
            }
            set
            {
                _trafficRoute.Name = value;
                ValidateName("RouteName", _trafficRoute.Name);
            }
        }

        string maxspeed = string.Empty;

        public string MaxSpeed
        {
            get
            {
                return maxspeed;
            }
            set
            {
                maxspeed = value;
                if (ValidateSpeed("MaxSpeed", value))
                {
                    _trafficRoute.MaxSpeed = short.Parse(value);
                }
                RaisePropertyChanged("MaxSpeed");
            }
        }

        string routewidth = string.Empty;
        public string RouteWidth
        {
            get
            {
                return routewidth;
            }
            set
            {
                routewidth = value;
                if (ValidateRouteWidth("RouteWidth", value))
                {
                    _trafficRoute.Width = short.Parse(value);
                }
                RaisePropertyChanged("RouteWidth");
            }
        }
        string duration = string.Empty;
        public string Duration
        {
            get
            {
                return duration;
            }
            set
            {
                duration = value;
                if (ValidateDuration("Duration", value))
                {
                    _trafficRoute.OverSpeedDuration = short.Parse(value);
                }
                RaisePropertyChanged("Duration");
            }
        }


        public bool InRouteAlarm
        {
            get
            {
                if (TrafficRoute == null || TrafficRoute.RouteProperty == null)
                {
                    return false;
                }
                List<string> lst = _trafficRoute.RouteProperty.Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries).ToList<string>();
                return (lst.IndexOf((((int)(Route_RouteProperty.In_AlertToPlatform))).ToString()) > -1);
            }
            set
            {
                AddOrRemoveRouteProperty(value == true, Route_RouteProperty.In_AlertToPlatform);
            }

        }

        public bool OutRouteAlarm
        {
            get
            {
                if (TrafficRoute == null || TrafficRoute.RouteProperty == null)
                {
                    return false;
                }
                List<string> lst = _trafficRoute.RouteProperty.Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries).ToList<string>();
                return (lst.IndexOf((((int)(FENCE_RegionProperty.Out_AlertToPlatform))).ToString()) > -1);
            }
            set
            {
                AddOrRemoveRouteProperty(value == true, Route_RouteProperty.Out_AlertToPlatform);
            }
        }

        public bool IsControlSpeed
        {
            get
            {
                if (TrafficRoute == null || TrafficRoute.RouteSegmentProperty == null)
                {
                    return false;
                }
                List<string> lst = _trafficRoute.RouteSegmentProperty.Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries).ToList<string>();
                return (lst.IndexOf((((int)(Route_RouteSegmentProperty.Speed_Limit))).ToString()) > -1);
            }
            set
            {
                AddOrRemoveRouteSegmentProperty(value == true, Route_RouteSegmentProperty.Speed_Limit);
                tbOverSpeedDuration.IsEnabled = (value == true);
                tbMaxSpeed.IsEnabled = (value == true);
                if (value != true)
                {
                    MaxSpeed = "0";
                    Duration = "0";
                }
            }
        }

        public bool IsControlTime
        {
            get
            {
                if (TrafficRoute == null || TrafficRoute.RouteProperty == null)
                {
                    return false;
                }
                List<string> lst = _trafficRoute.RouteProperty.Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries).ToList<string>();
                return (lst.IndexOf((((int)(Route_RouteProperty.Time_Limit))).ToString()) > -1);
            }
            set
            {
                AddOrRemoveRouteProperty(value == true, Route_RouteProperty.Time_Limit);
                EdtEndTime.IsEnabled = (value == true);
                EdtStartTime.IsEnabled = (value == true);

                if (value != true)
                {
                    RouteStartTime = null;
                    RouteEndTime = null;
                }
                else
                {
                    RouteStartTime = DateTime.Now;
                    RouteEndTime = DateTime.Now.AddDays(1);
                }
            }
        }

        public DateTime? RouteStartTime
        {
            get
            {
                if (!string.IsNullOrEmpty(_trafficRoute.StartTime))
                {
                    return DateTime.Parse(_trafficRoute.StartTime).ToLocalTime();
                }
                else
                {
                    return null;
                }
            }
            set
            {
                if (value.HasValue)
                {
                    _trafficRoute.StartTime = value.Value.ToUniversalTime().ToString("yyyy-MM-dd HH:mm:ss");
                }
                else
                {
                    _trafficRoute.StartTime = string.Empty;
                }
                if (RouteStartTime != null && RouteEndTime != null)
                    ValidateBeginAndEndDate("RouteStartTime", (DateTime)RouteStartTime, "RouteEndTime", (DateTime)RouteEndTime);
                RaisePropertyChanged("RouteStartTime");
            }
        }


        public DateTime? RouteEndTime
        {
            get
            {
                if (!string.IsNullOrEmpty(_trafficRoute.EndTime))
                {
                    return DateTime.Parse(_trafficRoute.EndTime).ToLocalTime();
                }
                else
                {
                    return null;
                }
            }
            set
            {
                if (value.HasValue)
                {
                    _trafficRoute.EndTime = value.Value.ToUniversalTime().ToString("yyyy-MM-dd HH:mm:ss");
                }
                else
                {
                    _trafficRoute.EndTime = string.Empty;
                }
                if (RouteStartTime != null && RouteEndTime != null)
                    ValidateBeginAndEndDate("RouteStartTime", (DateTime)RouteStartTime, "RouteEndTime", (DateTime)RouteEndTime);
                RaisePropertyChanged("RouteEndTime");
            }
        }

        public void ValidateBeginAndEndDate(string begin, DateTime beginValue, string end, DateTime endValue)
        {
            ClearErrors(begin);
            ClearErrors(end);

            if (beginValue > endValue)
            {
                base.SetError(begin, ApplicationContext.Instance.StringResourceReader.GetString("TimeError"));
                base.SetError(end, ApplicationContext.Instance.StringResourceReader.GetString("TimeError"));
            }
        }

        public bool BValidateBeginAndEndDate(string begin, DateTime beginValue, string end, DateTime endValue)
        {
           
            if (RouteStartTime != null && RouteEndTime != null)
            {
                if (beginValue > endValue)
                {
                    return false;
                }
            }
            else
            {
                return false;

            }
            return true;
        }

        private void AddOrRemoveRouteProperty(bool isAdd, Route_RouteProperty ftype)
        {
            List<string> lst = new List<string>();
            if (_trafficRoute.RouteProperty != null)
            {
                lst = _trafficRoute.RouteProperty.Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries).ToList<string>();
            }

            if (isAdd == true)
            {
                if (lst.IndexOf(((int)(ftype)).ToString()) == -1)
                {
                    lst.Add(((int)(ftype)).ToString());
                }
            }
            else
            {
                if (lst.IndexOf(((int)(ftype)).ToString()) > -1)
                {
                    lst.Remove(((int)(ftype)).ToString());
                }
            }
            string newRouteProperty = "";
            foreach (string str in lst)
            {
                if (newRouteProperty == "")
                {
                    newRouteProperty = str;
                }
                else
                {
                    newRouteProperty = newRouteProperty + "," + str;
                }
            }
            _trafficRoute.RouteProperty = newRouteProperty;
        }


        private void AddOrRemoveRouteSegmentProperty(bool isAdd, Route_RouteSegmentProperty ftype)
        {
            List<string> lst = new List<string>();
            if (_trafficRoute.RouteSegmentProperty != null)
            {
                lst = _trafficRoute.RouteSegmentProperty.Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries).ToList<string>();
            }

            if (isAdd == true)
            {
                if (lst.IndexOf(((int)(ftype)).ToString()) == -1)
                {
                    lst.Add(((int)(ftype)).ToString());
                }
            }
            else
            {
                if (lst.IndexOf(((int)(ftype)).ToString()) > -1)
                {
                    lst.Remove(((int)(ftype)).ToString());
                }
            }
            string newRouteSegmentProperty = "";
            foreach (string str in lst)
            {
                if (newRouteSegmentProperty == "")
                {
                    newRouteSegmentProperty = str;
                }
                else
                {
                    newRouteSegmentProperty = newRouteSegmentProperty + "," + str;
                }
            }
            _trafficRoute.RouteSegmentProperty = newRouteSegmentProperty;
        }

        private bool ValidateName(string prop, string value)
        {
            bool isSuccess = true;
            ClearErrors(prop);
            if (string.IsNullOrEmpty(value))
            {
                SetError(prop, ApplicationContext.Instance.StringResourceReader.GetString(PTMSBaseViewModel.notnull));
                isSuccess = false;
            }
            return isSuccess;
        }

        private bool ValidateSpeed(string prop, string value)
        {
            bool isSuccess = true;
            ClearErrors(prop);

            if (IsControlSpeed)
            {
                if (string.IsNullOrEmpty(value))
                {
                    base.SetError(prop, ApplicationContext.Instance.StringResourceReader.GetString(PTMSBaseViewModel.notnull));

                    return false;
                }

                int intvalue = 0;
                if (!int.TryParse(value, out intvalue))
                {
                    base.SetError(prop, ApplicationContext.Instance.StringResourceReader.GetString("LimitSpeed"));

                    return false;
                }
                if (intvalue <= 0 || intvalue > 200)
                {
                    SetError(prop, ApplicationContext.Instance.StringResourceReader.GetString("LimitSpeed"));
                    isSuccess = false;
                }
            }

            return isSuccess;
        }

        private bool ValidateRouteWidth(string prop, string value)
        {
            bool isSuccess = true;
            ClearErrors(prop);

            if (string.IsNullOrEmpty(value))
            {
                base.SetError(prop, ApplicationContext.Instance.StringResourceReader.GetString(PTMSBaseViewModel.notnull));

                return false;
            }

            int intvalue = 0;
            if (!int.TryParse(value, out intvalue))
            {
                base.SetError(prop, ApplicationContext.Instance.StringResourceReader.GetString("RouteWidthLimit"));

                return false;
            }
            if (intvalue <= 0 || intvalue > 255)
            {
                SetError(prop, ApplicationContext.Instance.StringResourceReader.GetString("RouteWidthLimit"));
                isSuccess = false;
            }


            return isSuccess;
        }



        private bool ValidateDuration(string prop, string value)
        {
            bool isSuccess = true;
            ClearErrors(prop);

            if (IsControlSpeed)
            {
                if (string.IsNullOrEmpty(value))
                {
                    base.SetError(prop, ApplicationContext.Instance.StringResourceReader.GetString(PTMSBaseViewModel.notnull));

                    return false;
                }

                int intvalue = 0;
                if (!int.TryParse(value, out intvalue))
                {
                    base.SetError(prop, ApplicationContext.Instance.StringResourceReader.GetString("LimitTime"));

                    return false;
                }

                if (intvalue <= 0 || intvalue > 255)
                {
                    SetError(prop, ApplicationContext.Instance.StringResourceReader.GetString("LimitTime"));
                    isSuccess = false;
                }
            }
            return isSuccess;
        }
    }
}

