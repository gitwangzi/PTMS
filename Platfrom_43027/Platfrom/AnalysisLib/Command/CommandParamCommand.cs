using Gsafety.Common.Logging;
using Gsafety.Common.Util;
using Gsafety.MQ;
using Gsafety.PTMS.Analysis.Helper;
using Gsafety.PTMS.Base.Contract.Data;
using Gsafety.PTMS.BaseInformation.Contract.Data;
using Gsafety.PTMS.Common.Data;
using Gsafety.PTMS.Common.Data.Enum;
using Gsafety.PTMS.Common.Enum;
using Gsafety.PTMS.DBEntity;
using Gsafety.PTMS.Manager.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gsafety.PTMS.AnalysisLib.Command
{
    public class CommandParamCommand : CommandBase
    {
        public CommandParamCommand()
        {
            routekey = SettingRoute.OriginalHeartBeatKey;
            this.CommandType = CommandTypeEnum.CommandParam;
        }

        public override void Send(PTMSEntities context)
        {
            var sending = from hv in context.TRF_COMMAND_VEHICLE
                          where hv.STATUS == (short)CommandStateEnum.Delivering
                          select hv.ID;
            List<string> sendinglist = sending.ToList();

            var hvs = from hv in context.TRF_COMMAND_VEHICLE
                      join w in context.RUN_SUITE_WORKING on hv.VEHICLE_ID equals w.VEHICLE_ID
                      where (hv.STATUS == (short)CommandStateEnum.WaitForDeliver || hv.STATUS == (short)CommandStateEnum.Failed) && w.ONLINE_FLAG == 1
                      group hv by hv.VEHICLE_ID into grp
                      let createtime = grp.Min(n => n.CREATE_TIME)
                      from row in grp
                      where row.CREATE_TIME == createtime
                      select row;


            List<TRF_COMMAND_VEHICLE> tosendlist = hvs.ToList();
            List<string> ids = tosendlist.Select(n => n.COMMAND_PARAM_ID).ToList();
            List<TRF_COMMAND_PARAM> commandparams = context.TRF_COMMAND_PARAM.Where(n => ids.Contains(n.ID)).ToList();

            foreach (var item in tosendlist)
            {
                try
                {
                    if (CommandBase.ShouldRun())
                    {
                        TRF_COMMAND_PARAM command = commandparams.FirstOrDefault(n => n.ID == item.COMMAND_PARAM_ID);
                        if (command != null)
                        {
                            SetTermParam param = new SetTermParam();
                            param.ParamList = new List<ParamInfo>();
                            param.UID = item.MDVR_CORE_SN;

                            CommandParaEnum commandtype = (CommandParaEnum)item.TYPE;
                            ParamInfo pi = new ParamInfo();
                            switch (commandtype)
                            {
                                case CommandParaEnum.HeartBeat:
                                    pi.ParaId = 1;
                                    pi.ParaValue = command.INTERVAL.ToString();
                                    pi.ParaLen = 4;
                                    param.ParamList.Add(pi);
                                    param.ParamCount = 1;
                                    break;
                                case CommandParaEnum.ReportStrategy:
                                    pi.ParaId = 0x0020;
                                    int stragety = command.REPORT_STRATEGY.Value;
                                    pi.ParaValue = stragety.ToString();
                                    pi.ParaLen = 4;
                                    param.ParamList.Add(pi);
                                    if (stragety == (int)ReportStrategyEnum.ByInterval)
                                    {
                                        param.ParamCount = 2;
                                        pi = new ParamInfo();
                                        pi.ParaId = 0x0029;
                                        pi.ParaValue = command.LOCATION_INTERVAL.ToString();
                                        pi.ParaLen = 4;
                                        param.ParamList.Add(pi);
                                    }
                                    else if (stragety == (int)ReportStrategyEnum.ByLength)
                                    {
                                        param.ParamCount = 2;
                                        pi = new ParamInfo();
                                        pi.ParaId = 0x002C;
                                        pi.ParaValue = command.LOCATION_LENGTH.ToString();
                                        pi.ParaLen = 4;
                                        param.ParamList.Add(pi);
                                    }
                                    else if (stragety == (int)ReportStrategyEnum.ByIntervalAndLength)
                                    {
                                        param.ParamCount = 3;
                                        pi = new ParamInfo();
                                        pi.ParaId = 0x0029;
                                        pi.ParaValue = command.LOCATION_INTERVAL.ToString();
                                        pi.ParaLen = 4;
                                        param.ParamList.Add(pi);

                                        pi = new ParamInfo();
                                        pi.ParaId = 0x002C;
                                        pi.ParaValue = command.LOCATION_LENGTH.ToString();
                                        pi.ParaLen = 4;
                                        param.ParamList.Add(pi);
                                    }
                                    break;
                                case CommandParaEnum.LED:
                                    pi.ParaId = 0x0071;
                                    pi.ParaValue = command.BRIGHTNESS.ToString();
                                    pi.ParaLen = 4;
                                    param.ParamList.Add(pi);

                                    pi = new ParamInfo();
                                    pi.ParaId = 0x0072;
                                    pi.ParaValue = command.CONTRAST.ToString();
                                    pi.ParaLen = 4;
                                    param.ParamList.Add(pi);

                                    param.ParamCount = 2;
                                    break;
                                case CommandParaEnum.Speed:
                                    pi.ParaId = 0x0055;
                                    pi.ParaValue = command.MAX_SPEED.ToString();
                                    pi.ParaLen = 4;
                                    param.ParamList.Add(pi);

                                    pi = new ParamInfo();
                                    pi.ParaId = 0x0056;
                                    pi.ParaValue = command.DURATION.ToString();
                                    pi.ParaLen = 4;
                                    param.ParamList.Add(pi);
                                    param.ParamCount = 2;
                                    break;
                                default:
                                    break;
                            }



                            string json = ConvertHelper.ConvertModelToJson(param);
                            byte[] sendbytes = System.Text.UnicodeEncoding.UTF8.GetBytes(json);

                            TransforMessage.PublishMessage(Constdefine.MDVREXCHANGE, "MDVR.SetTermParam." + param.UID, sendbytes);

                            item.PACKET_SEQ = param.SerialNo;
                            item.SEND_TIME = DateTime.Now.ToUniversalTime();
                            item.STATUS = (short)CommandStateEnum.Delivering;

                            context.SaveChanges();
                        }
                    }
                }
                catch (Exception ex)
                {
                    LoggerManager.Logger.Error(ex);
                }
            }

            var templist = from hv in context.TRF_COMMAND_VEHICLE
                           where hv.STATUS == (short)CommandStateEnum.Delivering && sendinglist.Contains(hv.ID)
                           select hv;

            foreach (var item in templist.ToList())
            {
                item.STATUS = (short)CommandStateEnum.Failed;
            }

            context.SaveChanges();
        }

        public override void OnReply(PTMSEntities context, byte[] bytes, string key)
        {
            string str = System.Text.UnicodeEncoding.UTF8.GetString(bytes);
            GenenalResponseEx response = JsonHelper.FromJsonString<GenenalResponseEx>(str);
            TRF_COMMAND_VEHICLE record = context.TRF_COMMAND_VEHICLE.Where(n => n.PACKET_SEQ == response.ResponseSerialNo && n.MDVR_CORE_SN == response.UID).OrderBy(n => n.CREATE_TIME).FirstOrDefault();
            if (record != null)
            {
                if (response.Result == 0)
                {
                    //update the record status
                    record.STATUS = (short)CommandStateEnum.Succeed;

                    List<TRF_COMMAND_VEHICLE> commands = context.TRF_COMMAND_VEHICLE.Where(n => n.MDVR_CORE_SN == response.UID && n.TYPE == record.TYPE && n.ID != record.ID && n.STATUS == (int)CommandStateEnum.Succeed).ToList();
                    foreach (var item in commands)
                    {
                        context.TRF_COMMAND_VEHICLE.Remove(item);
                    }
                }
                else
                {
                    //update the record status
                    record.STATUS = (short)CommandStateEnum.Failed;
                }
                //update the heartbeatrecord             

                context.SaveChanges();
            }
        }

    }
}
