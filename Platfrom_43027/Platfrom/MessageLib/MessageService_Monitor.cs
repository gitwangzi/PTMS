using Gsafety.Common.Logging;
using Gsafety.Common.Util;
using Gsafety.PTMS.Common.Data;
using Gsafety.PTMS.Message.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gsafety.PTMS.MessageLib
{
    public partial class MessageService
    {
        private void OnOffLine(byte[] bytes, string key)
        {
            try
            {
                LoggerManager.Logger.Info("OnOffLine");
                OnOfflineEx model = ConvertHelper.BytesToObject(bytes) as OnOfflineEx;
                if (model.OrganizationID != null)
                {
                    if (DataManager.Organization.ContainsKey(model.OrganizationID))
                    {
                        if (DataManager.Organization.ContainsKey(model.OrganizationID))
                        {
                            Dictionary<string, IMessageCallBackExt> dictionary = DataManager.Organization[model.OrganizationID];

                            foreach (var user in dictionary.Keys)
                            {
                                try
                                {
                                    var callback = dictionary[user];
                                    callback.MessageCallBack(model);
                                    LoggerManager.Logger.Info("Send OnLineOffLine To :" + user);
                                }
                                catch (Exception ex)
                                {
                                    LoggerManager.Logger.Error(ex);
                                }

                            }
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                LoggerManager.Logger.Error(ex);
            }
        }
    }
}
