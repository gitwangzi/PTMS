using BaseLib.ViewModels;
using Gsafety.PTMS.ServiceReference.CommandManageService;
using Gsafety.PTMS.Share;
using Jounce.Core.View;
using Jounce.Core.ViewModel;
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
namespace Gsafety.PTMS.Manager.ViewModels
{
    [ExportAsViewModel("")]
    public class HeartbeatVehicleDetailViewModel : DetailViewModel<HeartbeatVehicle>
    {
        protected string action;
        public HeartbeatVehicleDetailViewModel()
        {
        }
        protected override void ActivateView(string viewName, System.Collections.Generic.IDictionary<string, object> viewParameters)
        {
            base.ActivateView(viewName, viewParameters);
            action = viewParameters["action"].ToString();
            switch (action)
            {
                case "view":
                    Title = ApplicationContext.Instance.StringResourceReader.GetString("Common_View");
                    IsReadOnly = true;
                    ViewVisibility = Visibility.Collapsed;
                    InitialModel = viewParameters["view"] as HeartbeatVehicle;
                    InitialFromInitialModel();
                    break;
                case "update":
                    Title = ApplicationContext.Instance.StringResourceReader.GetString("Common_Update");
                    IsReadOnly = false;
                    ViewVisibility = Visibility.Visible;
                    InitialModel = viewParameters["view"] as HeartbeatVehicle;
                    InitialFromInitialModel();
                    CurrentModel = new HeartbeatVehicle();
                    break;
                case "add":
                    Title = ApplicationContext.Instance.StringResourceReader.GetString("Common_Add");
                    IsReadOnly = false;
                    ViewVisibility = Visibility.Visible;
                    CurrentModel = new HeartbeatVehicle();
                    Reset();
                    break;
                default:
                    break;
            }
        }
        protected override void Reset()
        {
            ID = string.Empty;
            HeartbeatID = string.Empty;
            MdvrCoreSn = string.Empty;
            Creator = string.Empty;
            VehicleID = string.Empty;
        }
        public void InitialFromInitialModel()
        {
            ID = InitialModel.ID;
            HeartbeatID = InitialModel.HeartbeatID;
            MdvrCoreSn = InitialModel.MdvrCoreSn;
            //SendTime = InitialModel.SendTime;
            //Status = InitialModel.Status;
            //PacketSeq = InitialModel.PacketSeq;
            //Creator = InitialModel.Creator;
            //VehicleID = InitialModel.VehicleID;
            //CreateTime = InitialModel.CreateTime;
        }
        protected override void ValidateAll()
        {
            ValidateID(ExtractPropertyName(() => ID), _id);
            ValidateHeartbeatID(ExtractPropertyName(() => HeartbeatID), _heartbeatid);
            ValidateMdvrCoreSn(ExtractPropertyName(() => MdvrCoreSn), _mdvrcoresn);
            ValidateSendTime(ExtractPropertyName(() => SendTime), _sendtime);
            ValidateStatus(ExtractPropertyName(() => Status), _status);
            ValidatePacketSeq(ExtractPropertyName(() => PacketSeq), _packetseq);
            ValidateCreator(ExtractPropertyName(() => Creator), _creator);
            ValidateVehicleID(ExtractPropertyName(() => VehicleID), _vehicleid);
            ValidateCreateTime(ExtractPropertyName(() => CreateTime), _createtime);
        }
        protected override void OnCommitted()
        {
            CurrentModel.ID = ID;
            CurrentModel.HeartbeatID = HeartbeatID;
            CurrentModel.MdvrCoreSn = MdvrCoreSn;
            //CurrentModel.SendTime = SendTime;
            //CurrentModel.Status = Status;
            //CurrentModel.PacketSeq = PacketSeq;
            //CurrentModel.Creator = Creator;
            //CurrentModel.VehicleID = VehicleID;
            //CurrentModel.CreateTime = CreateTime;
            if (action.Equals("update"))
            {
                Update();
            }
            else
            {
                Add();
            }
        }
        protected override void Return()
        {
            EventAggregator.Publish(new ViewNavigationArgs());
        }
        protected void Add()
        {
        }
        protected void Update()
        {
        }
        private string _id;
        public string ID
        {
            get { return _id; }
            set
            {
                _id = value == null ? null : value.Trim();
                ValidateID(ExtractPropertyName(() => ID), _id);
                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => ID));
            }
        }
        private void ValidateID(string prop, string value)
        {
            ClearErrors(prop);
            if (string.IsNullOrEmpty(ID))
            {
                base.SetError(prop, "帐户不允许为空");
            }
        }
        private string _heartbeatid;
        public string HeartbeatID
        {
            get { return _heartbeatid; }
            set
            {
                _heartbeatid = value == null ? null : value.Trim();
                ValidateHeartbeatID(ExtractPropertyName(() => HeartbeatID), _heartbeatid);
                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => HeartbeatID));
            }
        }
        private void ValidateHeartbeatID(string prop, string value)
        {
            ClearErrors(prop);
            if (string.IsNullOrEmpty(HeartbeatID))
            {
                base.SetError(prop, "帐户不允许为空");
            }
        }
        private string _mdvrcoresn;
        public string MdvrCoreSn
        {
            get { return _mdvrcoresn; }
            set
            {
                _mdvrcoresn = value == null ? null : value.Trim();
                ValidateMdvrCoreSn(ExtractPropertyName(() => MdvrCoreSn), _mdvrcoresn);
                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => MdvrCoreSn));
            }
        }
        private void ValidateMdvrCoreSn(string prop, string value)
        {
            ClearErrors(prop);
            if (string.IsNullOrEmpty(MdvrCoreSn))
            {
                base.SetError(prop, "帐户不允许为空");
            }
        }
        private string _sendtime;
        public string SendTime
        {
            get { return _sendtime; }
            set
            {
                _sendtime = value == null ? null : value.Trim();
                ValidateSendTime(ExtractPropertyName(() => SendTime), _sendtime);
                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => SendTime));
            }
        }
        private void ValidateSendTime(string prop, string value)
        {
            ClearErrors(prop);
            if (string.IsNullOrEmpty(SendTime))
            {
                base.SetError(prop, "帐户不允许为空");
            }
        }
        private string _status;
        public string Status
        {
            get { return _status; }
            set
            {
                _status = value == null ? null : value.Trim();
                ValidateStatus(ExtractPropertyName(() => Status), _status);
                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => Status));
            }
        }
        private void ValidateStatus(string prop, string value)
        {
            ClearErrors(prop);
            if (string.IsNullOrEmpty(Status))
            {
                base.SetError(prop, "帐户不允许为空");
            }
        }
        private string _packetseq;
        public string PacketSeq
        {
            get { return _packetseq; }
            set
            {
                _packetseq = value == null ? null : value.Trim();
                ValidatePacketSeq(ExtractPropertyName(() => PacketSeq), _packetseq);
                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => PacketSeq));
            }
        }
        private void ValidatePacketSeq(string prop, string value)
        {
            ClearErrors(prop);
            if (string.IsNullOrEmpty(PacketSeq))
            {
                base.SetError(prop, "帐户不允许为空");
            }
        }
        private string _creator;
        public string Creator
        {
            get { return _creator; }
            set
            {
                _creator = value == null ? null : value.Trim();
                ValidateCreator(ExtractPropertyName(() => Creator), _creator);
                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => Creator));
            }
        }
        private void ValidateCreator(string prop, string value)
        {
            ClearErrors(prop);
            if (string.IsNullOrEmpty(Creator))
            {
                base.SetError(prop, "帐户不允许为空");
            }
        }
        private string _vehicleid;
        public string VehicleID
        {
            get { return _vehicleid; }
            set
            {
                _vehicleid = value == null ? null : value.Trim();
                ValidateVehicleID(ExtractPropertyName(() => VehicleID), _vehicleid);
                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => VehicleID));
            }
        }
        private void ValidateVehicleID(string prop, string value)
        {
            ClearErrors(prop);
            if (string.IsNullOrEmpty(VehicleID))
            {
                base.SetError(prop, "帐户不允许为空");
            }
        }
        private string _createtime;
        public string CreateTime
        {
            get { return _createtime; }
            set
            {
                _createtime = value == null ? null : value.Trim();
                ValidateCreateTime(ExtractPropertyName(() => CreateTime), _createtime);
                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => CreateTime));
            }
        }
        private void ValidateCreateTime(string prop, string value)
        {
            ClearErrors(prop);
            if (string.IsNullOrEmpty(CreateTime))
            {
                base.SetError(prop, "帐户不允许为空");
            }
        }
    }
}

