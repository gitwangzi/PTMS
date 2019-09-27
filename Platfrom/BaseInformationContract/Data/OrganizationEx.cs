using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Gsafety.PTMS.BaseInformation.Contract.Data
{
    [DataContract]
    [KnownType(typeof(Organization))]
    public class OrganizationEx : Organization
    {
        private int row;
        [DataMember]
        public int Row
        {
            get { return row; }
            set { row = value; }
        }

        private string organizationName;
        [DataMember]
        public string OrganizationName
        {
            get { return organizationName; }
            set { organizationName = value; }
        }
    }
}
