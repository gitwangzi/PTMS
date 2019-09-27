using Gsafety.PTMS.Bases.Models;

namespace Gsafety.PTMS.ServiceReference.DeviceAlertService
{
    public partial class DeviceAlertEx
    {
        //public bool IsOnLine { get; set; }

        public Vehicle VehicleInfo { get; set; }
   
        public string VehicleOwner { get; set; }//车主

      
        public string OwnerPhone { get; set; }//车主电话  

        public string OrganizationId { get; set; }

      
        public string OrganizationName { get; set; }

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
