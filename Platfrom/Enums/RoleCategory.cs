using Gsafety.Common.Localization.Resource;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace Gsafety.PTMS.Bases.Enums
{
    public enum RoleCategory : short
    {
        /// <summary>
        /// 超级管理员
        /// </summary>
        [Description("SuperPower")]
        [EnumAttribute(ResourceName = "SuperPower")]
        SuperPower = 0,
        /// <summary>
        /// 定单客户
        /// </summary>
        [Description("ClientAdmin")]
        [EnumAttribute(ResourceName = "ClientAdmin")]
        ClientAdmin,
        /// <summary>
        /// 系统管理员
        /// </summary>
        [Description("SecurityAdmin")]
        [EnumAttribute(ResourceName = "SecurityAdmin")]
        SecurityAdmin,
        /// <summary>
        /// 监督管理员
        /// </summary>
        [Description("SecurityMonitor")]
        [EnumAttribute(ResourceName = "SecurityMonitor")]
        SecurityMonitor,
        /// <summary>
        /// 运维管理员
        /// </summary>
        [Description("MaintainAdmin")]
        [EnumAttribute(ResourceName = "MaintainAdmin")]
        MaintainAdmin,
        /// <summary>
        /// 运维人员
        /// </summary>
        [Description("MaintainMonitor")]
        [EnumAttribute(ResourceName = "MaintainMonitor")]
        MaintainMonitor
    }

    public class RoleCategoryConverter : EnumAdapter<RoleCategory>, IValueConverter
    {
        IList<EnumInfos> infoss = new List<EnumInfos>();

        public RoleCategoryConverter()
        {
            infoss = base.GetEnumInfos();
        }

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            try
            {
                var info = infoss.FirstOrDefault(t => t.Value == System.Convert.ToInt32(value));
                return info.LocalizedString;
            }
            catch (System.Exception ex)
            {
            }

            return string.Empty;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
