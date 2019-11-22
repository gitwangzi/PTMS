using Gsafety.PTMS.Base.Contract.Data;
using Gsafety.PTMS.BaseInformation.Contract.Data;
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
    public class VehicleTypeQueryModel : PagingInfo
    {
        DateTime? _openbegintime;
        ///<summary>
        ///shijian
        ///</summary>
        [DataMember]
        public DateTime? OpenBeginTime
        {
            get
            {
                return _openbegintime;
            }
            set
            {
                _openbegintime = value;
            }
        }

        DateTime? _openendtime;
        ///<summary>
        ///名称
        ///</summary>
        [DataMember]
        public DateTime? OpenEndTime
        {
            get
            {
                return _openendtime;
            }
            set
            {
                _openendtime = value;
            }
        }

        DateTime? _closebegintime;
        ///<summary>
        ///名称
        ///</summary>
        [DataMember]
        public DateTime? CloseBeginTime
        {
            get
            {
                return _closebegintime;
            }
            set
            {
                _closebegintime = value;
            }
        }

        DateTime? _closeendtime;
        ///<summary>
        ///名称
        ///</summary>
        [DataMember]
        public DateTime? CloseEndTime
        {
            get
            {
                return _closeendtime;
            }
            set
            {
                _closeendtime = value;
            }
        }

        public override string ToString()
        {
            var builder = new StringBuilder();
            builder.Append(base.ToString());

            if (OpenBeginTime.HasValue)
            {
                builder.AppendLine("OpenBeginTime:" + OpenBeginTime.Value.ToString());
            }
            if (OpenEndTime.HasValue)
            {
                builder.AppendLine("OpenEndTime:" + OpenEndTime.Value.ToString());
            }
            if (CloseBeginTime.HasValue)
            {
                builder.AppendLine("CloseBeginTime:" + CloseBeginTime.Value.ToString());
            }
            if (CloseEndTime.HasValue)
            {
                builder.AppendLine("CloseEndTime:" + CloseEndTime.Value.ToString());
            }
            return builder.ToString();
        }
    }
}
