/////Copyright (C) Gsafety 2014 .All Rights Reserved.
/////======================================================================
/////                   Guid: 7a711926-2899-4689-ac7f-52038d1d43df      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: PC-ZHANGY
/////                 Author: GJSY(zhangy)
/////======================================================================
/////           Project Name: Gsafety.PTMS.CommandManagement.Management
/////    Project Description:    
/////             Class Name: BasicInfoManager
/////          Class Version: v1.0.0.0
/////            Create Time: 2014/10/31 10:52:53
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2014/10/31 10:52:53
/////            Modified by:
/////   Modified Description: 
/////======================================================================
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gsafety.PTMS.Command.Repository;
using Gsafety.PTMS.Message.Contract.Data;
using Gsafety.PTMS.BaseInformation.Contract.Data;
using Gsafety.Common.Logging;
using Gsafety.PTMS.Analysis.Helper;
using Gsafety.PTMS.Base.Contract.Data;
using System.Threading;
using Gsafety.PTMS.DBEntity;

namespace Gsafety.PTMS.CommandManagement
{
    public class BasicInfoManager
    {
        static BasicCommandRepository gpsCommandRepository;
        public static List<SuiteStatusInfo> lstSuiteStatusInfo;
        public static List<TaskHelper> lstTaskHelper;
        public static List<SendingCommand> lstWaitUpdateCommand;
        public static List<RuleHelper> lstSuccessRuleID;
        static List<string> lstFinishId;
        public static object lockobj = new object();

        public static void Init(PTMSEntities context)
        {
            gpsCommandRepository = new BasicCommandRepository();
            lstSuiteStatusInfo = SuiteStatusInfoManage.GetAllSuite();
            lstWaitUpdateCommand = new List<SendingCommand>();
            lstTaskHelper = new List<TaskHelper>();
            lstFinishId = new List<string>();
            lstSuccessRuleID = new List<RuleHelper>();
            Task.Factory.StartNew(() => { WaitTaskFinish(context); });
            Task.Factory.StartNew(() => { SaveRuleStatus(context); });
        }

        protected static bool JudgeRepeatCommand(string mdvrID, string ruleID, Dictionary<string, string> dicRuleResultInfo)
        {
            if (dicRuleResultInfo != null && dicRuleResultInfo.Count > 0)
            {
                if (dicRuleResultInfo.Keys.Contains(mdvrID))
                {
                    string oldRuleID = dicRuleResultInfo[mdvrID];
                    if (oldRuleID.Equals(ruleID))
                        return true;
                    else
                        return false;
                }
                else
                {
                    return false;
                }
            }
            else
                return false;
        }

        protected static void UpdateStatusTimeout(PTMSEntities context, string recordID)
        {
            lock (lockobj)
            {
                gpsCommandRepository.UpdateStatusTimeout(context, recordID);
            }
        }





        protected static RuleHelper ToRuleHelperEntity(string mdvrID, string ruleID, IUpDateRuleStatus iUpDateRuleStatus)
        {
            RuleHelper ruleHelper = new RuleHelper();
            ruleHelper.MdvrID = mdvrID;
            ruleHelper.RuleID = ruleID;
            ruleHelper.iUpDateRuleStatus = iUpDateRuleStatus;
            return ruleHelper;
        }

        protected static List<string> GetLstMdvrs(PTMSEntities context, List<SelectInfoModel> Value)
        {
            List<string> lstMdvrs = new List<string>();
            if (Value != null && Value.Count > 0)
            {
                foreach (SelectInfoModel selectInfoModel in Value)
                {
                    List<string> templst = new List<string>();
                    if (selectInfoModel.Type == SettingType.CityCode || selectInfoModel.Type == SettingType.ProvinceCode)
                    {
                        templst = GetProvinceAndCity(selectInfoModel.Code, selectInfoModel.VehicleType, selectInfoModel.Type);
                    }
                    else if (selectInfoModel.Type == SettingType.VehicleType)
                    {
                        templst = GetVehicleType(selectInfoModel);
                    }
                    else if (selectInfoModel.Type == SettingType.Group)
                    {
                        templst = GetGroupVehicle(context,selectInfoModel.GroupID);
                    }
                    else if (selectInfoModel.Type == SettingType.Vehicle)
                    {
                        templst.Add(selectInfoModel.Code);
                    }
                    if (templst != null)
                        lstMdvrs.AddRange(templst);
                }
            }
            return lstMdvrs;
        }

        //获得key_value mdvr_fenceid()
        protected static List<string> GetLstMdvrs(string ruleID, List<SelectInfoModel> Value, Dictionary<string, string> dicRuleResultInfo)
        {

            List<string> existMdvrID = new List<string>();
            List<string> lstMdvrID = new List<string>();
            foreach (KeyValuePair<string, string> item in dicRuleResultInfo)
            {
                if (item.Value.Equals(ruleID))
                    lstMdvrID.Add(item.Key);
            }
            if (lstMdvrID.Count > 0)
            {
                if (Value != null && Value.Count > 0)
                {
                    foreach (SelectInfoModel mdvrID in Value)
                    {
                        if (lstMdvrID.Contains(mdvrID.Code))
                        {
                            existMdvrID.Add(mdvrID.Code);
                        }
                    }
                }
                else
                    existMdvrID = lstMdvrID;
            }
            return existMdvrID;
        }

        protected static List<string> GetLstMdvrs(string ruleID, List<SelectInfoModel> Value, Dictionary<string, List<string>> dicExistMdvrID)
        {
            List<string> existMdvrID = new List<string>();
            List<string> lstMdvrID = new List<string>();
            if (dicExistMdvrID != null && dicExistMdvrID.Count > 0)
            {
                foreach (KeyValuePair<string, List<string>> item in dicExistMdvrID)
                {
                    string mdvrID = item.Value.FirstOrDefault(c => c.StartsWith(ruleID));
                    if (!string.IsNullOrEmpty(mdvrID))
                        lstMdvrID.Add(item.Key);
                }
                if (lstMdvrID.Count > 0)
                {
                    if (Value != null && Value.Count > 0)
                    {
                        foreach (SelectInfoModel mdvrID in Value)
                        {
                            if (lstMdvrID.Contains(mdvrID.Code))
                            {
                                existMdvrID.Add(mdvrID.Code);
                            }
                        }
                    }
                    else
                        existMdvrID = lstMdvrID;
                }
            }
            return existMdvrID;
        }

        protected static List<string> GetFenceList(string fencID, string deviceID, Dictionary<string, List<string>> dicfenceID)
        {
            List<string> lstMdvrID = new List<string>();
            if (dicfenceID != null && dicfenceID.Count > 0)
            {
                foreach (KeyValuePair<string, List<string>> item in dicfenceID)
                {
                    if (fencID == item.Key.Trim())
                    {
                        if (deviceID != null)
                        {
                            foreach (var x in item.Value)
                                if (x == deviceID)
                                    lstMdvrID.Add(x);
                        }
                        else
                        {
                            foreach (var x in item.Value)
                                lstMdvrID.Add(x);

                        }
                    }
                }
            }
            return lstMdvrID;
        }

        protected static void AddRuleResultInfo(string mdvrID, string ruleID, Dictionary<string, string> dicRuleResultInfo)
        {
            if (dicRuleResultInfo != null && dicRuleResultInfo.Count > 0)
            {
                if (dicRuleResultInfo.Keys.Contains(mdvrID))
                {
                    dicRuleResultInfo[mdvrID] = ruleID;
                }
                else
                {
                    dicRuleResultInfo.Add(mdvrID, ruleID);
                }
            }
            else
            {
                dicRuleResultInfo.Add(mdvrID, ruleID);
            }
        }

        protected static void RemoveRuleResultInfo(string mdvrID, Dictionary<string, string> dicRuleResultInfo)
        {
            if (dicRuleResultInfo != null && dicRuleResultInfo.Count > 0)
            {
                if (dicRuleResultInfo.ContainsKey(mdvrID))
                {
                    dicRuleResultInfo.Remove(mdvrID);
                }
            }
        }

        protected static string GetType(int status)
        {
            string operationType = string.Empty;
            switch (status)
            {
                case 10:
                    operationType = "add";
                    break;
                case 20:
                    operationType = "edit";
                    break;
            }
            return operationType;
        }

        private static List<string> GetAllSuiteInfo()
        {
            List<string> lstVehicle = new List<string>();
            lstVehicle = lstSuiteStatusInfo.Select(c => c.MdvrCoreId).ToList();
            return lstVehicle;
        }

        private static List<string> GetProvinceAndCity(string code, List<int> vehicleType, SettingType settingType)
        {
            List<string> lstVehicle = new List<string>();
            List<SuiteStatusInfo> tempSuiteStatusInfo = new List<SuiteStatusInfo>();
            if (settingType == SettingType.ProvinceCode)
            {
                tempSuiteStatusInfo = lstSuiteStatusInfo.FindAll(c => c.DistrictCode.Substring(0, 2).Equals(code) && c.Status != 22);
            }
            else
            {
                tempSuiteStatusInfo = lstSuiteStatusInfo.FindAll(c => c.DistrictCode.Equals(code) && c.Status != 22);
            }
            List<string> tempList = new List<string>();
            if (vehicleType != null && vehicleType.Count > 0)
            {
                foreach (int i in vehicleType)
                {
                    tempList = tempSuiteStatusInfo.FindAll(c => c.VehicleType.Equals(i)).Select(c => c.MdvrCoreId).ToList();
                    if (tempList != null)
                        lstVehicle.AddRange(tempList);
                }
            }
            else
            {
                tempList = tempSuiteStatusInfo.Select(c => c.MdvrCoreId).ToList();
                if (tempList != null)
                    lstVehicle.AddRange(tempList);
            }
            return lstVehicle;
        }

        private static List<string> GetVehicleType(SelectInfoModel selectInfoModel)
        {
            List<string> lstVehicle = new List<string>();
            List<SuiteStatusInfo> tempSuiteInfo = lstSuiteStatusInfo.FindAll(c => c.DistrictCode.Equals(selectInfoModel.Code));
            if (tempSuiteInfo != null && tempSuiteInfo.Count > 0)
            {
                foreach (int i in selectInfoModel.VehicleType)
                {
                    List<string> tempList = tempSuiteInfo.FindAll(c => c.VehicleType.Equals(i)).Select(c => c.MdvrCoreId).ToList();
                    if (tempList != null)
                        lstVehicle.AddRange(tempList);
                }
            }
            return lstVehicle;
        }

        private static List<string> GetGroupVehicle(PTMSEntities context, string GroupID)
        {
            List<string> lstVehicle = new List<string>();
            lstVehicle = gpsCommandRepository.GetGroupWorkingSuiteInfo(context, GroupID);
            return lstVehicle;
        }

        private static void WaitTaskFinish(PTMSEntities context)
        {
            List<TaskHelper> _lstTaskHelper;
            while (true)
            {
                lock (lockobj)
                {
                    _lstTaskHelper = lstTaskHelper.FindAll(c => c.TaskMethod.IsCompleted.Equals(true));
                    if (_lstTaskHelper != null && _lstTaskHelper.Count > 0)
                    {
                        foreach (TaskHelper taskHelper in _lstTaskHelper)
                        {
                            if (!lstFinishId.Contains(taskHelper.TaskId))
                                lstFinishId.Add(taskHelper.TaskId);
                            lstTaskHelper.Remove(taskHelper);
                        }
                    }
                    Thread.Sleep(1000);
                    if (lstWaitUpdateCommand != null && lstWaitUpdateCommand.Count > 0)
                    {
                        List<SendingCommand> lstSendingCommand;
                        if (lstFinishId != null && lstFinishId.Count > 0)
                        {
                            foreach (string finishId in lstFinishId)
                            {
                                lstSendingCommand = lstWaitUpdateCommand.FindAll(c => c.OperationID.Equals(finishId));
                                if (lstSendingCommand != null && lstSendingCommand.Count > 0)
                                {
                                    foreach (SendingCommand sendCommand in lstSendingCommand)
                                    {
                                        gpsCommandRepository.UpdateCommandSendStatus(context, sendCommand.RecordID, DateTime.Now, sendCommand.commandSendStatus);
                                        if (sendCommand.CommandType == CommandType.OverSpeed && sendCommand.commandSendStatus == CommandSendStatus.Success)
                                        {
                                            gpsCommandRepository.AddSpeedSuccessResult(context, sendCommand);
                                        }
                                        else if (sendCommand.CommandType == CommandType.C80 && sendCommand.commandSendStatus == CommandSendStatus.Success)
                                        {
                                            gpsCommandRepository.ProcessSettingOneKeyAlarmResult(context, sendCommand);
                                        }
                                        else if (sendCommand.CommandType == CommandType.C30 && sendCommand.commandSendStatus == CommandSendStatus.Success)
                                        {
                                            gpsCommandRepository.ProcessSettingGpsSendUpResult(context, sendCommand);
                                        }
                                        else if (sendCommand.CommandType == CommandType.C82 && sendCommand.commandSendStatus == CommandSendStatus.Success)
                                        {
                                            gpsCommandRepository.ProcessSettingAbnormalDoorUpResult(context, sendCommand);
                                        }
                                        else if (sendCommand.CommandType == CommandType.C64 && sendCommand.commandSendStatus == CommandSendStatus.Success)
                                        {
                                            gpsCommandRepository.ProcessSettingTemperatureUpResult(context, sendCommand);
                                        }
                                        lstWaitUpdateCommand.Remove(sendCommand);
                                        //SendingManager.RemoveSendCommmand(sendCommand);
                                    }
                                }
                            }
                        }
                    }
                    Thread.Sleep(1000);
                }
            }
        }

        private static void SaveRuleStatus(PTMSEntities context)
        {
            while (true)
            {
                lock (lockobj)
                {
                    try
                    {
                        if (lstSuccessRuleID.Count > 0)
                        {
                            foreach (RuleHelper item in lstSuccessRuleID)
                            {
                                item.iUpDateRuleStatus.UpdateRuleStatus(context, item.MdvrID, item.RuleID);
                            }
                            lstSuccessRuleID.Clear();
                        }
                        Thread.Sleep(2000);
                    }
                    catch (Exception ex)
                    {
                        LoggerManager.Logger.Info(ex.Message);
                    }
                }
            }
        }
    }

    public class TaskHelper
    {
        public string TaskId { get; set; }

        public Task TaskMethod { get; set; }
    }

    public class RuleHelper
    {
        public string MdvrID { get; set; }

        public string RuleID { get; set; }

        public IUpDateRuleStatus iUpDateRuleStatus;
    }
}
