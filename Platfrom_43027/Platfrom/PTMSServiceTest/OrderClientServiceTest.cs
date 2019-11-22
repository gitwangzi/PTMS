using Gs.PTMS.Service;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Microsoft.VisualStudio.TestTools.UnitTesting.Web;
using Gsafety.PTMS.BaseInformation.Contract.Data;
using Gsafety.PTMS.Base.Contract.Data;
using Gsafety.PTMS.BaseInformation.Repository;
using Gsafety.PTMS.DBEntity;

namespace PTMSServiceTest
{


    /// <summary>
    ///这是 OrderClientServiceTest 的测试类，旨在
    ///包含所有 OrderClientServiceTest 单元测试
    ///</summary>
    [TestClass()]
    public class OrderClientServiceTest
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


        /// <summary>
        ///InsertOrderClient 的测试
        ///</summary>
        // TODO: 确保 UrlToTest 特性指定一个指向 ASP.NET 页的 URL(例如，
        // http://.../Default.aspx)。这对于在 Web 服务器上执行单元测试是必需的，
        //无论要测试页、Web 服务还是 WCF 服务都是如此。
        [TestMethod()]
        public void InsertOrderClientTest()
        {
            //OrderClientService target = new OrderClientService(); // TODO: 初始化为适当的值
            //OrderClientEx model = new OrderClientEx()
            //{
            //    ID=Guid.NewGuid().ToString(),
            //    Address = "五道口",
            //    BeginTime = new DateTime(2016, 1, 6),
            //    EndTime = new DateTime(2017, 1, 16),
            //    Contact = "张立明",
            //    DeviceCount = 1000,
            //    Name = "辰安信息",
            //    Email = "1000@gsafety.com",
            //    Mobile = "13622222222",
            //    Phone = "032545451",
            //    Status = StatusEnum.Normal,
            //    TansferMode = TansferModeEnum.DirectTransfer,
            //    UserCount = 100,
            //    UserName = "test2@gsafety.com"
            //};

            //target.InsertOrderClient(model);
        }

        /// <summary>
        ///UpdateOrderClient 的测试
        ///</summary>
        // TODO: 确保 UrlToTest 特性指定一个指向 ASP.NET 页的 URL(例如，
        // http://.../Default.aspx)。这对于在 Web 服务器上执行单元测试是必需的，
        //无论要测试页、Web 服务还是 WCF 服务都是如此。
        [TestMethod()]
        public void UpdateOrderClientTest()
        {
            //OrderClientService target = new OrderClientService(); // TODO: 初始化为适当的值
            //OrderClientEx model = new OrderClientEx()
            //{
            //    ID = "5d7c2aec-1170-41db-a143-d2f11f160654",
            //    Address = "五道口五道口",
            //    BeginTime = new DateTime(2016, 1, 6),
            //    EndTime = new DateTime(2017, 1, 16),
            //    Contact = "张立明",
            //    DeviceCount = 1000,
            //    Name = "辰安信息",
            //    Email = "1000@gsafety.com",
            //    Mobile = "13622222222",
            //    Phone = "032545451",
            //    Status = StatusEnum.Normal,
            //    TansferMode = TansferModeEnum.DirectTransfer,
            //    UserCount = 100,
            //    UserName = "chenan@gsafety.com"
            //};
            //target.UpdateOrderClient(model);
        }

        /// <summary>
        ///SetOrderClientStatus 的测试
        ///</summary>
        // TODO: 确保 UrlToTest 特性指定一个指向 ASP.NET 页的 URL(例如，
        // http://.../Default.aspx)。这对于在 Web 服务器上执行单元测试是必需的，
        //无论要测试页、Web 服务还是 WCF 服务都是如此。
        [TestMethod()]
        public void SetOrderClientStatusTest()
        {
            OrderClientService target = new OrderClientService(); // TODO: 初始化为适当的值
            string orderClientID = "5d7c2aec-1170-41db-a143-d2f11f160654"; // TODO: 初始化为适当的值
            bool enable = false; // TODO: 初始化为适当的值
            SingleMessage<bool> expected = null; // TODO: 初始化为适当的值
            SingleMessage<bool> actual;
            //actual = target.SetOrderClientStatus(orderClientID, enable);
        }

        [TestMethod()]
        public void GetVType()
        {
            //VehicleRepository target = new VehicleRepository(); // TODO: 初始化为适当的值
            ////Vehicle  v = new Vehicle();
            ////v.VehicleId="1";
            ////v.OwnerId="232";
            ////target.AddVehicle(v);

            //target.GetVehicleType("20");
            //VehicleType v = new VehicleType();
            //v.ID = "74eaf829-4bdd-41b0-afe2-58c2440b9463";
            //v.ClientID = "5";
            //v.Name = "kache";
            //v.CreateTime = DateTime.Now;
            //v.Creator = "admin";
            //v.Description = "gggggg";
            //VehicleTypeRepository.DeleteVehicleTypeByID(new PTMSEntities(), "74eaf829-4bdd-41b0-afe2-58c2440b9463");
            //v.Valid = 1;
            //VehicleTypeRepository.UpdateVehicleType(new PTMSEntities(), v);
            //VehicleTypeRepository.GetVehicleTypeList("20");

            InstallStationRepository s = new InstallStationRepository(); int w;
            //InstallStation t = new InstallStation();
            //t.ID = "20a8d936-47e8-47ja-8258-82c5819132d6";
            //t.Name = "er";
            //t.Address = "sd";
            //t.ClientID = "20a8d936-47e8-47ja-9278-82c5819138d5";
            //t.Contact = "gf";
            //t.ContactPhone = "gf";
            //t.CreateTime = DateTime.Now; ;
            //t.Director = "fsf";
            //t.DirectorPhone = "eer";
            //t.DistrictCode = "fsd";
            //t.Email = "gdg";
            //t.Note = "e";
            //s.AddInstallStation(t);

            s.GetInstallStationsFuzzy(null, "df", "we", new PagingInfo() { PageIndex = 1, PageSize = 10 }, out w, null, "20a8d936-47e8-47fa-8258-82c5819132d8");

            //s.GetInstallStationsByAlphabet(null, new PagingInfo() { PageIndex=1,PageSize=10}, out w, "20a8d936-47e8-47fa-8258-82c5819132d8");

        }


    }
}
