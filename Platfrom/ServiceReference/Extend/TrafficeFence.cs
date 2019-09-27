using System;
using System.Net;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace Gsafety.PTMS.ServiceReference.TrafficManageService
{
    public partial class TrafficFence
    {
        private bool _isSelct = false;
        public bool IsSelect
        {
            get { return _isSelct; }
            set
            {
                _isSelct = value;
                RaisePropertyChanged("IsSelect");
            }
        }

        private bool _IsmarkFenceGraphic;
        public bool IsmarkFenceGraphic
        {
            get
            {
                return this._IsmarkFenceGraphic;
            }
            set
            {
                _IsmarkFenceGraphic = value;
                this.RaisePropertyChanged("IsmarkFenceGraphic");
            }
        }
        private string _DisplayName;
        public string DisplayName
        {
            get
            {

                _DisplayName = this.Name + "(" + this.CreateTime.ToLocalTime().ToString() + ")";
                return _DisplayName;
            }
            set
            {
                _DisplayName = value;
                this.RaisePropertyChanged("DisplayName");
            }
        }

        public TrafficFence Clone(object o)
        {
            Type t = o.GetType();
            PropertyInfo[] properties = t.GetProperties();
            object p = t.InvokeMember("", System.Reflection.BindingFlags.CreateInstance, null, o, null);
            foreach (PropertyInfo pi in properties)
            {
                if (pi.CanWrite)
                {
                    object value = pi.GetValue(o, null);
                    pi.SetValue(p, value, null);
                }
            }
            return p as TrafficFence;
        }

        public void SetProperty(TrafficFence Source)
        {
            Type t = Source.GetType();
            PropertyInfo[] properties = t.GetProperties();
            foreach (PropertyInfo pi in properties)
            {
                if (pi.CanWrite)
                {
                    object value = pi.GetValue(Source, null);
                    pi.SetValue(this, value, null);
                }
            }
        }
    }

    public partial class TrafficRoute
    {
        private bool _isSelct = false;
        public bool IsSelect
        {
            get { return _isSelct; }
            set
            {
                _isSelct = value;
                RaisePropertyChanged("IsSelect");
            }
        }

        private bool _IsmarkRouteGraphic;
        public bool IsmarkRouteGraphic
        {
            get
            {
                return this._IsmarkRouteGraphic;
            }
            set
            {
                _IsmarkRouteGraphic = value;
                this.RaisePropertyChanged("IsmarkRouteGraphic");
            }
        }
        private string _DisplayName;
        public string DisplayName
        {
            get
            {

                _DisplayName = this.Name + "(" + this.CreateTime.ToLocalTime().ToString() + ")";
                return _DisplayName;
            }
            set
            {
                _DisplayName = value;
                this.RaisePropertyChanged("DisplayName");
            }
        }

        public TrafficRoute Clone(object o)
        {
            Type t = o.GetType();
            PropertyInfo[] properties = t.GetProperties();
            object p = t.InvokeMember("", System.Reflection.BindingFlags.CreateInstance, null, o, null);
            foreach (PropertyInfo pi in properties)
            {
                if (pi.CanWrite)
                {
                    object value = pi.GetValue(o, null);
                    pi.SetValue(p, value, null);
                }
            }
            return p as TrafficRoute;
        }

        public void SetProperty(TrafficRoute Source)
        {
            Type t = Source.GetType();
            PropertyInfo[] properties = t.GetProperties();
            foreach (PropertyInfo pi in properties)
            {
                if (pi.CanWrite)
                {
                    object value = pi.GetValue(Source, null);
                    pi.SetValue(this, value, null);
                }
            }
        }
    }
}
