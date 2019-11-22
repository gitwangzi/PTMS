/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: ebe8e57b-7f1d-449c-b241-a8ae8d3d0a45      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: PC-WANGJINCAI
/////                 Author: TEST(JinCaiWang)
/////======================================================================
/////           Project Name: Gsafety.PTMS.Alarm.Contract.Data
/////    Project Description:    
/////             Class Name: AlarmInfo
/////          Class Version: v1.0.0.0
/////            Create Time: 2013/8/8 11:07:41
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2013/8/8 11:07:41
/////            Modified by:
/////   Modified Description: 
/////======================================================================
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Windows.Navigation;
using Gsafety.PTMS.Manager.Model;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Data;
using Gsafety.PTMS.ServiceReference.AppConfigService;
using Jounce.Core.View;
using Gsafety.PTMS.Manager;
using Jounce.Regions.Core;
using Gsafety.PTMS.Share;

namespace Gsafety.PTMS.AppConfig.Models
{
    //[
    //ExportAsView(
    //    ManagerName.SettingAppConfigDefine
    //    , Category = ManagerName.CategoryName
    //    , MenuName = ManagerName.SettingManageMenuName
    //    , MenuTitle = "MANAGER_SettingAppConfigDefine"
    //    , ToolTip = "Click to view some text."
    //    , Url = "/SettingAppConfigDefine"
    //    , Order = 2
    //    ),
    //ExportViewToRegion(
    //       ManagerName.SettingAppConfigDefine
    //       , ManagerName.ManagerContainer
    //       )

    //  ]


    public partial class SettingAppConfigDefine : Page, INotifyPropertyChanged
    {
        private AppConfigManagerClient _client;
        private List<ItemState> _itemState;
        private SectionTypeModel _typeMode = new SectionTypeModel();
        private SectionLevelModel _levelMode = new SectionLevelModel();

        private bool _hasError;
        private bool _isDataChange;

        private bool _isInitPage;

        public bool IsDataChange
        {
            get { return _isDataChange; }
            set
            {
                _isDataChange = value;
                OnPropChanged("IsDataChange");
            }
        }

        public SettingAppConfigDefine()
        {
            InitializeComponent();
            Loaded += SettingAppConfigDefine_Loaded;

        }

        void SettingAppConfigDefine_Loaded(object sender, RoutedEventArgs e)
        {
            InitializePage();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            InitializePage();
        }


        private void InitializePage()
        {
            if (!_isInitPage)
            {
                _client = ServiceClientFactory.Create<AppConfigManagerClient>();
                _itemState = new List<ItemState>();
                LoadDbData();
                LoadLocalData();
                SetCustomerBind();
            }
            _isInitPage = true;

        }

        private void tv_Config_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            if (this.tv_Config.SelectedItem == null) return;

            PointDropdown();
            var current = ((TreeViewItem)e.NewValue).Tag as ConfigTree;
            this.btn_addItem.IsEnabled = string.IsNullOrWhiteSpace(current.Value.ParentId);
            this.dp_Type.IsEnabled = !this.btn_addItem.IsEnabled;
            // Commit();

        }


        private void OnPropChanged(string propName)
        {
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(propName));
            }
        }

        private void SetCustomerBind()
        {
            var saveBtnBind = new Binding();
            saveBtnBind.Mode = BindingMode.OneWay;
            saveBtnBind.Source = this;
            saveBtnBind.Path = new PropertyPath("IsDataChange");
            btn_save.SetBinding(IsEnabledProperty, saveBtnBind);

        }


        private void PointDropdown()
        {
            var current = ((TreeViewItem)(this.tv_Config.SelectedItem)).Tag as ConfigTree;
            this.dp_Type.SelectedIndex = _typeMode.GetSettingValueIndex(current.Value.SectionType);
            this.dp_level.SelectedIndex = _levelMode.GetSettingValueIndex(current.Value.SectionLevel);
        }


        private void Border_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (((Border)sender).Name == "br_left")
            {
                var wd = e.NewSize.Width - 30;
                if (wd <= 0)
                {
                    wd = 30;
                }

                if (wd < this.tv_Config.MinWidth)
                {
                    wd = this.tv_Config.MinWidth;
                }

                this.tv_Config.Width = wd;
                sp_left_toolbar.Width = wd;
            }
        }


        #region loading ServerData

        private void LoadDbData()
        {
            this.bi_root.IsBusy = true;
            _client.GetAllSectionsCompleted += (s, args) =>
            {
                if (args.Error != null)
                {
                    ShowServerError(args.Error);
                }
                else
                {
                    if (args.Result != null)
                    {
                        LoadTree(args.Result);
                    }
                }
                Dispatcher.BeginInvoke(() => this.bi_root.IsBusy = false);

            };
            _client.GetAllSectionsAsync();

        }



        private void LoadTree(IEnumerable<ConfigTree> model)
        {
            foreach (var item in model)
            {
                var root = CreateItem(item);
                AddChildNode(root, item);
                this.tv_Config.Items.Add(root);
            }

        }

        private void AddChildNode(TreeViewItem current, ConfigTree model)
        {
            if (model.Children == null) return;
            foreach (var x in model.Children)
            {
                var child = CreateItem(x);
                AddChildNode(child, x);
                current.Items.Add(child);
            }

        }
        private TreeViewItem CreateItem(ConfigTree model)
        {
            var result = new TreeViewItem();
            var bind = new System.Windows.Data.Binding();
            bind.Source = model;
            bind.Path = new PropertyPath("Value.SECTION_NAME");
            bind.Mode = System.Windows.Data.BindingMode.OneWay;
            result.SetBinding(TreeViewItem.HeaderProperty, bind);
            result.Tag = model;

            return result;

        }


        #endregion

        #region bindStatic

        private void LoadLocalData()
        {

            Action<ComboBox, IEnumerable<SerializableItem>> addItem = (comb, items) =>
            {
                foreach (var x in items)
                {
                    var item = new ComboBoxItem { Content = x.Desc, Tag = x, };
                    item.IsSelected = x.IsDefault;
                    comb.Items.Add(item);
                }
            };

            addItem(dp_level, _levelMode.Items);
            addItem(dp_Type, _typeMode.Items);
        }

        #endregion

        private void btn_addCata_Click(object sender, RoutedEventArgs e)
        {
            AddTreeNode(this.tv_Config.Items, null);
        }

        private void btn_addItem_Click(object sender, RoutedEventArgs e)
        {
            if (this.tv_Config.SelectedItem == null)
            {
                return;
            }
            var current = ((TreeViewItem)(this.tv_Config.SelectedItem)).Tag as ConfigTree;
            AddTreeNode(((TreeViewItem)(this.tv_Config.SelectedItem)).Items, current.Value.Id);
        }

        private void AddTreeNode(ItemCollection items, string parentId)
        {
            var state = new ItemState
            {
                Item = new ConfigTree(),
                State = ObjState.Add,
            };
            state.Item.Value.ParentId = parentId;

            var node = CreateItem(state.Item);
            node.IsSelected = true;
            items.Add(node);
            AddChangedItem(state);
            if (node.Parent is TreeViewItem)
            {
                ((TreeViewItem)node.Parent).IsExpanded = true;
            }

        }


        private void btn_remove_Click(object sender, RoutedEventArgs e)
        {
            if (this.tv_Config.SelectedItem == null) return;

            var current = ((TreeViewItem)(this.tv_Config.SelectedItem)).Tag as ConfigTree;
            var state = new ItemState
            {
                Item = current,
                State = ObjState.Delete,
            };


            ItemCollection coll = null;
            if (string.IsNullOrWhiteSpace(current.Value.ParentId))
            {
                coll = this.tv_Config.Items;
            }
            else
            {
                coll = ((TreeViewItem)((TreeViewItem)this.tv_Config.SelectedItem).Parent).Items;
            }
            coll.Remove(this.tv_Config.SelectedItem);
            AddChangedItem(state);

        }

        private void btn_save_Click(object sender, RoutedEventArgs e)
        {
            Commit();
        }


        private void Commit()
        {
            if (_hasError)
            {
                this.txt_sectionName.Focus();
                return;
            }
            this.bi_root.IsBusy = _itemState.Count > 0;
            for (int i = _itemState.Count - 1; i >= 0; i--)
            {
                SaveChanges(_itemState[i]);
            }

        }



        private void SaveChanges(ItemState item)
        {

            if (item.State == ObjState.Add)
            {
                _client.AddSectionCompleted += CheckComplete;
                _client.AddSectionAsync(item.Item, item);
            }
            else if (item.State == ObjState.Delete)
            {
                _client.DeleteSectionByIdCompleted += CheckComplete;
                _client.DeleteSectionByIdAsync(item.Item.Value.Id, item);
            }
            else if (item.State == ObjState.Update)
            {
                _client.UpdateSectionCompleted += CheckComplete;
                _client.UpdateSectionAsync(item.Item, item);

            }
        }


        private void CheckComplete(object sender, AsyncCompletedEventArgs arg)
        {
            if (arg.Error != null)
            {
                ShowServerError(arg.Error);
            }

            _itemState.Remove((ItemState)arg.UserState);
            this.bi_root.IsBusy = _itemState.Count > 0;
            IsDataChange = _itemState.Count > 0;
        }

        private void AddChangedItem(ItemState changeItem)
        {
            var item = this._itemState.FirstOrDefault(x => x.Item.Value.Id == changeItem.Item.Value.Id);
            if (item == null)
            {
                this._itemState.Add(changeItem);
            }
            else
            {
                switch (item.State)
                {
                    case ObjState.Add:
                        switch (changeItem.State)
                        {
                            case ObjState.Delete:
                                DeleteChangeItem(item);
                                break;
                        }
                        break;

                    case ObjState.Update:
                        switch (changeItem.State)
                        {
                            case ObjState.None:
                                item.State = ObjState.Update;
                                break;
                            case ObjState.Delete:
                                item.State = ObjState.Delete;
                                break;
                        }
                        break;

                }
            }
            this.IsDataChange = _itemState.Count > 0;

        }

        private void DeleteChangeItem(ItemState changeItem)
        {
            _itemState.Remove(changeItem);
            var children = _itemState.Where(x => x.Item.Value.ParentId == changeItem.Item.Value.Id);
            foreach (var x in children)
            {
                DeleteChangeItem(x);
            }
        }

        enum ObjState
        {
            None,
            Add,
            Update,
            Delete,
        }

        class ItemState
        {
            public ConfigTree Item { get; set; }

            public ObjState State { get; set; }
        }



        private void dp_Type_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (this.tv_Config.SelectedItem == null || this.dp_Type.SelectedItem == null) return;

            var current = ((TreeViewItem)(this.tv_Config.SelectedItem)).Tag as ConfigTree;
            var item = this.dp_Type.SelectedItem as ComboBoxItem;
            var tag = (TypeModelItem)item.Tag;
            var ctrl = (ItemTypeDefine)Activator.CreateInstance(tag.Control);
            ctrl.ShoDesignControl(current.Value.SectionType, x =>
            {
                if (current.Value.SectionType != x)
                {
                    current.Value.SectionType = x;
                    SetUpdateState();
                }
            });

        }

        private void dp_level_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (this.tv_Config.SelectedItem == null || this.dp_level.SelectedItem == null) return;

            var current = ((TreeViewItem)(this.tv_Config.SelectedItem)).Tag as ConfigTree;
            var item = this.dp_level.SelectedItem as ComboBoxItem;
            current.Value.SectionLevel = ((SerializableItem)item.Tag).Name;
            SetUpdateState();

        }

        private string _txt;
        private void txt_Focued(object sender, RoutedEventArgs e)
        {
            _txt = ((TextBox)sender).Text;
        }

        private void txt_lostFocuded(object sender, RoutedEventArgs e)
        {
            var curtxt = ((TextBox)sender).Text;
            if (_txt == curtxt || this.tv_Config.SelectedItem == null) return;

            SetUpdateState();
        }



        private void VaildSectionName()
        {
            var current = ((TreeViewItem)(this.tv_Config.SelectedItem)).Tag as ConfigTree;


            //step1 checkLocal

            IEnumerable<TreeViewItem> items = null;
            if (((TreeViewItem)(this.tv_Config.SelectedItem)).Parent is TreeViewItem)
            {
                var parent = ((TreeViewItem)(this.tv_Config.SelectedItem)).Parent as TreeViewItem;
                items = parent.Items.Where(x => x != this.tv_Config.SelectedItem).Select(x => (TreeViewItem)x);
            }
            else
            {
                var parent = ((TreeViewItem)(this.tv_Config.SelectedItem)).Parent as TreeView;
                items = parent.Items.Where(x => x != this.tv_Config.SelectedItem).Select(x => (TreeViewItem)x);
            }
            bool isValid = !items.Any(x => x.Header.ToString() == this.txt_sectionName.Text.Trim());

            //step checkDb

            if (isValid)
            {
                this.bi_root.IsBusy = true;
                _client.IsValidNameCompleted += (s, arg) =>
                {
                    if (arg.Error != null)
                    {
                        ShowServerError(arg.Error);
                    }
                    else
                    {
                        isValid = arg.Result.Result;
                        IsValidSectionName(isValid, current.Value);
                    }
                    bi_root.IsBusy = false;
                };

                _client.IsValidNameAsync(current.Value.Id, this.txt_sectionName.Text.Trim(), current.Value.ParentId);
            }
            else
            {
                IsValidSectionName(isValid, current.Value);

            }

        }

        private void ShowServerError(Exception exp)
        {
            var errorWin = new AppConfigManagement.ErrorWindow(exp);
            errorWin.Show();
        }

        private void IsValidSectionName(bool isValid, ConfigItem current)
        {
            this._hasError = !isValid;

            if (current.ErrorMap == null)
            {
                current.ErrorMap = new Dictionary<string, string>();
            }
            if (!isValid)
            {
                current.AddError("SECTION_NAME", ApplicationContext.Instance.StringResourceReader.GetString("SystemManager_Name_Duplicate"));
            }
            else
            {
                current.RemoveError("SECTION_NAME");

            }
        }

        private void SetUpdateState()
        {

            var current = ((TreeViewItem)(this.tv_Config.SelectedItem)).Tag as ConfigTree;
            var state = new ItemState
            {
                Item = current,
                State = ObjState.Update,
            };
            AddChangedItem(state);
        }

        public event PropertyChangedEventHandler PropertyChanged;



        private void txt_sectionName_TextChanged(object sender, TextChangedEventArgs e)
        {
            VaildSectionName();
        }


    }
}
