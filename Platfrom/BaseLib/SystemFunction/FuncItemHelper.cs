using System;
using System.Net;
using System.Resources;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Xml.Linq;
using System.IO;
using Gsafety.PTMS.Bases.Enums;
using System.Linq;
using Gsafety.PTMS.Share;
using System.Collections.Generic;

namespace BaseLib.SystemFunction
{
    public class FuncItemHelper
    {
        private static Dictionary<RoleCategory, FunctionItem> _cache = new Dictionary<RoleCategory, FunctionItem>();
        public static FunctionItem RoleManageFunc;
        public static FunctionItem CloudUserManageFunc;
        public static FunctionItem MANAGER_UserMangerFunc;

        private string _dependName = "Depend";
        private string _funcItemName = "FuncItem";
        private string _funcItemsName = "FuncItems.xml";

        static FuncItemHelper()
        {
            try
            {
                var helper = new FuncItemHelper();
                var doc = helper.GetDoc();

                var list = new List<RoleCategory>()
                {
                    RoleCategory.SecurityMonitor,RoleCategory.MaintainMonitor
                };

                foreach (var item in list)
                {
                    var roleFuncItemElement = helper.GetFuncItemElementByRoleType(doc, item);
                    var roleFuncItem = helper.ParseFuncItem(roleFuncItemElement, null);
                    _cache[item] = roleFuncItem;
                }

                var SystemManageMenu = _cache[RoleCategory.SecurityMonitor].Children.FirstOrDefault(t => t.ID == "02-08");
                MANAGER_UserMangerFunc = SystemManageMenu.Children.FirstOrDefault(t => t.ID == "02-08-01");
                RoleManageFunc = MANAGER_UserMangerFunc.Children.FirstOrDefault(t => t.ID == "02-08-01-01");
                CloudUserManageFunc = MANAGER_UserMangerFunc.Children.FirstOrDefault(t => t.ID == "02-08-01-02");
            }
            catch (System.Exception ex)
            {
                throw new Exception("read FuncItems Failed", ex);
            }
        }

        public static FunctionItem GetFuncItemByRoleType(RoleCategory roleCategory)
        {
            return _cache[roleCategory];
        }

        private FunctionItem ParseFuncItem(XElement functionItemElement, FunctionItem parent)
        {
            var item = CreateFunctionItem(functionItemElement, parent);

            if (functionItemElement.HasElements)
            {
                foreach (var xElement in functionItemElement.Elements())
                {
                    if (xElement.Name == _dependName)
                    {
                        var dependID = ParseDepend(xElement);
                        var depend = item.Parent.Children.FirstOrDefault(t => t.ID == dependID);
                        if (depend != null)
                        {
                            item.Depends.Add(depend);
                        }
                        else
                        {
                            throw new Exception("权限配置文件错误，依赖权限未找到");
                        }
                    }
                    else if (xElement.Name == _funcItemName)
                    {
                        var child = ParseFuncItem(xElement, item);
                        item.Children.Add(child);
                    }
                }
            }

            return item;
        }

        private static string ParseDepend(XElement xElement)
        {
            var dependID = xElement.Attribute("ID").Value;
            return dependID;
        }

        private FunctionItem CreateFunctionItem(XElement functionItemElement, FunctionItem parent)
        {
            var id = functionItemElement.Attribute("ID").Value;
            var name = functionItemElement.Attribute("Name").Value;
            name = ApplicationContext.Instance.StringResourceReader.GetString(name);
            var url = functionItemElement.Attribute("URL").Value;
            var version = functionItemElement.Attribute("Version").Value;
            var module = functionItemElement.Attribute("Module").Value;

            var item = new FunctionItem()
            {
                ID = id,
                Name = name,
                URL = url,
                Version = version,
                Module = module,
                Parent = parent
            };
            return item;
        }

        private XElement GetFuncItemElementByRoleType(XDocument xDoc, RoleCategory roleCategory)
        {
            var xElement = xDoc.Root.Elements().FirstOrDefault(t => t.Attribute("Name").Value == roleCategory.ToString());
            return xElement;
        }

        private XDocument GetDoc()
        {
            var assembly = this.GetType().Assembly;
            using (var stream = assembly.GetManifestResourceStream(assembly.FullName.Split(',')[0] + "." + _funcItemsName))
            {
                using (var reader = new StreamReader(stream))
                {
                    var result = reader.ReadToEnd();
                    return XDocument.Parse(result);
                }
            }
        }
    }
}
