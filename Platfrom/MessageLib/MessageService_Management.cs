using Gsafety.Common.Logging;
using Gsafety.Common.Util;
using Gsafety.MQ;
using Gsafety.PTMS.Analysis.Helper;
using Gsafety.PTMS.Common.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gsafety.PTMS.MessageLib
{
    public partial class MessageService
    {
        public void ClientChange(OrderClient client)
        {
            try
            {
                LoggerManager.Logger.Info("ClientStatusChange");
                byte[] msg = ConvertHelper.ObjectToBytes(client);

                SendMessage(Constdefine.APPEXCHANGE, ManagementRoute.ClientModeChange, msg);
            }
            catch (Exception ex)
            {
                LoggerManager.Logger.Error(ex);
            }
        }
    }
}
