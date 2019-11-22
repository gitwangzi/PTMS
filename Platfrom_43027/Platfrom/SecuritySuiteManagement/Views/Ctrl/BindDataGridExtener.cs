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
    internal class BindDataGridExtener<T> : INotifyPropertyChanged
    {
        private DataGrid _grid;

        public BindDataGridExtener(DataGrid grid)
        {
            this._grid = grid;
            this.GridSource = new ReadOnlyObservableCollection<T>(new ObservableCollection<T>());
        }

        private ReadOnlyObservableCollection<T> _gridSource;

        public ReadOnlyObservableCollection<T> GridSource
        {
            get { return _gridSource; }
            set
            {
                if (value != _gridSource)
                {
                    _gridSource = value;
                    OnPropertyChanged("GridSource");
                }
            }

        }


        private void InitGrid()
        {
          //  var col=DataGridColumn

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
