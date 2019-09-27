using Gsafety.Ant.Message.Test.MessageService;
using Jounce.Core.Event;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gsafety.PTMS.Message.Test
{
    /// <summary>
    /// 
    /// </summary>
    [TestClass]
    public class MessageTest
    {
        public MessageTest()
        {

        }

        System.ServiceModel.InstanceContext _InstanceContext = new System.ServiceModel.InstanceContext(new MQMessageCallBack());



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

        #region 附加测试特性
        //
        // 编写测试时，可以使用以下附加特性:
        //
        // 在运行类中的第一个测试之前使用 ClassInitialize 运行代码
        // [ClassInitialize()]
        // public static void MyClassInitialize(TestContext testContext) { }
        //
        // 在类中的所有测试都已运行之后使用 ClassCleanup 运行代码
        // [ClassCleanup()]
        // public static void MyClassCleanup() { }
        //
        // 在运行每个测试之前，使用 TestInitialize 来运行代码
        // [TestInitialize()]
        // public void MyTestInitialize() { }
        //
        // 在每个测试运行完之后，使用 TestCleanup 来运行代码
        // [TestCleanup()]
        // public void MyTestCleanup() { }
        //
        #endregion 

        #region 获取设备类告警信息

        [TestMethod]
        public void GetDeviceAlertMessage()
        {
            MessageServiceClient Client = new MessageServiceClient(_InstanceContext);

            Client.Init("laoxu");

            Client.GetCameraNoSignalAlertMessage();
 
            
        }

        #endregion
    }

    public class MQMessageCallBack : IMessageServiceCallback
    {
        /// <summary>
        /// 发布者
        /// </summary>
        [Import]
        public IEventAggregator _EventAggregator { get; set; }

        /// <summary>
        /// 消息回调接口
        /// </summary>
        /// <param name="message"></param>
        public void MessageCallBack(object message)
        {
            ////一键报警
            if (message is AlarmInfo)
            {
                _EventAggregator.Publish<AlarmInfo>(message as AlarmInfo);
            }

            ////一键报警处理完成
            if (message is CompleteAlarm)
            {
                _EventAggregator.Publish<CompleteAlarm>(message as CompleteAlarm);
            }

            ////一键报警处理信息
            if (message is HandingAlarm)
            {
                _EventAggregator.Publish<HandingAlarm>(message as HandingAlarm);
            }

            ////设备安装完成信息
            if (message is DeviceInstall)
            {
                _EventAggregator.Publish<DeviceInstall>(message as DeviceInstall);
            }

            ////一键报警处理信息 
            if (message is DeviceMaintain)
            {
                _EventAggregator.Publish<DeviceMaintain>(message as DeviceMaintain);
            }

            ////一键报警处理信息
            if (message is HandingAlert)
            {
                _EventAggregator.Publish<HandingAlert>(message as HandingAlert);
            }

            ////无信号的摄像头通道号
            if (message is CameraNoSignalAlert)
            {
                _EventAggregator.Publish<CameraNoSignalAlert>(message as CameraNoSignalAlert);
            }

            ////摄像头遮挡告警
            if (message is CameraOcclusionAlert)
            {
                _EventAggregator.Publish<CameraOcclusionAlert>(message as CameraOcclusionAlert);
            }

            ////点火报警实体
            if (message is FireAlert)
            {
                _EventAggregator.Publish<FireAlert>(message as FireAlert);
            }

            ////GPS接收机故障报警实体
            if (message is GpsReceiverFaultAlert)
            {
                _EventAggregator.Publish<GpsReceiverFaultAlert>(message as GpsReceiverFaultAlert);
            }

            ////MDVR存储卡错误报警实体
            if (message is MdvrMemoryCardErrorAlert)
            {
                _EventAggregator.Publish<MdvrMemoryCardErrorAlert>(message as MdvrMemoryCardErrorAlert);
            }

            ////异常开关门报警实体
            if (message is OpenOrCloseDoorAbnormalAlert)
            {
                _EventAggregator.Publish<OpenOrCloseDoorAbnormalAlert>(message as OpenOrCloseDoorAbnormalAlert);
            }

            ////超速报警实体
            if (message is OverSpeedAlert)
            {
                _EventAggregator.Publish<OverSpeedAlert>(message as OverSpeedAlert);
            }

            ////区域报警实体
            if (message is RegionAlert)
            {
                _EventAggregator.Publish<RegionAlert>(message as RegionAlert);
            }

            ////安全套件警情解除通知实体
            if (message is RemoveDeviceSuiteAlertNotify)
            {
                _EventAggregator.Publish<RemoveDeviceSuiteAlertNotify>(message as RemoveDeviceSuiteAlertNotify);
            }

            ////温度报警实体
            if (message is TemperatureAlert)
            {
                _EventAggregator.Publish<TemperatureAlert>(message as TemperatureAlert);
            }

            ////电压异常报警实体
            if (message is VoltageAbnormalAlert)
            {
                _EventAggregator.Publish<VoltageAbnormalAlert>(message as VoltageAbnormalAlert);
            }

            ////自检信息
            if (message is InspectInfo)
            {
                _EventAggregator.Publish<InspectInfo>(message as InspectInfo);
            }

            ////软件版本升级
            if (message is UpgradeCMD)
            {
                _EventAggregator.Publish<UpgradeCMD>(message as UpgradeCMD);
            }

            ////GPS位置数据 V30
            if (message is GPS)
            {
                _EventAggregator.Publish<GPS>(message as GPS);
            }

            ////上下线实体
            if (message is OnOffline)
            {
                _EventAggregator.Publish<OnOffline>(message as OnOffline);
            }

            ////设置电子围栏
            if (message is ElectricFenceCMD)
            {
                _EventAggregator.Publish<ElectricFenceCMD>(message as ElectricFenceCMD);
            }
        }
    }
}
