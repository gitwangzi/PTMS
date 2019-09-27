using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Linq;

namespace Gsafety.Ant.SecuritySuite.Views.Ctrl
{
    public partial class CusBindDataGrid : UserControl, INotifyPropertyChanged
    {
        private PagedCollectionView _pagedSource;

        public PagedCollectionView PagedSource
        {
            get { return _pagedSource; }
            set
            {
                if (_pagedSource != value)
                {
                    _pagedSource = value;
                    OnPropertyChanged("PagedSource");
                }
            }

        }
        

       // private ReadOnlyObservableCollection<

        public void SetGridSource<T>(IEnumerable<T> soruce)
        {


        }

        public CusBindDataGrid()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            
        }

        private void dp_page_PageIndexChanged(object sender, EventArgs e)
        {

        }

        private void OnPropertyChanged(string name)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(name));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        
        
    }


}
