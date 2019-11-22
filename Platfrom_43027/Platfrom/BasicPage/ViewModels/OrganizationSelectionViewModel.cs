using Gsafety.PTMS.Bases.Models;
using Gsafety.PTMS.ServiceReference.MessageService;
using Gsafety.PTMS.Share;
using Jounce.Core.Event;
using Jounce.Core.Model;
using Jounce.Core.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Linq;
using System.Threading;
using Gsafety.PTMS.Bases.Librarys;
using Gsafety.PTMS.BasicPage.Model;
using Gsafety.PTMS.ServiceReference.AccountService;
using System.Reflection;

namespace Gsafety.PTMS.BasicPage.ViewModels
{
    public class OrganizationSelectionViewModel : BaseViewModel
    {
        private string _userID;

        public OrganizationTreeFactory VehicleTreeFactory { get; set; }

        public OrganizationSelectionViewModel(string userID)
        {
            _userID = userID;

            VehicleTreeFactory = new OrganizationTreeFactory(Dataload);
        }

        private void Dataload()
        {
            InitServiceClient();
            GetUserVehicleOrg(_userID);
        }

        private void client_GetUserVehicleOrgCompleted(object sender, GetUserVehicleOrgCompletedEventArgs e)
        {
            try
            {
                foreach (var ite in VehicleTreeFactory.OrgnizationVehicleTrees)
                {
                    foreach (var item in e.Result.Result)
                    {
                        if (ite.Organization.ID == item.OrganizationId)
                        {
                            //ite.IsChecked = true;
                            ite.InternalCheck = true;
                            break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(), ex);
            }
            finally
            {
                CloseClient();
            }
        }
        private void client_InsertUserVehicleOrgCompleted(object sender, InsertUserVehicleOrgCompletedEventArgs e)
        {
            CloseClient();
        }

        private UserServiceClient client;
        private string currentUserID;

        public void InitServiceClient()
        {
            client = ServiceClientFactory.Create<UserServiceClient>();
            ServiceClientFactory.CreateMessageHeader(client.InnerChannel);
            client.GetUserVehicleOrgCompleted += client_GetUserVehicleOrgCompleted;
            client.InsertUserVehicleOrgCompleted += client_InsertUserVehicleOrgCompleted;
        }

        public void GetUserVehicleOrg(string userID)
        {
            currentUserID = null;
            client.GetUserVehicleOrgAsync(userID);
            currentUserID = userID;
        }

        public void SaveVehicleOrg()
        {
            try
            {
                InitServiceClient();
                var result = VehicleTreeFactory.OrgnizationVehicleTrees.Where(t => t.IsChecked).Select(t => t.Organization.ID).ToList();

                client.InsertUserVehicleOrgAsync(new ObservableCollection<string>(result), currentUserID);
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(), ex);
            }
        }

        private void CloseClient()
        {
            if (this.client != null)
            {
                this.client.CloseAsync();
            }
            this.client = null;
        }
    }
}
