using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gsafety.PTMS.Common.Data
{
    [Serializable]
    public class SendRecord
    {
        private string _mdvrcoresn;
        public string MdvrCoreSn
        {
            get
            {
                return _mdvrcoresn;
            }
            set
            {
                _mdvrcoresn = value;
            }
        }

        private string _operationid;
        public string OperationID
        {
            get
            {
                return _operationid;
            }
            set
            {
                _operationid = value;
            }
        }

        private string _cmdtype;
        public string CmdType
        {
            get
            {
                return _cmdtype;
            }
            set
            {
                _cmdtype = value;
            }
        }

        private string _vehicleid;
        public string VehicleID
        {
            get
            {
                return _vehicleid;
            }
            set
            {
                _vehicleid = value;
            }
        }

        private string _content;
        public string Content
        {
            get
            {
                return _content;
            }
            set
            {
                _content = value;
            }
        }
    }
}
