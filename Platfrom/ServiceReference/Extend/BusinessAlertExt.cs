using Gsafety.PTMS.Bases.Models;

namespace Gsafety.PTMS.ServiceReference.VehicleAlertService
{
    public partial class BusinessAlertEx
    {
        //public bool IsOnLine { get; set; }

        public Vehicle VehicleInfo { get; set; }


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
