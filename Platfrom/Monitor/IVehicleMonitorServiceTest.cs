using Gsafety.PTMS.Monitor.Contract;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Gsafety.PTMS.Monitor.Contract.Data;
using Gsafety.PTMS.Base.Contract.Data;
using Gsafety.PTMS.Monitor.Service;

namespace Gsafety.PTMS.Monitor.Test
{


    /// <summary>
    ///这是 IVehicleMonitorServiceTest 的测试类，旨在
    ///包含所有 IVehicleMonitorServiceTest 单元测试
    ///</summary>
    [TestClass()]
    public class IVehicleMonitorServiceTest
    {


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

        #region 附加测试特性
        // 
        //编写测试时，还可使用以下特性:
        //
        //使用 ClassInitialize 在运行类中的第一个测试前先运行代码
        //[ClassInitialize()]
        //public static void MyClassInitialize(TestContext testContext)
        //{
        //}
        //
        //使用 ClassCleanup 在运行完类中的所有测试后再运行代码
        //[ClassCleanup()]
        //public static void MyClassCleanup()
        //{
        //}
        //
        //使用 TestInitialize 在运行每个测试前先运行代码
        //[TestInitialize()]
        //public void MyTestInitialize()
        //{
        //}
        //
        //使用 TestCleanup 在运行完每个测试后运行代码
        //[TestCleanup()]
        //public void MyTestCleanup()
        //{
        //}
        //
        #endregion


        internal virtual IVehicleMonitorService CreateIVehicleMonitorService()
        {
            // 实现接口IVehicleMonitorService
            IVehicleMonitorService target = new VehicleMonitorService();
            return target;
        }

        #region 获取车辆产生的告警信息

        [TestMethod()]
        public void GetVehicleAlertTest()
        {
            IVehicleMonitorService target = CreateIVehicleMonitorService();
            string vehicleId = string.Empty;
            DateTime startDate = new DateTime();
            DateTime endDate = new DateTime();
            string alertType = string.Empty;
            MultiMessage<VehicleAlert> expected = new TestDataModel().Expected1; //期望结果
            MultiMessage<VehicleAlert> actual;
            actual = target.GetVehicleAlert(vehicleId, startDate, endDate, alertType);
            Assert.AreEqual(expected, actual);
            //Assert.Inconclusive("验证此测试方法的正确性。");
        }
        #endregion

        #region 获取车辆安全套件上的视频
        [TestMethod()]
        public void GetVehicleMdvrVedioTest()
        {
            IVehicleMonitorService target = CreateIVehicleMonitorService();
            string vehicleId = string.Empty;
            DateTime startDate = new DateTime();
            DateTime endDate = new DateTime();
            MultiMessage<VedioFile> expected = new TestDataModel().Expected2; //期望结果
            MultiMessage<VedioFile> actual;
            actual = new MultiMessage<VedioFile>();
            //actual = target.GetVehicleMdvrVedio(vehicleId, startDate, endDate);
            Assert.AreEqual(expected, actual);
            //Assert.Inconclusive("验证此测试方法的正确性。");
        }
        #endregion

        #region  根据车牌号获取车辆的历史轨迹
        [TestMethod()]
        public void GetVehicleTrackTest()
        {
            IVehicleMonitorService target = CreateIVehicleMonitorService();
            string vehicleId = string.Empty;
            DateTime startDate = new DateTime();
            DateTime endDate = new DateTime();
            MultiMessage<VehicleTrack> expected = new TestDataModel().Expected3; //期望结果
            MultiMessage<VehicleTrack> actual;
            actual = new MultiMessage<VehicleTrack>();
            //actual = target.GetVehicleTrack(vehicleId, startDate, endDate);
            Assert.AreEqual(expected, actual);
            //Assert.Inconclusive("验证此测试方法的正确性。");
        }
        #endregion

        #region 获取车辆已播放视频
        [TestMethod()]
        public void GetVehicleVedioTest()
        {
            IVehicleMonitorService target = CreateIVehicleMonitorService();
            string vehicleId = string.Empty;
            DateTime startDate = new DateTime();
            DateTime endDate = new DateTime();
            MultiMessage<VedioFile> expected = new TestDataModel().Expected4; //期望结果
            MultiMessage<VedioFile> actual;
            actual = new MultiMessage<VedioFile>();
            //actual = target.GetVehicleVedio(vehicleId, startDate, endDate);
            Assert.AreEqual(expected, actual);
            //Assert.Inconclusive("验证此测试方法的正确性。");
        }
        #endregion

    }
}
