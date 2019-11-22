using FiftyNine.Ag.OpenXML.Common.Storage;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Reflection;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Linq;

namespace Gsafety.PTMS.Spreadsheet
{
    public class FieldEx
    {
        /// <summary>
        /// Key
        /// </summary>
        public string Key { get; set; }
        /// <summary>
        /// Value
        /// </summary>
        public string Value { get; set; }
    }

    public class EnumsEx
    {
        /// <summary>
        /// Code
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// Content
        /// </summary>
        public List<FieldEx> Content { get; set; }
    }

    public class XLSXExporter
    {
        public XLSXExporter()
        {

        }

        public void Export<T>(List<T> list, Stream stream)
        {
            SpreadsheetDocument doc = new SpreadsheetDocument();
            doc.ApplicationName = "SilverSpreadsheet";
            doc.Creator = "GsEye";
            doc.Company = "PTMS";

            PropertyInfo[] propertyinfo = typeof(T).GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly);

            SetMargin(doc);

            int headcell = 0;
            foreach (PropertyInfo item in propertyinfo)
            {
                doc.Workbook.Sheets[0].Sheet.Rows[0].Cells[headcell].SetValue(item.Name);
                headcell++;
            }

            int contentrow = 1;
            foreach (T t in list)
            {
                int contentcell = 0;
                Type type = typeof(T);
                foreach (PropertyInfo item in propertyinfo)
                {
                    object value = item.GetValue(t, null);
                    if (value != null)
                    {
                        doc.Workbook.Sheets[0].Sheet.Rows[contentrow].Cells[contentcell].SetValue(value.ToString());
                    }
                    else
                    {
                        doc.Workbook.Sheets[0].Sheet.Rows[contentrow].Cells[contentcell].SetValue(string.Empty);
                    }
                    contentcell++;
                }
                contentrow++;
            }

            doc.Workbook.Sheets[0].Sheet.Rows[contentrow + 1].Cells[0].SetValue(DateTime.Now.ToString());
            using (IStreamProvider storage = new ZipStreamProvider(stream))
            {
                doc.Save(storage);
            }
        }

        public void Export<T>(List<T> list, Stream stream, List<string> Codes, List<string> Names)
        {
            SpreadsheetDocument doc = new SpreadsheetDocument();
            doc.ApplicationName = "SilverSpreadsheet";
            doc.Creator = "GsEye";
            doc.Company = "PTMS";

            PropertyInfo[] propertyinfo = typeof(T).GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly);

            //int headcell = 0;
            //foreach (PropertyInfo item in propertyinfo)
            //{
            //    if (!Codes.Contains(item.Name)) continue;

            //    doc.Workbook.Sheets[0].Sheet.Rows[0].Cells[Codes.IndexOf(item.Name)].SetValue(item.Name);

            //    //headcell++;
            //}

            SetMargin(doc);

            int headcell = 0;
            Names.ForEach(x =>
                {
                    doc.Workbook.Sheets[0].Sheet.Rows[0].Cells[headcell].SetValue(x);
                    headcell++;
                });

            int contentrow = 1;
            foreach (T t in list)
            {
                //int contentcell = 0;
                Type type = typeof(T);
                foreach (PropertyInfo item in propertyinfo)
                {
                    if (!Codes.Contains(item.Name)) continue;

                    object value = item.GetValue(t, null);
                    if (value != null)
                    {
                        doc.Workbook.Sheets[0].Sheet.Rows[contentrow].Cells[Codes.IndexOf(item.Name)].SetValue(value.ToString());
                    }
                    else
                    {
                        doc.Workbook.Sheets[0].Sheet.Rows[contentrow].Cells[Codes.IndexOf(item.Name)].SetValue(string.Empty);
                    }
                    //contentcell++;
                }
                contentrow++;
            }

            //doc.Workbook.Sheets[0].Sheet.Rows[contentrow + 1].Cells[0].SetValue(DateTime.Now.ToString());

            using (IStreamProvider storage = new ZipStreamProvider(stream))
            {
                doc.Save(storage);
            }
        }

        public void Export<T>(List<T> list, Stream stream, List<string> Codes, List<string> Names, List<EnumsEx> elist)
        {
            SpreadsheetDocument doc = new SpreadsheetDocument();
            doc.ApplicationName = "SilverSpreadsheet";
            doc.Creator = "GsEye";
            doc.Company = "PTMS";

            PropertyInfo[] propertyinfo = typeof(T).GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly);

            //int headcell = 0;
            //foreach (PropertyInfo item in propertyinfo)
            //{
            //    if (!Codes.Contains(item.Name)) continue;

            //    doc.Workbook.Sheets[0].Sheet.Rows[0].Cells[Codes.IndexOf(item.Name)].SetValue(item.Name);

            //    //headcell++;
            //}

            SetMargin(doc);

            int headcell = 0;
            Names.ForEach(x =>
            {
                doc.Workbook.Sheets[0].Sheet.Rows[0].Cells[headcell].SetValue(x);
                headcell++;
            });

            int contentrow = 1;
            foreach (T t in list)
            {
                //int contentcell = 0;
                Type type = typeof(T);
                foreach (PropertyInfo item in propertyinfo)
                {
                    if (!Codes.Contains(item.Name)) continue;

                    object value = item.GetValue(t, null);

                    if (value != null)
                    {
                        string strValue = value.ToString();
                        if (elist.Any(y => y.Code == item.Name))
                        {
                            var result = elist.Where(y => y.Code == item.Name).FirstOrDefault().Content;

                            if (result.Any(x => x.Key == strValue))
                            {
                                doc.Workbook.Sheets[0].Sheet.Rows[contentrow].Cells[Codes.IndexOf(item.Name)].SetValue(result.Where(x => x.Key == strValue).FirstOrDefault().Value);
                            }
                            else
                            {
                                doc.Workbook.Sheets[0].Sheet.Rows[contentrow].Cells[Codes.IndexOf(item.Name)].SetValue(strValue);
                            }
                        }
                        else
                        {
                            doc.Workbook.Sheets[0].Sheet.Rows[contentrow].Cells[Codes.IndexOf(item.Name)].SetValue(strValue);
                        }
                    }
                    else
                    {
                        doc.Workbook.Sheets[0].Sheet.Rows[contentrow].Cells[Codes.IndexOf(item.Name)].SetValue(string.Empty);
                    }
                    //contentcell++;
                }
                contentrow++;
            }

            //doc.Workbook.Sheets[0].Sheet.Rows[contentrow + 1].Cells[0].SetValue(DateTime.Now.ToString());

            using (IStreamProvider storage = new ZipStreamProvider(stream))
            {
                doc.Save(storage);
            }
        }

        public void Export(List<Dictionary<string, string>> list, Stream stream, List<string> Codes, List<string> Names)
        {
            SpreadsheetDocument doc = new SpreadsheetDocument();
            doc.ApplicationName = "SilverSpreadsheet";
            doc.Creator = "GsEye";
            doc.Company = "PTMS";

            SetMargin(doc);

            int headcell = 0;
            Names.ForEach(x =>
            {
                doc.Workbook.Sheets[0].Sheet.Rows[0].Cells[headcell].SetValue(x);
                headcell++;
            });

            int contentrow = 1;
            foreach (var s in list)
            {
                foreach (var item in s)
                {
                    string strValue = item.Value;
                    if (item.Value != null)
                    {
                        doc.Workbook.Sheets[0].Sheet.Rows[contentrow].Cells[Codes.IndexOf(item.Key)].SetValue(strValue);
                    }
                    else
                    {
                        doc.Workbook.Sheets[0].Sheet.Rows[contentrow].Cells[Codes.IndexOf(item.Key)].SetValue(string.Empty);
                    }
                }
                contentrow++;
            }

            //doc.Workbook.Sheets[0].Sheet.Rows[contentrow + 1].Cells[0].SetValue(DateTime.Now.ToString());

            using (IStreamProvider storage = new ZipStreamProvider(stream))
            {
                doc.Save(storage);
            }
        }


        public void ExportDataGrid(DataGrid dGrid, Stream stream)
        {
            SpreadsheetDocument doc = new SpreadsheetDocument();
            doc.ApplicationName = "SilverSpreadsheet";
            doc.Creator = "GsEye";
            doc.Company = "PTMS";
            doc.Created = DateTime.Now;

            StringBuilder strBuilder = new StringBuilder();
            if (dGrid.ItemsSource == null)
            {
                return;
            }
            if (dGrid.HeadersVisibility == DataGridHeadersVisibility.Column ||
                dGrid.HeadersVisibility == DataGridHeadersVisibility.All)
            {
                int col = 0;
                foreach (DataGridColumn dgcol in dGrid.Columns)
                {
                    doc.Workbook.Sheets[0].Sheet.Rows[0].Cells[col].SetValue(dgcol.Header.ToString());
                    col++;
                }
            }

            SetMargin(doc);

            int contentrow = 1;
            foreach (object data in dGrid.ItemsSource)
            {
                int contentcell = 0;
                foreach (DataGridColumn col in dGrid.Columns)
                {
                    string strValue = "";
                    Binding objBinding = null;
                    if (col is DataGridBoundColumn)
                    {
                        objBinding = (col as DataGridBoundColumn).Binding;
                    }
                    if (col is DataGridTemplateColumn)
                    {
                        //This is a template column...
                        //    let us see the underlying dependency object
                        DependencyObject objDO = (col as DataGridTemplateColumn).CellTemplate.LoadContent();
                        FrameworkElement oFE = (FrameworkElement)objDO;
                        FieldInfo oFI = oFE.GetType().GetField("TextProperty");
                        if (oFI != null)
                        {
                            if (oFI.GetValue(null) != null)
                            {
                                if (oFE.GetBindingExpression((DependencyProperty)oFI.GetValue(null)) != null)
                                {
                                    objBinding = oFE.GetBindingExpression((DependencyProperty)oFI.GetValue(null)).ParentBinding;
                                }
                            }
                        }
                    }
                    if (objBinding != null)
                    {
                        if (objBinding.Path.Path != "")
                        {
                            PropertyInfo pi = data.GetType().GetProperty(objBinding.Path.Path);
                            if (pi != null)
                            {
                                strValue = pi.GetValue(data, null).ToString();
                            }
                        }
                        if (objBinding.Converter != null)
                        {
                            if (strValue != "")
                            {
                                strValue = objBinding.Converter.Convert(strValue, typeof(string), objBinding.ConverterParameter, objBinding.ConverterCulture).ToString();
                            }
                            else
                            {
                                strValue = objBinding.Converter.Convert(data, typeof(string), objBinding.ConverterParameter, objBinding.ConverterCulture).ToString();
                            }
                        }
                    }
                    doc.Workbook.Sheets[0].Sheet.Rows[contentrow].Cells[contentcell].SetValue(strValue);
                    contentcell++;
                }
                contentrow++;
            }

            using (IStreamProvider storage = new ZipStreamProvider(stream))
            {
                doc.Save(storage);
            }
        }

        private void SetMargin(SpreadsheetDocument doc)
        {
            //try
            //{
            //    foreach (var sheet in doc.Workbook.Sheets)
            //    {
            //        sheet.Sheet.Margin = new Thickness(1, 1, 1, 1);
            //        sheet.Sheet.HeaderSpace = 1;
            //        sheet.Sheet.FooterSpace = 1;
            //    }
            //}
            //catch (System.Exception ex)
            //{

            //}
        }

    }
}
