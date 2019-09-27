using System.Collections.ObjectModel;

namespace Gsafety.PTMS.Bases.Models
{
    public class Province
    {
        public delegate void RefreshEvent();
        public event RefreshEvent refreshEvent;
        private ObservableCollection<City> _Citys;

        public string Code { get; set; }

        public string Name { get; set; }

        public ObservableCollection<City> Citys
        {
            get { return _Citys; }
            set
            {
                _Citys = value;
            }
        }

        public Province()
        {
            Citys = new ObservableCollection<City>();
        }

        public void UiRefresh()
        {
            if (refreshEvent != null)
            {
                refreshEvent();
            }
        }

    }
}
