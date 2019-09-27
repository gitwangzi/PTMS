using Gs.PTMS.Service;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Microsoft.VisualStudio.TestTools.UnitTesting.Web;
using Gsafety.PTMS.Manager.Contract.Data;
using Gsafety.PTMS.Base.Contract.Data;
using System.Dynamic;

namespace PTMSServiceTest
{


    /// <summary>
    ///这是 AccountServiceTest 的测试类，旨在
    ///包含所有 AccountServiceTest 单元测试
    ///</summary>
    [TestClass()]
    public class AccountServiceTest
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
        ///GetGUser 的测试
        ///</summary>
        // TODO: 确保 UrlToTest 特性指定一个指向 ASP.NET 页的 URL(例如，
        // http://.../Default.aspx)。这对于在 Web 服务器上执行单元测试是必需的，
        //无论要测试页、Web 服务还是 WCF 服务都是如此。
        [TestMethod()]
        //[HostType("ASP.NET")]
        //[UrlToTest("http://localhost:2258")]
        public void GetGUserTest()
        {
            AccountService target = new AccountService(); // TODO: 初始化为适当的值
            int pageIndex = 0; // TODO: 初始化为适当的值
            int pageSize = 0; // TODO: 初始化为适当的值
            //var actual = target.GetGUserList(pageIndex, pageSize);
            //Assert.IsTrue(actual.IsSuccess);
        }

        /// <summary>
        ///GetAllFunItems 的测试
        ///</summary>
        // TODO: 确保 UrlToTest 特性指定一个指向 ASP.NET 页的 URL(例如，
        // http://.../Default.aspx)。这对于在 Web 服务器上执行单元测试是必需的，
        //无论要测试页、Web 服务还是 WCF 服务都是如此。
        [TestMethod()]
        public void GetAllFunItemsTest()
        {
            AccountService target = new AccountService(); // TODO: 初始化为适当的值
            MultiMessage<FuncItem> expected = null; // TODO: 初始化为适当的值
            MultiMessage<FuncItem> actual;
            //actual = target.GetAllFunItems();
        }

        /// <summary>
        ///InsertRole 的测试
        ///</summary>
        // TODO: 确保 UrlToTest 特性指定一个指向 ASP.NET 页的 URL(例如，
        // http://.../Default.aspx)。这对于在 Web 服务器上执行单元测试是必需的，
        //无论要测试页、Web 服务还是 WCF 服务都是如此。
        [TestMethod()]
        [HostType("ASP.NET")]
        [AspNetDevelopmentServerHost("F:\\Project\\PTMS\\trunk\\A8_resource\\Platfrom\\PTMSService", "/")]
        [UrlToTest("http://localhost:2258/")]
        public void InsertRoleTest()
        {
            AccountService target = new AccountService(); // TODO: 初始化为适当的值
            Role model = new Role()
            {

            };

            //target.InsertRole(model);
        }
    }
}
