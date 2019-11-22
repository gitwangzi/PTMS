using System.Runtime.CompilerServices;
using Gsafety.PTMS.Traffic.Contract;
using Gsafety.PTMS.DBEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gsafety.PTMS.Message.Contract.Data;
using System.Transactions;
using Gsafety.PTMS.Traffic.Contract.Data;
using Gsafety.PTMS.Base.Contract.Data;
using System.Linq.Expressions;
using Gsafety.PTMS.BaseInfo.Conditions;
using Gsafety.PTMS.BaseInfo.MakerContions.Items;
using Gsafety.PTMS.BaseInfo.Conditions.QueryFiler;
using Gsafety.PTMS.BaseInfo.Conditons.QueryFiler;
using Gsafety.PTMS.Common.Data;
using Gsafety.PTMS.Common.Enum;

namespace Gsafety.PTMS.Traffic.Repository
{
    public class TrafficRepository
    {
        public TrafficRepository()
        {

        }

        ///10Adding（The device is not exist）
        ///11Add Success(The device is exist in the fence)
        ///12Add failure(The device is not exist in the fence)
        ///20modfity(The device is exist，But the device is not update)
        ///21Modfity success(The device is exist，The device into is new）
        ///22Modftiy failure(The device is exist，The device into is not new)
        ///30Deleteing
        ///31Delete Success
        ///32Delete Failure
        public bool BeExistFenceName(PTMSEntities _context, string strName, short nType)
        {
            if (strName == null)
                return false;

            //+ zhoudd 2015-02-11   && x.VALID == 1
            //var result =
            //    _context.TRAFFICFENCE_VIEW.Any(
            //        x =>
            //            x.NAME.Trim().ToLower() == strName.Trim().ToLower() && x.FENCE_TYPE == nType && x.VALID == 1);
            //return result;
            return false;
        }

        public List<Gsafety.PTMS.Traffic.Contract.Vehicle> GetVehicleByFence(PTMSEntities _context, string fenceID, short nStatus)
        {

            var result = from x in _context.TRF_FENCE_QUEUE
                         where x.FENCE_ID.Trim() == fenceID.Trim()
                         where (nStatus == 1) ? true : x.STATUS == nStatus
                         select new Gsafety.PTMS.Traffic.Contract.Vehicle
                                {
                                    Vehicle_ID = x.VEHICLE_ID,
                                    MDVR_CODE_SN = x.MDVR_CORE_SN
                                };
            return result.ToList();

        }


        public List<string> GetFenceIDByCarID(PTMSEntities _context, string strCarID, short nStatus)
        {
            var result = from x in _context.TRF_FENCE_QUEUE
                         where
                             (strCarID == null) ? true : x.VEHICLE_ID.ToLower().Trim() == strCarID.Trim().ToLower()
                         where (nStatus == 1) ? true : x.STATUS == nStatus
                         select x.FENCE_ID;
            return result.ToList();

        }


        public bool DeleteVehicleFenceByFenceID(PTMSEntities _context, string strFencid)
        {
            List<TRF_FENCE_QUEUE> list;

            var result = from x in _context.TRF_FENCE_QUEUE
                         where x.FENCE_ID.Trim() == strFencid.Trim()
                         select x;
            list = result.ToList();
            if (list == null || list.Count == 0)
                return false;
            for (int i = 0; i < list.Count; i++)
            {
                result = from x in _context.TRF_FENCE_QUEUE
                         where x.ID == list[i].ID
                         select x;
                TRF_FENCE_QUEUE vf = result.FirstOrDefault();
                _context.TRF_FENCE_QUEUE.Remove(vf);
            }
            int nResult = _context.SaveChanges();
            if (nResult > 0)
            {
                return true;
            }
            else
            {
                return false;
            }

        }

        //public int UpdateElectricFenceResult(PTMSEntities context, ElectricFenceReply item)
        public int UpdateElectricFenceResult(PTMSEntities context, GenenalResponseEx item)
        {
            TransactionOptions optons = new TransactionOptions();
            optons.IsolationLevel = IsolationLevel.ReadUncommitted;
            int Index = 0;
            var scope = new TransactionScope(TransactionScopeOption.Required, optons);


            try
            {
                //var fenceSetup = (from e in context.ELECTRIC_FENCE_SETUP
                //                  where e.MDVR_CORE_SN == item.MdvrCoreId
                //                  && e.SEND_TIME == item.OriginalTime
                //                  select e).FirstOrDefault();

                //if (fenceSetup != null)
                //{
                //    fenceSetup.RESULT = (short)item.ReplyResult;  ////更改电子围栏流水记录结果
                //    fenceSetup.RESULT_PACKET = item.Context;////返回的内容

                var vehiclefence = (from e in context.TRF_FENCE_QUEUE
                                    where e.MDVR_CORE_SN == item.MdvrCoreId
                                        //&& e.FENCE_ID == fenceSetup.FENCE_ID
                                    && e.PACKET_SEQ == item.ResponseSerialNo
                                    select e).FirstOrDefault();

                if (vehiclefence != null)
                {
                    vehiclefence.CREATE_TIME = DateTime.Now;
                    ////回复成功 
                    if (item.Result == (int)GenenalResponseResult.Success)
                    {
                        ////1：新增电子围栏，2：修改电子围栏，3：删除电子围栏
                        if (item.OperationType == (int)RuleOperationType.Add)
                        {
                            ////新增电子围栏成功
                            vehiclefence.STATUS = 11;
                        }
                        else if (item.OperationType == (int)RuleOperationType.Update)
                        {
                            ////修改电子围栏成功
                            vehiclefence.STATUS = 21;
                        }
                        else if (item.OperationType == (int)RuleOperationType.Delete)
                        {
                            ////删除成功
                            context.TRF_FENCE_QUEUE.Remove(vehiclefence);
                        }
                    }
                    ////回复失败
                    else
                    {
                        ////1：新增电子围栏，2：修改电子围栏，3：删除电子围栏
                        if (item.OperationType == (int)RuleOperationType.Add)
                        {
                            ////新增电子围栏失败

                            //if (RepeatAdd(item) == "00030012")
                            //{
                            //    vehiclefence.STATUS = 11;
                            //}
                            //else
                            //{
                            vehiclefence.STATUS = 12;
                            //}
                        }
                        else if (item.OperationType == (int)RuleOperationType.Delete)
                        {
                            ////修改电子围栏失败
                            vehiclefence.STATUS = 22;
                        }
                        else if (item.OperationType == (int)RuleOperationType.Delete)
                        {
                            ////删除电子围栏失败 ，设备围栏还在
                            vehiclefence.STATUS = 32;
                        }
                    }
                }
                //}

                Index = context.SaveChanges();
                scope.Complete();


                return Index;

            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                scope.Dispose();
            }
        }

        public string RepeatAdd(ElectricFenceReply item)
        {
            char a = ',';
            string[] t;
            string failedReasonConvert = string.Empty;
            string failedReason = string.Empty;
            t = item.Context.Split(a);
            failedReasonConvert = t[t.Length - 2];
            return failedReasonConvert;
        }


        public int UpdateRouteInfoResult(PTMSEntities context, GenenalResponseEx item)
        {
            TransactionOptions optons = new TransactionOptions();
            optons.IsolationLevel = IsolationLevel.ReadUncommitted;
            int Index = 0;
            var scope = new TransactionScope(TransactionScopeOption.Required, optons);


            try
            {
                var vehiclefence = (from e in context.TRF_ROUTE_QUEUE
                                    where e.MDVR_CORE_SN == item.MdvrCoreId
                                    && e.PACKET_SEQ == item.ResponseSerialNo
                                    select e).FirstOrDefault();

                if (vehiclefence != null)
                {
                    vehiclefence.CREATE_TIME = DateTime.Now;
                    ////回复成功 
                    if (item.Result == (int)GenenalResponseResult.Success)
                    {
                        ////1：新增电子围栏，2：修改电子围栏，3：删除电子围栏
                        if (item.OperationType == (int)RuleOperationType.Add)
                        {
                            ////新增电子围栏成功
                            vehiclefence.STATUS = 11;
                        }
                        else if (item.OperationType == (int)RuleOperationType.Update)
                        {
                            ////修改电子围栏成功
                            vehiclefence.STATUS = 21;
                        }
                        else if (item.OperationType == (int)RuleOperationType.Delete)
                        {
                            ////删除成功
                            context.TRF_ROUTE_QUEUE.Remove(vehiclefence);
                        }
                    }
                    ////回复失败
                    else
                    {
                        ////1：新增电子围栏，2：修改电子围栏，3：删除电子围栏
                        if (item.OperationType == (int)RuleOperationType.Add)
                        {
                            ////新增电子围栏失败

                            //if (RepeatAdd(item) == "00030012")
                            //{
                            //    vehiclefence.STATUS = 11;
                            //}
                            //else
                            //{
                            vehiclefence.STATUS = 12;
                            //}
                        }
                        else if (item.OperationType == (int)RuleOperationType.Delete)
                        {
                            ////修改电子围栏失败
                            vehiclefence.STATUS = 22;
                        }
                        else if (item.OperationType == (int)RuleOperationType.Delete)
                        {
                            ////删除电子围栏失败 ，设备围栏还在
                            vehiclefence.STATUS = 32;
                        }
                    }
                }
                //}

                Index = context.SaveChanges();
                scope.Complete();


                return Index;

            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                scope.Dispose();
            }
        }

        public bool BeExistSpeedLimitName(PTMSEntities _context, string strName)
        {
            //var result = _context.SPEED_LIMIT.Any(x => x.NAME == strName);
            //return result;
            return false;
        }

        public bool BeExistSpeedLimitidName(PTMSEntities _context, string strName, string id)
        {
            //var result = _context.SPEED_LIMIT.Any(x => x.NAME == strName && x.ID != id);
            //return result;
            return false;
        }

        public class FenceComparer : IEqualityComparer<CarFence>
        {
            public bool Equals(CarFence x, CarFence y)
            {
                if (x.CarNumber == y.CarNumber && x.FenceId == y.FenceId && x.MDVR_SN_CODE == y.MDVR_SN_CODE)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }

            public int GetHashCode(CarFence obj)
            {
                return obj.CarNumber.GetHashCode();
            }
        }
    }
}