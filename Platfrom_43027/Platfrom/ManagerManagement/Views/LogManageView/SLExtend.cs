/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: 0e27053a-0070-4d6e-911d-cb63730c5b08      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: PC-WANGZS
/////                 Author: TEST(wangzs)
/////======================================================================
/////           Project Name: Gsafety.PTMS.Manager.Views.LogManageView
/////    Project Description:    
/////             Class Name: SLExtend
/////          Class Version: v1.0.0.0
/////            Create Time: 2013/8/13 14:24:04
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2013/8/13 14:24:04
/////            Modified by:
/////   Modified Description: 
/////======================================================================
using System;
using System.Collections.Generic;
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
using System.Collections;
using System.Text;
using System.Windows.Data;
using System.Reflection;

namespace Gsafety.PTMS.Manager.Views.LogManageView
{
    public static class SLExtend
    {
        public static string ExportDataGrid(this DataGrid grid, bool withHeaders)
        {
            string colPath;
            PropertyInfo propInfo;
            Binding binding;
            StringBuilder strBuilder = new System.Text.StringBuilder();
            IEnumerable source = (grid.ItemsSource as IEnumerable);
            if (source == null)
                return "";

            List<string> headers = new List<string>();
            grid.Columns.ToList().ForEach(col =>
            {
                if (col is DataGridBoundColumn)
                {
                    headers.Add(FormatCSVField(col.Header.ToString()));
                }
            });
            strBuilder
            .Append(String.Join("", headers.ToArray()))
            .Append("\t\n");

            foreach (Object data in source)
            {
                List<string> csvRow = new List<string>();
                foreach (DataGridColumn col in grid.Columns)
                {
                    if (col is DataGridBoundColumn)
                    {
                        binding = (col as DataGridBoundColumn).Binding;
                        colPath = binding.Path.Path;
                        string[] pathlist = colPath.Split('.');

                        object currentData = data;
                        int count = 0;
                        foreach (string item in pathlist)
                        {
                            propInfo = currentData.GetType().GetProperty(item);
                            if (propInfo == null) break;
                            count++;
                            if (count == pathlist.Count())
                            {
                                string dt = propInfo.GetValue(currentData, null) != null ? propInfo.GetValue(currentData, null).ToString() : string.Empty;
                                csvRow.Add(FormatCSVField(dt));
                                break;
                            }
                            else
                            {
                                currentData = propInfo.GetValue(currentData, null);
                            }
                        }
                    }
                }
                strBuilder
                    .Append(String.Join("", csvRow.ToArray()))
                    .Append("\t\n");
            }
            return strBuilder.ToString();
        }
        private static string FormatCSVField(string data)
        {
            return String.Format("{0}\t", data.Replace("\"", "\t\n"));
        }

        public static void ExportExcel(DataGrid grid)
        {
            string data = grid.ExportDataGrid(true);
            byte[] tmp;
            tmp = Encoding.Unicode.GetBytes(data);
            SaveFileDialog sfd = new SaveFileDialog()
            {
                DefaultExt = "csv",
                Filter = "CSV Files (*.csv)|*.csv|All files (*.*)|*.*",
                FilterIndex = 1
            };
            if (sfd.ShowDialog() == true)
            {
                using (System.IO.Stream stream = sfd.OpenFile())
                {
                    using (System.IO.StreamWriter writer = new System.IO.StreamWriter(stream, Encoding.Unicode, 30))
                    {
                        writer.Write(data);
                        writer.Close();
                    }
                    stream.Close();
                }
            }
        }
    }
}
