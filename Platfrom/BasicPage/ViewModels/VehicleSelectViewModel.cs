using Gsafety.PTMS.Bases.Models;
using Gsafety.PTMS.Share;
using Jounce.Core.ViewModel;
using Jounce.Framework.Command;
using System;
using System.Collections.Generic;
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

namespace Gsafety.PTMS.BasicPage.ViewModels
{
    public class VehicleSelectViewModel : BaseViewModel
    {
        private string _fiterText = null;
        /// <summary>
        ///Vehicles query text
        /// </summary>
        public string FilterText
        {
            get
            {
                return _fiterText;
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    _fiterText = null;
                }
                else
                {
                    _fiterText = value.Trim();
                }

                RaisePropertyChanged(() => FilterText);
            }
        }

        public VehicleTreeFactory VehicleTreeFactory { get; set; }

        public ICommand VehicleSearchCommand { get; private set; }

        public VehicleSelectViewModel(Func<Vehicle, bool> filter = null)
        {
            try
            {
                VehicleSearchCommand = new ActionCommand<object>((treeview) => VehicleSearch(treeview));
                VehicleTreeFactory = new VehicleTreeFactory(filter);
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(), ex);
            }
        }

        public void VehicleSearch(object treeview)
        {
            VehicleTreeFactory.SearchVehicleTree(FilterText);
        }
    }
}
