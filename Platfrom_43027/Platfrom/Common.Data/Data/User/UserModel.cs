using Gsafety.PTMS.Common.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Gsafety.PTMS.Message.Contract
{
    [DataContract]
    [Serializable]
    [KnownType(typeof(BaseInfo))]
    public class UserModel : BaseInfo
    {
        List<string> _organization = new List<string>();
        [DataMember]
        public List<string> Organization
        {
            get { return _organization; }
            set { _organization = value; }
        }

        /// <summary>
        /// 是否是报警处理员
        /// </summary>
        [DataMember]
        public bool AlarmProcessor { get; set; }

        List<string> _vehicles = new List<string>();
        [DataMember]
        public List<string> Vehicles
        {
            get { return _vehicles; }
            set { _vehicles = value; }
        }

        string _usertoken = string.Empty;
        [DataMember]
        public string UserToken
        {
            get { return _usertoken; }
            set { _usertoken = value; }
        }

        public override string ToString()
        {
            return base.ToString();
        }
    }
}
