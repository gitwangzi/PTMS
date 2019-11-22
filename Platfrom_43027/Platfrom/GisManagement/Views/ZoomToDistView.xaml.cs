using Gsafety.PTMS.Share;
using Gsafety.Common.CommMessage;
using Jounce.Core.Event;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Gsafety.PTMS.Bases.Models;

namespace GisManagement.Views
{
    public partial class ZoomToDistView : UserControl
    {
        [Import]
        public IEventAggregator _EventAggregator { get; set; }
        public ZoomToDistView()
        {
            CompositionInitializer.SatisfyImports(this);
            InitializeComponent();
            this.cmbPrivince.DisplayMemberPath = "Name";
            List<Province> list = new List<Province>(ApplicationContext.Instance.BufferManager.DistrictManager.Provinces);

            var sortedList = list.OrderBy(a => a.Name);
            list = sortedList.ToList();
            //if (list != null)
            //{
            //    Province pro = new Province();
            //    pro.Name = ApplicationContext.Instance.StringResourceReader.GetString("All");
            //    pro.Code = null;
            //    list.Insert(0, pro);
            //}
            this.cmbPrivince.ItemsSource = list;
        }

        private void cmbPrivince_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbPrivince.SelectedIndex == -1 || cmbPrivince.SelectedValue == null)
                return;
            Province pro = cmbPrivince.SelectedValue as Province;
            if (pro != null && pro.Code != null)
                _EventAggregator.Publish<ZoomToDistByDistCode>(new ZoomToDistByDistCode() { DISTTYPE = Gsafety.PTMS.Bases.Enums.ZoomToDistType.ZoomToProvince, DISTCODE = pro.Code });
            this.cmbCity.DisplayMemberPath = "Name";
            List<City> list = new List<City>();
            if (pro != null && pro.Code != null)
            {
                list = new List<City>(pro.Citys);
                var sortedList = list.OrderBy(a => a.Name);
                list = sortedList.ToList();
            }
            //City city = new City(null);
            //city.Name =ApplicationContext.Instance.StringResourceReader.GetString("All");
            //city.Code = null;
            //list.Insert(0, city);
            this.cmbCity.ItemsSource = list;
        }

        private void cmbCity_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbCity.SelectedIndex == -1 || cmbCity.SelectedValue == null)
                return;
            City pro = cmbCity.SelectedValue as City;
            if (pro != null && pro.Code != null)
            {
                _EventAggregator.Publish<ZoomToDistByDistCode>(new ZoomToDistByDistCode() { DISTTYPE = Gsafety.PTMS.Bases.Enums.ZoomToDistType.ZoomToCity, DISTCODE = pro.Code });
                //  ApplicationContext.Instance.BufferManager.
                //  EventAggregator.Publish<ShowBusStopInfoArgs>(new ShowBusStopInfoArgs() { SelectStop = _sleBusStop });
                // ApplicationContext.Instance.MessageManager.
            }
        }
    }
}
