using Gsafety.PTMS.ServiceReference.BscDevSuiteService;
using Gsafety.PTMS.ServiceReference.VehicleService;
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

namespace Gsafety.Ant.BaseInformation.Model
{
    public class VehicleExportModel
    {
        public string ProvinceName { get; set; }
        public string CityName { get; set; }
        public Gsafety.PTMS.ServiceReference.VehicleService.InstallStatusType InstallStatus { get; set; }
        public string VehicleId { get; set; }
        public string OrgnizationName { get; set; }
        public string VehicleSn { get; set; }
        public string EngineId { get; set; }
        public string BrandModel { get; set; }
        public string DistrictCode { get; set; }
        public string OperationLicense { get; set; }
        public string Owner { get; set; }
        public string Contact { get; set; }
        public string ContactAddress { get; set; }
        public string ContactEmail { get; set; }
        public string ContactPhone { get; set; }
        public string Region { get; set; }
        public string StartYear { get; set; }
        public VehicleSeviceType ServiceType { get; set; }
        public string Note { get; set; }
        public string VehicleType { get; set; }

        public string Ficha { get; set; }

        public DateTime? ProductDate { get; set; }
    }
}
