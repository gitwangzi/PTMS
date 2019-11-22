using Gsafety.Common.Logging;
using Gsafety.Common.Util;
using Gsafety.PTMS.Analysis.Helper;
using Gsafety.PTMS.Base.Contract.Data;
using Gsafety.PTMS.BaseInformation.Contract.Data;
using Gsafety.PTMS.DBEntity;
using Gsafety.PTMS.Message.Contract.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gsafety.PTMS.CommandManagement.Management
{
    public class C57CommandManager : BasicInfoManager
    {
        public static void CommandSend(PTMSEntities context, byte[] bytes, string key)
        {
                List<string> lstMdvrs = new List<string>();
            string str = System.Text.UnicodeEncoding.UTF8.GetString(bytes);
            var model = ConvertHelper.BytesToObject(bytes) as SendInfomationModel;
            lstMdvrs = GetLstMdvrs(context,model.Value);
          
            foreach (string mdvrid in lstMdvrs)
            {
                model.Setting.DvId = mdvrid;
                model.Setting.MsgId = mdvrid;
                var commandContent = System.Text.UTF8Encoding.UTF8.GetBytes(model.Setting.ToString());
                CommandManager.PublishMessage(
                    Gsafety.MQ.Constdefine.MDVREXCHANGE,
                    string.Format("{0}{1}", "MDVR.C57.", mdvrid), commandContent
                    );
            }
        }
    }
}


