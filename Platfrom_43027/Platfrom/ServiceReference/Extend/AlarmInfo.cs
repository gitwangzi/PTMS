using Gsafety.PTMS.Bases.Models;
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

namespace Gsafety.PTMS.ServiceReference.VehicleAlarmService
{
    public partial class AlarmInfoEx
    {
        public AlarmInfoEx Self
        {
            get
            {
                return this;
            }
        }

        public bool IsHandled
        {
            get
            {
                return AppealStatus == 4;
            }
            set
            {
                RaisePropertyChanged("IsHandled");
                RaisePropertyChanged("Self");
            }
        }

        public Vehicle VehicleInfo { get; set; }

        //public bool IsOnLine { get; set; }

        public bool IsAlive { get; set; }

        public bool IsDesignated { get; set; }

        private bool _IsMarkGraphic;
        public bool IsMarkGraphic
        {
            get
            {
                return this._IsMarkGraphic;
            }
            set
            {
                _IsMarkGraphic = value;
                this.RaisePropertyChanged("IsMarkGraphic");
            }
        }
    }
}
