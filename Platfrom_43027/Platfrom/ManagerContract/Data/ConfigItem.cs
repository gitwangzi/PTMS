using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Gsafety.PTMS.Manager.Contract.Data
{
    /// <summary>
    /// ConfigItem
    /// </summary>
    [DataContract]
    public class ConfigItem
    {
        public ConfigItem()
        {
            this.Id = Guid.NewGuid().ToString();
        }
        /// <summary>
        /// ID
        /// </summary>
        [DataMember]
        public string Id { get; set; }
        /// <summary>
        /// SectionName
        /// </summary>
        [DataMember]
        public string SectionName { get; set; }
        /// <summary>
        /// SectionDescription
        /// </summary>
        [DataMember]
        public string SectionDescription { get; set; }
        /// <summary>
        /// SectionValue
        /// </summary>
        [DataMember]
        public string SectionValue { get; set; }
        /// <summary>
        /// SectionType
        /// </summary>
        [DataMember]
        public string SectionType { get; set; }
        /// <summary>
        /// SectionLevel
        /// </summary>
        [DataMember]
        public string SectionLevel { get; set; }
        /// <summary>
        /// ParentId
        /// </summary>
        [DataMember]
        public string ParentId { get; set; }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            if (!string.IsNullOrEmpty(Convert.ToString(Id)))
            {
                builder.AppendLine("Id:" + Id.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(SectionName)))
            {
                builder.AppendLine("SectionName:" + SectionName.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(SectionDescription)))
            {
                builder.AppendLine("SectionDescription:" + SectionDescription.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(SectionValue)))
            {
                builder.AppendLine("SectionValue:" + SectionValue.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(SectionType)))
            {
                builder.AppendLine("SectionType:" + SectionType.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(SectionLevel)))
            {
                builder.AppendLine("SectionLevel:" + SectionLevel.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(ParentId)))
            {
                builder.AppendLine("ParentId:" + ParentId.ToString());
            }
            return builder.ToString();
        }

    }
}
