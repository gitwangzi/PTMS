﻿//------------------------------------------------------------------------------
// <auto-generated>
//     此代码由工具生成。
//     运行时版本:4.0.30319.17929
//
//     对此文件的更改可能会导致不正确的行为，并且如果
//     重新生成代码，这些更改将会丢失。
// </auto-generated>
//------------------------------------------------------------------------------

// 
// This code was auto-generated by Microsoft.Silverlight.ServiceReference, version 5.0.61118.0
// 
namespace Gsafety.PTMS.ServiceReference.InstallStaffService {
    using System.Runtime.Serialization;
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="MaintenanceStaffType", Namespace="http://schemas.datacontract.org/2004/07/Gsafety.PTMS.BaseInformation.Contract.Dat" +
        "a")]
    public enum MaintenanceStaffType : int {
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        InstallationPersonnel = 1,
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        MaintenancePersonnel = 2,
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        AlmightyPersonnel = 3,
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="MultiMessageOfInstallStaffBasicInfoh_PhsrrDr", Namespace="http://schemas.datacontract.org/2004/07/Gsafety.PTMS.Base.Contract.Data")]
    public partial class MultiMessageOfInstallStaffBasicInfoh_PhsrrDr : object, System.ComponentModel.INotifyPropertyChanged {
        
        private string ErrorDetailMsgField;
        
        private string ErrorMsgField;
        
        private System.Exception ExceptionMessageField;
        
        private bool IsSuccessField;
        
        private System.Collections.ObjectModel.ObservableCollection<Gsafety.PTMS.ServiceReference.InstallStaffService.InstallStaffBasicInfo> ResultField;
        
        private int TotalRecordField;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string ErrorDetailMsg {
            get {
                return this.ErrorDetailMsgField;
            }
            set {
                if ((object.ReferenceEquals(this.ErrorDetailMsgField, value) != true)) {
                    this.ErrorDetailMsgField = value;
                    this.RaisePropertyChanged("ErrorDetailMsg");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string ErrorMsg {
            get {
                return this.ErrorMsgField;
            }
            set {
                if ((object.ReferenceEquals(this.ErrorMsgField, value) != true)) {
                    this.ErrorMsgField = value;
                    this.RaisePropertyChanged("ErrorMsg");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public System.Exception ExceptionMessage {
            get {
                return this.ExceptionMessageField;
            }
            set {
                if ((object.ReferenceEquals(this.ExceptionMessageField, value) != true)) {
                    this.ExceptionMessageField = value;
                    this.RaisePropertyChanged("ExceptionMessage");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public bool IsSuccess {
            get {
                return this.IsSuccessField;
            }
            set {
                if ((this.IsSuccessField.Equals(value) != true)) {
                    this.IsSuccessField = value;
                    this.RaisePropertyChanged("IsSuccess");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public System.Collections.ObjectModel.ObservableCollection<Gsafety.PTMS.ServiceReference.InstallStaffService.InstallStaffBasicInfo> Result {
            get {
                return this.ResultField;
            }
            set {
                if ((object.ReferenceEquals(this.ResultField, value) != true)) {
                    this.ResultField = value;
                    this.RaisePropertyChanged("Result");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public int TotalRecord {
            get {
                return this.TotalRecordField;
            }
            set {
                if ((this.TotalRecordField.Equals(value) != true)) {
                    this.TotalRecordField = value;
                    this.RaisePropertyChanged("TotalRecord");
                }
            }
        }
        
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        
        protected void RaisePropertyChanged(string propertyName) {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null)) {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="InstallStaffBasicInfo", Namespace="http://schemas.datacontract.org/2004/07/Gsafety.PTMS.BaseInformation.Contract.Dat" +
        "a")]
    public partial class InstallStaffBasicInfo : object, System.ComponentModel.INotifyPropertyChanged {
        
        private string IDField;
        
        private string NameField;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string ID {
            get {
                return this.IDField;
            }
            set {
                if ((object.ReferenceEquals(this.IDField, value) != true)) {
                    this.IDField = value;
                    this.RaisePropertyChanged("ID");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Name {
            get {
                return this.NameField;
            }
            set {
                if ((object.ReferenceEquals(this.NameField, value) != true)) {
                    this.NameField = value;
                    this.RaisePropertyChanged("Name");
                }
            }
        }
        
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        
        protected void RaisePropertyChanged(string propertyName) {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null)) {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="InstallStaffService.IInstallStaffService")]
    public interface IInstallStaffService {
        
        [System.ServiceModel.OperationContractAttribute(AsyncPattern=true, Action="http://tempuri.org/IInstallStaffService/GetInstallStaffByType", ReplyAction="http://tempuri.org/IInstallStaffService/GetInstallStaffByTypeResponse")]
        System.IAsyncResult BeginGetInstallStaffByType(Gsafety.PTMS.ServiceReference.InstallStaffService.MaintenanceStaffType type, System.AsyncCallback callback, object asyncState);
        
        Gsafety.PTMS.ServiceReference.InstallStaffService.MultiMessageOfInstallStaffBasicInfoh_PhsrrDr EndGetInstallStaffByType(System.IAsyncResult result);
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface IInstallStaffServiceChannel : Gsafety.PTMS.ServiceReference.InstallStaffService.IInstallStaffService, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class GetInstallStaffByTypeCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        public GetInstallStaffByTypeCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        public Gsafety.PTMS.ServiceReference.InstallStaffService.MultiMessageOfInstallStaffBasicInfoh_PhsrrDr Result {
            get {
                base.RaiseExceptionIfNecessary();
                return ((Gsafety.PTMS.ServiceReference.InstallStaffService.MultiMessageOfInstallStaffBasicInfoh_PhsrrDr)(this.results[0]));
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class InstallStaffServiceClient : System.ServiceModel.ClientBase<Gsafety.PTMS.ServiceReference.InstallStaffService.IInstallStaffService>, Gsafety.PTMS.ServiceReference.InstallStaffService.IInstallStaffService {
        
        private BeginOperationDelegate onBeginGetInstallStaffByTypeDelegate;
        
        private EndOperationDelegate onEndGetInstallStaffByTypeDelegate;
        
        private System.Threading.SendOrPostCallback onGetInstallStaffByTypeCompletedDelegate;
        
        private BeginOperationDelegate onBeginOpenDelegate;
        
        private EndOperationDelegate onEndOpenDelegate;
        
        private System.Threading.SendOrPostCallback onOpenCompletedDelegate;
        
        private BeginOperationDelegate onBeginCloseDelegate;
        
        private EndOperationDelegate onEndCloseDelegate;
        
        private System.Threading.SendOrPostCallback onCloseCompletedDelegate;
        
        public InstallStaffServiceClient() {
        }
        
        public InstallStaffServiceClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public InstallStaffServiceClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public InstallStaffServiceClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public InstallStaffServiceClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        public System.Net.CookieContainer CookieContainer {
            get {
                System.ServiceModel.Channels.IHttpCookieContainerManager httpCookieContainerManager = this.InnerChannel.GetProperty<System.ServiceModel.Channels.IHttpCookieContainerManager>();
                if ((httpCookieContainerManager != null)) {
                    return httpCookieContainerManager.CookieContainer;
                }
                else {
                    return null;
                }
            }
            set {
                System.ServiceModel.Channels.IHttpCookieContainerManager httpCookieContainerManager = this.InnerChannel.GetProperty<System.ServiceModel.Channels.IHttpCookieContainerManager>();
                if ((httpCookieContainerManager != null)) {
                    httpCookieContainerManager.CookieContainer = value;
                }
                else {
                    throw new System.InvalidOperationException("无法设置 CookieContainer。请确保绑定包含 HttpCookieContainerBindingElement。");
                }
            }
        }
        
        public event System.EventHandler<GetInstallStaffByTypeCompletedEventArgs> GetInstallStaffByTypeCompleted;
        
        public event System.EventHandler<System.ComponentModel.AsyncCompletedEventArgs> OpenCompleted;
        
        public event System.EventHandler<System.ComponentModel.AsyncCompletedEventArgs> CloseCompleted;
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        System.IAsyncResult Gsafety.PTMS.ServiceReference.InstallStaffService.IInstallStaffService.BeginGetInstallStaffByType(Gsafety.PTMS.ServiceReference.InstallStaffService.MaintenanceStaffType type, System.AsyncCallback callback, object asyncState) {
            return base.Channel.BeginGetInstallStaffByType(type, callback, asyncState);
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        Gsafety.PTMS.ServiceReference.InstallStaffService.MultiMessageOfInstallStaffBasicInfoh_PhsrrDr Gsafety.PTMS.ServiceReference.InstallStaffService.IInstallStaffService.EndGetInstallStaffByType(System.IAsyncResult result) {
            return base.Channel.EndGetInstallStaffByType(result);
        }
        
        private System.IAsyncResult OnBeginGetInstallStaffByType(object[] inValues, System.AsyncCallback callback, object asyncState) {
            Gsafety.PTMS.ServiceReference.InstallStaffService.MaintenanceStaffType type = ((Gsafety.PTMS.ServiceReference.InstallStaffService.MaintenanceStaffType)(inValues[0]));
            return ((Gsafety.PTMS.ServiceReference.InstallStaffService.IInstallStaffService)(this)).BeginGetInstallStaffByType(type, callback, asyncState);
        }
        
        private object[] OnEndGetInstallStaffByType(System.IAsyncResult result) {
            Gsafety.PTMS.ServiceReference.InstallStaffService.MultiMessageOfInstallStaffBasicInfoh_PhsrrDr retVal = ((Gsafety.PTMS.ServiceReference.InstallStaffService.IInstallStaffService)(this)).EndGetInstallStaffByType(result);
            return new object[] {
                    retVal};
        }
        
        private void OnGetInstallStaffByTypeCompleted(object state) {
            if ((this.GetInstallStaffByTypeCompleted != null)) {
                InvokeAsyncCompletedEventArgs e = ((InvokeAsyncCompletedEventArgs)(state));
                this.GetInstallStaffByTypeCompleted(this, new GetInstallStaffByTypeCompletedEventArgs(e.Results, e.Error, e.Cancelled, e.UserState));
            }
        }
        
        public void GetInstallStaffByTypeAsync(Gsafety.PTMS.ServiceReference.InstallStaffService.MaintenanceStaffType type) {
            this.GetInstallStaffByTypeAsync(type, null);
        }
        
        public void GetInstallStaffByTypeAsync(Gsafety.PTMS.ServiceReference.InstallStaffService.MaintenanceStaffType type, object userState) {
            if ((this.onBeginGetInstallStaffByTypeDelegate == null)) {
                this.onBeginGetInstallStaffByTypeDelegate = new BeginOperationDelegate(this.OnBeginGetInstallStaffByType);
            }
            if ((this.onEndGetInstallStaffByTypeDelegate == null)) {
                this.onEndGetInstallStaffByTypeDelegate = new EndOperationDelegate(this.OnEndGetInstallStaffByType);
            }
            if ((this.onGetInstallStaffByTypeCompletedDelegate == null)) {
                this.onGetInstallStaffByTypeCompletedDelegate = new System.Threading.SendOrPostCallback(this.OnGetInstallStaffByTypeCompleted);
            }
            base.InvokeAsync(this.onBeginGetInstallStaffByTypeDelegate, new object[] {
                        type}, this.onEndGetInstallStaffByTypeDelegate, this.onGetInstallStaffByTypeCompletedDelegate, userState);
        }
        
        private System.IAsyncResult OnBeginOpen(object[] inValues, System.AsyncCallback callback, object asyncState) {
            return ((System.ServiceModel.ICommunicationObject)(this)).BeginOpen(callback, asyncState);
        }
        
        private object[] OnEndOpen(System.IAsyncResult result) {
            ((System.ServiceModel.ICommunicationObject)(this)).EndOpen(result);
            return null;
        }
        
        private void OnOpenCompleted(object state) {
            if ((this.OpenCompleted != null)) {
                InvokeAsyncCompletedEventArgs e = ((InvokeAsyncCompletedEventArgs)(state));
                this.OpenCompleted(this, new System.ComponentModel.AsyncCompletedEventArgs(e.Error, e.Cancelled, e.UserState));
            }
        }
        
        public void OpenAsync() {
            this.OpenAsync(null);
        }
        
        public void OpenAsync(object userState) {
            if ((this.onBeginOpenDelegate == null)) {
                this.onBeginOpenDelegate = new BeginOperationDelegate(this.OnBeginOpen);
            }
            if ((this.onEndOpenDelegate == null)) {
                this.onEndOpenDelegate = new EndOperationDelegate(this.OnEndOpen);
            }
            if ((this.onOpenCompletedDelegate == null)) {
                this.onOpenCompletedDelegate = new System.Threading.SendOrPostCallback(this.OnOpenCompleted);
            }
            base.InvokeAsync(this.onBeginOpenDelegate, null, this.onEndOpenDelegate, this.onOpenCompletedDelegate, userState);
        }
        
        private System.IAsyncResult OnBeginClose(object[] inValues, System.AsyncCallback callback, object asyncState) {
            return ((System.ServiceModel.ICommunicationObject)(this)).BeginClose(callback, asyncState);
        }
        
        private object[] OnEndClose(System.IAsyncResult result) {
            ((System.ServiceModel.ICommunicationObject)(this)).EndClose(result);
            return null;
        }
        
        private void OnCloseCompleted(object state) {
            if ((this.CloseCompleted != null)) {
                InvokeAsyncCompletedEventArgs e = ((InvokeAsyncCompletedEventArgs)(state));
                this.CloseCompleted(this, new System.ComponentModel.AsyncCompletedEventArgs(e.Error, e.Cancelled, e.UserState));
            }
        }
        
        public void CloseAsync() {
            this.CloseAsync(null);
        }
        
        public void CloseAsync(object userState) {
            if ((this.onBeginCloseDelegate == null)) {
                this.onBeginCloseDelegate = new BeginOperationDelegate(this.OnBeginClose);
            }
            if ((this.onEndCloseDelegate == null)) {
                this.onEndCloseDelegate = new EndOperationDelegate(this.OnEndClose);
            }
            if ((this.onCloseCompletedDelegate == null)) {
                this.onCloseCompletedDelegate = new System.Threading.SendOrPostCallback(this.OnCloseCompleted);
            }
            base.InvokeAsync(this.onBeginCloseDelegate, null, this.onEndCloseDelegate, this.onCloseCompletedDelegate, userState);
        }
        
        protected override Gsafety.PTMS.ServiceReference.InstallStaffService.IInstallStaffService CreateChannel() {
            return new InstallStaffServiceClientChannel(this);
        }
        
        private class InstallStaffServiceClientChannel : ChannelBase<Gsafety.PTMS.ServiceReference.InstallStaffService.IInstallStaffService>, Gsafety.PTMS.ServiceReference.InstallStaffService.IInstallStaffService {
            
            public InstallStaffServiceClientChannel(System.ServiceModel.ClientBase<Gsafety.PTMS.ServiceReference.InstallStaffService.IInstallStaffService> client) : 
                    base(client) {
            }
            
            public System.IAsyncResult BeginGetInstallStaffByType(Gsafety.PTMS.ServiceReference.InstallStaffService.MaintenanceStaffType type, System.AsyncCallback callback, object asyncState) {
                object[] _args = new object[1];
                _args[0] = type;
                System.IAsyncResult _result = base.BeginInvoke("GetInstallStaffByType", _args, callback, asyncState);
                return _result;
            }
            
            public Gsafety.PTMS.ServiceReference.InstallStaffService.MultiMessageOfInstallStaffBasicInfoh_PhsrrDr EndGetInstallStaffByType(System.IAsyncResult result) {
                object[] _args = new object[0];
                Gsafety.PTMS.ServiceReference.InstallStaffService.MultiMessageOfInstallStaffBasicInfoh_PhsrrDr _result = ((Gsafety.PTMS.ServiceReference.InstallStaffService.MultiMessageOfInstallStaffBasicInfoh_PhsrrDr)(base.EndInvoke("GetInstallStaffByType", _args, result)));
                return _result;
            }
        }
    }
}
