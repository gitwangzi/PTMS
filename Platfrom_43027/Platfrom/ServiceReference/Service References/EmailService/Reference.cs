﻿//------------------------------------------------------------------------------
// <auto-generated>
//     此代码由工具生成。
//     运行时版本:4.0.30319.42000
//
//     对此文件的更改可能会导致不正确的行为，并且如果
//     重新生成代码，这些更改将会丢失。
// </auto-generated>
//------------------------------------------------------------------------------

// 
// This code was auto-generated by Microsoft.Silverlight.ServiceReference, version 5.0.61118.0
// 
namespace Gsafety.PTMS.ServiceReference.EmailService {
    using System.Runtime.Serialization;
    
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="EmailInfo", Namespace="http://schemas.datacontract.org/2004/07/Gsafety.PTMS.Email.Contract.Data")]
    public partial class EmailInfo : object, System.ComponentModel.INotifyPropertyChanged {
        
        private System.Collections.ObjectModel.ObservableCollection<string> AttachmentsPathField;
        
        private bool IsbodyHtmlField;
        
        private string MailBodyField;
        
        private System.Collections.ObjectModel.ObservableCollection<string> MailCcArrayField;
        
        private string MailSubjectField;
        
        private System.Collections.ObjectModel.ObservableCollection<string> MailToArrayField;
        
        private byte[] bytepictureField;
        
        private byte[] picturestreamField;
        
        private string streamField;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public System.Collections.ObjectModel.ObservableCollection<string> AttachmentsPath {
            get {
                return this.AttachmentsPathField;
            }
            set {
                if ((object.ReferenceEquals(this.AttachmentsPathField, value) != true)) {
                    this.AttachmentsPathField = value;
                    this.RaisePropertyChanged("AttachmentsPath");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public bool IsbodyHtml {
            get {
                return this.IsbodyHtmlField;
            }
            set {
                if ((this.IsbodyHtmlField.Equals(value) != true)) {
                    this.IsbodyHtmlField = value;
                    this.RaisePropertyChanged("IsbodyHtml");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string MailBody {
            get {
                return this.MailBodyField;
            }
            set {
                if ((object.ReferenceEquals(this.MailBodyField, value) != true)) {
                    this.MailBodyField = value;
                    this.RaisePropertyChanged("MailBody");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public System.Collections.ObjectModel.ObservableCollection<string> MailCcArray {
            get {
                return this.MailCcArrayField;
            }
            set {
                if ((object.ReferenceEquals(this.MailCcArrayField, value) != true)) {
                    this.MailCcArrayField = value;
                    this.RaisePropertyChanged("MailCcArray");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string MailSubject {
            get {
                return this.MailSubjectField;
            }
            set {
                if ((object.ReferenceEquals(this.MailSubjectField, value) != true)) {
                    this.MailSubjectField = value;
                    this.RaisePropertyChanged("MailSubject");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public System.Collections.ObjectModel.ObservableCollection<string> MailToArray {
            get {
                return this.MailToArrayField;
            }
            set {
                if ((object.ReferenceEquals(this.MailToArrayField, value) != true)) {
                    this.MailToArrayField = value;
                    this.RaisePropertyChanged("MailToArray");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public byte[] bytepicture {
            get {
                return this.bytepictureField;
            }
            set {
                if ((object.ReferenceEquals(this.bytepictureField, value) != true)) {
                    this.bytepictureField = value;
                    this.RaisePropertyChanged("bytepicture");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public byte[] picturestream {
            get {
                return this.picturestreamField;
            }
            set {
                if ((object.ReferenceEquals(this.picturestreamField, value) != true)) {
                    this.picturestreamField = value;
                    this.RaisePropertyChanged("picturestream");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string stream {
            get {
                return this.streamField;
            }
            set {
                if ((object.ReferenceEquals(this.streamField, value) != true)) {
                    this.streamField = value;
                    this.RaisePropertyChanged("stream");
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
    [System.Runtime.Serialization.DataContractAttribute(Name="MarshalByRefObject", Namespace="http://schemas.datacontract.org/2004/07/System")]
    [System.Runtime.Serialization.KnownTypeAttribute(typeof(byte[]))]
    [System.Runtime.Serialization.KnownTypeAttribute(typeof(System.Collections.ObjectModel.ObservableCollection<string>))]
    [System.Runtime.Serialization.KnownTypeAttribute(typeof(System.Exception))]
    [System.Runtime.Serialization.KnownTypeAttribute(typeof(Gsafety.PTMS.ServiceReference.EmailService.EmailInfo))]
    [System.Runtime.Serialization.KnownTypeAttribute(typeof(Gsafety.PTMS.ServiceReference.EmailService.SingleMessageOfboolean))]
    public partial class MarshalByRefObject : object, System.ComponentModel.INotifyPropertyChanged {
        
        private object @__identityField;
        
        [System.Runtime.Serialization.DataMemberAttribute(IsRequired=true)]
        public object @__identity {
            get {
                return this.@__identityField;
            }
            set {
                if ((object.ReferenceEquals(this.@__identityField, value) != true)) {
                    this.@__identityField = value;
                    this.RaisePropertyChanged("__identity");
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
    [System.Runtime.Serialization.DataContractAttribute(Name="SingleMessageOfboolean", Namespace="http://schemas.datacontract.org/2004/07/Gsafety.PTMS.Base.Contract.Data")]
    public partial class SingleMessageOfboolean : object, System.ComponentModel.INotifyPropertyChanged {
        
        private string ErrorDetailMsgField;
        
        private string ErrorMsgField;
        
        private System.Exception ExceptionMessageField;
        
        private bool IsSuccessField;
        
        private bool ResultField;
        
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
        public bool Result {
            get {
                return this.ResultField;
            }
            set {
                if ((this.ResultField.Equals(value) != true)) {
                    this.ResultField = value;
                    this.RaisePropertyChanged("Result");
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
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="EmailService.IEmailService")]
    public interface IEmailService {
        
        [System.ServiceModel.OperationContractAttribute(AsyncPattern=true, Action="http://tempuri.org/IEmailService/SendEmail", ReplyAction="http://tempuri.org/IEmailService/SendEmailResponse")]
        System.IAsyncResult BeginSendEmail(Gsafety.PTMS.ServiceReference.EmailService.EmailInfo email, System.AsyncCallback callback, object asyncState);
        
        Gsafety.PTMS.ServiceReference.EmailService.SingleMessageOfboolean EndSendEmail(System.IAsyncResult result);
        
        [System.ServiceModel.OperationContractAttribute(AsyncPattern=true, Action="http://tempuri.org/IEmailService/SendTest", ReplyAction="http://tempuri.org/IEmailService/SendTestResponse")]
        System.IAsyncResult BeginSendTest(bool x, System.AsyncCallback callback, object asyncState);
        
        bool EndSendTest(System.IAsyncResult result);
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface IEmailServiceChannel : Gsafety.PTMS.ServiceReference.EmailService.IEmailService, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class SendEmailCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        public SendEmailCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        public Gsafety.PTMS.ServiceReference.EmailService.SingleMessageOfboolean Result {
            get {
                base.RaiseExceptionIfNecessary();
                return ((Gsafety.PTMS.ServiceReference.EmailService.SingleMessageOfboolean)(this.results[0]));
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class SendTestCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        public SendTestCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        public bool Result {
            get {
                base.RaiseExceptionIfNecessary();
                return ((bool)(this.results[0]));
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class EmailServiceClient : System.ServiceModel.ClientBase<Gsafety.PTMS.ServiceReference.EmailService.IEmailService>, Gsafety.PTMS.ServiceReference.EmailService.IEmailService {
        
        private BeginOperationDelegate onBeginSendEmailDelegate;
        
        private EndOperationDelegate onEndSendEmailDelegate;
        
        private System.Threading.SendOrPostCallback onSendEmailCompletedDelegate;
        
        private BeginOperationDelegate onBeginSendTestDelegate;
        
        private EndOperationDelegate onEndSendTestDelegate;
        
        private System.Threading.SendOrPostCallback onSendTestCompletedDelegate;
        
        private BeginOperationDelegate onBeginOpenDelegate;
        
        private EndOperationDelegate onEndOpenDelegate;
        
        private System.Threading.SendOrPostCallback onOpenCompletedDelegate;
        
        private BeginOperationDelegate onBeginCloseDelegate;
        
        private EndOperationDelegate onEndCloseDelegate;
        
        private System.Threading.SendOrPostCallback onCloseCompletedDelegate;
        
        public EmailServiceClient() {
        }
        
        public EmailServiceClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public EmailServiceClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public EmailServiceClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public EmailServiceClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
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
        
        public event System.EventHandler<SendEmailCompletedEventArgs> SendEmailCompleted;
        
        public event System.EventHandler<SendTestCompletedEventArgs> SendTestCompleted;
        
        public event System.EventHandler<System.ComponentModel.AsyncCompletedEventArgs> OpenCompleted;
        
        public event System.EventHandler<System.ComponentModel.AsyncCompletedEventArgs> CloseCompleted;
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        System.IAsyncResult Gsafety.PTMS.ServiceReference.EmailService.IEmailService.BeginSendEmail(Gsafety.PTMS.ServiceReference.EmailService.EmailInfo email, System.AsyncCallback callback, object asyncState) {
            return base.Channel.BeginSendEmail(email, callback, asyncState);
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        Gsafety.PTMS.ServiceReference.EmailService.SingleMessageOfboolean Gsafety.PTMS.ServiceReference.EmailService.IEmailService.EndSendEmail(System.IAsyncResult result) {
            return base.Channel.EndSendEmail(result);
        }
        
        private System.IAsyncResult OnBeginSendEmail(object[] inValues, System.AsyncCallback callback, object asyncState) {
            Gsafety.PTMS.ServiceReference.EmailService.EmailInfo email = ((Gsafety.PTMS.ServiceReference.EmailService.EmailInfo)(inValues[0]));
            return ((Gsafety.PTMS.ServiceReference.EmailService.IEmailService)(this)).BeginSendEmail(email, callback, asyncState);
        }
        
        private object[] OnEndSendEmail(System.IAsyncResult result) {
            Gsafety.PTMS.ServiceReference.EmailService.SingleMessageOfboolean retVal = ((Gsafety.PTMS.ServiceReference.EmailService.IEmailService)(this)).EndSendEmail(result);
            return new object[] {
                    retVal};
        }
        
        private void OnSendEmailCompleted(object state) {
            if ((this.SendEmailCompleted != null)) {
                InvokeAsyncCompletedEventArgs e = ((InvokeAsyncCompletedEventArgs)(state));
                this.SendEmailCompleted(this, new SendEmailCompletedEventArgs(e.Results, e.Error, e.Cancelled, e.UserState));
            }
        }
        
        public void SendEmailAsync(Gsafety.PTMS.ServiceReference.EmailService.EmailInfo email) {
            this.SendEmailAsync(email, null);
        }
        
        public void SendEmailAsync(Gsafety.PTMS.ServiceReference.EmailService.EmailInfo email, object userState) {
            if ((this.onBeginSendEmailDelegate == null)) {
                this.onBeginSendEmailDelegate = new BeginOperationDelegate(this.OnBeginSendEmail);
            }
            if ((this.onEndSendEmailDelegate == null)) {
                this.onEndSendEmailDelegate = new EndOperationDelegate(this.OnEndSendEmail);
            }
            if ((this.onSendEmailCompletedDelegate == null)) {
                this.onSendEmailCompletedDelegate = new System.Threading.SendOrPostCallback(this.OnSendEmailCompleted);
            }
            base.InvokeAsync(this.onBeginSendEmailDelegate, new object[] {
                        email}, this.onEndSendEmailDelegate, this.onSendEmailCompletedDelegate, userState);
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        System.IAsyncResult Gsafety.PTMS.ServiceReference.EmailService.IEmailService.BeginSendTest(bool x, System.AsyncCallback callback, object asyncState) {
            return base.Channel.BeginSendTest(x, callback, asyncState);
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        bool Gsafety.PTMS.ServiceReference.EmailService.IEmailService.EndSendTest(System.IAsyncResult result) {
            return base.Channel.EndSendTest(result);
        }
        
        private System.IAsyncResult OnBeginSendTest(object[] inValues, System.AsyncCallback callback, object asyncState) {
            bool x = ((bool)(inValues[0]));
            return ((Gsafety.PTMS.ServiceReference.EmailService.IEmailService)(this)).BeginSendTest(x, callback, asyncState);
        }
        
        private object[] OnEndSendTest(System.IAsyncResult result) {
            bool retVal = ((Gsafety.PTMS.ServiceReference.EmailService.IEmailService)(this)).EndSendTest(result);
            return new object[] {
                    retVal};
        }
        
        private void OnSendTestCompleted(object state) {
            if ((this.SendTestCompleted != null)) {
                InvokeAsyncCompletedEventArgs e = ((InvokeAsyncCompletedEventArgs)(state));
                this.SendTestCompleted(this, new SendTestCompletedEventArgs(e.Results, e.Error, e.Cancelled, e.UserState));
            }
        }
        
        public void SendTestAsync(bool x) {
            this.SendTestAsync(x, null);
        }
        
        public void SendTestAsync(bool x, object userState) {
            if ((this.onBeginSendTestDelegate == null)) {
                this.onBeginSendTestDelegate = new BeginOperationDelegate(this.OnBeginSendTest);
            }
            if ((this.onEndSendTestDelegate == null)) {
                this.onEndSendTestDelegate = new EndOperationDelegate(this.OnEndSendTest);
            }
            if ((this.onSendTestCompletedDelegate == null)) {
                this.onSendTestCompletedDelegate = new System.Threading.SendOrPostCallback(this.OnSendTestCompleted);
            }
            base.InvokeAsync(this.onBeginSendTestDelegate, new object[] {
                        x}, this.onEndSendTestDelegate, this.onSendTestCompletedDelegate, userState);
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
        
        protected override Gsafety.PTMS.ServiceReference.EmailService.IEmailService CreateChannel() {
            return new EmailServiceClientChannel(this);
        }
        
        private class EmailServiceClientChannel : ChannelBase<Gsafety.PTMS.ServiceReference.EmailService.IEmailService>, Gsafety.PTMS.ServiceReference.EmailService.IEmailService {
            
            public EmailServiceClientChannel(System.ServiceModel.ClientBase<Gsafety.PTMS.ServiceReference.EmailService.IEmailService> client) : 
                    base(client) {
            }
            
            public System.IAsyncResult BeginSendEmail(Gsafety.PTMS.ServiceReference.EmailService.EmailInfo email, System.AsyncCallback callback, object asyncState) {
                object[] _args = new object[1];
                _args[0] = email;
                System.IAsyncResult _result = base.BeginInvoke("SendEmail", _args, callback, asyncState);
                return _result;
            }
            
            public Gsafety.PTMS.ServiceReference.EmailService.SingleMessageOfboolean EndSendEmail(System.IAsyncResult result) {
                object[] _args = new object[0];
                Gsafety.PTMS.ServiceReference.EmailService.SingleMessageOfboolean _result = ((Gsafety.PTMS.ServiceReference.EmailService.SingleMessageOfboolean)(base.EndInvoke("SendEmail", _args, result)));
                return _result;
            }
            
            public System.IAsyncResult BeginSendTest(bool x, System.AsyncCallback callback, object asyncState) {
                object[] _args = new object[1];
                _args[0] = x;
                System.IAsyncResult _result = base.BeginInvoke("SendTest", _args, callback, asyncState);
                return _result;
            }
            
            public bool EndSendTest(System.IAsyncResult result) {
                object[] _args = new object[0];
                bool _result = ((bool)(base.EndInvoke("SendTest", _args, result)));
                return _result;
            }
        }
    }
}