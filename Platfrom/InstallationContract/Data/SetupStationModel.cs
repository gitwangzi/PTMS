using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Gsafety.Ant.Installation.Contract.Data
{
    [DataContract]
    public class SetupStationModel //: INotifyPropertyChanged
    {
        //public event PropertyChangedEventHandler PropertyChanged = delegate { };

        //private void OnPropertyChanged(string propertyName)
        //{
        //    PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        //}

        [DataMember]
        public string ID { get; set; }

        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public string Address { get; set; }

        [DataMember]
        public string Director { get; set; }

        [DataMember]
        public string DirectorPhone { get; set; }

        [DataMember]
        public string Contact { get; set; }

        [DataMember]
        public string ContactPhone { get; set; }

        [DataMember]
        public string Email { get; set; }

        [DataMember]
        public string Note { get; set; }

        [DataMember]
        public string RegionCode { get; set; }

        [DataMember]
        public string ProvinceCode { get; set; }

        [DataMember]
        public string ProvinceName { get; set; }

        [DataMember]
        public string CityCode { get; set; }

        [DataMember]
        public string CityName { get; set; }

        //[DataMember]
        //public StationType? StationType
        //{
        //    get
        //    {

        //        StationType result = Enum.StationType.InstallationStation;
        //        var num = ((int)this.StationType_Aide).ToString();
        //        System.Enum.TryParse(num, out result);
        //        return result;
        //    }
        //    set
        //    {
        //        if (StationType_Aide != null)
        //        {                    
        //            StationType_Aide = (int)value;
        //        }
        //    }
        //}


        //public decimal? StationType_Aide;
    }
}
