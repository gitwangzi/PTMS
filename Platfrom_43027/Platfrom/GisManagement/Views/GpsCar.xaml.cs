/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: 10201521-9307-45c8-a06a-df8ba7a73ec8      
/////             clrversion: 4.0.30319.17929
/////Registered organization: 
/////           Machine Name: PC-LILF
/////                 Author: TEST(lilf)
/////======================================================================
/////           Project Name: GisManagement.Views
/////    Project Description:    
/////             Class Name: GpsCar
/////          Class Version: v1.0.0.0
/////            Create Time: 2013/9/5 14:53:54
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2013/9/5 14:53:54
/////            Modified by:
/////   Modified Description: 
/////======================================================================
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using ESRI.ArcGIS.Client;
using ESRI.ArcGIS.Client.Geometry;
using GisManagement.Models;
using GisManagement.ViewModels;
using Gsafety.PTMS.Share;
using Gsafety.PTMS.Bases.Enums;
using Gsafety.Common.Converts;
using Gs.PTMS.Common.Data.Enum;
using Gsafety.Common.CommMessage;
using System.ComponentModel.Composition;
using Jounce.Core.Event;
using Gsafety.PTMS.BasicPage.Views;
using Gsafety.Common.Controls;
using System.Windows.Threading;
using System.Windows.Shapes;
using System.IO;
using Gsafety.PTMS.ServiceReference.VehicleService;

namespace GisManagement.Views
{
    public partial class GpsCar : UserControl, INotifyPropertyChanged, IPartImportsSatisfiedNotification
    {
        [Import]
        public IEventAggregator EventAggregator { get; set; }

        private DispatcherTimer _SamplerTimer;
        public GpsCar(string csCarNo, string csUniqueID)
        {
            CompositionInitializer.SatisfyImports(this);

            // TODO: Complete member initialization
            InitializeComponent();
            this.FixOperate = false;
            this.CarNo = csCarNo;
            this._AlarmFlag = false;
            this._AlertFlag = false;
            this._UniqueID = csUniqueID;
            this.HasDraw = false;
            this._lastGPSTime = DateTime.MinValue;
            this.Valid = "N";
            this.EventID = "";
            this.IsTracked = false;
            this.TrackImage = new BitmapImage(new Uri("/GisManagement;component/Image/track_disnable.png", UriKind.RelativeOrAbsolute));
            this._GroupName = "";
            this._DisplayGroupName = "";
            this._MemoInfo = "";
            this.GPSSource = GPSSourceEnum.UnKnown;
            this.AddedDateTime = DateTime.Now;
            this.AlertList = new List<short>();
            this.DataContext = this;
            this._DisplayVideo = false;
            this._SamplerTimer =new DispatcherTimer
            {
                Interval = TimeSpan.FromMilliseconds(3000)
            };
            this._SamplerTimer.Tick += OnSamplerOnTick;
            this._SamplerTimer.Stop();
            //this.Graphics = MonitorList.RequestGpsDataVechileGraphics;
        }

        private List<VehicleTypeColor> _speedcolorlist = new List<VehicleTypeColor>();
        public List<VehicleTypeColor> SpeedColorList
        {
            get { return _speedcolorlist; }
            set
            {
                _speedcolorlist = value;

            }
        }

        private void OnSamplerOnTick(object sender, EventArgs e)
        {
             RaisePropertyChanged("GpsTime");
             this._SamplerTimer.Stop();
        }

        public GpsCar(string QueryCarNo)
        {
            // TODO: Complete member initialization
            CompositionInitializer.SatisfyImports(this);
            InitializeComponent();
            this.FixOperate = false;
            this.CarNo = QueryCarNo;
            this._AlarmFlag = false;
            this._AlertFlag = false;
            this.HasDraw = false;
            this.Valid = "N";
            this._GroupName = "";
            this._DisplayGroupName = "";
            this.EventID = "";
            this._MemoInfo = "";
            this._lastGPSTime = DateTime.MinValue;
            this.AddedDateTime = DateTime.Now;
            this.GPSSource = GPSSourceEnum.UnKnown;
            this.IsTracked = false;
            this.AlertList = new List<short>();
            this.TrackImage = new BitmapImage(new Uri("/GisManagement;component/Image/track_disnable.png", UriKind.RelativeOrAbsolute));
            this.DataContext = this;
            this._DisplayVideo = false;
            this._SamplerTimer = new DispatcherTimer
            {
                Interval = TimeSpan.FromMilliseconds(1000)
            };
            this._SamplerTimer.Tick += OnSamplerOnTick;
            this._SamplerTimer.Stop();
        }
        public GPSSourceEnum GPSSource
        {
            get
            {
                return _lastGPSSource;
            }
            set
            {
                _lastGPSSource = value;
                RaisePropertyChanged("GPSSource");
            }
        }

        //上一次的GPS位置时间
        private DateTime _lastGPSTime;
        private GPSSourceEnum _lastGPSSource;
        //报警标识
        private bool _AlarmFlag;
        public bool AlarmFlag
        {
            get
            {
                return _AlarmFlag;
            }
            set
            {
                _AlarmFlag = value;
                RefreshCarStyleImage();
            }
        }

        //告警标识
        private bool _AlertFlag;
        public bool AlertFlag
        {
            get
            {
                return _AlertFlag;
            }
            set
            {
                _AlertFlag = value;
                RefreshCarStyleImage();
            }
        }


        //上线标识
        private bool _OnlineFlag;
        public bool OnlineFlag
        {
            get
            {
                return _OnlineFlag;
            }
            set
            {
                _OnlineFlag = value;
                RefreshCarStyleImage();
            }
        }
        private DateTime FLastUpdateTime;

        //private VehicleType _CarStyle;

        //public VehicleType CarStyle
        //{
        //    get
        //    {
        //        return _CarStyle;
        //    }
        //    set
        //    {
        //        _CarStyle = value;
        //        RefreshCarStyleImage();
        //    }
        //}

        public BitmapImage CarStyleImage
        {
            get
            {
                if (ElementLayDefine == ElementLayerDefine.miVEAlarmHappenLocation)
                {
                    SpeedColor.Visibility = Visibility.Collapsed;
                    Backimage.Visibility = Visibility.Collapsed;
                    Typeimage.Visibility = Visibility.Collapsed;
                    StateImage.Visibility = Visibility.Collapsed;



                    return new BitmapImage(new Uri("/GisManagement;component/Image/AlarmPosition.png", UriKind.RelativeOrAbsolute));
                }

                else if (ElementLayDefine == ElementLayerDefine.miVEAlertHappenLocation)
                {
                    SpeedColor.Visibility = Visibility.Collapsed;
                    Backimage.Visibility = Visibility.Collapsed;
                    Typeimage.Visibility = Visibility.Collapsed;
                    StateImage.Visibility = Visibility.Collapsed;
                    return new BitmapImage(new Uri("/GisManagement;component/Image/AlertPosition.png", UriKind.RelativeOrAbsolute));
                }
                else
                {
                    SpeedColor.Visibility = Visibility.Visible;
                    Backimage.Visibility = Visibility.Visible;
                    Typeimage.Visibility = Visibility.Visible;
                    StateImage.Visibility = Visibility.Visible;

                    if ((_AlarmFlag) && (_AlertFlag))
                    {
                        if (_OnlineFlag)
                        {
                            return new BitmapImage(new Uri("/GisManagement;component/Image/CarAlarmAlert.png", UriKind.RelativeOrAbsolute));
                        }
                        else
                        {
                            return new BitmapImage(new Uri("/GisManagement;component/Image/CarAlarmAlert_offline.png", UriKind.RelativeOrAbsolute));

                        }
                    }
                    else if (_AlarmFlag)
                    {
                        if (_OnlineFlag)
                        {
                            return new BitmapImage(new Uri("/GisManagement;component/Image/CarAlarm.png", UriKind.RelativeOrAbsolute));
                        }
                        else
                        {
                            return new BitmapImage(new Uri("/GisManagement;component/Image/CarAlarm_offline.png", UriKind.RelativeOrAbsolute));

                        }
                    }
                    else if (_AlertFlag)
                    {
                        if (_OnlineFlag)
                        {
                            return new BitmapImage(new Uri("/GisManagement;component/Image/CarAlert.png", UriKind.RelativeOrAbsolute));
                        }
                        else
                        {
                            return new BitmapImage(new Uri("/GisManagement;component/Image/CarAlert_offline.png", UriKind.RelativeOrAbsolute));

                        }
                    }
                    else
                    {
                        if (_OnlineFlag)
                        {
                            return new BitmapImage(new Uri("/GisManagement;component/Image/Car.png", UriKind.RelativeOrAbsolute));
                        }
                        else
                        {
                            return new BitmapImage(new Uri("/GisManagement;component/Image/Car_offline.png", UriKind.RelativeOrAbsolute));

                        }
                    }
                }

            }
        }
        public BitmapImage CarStyleImageDisplay
        {
            get
            {
                if (ElementLayDefine == ElementLayerDefine.miVEAlarmHappenLocation)
                {
                    return new BitmapImage(new Uri("/GisManagement;component/Image/AlarmPosition_D.png", UriKind.RelativeOrAbsolute));
                }


                if (ElementLayDefine == ElementLayerDefine.miVERealLocation)
                {
                    return new BitmapImage(new Uri("/GisManagement;component/Image/CarAlarm_D.png", UriKind.RelativeOrAbsolute));
                }
                else
                {
                    return new BitmapImage(new Uri("/GisManagement;component/Image/Car_D.png", UriKind.RelativeOrAbsolute));
                }

            }
        }
        //更新GPS信息
        public void UpdateGpsInfo(Gsafety.PTMS.ServiceReference.MessageServiceExt.GPS gpsinfo)
        {
            if (!GPSState.Valid(gpsinfo.Valid)) return;
            if (gpsinfo.GpsTime == null) return;
            //if ((_lastGPSTime != null) && (_lastGPSTime.CompareTo(gpsinfo.GpsTime) > 0)) return;//说明新来的数据旧
            if (this.GPSSource != GPSSourceEnum.UnKnown)//以前有过GPS
            {
                //如果上一次不是手机，则本次如果是手机则仍掉
                if ((this.GPSSource != GPSSourceEnum.Mobile) && ((GPSSourceEnum)gpsinfo.Source == GPSSourceEnum.Mobile)) return;
            }
            this.GPSSource = (GPSSourceEnum)gpsinfo.Source;
            if (!string.IsNullOrEmpty(gpsinfo.VehicleId))
            {
                this.AlertFlag = ApplicationContext.Instance.BufferManager.VehicleAlertManager.HasAlert(gpsinfo.VehicleId);
                this.AlarmFlag = ApplicationContext.Instance.BufferManager.AlarmManager.HasAlarm(gpsinfo.VehicleId);
                var vehicleInfo = ApplicationContext.Instance.BufferManager.VehicleOrganizationManage.VehicleList.FirstOrDefault(t => t.VehicleId == gpsinfo.VehicleId);
                this.OnlineFlag = vehicleInfo.IsOnLine;
                if (vehicleInfo != null)
                {
                    if (!string.IsNullOrEmpty(vehicleInfo.VehicleTypeImage))
                    {
                        byte[] data = Convert.FromBase64String(vehicleInfo.VehicleTypeImage);

                        BitmapImage image = new BitmapImage();

                        image.SetSource(new MemoryStream(data));

                        this.VehicleType = image;

                    }
                    if (!string.IsNullOrEmpty(vehicleInfo.VehicleTypeDescribe))
                    {

                        SpeedColorList = ApplicationContext.Instance.BufferManager.VehicleOrganizationManage.SpeedColorList.Where(x => x.TypeName == vehicleInfo.VehicleTypeDescribe).ToList();
                    }

                }
            }
            if (gpsinfo.Direction != null)
                this.Dir = gpsinfo.Direction;
            if (gpsinfo.Speed != null)
                this.Speed = gpsinfo.Speed;
            if (gpsinfo.Latitude != null)
                this.RecLat = gpsinfo.Latitude;
            if (gpsinfo.Longitude != null)
                this.RecLon = gpsinfo.Longitude;
            if (gpsinfo.GpsTime != null)
                this.GpsTime = gpsinfo.GpsTime.ToString();
           

            _lastGPSTime = (DateTime)(gpsinfo.GpsTime);
            GPSSource = (GPSSourceEnum)gpsinfo.Source;
        }
        private string _RecLat;

        public string RecLat
        {
            get
            {
                return _RecLat;
            }
            set
            {
                _RecLat = value;
            }
        }
        private string _RecLon;

        public string RecLon
        {
            get
            {
                return _RecLon;
            }
            set
            {
                _RecLon = value;
            }

        }


        private string _GpsTime;

        public string GpsTime
        {
            get
            {
                return _GpsTime;
            }
            set
            {
                _GpsTime = value;
                this._SamplerTimer.Stop();
                DisplayGpsDateTime.Text = _GpsTime;

                if (value !=null && value != "")
                {
                    TimeSpan ts = DateTime.Now - DateTime.Parse(value);
                    if (ts.TotalSeconds < 3*60)
                    {
                        this._SamplerTimer.Interval = TimeSpan.FromSeconds(3 * 60 - ts.TotalSeconds);
                        this._SamplerTimer.Start();
                    }
                }
                RaisePropertyChanged("GpsTime");
            }
        }

        public void RefreshDisplay()
        {
            DisplayGpsDateTime.Text = _GpsTime;
            if (!GPSState.Valid(this.Valid))
            {
                DisplayLat.Text = "-";
                DisplayLon.Text = "-";
                DisplaySpeed.Text = "-";
                SpeedColor.Fill = new SolidColorBrush(Colors.Green);
                DisplayDir.Text = "-";
                return;
            }

            string temp = "";
            if (_RecLat != null)
            {
                DisplayLatConvert con = new DisplayLatConvert();
                temp = con.ConvertBack(_RecLat, null, null, null).ToString();
                DisplayLat.Text = getDisplayLat(temp);
            }
            else
            {
                DisplayLat.Text = "-";
            }

            if (_RecLon != null)
            {
                DisplayLonConvert con = new DisplayLonConvert();
                temp = con.ConvertBack(_RecLon, null, null, null).ToString();
                DisplayLon.Text = getDisplayLon(temp);
            }
            else
            {
                DisplayLon.Text = "-";
            }
        }
        private string _CarNo;

        public string CarNo
        {
            get
            {
                return _CarNo;
            }
            set
            {
                _CarNo = value;
                minDisplayCarNo.Text = _CarNo;
                // maxDisplayCarNo.Text = TranslateInfo.Translate(TranslateInfo.CarNo) + ":" + _CarNo;
                RaisePropertyChanged("CarNo");
            }
        }

        private string _MemoInfo;

        public string MemoInfo
        {
            get
            {
                return _MemoInfo;
            }
            set
            {
                _MemoInfo = value;
                // DisplayMemo.Text = _MemoInfo;
                RaisePropertyChanged("MemoInfo");
            }
        }

        private string _UniqueID;

        public string UniqueID
        {
            get
            {
                return _UniqueID;
            }
            set
            {
                _UniqueID = value;
                RaisePropertyChanged("UniqueID");
            }
        }
        public Color ToColor(string code)
        {

            if (!code.StartsWith("#"))
            {
                return Colors.Green;
            }
            else
            {
                code = code.Replace("#", string.Empty);
                int v = int.Parse(code, System.Globalization.NumberStyles.HexNumber);
                return new Color()
                {
                    A = Convert.ToByte((v >> 24) & 255),
                    R = Convert.ToByte((v >> 16) & 255),
                    G = Convert.ToByte((v >> 8) & 255),
                    B = Convert.ToByte((v >> 0) & 255)


                };


            }
            return Colors.Green;
        }


        private string _Speed;

        public string Speed
        {
            get
            {
                return _Speed;
            }
            set
            {
                _Speed = value;
                if (_Speed != null)
                {

                    if (_Speed == "-.-" || _Speed == "-" || _Speed == "")
                    {
                        _Speed = "0";
                    
                    }
                    //DisplaySpeed.Text = TranslateInfo.Translate(TranslateInfo.Speed) + ":" + _Speed.Replace(".", System.Threading.Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalSeparator);
                    DisplaySpeed.Text = _Speed.Replace(".", System.Threading.Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalSeparator);
                    SpeedColor.Visibility = Visibility.Visible;

                    double data = double.Parse(_Speed, System.Globalization.CultureInfo.InvariantCulture);

                    if (SpeedColorList.Count > 0)
                    {

                        if (data == 0)
                        {
                            Stop.Source = new BitmapImage(new Uri("/GisManagement;component/Image/Gisstop.png", UriKind.RelativeOrAbsolute));
                            Stop.Visibility = Visibility.Visible;
                            Start.Visibility = Visibility.Collapsed;
                        }
                        else
                        {

                            Stop.Visibility = Visibility.Collapsed;
                            Start.Visibility = Visibility.Visible;
                        }

                        foreach (var item in SpeedColorList)
                        {
                            if (data >= item.MinSpeed && data <= item.MaxSpeed)
                            {
                                if (item.Color != null)
                                {

                                    SpeedColor.Fill = new SolidColorBrush(ToColor(item.Color));
                                }
                                break;
                            }

                        }

                    }
                    else
                    {
                        if (data == 0)
                        {
                            Stop.Source = new BitmapImage(new Uri("/GisManagement;component/Image/Gisstop.png", UriKind.RelativeOrAbsolute));
                            Stop.Visibility = Visibility.Visible;
                            Start.Visibility = Visibility.Collapsed;
                            SpeedColor.Fill = new SolidColorBrush(Colors.Green);
                        }
                        else
                        {

                            if (data <= 50)
                            {
                                Stop.Visibility = Visibility.Collapsed;
                                Start.Visibility = Visibility.Visible;
                                SpeedColor.Fill = new SolidColorBrush(Colors.Green);
                            }
                            else if (data <= 100 && data > 50)
                            {
                                Stop.Visibility = Visibility.Collapsed;
                                Start.Visibility = Visibility.Visible;
                                SpeedColor.Fill = new SolidColorBrush(Colors.Yellow);
                            }
                            else if (data > 100)
                            {
                                Stop.Visibility = Visibility.Collapsed;
                                Start.Visibility = Visibility.Visible;
                                SpeedColor.Fill = new SolidColorBrush(Colors.Red);
                            }
                        }

                    }

                }
                else
                {
                    DisplaySpeed.Text = "";
                    SpeedColor.Visibility = Visibility.Collapsed;
                }
                RaisePropertyChanged("Speed");
            }
        }



        private BitmapImage _vehicletype;

        public BitmapImage VehicleType
        {
            get
            {
                return _vehicletype;
            }
            set
            {
                _vehicletype = value;
                if (_vehicletype != null)
                {
                    VehicleTypeImage.ImageSource = _vehicletype;
                  

                }
                else
                {
                    VehicleTypeImage.ImageSource = new BitmapImage(new Uri("/ExternalResource;component/Images/onLineTaxi.png", UriKind.RelativeOrAbsolute));
                }
                RaisePropertyChanged("VehicleType");
            }
        }

        public string Lon
        {
            get
            {
                string temp = "";
                if (_RecLon != null)
                {
                    DisplayLonConvert con = new DisplayLonConvert();
                    temp = con.ConvertBack(_RecLon, null, null, null).ToString();
                    DisplayLon.Text = getDisplayLon(temp);
                    //DisplayLon.Text = TranslateInfo.Translate(TranslateInfo.Lon) + ":" + getDisplayLon(temp);
                }
                return temp;
            }
        }

        public string Lat
        {
            get
            {
                string temp = "";
                if (_RecLat != null)
                {
                    DisplayLatConvert con = new DisplayLatConvert();
                    temp = con.ConvertBack(_RecLat, null, null, null).ToString();
                    //DisplayLat.Text = TranslateInfo.Translate(TranslateInfo.Lat) + ":" + getDisplayLat(temp);
                    DisplayLat.Text = getDisplayLat(temp);
                }
                return temp;

            }
        }
        private string _Dir;

        public string Dir
        {
            get
            {
                return _Dir;
            }
            set
            {
                _Dir = value;
                if (_Dir != null)
                {
                    if (_Dir == "-.-" || _Dir == "-" || _Dir=="")
                    {
                        _Dir = "0";

                    }
                    //DisplayDir.Text = TranslateInfo.Translate(TranslateInfo.Dir) + ":" + _Dir.Replace(".", System.Threading.Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalSeparator);
                    DisplayDir.Text = _Dir.Replace(".", System.Threading.Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalSeparator);

                    Direcion.Rotation = double.Parse(_Dir, System.Globalization.CultureInfo.InvariantCulture);
                }
                else
                {
                    DisplayDir.Text = "";
                }
                RaisePropertyChanged("Dir");
            }
        }

        public string _Valid;

        public string Valid
        {
            get
            {
                return _Valid;
            }
            set
            {
                _Valid = value;
                RaisePropertyChanged("Valid");
                RaisePropertyChanged("DisplayGpsValid");
            }
        }

        public DateTime AddedDateTime
        {
            get;
            set;
        }
        private string _Prov;

        public string Prov
        {
            get
            {
                return _Prov;
            }
            set
            {
                _Prov = value;
                RaisePropertyChanged("Prov");
            }
        }


        private bool _IsTracked;

        public bool IsTracked
        {
            get
            {
                return _IsTracked;
            }
            set
            {
                if (_IsTracked != value)
                {
                    _IsTracked = value;
                    if ((_IsTracked))
                    {
                        //PlateNumber.BorderBrush = new SolidColorBrush(Color.FromArgb(255, 218, 59, 59));
                        PlateNumber.Background = new SolidColorBrush(Color.FromArgb(255, 248, 227, 227));
                        minDisplayCarNo.Foreground = new SolidColorBrush(Color.FromArgb(255, 218, 59, 59));
                        //TrackImage = new BitmapImage(new Uri("/GisManagement;component/Image/track.png", UriKind.RelativeOrAbsolute));
                        Canvas.SetZIndex(this, NearestZIndex - 1);
                    }
                    else
                    {
                        //PlateNumber.BorderBrush = new SolidColorBrush(Color.FromArgb(255, 176, 176, 176));
                        PlateNumber.Background = new SolidColorBrush(Color.FromArgb(255, 225, 225, 225));
                        minDisplayCarNo.Foreground = new SolidColorBrush(Color.FromArgb(255, 78, 77, 77));

                        //TrackImage = new BitmapImage(new Uri("/GisManagement;component/Image/track_disnable.png", UriKind.RelativeOrAbsolute));                     
                        Canvas.SetZIndex(this, 0);
                    }
                }
                CarBtn2.IsChecked = value;
                //RaisePropertyChanged("IsTracked");
            }
        }

        public bool _IsSelected;
        public bool IsSelected
        {
            get
            {
                return _IsSelected;
            }
            set
            {
                if (_IsSelected != value)
                {
                    _IsSelected = value;
                    if (_IsSelected)
                    {
                        PlateNumber.BorderBrush = new SolidColorBrush(Color.FromArgb(255, 218, 59, 59));
                        //PlateNumber.Background = new SolidColorBrush(Color.FromArgb(255, 248, 227, 227));
                        //minDisplayCarNo.Foreground = new SolidColorBrush(Color.FromArgb(255, 218, 59, 59));
                        //TrackImage = new BitmapImage(new Uri("/GisManagement;component/Image/track.png", UriKind.RelativeOrAbsolute));
                    }
                    else
                    {
                        PlateNumber.BorderBrush = new SolidColorBrush(Color.FromArgb(255, 176, 176, 176));
                        //PlateNumber.Background = new SolidColorBrush(Color.FromArgb(255, 225, 225, 225));
                        //minDisplayCarNo.Foreground = new SolidColorBrush(Color.FromArgb(255, 78, 77, 77));
                        //TrackImage = new BitmapImage(new Uri("/GisManagement;component/Image/track_disnable.png", UriKind.RelativeOrAbsolute)); 

                        //取消定位的同时取消追踪
                        //IsTracked = false;
                    }
                    RaisePropertyChanged("IsLocated");

                }
            }
        }
        private BitmapImage _TrackImage;
        public BitmapImage TrackImage
        {
            get
            {
                return _TrackImage;
            }
            set
            {
                _TrackImage = value;
                RaisePropertyChanged("TrackImage");
            }
        }
        private Color ReturnColorFromString(string color)
        {
            color = color.Substring(1, color.Length - 1);
            string alpha = color.Substring(0, 2);
            string red = color.Substring(2, 2);
            string green = color.Substring(4, 2);
            string blue = color.Substring(6, 2);

            byte alphabyte = Convert.ToByte(alpha, 16);
            byte redbyte = Convert.ToByte(red, 16);
            byte greenbyte = Convert.ToByte(green, 16);
            byte bluebyte = Convert.ToByte(blue, 16);
            return Color.FromArgb(alphabyte, redbyte, greenbyte, bluebyte);
        }


        private string _RecDateTime;
        public string RecDateTime
        {
            get
            {
                return _RecDateTime;
            }
            set
            {
                _RecDateTime = value;
                RaisePropertyChanged("RecDateTime");
            }
        }

        public string EventID
        {
            get;
            set;
        }

        public VechileGrapicLst Graphics
        {
            get;
            set;
        }

        private string _GroupName;
        public string GroupName
        {
            get
            {
                return _GroupName;
            }
            set
            {
                _GroupName = value;
                RefreshGpsCarVisibility();
            }
        }

        private void RefreshGpsCarVisibility()
        {
            if ((_HasDraw == true) && (GroupName == DisplayGroupName))
            {
                this.Visibility = System.Windows.Visibility.Visible;
            }
            else
            {
                this.Visibility = System.Windows.Visibility.Collapsed;
            }
        }
        private string _DisplayGroupName;
        public string DisplayGroupName
        {
            get
            {
                return _DisplayGroupName;
            }
            set
            {
                _DisplayGroupName = value;
                RefreshGpsCarVisibility();
            }
        }
        private List<short> AlertList;

        public void AddAlert(short at)
        {
            if (AlertList.IndexOf(at) == -1) AlertList.Add(at);
            RefreshCarStyleImage();
            RefreshAlertNameLst();
        }


        private void RefreshAlertNameLst()
        {
            foreach (short at in AlertList)
            {
                if (MemoInfo == "")
                {
                    MemoInfo = TranslateInfo.Translate(((BusinessAlertType)at).ToString());
                }
                else
                {
                    MemoInfo = MemoInfo + Environment.NewLine + TranslateInfo.Translate(((BusinessAlertType)at).ToString());
                }
            }
        }

        public void RemoveAlert(short at)
        {
            if (AlertList.IndexOf(at) > -1) AlertList.Remove(at);
            RefreshCarStyleImage();
            RefreshAlertNameLst();
        }

        private void RefreshCarStyleImage()
        {
            CarImage.Source = CarStyleImage;
            
            RaisePropertyChanged("CarStyle");
            RaisePropertyChanged("CarStyleImageDisplay");
        }

        private string getDisplayLon(string csLon)
        {
            DisplayLonConvert con = new DisplayLonConvert();
            return con.ConvertToWESN(csLon, null, null, null).ToString();
        }

        private string getDisplayLat(string csLat)
        {
            DisplayLatConvert con = new DisplayLatConvert();
            return con.ConvertToWESN(csLat, null, null, null).ToString();
        }

        #region

        private bool _HasDraw;
        public bool HasDraw
        {
            get
            {
                return _HasDraw;
            }
            set
            {
                _HasDraw = value;
                RefreshGpsCarVisibility();
            }
        }


        private ElementLayerDefine _ElementLayDefine;
        public ElementLayerDefine ElementLayDefine
        {
            get
            {
                return _ElementLayDefine;
            }
            set
            {
                _ElementLayDefine = value;
                RefreshCarStyleImage();
            }
        }

        #endregion

        #region

        private bool _FixOperate;
        public bool FixOperate
        {
            get
            {
                return _FixOperate;
            }
            set
            {
                _FixOperate = value;
                if (_FixOperate == false)
                {
                    if (_DisplayVideo == true)
                    {
                        DisplayVideo = false;
                        vedioDisplay.Close();
                    }
                }
                else
                {
                    EventAggregator.Publish<GisFixChangeRoute>(new GisFixChangeRoute()
                    {
                        VechileId = this.CarNo
                        
                    }
                    );
                }
                CarBtn1.IsChecked = value;
                //RaisePropertyChanged("FixOperate");
            }
        }
        private void Car_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            // TODO: Add event handler implementation here.
            if ((ElementLayDefine != ElementLayerDefine.miVERealLocation) && (ElementLayDefine != ElementLayerDefine.miVEHisData)) return;
            if (FixOperate == false)
            {
                //if (ElementLayDefine != ElementLayerDefine.miVEHisData) //播放历史轨迹时，不显示操作盘
                //{
                //    LocateShadowSelected.Visibility = System.Windows.Visibility.Visible;
                //    //CarInfo.Margin = new Thickness(0, 0, 0, 220); 
                //}
                //else
                //{
                //    LocateShadowSelected.Visibility = System.Windows.Visibility.Collapsed;
                //    //  CarInfo.Margin = new Thickness(0,0,0,100);
                //}
                LocateShadowSelected.Visibility = System.Windows.Visibility.Visible;
                LocateShadow.Visibility = System.Windows.Visibility.Collapsed;
                CarInfoShow.Begin();
                SetToFront();
            }

        }
        private void Car_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            // TODO: Add event handler implementation here.
            if ((ElementLayDefine != ElementLayerDefine.miVERealLocation) && (ElementLayDefine != ElementLayerDefine.miVEHisData)) return;
            if (FixOperate == false)
            {
                LocateShadowSelected.Visibility = System.Windows.Visibility.Collapsed;
                LocateShadow.Visibility = System.Windows.Visibility.Visible;
                CarInfoHidden.Begin();
                RestoreZIndex();
            }
        }


        public void SetToFront()
        {
            ZIndex = Canvas.GetZIndex(this);
            Canvas.SetZIndex(this, NearestZIndex);
        }

        public void RestoreZIndex()
        {
            Canvas.SetZIndex(this, ZIndex);
        }

        private int ZIndex = 0;

        private int NearestZIndex = 10000;


        #endregion
        #region
        private bool _RemoveLastPointX;
        private bool _RemoveLastPointY;
        private MapPoint _OldPosition;
        private MapPoint _LastPosition;
        private static readonly DependencyProperty Xproperty = DependencyProperty.Register("X", typeof(double), typeof(GpsCar), new PropertyMetadata(OnXChanged));
        private double X
        {
            get { return (double)base.GetValue(Xproperty); }
            set
            {
                base.SetValue(Xproperty, value);
                if (Y == 0) return;
                UpdateRoute();
                if (_IsTrajectory) DrawRoute(_OldPosition, new MapPoint(X, Y), _RemoveLastPointX, false);
                _RemoveLastPointX = false;
            }
        }
        private static void OnXChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            (sender as GpsCar).X = (double)e.NewValue;
        }
        public static readonly DependencyProperty Yproperty = DependencyProperty.Register("Y", typeof(double), typeof(GpsCar), new PropertyMetadata(OnYChanged));
        private double Y
        {
            get { return (double)base.GetValue(Yproperty); }
            set
            {
                base.SetValue(Yproperty, value);
                if (X == 0) return;
                UpdateRoute();
                if (_IsTrajectory) DrawRoute(_OldPosition, new MapPoint(X, Y), _RemoveLastPointY, false);
                _RemoveLastPointY = false;
                ;
            }
        }
        private static void OnYChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            (sender as GpsCar).Y = (double)e.NewValue;
        }

        public Storyboard sb = new Storyboard();
        DoubleAnimation dba;
        DoubleAnimation dba1;

        private void sb_Completed(object sender, EventArgs e)
        {
            if (_IsTrajectory) DrawRoute(_OldPosition, new MapPoint(X, Y), false, true);

            if (onGetNextPointEvent != null)
            {
                bool sucess = false;

                MapPoint nextpt = new MapPoint();
                onGetNextPointEvent(out sucess, out nextpt);
                if (sucess)
                {
                    UpdateCarRotate(_LastPosition, nextpt);
                    DrawTo(_LastPosition, nextpt, _Interval, _IsTrajectory);
                }
            }
        }

        public delegate void GetNextPointEvent(out bool sucess, out MapPoint pt);
        public GetNextPointEvent onGetNextPointEvent;
        #endregion





        #region
        private bool _IsTrajectory;
        private int _Interval;

        public int Interval
        {
            get
            {
                return _Interval;
            }
            set
            {
                _Interval = value;
            }
        }

        public void UpdateCarRotate(MapPoint pt1, MapPoint pt2)
        {
            double angle = CalulateXYAnagle(pt1.X, pt1.Y, pt2.X, pt2.Y);
            RotateItemCanvas.Angle = angle;
        }

        public void DrawTo(ESRI.ArcGIS.Client.Geometry.MapPoint pt1, ESRI.ArcGIS.Client.Geometry.MapPoint pt2, int csInterval, bool csIsTrajectory)
        {
            try
            {
                _OldPosition = new MapPoint(pt1.X, pt1.Y);
                _LastPosition = new MapPoint(pt2.X, pt2.Y);
                _IsTrajectory = csIsTrajectory;
                _Interval = csInterval;

                //ElementLayer.SetEnvelope(this, pt1.Extent);
                if (dba == null)
                {
                    dba = new DoubleAnimation();

                    Storyboard.SetTarget(dba, this);
                    Storyboard.SetTargetProperty(dba, new PropertyPath("X"));
                    sb.Children.Add(dba);

                    dba1 = new DoubleAnimation();
                    Storyboard.SetTarget(dba1, this);
                    Storyboard.SetTargetProperty(dba1, new PropertyPath("Y"));

                    sb.Children.Add(dba1);

                    sb.Completed += new EventHandler(sb_Completed);
                }

                _RemoveLastPointX = false;
                _RemoveLastPointY = false;
                dba.From = pt1.X;
                dba.To = pt2.X;
                dba.Duration = new Duration(TimeSpan.FromMilliseconds(_Interval));

                dba1.From = pt1.Y;
                dba1.To = pt2.Y;
                dba1.Duration = new Duration(TimeSpan.FromMilliseconds(_Interval));
                sb.Begin();
            }
            catch (Exception e)
            {
                ApplicationContext.Instance.Logger.LogException("gpscar", e);
            }

        }

        public static double CalulateXYAnagle(double startx, double starty, double endx, double endy)
        {
            return 0;

        }

        private void UpdateRoute()
        {
            //RouteCarList.RemoveCars(this);
            ElementLayer.SetEnvelope(this, new ESRI.ArcGIS.Client.Geometry.Envelope(X, Y, X, Y));

            //RouteCarList.AddCars(this);
        }
        private bool NeedDrawArrow(MapPoint pt1, MapPoint pt2)
        {
            //距离超过10m绘制
            if ((Math.Sqrt(Math.Pow(pt2.X - pt1.X, 2) + Math.Pow(pt2.Y - pt1.Y, 2))) > 30) return true;
            return false;
        }
        private ESRI.ArcGIS.Client.Geometry.Polyline GetArrowLine(MapPoint pt, double lineangle)
        {
            int len = 20;
            double angle = Math.PI / 6;

            ESRI.ArcGIS.Client.Geometry.PointCollection newpts = new ESRI.ArcGIS.Client.Geometry.PointCollection();

            MapPoint pt1 = new MapPoint();
            pt1.X = pt.X + len * Math.Cos(Math.PI - angle + lineangle);
            pt1.Y = pt.Y + len * Math.Sin(Math.PI - angle + lineangle);

            MapPoint pt2 = new MapPoint();
            pt2.X = pt.X + len * Math.Cos(Math.PI + angle + lineangle);
            pt2.Y = pt.Y + len * Math.Sin(Math.PI + angle + lineangle);

            newpts.Add(pt1);
            newpts.Add(pt);
            newpts.Add(pt2);
            ESRI.ArcGIS.Client.Geometry.Polyline line = new ESRI.ArcGIS.Client.Geometry.Polyline();
            line.Paths.Add(newpts);
            return line;
        }
        public void DrawRoute(MapPoint oldpt, MapPoint newpt, bool RemoveLastPoint, bool NewPtMustDraw)
        {
            try
            {
                ESRI.ArcGIS.Client.Geometry.PointCollection newpts;
                Graphic graphic = MonitorList.VechileRealLocationGraphics.GetGraphics(this.UniqueID + "@" + "Trace");
                if (graphic == null)
                {
                    newpts = new ESRI.ArcGIS.Client.Geometry.PointCollection();
                    newpts.Add(oldpt);
                    newpts.Add(newpt);
                }
                else
                {
                    if ((!NewPtMustDraw) && (DateTime.Now < FLastUpdateTime.AddMilliseconds(ConstDefine.UpdateDisplayInterval))) return;
                    newpts = new ESRI.ArcGIS.Client.Geometry.PointCollection();

                    foreach (MapPoint pt in (graphic.Geometry as ESRI.ArcGIS.Client.Geometry.Polyline).Paths[0])
                    {
                        newpts.Add(pt);
                    }
                    if (RemoveLastPoint) newpts.RemoveAt(newpts.Count - 1);
                    newpts.Add(newpt);

                }

                for (int i = 0; i < newpts.Count - 1; i++)
                {

                  
                    Graphic Locategraphic = new Graphic()
                    {
                        Geometry = newpts[i],
                        Symbol = new ESRI.ArcGIS.Client.Symbols.SimpleMarkerSymbol
                        {
                            Style = ESRI.ArcGIS.Client.Symbols.SimpleMarkerSymbol.SimpleMarkerStyle.Circle,
                            Size = 8,
                            Color = new SolidColorBrush(Colors.Red),
                        }
                    };

                    MonitorList.VechileRealLocationGraphics.AddGraphic(Locategraphic, UniqueID + "@" + "Locate-" + i.ToString());

                    //if (NeedDrawArrow(newpts[i], newpts[i + 1]))
                    //{
                        //double angleofline = Math.Atan2(newpts[i + 1].Y - newpts[i].Y, newpts[i + 1].X - newpts[i].X);
                        //MapPoint pt = new MapPoint();
                        //pt.X = (newpts[i].X + newpts[i + 1].X) / 2;
                        //pt.Y = (newpts[i].Y + newpts[i + 1].Y) / 2;

                        //Graphic arrowgraphic = new Graphic()
                        //{
                        //    Geometry = GetArrowLine(pt, angleofline),
                        //    Symbol = new ESRI.ArcGIS.Client.Symbols.SimpleLineSymbol()
                        //    {
                        //        Style = ESRI.ArcGIS.Client.Symbols.SimpleLineSymbol.LineStyle.Solid,
                        //        Width = 2,
                        //        Color = new SolidColorBrush(Colors.Red),
                        //    }
                        //};
                        //MonitorList.VechileRealLocationGraphics.AddGraphic(arrowgraphic, UniqueID + "@" + "Arrow-" + i.ToString());
                    //}
                }

                ESRI.ArcGIS.Client.Geometry.Polyline line = new ESRI.ArcGIS.Client.Geometry.Polyline();
                line.Paths.Add(newpts);

                Graphic newgraphic = new Graphic()
                {
                    Geometry = line,
                    Symbol = new ESRI.ArcGIS.Client.Symbols.SimpleLineSymbol()
                    {
                        Style = ESRI.ArcGIS.Client.Symbols.SimpleLineSymbol.LineStyle.Dash,
                        Width = 2,
                        Color = new SolidColorBrush(Colors.Red),
                    }
                };
               // this.Graphics.AddGraphic(newgraphic, UniqueID + "@" + "Trace");
                MonitorList.VechileRealLocationGraphics.AddGraphic(newgraphic, UniqueID + "@" + "Trace" );
                FLastUpdateTime = DateTime.Now;
            }
            catch (Exception e)
            {
                ApplicationContext.Instance.Logger.LogException("gpscar", e);
            }
        }
        #endregion

        #region
        public event PropertyChangedEventHandler PropertyChanged;
        protected void RaisePropertyChanged(string propertyName)
        {
            var handle = this.PropertyChanged;
            if (handle != null)
            {
                handle(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        #endregion



        private void LayoutRoot_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {

        }

        private void LayoutRoot_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
        }

        private void CarBtn1_Click(object sender, RoutedEventArgs e)
        {
            this.FixOperate = !this.FixOperate;

        }

        private void CarBtn2_Click(object sender, RoutedEventArgs e)
        {
            IsTracked = !IsTracked;
            //if (IsTracked == true)//将其他车的跟踪状态设置为false
            //{
            //    foreach (GpsCar car in MonitorList.VechileRealLocationElements.Elements)
            //    {
            //        if (car.UniqueID != this.UniqueID)
            //        {
            //            if (car.IsTracked) car.IsTracked = false;
            //        }
            //    }
            //}
        }

        private void CarBtn3_Click(object sender, RoutedEventArgs e)
        {
            EventAggregator.Publish<DisplayHistoricalRoute>(new DisplayHistoricalRoute()
            {
                VechileId = this.CarNo,
                StartTime = DateTime.Now.AddHours(-1),
                EndTime = DateTime.Now
            }
            );
        }



        internal GpsCar ConleGPSCar()
        {
            GpsCar car = new GpsCar(this._CarNo);
            car._AlarmFlag = this._AlarmFlag;
            car._AlertFlag = this._AlertFlag;
            car.Dir = this.Dir;
            car.RecLat = this.RecLat;
            car.RecLon = this.RecLon;
            car.GpsTime = this.GpsTime;
            car.GroupName = this.GroupName;
            car.EventID = this.EventID;
            car.DisplayGroupName = this.DisplayGroupName;
            car.IsSelected = this.IsSelected;
            car.FixOperate = this.FixOperate;
            car.IsTracked = this.IsTracked;
            car.HasDraw = this.HasDraw;

            //if ((car.Lat != null) && (car.Lon != null)&&(car.Lat!="")&&(car.Lon!=""))
            //{
            //    ESRI.ArcGIS.Client.Geometry.MapPoint pt = new ESRI.ArcGIS.Client.Geometry.MapPoint();
            //    double lslon = double.Parse(car.Lon);
            //    double lslat = double.Parse(car.Lat);

            //    pt = VechileMemDataOperate.GetProjCoord(lslon, lslat);
            //    ElementLayer.SetEnvelope(car, new ESRI.ArcGIS.Client.Geometry.Envelope(pt, pt));
            //}
            return car;
        }

        public void OnImportsSatisfied()
        {
            //
        }

        private void VideoButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void CarBtn4_Click(object sender, RoutedEventArgs e)
        {
            if (GetVideoPlayCount() >= 4)
            {
                MessageBoxHelper.ShowDialog(ApplicationContext.Instance.StringResourceReader.GetString("Tip"), ApplicationContext.Instance.StringResourceReader.GetString("GIS_TooManyVideo"), MessageDialogButton.Ok);
                return;
            }
            //   
            Gsafety.PTMS.Bases.Models.Vehicle vehicle = ApplicationContext.Instance.BufferManager.VehicleOrganizationManage.GetVehicle(this._CarNo);
            if (vehicle == null) return;
            var window = new CameraSelectWindow(vehicle.UniqueId, 1);
            window.Closed += (m, n) =>
                {
                    if (window.DialogResult == true)
                    {
                        if (window.SelectResult.Count >= 1)
                        {
                            //rtsp://172.16.10.199:8554/local/test4.ts
                            // sample:"rtsp://172.16.10.199:8554/live/0:1:99999AA12311";

                            //var paras = window.SelectResult[0].Url.Split(':');
                            //var url = string.Format("rtsp://{0}:{1}/live/{2}:{3}:{4}", ApplicationContext.Instance.ServerConfig.RTSPServiceIP, ApplicationContext.Instance.ServerConfig.RTSPServicePort, paras[0], ApplicationContext.Instance.ServerConfig.RTSPStreamChannel, paras[1]);

                            vedioDisplay.Url = MediaPlayerContainer.GenerateRealVideoUrl(window.SelectResult[0]);
                            vedioDisplay.RealVideoMode = true;
                            vedioDisplay.Connect();

                            this.DisplayVideo = true;
                            this.FixOperate = true;
                        }
                        else
                        {
                            this.DisplayVideo = false;
                        }
                    }
                };
            window.Show();

        }


        private int GetVideoPlayCount()
        {
            int num = 0;
            foreach (GpsCar car in MonitorList.VechileRealLocationElements.Elements)
            {
                if (car.DisplayVideo == true) num++;
            }
            return num;
        }

        public bool DisplayBaseInfo
        {
            get
            {
                return !_DisplayVideo;
            }
        }
        private bool _DisplayVideo;
        public bool DisplayVideo
        {
            get
            {
                return _DisplayVideo;
            }
            set
            {
                _DisplayVideo = value;
                RaisePropertyChanged("DisplayVideo");
                RaisePropertyChanged("DisplayBaseInfo");
            }
        }

        private void Grid_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.FixOperate = !this.FixOperate;
        }
    }
}
