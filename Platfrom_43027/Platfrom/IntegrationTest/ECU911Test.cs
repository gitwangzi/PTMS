/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: e21fb932-e105-4687-9d13-79f65dcbab25      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: WANGJINCAI
/////                 Author: TEST(wangjc)
/////======================================================================
/////           Project Name: Gsafety.PTMS.Integration.Test
/////    Project Description:    
/////             Class Name: ECU911Test
/////          Class Version: v1.0.0.0
/////            Create Time: 2013-09-27 14:46:43
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2013-09-27 14:46:43
/////            Modified by:
/////   Modified Description: 
/////======================================================================
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gsafety.PTMS.Integration.Service;
using Gsafety.PTMS.DBEntity;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Gsafety.PTMS.Integration.Test
{
    [TestClass]
    public class ECU911Test
    {
        private Ecu911Service _srv;

        private TestContext testContextInstance;

        /// <summary>
        ///获取或设置测试上下文，上下文提供
        ///有关当前测试运行及其功能的信息。
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }
        public ECU911Test()
        {
            _srv = new Ecu911Service();
        }

        [TestMethod]
        public void GetLocation_Test()
        {
            var _context = new PTMSEntities();
            var lo = _context.VEHICLE_LOCATION.FirstOrDefault();
            if (lo != null)
            {
                var result = _srv.GetLocation(lo.VEHICLE_ID);
                Assert.IsNotNull(result);
            }
            else
            {
                Assert.IsTrue(true);
            }
        }

        [TestMethod]
        public void GetLocationHistory_Test()
        {
            var _context = new PTMSEntities();
            var lo = _context.VEHICLE_LOCATION.FirstOrDefault();
            if (lo != null)
            {
                var st = _context.VEHICLE_LOCATION.Min(x => x.GPS_TIME);
                var nt = st.Value.AddDays(1);
                var result = _srv.GetLocationHistory(lo.VEHICLE_ID, st.Value, nt);
                Assert.IsNotNull(result);
            }
            else
            {
                Assert.IsTrue(true);
            }
        }

        [TestMethod]
        public void GetVehicleInfo_Test()
        {
            var _context = new PTMSEntities();
            var q = _context.VEHICLE
                        //.Join(_context.VEHICLE_COMPANY, x => x.COMPANY_ID, y => y.ID, (a, b) => new { A = a, B = b })
                        .Join(_context.SECURITY_SUITE_WORKING, x => x.VEHICLE_ID, y => y.VEHICLE_ID, (a, b) => new { A = a,  C = b })
                        .Join(_context.SECURITY_SUITE_INFO, x => x.C.SUITE_INFO_ID, y => y.SUITE_INFO_ID, (a, b) => new { A = a.A, C = a.C, D = b })
                        .Join(_context.DISTRICT, x => x.A.DISTRICT_CODE, y => y.CODE, (a, b) => new { A = a.A, C = a.C, D = a.D, E = b })
                     ;
            var item= q.FirstOrDefault();

            if (q != null)
            {
                var result = _srv.GetVehicleInfo(item.A.VEHICLE_ID);
                Assert.IsNotNull(result);
            }
            else
            {
                Assert.IsTrue(true);
            }
        }

        [TestMethod]
        public void EndAlarm_Test()
        {
            var _context = new PTMSEntities();
            var al = _context.ECU911_DISPOSE.FirstOrDefault();
            if (al != null)
            {
                var args = new Gsafety.PTMS.Integration.Contract.Data.AlarmArgs
                {
                    AlarmId = al.ALARM_ID,
                    AlarmType = 1,
                    Content = "DDDDD",
                    DispatchEndTime = DateTime.Now,
                    Dispatcher = "xxxxx",
                    VehicleId = "BJ0001",

                };
                var result = _srv.EndAlarm(args);
                Assert.IsNotNull(result);
            }
            else
            {
                Assert.IsTrue(true);
            }

        }
    }
}
