using Gsafety.PTMS.Base.Contract.Data;
using Gsafety.PTMS.BaseInformation.Contract.Data;
using Gsafety.PTMS.Common.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Gsafety.PTMS.BaseInformation.Contract.Models
{
    [DataContract]
    [KnownType(typeof(PagingInfo))]
    [KnownType(typeof(StatusEnum))]
    public class OrderClientManagerQueryModel : PagingInfo
    {
        int? _status;
        ///<summary>
        ///名称
        ///</summary>
        [DataMember]
        public int? Status
        {
            get
            {
                return _status;
            }
            set
            {
                _status = value;
            }
        }

        private string _name;
        [DataMember]
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }


        public override string ToString()
        {
            var builder = new StringBuilder();
            builder.Append(base.ToString());

            return builder.ToString();
        }
    }
}
