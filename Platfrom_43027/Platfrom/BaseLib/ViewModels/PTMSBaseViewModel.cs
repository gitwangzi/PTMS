using Gsafety.PTMS.Share;
using Jounce.Core.Application;
using Jounce.Framework.ViewModel;
using System;
using System.Collections.ObjectModel;

namespace BaseLib.ViewModels
{
    public class PTMSBaseViewModel : BaseEntityViewModel
    {
        //public static string notnull = "不允许为空";
        //public const string ServerError = "服务器连接异常";
        //public static string wrongformat = "格式错误或超出范围";
        public static string notnull = "NotNull";
        public const string ServerError = "ServerError";
        public static string wrongformat = "wrongformat";
        protected string _rolefucs;

        protected bool _initialrole = false;

        protected void Error(string methodname, Exception ex)
        {
            ApplicationContext.Instance.Logger.LogException(methodname, ex);
        }

        protected void Log(string source, string info)
        {
            ApplicationContext.Instance.Logger.Log(LogSeverity.Information, source, info);
        }

        public virtual void OnRoleEvent(ObservableCollection<string> FuncItems)
        {
            var items = OnFilterRoleFunc(FuncItems);
            OnInitialUI(items);

            _initialrole = true;
        }

        protected override void InitializeVm()
        {
            base.InitializeVm();

            if(!_initialrole)
            {
                OnRoleEvent(ApplicationContext.Instance.AuthenticationInfo.Role.FuncItems);
            }
        }

        protected virtual ObservableCollection<string> OnFilterRoleFunc(ObservableCollection<string> FuncItems)
        {
            if(FuncItems != null)
            {
                ObservableCollection<string> items = new ObservableCollection<string>();
                if(!string.IsNullOrEmpty(_rolefucs))
                {
                    foreach(var item in FuncItems)
                    {
                        if(item.Contains(_rolefucs))
                        {
                            items.Add(item);
                        }
                    }
                }

                return items;
            }

            return null;
        }

        protected virtual void OnInitialUI(ObservableCollection<string> FuncItems)
        {

        }

        public virtual void OnHelp()
        {

        }
    }
}
