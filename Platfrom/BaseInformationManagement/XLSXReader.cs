/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: 87050cdc-d9c5-4a85-bc3f-d47f9274ac85      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: PC-PENGGL
/////                 Author: TEST(penggl)
/////======================================================================
/////           Project Name: Gsafety.PTMS.BaseInformation
/////    Project Description:    
/////             Class Name: XLSXReader
/////          Class Version: v1.0.0.0
/////            Create Time: 2013/8/31 15:23:52
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2013/8/31 15:23:52
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
using System.Xml.Linq;
using System.Xml;
using System.IO;
using System.Globalization;
using System.Collections;
using System.Linq;


namespace Gsafety.PTMS.BaseInformation
{
    public class XLSXReader
    {
        FileInfo theFile;
        internal static XNamespace excelNamespace = XNamespace.Get("http://schemas.openxmlformats.org/spreadsheetml/2006/main");
        internal static XNamespace excelRelationshipsNamepace = XNamespace.Get("http://schemas.openxmlformats.org/officeDocument/2006/relationships");

        const string sharedStringsMarker = @"xl/sharedStrings.xml";
        const string worksheetsMarker = @"/xl/worksheets/";
        const string worksheetMarker = "/xl/worksheets/sheet{0}.xml";
        const string workbooksMarker = @"xl/workbook.xml";


        private Dictionary<int, string> sharedStrings;

        public XLSXReader(FileInfo _file)
        {
            theFile = _file;
        }

        public string GetFilename()
        {
            return theFile.Name;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="worksheetID"></param>
        /// <returns></returns>
        public XElement GetWorksheet(int worksheetID)
        {
            string worksheetMarker = String.Format(CultureInfo.CurrentCulture, "xl/worksheets/sheet{0}.xml", worksheetID);

            return GetXLSXPart(worksheetMarker);
        }

        public List<string> GetListSubItems()
        {

            XElement worksheetElement = GetXLSXPart(workbooksMarker);

            IEnumerable<XElement> workSheetItems = from s in worksheetElement.Descendants(XLSXReader.excelNamespace + "sheet")
                                                   select s;

            IEnumerable<XAttribute> workSheetAttrs = workSheetItems.Attributes("name");

            List<string> worksheets = new List<string>();

            foreach (XAttribute xattr in workSheetAttrs)
            {
                worksheets.Add(xattr.Value);
            }

            // a good place to get the sharedStrings included in the xlsx file
            ParseSharedStrings(GetSharedStringPart());

            return worksheets;

        }



        private XElement GetSharedStringPart()
        {
            return GetXLSXPart(sharedStringsMarker);
        }

        /// <summary>
        /// Extracts from the xslx file the part specified with partMarker
        /// </summary>
        /// <param name="partMarker"></param>
        /// <returns></returns>
        private XElement GetXLSXPart(string partMarker)
        {

            Stream s = theFile.OpenRead();
            using (UnZipper unzip = new UnZipper(s))
            {
                XElement partElement = null;

                foreach (string filename in unzip.GetFileNamesInZip())
                {
                    Stream partStream = unzip.GetFileStream(filename);
                    if (filename == partMarker)
                    {
                        partElement = XElement.Load(XmlReader.Create(partStream));
                        partStream.Close();
                        return partElement;
                    }
                }
            }
            return null;
        }


        public int GetWorksheetIndex(string itemSelected)
        {
            if (string.IsNullOrEmpty(itemSelected) == true)
                return 0;

            List<string> worksheets = GetListSubItems();

            if (worksheets == null)
                return 0;

            int count = 0;
            foreach (string worksheetName in worksheets)
            {
                count += 1;
                if (worksheetName == itemSelected)
                {
                    return count;
                }
            }
            return 0;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="WorksheetIdex"></param>
        /// <returns></returns>
        public IEnumerable<IDictionary> GetData(string itemSelected)
        {
            int worksheetIdex = GetWorksheetIndex(itemSelected);

            if (worksheetIdex <= 0)
                yield break;

            XElement wsSelectedElement = GetWorksheet(worksheetIdex);
            if (wsSelectedElement == null)
                yield break;

            IEnumerable<XElement> rowsExcel = from row in wsSelectedElement.Descendants(XLSXReader.excelNamespace + "row")
                                              select row;

            if (rowsExcel == null)
                yield break;

            foreach (XElement row in rowsExcel)
            {
                var dict = new Dictionary<string, object>();
                IEnumerable<XElement> cellsRow = row.Elements(XLSXReader.excelNamespace + "c");
                foreach (XElement cell in cellsRow)
                {
                    if (cell.HasElements == true)
                    {
                        string cellValue = cell.Element(XLSXReader.excelNamespace + "v").Value;
                        if (cell.Attribute("t") != null)
                        {
                            if (cell.Attribute("t").Value == "s")
                            {
                                cellValue = sharedStrings[Convert.ToInt32(cellValue)];
                            }
                        }

                        var index = cell.Attribute("r").Value;
                        var flag = index.FirstOrDefault(t => (t >= 65 && t <= 90) == false);
                        var rowIndex = index.Substring(0, index.IndexOf(flag));

                        dict[rowIndex] = cellValue as Object;
                        //dict[cell.Attribute("r").Value.Substring(0, 1)] = cellValue as Object;
                    }
                }
                yield return dict;
            }
        }

        private void ParseSharedStrings(XElement SharedStringsElement)
        {
            sharedStrings = new Dictionary<int, string>();
            IEnumerable<XElement> sharedStringsElements = from s in SharedStringsElement.Descendants(excelNamespace + "t")
                                                          select s;
            int Counter = 0;
            foreach (XElement sharedString in sharedStringsElements)
            {
                sharedStrings.Add(Counter, sharedString.Value);
                Counter++;
            }
            return;
        }
    }
}
