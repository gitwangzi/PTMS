using System;
using System.Net;
using FiftyNine.Ag.OpenXML.Common;
using System.Collections.Generic;
using System.Xml;
using Gsafety.PTMS.Spreadsheet.Utilities;
using FiftyNine.Ag.OpenXML.Common.BaseClasses;

namespace Gsafety.PTMS.Spreadsheet.Elements
{
    public abstract class SpreadsheetElement : OpenXMLElement
    {
        public override void Save(XmlWriter writer)
        {
            writer.WriteStartElement(NodeName);
            SaveContent(writer);
            writer.WriteEndElement();
        }

        protected override Namespace Namespace
        {
            get { return SpreadsheetNamespaces.Main; }
        }
    }
}

