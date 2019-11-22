using Gsafety.Common.Logging;
using Gsafety.Common.Util;
using Gsafety.PTMS.Common.Data;
using Gsafety.PTMS.DBEntity;
using Gsafety.PTMS.Installation.Repository;
using Gsafety.PTMS.Message.Contract.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gsafety.PTMS.Analysis.DeviceAccess
{
    public class DeviceTreatment
    {

        private static InstallationRepository installationRepository;

        static DeviceTreatment()
        {
            installationRepository = new InstallationRepository();
        }

        public static ReturnInfo Register(PTMSEntities context, byte[] bytes)
        {
            ReturnInfo returnInfo = new ReturnInfo();

            //byte[] -> json
            string str = System.Text.UnicodeEncoding.UTF8.GetString(bytes);
            LoggerManager.Logger.Info(string.Format("Get Register info:{0}", str));
            //json -> entity
            Register register = JsonHelper.FromJsonString<Register>(str);

            RegisterResponse response = new RegisterResponse();

            if (register != null)
            {
                LoggerManager.Logger.Info(string.Format("Before Add RegisterInfo to database!UID:{0}", str));
                //check && storge && response 
                response = installationRepository.Register(context, register);
                LoggerManager.Logger.Info(string.Format("After Add RegisterInfo to database!UID:{0}", str));

                if (response != null)
                {
                    //entity -> json
                    string s = JsonHelper.ToJsonString(response);
                    //json -> byte[]
                    var msg = System.Text.UTF8Encoding.UTF8.GetBytes(s);

                    if (msg != null && msg.Length > 0)
                    {
                        returnInfo.Message = msg;
                    }
                }
            }
            else
            {
                LoggerManager.Logger.Warn(string.Format("Converted register to entity is null,string:{0}", str));
            }
            return returnInfo;
        }

        public static ReturnInfo UnRegister(PTMSEntities context, byte[] bytes)
        {
            ReturnInfo returnInfo = new ReturnInfo();

            //byte[] -> json
            string str = System.Text.UnicodeEncoding.UTF8.GetString(bytes);
            LoggerManager.Logger.Info(string.Format("Get Register info:{0}", str));
            //json -> entity
            UnRegister unregister = JsonHelper.FromJsonString<UnRegister>(str);

            UnRegisterResponse response = new UnRegisterResponse();

            if (unregister != null)
            {
                //fill model 
                //if (FillAlarmModel(AlarmModel, out ruleKey))
                {
                    LoggerManager.Logger.Info(string.Format("Before Add RegisterInfo to database!UID:{0}", str));
                    //check && storge && response 
                    response = installationRepository.DelRegisterInfo(context, unregister);
                    LoggerManager.Logger.Info(string.Format("After Add RegisterInfo to database!UID:{0}", str));

                    if (response != null)
                    {
                        //entity -> json
                        string s = JsonHelper.ToJsonString(response);
                        //json -> byte[]
                        var msg = System.Text.UTF8Encoding.UTF8.GetBytes(s);

                        if (msg != null && msg.Length > 0)
                        {
                            returnInfo.Message = msg;
                        }
                    }
                }
            }
            else
            {
                LoggerManager.Logger.Warn(string.Format("Converted register to entity is null,string:{0}", str));
            }
            return returnInfo;
        }
    }

}
