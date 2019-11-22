/////Copyright (C) Gsafety 2014 .All Rights Reserved.
/////======================================================================
/////                   Guid: c178330c-2c6b-476d-8e5d-2fc54dff85af      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: PC-ZHOUDD
/////                 Author: GJSY(zhoudd)
/////======================================================================
/////           Project Name: Gsafety.PTMS.Manager
/////    Project Description:    
/////             Class Name: AddEnumWindow
/////          Class Version: v1.0.0.0
/////            Create Time: 2013/07/24 17:57:28
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2014/10/13 17:57:28
/////            Modified by:
/////   Modified Description: 
/////======================================================================
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace Gsafety.PTMS.Manager
{
    public partial class AddEnumWindow : ChildWindow
    {

        public ObservableCollection<string> SourceList { get; internal set; }

        public AddEnumWindow()
        {
            InitializeComponent();
            SetListItemSource();
        }

        private void SetListItemSource()
        {
            SourceList = new ObservableCollection<string>();
            this.lb_enumValues.ItemsSource = SourceList;
        }

        public void AddItem(string str)
        {
             
            this.SourceList.Add(str.Replace(',','，'));
            
        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        private void lb_enumValues_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (this.lb_enumValues.SelectedIndex >= 0)
            {
                this.txt_value.Text = lb_enumValues.SelectedValue.ToString();
                SourceList.Remove(this.txt_value.Text);
            }
        }

        private void btn_add_Click(object sender, RoutedEventArgs e)
        {
            if (CheckText())
            {
                AddItem(this.txt_value.Text);
                this.txt_value.Text = "";
            }
        }


        private bool CheckText()
        {
            if (string.IsNullOrWhiteSpace(this.txt_value.Text))
            {
                string msg = "文本内容不能为空";
                MessageBox.Show(msg);
                return false;
            }

            if (SourceList.Contains(this.txt_value.Text))
            {
                string msg = "添加的内容重复";
                MessageBox.Show(msg);
                return false;
            }
            return true;
        }
    }
}

