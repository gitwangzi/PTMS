using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;
using Gsafety.PTMS.Manager.Contract.Data;
using Gsafety.PTMS.Base.Contract.Data;
using Gsafety.PTMS.Manager.Service;
namespace Gsafety.Ant.Manager.Test
{
    /// <summary>
    /// VehicleRepository 的摘要说明
    /// </summary>
    [TestClass]
    public class AlarmDealLogInfoServiceTest
    {
        private TestContext testContextInstance;
        /// <summary>
        ///获取或设置测试上下文，该上下文提供
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
        public PagingInfo Paginfo
        {
            get
            {

                return new PagingInfo()
                {

                    PageIndex = 1,
                    PageSize = 20
                };

            }
        }

        [TestMethod]
        public void GetAlarmDealLog()
        {
            PTMSLogManageService service = new PTMSLogManageService();
            MultiMessage<AlarmDealLogInfo> list = service.GetAlarmDealLog("ant_tester", DateTime.Parse("2015-1-1"), DateTime.Now, this.Paginfo);

            Assert.IsNull(list.ExceptionMessage);
            Assert.IsNotNull(list.Result);

        }

    }
}

