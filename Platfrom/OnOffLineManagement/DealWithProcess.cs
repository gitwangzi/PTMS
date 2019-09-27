using Gsafety.Common.Logging;
using Gsafety.PTMS.Analysis.Helper;
using Gsafety.PTMS.Analysis.MonitorTreatment;
using ServiceStack.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnOffLineManagement
{
    public class DealWithProcess
    {
        #region OnOffLine

        /// <summary>
        /// Online
        /// </summary>
        /// <param name="bytes"></param>
        /// <param name="key"></param>
        [DealWith(typeName: MonitorRoute.OriginalOnlineKey)]
        public void ProcessOnline(byte[] bytes)
        {
            try
            {
                VehicleOnOffTimeTreatment.StatOnOffTime(bytes);
            }
            catch (Exception ex)
            {
                LoggerManager.Logger.Error(ex);
            }
        }

        /// <summary>
        /// OffLine A1
        /// </summary>
        /// <param name="bytes"></param>
        /// <param name="key"></param>
        [DealWith(typeName: MonitorRoute.OriginalOfflineA1Key)]
        public void ProcessOfflineA1(byte[] bytes)
        {
            try
            {
                VehicleOnOffTimeTreatment.StatOnOffTime(bytes);
            }
            catch (Exception ex)
            {
                LoggerManager.Logger.Error(ex);
            }
        }

        /// <summary>
        /// OffLine A2
        /// </summary>
        /// <param name="bytes"></param>
        /// <param name="key"></param>
        [DealWith(typeName: MonitorRoute.OriginalOfflineA2Key)]
        public void ProcessOfflineA2(byte[] bytes)
        {
            try
            {
                VehicleOnOffTimeTreatment.StatOnOffTime(bytes);
            }
            catch (Exception ex)
            {
                LoggerManager.Logger.Error(ex);
            }
        }

        /// <summary>
        /// Close V20
        /// </summary>
        /// <param name="bytes"></param>
        /// <param name="key"></param>
        [DealWith(typeName: MonitorRoute.OriginalShutDowmKey)]
        public void ProcessShutDowm(byte[] bytes)
        {
            try
            {
                VehicleOnOffTimeTreatment.StatOnOffTime(bytes);
            }
            catch (Exception ex)
            {
                LoggerManager.Logger.Error(ex);
            }
        }

        [DealWith(typeName: "MDVR.V30.")]
        public void PreccessGpsData(byte[] bytes)
        {
            try
            {
                VehicleOnOffTimeTreatment.StatGpsDistance(bytes);
            }
            catch (Exception ex)
            {
                LoggerManager.Logger.Error(ex);
            }
        }
        #endregion
    }
}
