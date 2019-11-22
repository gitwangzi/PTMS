using Gsafety.Common.Util;
using Gsafety.PTMS.Base.Contract.Data;
using Gsafety.PTMS.BaseInformation.Contract.Data;
using Gsafety.PTMS.Common.Data;
using Gsafety.PTMS.Common.Enum;
using Gsafety.PTMS.DBEntity;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Gsafety.PTMS.AnalysisLib.Command
{
    public class CommandBase
    {
        static bool run = true;

        protected string routekey = string.Empty;

        protected CommandTypeEnum CommandType;

        public virtual void Send(PTMSEntities context)
        {

        }

        public virtual void OnSend(PTMSEntities context)
        {

        }

        public virtual void OnReply(PTMSEntities context, byte[] bytes, string key)
        {

        }

        public virtual void OnSendTimeOut(PTMSEntities context, object data)
        {

        }

        public virtual void OnWaitTimeOut(PTMSEntities context, object data)
        {

        }

        public static void Stop()
        {
            run = false;

            Thread.Sleep(1000 * 60);
        }

        public static bool ShouldRun()
        {
            int min = DateTime.Now.Minute;
            int temp = min % ConfigInfo.TimeInterval;
            return temp == ConfigInfo.RunTime;
        }

        public static void RunJob()
        {
            while (run)
            {
                try
                {

                    if (ShouldRun())
                    {
                        using (PTMSEntities context = new PTMSEntities())
                        {
                            foreach (var item in CommandFactory.commands.Values)
                            {
                                if (ShouldRun())
                                {
                                    item.Send(context);
                                }
                            }
                        }
                    }

                }
                catch (Exception ex)
                {

                }

                Thread.Sleep(6000);
            }
        }
    }
}
