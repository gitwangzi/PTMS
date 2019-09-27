using Gsafety.PTMS.SecuritySuite.Contract;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Gsafety.PTMS.SecuritySuite.Contract.Data;
using Gsafety.PTMS.Base.Contract.Data;
using Gsafety.PTMS.SecuritySuite.Service;

/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
///// Guid: a2a104b0-cf3f-43b2-b2c8-eefd726551f5      
///// clrversion: 4.0.30319.17929
/////Registered organization: 
///// Machine Name: PC-Shihs
///// Author: Hongsheng Shi
/////======================================================================
/////  Project Name: Gsafety.PTMS.BaseInformation.Contract.Data
/////  Project Description:    
///// Class Name: MonitorRepository
///// Class Version: v1.0.0.0
///// Create Time: 2013/8/27 15:47:53
///// Class Description:  
/////======================================================================
/////  Modified Time: 
/////  Modified by: 
/////  Modified Description: 
/////======================================================================
namespace Gsafety.PTMS.SecuritySuite.Test
{


    /// <summary>
    ///这是 IVehicleStatusServiceTest 的测试类，旨在
    ///包含所有 IVehicleStatusServiceTest 单元测试
    ///</summary>
    [TestClass()]
    public class IVehicleStatusServiceTest
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


        internal virtual IVehicleStatusService CreateIVehicleStatusService()
        {
            // TODO: 实例化相应的具体类。
            IVehicleStatusService target = new VehicleStatusService();
          	return target;         
        }

        /// <summary>
        ///根据车公司获取车辆状态 
        ///</summary>
        [TestMethod()]
        public void GetVehicleStatusByCompanyTest()
        {
            IVehicleStatusService target = CreateIVehicleStatusService(); 
            string companyId = string.Empty; 
            MultiMessage<VehicleStatus> expected = new TestDataModel().Expected1; 
            MultiMessage<VehicleStatus> actual;

        }

        /// <summary>
        ///GetVehicleStatusByCompanyEx 的测试
        ///</summary>
        [TestMethod()]
        public void GetVehicleStatusByCompanyExTest()
        {
            IVehicleStatusService target = CreateIVehicleStatusService(); 
            string companyId = string.Empty; 
            int vehicleType = 0; 
            MultiMessage<VehicleStatus> expected = new TestDataModel().Expected2; 
            MultiMessage<VehicleStatus> actual;
        }

        /// <summary>
        ///GetVehicleStatusByDistrict 的测试
        ///</summary>
        [TestMethod()]
        public void GetVehicleStatusByDistrictTest()
        {
            IVehicleStatusService target = CreateIVehicleStatusService();
            string districtCode = "DC001"; 
            MultiMessage<VehicleStatus> expected = new TestDataModel().Expected3;
            MultiMessage<VehicleStatus> actual;
            actual = target.GetVehicleStatusByDistrict(districtCode);
            Assert.AreEqual(expected, actual);
            //Assert.Inconclusive("验证此测试方法的正确性。");
        }

        /// <summary>
        ///GetVehicleStatusByDistrictEx 的测试
        ///</summary>
        [TestMethod()]
        public void GetVehicleStatusByDistrictExTest()
        {
            IVehicleStatusService target = CreateIVehicleStatusService();
            string districtCode = "DC001"; 
            int vehicleType = 3;
            MultiMessage<VehicleStatus> expected = new TestDataModel().Expected4;
            MultiMessage<VehicleStatus> actual;
            actual = target.GetVehicleStatusByDistrictEx(districtCode, vehicleType);
            Assert.AreEqual(expected, actual);
            //Assert.Inconclusive("验证此测试方法的正确性。");
        }

        /// <summary>
        ///GetVehicleStatusByGroup 的测试
        ///</summary>
        [TestMethod()]
        public void GetVehicleStatusByGroupTest()
        {
            IVehicleStatusService target = CreateIVehicleStatusService(); 
            string groupId = string.Empty; 
            MultiMessage<VehicleStatus> expected = new TestDataModel().Expected5; 
            MultiMessage<VehicleStatus> actual;
            actual = target.GetVehicleStatusByGroup(groupId);
            Assert.AreEqual(expected, actual);
            //Assert.Inconclusive("验证此测试方法的正确性。");
        }

        /// <summary>
        ///GetVehicleStatusByStatus 的测试
        ///</summary>
        [TestMethod()]
        public void GetVehicleStatusByStatusTest()
        {
            IVehicleStatusService target = CreateIVehicleStatusService(); 
            string districtCode = string.Empty; 
            bool isOnline = false; 
            MultiMessage<VehicleStatus> expected = new TestDataModel().Expected6; 
            MultiMessage<VehicleStatus> actual;
            actual = target.GetVehicleStatusByStatus(districtCode, isOnline);
            Assert.AreEqual(expected, actual);
            //Assert.Inconclusive("验证此测试方法的正确性。");
        }

        /// <summary>
        ///GetVehicleStatusByVehicleNumber 的测试
        ///</summary>
        [TestMethod()]
        public void GetVehicleStatusByVehicleNumberTest()
        {
            IVehicleStatusService target = CreateIVehicleStatusService();
            string vehicleId = "BJ0001"; 
            SingleMessage<VehicleStatus> expected = new TestDataModel().Expected7; 
            SingleMessage<VehicleStatus> actual;
            actual = target.GetVehicleStatusByVehicleNumber(vehicleId);
            Assert.AreEqual(expected, actual);
            //Assert.Inconclusive("验证此测试方法的正确性。");
        }
    }
}
