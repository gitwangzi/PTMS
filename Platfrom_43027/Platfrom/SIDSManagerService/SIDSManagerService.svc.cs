using Gsafety.PTMS.BaseInformation.Repository;
using Gsafety.PTMS.DBEntity;
using Gsafety.PTMS.SIDS.Contract;
using Gsafety.PTMS.SIDS.Contract.Data;
using Gsafety.PTMS.SIDS.Data;
//using Gsafety.PTMS.SIDS.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace Gsafety.PTMS.SIDS.Service
{
    // 注意: 使用“重构”菜单上的“重命名”命令，可以同时更改代码、svc 和配置文件中的类名“SIDSManagerService”。
    // 注意: 为了启动 WCF 测试客户端以测试此服务，请在解决方案资源管理器中选择 SIDSManagerService.svc 或 SIDSManagerService.svc.cs，然后开始调试。
    public class SIDSManagerService : ISIDSManageService
    {
        private VehicleRepository _helper;

        public SIDSManagerService()
        {
            this._helper = new VehicleRepository();
        }

        //1、所有在线车辆位置
        public IList<VehicleModel> GetAllOnLineVehicleMes(string strControlCenter)
        {

            using (PTMSEntities context = new PTMSEntities())
            {
                //return VehicleRepository.GetAllOnLineVehicleMes(context, strControlCenter);
                return null;
            }

        }
        //2、报警车辆位置
        public IList<VehicleModel> GetAlarmVehicleMes(string strControlCenter)
        {

            using (PTMSEntities context = new PTMSEntities())
            {
                //return _helper.GetAlarmVehicleMes(context, strControlCenter);
                return null;
            }

        }
        //3、离线车辆位置
        public IList<VehicleModel> GetOutLineVehicleMes(string strControlCenter)
        {

            using (PTMSEntities context = new PTMSEntities())
            {
                //return _helper.GetOutLineVehicleMes(context, strControlCenter);
                return null;
            }

        }
        //4、车辆在线情况
        public Dictionary<string, int> GetAllVehicleMessage(string strControlCenter)
        {

            using (PTMSEntities context = new PTMSEntities())
            {
                //return _helper.GetAllVehicleMessage(context, strControlCenter);
                return null;
            }

        }
        //5、一键报警信息列表
        public IList<AlarmModel> GetTop10Alarm(string strControlCenter)
        {

            using (PTMSEntities context = new PTMSEntities())
            {
                //return _helper.GetTop10Alarm(context, strControlCenter);
                return null;
            }

        }
        //6、一键报警统计(12个月) 
        public Dictionary<string, int> StatisticsAlarmCountByMonth(string strControlCenter, string StartTime, string EndTime)
        {

            using (PTMSEntities context = new PTMSEntities())
            {
                //return _helper.StatisticsAlarmCountByMonth(context, strControlCenter, StartTime, EndTime);
                return null;
            }

        }
        //7、车辆未上线情况统计（过去7天）
        public Dictionary<string, int> StatisticsOutLineVehicleCount(string strControlCenter, string StartTime, string EndTime)
        {

            using (PTMSEntities context = new PTMSEntities())
            {
                //return _helper.StatisticsOutLineVehicleCount(context, strControlCenter, StartTime, EndTime);
                return null;
            }

        }
        //8、报警车辆态势展示
        public IList<VehicleModel> GetAlarmVehicleLocation(string strControlCenter, string WarningMark, string WarningTime, string HandleTime)
        {

            using (PTMSEntities context = new PTMSEntities())
            {
                //return _helper.GetAlarmVehicleLocation(context, strControlCenter, WarningMark, WarningTime, HandleTime);
                return null;
            }

        }
        //9、车辆信息展示
        public PerVehicleModel GetPerVehicleMes(string strControlCenter, string VehicleSn)
        {

            using (PTMSEntities context = new PTMSEntities())
            {
                //return _helper.GetPerVehicleMes(context, strControlCenter, VehicleSn);
                return null;
            }

        }
        //10、安全套件状态
        public MDVR_Model GetMDVRMessage(string strControlCenter, string VehicleSn)
        {

            using (PTMSEntities context = new PTMSEntities())
            {
                //return _helper.GetMDVRMessage(context, strControlCenter, VehicleSn);
                return null;
            }

        }
        //11、历史在线时长统计
        public Dictionary<string, decimal> GetOnLineTime(string strContralCenter, string BusID, string StartTime, string EndTime)
        {

            using (PTMSEntities context = new PTMSEntities())
            {
                //return _helper.GetOnLineTime(context, strContralCenter, BusID, StartTime, EndTime);
                return null;
            }

        }

        //附加1  公交车辆列表
        public IList<Vehicle_MDVR_Model> GetVehicle_MDVR(string strControlCenter)
        {

            using (PTMSEntities context = new PTMSEntities())
            {
                //return _helper.GetVehicle_MDVR(context, strControlCenter);
                return null;
            }

        }
        //附加2  获取历史视频文件
        public IList<Vehicle_Video_Model> GetVehicle_Video(string VehicleSn)
        {

            using (PTMSEntities context = new PTMSEntities())
            {
                //return _helper.GetVehicle_Video(context, VehicleSn);
                return null;
            }

        }
    }
}
