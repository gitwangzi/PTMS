using BaseLib.ViewModels;
using Gsafety.Common.Controls;
using Gsafety.PTMS.Bases.Enums;
using Gsafety.PTMS.ServiceReference.TrafficManageService;
/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: e0866101-4b5c-488b-9165-51155bccc27f      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: PC-WANGYL
/////                 Author: TEST(wangyl)
/////======================================================================
/////           Project Name: Gsafety.PTMS.Share.Enum
/////    Project Description:    
/////             Class Name: AddFence
/////          Class Version: v1.0.0.0
/////            Create Time: 2013/8/30 17:33:07
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2013/8/30 17:33:07
/////            Modified by:
/////   Modified Description: 
/////======================================================================
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
    public delegate void AferUpdateFenceInfo(TrafficFence e);
    public delegate void AfterAddFenceInfo(TrafficFence e, TrafficDrawType nType, double dDist);
    /// <summary>
    /// add fence
    /// </summary>
    public partial class AddFence : ChildWindowWithCheck
    {
        public AddFence(TrafficFence csTrafficFence)
        {
            InitializeComponent();

            _trafficFence = csTrafficFence;
            maxspeed = _trafficFence.MaxSpeed.ToString();
            duration = _trafficFence.OverSpeedDuration.ToString();
            this.DataContext = this;

            SaveFenceCommand = new ActionCommand<object>(obj => SaveFence());
            this.MouseRightButtonDown += ChildWindow_MouseRightButtonDown;
        }

        private void ChildWindow_MouseRightButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            e.Handled = true;
        }

        private void SaveFence()
        {
            if (ValidateName("FenceName", FenceName))
            {
                bool pass = false;
                if (IsControlSpeed)
                {
                    pass = false;
                    if (ValidateSpeed("MaxSpeed", MaxSpeed) && ValidateDuration("Duration", Duration))
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
                    if (BValidateBeginAndEndDate("FenceStartTime", (DateTime)FenceStartTime, "FenceEndTime", (DateTime)FenceEndTime))
                    {
                        pass = true;
                    }
                }
                else
                {
                    pass = true;
                }

                if (!IsControlSpeed && !InFenceAlarm && !OutFenceAlarm)
                {
                    MessageBoxHelper.ShowDialog(ApplicationContext.Instance.StringResourceReader.GetString("PleaseGoFenceProprities"), MessageDialogButton.Ok);
                    return;
                }

                if (pass)
                {
                    this.DialogResult = true;
                    if (_bNew)//新建才需要设置以下值
                    {
                        var result = MessageBoxHelper.ShowDialog(ApplicationContext.Instance.StringResourceReader.GetString("TRAFFIC_TIP"), ApplicationContext.Instance.StringResourceReader.GetString("TRAFFIC_DrawFenceGraphics"), MessageDialogButton.Ok);                      
                        result.Closed += delegate(object sender, EventArgs e)
                        {
                            ChildWindow childwindow = sender as ChildWindow;
                            if (childwindow.DialogResult == true)
                            {
                                Views.FenceShapeSelectView shapeSelect = new Views.FenceShapeSelectView();
                                shapeSelect.Show();

                                shapeSelect.Closed += (a, b) =>
                                {
                                    if (null != afterAddFenceInfo&&!string.IsNullOrEmpty(shapeSelect.selectShape))
                                    {
                                        switch (shapeSelect.selectShape)
                                        {
                                            case "polygon":
                                                afterAddFenceInfo(_trafficFence, TrafficDrawType.Polygon, 0);
                                                break;
                                            case "rectangle":
                                                afterAddFenceInfo(_trafficFence, TrafficDrawType.Rectangle, 0);
                                                break;
                                            case "circle":
                                                afterAddFenceInfo(_trafficFence, TrafficDrawType.Circular, 0);
                                                break;
                                        }
                                        this.DialogResult = true;
                                    }
                                    else
                                    {
                                        this.DialogResult = false;
                                    }
                                };
                            }
                            else
                            {
                                this.DialogResult = false;
                            }
                        };
                    }
                    else//修改
                    {
                        if (afterUpdatefenceInfo != null)
                        {
                            afterUpdatefenceInfo(_trafficFence);
                        }
                    }
                }
            }
        }

        private TrafficFence _trafficFence;
        public TrafficFence TrafficFence
        {
            get
            {
                return _trafficFence;
            }
            set
            {
                _trafficFence = value;
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
        public event AferUpdateFenceInfo afterUpdatefenceInfo;
        /// <summary>
        /// 
        /// </summary>
        public event AfterAddFenceInfo afterAddFenceInfo;

        public IActionCommand SaveFenceCommand { get; private set; }


        /// <summary>
        /// cancel
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        public string FenceName
        {
            get
            {
                return _trafficFence.Name;
            }
            set
            {
                _trafficFence.Name = value;
                ValidateName("FenceName", _trafficFence.Name);
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
                    _trafficFence.MaxSpeed = int.Parse(value);
                }
                RaisePropertyChanged("MaxSpeed");
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
                    _trafficFence.OverSpeedDuration = int.Parse(value);
                }
                RaisePropertyChanged("Duration");
            }
        }

        public bool InFenceAlarm
        {
            get
            {
                if (TrafficFence == null || TrafficFence.RegionProperty == null)
                {
                    return false;
                }
                List<string> lst = _trafficFence.RegionProperty.Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries).ToList<string>();
                return (lst.IndexOf((((int)(FENCE_RegionProperty.In_AlertToPlatform))).ToString()) > -1);
            }
            set
            {
                AddOrRemoveProperty(value == true, FENCE_RegionProperty.In_AlertToPlatform);
            }
        }

        public bool OutFenceAlarm
        {
            get
            {
                if (TrafficFence == null || TrafficFence.RegionProperty == null)
                {
                    return false;
                }
                List<string> lst = _trafficFence.RegionProperty.Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries).ToList<string>();
                return (lst.IndexOf((((int)(FENCE_RegionProperty.Out_AlertToPlatform))).ToString()) > -1);
            }
            set
            {
                AddOrRemoveProperty(value == true, FENCE_RegionProperty.Out_AlertToPlatform);
            }
        }

        public bool IsControlSpeed
        {
            get
            {
                if (TrafficFence == null || TrafficFence.RegionProperty == null)
                {
                    return false;
                }
                List<string> lst = _trafficFence.RegionProperty.Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries).ToList<string>();
                return (lst.IndexOf((((int)(FENCE_RegionProperty.Speed_Limit))).ToString()) > -1);
            }
            set
            {
                AddOrRemoveProperty(value == true, FENCE_RegionProperty.Speed_Limit);
                tbOverSpeedDuration.IsEnabled = (value == true);
                tbMaxSpeed.IsEnabled = (value == true);

                if (value == false)
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
                if (TrafficFence == null || TrafficFence.RegionProperty == null)
                {
                    return false;
                }
                List<string> lst = _trafficFence.RegionProperty.Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries).ToList<string>();
                return (lst.IndexOf((((int)(FENCE_RegionProperty.Time_Limit))).ToString()) > -1);
            }
            set
            {
                AddOrRemoveProperty(value == true, FENCE_RegionProperty.Time_Limit);
                EdtEndTime.IsEnabled = (value == true);
                EdtStartTime.IsEnabled = (value == true);
                if (value == false)
                {
                    FenceStartTime = null;
                    FenceEndTime = null;
                }
                else
                {
                    FenceStartTime = DateTime.Now;
                    FenceEndTime = DateTime.Now.AddDays(1);
                }
            }
        }

        public DateTime? FenceStartTime
        {
            get
            {
                if (!string.IsNullOrEmpty(_trafficFence.StartTime))
                {
                    return DateTime.Parse(_trafficFence.StartTime).ToLocalTime();
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
                    _trafficFence.StartTime = value.Value.ToUniversalTime().ToString("yyyy-MM-dd HH:mm:ss");
                }
                else
                {
                    _trafficFence.StartTime = string.Empty;
                }
                if (FenceStartTime != null && FenceEndTime != null)
                    ValidateBeginAndEndDate("FenceStartTime", (DateTime)FenceStartTime, "FenceEndTime", (DateTime)FenceEndTime);
                RaisePropertyChanged("FenceStartTime");
            }
        }


        public DateTime? FenceEndTime
        {
            get
            {
                if (!string.IsNullOrEmpty(_trafficFence.EndTime))
                {
                    return DateTime.Parse(_trafficFence.EndTime).ToLocalTime();
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
                    _trafficFence.EndTime = value.Value.ToUniversalTime().ToString("yyyy-MM-dd HH:mm:ss");
                }
                else
                {
                    _trafficFence.EndTime = string.Empty;
                }
                if (FenceStartTime != null && FenceEndTime != null)
                    ValidateBeginAndEndDate("FenceStartTime", (DateTime)FenceStartTime, "FenceEndTime", (DateTime)FenceEndTime);
                RaisePropertyChanged("FenceEndTime");
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
          
            if (FenceStartTime != null && FenceEndTime != null)
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



        private void AddOrRemoveProperty(bool isAdd, FENCE_RegionProperty ftype)
        {
            List<string> lst = new List<string>();
            if (_trafficFence.RegionProperty != null)
            {
                lst = _trafficFence.RegionProperty.Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries).ToList<string>();
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
            string newRegionProperty = "";
            foreach (string str in lst)
            {
                if (newRegionProperty == "")
                {
                    newRegionProperty = str;
                }
                else
                {
                    newRegionProperty = newRegionProperty + "," + str;
                }
            }
            _trafficFence.RegionProperty = newRegionProperty;
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
                    return false;
                }
            }

            return true;
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

        private void tbFenceName_TextChanged_1(object sender, TextChangedEventArgs e)
        {

        }

        private void tbMaxSpeed_TextChanged_1(object sender, TextChangedEventArgs e)
        {

        }

        private void tbOverSpeedDuration_TextChanged_1(object sender, TextChangedEventArgs e)
        {

        }
    }
}

