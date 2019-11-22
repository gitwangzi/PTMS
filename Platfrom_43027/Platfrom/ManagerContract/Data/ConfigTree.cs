using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Gsafety.PTMS.Manager.Contract.Data
{
    /// <summary>
    /// ConfigTree
    /// </summary>
    [DataContract]
    public class ConfigTree
    {
        public ConfigTree()
        {
            this.Children = new List<ConfigTree>();
        }
        /// <summary>
        /// Value
        /// </summary>
        [DataMember]
        public ConfigItem Value { get; set; }
        /// <summary>
        /// Children
        /// </summary>
        [DataMember]
        public List<ConfigTree> Children { get; set; }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            if (!string.IsNullOrEmpty(Convert.ToString(Value)))
            {
                builder.AppendLine("Value:" + Value.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(Children)))
            {
                builder.AppendLine("Children:" + Children.ToString());
            }
            return builder.ToString();
        }

    }
}
