using Gsafety.PTMS.Traffic.Contract.Data;
using System;
using System.Collections.Generic;
namespace Gsafety.PTMS.MonitorAlert
{
    public class MonitorPlan
    {
        /// <summary>
        /// 行驶计划结果表ID
        /// </summary>
        public string _planGuid;
        /// <summary>
        /// 行驶计划ID
        /// </summary>
        public string _planId;
        /// <summary>
        /// 行驶计划名称
        /// </summary>
        public string _planName;
        /// <summary>
        /// 开始执行告警Id
        /// </summary>
        public string _start_alert_id;
        /// <summary>
        /// 行驶计划开始时间
        /// </summary>
        public DateTime _startTime;
        /// <summary>
        /// 行驶计划结束时间
        /// </summary>
        public DateTime _endTime;
        /// <summary>
        /// 行驶计划实际开始时间
        /// </summary>
        public DateTime _actstartTime;
        /// <summary>
        /// 行驶计划实际结束时间
        /// </summary>
        public DateTime _actendTime;
        /// <summary>
        /// 行驶计划时间偏差
        /// </summary>
        public double _toleranceTime;
        /// <summary>
        /// 行驶计划控制点半径
        /// </summary>
        public double _radius;
        /// <summary>
        /// 行驶计划是否执行
        /// </summary>
        public Boolean _isValid;     
        /// <summary>
        /// 行驶计划控制点列表
        /// </summary>
        public List<GPSStopScheDulePoint> _PlanPointList;

        /// <summary>
        /// 行驶计划关联车辆
        /// </summary>
        public string _vehicleID;       
        /// <summary>
        /// 关联车辆Raptor号
        /// </summary>
        public string _antGpsID;
        /// <summary>
        /// 执行计划的状态
        /// </summary>
        public enum EXEStatus
        {
            /// <summary>
            /// 其余状态
            /// </summary>
            Initial=0,
            /// <summary>
            /// 开始执行
            /// </summary>
            Start = 1,
            /// <summary>
            /// 执行结束
            /// </summary>
            Finish = 2,
        };
    }
  
}