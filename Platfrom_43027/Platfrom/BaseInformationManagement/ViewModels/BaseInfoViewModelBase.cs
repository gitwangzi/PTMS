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
using Jounce.Core.Command;
using Jounce.Framework.Command;
using Jounce.Core.ViewModel;
using Jounce.Core.View;
using System.Collections.Generic;
using Jounce.Framework.ViewModel;
using Gsafety.PTMS.Bases.Models;

namespace Gsafety.PTMS.BaseInformation.ViewModels
{
    public class BaseInfoViewModelBase : BaseEntityViewModel
    {
        public List<int> PageSizeList { get; set; }
        public EnumModel CurrentInstallStatus { get; set; }
        public List<EnumModel> InstallStatusList { get; set; }
        public Uri Url { get; protected set; }
        public string Title { get; protected set; }

        public bool IsReadOnly { get; protected set; }
        public bool KeyIsReadOnly { get; protected set; }
        public Visibility IsView { get; protected set; }
        public Visibility IsNotView { get; protected set; }
        public bool UploadBtnStatus { get; protected set; }

        protected int currentIndex = 1;
        protected bool isInitialOrQuery;
        protected string action;
        protected bool ExportBtnStatus { get; set; }

        public IActionCommand AddCommand { get; protected set; }
        public IActionCommand DeleteCommand { get; protected set; }
        public IActionCommand UpdateCommand { get; protected set; }
        public IActionCommand ViewCommand { get; protected set; }
        public IActionCommand QueryCommand { get; protected set; }
        public IActionCommand UploadCommand { get; protected set; }
        public IActionCommand ExportCommand { get; protected set; }
        public IActionCommand ResetCommand { get; protected set; }
        public IActionCommand ReturnCommand { get; protected set; }

        public BaseInfoViewModelBase()
        {
            AddCommand = new ActionCommand<object>(obj => Publish("add"));
            DeleteCommand = new ActionCommand<object>(obj => Delete());
            UpdateCommand = new ActionCommand<object>(obj => Publish("update"));
            ViewCommand = new ActionCommand<object>(obj => Publish("view"));
            QueryCommand = new ActionCommand<object>(obj => Query());
            UploadCommand = new ActionCommand<object>(obj => UploadExcel());
            ExportCommand = new ActionCommand<object>(obj => Export());
            ResetCommand = new ActionCommand<object>(obj => Reset());
            ReturnCommand = new ActionCommand<object>(obj => Return());
        }

        protected int batchCount;
        protected int batchIndex;
        protected int errorIndex;
        protected string errorCode;
        protected string[][] uploadContent;


        protected void setUploadBtnStatus(bool Flag)
        {
            UploadBtnStatus = Flag;
            Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => UploadBtnStatus));
            if (Flag)
            {
                uploadContent = null;
                GoNextPage();
            }
        }

        protected virtual void GoNextPage()
        {

        }

        protected void setExportBtnStatus(bool Flag)
        {
            ExportBtnStatus = Flag;
            Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => ExportBtnStatus));
        }

        protected virtual void Publish(string name)
        {

        }

        protected override void ActivateView(string viewName, System.Collections.Generic.IDictionary<string, object> viewParameters)
        {

        }

        protected virtual void InitPagedServerCollection()
        {

        }


        protected virtual void Add() { }
        protected virtual void Delete() { }
        protected virtual void Update() { }
        protected virtual void View() { }
        protected virtual void Query() { }
        protected virtual void UploadExcel() { }
        protected virtual void Export() { }
        protected virtual void OnComitted() { }
        protected virtual void Reset() { }
        protected virtual void Return() { }
    }
}
