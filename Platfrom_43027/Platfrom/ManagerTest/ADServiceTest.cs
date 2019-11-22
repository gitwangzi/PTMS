/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: cb1e4848-302a-48f4-9fc8-90d68fddeae5      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: PC-WANGZS
/////                 Author: TEST(wangzs)
/////======================================================================
/////           Project Name: Gsafety.PTMS.Manager.Test
/////    Project Description:    
/////             Class Name: UnitTest1
/////          Class Version: v1.0.0.0
/////            Create Time: 2013/7/31 17:09:00
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2013/7/31 17:09:00
/////            Modified by:
/////   Modified Description: 
/////======================================================================
using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Gsafety.PTMS.Manager.Test.TestDataModel;
using System.Linq;
using System.Xml.Linq;
using System.IO;
using System.Text;
using System.Collections.Generic;
using Gsafety.Ant.Manager.Test.ADUserInfoService;
using Gsafety.Ant.Manager.Test.GroupService;
using Gsafety.PTMS.Manager.Contract.Data.CommandManage;
using Gsafety.PTMS.Manager.Service;
using Gsafety.PTMS.Base.Contract.Data;
namespace Gsafety.PTMS.Manager.Test
{
    /// <summary>
    /// ADServiceTest 的摘要说明
    /// </summary>
    [TestClass]
    public class ADServiceTest
    {
        public ADServiceTest()
        {
            //
            //TODO: 在此处添加构造函数逻辑
            //
        }
        ADAccountServiceClient Client = new ADAccountServiceClient();
        GroupServiceClient groupClient = new GroupServiceClient();
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

        #region 添加用户接口的单元测试
        /// <summary>
        /// 对象为空的情形
        /// </summary>
        [TestMethod]
        public void AddUserAccountByModel()
        {
            // ADUserInfoServiceClient Client = new ADUserInfoServiceClient();

            Gsafety.Ant.Manager.Test.ADUserInfoService.ADAccountInfo inputmodel = null;
            var result = Client.AddAccount(inputmodel);
            Assert.IsNull(result);
        }
        /// <summary>
        /// 正常的流程
        /// </summary>
        [TestMethod]
        public void TestAddUserAccountByModel1()
        {

            Gsafety.Ant.Manager.Test.ADUserInfoService.ADAccountInfo inputmodel = new Gsafety.Ant.Manager.Test.ADUserInfoService.ADAccountInfo();
            inputmodel.UserName = "wzsunittest6";
            inputmodel.DisplayName = "testprince";
            inputmodel.UserPassword = "wang_123";
            inputmodel.SecurityGroup = "SecurityManager";
            var result = false;
            if (Client.IsUserExits(inputmodel.UserName).Result)
            {

                Assert.IsFalse(result);
            }
            else
            {
                var result1 = Client.AddAccount(inputmodel);

                Assert.IsTrue(result1.Result);
            }

        }
        /// <summary>
        /// 验证用户名为空的情形
        /// </summary>
        [TestMethod]
        public void TestAddUserAccountByModel2()
        {

            Gsafety.Ant.Manager.Test.ADUserInfoService.ADAccountInfo inputmodel = new Gsafety.Ant.Manager.Test.ADUserInfoService.ADAccountInfo();
            inputmodel.UserName = string.Empty;
            inputmodel.DisplayName = "testprince";
            inputmodel.UserPassword = "wang_123";
            inputmodel.SecurityGroup = "SecurityManager";
            var result = Client.AddAccount(inputmodel);
            var expect = false;
            Assert.AreEqual(expect, result.Result);
        }

        #endregion
        #region 删除用户账户接口的单元测试
        [TestMethod]
        public void DeleteAccount()
        {

            var inputmessage = "wzsunittest6";
            var target = false;
            if (Client.IsUserExits("zhangwuliua1333").Result)
            {
                target = Client.DeleteAccount(inputmessage).Result;
                Assert.IsTrue(target);
            }
            else
            {
                Assert.IsFalse(target);
            }



        }
        [TestMethod]
        public void DeleteAccount1()
        {

            var inputmessage = string.Empty;
            var result = Client.DeleteAccount(inputmessage);
            Assert.IsFalse(result.Result);
        }
        [TestMethod]
        [ExpectedException(typeof(System.ServiceModel.FaultException))]
        public void DeleteAccount2()
        {

            var inputmessage = "zhangwuliua1333";
            var result = Client.DeleteAccount(inputmessage);

        }
        #endregion
        #region 修改用户信息的单元测试
        [TestMethod]
        public void ChangeUserInfo()
        {

            Gsafety.Ant.Manager.Test.ADUserInfoService.ADAccountInfo user = new Gsafety.Ant.Manager.Test.ADUserInfoService.ADAccountInfo();
            user.UserName = string.Empty;
            var target = Client.UpdateAccount(user);
            var expected = false;
            Assert.AreEqual(expected, target.Result);
        }
        [TestMethod]
        public void ChangeUserInfo1()
        {

            Gsafety.Ant.Manager.Test.ADUserInfoService.ADAccountInfo user = new Gsafety.Ant.Manager.Test.ADUserInfoService.ADAccountInfo();
            user.UserName = "unexit";
            var target = Client.UpdateAccount(user);
            var expected = false;
            Assert.AreEqual(expected, target.Result);
        }
        [TestMethod]
        public void ChangeUserInfo12()
        {

            Gsafety.Ant.Manager.Test.ADUserInfoService.ADAccountInfo user = new Gsafety.Ant.Manager.Test.ADUserInfoService.ADAccountInfo();
            user.UserName = "unexit";
            var target = Client.UpdateAccount(user);
            var expected = false;
            Assert.AreEqual(expected, target.Result);
        }
        [TestMethod]
        public void ChangeUserInfo2()
        {

            Gsafety.Ant.Manager.Test.ADUserInfoService.ADAccountInfo user = new Gsafety.Ant.Manager.Test.ADUserInfoService.ADAccountInfo();
            user.UserName = "ant123";
            user.Phone = "123444";
            user.Description = "222222222";
            user.Company = "123";
            var target = Client.UpdateAccount(user);
            var expected = true;
            Assert.AreEqual(expected, target.Result);
        }

        #endregion
        #region 重置密码测试
        [TestMethod]
        public void ResetPassword()
        {

            var accountName = string.Empty;
            var password = string.Empty;
            var expected = false;
            var result = Client.ResetPassword(accountName, password);
            Assert.AreEqual(expected, result.Result);
        }
        [TestMethod]
        public void ResetPassword1()
        {

            var accountName = "ant123";
            var password = "wang_1234";
            var expected = true;
            var result = Client.ResetPassword(accountName, password);
            Assert.AreEqual(expected, result.Result);
        }
        [TestMethod]
        public void ResetPassword2()
        {

            var accountName = "ant123";
            var password = "wang_1234";
            var expected = true;
            var result = Client.ResetPassword(accountName, password);
            Assert.AreEqual(expected, result.Result);
        }
        [TestMethod]
        public void ResetPassword3()
        {

            var accountName = "ant1235555";
            var password = "wang_1234";
            var expected = false;
            var result = Client.ResetPassword(accountName, password);
            Assert.AreEqual(expected, result.Result);
        }
        #endregion
        #region 禁用用户
        /// <summary>
        /// 用户名为空的情形
        /// </summary>
        [TestMethod]
        public void DisableUserAccount()
        {
            var inputuserName = string.Empty;
            var expect = false;
            var result = Client.DisableAccount(inputuserName);
            Assert.AreEqual(expect, result.Result);
        }
        /// <summary>
        /// 禁用某用户
        /// </summary>
        [TestMethod]
        public void DisableUserAccount1()
        {
            var inputuserName = "ant123";
            var expect = true;
            var result = Client.DisableAccount(inputuserName);
            Assert.AreEqual(expect, result.Result);
        }
        /// <summary>
        /// 已经被禁用的用户
        /// </summary>
        [TestMethod]
        public void DisableUserAccount2()
        {
            var inputuserName = "ant123";
            var expect = true;
            var result = Client.DisableAccount(inputuserName);
            Assert.AreEqual(expect, result.Result);
        }
        /// <summary>
        /// 用户名不存在的情形
        /// </summary>
        [TestMethod]
        public void DisableUserAccount23()
        {
            var inputuserName = "ant1234";
            var expect = false;
            var result = Client.DisableAccount(inputuserName);
            Assert.AreEqual(expect, result.Result);
        }
        #endregion
        #region 激活用户
        /// <summary>
        /// 空用户 期望为fasle
        /// </summary>
        [TestMethod]
        public void EnableUserAccount()
        {
            var inputmessage = "";
            var expect = false;
            var result = Client.EnableAccount(inputmessage);
            Assert.AreEqual(expect, result.Result);
        }
        [TestMethod]
        public void EnableUserAccount1()
        {
            var inputmessage = "ant123";
            var expect = true;
            var result = Client.EnableAccount(inputmessage);
            Assert.AreEqual(expect, result.Result);
        }
        [TestMethod]
        public void EnableUserAccount13()
        {
            var inputmessage = "ant123eeee";
            var expect = false;
            var result = Client.EnableAccount(inputmessage);
            Assert.AreEqual(expect, result.Result);
        }
        #endregion
        #region 验证 用户是否存在
        [TestMethod]
        public void IsUserExits()
        {
            var inputmessage = string.Empty;
            var expected = false;
            var result = Client.IsUserExits(inputmessage);
            Assert.AreEqual(expected, result.Result);
        }
        [TestMethod]
        public void IsUserExits1()
        {
            var inputmessage = "ant123";
            var expected = true;
            var result = Client.IsUserExits(inputmessage);
            Assert.AreEqual(expected, result.Result);
        }
        [TestMethod]
        public void IsUserExits2()
        {
            var inputmessage = "ant12344443";
            var expected = false;
            var result = Client.IsUserExits(inputmessage);
            Assert.AreEqual(expected, result.Result);
        }
        #endregion
        //#region 获取用户信息
        //[TestMethod]
        //public void GetUserInfoModel()
        //{

        //    var inputmessage = string.Empty;
        //    Gsafety.Ant.Manager.Test.GroupService.ADAccountInfo expected = null;
        //    var result = groupClient.GetAccountInfoByGroupName(inputmessage);
        //    Assert.AreEqual(expected, result);
        //}
        ///// <summary>
        ///// 
        ///// </summary>
        //[TestMethod]
        //public void GetUserInfoModel1()
        //{
        //    var inputmessage = "ant123";
        //    Gsafety.Ant.Manager.Test.GroupService.ADAccountInfo expected = new Gsafety.Ant.Manager.Test.GroupService.ADAccountInfo();
        //    expected.UserName = "ant123";
        //    expected.Company = "123";
        //    var result = groupClient.GetAccountInfoByGroupName(inputmessage); 

        //    Assert.AreEqual(expected.UserName, result.);
        //    Assert.AreEqual(expected.Company, result.Company);
        //}
        ///// <summary>
        ///// 测试用例:
        ///// 期望结果:
        ///// </summary>
        //[TestMethod]
        //public void GetUserInfoModel2()
        //{
        //    var inputmessage = "unexituser";
        //    //  ADAccountInfoModel expected =null;
        //    var result = Client.GetUserInfoModel(inputmessage);
        //    Assert.IsNull(result);
        //}
        //#endregion
        #region 创建安全组
        /// <summary>
        /// 测试用例:输入值为空的情形
        ///期望:false
        /// </summary>
        [TestMethod]
        public void CreateGroup()
        {
            var inputgroupName = string.Empty;
            var expected = false;

            var result = groupClient.AddGroup(inputgroupName);
            Assert.AreEqual(expected, result.Result);

        }
        /// <summary>
        /// 测试用例:正确的输入参数
        /// 期望结果:true
        /// </summary>
        [TestMethod]
        public void CreateGroup1()
        {
            var inputgroupName = "AManage";
            var expected = false;
            var result = groupClient.AddGroup(inputgroupName);
            Assert.AreEqual(expected, result.Result);

        }
        /// <summary>
        /// 测试用例:创建已经存在的安全组
        /// 期望结果:false
        /// </summary>
        [TestMethod]
        public void CreateGroup2()
        {
            var inputgroupName = "AManage";
            var expected = false;
            var result = groupClient.AddGroup(inputgroupName);
            Assert.AreEqual(expected, result.Result);

        }

        #endregion
        #region 验证用户合法性
        /// <summary>
        /// 测试用例:错误的用户名或者密码
        /// 期望结果:result为NULL;
        /// </summary>
        [TestMethod]
        public void ValidateUser()
        {

            var inputName = "ant123";
            var inputpasswprd = "wang_12345";


            var result = Client.ValidateUser(inputName, inputpasswprd);

            Assert.IsNull(result);
        }
        /// <summary>
        /// 测试用例:正确的用户名和密码
        /// 期望结果:result不为 NULL;
        /// </summary>
        [TestMethod]
        public void ValidateUser1()
        {

            var inputName = "ant_tester";
            var inputpasswprd = "wang_123";

            var result = Client.ValidateUser(inputName, inputpasswprd);

            Assert.IsNotNull(result);
        }
        // 533b18ac-2572-4f04-9c92-0cee492b751a
        #endregion

        # region  Alarm
        const string chinesecharacter = @"阿啊哀唉挨矮爱碍安岸按案暗昂袄傲奥八巴扒吧疤拔把坝爸罢霸白百柏摆败拜班般斑搬板版办半伴扮拌瓣帮绑榜膀傍棒包胞雹宝饱保堡报抱暴爆杯悲碑北贝备背倍被辈奔本笨蹦逼鼻比彼笔鄙币必毕闭毙弊碧蔽壁避臂边编鞭扁便变遍辨辩辫标表别宾滨冰兵丙柄饼并病拨波玻剥脖菠播伯驳泊博搏膊薄卜补捕不布步怖部擦猜才材财裁采彩睬踩菜参餐残蚕惭惨灿仓苍舱藏操槽草册侧厕测策层叉插查茶察岔差拆柴馋缠产铲颤昌长肠尝偿常厂场敞畅倡唱抄钞超朝潮吵炒车扯彻撤尘臣沉辰陈晨闯衬称趁撑成呈承诚城乘惩程秤吃驰迟持池匙尺齿耻斥赤翅充冲虫崇抽仇绸愁稠筹酬丑臭出初除厨锄础储楚处触畜川穿传船喘串疮窗床创吹炊垂锤春纯唇蠢词慈辞磁此次刺从匆葱聪丛凑粗促醋窜催摧脆翠村存寸错曾搭达答打大呆代带待怠贷袋逮戴丹单担耽胆旦但诞弹淡蛋当挡党荡档刀叨导岛倒蹈到悼盗道稻得德的灯登等凳低堤滴敌笛底抵地弟帝递第颠典点电店垫殿叼雕吊钓调掉爹跌叠蝶丁叮盯钉顶订定丢东冬董懂动冻栋洞都斗抖陡豆逗督毒读独堵赌杜肚度渡端短段断缎锻堆队对吨蹲盾顿多夺朵躲惰鹅蛾额恶饿恩儿而耳二发乏伐罚阀法帆番翻凡烦繁反返犯泛饭范贩方坊芳防妨房仿访纺放飞非肥匪废沸肺费分吩纷芬坟粉份奋愤粪丰风封疯峰锋蜂逢缝讽凤奉佛否夫肤伏扶服俘浮符幅福抚府斧俯辅腐父付妇负附咐复赴副傅富腹覆该改盖溉概干甘杆肝竿秆赶敢感冈刚岗纲缸钢港杠高膏糕搞稿告哥胳鸽割搁歌阁革格葛隔个各给根跟更耕工弓公功攻供宫恭躬巩共贡勾沟钩狗构购够估姑孤辜古谷股骨鼓固故顾瓜刮挂乖拐怪关观官冠馆管贯惯灌罐光广归龟规轨鬼柜贵桂跪滚棍锅国果裹过哈孩海害含寒喊汉汗旱航毫豪好号浩耗喝禾合何和河核荷盒贺黑痕很狠恨恒横衡轰哄烘红宏洪虹喉猴吼后厚候乎呼忽狐胡壶湖糊蝴虎互户护花华哗滑猾化划画话怀槐坏欢还环缓幻唤换患荒慌皇黄煌晃谎灰恢挥辉回悔汇会绘贿惠毁慧昏婚浑魂混活火伙或货获祸惑击饥圾机肌鸡迹积基绩激及吉级即极急疾集籍几己挤脊计记纪忌技际剂季既济继寄加夹佳家嘉甲价驾架假嫁稼奸尖坚歼间肩艰兼监煎拣俭茧捡减剪检简见件建剑荐贱健舰渐践鉴键箭江姜将浆僵疆讲奖桨匠降酱交郊娇浇骄胶椒焦蕉角狡绞饺脚搅缴叫轿较教阶皆接揭街节劫杰洁结捷截竭姐解介戒届界借巾今斤金津筋仅紧谨锦尽劲近进晋浸禁京经茎惊晶睛精井颈景警净径竞竟敬境静镜纠究揪九久酒旧救就舅居拘鞠局菊橘举矩句巨拒具俱剧惧据距锯聚捐卷倦绢决绝觉掘嚼军君均菌俊卡开凯慨刊堪砍做看康糠扛抗炕考烤靠科棵颗壳咳可渴克刻客课肯垦恳坑空孔恐控口扣寇枯哭苦库裤酷夸垮挎跨块快宽款筐狂况旷矿框亏葵愧昆捆困扩括阔垃拉啦喇腊蜡辣来赖兰拦栏蓝篮览懒烂滥郎狼廊朗浪捞劳牢老姥涝乐勒雷垒泪类累冷厘梨狸离犁鹂璃黎礼李里理力历厉立丽利励例隶栗粒俩连帘怜莲联廉镰脸练炼恋链良凉梁粮粱两亮谅辆量辽疗僚了料列劣烈猎裂邻林临淋伶灵岭铃陵零龄领令另溜刘流留榴柳六龙笼聋隆垄拢楼搂漏露芦炉虏鲁陆录鹿滤碌路驴旅屡律虑率绿卵乱掠略轮论罗萝锣箩骡螺络骆落妈麻马码蚂骂吗埋买迈麦卖脉蛮馒瞒满慢漫忙芒盲茫猫毛矛茅茂冒贸帽貌么没眉梅煤霉每美妹门闷们萌盟猛蒙孟梦迷谜米眯秘密蜜眠绵棉免勉面苗描秒妙庙灭蔑民敏名明鸣命摸模膜摩磨魔抹末沫莫漠墨默谋某母亩木目牧墓幕慕暮拿哪内那纳乃奶耐男南难囊挠恼脑闹呢嫩能尼泥你逆年念娘酿鸟尿捏您宁凝牛扭纽农浓弄奴努怒女暖挪欧偶辟趴爬怕拍牌派攀盘判叛盼乓旁胖抛炮袍跑泡陪培赔佩配喷盆朋棚蓬膨捧碰批披劈皮疲脾匹僻片偏篇骗漂飘票撇拼贫品乒平评凭苹瓶萍坡泼婆迫破魄剖仆扑铺葡朴普谱七妻戚期欺漆齐其奇骑棋旗乞企岂启起气弃汽砌器恰洽千迁牵铅谦签前钱钳潜浅遣欠歉枪腔强墙抢悄敲锹乔侨桥瞧巧切茄且窃亲侵芹琴禽勤青轻倾清蜻情晴顷请庆穷丘秋求球区曲驱屈趋渠取去趣圈全权泉拳犬劝券缺却雀确鹊裙群然燃染嚷壤让饶扰绕惹热人仁忍刃认任扔仍日绒荣容熔融柔揉肉如乳辱入软锐瑞润若弱撒洒塞赛三伞散桑嗓丧扫嫂色森杀沙纱傻筛晒山删衫闪陕扇善伤商裳晌赏上尚捎梢烧稍勺少绍哨舌蛇舍设社射涉摄申伸身深神沈审婶肾甚渗慎升生声牲胜绳省圣盛剩尸失师诗施狮湿十什石时识实拾蚀食史使始驶士氏世市示式事侍势视试饰室是柿适逝释誓收手守首寿受兽售授瘦书叔殊梳疏舒输蔬熟暑鼠属薯术束述树竖数刷耍衰摔甩帅拴双霜爽谁水税睡顺说嗽丝司私思斯撕死四寺似饲肆松宋诵送颂搜艘苏俗诉肃素速宿塑酸蒜算虽随岁碎穗孙损笋缩所索锁她他它塌塔踏台抬太态泰贪摊滩坛谈痰坦毯叹炭探汤唐堂塘膛糖倘躺烫趟涛掏滔逃桃陶淘萄讨套特疼腾梯踢提题蹄体剃惕替天添田甜填挑条跳贴铁帖厅听亭庭停挺艇通同桐铜童统桶筒痛偷头投透秃突图徒涂途屠土吐兔团推腿退吞屯托拖脱驼妥娃挖蛙瓦袜歪外弯湾丸完玩顽挽晚碗万汪亡王网往妄忘旺望危威微为围违唯维伟伪尾委卫未位味畏胃喂慰温文纹闻蚊稳问翁窝我沃卧握乌污呜屋无吴五午伍武侮舞勿务物误悟雾夕西吸希析息牺悉惜稀溪锡熄膝习席袭洗喜戏系细隙虾瞎峡狭霞下吓夏厦仙先纤掀鲜闲弦贤咸衔嫌显险县现线限宪陷馅羡献乡相香箱详祥享响想向巷项象像橡削宵消销小晓孝效校笑些歇协邪胁斜携鞋写泄泻卸屑械谢心辛欣新薪信兴星腥刑行形型醒杏姓幸性凶兄胸雄熊休修羞朽秀绣袖锈须虚需徐许序叙绪续絮蓄宣悬旋选穴学雪血寻巡旬询循训讯迅压呀押鸦鸭牙芽崖哑雅亚咽烟淹延严言岩沿炎研盐蜒颜掩眼演厌宴艳验焰雁燕央殃秧扬羊阳杨洋仰养氧痒样妖腰邀窑谣摇遥咬药要耀爷也冶野业叶页夜液一衣医依仪宜姨移遗疑乙已以蚁倚椅义亿忆艺议亦异役译易疫益谊意毅翼因阴姻音银引饮隐印应英樱鹰迎盈营蝇赢影映硬佣拥庸永咏泳勇涌用优忧悠尤由犹邮油游友有又右幼诱于予余鱼娱渔愉愚榆与宇屿羽雨语玉育郁狱浴预域欲御裕遇愈誉冤元员园原圆援缘源远怨院愿约月钥悦阅跃越云匀允孕运晕韵杂灾栽宰载再在咱暂赞脏葬遭糟早枣澡灶皂造燥躁则择泽责贼怎增赠渣扎轧闸眨炸榨摘宅窄债寨沾粘斩展盏崭占战站张章涨掌丈仗帐胀障招找召兆赵照罩遮折哲者这浙贞针侦珍真诊枕阵振镇震争征挣睁筝蒸整正证郑政症之支汁芝枝知织肢脂蜘执侄直值职植殖止只旨址纸指至志制帜治质秩致智置中忠终钟肿种众重州舟周洲粥宙昼皱骤朱株珠诸猪蛛竹烛逐主煮嘱住助注驻柱祝著筑铸抓爪专砖转赚庄装壮状撞追准捉桌浊啄着仔姿资滋子紫字自宗棕踪总纵走奏租足族阻组祖钻嘴最罪醉尊遵昨左作坐座做";
        const string spanishcharacter = "qwértyúíopasdghjklñ´zxcvbnm?¿¡!1234567890 \"";
        const string englishcharacter = "abcdefghigklmnopqrstuvwxyz1234567890 ;*";
        //private TestContext testContextInstance;
        /// <summary>
        ///获取或设置测试上下文，该上下文提供
        ///有关当前测试运行及其功能的信息。
        ///</summary>
        //public TestContext TestContext
        //{
        //    get
        //    {
        //        return testContextInstance;
        //    }
        //    set
        //    {
        //        testContextInstance = value;
        //    }
        //}
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
        #region
        /// <summary>
        /// 所有的字段为空,找不到连接字符串
        /// </summary>
        [TestMethod]
        public void TestStringForAllEmpty()
        {
            AlarmSettingRules alarmsettingrules = new AlarmSettingRules();
            CommandManageService service = new CommandManageService();
            alarmsettingrules.Alarm_RuleID = null;
            alarmsettingrules.Alarm_RuleName = null;
            alarmsettingrules.Alarm_Creator = null;
            alarmsettingrules.Alarm_Description = null;
            SingleMessage<bool> ret = service.AlarmSettingAdd(alarmsettingrules);
            Assert.IsTrue(ret.IsSuccess);
        }


        /// <summary>
        /// 允许的字段为空
        /// </summary>
        [TestMethod]
        public void TestStringForAllowEmpty()
        {
            AlarmSettingRules alarmsettingrules = new AlarmSettingRules();
            CommandManageService service = new CommandManageService();
            StringBuilder builder = new StringBuilder();
            Random random = new Random();
            for (int i = 0; i < 36; i++)
            {
                builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
            }
            alarmsettingrules.Alarm_RuleID = builder.ToString();
            builder.Length = 0;
            alarmsettingrules.Alarm_RuleName = null;
            alarmsettingrules.Alarm_Creator = null;
            alarmsettingrules.Alarm_Description = null;
            SingleMessage<bool> ret = service.AlarmSettingAdd(alarmsettingrules);
            Assert.IsTrue(ret.IsSuccess);
        }


        /// <summary>
        /// 测试最大中文长度
        /// </summary>
        [TestMethod]
        public void TestStringForMaxLength_Chinese()
        {
            AlarmSettingRules alarmsettingrules = new AlarmSettingRules();
            CommandManageService service = new CommandManageService();
            StringBuilder builder = new StringBuilder();
            Random random = new Random();
            for (int i = 0; i < 17; i++)
            {
                builder.Append(chinesecharacter[random.Next(chinesecharacter.Length - 1)]);
            }
            alarmsettingrules.Alarm_RuleID = builder.ToString();
            builder.Length = 0;

            for (int i = 0; i < 255; i++)
            {
                builder.Append(chinesecharacter[random.Next(chinesecharacter.Length - 1)]);
            }
            alarmsettingrules.Alarm_RuleName = builder.ToString();
            builder.Length = 0;

            for (int i = 0; i < 255; i++)
            {
                builder.Append(chinesecharacter[random.Next(chinesecharacter.Length - 1)]);
            }
            alarmsettingrules.Alarm_Creator = builder.ToString();
            builder.Length = 0;

            for (int i = 0; i < 999; i++)
            {
                builder.Append(chinesecharacter[random.Next(chinesecharacter.Length - 1)]);
            }
            alarmsettingrules.Alarm_Description = builder.ToString();
            builder.Length = 0;

            SingleMessage<bool> ret = service.AlarmSettingAdd(alarmsettingrules);
            //Assert.IsFalse(ret.IsSuccess);
        }


        /// <summary>
        /// 测试西文最大长度
        /// </summary>
        [TestMethod]
        public void TestStringForMaxLength_Spanish()
        {
            AlarmSettingRules alarmsettingrules = new AlarmSettingRules();
            CommandManageService service = new CommandManageService();
            StringBuilder builder = new StringBuilder();
            Random random = new Random();
            for (int i = 0; i < 17; i++)
            {
                builder.Append(spanishcharacter[random.Next(spanishcharacter.Length - 1)]);
            }
            alarmsettingrules.Alarm_RuleID = builder.ToString();
            builder.Length = 0;

            for (int i = 0; i < 255; i++)
            {
                builder.Append(spanishcharacter[random.Next(spanishcharacter.Length - 1)]);
            }
            alarmsettingrules.Alarm_RuleName = builder.ToString();
            builder.Length = 0;

            for (int i = 0; i < 255; i++)
            {
                builder.Append(spanishcharacter[random.Next(spanishcharacter.Length - 1)]);
            }
            alarmsettingrules.Alarm_Creator = builder.ToString();
            builder.Length = 0;

            for (int i = 0; i < 999; i++)
            {
                builder.Append(spanishcharacter[random.Next(spanishcharacter.Length - 1)]);
            }
            alarmsettingrules.Alarm_Description = builder.ToString();
            builder.Length = 0;

            SingleMessage<bool> ret = service.AlarmSettingAdd(alarmsettingrules);
            //Assert.IsFalse(ret.IsSuccess);
        }


        /// <summary>
        /// 测试英文最大长度
        /// </summary>
        [TestMethod]
        public void TestStringForMaxLength_English()
        {
            AlarmSettingRules alarmsettingrules = new AlarmSettingRules();
            CommandManageService service = new CommandManageService();
            StringBuilder builder = new StringBuilder();
            Random random = new Random();
            for (int i = 0; i < 17; i++)
            {
                builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
            }
            alarmsettingrules.Alarm_RuleID = builder.ToString();
            builder.Length = 0;

            for (int i = 0; i < 255; i++)
            {
                builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
            }
            alarmsettingrules.Alarm_RuleName = builder.ToString();
            builder.Length = 0;

            for (int i = 0; i < 255; i++)
            {
                builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
            }
            alarmsettingrules.Alarm_Creator = builder.ToString();
            builder.Length = 0;

            for (int i = 0; i < 999; i++)
            {
                builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
            }
            alarmsettingrules.Alarm_Description = builder.ToString();
            builder.Length = 0;

            SingleMessage<bool> ret = service.AlarmSettingAdd(alarmsettingrules);
            //Assert.IsFalse(ret.IsSuccess);
        }


        /// <summary>
        /// 测试Alarm_RuleID中文超长
        /// </summary>
        [TestMethod]
        public void TestAlarm_RuleIDStringForOverLength_Chinese()
        {
            AlarmSettingRules alarmsettingrules = new AlarmSettingRules();
            CommandManageService service = new CommandManageService();
            StringBuilder builder = new StringBuilder();
            Random random = new Random();
            for (int i = 0; i < 18; i++)
            {
                builder.Append(chinesecharacter[random.Next(chinesecharacter.Length - 1)]);
            }
            alarmsettingrules.Alarm_RuleID = builder.ToString();
            builder.Length = 0;

            for (int i = 0; i < 255; i++)
            {
                builder.Append(chinesecharacter[random.Next(chinesecharacter.Length - 1)]);
            }
            alarmsettingrules.Alarm_RuleName = builder.ToString();
            builder.Length = 0;

            for (int i = 0; i < 255; i++)
            {
                builder.Append(chinesecharacter[random.Next(chinesecharacter.Length - 1)]);
            }
            alarmsettingrules.Alarm_Creator = builder.ToString();
            builder.Length = 0;

            for (int i = 0; i < 999; i++)
            {
                builder.Append(chinesecharacter[random.Next(chinesecharacter.Length - 1)]);
            }
            alarmsettingrules.Alarm_Description = builder.ToString();
            builder.Length = 0;

            SingleMessage<bool> ret = service.AlarmSettingAdd(alarmsettingrules);
            //Assert.IsFalse(ret.IsSuccess);
        }


        /// <summary>
        /// 测试Alarm_RuleID西文超长
        /// </summary>
        [TestMethod]
        public void TestAlarm_RuleIDStringForMaxLength_Spanish()
        {
            AlarmSettingRules alarmsettingrules = new AlarmSettingRules();
            CommandManageService service = new CommandManageService();
            StringBuilder builder = new StringBuilder();
            Random random = new Random();
            for (int i = 0; i < 18; i++)
            {
                builder.Append(spanishcharacter[random.Next(spanishcharacter.Length - 1)]);
            }
            alarmsettingrules.Alarm_RuleID = builder.ToString();
            builder.Length = 0;

            for (int i = 0; i < 255; i++)
            {
                builder.Append(spanishcharacter[random.Next(spanishcharacter.Length - 1)]);
            }
            alarmsettingrules.Alarm_RuleName = builder.ToString();
            builder.Length = 0;

            for (int i = 0; i < 255; i++)
            {
                builder.Append(spanishcharacter[random.Next(spanishcharacter.Length - 1)]);
            }
            alarmsettingrules.Alarm_Creator = builder.ToString();
            builder.Length = 0;

            for (int i = 0; i < 999; i++)
            {
                builder.Append(spanishcharacter[random.Next(spanishcharacter.Length - 1)]);
            }
            alarmsettingrules.Alarm_Description = builder.ToString();
            builder.Length = 0;

            SingleMessage<bool> ret = service.AlarmSettingAdd(alarmsettingrules);
            //Assert.IsFalse(ret.IsSuccess);
        }


        /// <summary>
        /// 测试Alarm_RuleID英文超长
        /// </summary>
        [TestMethod]
        public void TestAlarm_RuleIDStringForMaxLength_English()
        {
            AlarmSettingRules alarmsettingrules = new AlarmSettingRules();
            CommandManageService service = new CommandManageService();
            StringBuilder builder = new StringBuilder();
            Random random = new Random();
            for (int i = 0; i < 18; i++)
            {
                builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
            }
            alarmsettingrules.Alarm_RuleID = builder.ToString();
            builder.Length = 0;

            for (int i = 0; i < 255; i++)
            {
                builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
            }
            alarmsettingrules.Alarm_RuleName = builder.ToString();
            builder.Length = 0;

            for (int i = 0; i < 255; i++)
            {
                builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
            }
            alarmsettingrules.Alarm_Creator = builder.ToString();
            builder.Length = 0;

            for (int i = 0; i < 999; i++)
            {
                builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
            }
            alarmsettingrules.Alarm_Description = builder.ToString();
            builder.Length = 0;

            SingleMessage<bool> ret = service.AlarmSettingAdd(alarmsettingrules);
            //Assert.IsFalse(ret.IsSuccess);
        }


        /// <summary>
        /// 测试Alarm_RuleName中文超长
        /// </summary>
        [TestMethod]
        public void TestAlarm_RuleNameStringForOverLength_Chinese()
        {
            AlarmSettingRules alarmsettingrules = new AlarmSettingRules();
            CommandManageService service = new CommandManageService();
            StringBuilder builder = new StringBuilder();
            Random random = new Random();
            for (int i = 0; i < 17; i++)
            {
                builder.Append(chinesecharacter[random.Next(chinesecharacter.Length - 1)]);
            }
            alarmsettingrules.Alarm_RuleID = builder.ToString();
            builder.Length = 0;

            for (int i = 0; i < 256; i++)
            {
                builder.Append(chinesecharacter[random.Next(chinesecharacter.Length - 1)]);
            }
            alarmsettingrules.Alarm_RuleName = builder.ToString();
            builder.Length = 0;

            for (int i = 0; i < 255; i++)
            {
                builder.Append(chinesecharacter[random.Next(chinesecharacter.Length - 1)]);
            }
            alarmsettingrules.Alarm_Creator = builder.ToString();
            builder.Length = 0;

            for (int i = 0; i < 999; i++)
            {
                builder.Append(chinesecharacter[random.Next(chinesecharacter.Length - 1)]);
            }
            alarmsettingrules.Alarm_Description = builder.ToString();
            builder.Length = 0;

            SingleMessage<bool> ret = service.AlarmSettingAdd(alarmsettingrules);
            //Assert.IsFalse(ret.IsSuccess);
        }


        /// <summary>
        /// 测试Alarm_RuleName西文超长
        /// </summary>
        [TestMethod]
        public void TestAlarm_RuleNameStringForMaxLength_Spanish()
        {
            AlarmSettingRules alarmsettingrules = new AlarmSettingRules();
            CommandManageService service = new CommandManageService();
            StringBuilder builder = new StringBuilder();
            Random random = new Random();
            for (int i = 0; i < 17; i++)
            {
                builder.Append(spanishcharacter[random.Next(spanishcharacter.Length - 1)]);
            }
            alarmsettingrules.Alarm_RuleID = builder.ToString();
            builder.Length = 0;

            for (int i = 0; i < 256; i++)
            {
                builder.Append(spanishcharacter[random.Next(spanishcharacter.Length - 1)]);
            }
            alarmsettingrules.Alarm_RuleName = builder.ToString();
            builder.Length = 0;

            for (int i = 0; i < 255; i++)
            {
                builder.Append(spanishcharacter[random.Next(spanishcharacter.Length - 1)]);
            }
            alarmsettingrules.Alarm_Creator = builder.ToString();
            builder.Length = 0;

            for (int i = 0; i < 999; i++)
            {
                builder.Append(spanishcharacter[random.Next(spanishcharacter.Length - 1)]);
            }
            alarmsettingrules.Alarm_Description = builder.ToString();
            builder.Length = 0;

            SingleMessage<bool> ret = service.AlarmSettingAdd(alarmsettingrules);
            //Assert.IsFalse(ret.IsSuccess);
        }


        /// <summary>
        /// 测试Alarm_RuleName英文超长
        /// </summary>
        [TestMethod]
        public void TestAlarm_RuleNameStringForMaxLength_English()
        {
            AlarmSettingRules alarmsettingrules = new AlarmSettingRules();
            CommandManageService service = new CommandManageService();
            StringBuilder builder = new StringBuilder();
            Random random = new Random();
            for (int i = 0; i < 17; i++)
            {
                builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
            }
            alarmsettingrules.Alarm_RuleID = builder.ToString();
            builder.Length = 0;

            for (int i = 0; i < 256; i++)
            {
                builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
            }
            alarmsettingrules.Alarm_RuleName = builder.ToString();
            builder.Length = 0;

            for (int i = 0; i < 255; i++)
            {
                builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
            }
            alarmsettingrules.Alarm_Creator = builder.ToString();
            builder.Length = 0;

            for (int i = 0; i < 999; i++)
            {
                builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
            }
            alarmsettingrules.Alarm_Description = builder.ToString();
            builder.Length = 0;

            SingleMessage<bool> ret = service.AlarmSettingAdd(alarmsettingrules);
            //Assert.IsFalse(ret.IsSuccess);
        }


        /// <summary>
        /// 测试Alarm_ButtonTime中文超长
        /// </summary>
        [TestMethod]
        public void TestAlarm_ButtonTimeStringForOverLength_Chinese()
        {
            AlarmSettingRules alarmsettingrules = new AlarmSettingRules();
            CommandManageService service = new CommandManageService();
            StringBuilder builder = new StringBuilder();
            Random random = new Random();
            for (int i = 0; i < 17; i++)
            {
                builder.Append(chinesecharacter[random.Next(chinesecharacter.Length - 1)]);
            }
            alarmsettingrules.Alarm_RuleID = builder.ToString();
            builder.Length = 0;

            for (int i = 0; i < 255; i++)
            {
                builder.Append(chinesecharacter[random.Next(chinesecharacter.Length - 1)]);
            }
            alarmsettingrules.Alarm_RuleName = builder.ToString();
            builder.Length = 0;

            for (int i = 0; i < 255; i++)
            {
                builder.Append(chinesecharacter[random.Next(chinesecharacter.Length - 1)]);
            }
            alarmsettingrules.Alarm_Creator = builder.ToString();
            builder.Length = 0;

            for (int i = 0; i < 999; i++)
            {
                builder.Append(chinesecharacter[random.Next(chinesecharacter.Length - 1)]);
            }
            alarmsettingrules.Alarm_Description = builder.ToString();
            builder.Length = 0;

            SingleMessage<bool> ret = service.AlarmSettingAdd(alarmsettingrules);
            //Assert.IsFalse(ret.IsSuccess);
        }


        /// <summary>
        /// 测试Alarm_ButtonTime西文超长
        /// </summary>
        [TestMethod]
        public void TestAlarm_ButtonTimeStringForMaxLength_Spanish()
        {
            AlarmSettingRules alarmsettingrules = new AlarmSettingRules();
            CommandManageService service = new CommandManageService();
            StringBuilder builder = new StringBuilder();
            Random random = new Random();
            for (int i = 0; i < 17; i++)
            {
                builder.Append(spanishcharacter[random.Next(spanishcharacter.Length - 1)]);
            }
            alarmsettingrules.Alarm_RuleID = builder.ToString();
            builder.Length = 0;

            for (int i = 0; i < 255; i++)
            {
                builder.Append(spanishcharacter[random.Next(spanishcharacter.Length - 1)]);
            }
            alarmsettingrules.Alarm_RuleName = builder.ToString();
            builder.Length = 0;

            for (int i = 0; i < 255; i++)
            {
                builder.Append(spanishcharacter[random.Next(spanishcharacter.Length - 1)]);
            }
            alarmsettingrules.Alarm_Creator = builder.ToString();
            builder.Length = 0;

            for (int i = 0; i < 999; i++)
            {
                builder.Append(spanishcharacter[random.Next(spanishcharacter.Length - 1)]);
            }
            alarmsettingrules.Alarm_Description = builder.ToString();
            builder.Length = 0;

            SingleMessage<bool> ret = service.AlarmSettingAdd(alarmsettingrules);
            //Assert.IsFalse(ret.IsSuccess);
        }


        /// <summary>
        /// 测试Alarm_ButtonTime英文超长
        /// </summary>
        [TestMethod]
        public void TestAlarm_ButtonTimeStringForMaxLength_English()
        {
            AlarmSettingRules alarmsettingrules = new AlarmSettingRules();
            CommandManageService service = new CommandManageService();
            StringBuilder builder = new StringBuilder();
            Random random = new Random();
            for (int i = 0; i < 17; i++)
            {
                builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
            }
            alarmsettingrules.Alarm_RuleID = builder.ToString();
            builder.Length = 0;

            for (int i = 0; i < 255; i++)
            {
                builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
            }
            alarmsettingrules.Alarm_RuleName = builder.ToString();
            builder.Length = 0;

            for (int i = 0; i < 255; i++)
            {
                builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
            }
            alarmsettingrules.Alarm_Creator = builder.ToString();
            builder.Length = 0;

            for (int i = 0; i < 999; i++)
            {
                builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
            }
            alarmsettingrules.Alarm_Description = builder.ToString();
            builder.Length = 0;

            SingleMessage<bool> ret = service.AlarmSettingAdd(alarmsettingrules);
            //Assert.IsFalse(ret.IsSuccess);
        }


        /// <summary>
        /// 测试Alarm_Normal中文超长
        /// </summary>
        [TestMethod]
        public void TestAlarm_NormalStringForOverLength_Chinese()
        {
            AlarmSettingRules alarmsettingrules = new AlarmSettingRules();
            CommandManageService service = new CommandManageService();
            StringBuilder builder = new StringBuilder();
            Random random = new Random();
            for (int i = 0; i < 17; i++)
            {
                builder.Append(chinesecharacter[random.Next(chinesecharacter.Length - 1)]);
            }
            alarmsettingrules.Alarm_RuleID = builder.ToString();
            builder.Length = 0;

            for (int i = 0; i < 255; i++)
            {
                builder.Append(chinesecharacter[random.Next(chinesecharacter.Length - 1)]);
            }
            alarmsettingrules.Alarm_RuleName = builder.ToString();
            builder.Length = 0;

            for (int i = 0; i < 255; i++)
            {
                builder.Append(chinesecharacter[random.Next(chinesecharacter.Length - 1)]);
            }
            alarmsettingrules.Alarm_Creator = builder.ToString();
            builder.Length = 0;

            for (int i = 0; i < 999; i++)
            {
                builder.Append(chinesecharacter[random.Next(chinesecharacter.Length - 1)]);
            }
            alarmsettingrules.Alarm_Description = builder.ToString();
            builder.Length = 0;

            SingleMessage<bool> ret = service.AlarmSettingAdd(alarmsettingrules);
            //Assert.IsFalse(ret.IsSuccess);
        }


        /// <summary>
        /// 测试Alarm_Normal西文超长
        /// </summary>
        [TestMethod]
        public void TestAlarm_NormalStringForMaxLength_Spanish()
        {
            AlarmSettingRules alarmsettingrules = new AlarmSettingRules();
            CommandManageService service = new CommandManageService();
            StringBuilder builder = new StringBuilder();
            Random random = new Random();
            for (int i = 0; i < 17; i++)
            {
                builder.Append(spanishcharacter[random.Next(spanishcharacter.Length - 1)]);
            }
            alarmsettingrules.Alarm_RuleID = builder.ToString();
            builder.Length = 0;

            for (int i = 0; i < 255; i++)
            {
                builder.Append(spanishcharacter[random.Next(spanishcharacter.Length - 1)]);
            }
            alarmsettingrules.Alarm_RuleName = builder.ToString();
            builder.Length = 0;

            for (int i = 0; i < 255; i++)
            {
                builder.Append(spanishcharacter[random.Next(spanishcharacter.Length - 1)]);
            }
            alarmsettingrules.Alarm_Creator = builder.ToString();
            builder.Length = 0;

            for (int i = 0; i < 999; i++)
            {
                builder.Append(spanishcharacter[random.Next(spanishcharacter.Length - 1)]);
            }
            alarmsettingrules.Alarm_Description = builder.ToString();
            builder.Length = 0;

            SingleMessage<bool> ret = service.AlarmSettingAdd(alarmsettingrules);
            //Assert.IsFalse(ret.IsSuccess);
        }


        /// <summary>
        /// 测试Alarm_Normal英文超长
        /// </summary>
        [TestMethod]
        public void TestAlarm_NormalStringForMaxLength_English()
        {
            AlarmSettingRules alarmsettingrules = new AlarmSettingRules();
            CommandManageService service = new CommandManageService();
            StringBuilder builder = new StringBuilder();
            Random random = new Random();
            for (int i = 0; i < 17; i++)
            {
                builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
            }
            alarmsettingrules.Alarm_RuleID = builder.ToString();
            builder.Length = 0;

            for (int i = 0; i < 255; i++)
            {
                builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
            }
            alarmsettingrules.Alarm_RuleName = builder.ToString();
            builder.Length = 0;

            for (int i = 0; i < 255; i++)
            {
                builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
            }
            alarmsettingrules.Alarm_Creator = builder.ToString();
            builder.Length = 0;

            for (int i = 0; i < 999; i++)
            {
                builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
            }
            alarmsettingrules.Alarm_Description = builder.ToString();
            builder.Length = 0;

            SingleMessage<bool> ret = service.AlarmSettingAdd(alarmsettingrules);
            //Assert.IsFalse(ret.IsSuccess);
        }


        /// <summary>
        /// 测试Alarm_Creator中文超长
        /// </summary>
        [TestMethod]
        public void TestAlarm_CreatorStringForOverLength_Chinese()
        {
            AlarmSettingRules alarmsettingrules = new AlarmSettingRules();
            CommandManageService service = new CommandManageService();
            StringBuilder builder = new StringBuilder();
            Random random = new Random();
            for (int i = 0; i < 17; i++)
            {
                builder.Append(chinesecharacter[random.Next(chinesecharacter.Length - 1)]);
            }
            alarmsettingrules.Alarm_RuleID = builder.ToString();
            builder.Length = 0;

            for (int i = 0; i < 255; i++)
            {
                builder.Append(chinesecharacter[random.Next(chinesecharacter.Length - 1)]);
            }
            alarmsettingrules.Alarm_RuleName = builder.ToString();
            builder.Length = 0;

            for (int i = 0; i < 256; i++)
            {
                builder.Append(chinesecharacter[random.Next(chinesecharacter.Length - 1)]);
            }
            alarmsettingrules.Alarm_Creator = builder.ToString();
            builder.Length = 0;

            for (int i = 0; i < 999; i++)
            {
                builder.Append(chinesecharacter[random.Next(chinesecharacter.Length - 1)]);
            }
            alarmsettingrules.Alarm_Description = builder.ToString();
            builder.Length = 0;

            SingleMessage<bool> ret = service.AlarmSettingAdd(alarmsettingrules);
            //Assert.IsFalse(ret.IsSuccess);
        }


        /// <summary>
        /// 测试Alarm_Creator西文超长
        /// </summary>
        [TestMethod]
        public void TestAlarm_CreatorStringForMaxLength_Spanish()
        {
            AlarmSettingRules alarmsettingrules = new AlarmSettingRules();
            CommandManageService service = new CommandManageService();
            StringBuilder builder = new StringBuilder();
            Random random = new Random();
            for (int i = 0; i < 17; i++)
            {
                builder.Append(spanishcharacter[random.Next(spanishcharacter.Length - 1)]);
            }
            alarmsettingrules.Alarm_RuleID = builder.ToString();
            builder.Length = 0;

            for (int i = 0; i < 255; i++)
            {
                builder.Append(spanishcharacter[random.Next(spanishcharacter.Length - 1)]);
            }
            alarmsettingrules.Alarm_RuleName = builder.ToString();
            builder.Length = 0;

            for (int i = 0; i < 256; i++)
            {
                builder.Append(spanishcharacter[random.Next(spanishcharacter.Length - 1)]);
            }
            alarmsettingrules.Alarm_Creator = builder.ToString();
            builder.Length = 0;

            for (int i = 0; i < 999; i++)
            {
                builder.Append(spanishcharacter[random.Next(spanishcharacter.Length - 1)]);
            }
            alarmsettingrules.Alarm_Description = builder.ToString();
            builder.Length = 0;

            SingleMessage<bool> ret = service.AlarmSettingAdd(alarmsettingrules);
            //Assert.IsFalse(ret.IsSuccess);
        }


        /// <summary>
        /// 测试Alarm_Creator英文超长
        /// </summary>
        [TestMethod]
        public void TestAlarm_CreatorStringForMaxLength_English()
        {
            AlarmSettingRules alarmsettingrules = new AlarmSettingRules();
            CommandManageService service = new CommandManageService();
            StringBuilder builder = new StringBuilder();
            Random random = new Random();
            for (int i = 0; i < 17; i++)
            {
                builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
            }
            alarmsettingrules.Alarm_RuleID = builder.ToString();
            builder.Length = 0;

            for (int i = 0; i < 255; i++)
            {
                builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
            }
            alarmsettingrules.Alarm_RuleName = builder.ToString();
            builder.Length = 0;

            for (int i = 0; i < 256; i++)
            {
                builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
            }
            alarmsettingrules.Alarm_Creator = builder.ToString();
            builder.Length = 0;

            for (int i = 0; i < 999; i++)
            {
                builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
            }
            alarmsettingrules.Alarm_Description = builder.ToString();
            builder.Length = 0;

            SingleMessage<bool> ret = service.AlarmSettingAdd(alarmsettingrules);
            //Assert.IsFalse(ret.IsSuccess);
        }


        /// <summary>
        /// 测试Alarm_CreateTime中文超长
        /// </summary>
        [TestMethod]
        public void TestAlarm_CreateTimeStringForOverLength_Chinese()
        {
            AlarmSettingRules alarmsettingrules = new AlarmSettingRules();
            CommandManageService service = new CommandManageService();
            StringBuilder builder = new StringBuilder();
            Random random = new Random();
            for (int i = 0; i < 17; i++)
            {
                builder.Append(chinesecharacter[random.Next(chinesecharacter.Length - 1)]);
            }
            alarmsettingrules.Alarm_RuleID = builder.ToString();
            builder.Length = 0;

            for (int i = 0; i < 255; i++)
            {
                builder.Append(chinesecharacter[random.Next(chinesecharacter.Length - 1)]);
            }
            alarmsettingrules.Alarm_RuleName = builder.ToString();
            builder.Length = 0;

            for (int i = 0; i < 255; i++)
            {
                builder.Append(chinesecharacter[random.Next(chinesecharacter.Length - 1)]);
            }
            alarmsettingrules.Alarm_Creator = builder.ToString();
            builder.Length = 0;

            for (int i = 0; i < 999; i++)
            {
                builder.Append(chinesecharacter[random.Next(chinesecharacter.Length - 1)]);
            }
            alarmsettingrules.Alarm_Description = builder.ToString();
            builder.Length = 0;

            SingleMessage<bool> ret = service.AlarmSettingAdd(alarmsettingrules);
            //Assert.IsFalse(ret.IsSuccess);
        }


        /// <summary>
        /// 测试Alarm_CreateTime西文超长
        /// </summary>
        [TestMethod]
        public void TestAlarm_CreateTimeStringForMaxLength_Spanish()
        {
            AlarmSettingRules alarmsettingrules = new AlarmSettingRules();
            CommandManageService service = new CommandManageService();
            StringBuilder builder = new StringBuilder();
            Random random = new Random();
            for (int i = 0; i < 17; i++)
            {
                builder.Append(spanishcharacter[random.Next(spanishcharacter.Length - 1)]);
            }
            alarmsettingrules.Alarm_RuleID = builder.ToString();
            builder.Length = 0;

            for (int i = 0; i < 255; i++)
            {
                builder.Append(spanishcharacter[random.Next(spanishcharacter.Length - 1)]);
            }
            alarmsettingrules.Alarm_RuleName = builder.ToString();
            builder.Length = 0;

            for (int i = 0; i < 255; i++)
            {
                builder.Append(spanishcharacter[random.Next(spanishcharacter.Length - 1)]);
            }
            alarmsettingrules.Alarm_Creator = builder.ToString();
            builder.Length = 0;

            for (int i = 0; i < 999; i++)
            {
                builder.Append(spanishcharacter[random.Next(spanishcharacter.Length - 1)]);
            }
            alarmsettingrules.Alarm_Description = builder.ToString();
            builder.Length = 0;

            SingleMessage<bool> ret = service.AlarmSettingAdd(alarmsettingrules);
            //Assert.IsFalse(ret.IsSuccess);
        }


        /// <summary>
        /// 测试Alarm_CreateTime英文超长
        /// </summary>
        [TestMethod]
        public void TestAlarm_CreateTimeStringForMaxLength_English()
        {
            AlarmSettingRules alarmsettingrules = new AlarmSettingRules();
            CommandManageService service = new CommandManageService();
            StringBuilder builder = new StringBuilder();
            Random random = new Random();
            for (int i = 0; i < 17; i++)
            {
                builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
            }
            alarmsettingrules.Alarm_RuleID = builder.ToString();
            builder.Length = 0;

            for (int i = 0; i < 255; i++)
            {
                builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
            }
            alarmsettingrules.Alarm_RuleName = builder.ToString();
            builder.Length = 0;

            for (int i = 0; i < 255; i++)
            {
                builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
            }
            alarmsettingrules.Alarm_Creator = builder.ToString();
            builder.Length = 0;

            for (int i = 0; i < 999; i++)
            {
                builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
            }
            alarmsettingrules.Alarm_Description = builder.ToString();
            builder.Length = 0;

            SingleMessage<bool> ret = service.AlarmSettingAdd(alarmsettingrules);
            //Assert.IsFalse(ret.IsSuccess);
        }


        /// <summary>
        /// 测试Alarm_Description中文超长
        /// </summary>
        [TestMethod]
        public void TestAlarm_DescriptionStringForOverLength_Chinese()
        {
            AlarmSettingRules alarmsettingrules = new AlarmSettingRules();
            CommandManageService service = new CommandManageService();
            StringBuilder builder = new StringBuilder();
            Random random = new Random();
            for (int i = 0; i < 17; i++)
            {
                builder.Append(chinesecharacter[random.Next(chinesecharacter.Length - 1)]);
            }
            alarmsettingrules.Alarm_RuleID = builder.ToString();
            builder.Length = 0;

            for (int i = 0; i < 255; i++)
            {
                builder.Append(chinesecharacter[random.Next(chinesecharacter.Length - 1)]);
            }
            alarmsettingrules.Alarm_RuleName = builder.ToString();
            builder.Length = 0;

            for (int i = 0; i < 255; i++)
            {
                builder.Append(chinesecharacter[random.Next(chinesecharacter.Length - 1)]);
            }
            alarmsettingrules.Alarm_Creator = builder.ToString();
            builder.Length = 0;

            for (int i = 0; i < 1000; i++)
            {
                builder.Append(chinesecharacter[random.Next(chinesecharacter.Length - 1)]);
            }
            alarmsettingrules.Alarm_Description = builder.ToString();
            builder.Length = 0;

            SingleMessage<bool> ret = service.AlarmSettingAdd(alarmsettingrules);
            //Assert.IsFalse(ret.IsSuccess);
        }


        /// <summary>
        /// 测试Alarm_Description西文超长
        /// </summary>
        [TestMethod]
        public void TestAlarm_DescriptionStringForMaxLength_Spanish()
        {
            AlarmSettingRules alarmsettingrules = new AlarmSettingRules();
            CommandManageService service = new CommandManageService();
            StringBuilder builder = new StringBuilder();
            Random random = new Random();
            for (int i = 0; i < 17; i++)
            {
                builder.Append(spanishcharacter[random.Next(spanishcharacter.Length - 1)]);
            }
            alarmsettingrules.Alarm_RuleID = builder.ToString();
            builder.Length = 0;

            for (int i = 0; i < 255; i++)
            {
                builder.Append(spanishcharacter[random.Next(spanishcharacter.Length - 1)]);
            }
            alarmsettingrules.Alarm_RuleName = builder.ToString();
            builder.Length = 0;

            for (int i = 0; i < 255; i++)
            {
                builder.Append(spanishcharacter[random.Next(spanishcharacter.Length - 1)]);
            }
            alarmsettingrules.Alarm_Creator = builder.ToString();
            builder.Length = 0;

            for (int i = 0; i < 1000; i++)
            {
                builder.Append(spanishcharacter[random.Next(spanishcharacter.Length - 1)]);
            }
            alarmsettingrules.Alarm_Description = builder.ToString();
            builder.Length = 0;

            SingleMessage<bool> ret = service.AlarmSettingAdd(alarmsettingrules);
            //Assert.IsFalse(ret.IsSuccess);
        }


        /// <summary>
        /// 测试Alarm_Description英文超长
        /// </summary>
        [TestMethod]
        public void TestAlarm_DescriptionStringForMaxLength_English()
        {
            AlarmSettingRules alarmsettingrules = new AlarmSettingRules();
            CommandManageService service = new CommandManageService();
            StringBuilder builder = new StringBuilder();
            Random random = new Random();
            for (int i = 0; i < 17; i++)
            {
                builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
            }
            alarmsettingrules.Alarm_RuleID = builder.ToString();
            builder.Length = 0;

            for (int i = 0; i < 255; i++)
            {
                builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
            }
            alarmsettingrules.Alarm_RuleName = builder.ToString();
            builder.Length = 0;

            for (int i = 0; i < 255; i++)
            {
                builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
            }
            alarmsettingrules.Alarm_Creator = builder.ToString();
            builder.Length = 0;

            for (int i = 0; i < 1000; i++)
            {
                builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
            }
            alarmsettingrules.Alarm_Description = builder.ToString();
            builder.Length = 0;

            SingleMessage<bool> ret = service.AlarmSettingAdd(alarmsettingrules);
            //Assert.IsFalse(ret.IsSuccess);
        }


        /// <summary>
        /// 测试Alarm_VehcileCount中文超长
        /// </summary>
        [TestMethod]
        public void TestAlarm_VehcileCountStringForOverLength_Chinese()
        {
            AlarmSettingRules alarmsettingrules = new AlarmSettingRules();
            CommandManageService service = new CommandManageService();
            StringBuilder builder = new StringBuilder();
            Random random = new Random();
            for (int i = 0; i < 17; i++)
            {
                builder.Append(chinesecharacter[random.Next(chinesecharacter.Length - 1)]);
            }
            alarmsettingrules.Alarm_RuleID = builder.ToString();
            builder.Length = 0;

            for (int i = 0; i < 255; i++)
            {
                builder.Append(chinesecharacter[random.Next(chinesecharacter.Length - 1)]);
            }
            alarmsettingrules.Alarm_RuleName = builder.ToString();
            builder.Length = 0;

            for (int i = 0; i < 255; i++)
            {
                builder.Append(chinesecharacter[random.Next(chinesecharacter.Length - 1)]);
            }
            alarmsettingrules.Alarm_Creator = builder.ToString();
            builder.Length = 0;

            for (int i = 0; i < 999; i++)
            {
                builder.Append(chinesecharacter[random.Next(chinesecharacter.Length - 1)]);
            }
            alarmsettingrules.Alarm_Description = builder.ToString();
            builder.Length = 0;

            SingleMessage<bool> ret = service.AlarmSettingAdd(alarmsettingrules);
            //Assert.IsFalse(ret.IsSuccess);
        }


        /// <summary>
        /// 测试Alarm_VehcileCount西文超长
        /// </summary>
        [TestMethod]
        public void TestAlarm_VehcileCountStringForMaxLength_Spanish()
        {
            AlarmSettingRules alarmsettingrules = new AlarmSettingRules();
            CommandManageService service = new CommandManageService();
            StringBuilder builder = new StringBuilder();
            Random random = new Random();
            for (int i = 0; i < 17; i++)
            {
                builder.Append(spanishcharacter[random.Next(spanishcharacter.Length - 1)]);
            }
            alarmsettingrules.Alarm_RuleID = builder.ToString();
            builder.Length = 0;

            for (int i = 0; i < 255; i++)
            {
                builder.Append(spanishcharacter[random.Next(spanishcharacter.Length - 1)]);
            }
            alarmsettingrules.Alarm_RuleName = builder.ToString();
            builder.Length = 0;

            for (int i = 0; i < 255; i++)
            {
                builder.Append(spanishcharacter[random.Next(spanishcharacter.Length - 1)]);
            }
            alarmsettingrules.Alarm_Creator = builder.ToString();
            builder.Length = 0;

            for (int i = 0; i < 999; i++)
            {
                builder.Append(spanishcharacter[random.Next(spanishcharacter.Length - 1)]);
            }
            alarmsettingrules.Alarm_Description = builder.ToString();
            builder.Length = 0;

            SingleMessage<bool> ret = service.AlarmSettingAdd(alarmsettingrules);
            //Assert.IsFalse(ret.IsSuccess);
        }


        /// <summary>
        /// 测试Alarm_VehcileCount英文超长
        /// </summary>
        [TestMethod]
        public void TestAlarm_VehcileCountStringForMaxLength_English()
        {
            AlarmSettingRules alarmsettingrules = new AlarmSettingRules();
            CommandManageService service = new CommandManageService();
            StringBuilder builder = new StringBuilder();
            Random random = new Random();
            for (int i = 0; i < 17; i++)
            {
                builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
            }
            alarmsettingrules.Alarm_RuleID = builder.ToString();
            builder.Length = 0;

            for (int i = 0; i < 255; i++)
            {
                builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
            }
            alarmsettingrules.Alarm_RuleName = builder.ToString();
            builder.Length = 0;

            for (int i = 0; i < 255; i++)
            {
                builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
            }
            alarmsettingrules.Alarm_Creator = builder.ToString();
            builder.Length = 0;

            for (int i = 0; i < 999; i++)
            {
                builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
            }
            alarmsettingrules.Alarm_Description = builder.ToString();
            builder.Length = 0;

            SingleMessage<bool> ret = service.AlarmSettingAdd(alarmsettingrules);
            //Assert.IsFalse(ret.IsSuccess);
        }


        /// <summary>
        /// 测试Alarm_IsDefault中文超长
        /// </summary>
        [TestMethod]
        public void TestAlarm_IsDefaultStringForOverLength_Chinese()
        {
            AlarmSettingRules alarmsettingrules = new AlarmSettingRules();
            CommandManageService service = new CommandManageService();
            StringBuilder builder = new StringBuilder();
            Random random = new Random();
            for (int i = 0; i < 17; i++)
            {
                builder.Append(chinesecharacter[random.Next(chinesecharacter.Length - 1)]);
            }
            alarmsettingrules.Alarm_RuleID = builder.ToString();
            builder.Length = 0;

            for (int i = 0; i < 255; i++)
            {
                builder.Append(chinesecharacter[random.Next(chinesecharacter.Length - 1)]);
            }
            alarmsettingrules.Alarm_RuleName = builder.ToString();
            builder.Length = 0;

            for (int i = 0; i < 255; i++)
            {
                builder.Append(chinesecharacter[random.Next(chinesecharacter.Length - 1)]);
            }
            alarmsettingrules.Alarm_Creator = builder.ToString();
            builder.Length = 0;

            for (int i = 0; i < 999; i++)
            {
                builder.Append(chinesecharacter[random.Next(chinesecharacter.Length - 1)]);
            }
            alarmsettingrules.Alarm_Description = builder.ToString();
            builder.Length = 0;

            SingleMessage<bool> ret = service.AlarmSettingAdd(alarmsettingrules);
            //Assert.IsFalse(ret.IsSuccess);
        }


        /// <summary>
        /// 测试Alarm_IsDefault西文超长
        /// </summary>
        [TestMethod]
        public void TestAlarm_IsDefaultStringForMaxLength_Spanish()
        {
            AlarmSettingRules alarmsettingrules = new AlarmSettingRules();
            CommandManageService service = new CommandManageService();
            StringBuilder builder = new StringBuilder();
            Random random = new Random();
            for (int i = 0; i < 17; i++)
            {
                builder.Append(spanishcharacter[random.Next(spanishcharacter.Length - 1)]);
            }
            alarmsettingrules.Alarm_RuleID = builder.ToString();
            builder.Length = 0;

            for (int i = 0; i < 255; i++)
            {
                builder.Append(spanishcharacter[random.Next(spanishcharacter.Length - 1)]);
            }
            alarmsettingrules.Alarm_RuleName = builder.ToString();
            builder.Length = 0;

            for (int i = 0; i < 255; i++)
            {
                builder.Append(spanishcharacter[random.Next(spanishcharacter.Length - 1)]);
            }
            alarmsettingrules.Alarm_Creator = builder.ToString();
            builder.Length = 0;

            for (int i = 0; i < 999; i++)
            {
                builder.Append(spanishcharacter[random.Next(spanishcharacter.Length - 1)]);
            }
            alarmsettingrules.Alarm_Description = builder.ToString();
            builder.Length = 0;

            SingleMessage<bool> ret = service.AlarmSettingAdd(alarmsettingrules);
            //Assert.IsFalse(ret.IsSuccess);
        }


        /// <summary>
        /// 测试Alarm_IsDefault英文超长
        /// </summary>
        [TestMethod]
        public void TestAlarm_IsDefaultStringForMaxLength_English()
        {
            AlarmSettingRules alarmsettingrules = new AlarmSettingRules();
            CommandManageService service = new CommandManageService();
            StringBuilder builder = new StringBuilder();
            Random random = new Random();
            for (int i = 0; i < 17; i++)
            {
                builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
            }
            alarmsettingrules.Alarm_RuleID = builder.ToString();
            builder.Length = 0;

            for (int i = 0; i < 255; i++)
            {
                builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
            }
            alarmsettingrules.Alarm_RuleName = builder.ToString();
            builder.Length = 0;

            for (int i = 0; i < 255; i++)
            {
                builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
            }
            alarmsettingrules.Alarm_Creator = builder.ToString();
            builder.Length = 0;

            for (int i = 0; i < 999; i++)
            {
                builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
            }
            alarmsettingrules.Alarm_Description = builder.ToString();
            builder.Length = 0;

            SingleMessage<bool> ret = service.AlarmSettingAdd(alarmsettingrules);
            //Assert.IsFalse(ret.IsSuccess);
        }


        /// <summary>
        /// 测试Alarm_Valid中文超长
        /// </summary>
        [TestMethod]
        public void TestAlarm_ValidStringForOverLength_Chinese()
        {
            AlarmSettingRules alarmsettingrules = new AlarmSettingRules();
            CommandManageService service = new CommandManageService();
            StringBuilder builder = new StringBuilder();
            Random random = new Random();
            for (int i = 0; i < 17; i++)
            {
                builder.Append(chinesecharacter[random.Next(chinesecharacter.Length - 1)]);
            }
            alarmsettingrules.Alarm_RuleID = builder.ToString();
            builder.Length = 0;

            for (int i = 0; i < 255; i++)
            {
                builder.Append(chinesecharacter[random.Next(chinesecharacter.Length - 1)]);
            }
            alarmsettingrules.Alarm_RuleName = builder.ToString();
            builder.Length = 0;

            for (int i = 0; i < 255; i++)
            {
                builder.Append(chinesecharacter[random.Next(chinesecharacter.Length - 1)]);
            }
            alarmsettingrules.Alarm_Creator = builder.ToString();
            builder.Length = 0;

            for (int i = 0; i < 999; i++)
            {
                builder.Append(chinesecharacter[random.Next(chinesecharacter.Length - 1)]);
            }
            alarmsettingrules.Alarm_Description = builder.ToString();
            builder.Length = 0;

            SingleMessage<bool> ret = service.AlarmSettingAdd(alarmsettingrules);
            //Assert.IsFalse(ret.IsSuccess);
        }


        /// <summary>
        /// 测试Alarm_Valid西文超长
        /// </summary>
        [TestMethod]
        public void TestAlarm_ValidStringForMaxLength_Spanish()
        {
            AlarmSettingRules alarmsettingrules = new AlarmSettingRules();
            CommandManageService service = new CommandManageService();
            StringBuilder builder = new StringBuilder();
            Random random = new Random();
            for (int i = 0; i < 17; i++)
            {
                builder.Append(spanishcharacter[random.Next(spanishcharacter.Length - 1)]);
            }
            alarmsettingrules.Alarm_RuleID = builder.ToString();
            builder.Length = 0;

            for (int i = 0; i < 255; i++)
            {
                builder.Append(spanishcharacter[random.Next(spanishcharacter.Length - 1)]);
            }
            alarmsettingrules.Alarm_RuleName = builder.ToString();
            builder.Length = 0;

            for (int i = 0; i < 255; i++)
            {
                builder.Append(spanishcharacter[random.Next(spanishcharacter.Length - 1)]);
            }
            alarmsettingrules.Alarm_Creator = builder.ToString();
            builder.Length = 0;

            for (int i = 0; i < 999; i++)
            {
                builder.Append(spanishcharacter[random.Next(spanishcharacter.Length - 1)]);
            }
            alarmsettingrules.Alarm_Description = builder.ToString();
            builder.Length = 0;

            SingleMessage<bool> ret = service.AlarmSettingAdd(alarmsettingrules);
            //Assert.IsFalse(ret.IsSuccess);
        }


        /// <summary>
        /// 测试Alarm_Valid英文超长
        /// </summary>
        [TestMethod]
        public void TestAlarm_ValidStringForMaxLength_English()
        {
            AlarmSettingRules alarmsettingrules = new AlarmSettingRules();
            CommandManageService service = new CommandManageService();
            StringBuilder builder = new StringBuilder();
            Random random = new Random();
            for (int i = 0; i < 17; i++)
            {
                builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
            }
            alarmsettingrules.Alarm_RuleID = builder.ToString();
            builder.Length = 0;

            for (int i = 0; i < 255; i++)
            {
                builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
            }
            alarmsettingrules.Alarm_RuleName = builder.ToString();
            builder.Length = 0;

            for (int i = 0; i < 255; i++)
            {
                builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
            }
            alarmsettingrules.Alarm_Creator = builder.ToString();
            builder.Length = 0;

            for (int i = 0; i < 999; i++)
            {
                builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
            }
            alarmsettingrules.Alarm_Description = builder.ToString();
            builder.Length = 0;

            SingleMessage<bool> ret = service.AlarmSettingAdd(alarmsettingrules);
            //Assert.IsFalse(ret.IsSuccess);
        }


        /// <summary>
        /// 测试空格
        /// </summary>
        [TestMethod]
        public void TestStringForSpace()
        {
            AlarmSettingRules alarmsettingrules = new AlarmSettingRules();
            CommandManageService service = new CommandManageService();
            Random random = new Random();
            StringBuilder builder = new StringBuilder();
            builder.AppendLine("			Random random = new Random();");
            builder.Append(" ");
            for (int i = 1; i < 17; i++)
            {
                builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
            }
            builder.Append(" ");
            alarmsettingrules.Alarm_RuleID = builder.ToString();
            builder.Length = 0;

            builder.Append(" ");
            for (int i = 1; i < 255; i++)
            {
                builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
            }
            builder.Append(" ");
            alarmsettingrules.Alarm_RuleName = builder.ToString();
            builder.Length = 0;

            builder.Append(" ");
            for (int i = 1; i < 255; i++)
            {
                builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
            }
            builder.Append(" ");
            alarmsettingrules.Alarm_Creator = builder.ToString();
            builder.Length = 0;

            builder.Append(" ");
            for (int i = 1; i < 999; i++)
            {
                builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
            }
            builder.Append(" ");
            alarmsettingrules.Alarm_Description = builder.ToString();
            builder.Length = 0;

            SingleMessage<bool> ret = service.AlarmSettingAdd(alarmsettingrules);
            //Assert.IsFalse(ret.IsSuccess);
        }


        /// <summary>
        /// 测试,
        /// </summary>
        [TestMethod]
        public void TestStringForSpecialCharacter_Comma()
        {
            AlarmSettingRules alarmsettingrules = new AlarmSettingRules();
            CommandManageService service = new CommandManageService();
            Random random = new Random();
            StringBuilder builder = new StringBuilder();
            builder.AppendLine("			Random random = new Random();");
            builder.Append(",");
            for (int i = 1; i < 17; i++)
            {
                builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
            }
            builder.Append(",");
            alarmsettingrules.Alarm_RuleID = builder.ToString();
            builder.Length = 0;

            builder.Append(",");
            for (int i = 1; i < 255; i++)
            {
                builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
            }
            builder.Append(",");
            alarmsettingrules.Alarm_RuleName = builder.ToString();
            builder.Length = 0;

            builder.Append(",");
            for (int i = 1; i < 255; i++)
            {
                builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
            }
            builder.Append(",");
            alarmsettingrules.Alarm_Creator = builder.ToString();
            builder.Length = 0;

            builder.Append(",");
            for (int i = 1; i < 999; i++)
            {
                builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
            }
            builder.Append(",");
            alarmsettingrules.Alarm_Description = builder.ToString();
            builder.Length = 0;

            SingleMessage<bool> ret = service.AlarmSettingAdd(alarmsettingrules);
            //Assert.IsFalse(ret.IsSuccess);
        }


        /// <summary>
        /// 测试;
        /// </summary>
        [TestMethod]
        public void TestStringForSpecialCharacter_SemiComma()
        {
            AlarmSettingRules alarmsettingrules = new AlarmSettingRules();
            CommandManageService service = new CommandManageService();
            Random random = new Random();
            StringBuilder builder = new StringBuilder();
            builder.AppendLine("			Random random = new Random();");
            builder.Append(";");
            for (int i = 1; i < 17; i++)
            {
                builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
            }
            builder.Append(";");
            alarmsettingrules.Alarm_RuleID = builder.ToString();
            builder.Length = 0;

            builder.Append(";");
            for (int i = 1; i < 255; i++)
            {
                builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
            }
            builder.Append(";");
            alarmsettingrules.Alarm_RuleName = builder.ToString();
            builder.Length = 0;

            builder.Append(";");
            for (int i = 1; i < 255; i++)
            {
                builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
            }
            builder.Append(";");
            alarmsettingrules.Alarm_Creator = builder.ToString();
            builder.Length = 0;

            builder.Append(";");
            for (int i = 1; i < 999; i++)
            {
                builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
            }
            builder.Append(";");
            alarmsettingrules.Alarm_Description = builder.ToString();
            builder.Length = 0;

            SingleMessage<bool> ret = service.AlarmSettingAdd(alarmsettingrules);
            //Assert.IsFalse(ret.IsSuccess);
        }


        //        /// <summary>
        //        /// 测试;
        //        /// </summary>
        //        [TestMethod]
        //        public void TestStringForSpecialCharacter_quote()
        //        {
        //            AlarmSettingRules alarmsettingrules = new AlarmSettingRules();
        //            CommandManageService service = new CommandManageService();
        //            StringBuilder builder = new StringBuilder();
        //            builder.AppendLine("			Random random = new Random();");
        //builder.Append(""");
        //            for (int i = 1; i < 17; i++)
        //            {
        //                builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
        //            }
        //builder.Append(""");
        //            alarmsettingrules.Alarm_RuleID=builder.ToString();
        //            builder.Length=0;

        //builder.Append(""");
        //            for (int i = 1; i < 255; i++)
        //            {
        //                builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
        //            }
        //builder.Append(""");
        //            alarmsettingrules.Alarm_RuleName=builder.ToString();
        //            builder.Length=0;

        //builder.Append(""");
        //            for (int i = 1; i < 255; i++)
        //            {
        //                builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
        //            }
        //builder.Append(""");
        //            alarmsettingrules.Alarm_Creator=builder.ToString();
        //            builder.Length=0;

        //builder.Append(""");
        //            for (int i = 1; i < 999; i++)
        //            {
        //                builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
        //            }
        //builder.Append(""");
        //            alarmsettingrules.Alarm_Description=builder.ToString();
        //            builder.Length=0;

        //            SingleMessage<bool> ret = service.AlarmSettingAdd(alarmsettingrules);
        //            //Assert.IsFalse(ret.IsSuccess);
        //        }


        /// <summary>
        /// 测试select
        /// </summary>
        [TestMethod]
        public void TestStringForSpecialCharacter_quoteselect()
        {
            AlarmSettingRules alarmsettingrules = new AlarmSettingRules();
            CommandManageService service = new CommandManageService();
            alarmsettingrules.Alarm_RuleID = "select";

            alarmsettingrules.Alarm_RuleName = "select";

            alarmsettingrules.Alarm_Creator = "select";

            alarmsettingrules.Alarm_Description = "select";

            SingleMessage<bool> ret = service.AlarmSettingAdd(alarmsettingrules);
            //Assert.IsFalse(ret.IsSuccess);
        }


        /// <summary>
        /// 测试update
        /// </summary>
        [TestMethod]
        public void TestStringForSpecialCharacter_quoteupdate()
        {
            AlarmSettingRules alarmsettingrules = new AlarmSettingRules();
            CommandManageService service = new CommandManageService();
            alarmsettingrules.Alarm_RuleID = "update";

            alarmsettingrules.Alarm_RuleName = "update";

            alarmsettingrules.Alarm_Creator = "update";

            alarmsettingrules.Alarm_Description = "update";

            SingleMessage<bool> ret = service.AlarmSettingAdd(alarmsettingrules);
            //Assert.IsFalse(ret.IsSuccess);
        }


        /// <summary>
        /// 测试delete
        /// </summary>
        [TestMethod]
        public void TestStringForSpecialCharacter_quotedelete()
        {
            AlarmSettingRules alarmsettingrules = new AlarmSettingRules();
            CommandManageService service = new CommandManageService();
            alarmsettingrules.Alarm_RuleID = "delete";

            alarmsettingrules.Alarm_RuleName = "delete";

            alarmsettingrules.Alarm_Creator = "delete";

            alarmsettingrules.Alarm_Description = "delete";

            SingleMessage<bool> ret = service.AlarmSettingAdd(alarmsettingrules);
            //Assert.IsFalse(ret.IsSuccess);
        }


        /// <summary>
        /// 测试最大值
        /// </summary>
        [TestMethod]
        public void TestNumberForMax()
        {
            AlarmSettingRules alarmsettingrules = new AlarmSettingRules();
            CommandManageService service = new CommandManageService();
            alarmsettingrules.Alarm_RuleID = "";
            alarmsettingrules.Alarm_RuleName = "";
            alarmsettingrules.Alarm_Creator = "";
            alarmsettingrules.Alarm_Description = "";
            SingleMessage<bool> ret = service.AlarmSettingAdd(alarmsettingrules);
            //Assert.IsFalse(ret.IsSuccess);
        }


        /// <summary>
        /// 测试最小值
        /// </summary>
        [TestMethod]
        public void TestNumberForMin()
        {
            AlarmSettingRules alarmsettingrules = new AlarmSettingRules();
            CommandManageService service = new CommandManageService();
            alarmsettingrules.Alarm_RuleID = "";
            alarmsettingrules.Alarm_RuleName = "";
            alarmsettingrules.Alarm_Creator = "";
            alarmsettingrules.Alarm_Description = "";
            SingleMessage<bool> ret = service.AlarmSettingAdd(alarmsettingrules);
            //Assert.IsFalse(ret.IsSuccess);
        }


        /// <summary>
        /// 正数
        /// </summary>
        [TestMethod]
        public void TestNumberForPositive()
        {
            AlarmSettingRules alarmsettingrules = new AlarmSettingRules();
            CommandManageService service = new CommandManageService();
            alarmsettingrules.Alarm_RuleID = "";
            alarmsettingrules.Alarm_RuleName = "";
            alarmsettingrules.Alarm_Creator = "";
            alarmsettingrules.Alarm_Description = "";
            SingleMessage<bool> ret = service.AlarmSettingAdd(alarmsettingrules);
            //Assert.IsFalse(ret.IsSuccess);
        }


        /// <summary>
        /// 负数
        /// </summary>
        [TestMethod]
        public void TestNumberForNegative()
        {
            AlarmSettingRules alarmsettingrules = new AlarmSettingRules();
            CommandManageService service = new CommandManageService();
            alarmsettingrules.Alarm_RuleID = "";
            alarmsettingrules.Alarm_RuleName = "";
            alarmsettingrules.Alarm_Creator = "";
            alarmsettingrules.Alarm_Description = "";
            SingleMessage<bool> ret = service.AlarmSettingAdd(alarmsettingrules);
            //Assert.IsFalse(ret.IsSuccess);
        }


        /// <summary>
        /// 零
        /// </summary>
        [TestMethod]
        public void TestNumberForZero()
        {
            AlarmSettingRules alarmsettingrules = new AlarmSettingRules();
            CommandManageService service = new CommandManageService();
            alarmsettingrules.Alarm_RuleID = "";
            alarmsettingrules.Alarm_RuleName = "";
            alarmsettingrules.Alarm_Creator = "";
            alarmsettingrules.Alarm_Description = "";
            SingleMessage<bool> ret = service.AlarmSettingAdd(alarmsettingrules);
            //Assert.IsFalse(ret.IsSuccess);
        }


        /// <summary>
        /// 小数
        /// </summary>
        [TestMethod]
        public void TestNumberForPortionfloat()
        {
            AlarmSettingRules alarmsettingrules = new AlarmSettingRules();
            CommandManageService service = new CommandManageService();
            alarmsettingrules.Alarm_RuleID = "";
            alarmsettingrules.Alarm_RuleName = "";
            alarmsettingrules.Alarm_Creator = "";
            alarmsettingrules.Alarm_Description = "";
            SingleMessage<bool> ret = service.AlarmSettingAdd(alarmsettingrules);
            //Assert.IsFalse(ret.IsSuccess);
        }


        /// <summary>
        /// 正数小数
        /// </summary>
        [TestMethod]
        public void TestNumberForPortionFloatinting()
        {
            AlarmSettingRules alarmsettingrules = new AlarmSettingRules();
            CommandManageService service = new CommandManageService();
            alarmsettingrules.Alarm_RuleID = "";
            alarmsettingrules.Alarm_RuleName = "";
            alarmsettingrules.Alarm_Creator = "";
            alarmsettingrules.Alarm_Description = "";
            SingleMessage<bool> ret = service.AlarmSettingAdd(alarmsettingrules);
            //Assert.IsFalse(ret.IsSuccess);
        }


        /// <summary>
        /// 字母
        /// </summary>
        [TestMethod]
        public void TestNumberForPortionV()
        {
            AlarmSettingRules alarmsettingrules = new AlarmSettingRules();
            CommandManageService service = new CommandManageService();
            alarmsettingrules.Alarm_RuleID = "";
            alarmsettingrules.Alarm_RuleName = "";
            alarmsettingrules.Alarm_Creator = "";
            alarmsettingrules.Alarm_Description = "";
            SingleMessage<bool> ret = service.AlarmSettingAdd(alarmsettingrules);
            //Assert.IsFalse(ret.IsSuccess);
        }


        /// <summary>
        /// DateTime
        /// </summary>
        [TestMethod]
        public void TestDateTime()
        {
            AlarmSettingRules alarmsettingrules = new AlarmSettingRules();
            CommandManageService service = new CommandManageService();
            alarmsettingrules.Alarm_RuleID = "";
            alarmsettingrules.Alarm_RuleName = "";
            alarmsettingrules.Alarm_Creator = "";
            alarmsettingrules.Alarm_Description = "";
            SingleMessage<bool> ret = service.AlarmSettingAdd(alarmsettingrules);
            //Assert.IsFalse(ret.IsSuccess);
        }


        /// <summary>
        /// 日期为空
        /// </summary>
        [TestMethod]
        public void TestAlarm_RuleIDDateTimeForNull()
        {
            AlarmSettingRules alarmsettingrules = new AlarmSettingRules();
            CommandManageService service = new CommandManageService();
            alarmsettingrules.Alarm_RuleID = null;
            alarmsettingrules.Alarm_RuleName = "";
            alarmsettingrules.Alarm_Creator = "";
            alarmsettingrules.Alarm_Description = "";
            SingleMessage<bool> ret = service.AlarmSettingAdd(alarmsettingrules);
            //Assert.IsFalse(ret.IsSuccess);
        }


        /// <summary>
        /// 日期为空
        /// </summary>
        [TestMethod]
        public void TestAlarm_RuleNameDateTimeForNull()
        {
            AlarmSettingRules alarmsettingrules = new AlarmSettingRules();
            CommandManageService service = new CommandManageService();
            alarmsettingrules.Alarm_RuleID = "";
            alarmsettingrules.Alarm_RuleName = null;
            alarmsettingrules.Alarm_Creator = "";
            alarmsettingrules.Alarm_Description = "";
            SingleMessage<bool> ret = service.AlarmSettingAdd(alarmsettingrules);
            //Assert.IsFalse(ret.IsSuccess);
        }


        /// <summary>
        /// 日期为空
        /// </summary>
        [TestMethod]
        public void TestAlarm_ButtonTimeDateTimeForNull()
        {
            AlarmSettingRules alarmsettingrules = new AlarmSettingRules();
            CommandManageService service = new CommandManageService();
            alarmsettingrules.Alarm_RuleID = "";
            alarmsettingrules.Alarm_RuleName = "";
            alarmsettingrules.Alarm_ButtonTime = null;
            alarmsettingrules.Alarm_Creator = "";
            alarmsettingrules.Alarm_Description = "";
            SingleMessage<bool> ret = service.AlarmSettingAdd(alarmsettingrules);
            //Assert.IsFalse(ret.IsSuccess);
        }


        /// <summary>
        /// 日期为空
        /// </summary>
        [TestMethod]
        public void TestAlarm_NormalDateTimeForNull()
        {
            AlarmSettingRules alarmsettingrules = new AlarmSettingRules();
            CommandManageService service = new CommandManageService();
            alarmsettingrules.Alarm_RuleID = "";
            alarmsettingrules.Alarm_RuleName = "";
            alarmsettingrules.Alarm_Normal = null;
            alarmsettingrules.Alarm_Creator = "";
            alarmsettingrules.Alarm_Description = "";
            SingleMessage<bool> ret = service.AlarmSettingAdd(alarmsettingrules);
            //Assert.IsFalse(ret.IsSuccess);
        }


        /// <summary>
        /// 日期为空
        /// </summary>
        [TestMethod]
        public void TestAlarm_CreatorDateTimeForNull()
        {
            AlarmSettingRules alarmsettingrules = new AlarmSettingRules();
            CommandManageService service = new CommandManageService();
            alarmsettingrules.Alarm_RuleID = "";
            alarmsettingrules.Alarm_RuleName = "";
            alarmsettingrules.Alarm_Creator = null;
            alarmsettingrules.Alarm_Description = "";
            SingleMessage<bool> ret = service.AlarmSettingAdd(alarmsettingrules);
            //Assert.IsFalse(ret.IsSuccess);
        }


        /// <summary>
        /// 日期为空
        /// </summary>
        [TestMethod]
        public void TestAlarm_CreateTimeDateTimeForNull()
        {
            AlarmSettingRules alarmsettingrules = new AlarmSettingRules();
            CommandManageService service = new CommandManageService();
            alarmsettingrules.Alarm_RuleID = "";
            alarmsettingrules.Alarm_RuleName = "";
            alarmsettingrules.Alarm_Creator = "";
            alarmsettingrules.Alarm_CreateTime = null;
            alarmsettingrules.Alarm_Description = "";
            SingleMessage<bool> ret = service.AlarmSettingAdd(alarmsettingrules);
            //Assert.IsFalse(ret.IsSuccess);
        }


        /// <summary>
        /// 日期为空
        /// </summary>
        [TestMethod]
        public void TestAlarm_DescriptionDateTimeForNull()
        {
            AlarmSettingRules alarmsettingrules = new AlarmSettingRules();
            CommandManageService service = new CommandManageService();
            alarmsettingrules.Alarm_RuleID = "";
            alarmsettingrules.Alarm_RuleName = "";
            alarmsettingrules.Alarm_Creator = "";
            alarmsettingrules.Alarm_Description = null;
            SingleMessage<bool> ret = service.AlarmSettingAdd(alarmsettingrules);
            //Assert.IsFalse(ret.IsSuccess);
        }


        /// <summary>
        /// 日期为空,don`t test
        /// </summary>
        [TestMethod]
        public void TestAlarm_VehcileCountDateTimeForNull()
        {
            AlarmSettingRules alarmsettingrules = new AlarmSettingRules();
            CommandManageService service = new CommandManageService();
            alarmsettingrules.Alarm_RuleID = "";
            alarmsettingrules.Alarm_RuleName = "";
            alarmsettingrules.Alarm_Creator = "";
            alarmsettingrules.Alarm_Description = "";
            //alarmsettingrules.Alarm_VehcileCount=null;
            SingleMessage<bool> ret = service.AlarmSettingAdd(alarmsettingrules);
            //Assert.IsFalse(ret.IsSuccess);
        }


        /// <summary>
        /// 日期为空,don`t test
        /// </summary>
        [TestMethod]
        public void TestAlarm_IsDefaultDateTimeForNull()
        {
            AlarmSettingRules alarmsettingrules = new AlarmSettingRules();
            CommandManageService service = new CommandManageService();
            alarmsettingrules.Alarm_RuleID = "";
            alarmsettingrules.Alarm_RuleName = "";
            alarmsettingrules.Alarm_Creator = "";
            alarmsettingrules.Alarm_Description = "";
            //alarmsettingrules.Alarm_IsDefault=null;
            SingleMessage<bool> ret = service.AlarmSettingAdd(alarmsettingrules);
            //Assert.IsFalse(ret.IsSuccess);
        }


        /// <summary>
        /// 日期为空,don`t test
        /// </summary>
        [TestMethod]
        public void TestAlarm_ValidDateTimeForNull()
        {
            AlarmSettingRules alarmsettingrules = new AlarmSettingRules();
            CommandManageService service = new CommandManageService();
            alarmsettingrules.Alarm_RuleID = "";
            alarmsettingrules.Alarm_RuleName = "";
            alarmsettingrules.Alarm_Creator = "";
            alarmsettingrules.Alarm_Description = "";
            //alarmsettingrules.Alarm_Valid=null;
            SingleMessage<bool> ret = service.AlarmSettingAdd(alarmsettingrules);
            //Assert.IsFalse(ret.IsSuccess);
        }


        /// <summary>
        /// Alarm_RuleID日期为空
        /// </summary>
        [TestMethod]
        public void TestAlarm_RuleIDDateTimeForIncomplete()
        {
            AlarmSettingRules alarmsettingrules = new AlarmSettingRules();
            CommandManageService service = new CommandManageService();
            alarmsettingrules.Alarm_RuleID = DateTime.Parse("2015/02/12").ToString();
            alarmsettingrules.Alarm_RuleName = "";
            alarmsettingrules.Alarm_Creator = "";
            alarmsettingrules.Alarm_Description = "";
            SingleMessage<bool> ret = service.AlarmSettingAdd(alarmsettingrules);
            //Assert.IsFalse(ret.IsSuccess);
        }


        /// <summary>
        /// Alarm_RuleName日期为空
        /// </summary>
        [TestMethod]
        public void TestAlarm_RuleNameDateTimeForIncomplete()
        {
            AlarmSettingRules alarmsettingrules = new AlarmSettingRules();
            CommandManageService service = new CommandManageService();
            alarmsettingrules.Alarm_RuleID = "";
            alarmsettingrules.Alarm_RuleName = DateTime.Parse("2015/02/12").ToString();
            alarmsettingrules.Alarm_Creator = "";
            alarmsettingrules.Alarm_Description = "";
            SingleMessage<bool> ret = service.AlarmSettingAdd(alarmsettingrules);
            //Assert.IsFalse(ret.IsSuccess);
        }


        /// <summary>
        /// Alarm_ButtonTime日期为空
        /// </summary>
        [TestMethod]
        public void TestAlarm_ButtonTimeDateTimeForIncomplete()
        {
            AlarmSettingRules alarmsettingrules = new AlarmSettingRules();
            CommandManageService service = new CommandManageService();
            alarmsettingrules.Alarm_RuleID = "";
            alarmsettingrules.Alarm_RuleName = "";
            alarmsettingrules.Alarm_ButtonTime = short.Parse(DateTime.Parse("2015/02/12").ToString());
            alarmsettingrules.Alarm_Creator = "";
            alarmsettingrules.Alarm_Description = "";
            SingleMessage<bool> ret = service.AlarmSettingAdd(alarmsettingrules);
            //Assert.IsFalse(ret.IsSuccess);
        }


        /// <summary>
        /// Alarm_Normal日期为空
        /// </summary>
        [TestMethod]
        public void TestAlarm_NormalDateTimeForIncomplete()
        {
            AlarmSettingRules alarmsettingrules = new AlarmSettingRules();
            CommandManageService service = new CommandManageService();
            alarmsettingrules.Alarm_RuleID = "";
            alarmsettingrules.Alarm_RuleName = "";
            alarmsettingrules.Alarm_Normal = short.Parse(DateTime.Parse("2015/02/12").ToString());
            alarmsettingrules.Alarm_Creator = "";
            alarmsettingrules.Alarm_Description = "";
            SingleMessage<bool> ret = service.AlarmSettingAdd(alarmsettingrules);
            //Assert.IsFalse(ret.IsSuccess);
        }


        /// <summary>
        /// Alarm_Creator日期为空
        /// </summary>
        [TestMethod]
        public void TestAlarm_CreatorDateTimeForIncomplete()
        {
            AlarmSettingRules alarmsettingrules = new AlarmSettingRules();
            CommandManageService service = new CommandManageService();
            alarmsettingrules.Alarm_RuleID = "";
            alarmsettingrules.Alarm_RuleName = "";
            alarmsettingrules.Alarm_Creator = DateTime.Parse("2015/02/12").ToString();
            alarmsettingrules.Alarm_Description = "";
            SingleMessage<bool> ret = service.AlarmSettingAdd(alarmsettingrules);
            //Assert.IsFalse(ret.IsSuccess);
        }


        /// <summary>
        /// Alarm_CreateTime日期为空
        /// </summary>
        [TestMethod]
        public void TestAlarm_CreateTimeDateTimeForIncomplete()
        {
            AlarmSettingRules alarmsettingrules = new AlarmSettingRules();
            CommandManageService service = new CommandManageService();
            alarmsettingrules.Alarm_RuleID = "";
            alarmsettingrules.Alarm_RuleName = "";
            alarmsettingrules.Alarm_Creator = "";
            alarmsettingrules.Alarm_CreateTime = DateTime.Parse("2015/02/12");
            alarmsettingrules.Alarm_Description = "";
            SingleMessage<bool> ret = service.AlarmSettingAdd(alarmsettingrules);
            //Assert.IsFalse(ret.IsSuccess);
        }


        /// <summary>
        /// Alarm_Description日期为空
        /// </summary>
        [TestMethod]
        public void TestAlarm_DescriptionDateTimeForIncomplete()
        {
            AlarmSettingRules alarmsettingrules = new AlarmSettingRules();
            CommandManageService service = new CommandManageService();
            alarmsettingrules.Alarm_RuleID = "";
            alarmsettingrules.Alarm_RuleName = "";
            alarmsettingrules.Alarm_Creator = "";
            alarmsettingrules.Alarm_Description = DateTime.Parse("2015/02/12").ToString();
            SingleMessage<bool> ret = service.AlarmSettingAdd(alarmsettingrules);
            //Assert.IsFalse(ret.IsSuccess);
        }


        /// <summary>
        /// Alarm_VehcileCount日期为空
        /// </summary>
        [TestMethod]
        public void TestAlarm_VehcileCountDateTimeForIncomplete()
        {
            AlarmSettingRules alarmsettingrules = new AlarmSettingRules();
            CommandManageService service = new CommandManageService();
            alarmsettingrules.Alarm_RuleID = "";
            alarmsettingrules.Alarm_RuleName = "";
            alarmsettingrules.Alarm_Creator = "";
            alarmsettingrules.Alarm_Description = "";
            alarmsettingrules.Alarm_VehcileCount = int.Parse(DateTime.Parse("2015/02/12").ToString());
            SingleMessage<bool> ret = service.AlarmSettingAdd(alarmsettingrules);
            //Assert.IsFalse(ret.IsSuccess);
        }


        /// <summary>
        /// Alarm_IsDefault日期为空
        /// </summary>
        [TestMethod]
        public void TestAlarm_IsDefaultDateTimeForIncomplete()
        {
            AlarmSettingRules alarmsettingrules = new AlarmSettingRules();
            CommandManageService service = new CommandManageService();
            alarmsettingrules.Alarm_RuleID = "";
            alarmsettingrules.Alarm_RuleName = "";
            alarmsettingrules.Alarm_Creator = "";
            alarmsettingrules.Alarm_Description = "";
            alarmsettingrules.Alarm_IsDefault = short.Parse(DateTime.Parse("2015/02/12").ToString());
            SingleMessage<bool> ret = service.AlarmSettingAdd(alarmsettingrules);
            //Assert.IsFalse(ret.IsSuccess);
        }


        /// <summary>
        /// Alarm_Valid日期为空
        /// </summary>
        [TestMethod]
        public void TestAlarm_ValidDateTimeForIncomplete()
        {
            AlarmSettingRules alarmsettingrules = new AlarmSettingRules();
            CommandManageService service = new CommandManageService();
            alarmsettingrules.Alarm_RuleID = "";
            alarmsettingrules.Alarm_RuleName = "";
            alarmsettingrules.Alarm_Creator = "";
            alarmsettingrules.Alarm_Description = "";
            alarmsettingrules.Alarm_Valid = short.Parse(DateTime.Parse("2015/02/12").ToString());
            SingleMessage<bool> ret = service.AlarmSettingAdd(alarmsettingrules);
            //Assert.IsFalse(ret.IsSuccess);
        }


        /// <summary>
        /// 测试Alarm_RuleID日期格式错误
        /// </summary>
        [TestMethod]
        public void TestAlarm_RuleIDDateTimeForIncompleteAlarm_RuleID()
        {
            AlarmSettingRules alarmsettingrules = new AlarmSettingRules();
            CommandManageService service = new CommandManageService();
            alarmsettingrules.Alarm_RuleID = DateTime.Parse("2015/33/33").ToString();
            alarmsettingrules.Alarm_RuleName = "";
            alarmsettingrules.Alarm_Creator = "";
            alarmsettingrules.Alarm_Description = "";
            SingleMessage<bool> ret = service.AlarmSettingAdd(alarmsettingrules);
            //Assert.IsFalse(ret.IsSuccess);
        }


        /// <summary>
        /// 测试Alarm_RuleName日期格式错误
        /// </summary>
        [TestMethod]
        public void TestAlarm_RuleNameDateTimeForIncompleteAlarm_RuleName()
        {
            AlarmSettingRules alarmsettingrules = new AlarmSettingRules();
            CommandManageService service = new CommandManageService();
            alarmsettingrules.Alarm_RuleID = "";
            alarmsettingrules.Alarm_RuleName = DateTime.Parse("2015/33/33").ToString();
            alarmsettingrules.Alarm_Creator = "";
            alarmsettingrules.Alarm_Description = "";
            SingleMessage<bool> ret = service.AlarmSettingAdd(alarmsettingrules);
            //Assert.IsFalse(ret.IsSuccess);
        }


        /// <summary>
        /// 测试Alarm_ButtonTime日期格式错误
        /// </summary>
        [TestMethod]
        public void TestAlarm_ButtonTimeDateTimeForIncompleteAlarm_ButtonTime()
        {
            AlarmSettingRules alarmsettingrules = new AlarmSettingRules();
            CommandManageService service = new CommandManageService();
            alarmsettingrules.Alarm_RuleID = "";
            alarmsettingrules.Alarm_RuleName = "";
            alarmsettingrules.Alarm_ButtonTime = short.Parse(DateTime.Parse("2015/33/33").ToString());
            alarmsettingrules.Alarm_Creator = "";
            alarmsettingrules.Alarm_Description = "";
            SingleMessage<bool> ret = service.AlarmSettingAdd(alarmsettingrules);
            //Assert.IsFalse(ret.IsSuccess);
        }


        /// <summary>
        /// 测试Alarm_Normal日期格式错误
        /// </summary>
        [TestMethod]
        public void TestAlarm_NormalDateTimeForIncompleteAlarm_Normal()
        {
            AlarmSettingRules alarmsettingrules = new AlarmSettingRules();
            CommandManageService service = new CommandManageService();
            alarmsettingrules.Alarm_RuleID = "";
            alarmsettingrules.Alarm_RuleName = "";
            alarmsettingrules.Alarm_Normal = int.Parse(DateTime.Parse("2015/33/33").ToString());
            alarmsettingrules.Alarm_Creator = "";
            alarmsettingrules.Alarm_Description = "";
            SingleMessage<bool> ret = service.AlarmSettingAdd(alarmsettingrules);
            //Assert.IsFalse(ret.IsSuccess);
        }


        /// <summary>
        /// 测试Alarm_Creator日期格式错误
        /// </summary>
        [TestMethod]
        public void TestAlarm_CreatorDateTimeForIncompleteAlarm_Creator()
        {
            AlarmSettingRules alarmsettingrules = new AlarmSettingRules();
            CommandManageService service = new CommandManageService();
            alarmsettingrules.Alarm_RuleID = "";
            alarmsettingrules.Alarm_RuleName = "";
            alarmsettingrules.Alarm_Creator = DateTime.Parse("2015/33/33").ToString();
            alarmsettingrules.Alarm_Description = "";
            SingleMessage<bool> ret = service.AlarmSettingAdd(alarmsettingrules);
            //Assert.IsFalse(ret.IsSuccess);
        }


        /// <summary>
        /// 测试Alarm_CreateTime日期格式错误
        /// </summary>
        [TestMethod]
        public void TestAlarm_CreateTimeDateTimeForIncompleteAlarm_CreateTime()
        {
            AlarmSettingRules alarmsettingrules = new AlarmSettingRules();
            CommandManageService service = new CommandManageService();
            alarmsettingrules.Alarm_RuleID = "";
            alarmsettingrules.Alarm_RuleName = "";
            alarmsettingrules.Alarm_Creator = "";
            alarmsettingrules.Alarm_CreateTime = DateTime.Parse("2015/33/33");
            alarmsettingrules.Alarm_Description = "";
            SingleMessage<bool> ret = service.AlarmSettingAdd(alarmsettingrules);
            //Assert.IsFalse(ret.IsSuccess);
        }


        /// <summary>
        /// 测试Alarm_Description日期格式错误
        /// </summary>
        [TestMethod]
        public void TestAlarm_DescriptionDateTimeForIncompleteAlarm_Description()
        {
            AlarmSettingRules alarmsettingrules = new AlarmSettingRules();
            CommandManageService service = new CommandManageService();
            alarmsettingrules.Alarm_RuleID = "";
            alarmsettingrules.Alarm_RuleName = "";
            alarmsettingrules.Alarm_Creator = "";
            alarmsettingrules.Alarm_Description = DateTime.Parse("2015/33/33").ToString();
            SingleMessage<bool> ret = service.AlarmSettingAdd(alarmsettingrules);
            //Assert.IsFalse(ret.IsSuccess);
        }


        /// <summary>
        /// 测试Alarm_VehcileCount日期格式错误
        /// </summary>
        [TestMethod]
        public void TestAlarm_VehcileCountDateTimeForIncompleteAlarm_VehcileCount()
        {
            AlarmSettingRules alarmsettingrules = new AlarmSettingRules();
            CommandManageService service = new CommandManageService();
            alarmsettingrules.Alarm_RuleID = "";
            alarmsettingrules.Alarm_RuleName = "";
            alarmsettingrules.Alarm_Creator = "";
            alarmsettingrules.Alarm_Description = "";
            alarmsettingrules.Alarm_VehcileCount = int.Parse(DateTime.Parse("2015/33/33").ToString());
            SingleMessage<bool> ret = service.AlarmSettingAdd(alarmsettingrules);
            //Assert.IsFalse(ret.IsSuccess);
        }


        /// <summary>
        /// 测试Alarm_IsDefault日期格式错误
        /// </summary>
        [TestMethod]
        public void TestAlarm_IsDefaultDateTimeForIncompleteIsDefault()
        {
            AlarmSettingRules alarmsettingrules = new AlarmSettingRules();
            CommandManageService service = new CommandManageService();
            alarmsettingrules.Alarm_RuleID = "";
            alarmsettingrules.Alarm_RuleName = "";
            alarmsettingrules.Alarm_Creator = "";
            alarmsettingrules.Alarm_Description = "";
            alarmsettingrules.Alarm_IsDefault = short.Parse(DateTime.Parse("2015/33/33").ToString());
            SingleMessage<bool> ret = service.AlarmSettingAdd(alarmsettingrules);
            //Assert.IsFalse(ret.IsSuccess);
        }


        /// <summary>
        /// 测试Alarm_Valid日期格式错误
        /// </summary>
        [TestMethod]
        public void TestAlarm_ValidDateTimeForIncompleteValid()
        {
            AlarmSettingRules alarmsettingrules = new AlarmSettingRules();
            CommandManageService service = new CommandManageService();
            alarmsettingrules.Alarm_RuleID = "";
            alarmsettingrules.Alarm_RuleName = "";
            alarmsettingrules.Alarm_Creator = "";
            alarmsettingrules.Alarm_Description = "";
            alarmsettingrules.Alarm_Valid = short.Parse(DateTime.Parse("2015/33/33").ToString());
            SingleMessage<bool> ret = service.AlarmSettingAdd(alarmsettingrules);
            //Assert.IsFalse(ret.IsSuccess);
        }


        /// <summary>
        /// 测试Alarm_RuleID日期最大值
        /// </summary>
        [TestMethod]
        public void TestAlarm_RuleIDDateTimeForMax()
        {
            AlarmSettingRules alarmsettingrules = new AlarmSettingRules();
            CommandManageService service = new CommandManageService();
            alarmsettingrules.Alarm_RuleID = DateTime.MaxValue.ToString();
            alarmsettingrules.Alarm_RuleName = "";
            alarmsettingrules.Alarm_Creator = "";
            alarmsettingrules.Alarm_Description = "";
            SingleMessage<bool> ret = service.AlarmSettingAdd(alarmsettingrules);
            //Assert.IsFalse(ret.IsSuccess);
        }


        /// <summary>
        /// 测试Alarm_RuleName日期最大值
        /// </summary>
        [TestMethod]
        public void TestAlarm_RuleNameDateTimeForMax()
        {
            AlarmSettingRules alarmsettingrules = new AlarmSettingRules();
            CommandManageService service = new CommandManageService();
            alarmsettingrules.Alarm_RuleID = "";
            alarmsettingrules.Alarm_RuleName = DateTime.MaxValue.ToString();
            alarmsettingrules.Alarm_Creator = "";
            alarmsettingrules.Alarm_Description = "";
            SingleMessage<bool> ret = service.AlarmSettingAdd(alarmsettingrules);
            //Assert.IsFalse(ret.IsSuccess);
        }


        /// <summary>
        /// 测试Alarm_ButtonTime日期最大值
        /// </summary>
        [TestMethod]
        public void TestAlarm_ButtonTimeDateTimeForMax()
        {
            AlarmSettingRules alarmsettingrules = new AlarmSettingRules();
            CommandManageService service = new CommandManageService();
            alarmsettingrules.Alarm_RuleID = "";
            alarmsettingrules.Alarm_RuleName = "";
            alarmsettingrules.Alarm_ButtonTime = short.Parse(DateTime.MaxValue.ToString());
            alarmsettingrules.Alarm_Creator = "";
            alarmsettingrules.Alarm_Description = "";
            SingleMessage<bool> ret = service.AlarmSettingAdd(alarmsettingrules);
            //Assert.IsFalse(ret.IsSuccess);
        }


        /// <summary>
        /// 测试Alarm_Normal日期最大值
        /// </summary>
        [TestMethod]
        public void TestAlarm_NormalDateTimeForMax()
        {
            AlarmSettingRules alarmsettingrules = new AlarmSettingRules();
            CommandManageService service = new CommandManageService();
            alarmsettingrules.Alarm_RuleID = "";
            alarmsettingrules.Alarm_RuleName = "";
            alarmsettingrules.Alarm_Normal = int.Parse(DateTime.MaxValue.ToString());
            alarmsettingrules.Alarm_Creator = "";
            alarmsettingrules.Alarm_Description = "";
            SingleMessage<bool> ret = service.AlarmSettingAdd(alarmsettingrules);
            //Assert.IsFalse(ret.IsSuccess);
        }


        /// <summary>
        /// 测试Alarm_Creator日期最大值
        /// </summary>
        [TestMethod]
        public void TestAlarm_CreatorDateTimeForMax()
        {
            AlarmSettingRules alarmsettingrules = new AlarmSettingRules();
            CommandManageService service = new CommandManageService();
            alarmsettingrules.Alarm_RuleID = "";
            alarmsettingrules.Alarm_RuleName = "";
            alarmsettingrules.Alarm_Creator = DateTime.MaxValue.ToString();
            alarmsettingrules.Alarm_Description = "";
            SingleMessage<bool> ret = service.AlarmSettingAdd(alarmsettingrules);
            //Assert.IsFalse(ret.IsSuccess);
        }


        /// <summary>
        /// 测试Alarm_CreateTime日期最大值
        /// </summary>
        [TestMethod]
        public void TestAlarm_CreateTimeDateTimeForMax()
        {
            AlarmSettingRules alarmsettingrules = new AlarmSettingRules();
            CommandManageService service = new CommandManageService();
            alarmsettingrules.Alarm_RuleID = "";
            alarmsettingrules.Alarm_RuleName = "";
            alarmsettingrules.Alarm_Creator = "";
            alarmsettingrules.Alarm_CreateTime = DateTime.MaxValue;
            alarmsettingrules.Alarm_Description = "";
            SingleMessage<bool> ret = service.AlarmSettingAdd(alarmsettingrules);
            //Assert.IsFalse(ret.IsSuccess);
        }


        /// <summary>
        /// 测试Alarm_Description日期最大值
        /// </summary>
        [TestMethod]
        public void TestAlarm_DescriptionDateTimeForMax()
        {
            AlarmSettingRules alarmsettingrules = new AlarmSettingRules();
            CommandManageService service = new CommandManageService();
            alarmsettingrules.Alarm_RuleID = "";
            alarmsettingrules.Alarm_RuleName = "";
            alarmsettingrules.Alarm_Creator = "";
            alarmsettingrules.Alarm_Description = DateTime.MaxValue.ToString();
            SingleMessage<bool> ret = service.AlarmSettingAdd(alarmsettingrules);
            //Assert.IsFalse(ret.IsSuccess);
        }


        /// <summary>
        /// 测试Alarm_VehcileCount日期最大值
        /// </summary>
        [TestMethod]
        public void TestAlarm_VehcileCountDateTimeForMax()
        {
            AlarmSettingRules alarmsettingrules = new AlarmSettingRules();
            CommandManageService service = new CommandManageService();
            alarmsettingrules.Alarm_RuleID = "";
            alarmsettingrules.Alarm_RuleName = "";
            alarmsettingrules.Alarm_Creator = "";
            alarmsettingrules.Alarm_Description = "";
            alarmsettingrules.Alarm_VehcileCount = int.Parse(DateTime.MaxValue.ToString());
            SingleMessage<bool> ret = service.AlarmSettingAdd(alarmsettingrules);
            //Assert.IsFalse(ret.IsSuccess);
        }


        /// <summary>
        /// 测试Alarm_IsDefault日期最大值
        /// </summary>
        [TestMethod]
        public void TestAlarm_IsDefaultDateTimeForMax()
        {
            AlarmSettingRules alarmsettingrules = new AlarmSettingRules();
            CommandManageService service = new CommandManageService();
            alarmsettingrules.Alarm_RuleID = "";
            alarmsettingrules.Alarm_RuleName = "";
            alarmsettingrules.Alarm_Creator = "";
            alarmsettingrules.Alarm_Description = "";
            alarmsettingrules.Alarm_IsDefault = short.Parse(DateTime.MaxValue.ToString());
            SingleMessage<bool> ret = service.AlarmSettingAdd(alarmsettingrules);
            //Assert.IsFalse(ret.IsSuccess);
        }


        /// <summary>
        /// 测试Alarm_Valid日期最大值
        /// </summary>
        [TestMethod]
        public void TestAlarm_ValidDateTimeForMax()
        {
            AlarmSettingRules alarmsettingrules = new AlarmSettingRules();
            CommandManageService service = new CommandManageService();
            alarmsettingrules.Alarm_RuleID = "";
            alarmsettingrules.Alarm_RuleName = "";
            alarmsettingrules.Alarm_Creator = "";
            alarmsettingrules.Alarm_Description = "";
            alarmsettingrules.Alarm_Valid = short.Parse(DateTime.MaxValue.ToString());
            SingleMessage<bool> ret = service.AlarmSettingAdd(alarmsettingrules);
            //Assert.IsFalse(ret.IsSuccess);
        }


        /// <summary>
        /// 测试Alarm_RuleID日期格式错误
        /// </summary>
        [TestMethod]
        public void TestAlarm_RuleIDDateTimeForMin()
        {
            AlarmSettingRules alarmsettingrules = new AlarmSettingRules();
            CommandManageService service = new CommandManageService();
            alarmsettingrules.Alarm_RuleID = DateTime.MinValue.ToString();
            alarmsettingrules.Alarm_RuleName = "";
            alarmsettingrules.Alarm_Creator = "";
            alarmsettingrules.Alarm_Description = "";
            SingleMessage<bool> ret = service.AlarmSettingAdd(alarmsettingrules);
            Assert.IsFalse(ret.IsSuccess);
        }


        /// <summary>
        /// 测试Alarm_RuleName日期格式错误
        /// </summary>
        [TestMethod]
        public void TestAlarm_RuleNameDateTimeForMin()
        {
            AlarmSettingRules alarmsettingrules = new AlarmSettingRules();
            CommandManageService service = new CommandManageService();
            alarmsettingrules.Alarm_RuleID = "";
            alarmsettingrules.Alarm_RuleName = DateTime.MinValue.ToString();
            alarmsettingrules.Alarm_Creator = "";
            alarmsettingrules.Alarm_Description = "";
            SingleMessage<bool> ret = service.AlarmSettingAdd(alarmsettingrules);
            //Assert.IsFalse(ret.IsSuccess);
        }


        /// <summary>
        /// 测试Alarm_ButtonTime日期格式错误
        /// </summary>
        [TestMethod]
        public void TestAlarm_ButtonTimeDateTimeForMin()
        {
            AlarmSettingRules alarmsettingrules = new AlarmSettingRules();
            CommandManageService service = new CommandManageService();
            alarmsettingrules.Alarm_RuleID = "";
            alarmsettingrules.Alarm_RuleName = "";
            alarmsettingrules.Alarm_ButtonTime = short.Parse(DateTime.MinValue.ToString());
            alarmsettingrules.Alarm_Creator = "";
            alarmsettingrules.Alarm_Description = "";
            SingleMessage<bool> ret = service.AlarmSettingAdd(alarmsettingrules);
            //Assert.IsFalse(ret.IsSuccess);
        }


        /// <summary>
        /// 测试Alarm_Normal日期格式错误
        /// </summary>
        [TestMethod]
        public void TestAlarm_NormalDateTimeForMin()
        {
            AlarmSettingRules alarmsettingrules = new AlarmSettingRules();
            CommandManageService service = new CommandManageService();
            alarmsettingrules.Alarm_RuleID = "";
            alarmsettingrules.Alarm_RuleName = "";
            alarmsettingrules.Alarm_Normal = short.Parse(DateTime.MinValue.ToString());
            alarmsettingrules.Alarm_Creator = "";
            alarmsettingrules.Alarm_Description = "";
            SingleMessage<bool> ret = service.AlarmSettingAdd(alarmsettingrules);
            //Assert.IsFalse(ret.IsSuccess);
        }


        /// <summary>
        /// 测试Alarm_Creator日期格式错误
        /// </summary>
        [TestMethod]
        public void TestAlarm_CreatorDateTimeForMin()
        {
            AlarmSettingRules alarmsettingrules = new AlarmSettingRules();
            CommandManageService service = new CommandManageService();
            alarmsettingrules.Alarm_RuleID = "";
            alarmsettingrules.Alarm_RuleName = "";
            alarmsettingrules.Alarm_Creator = DateTime.MinValue.ToString();
            alarmsettingrules.Alarm_Description = "";
            SingleMessage<bool> ret = service.AlarmSettingAdd(alarmsettingrules);
            //Assert.IsFalse(ret.IsSuccess);
        }


        /// <summary>
        /// 测试Alarm_CreateTime日期格式错误
        /// </summary>
        [TestMethod]
        public void TestAlarm_CreateTimeDateTimeForMin()
        {
            AlarmSettingRules alarmsettingrules = new AlarmSettingRules();
            CommandManageService service = new CommandManageService();
            alarmsettingrules.Alarm_RuleID = "";
            alarmsettingrules.Alarm_RuleName = "";
            alarmsettingrules.Alarm_Creator = "";
            alarmsettingrules.Alarm_CreateTime = DateTime.MinValue;
            alarmsettingrules.Alarm_Description = "";
            SingleMessage<bool> ret = service.AlarmSettingAdd(alarmsettingrules);
            //Assert.IsFalse(ret.IsSuccess);
        }


        /// <summary>
        /// 测试Alarm_Description日期格式错误
        /// </summary>
        [TestMethod]
        public void TestAlarm_DescriptionDateTimeForMin()
        {
            AlarmSettingRules alarmsettingrules = new AlarmSettingRules();
            CommandManageService service = new CommandManageService();
            alarmsettingrules.Alarm_RuleID = "";
            alarmsettingrules.Alarm_RuleName = "";
            alarmsettingrules.Alarm_Creator = "";
            alarmsettingrules.Alarm_Description = DateTime.MinValue.ToString();
            SingleMessage<bool> ret = service.AlarmSettingAdd(alarmsettingrules);
            //Assert.IsFalse(ret.IsSuccess);
        }


        /// <summary>
        /// 测试Alarm_VehcileCount日期格式错误
        /// </summary>
        [TestMethod]
        public void TestAlarm_VehcileCountDateTimeForMin()
        {
            AlarmSettingRules alarmsettingrules = new AlarmSettingRules();
            CommandManageService service = new CommandManageService();
            alarmsettingrules.Alarm_RuleID = "";
            alarmsettingrules.Alarm_RuleName = "";
            alarmsettingrules.Alarm_Creator = "";
            alarmsettingrules.Alarm_Description = "";
            alarmsettingrules.Alarm_VehcileCount = int.Parse(DateTime.MinValue.ToString()); ;
            SingleMessage<bool> ret = service.AlarmSettingAdd(alarmsettingrules);
            //Assert.IsFalse(ret.IsSuccess);
        }


        /// <summary>
        /// 测试Alarm_IsDefault日期格式错误
        /// </summary>
        [TestMethod]
        public void TestAlarm_IsDefaultDateTimeForMin()
        {
            AlarmSettingRules alarmsettingrules = new AlarmSettingRules();
            CommandManageService service = new CommandManageService();
            alarmsettingrules.Alarm_RuleID = "";
            alarmsettingrules.Alarm_RuleName = "";
            alarmsettingrules.Alarm_Creator = "";
            alarmsettingrules.Alarm_Description = "";
            alarmsettingrules.Alarm_IsDefault = short.Parse(DateTime.MinValue.ToString());
            SingleMessage<bool> ret = service.AlarmSettingAdd(alarmsettingrules);
            //Assert.IsFalse(ret.IsSuccess);
        }


        /// <summary>
        /// 测试Alarm_Valid日期格式错误
        /// </summary>
        [TestMethod]
        public void TestAlarm_ValidDateTimeForMin()
        {
            AlarmSettingRules alarmsettingrules = new AlarmSettingRules();
            CommandManageService service = new CommandManageService();
            alarmsettingrules.Alarm_RuleID = "";
            alarmsettingrules.Alarm_RuleName = "";
            alarmsettingrules.Alarm_Creator = "";
            alarmsettingrules.Alarm_Description = "";
            alarmsettingrules.Alarm_Valid = short.Parse(DateTime.MinValue.ToString());
            SingleMessage<bool> ret = service.AlarmSettingAdd(alarmsettingrules);
            //Assert.IsFalse(ret.IsSuccess);
        }


        /// <summary>
        /// update no exists
        /// </summary>
        [TestMethod]
        public void TestAlarm_RuleIDUpdateNotExist()
        {
            AlarmSettingRules alarmsettingrules = new AlarmSettingRules();
            CommandManageService service = new CommandManageService();
            alarmsettingrules.Alarm_RuleID = DateTime.MinValue.ToString();
            alarmsettingrules.Alarm_RuleName = "";
            alarmsettingrules.Alarm_Creator = "";
            alarmsettingrules.Alarm_Description = "";
            SingleMessage<bool> ret = service.ModifyAlarmSettings(alarmsettingrules);
            //Assert.IsFalse(ret.IsSuccess);
        }


        /// <summary>
        /// update no exists
        /// </summary>
        [TestMethod]
        public void TestAlarm_RuleNameUpdateNotExist()
        {
            AlarmSettingRules alarmsettingrules = new AlarmSettingRules();
            CommandManageService service = new CommandManageService();
            alarmsettingrules.Alarm_RuleID = "";
            alarmsettingrules.Alarm_RuleName = DateTime.MinValue.ToString();
            alarmsettingrules.Alarm_Creator = "";
            alarmsettingrules.Alarm_Description = "";
            SingleMessage<bool> ret = service.ModifyAlarmSettings(alarmsettingrules);
            //Assert.IsFalse(ret.IsSuccess);
        }


        /// <summary>
        /// update no exists
        /// </summary>
        [TestMethod]
        public void TestAlarm_ButtonTimeUpdateNotExist()
        {
            AlarmSettingRules alarmsettingrules = new AlarmSettingRules();
            CommandManageService service = new CommandManageService();
            alarmsettingrules.Alarm_RuleID = "";
            alarmsettingrules.Alarm_RuleName = "";
            alarmsettingrules.Alarm_ButtonTime = short.Parse(DateTime.MinValue.ToString());
            alarmsettingrules.Alarm_Creator = "";
            alarmsettingrules.Alarm_Description = "";
            SingleMessage<bool> ret = service.ModifyAlarmSettings(alarmsettingrules);
            //Assert.IsFalse(ret.IsSuccess);
        }


        /// <summary>
        /// update no exists
        /// </summary>
        [TestMethod]
        public void TestAlarm_NormalUpdateNotExist()
        {
            AlarmSettingRules alarmsettingrules = new AlarmSettingRules();
            CommandManageService service = new CommandManageService();
            alarmsettingrules.Alarm_RuleID = "";
            alarmsettingrules.Alarm_RuleName = "";
            alarmsettingrules.Alarm_Normal = short.Parse(DateTime.MinValue.ToString());
            alarmsettingrules.Alarm_Creator = "";
            alarmsettingrules.Alarm_Description = "";
            SingleMessage<bool> ret = service.ModifyAlarmSettings(alarmsettingrules);
            //Assert.IsFalse(ret.IsSuccess);
        }


        /// <summary>
        /// update no exists
        /// </summary>
        [TestMethod]
        public void TestAlarm_CreatorUpdateNotExist()
        {
            AlarmSettingRules alarmsettingrules = new AlarmSettingRules();
            CommandManageService service = new CommandManageService();
            alarmsettingrules.Alarm_RuleID = "";
            alarmsettingrules.Alarm_RuleName = "";
            alarmsettingrules.Alarm_Creator = DateTime.MinValue.ToString();
            alarmsettingrules.Alarm_Description = "";
            SingleMessage<bool> ret = service.ModifyAlarmSettings(alarmsettingrules);
            //Assert.IsFalse(ret.IsSuccess);
        }


        /// <summary>
        /// update no exists
        /// </summary>
        [TestMethod]
        public void TestAlarm_CreateTimeUpdateNotExist()
        {
            AlarmSettingRules alarmsettingrules = new AlarmSettingRules();
            CommandManageService service = new CommandManageService();
            alarmsettingrules.Alarm_RuleID = "";
            alarmsettingrules.Alarm_RuleName = "";
            alarmsettingrules.Alarm_Creator = "";
            alarmsettingrules.Alarm_CreateTime = DateTime.MinValue;
            alarmsettingrules.Alarm_Description = "";
            SingleMessage<bool> ret = service.ModifyAlarmSettings(alarmsettingrules);
            //Assert.IsFalse(ret.IsSuccess);
        }


        /// <summary>
        /// update no exists
        /// </summary>
        [TestMethod]
        public void TestAlarm_DescriptionUpdateNotExist()
        {
            AlarmSettingRules alarmsettingrules = new AlarmSettingRules();
            CommandManageService service = new CommandManageService();
            alarmsettingrules.Alarm_RuleID = "";
            alarmsettingrules.Alarm_RuleName = "";
            alarmsettingrules.Alarm_Creator = "";
            alarmsettingrules.Alarm_Description = DateTime.MinValue.ToString();
            SingleMessage<bool> ret = service.ModifyAlarmSettings(alarmsettingrules);
            //Assert.IsFalse(ret.IsSuccess);
        }


        /// <summary>
        /// update no exists
        /// </summary>
        [TestMethod]
        public void TestAlarm_VehcileCountUpdateNotExist()
        {
            AlarmSettingRules alarmsettingrules = new AlarmSettingRules();
            CommandManageService service = new CommandManageService();
            alarmsettingrules.Alarm_RuleID = "";
            alarmsettingrules.Alarm_RuleName = "";
            alarmsettingrules.Alarm_Creator = "";
            alarmsettingrules.Alarm_Description = "";
            alarmsettingrules.Alarm_VehcileCount = int.Parse(DateTime.MinValue.ToString());
            SingleMessage<bool> ret = service.ModifyAlarmSettings(alarmsettingrules);
            //Assert.IsFalse(ret.IsSuccess);
        }


        /// <summary>
        /// update no exists
        /// </summary>
        [TestMethod]
        public void TestAlarm_IsDefaultUpdateNotExist()
        {
            AlarmSettingRules alarmsettingrules = new AlarmSettingRules();
            CommandManageService service = new CommandManageService();
            alarmsettingrules.Alarm_RuleID = "";
            alarmsettingrules.Alarm_RuleName = "";
            alarmsettingrules.Alarm_Creator = "";
            alarmsettingrules.Alarm_Description = "";
            alarmsettingrules.Alarm_IsDefault = short.Parse(DateTime.MinValue.ToString());
            SingleMessage<bool> ret = service.ModifyAlarmSettings(alarmsettingrules);
            //Assert.IsFalse(ret.IsSuccess);
        }


        /// <summary>
        /// update no exists
        /// </summary>
        [TestMethod]
        public void TestAlarm_ValidUpdateNotExist()
        {
            AlarmSettingRules alarmsettingrules = new AlarmSettingRules();
            CommandManageService service = new CommandManageService();
            alarmsettingrules.Alarm_RuleID = "";
            alarmsettingrules.Alarm_RuleName = "";
            alarmsettingrules.Alarm_Creator = "";
            alarmsettingrules.Alarm_Description = "";
            alarmsettingrules.Alarm_Valid = short.Parse(DateTime.MinValue.ToString());
            SingleMessage<bool> ret = service.ModifyAlarmSettings(alarmsettingrules);
            //Assert.IsFalse(ret.IsSuccess);
        }


        /// <summary>
        /// update no exists
        /// </summary>
        [TestMethod]
        public void TestAlarm_RuleIDDeleteNotExist()
        {
            AlarmSettingRules alarmsettingrules = new AlarmSettingRules();
            CommandManageService service = new CommandManageService();
            alarmsettingrules.Alarm_RuleID = DateTime.MinValue.ToString();
            alarmsettingrules.Alarm_RuleName = "";
            alarmsettingrules.Alarm_Creator = "";
            alarmsettingrules.Alarm_Description = "";
            SingleMessage<bool> ret = service.DeleteAlarmSetting(alarmsettingrules.Alarm_RuleID);
            //Assert.IsFalse(ret.IsSuccess);
        }


        /// <summary>
        /// update no exists
        /// </summary>
        [TestMethod]
        public void TestAlarm_RuleNameDeleteNotExist()
        {
            AlarmSettingRules alarmsettingrules = new AlarmSettingRules();
            CommandManageService service = new CommandManageService();
            alarmsettingrules.Alarm_RuleID = "";
            alarmsettingrules.Alarm_RuleName = DateTime.MinValue.ToString();
            alarmsettingrules.Alarm_Creator = "";
            alarmsettingrules.Alarm_Description = "";
            SingleMessage<bool> ret = service.DeleteAlarmSetting(alarmsettingrules.Alarm_RuleID);
            //Assert.IsFalse(ret.IsSuccess);
        }


        /// <summary>
        /// update no exists
        /// </summary>
        [TestMethod]
        public void TestAlarm_ButtonTimeDeleteNotExist()
        {
            AlarmSettingRules alarmsettingrules = new AlarmSettingRules();
            CommandManageService service = new CommandManageService();
            alarmsettingrules.Alarm_RuleID = "";
            alarmsettingrules.Alarm_RuleName = "";
            alarmsettingrules.Alarm_ButtonTime = short.Parse(DateTime.MinValue.ToString());
            alarmsettingrules.Alarm_Creator = "";
            alarmsettingrules.Alarm_Description = "";
            SingleMessage<bool> ret = service.DeleteAlarmSetting(alarmsettingrules.Alarm_RuleID);
            //Assert.IsFalse(ret.IsSuccess);
        }


        /// <summary>
        /// update no exists
        /// </summary>
        [TestMethod]
        public void TestAlarm_NormalDeleteNotExist()
        {
            AlarmSettingRules alarmsettingrules = new AlarmSettingRules();
            CommandManageService service = new CommandManageService();
            alarmsettingrules.Alarm_RuleID = "";
            alarmsettingrules.Alarm_RuleName = "";
            alarmsettingrules.Alarm_Normal = int.Parse(DateTime.MinValue.ToString());
            alarmsettingrules.Alarm_Creator = "";
            alarmsettingrules.Alarm_Description = "";
            SingleMessage<bool> ret = service.DeleteAlarmSetting(alarmsettingrules.Alarm_RuleID);
            //Assert.IsFalse(ret.IsSuccess);
        }


        /// <summary>
        /// update no exists
        /// </summary>
        [TestMethod]
        public void TestAlarm_CreatorDeleteNotExist()
        {
            AlarmSettingRules alarmsettingrules = new AlarmSettingRules();
            CommandManageService service = new CommandManageService();
            alarmsettingrules.Alarm_RuleID = "";
            alarmsettingrules.Alarm_RuleName = "";
            alarmsettingrules.Alarm_Creator = DateTime.MinValue.ToString();
            alarmsettingrules.Alarm_Description = "";
            SingleMessage<bool> ret = service.DeleteAlarmSetting(alarmsettingrules.Alarm_RuleID);
            //Assert.IsFalse(ret.IsSuccess);
        }


        /// <summary>
        /// update no exists
        /// </summary>
        [TestMethod]
        public void TestAlarm_CreateTimeDeleteNotExist()
        {
            AlarmSettingRules alarmsettingrules = new AlarmSettingRules();
            CommandManageService service = new CommandManageService();
            alarmsettingrules.Alarm_RuleID = "";
            alarmsettingrules.Alarm_RuleName = "";
            alarmsettingrules.Alarm_Creator = "";
            alarmsettingrules.Alarm_CreateTime = DateTime.MinValue;
            alarmsettingrules.Alarm_Description = "";
            SingleMessage<bool> ret = service.DeleteAlarmSetting(alarmsettingrules.Alarm_RuleID);
            //Assert.IsFalse(ret.IsSuccess);
        }


        /// <summary>
        /// update no exists
        /// </summary>
        [TestMethod]
        public void TestAlarm_DescriptionDeleteNotExist()
        {
            AlarmSettingRules alarmsettingrules = new AlarmSettingRules();
            CommandManageService service = new CommandManageService();
            alarmsettingrules.Alarm_RuleID = "";
            alarmsettingrules.Alarm_RuleName = "";
            alarmsettingrules.Alarm_Creator = "";
            alarmsettingrules.Alarm_Description = DateTime.MinValue.ToString();
            SingleMessage<bool> ret = service.DeleteAlarmSetting(alarmsettingrules.Alarm_RuleID);
            //Assert.IsFalse(ret.IsSuccess);
        }


        /// <summary>
        /// update no exists
        /// </summary>
        [TestMethod]
        public void TestAlarm_VehcileCountDeleteNotExist()
        {
            AlarmSettingRules alarmsettingrules = new AlarmSettingRules();
            CommandManageService service = new CommandManageService();
            alarmsettingrules.Alarm_RuleID = "";
            alarmsettingrules.Alarm_RuleName = "";
            alarmsettingrules.Alarm_Creator = "";
            alarmsettingrules.Alarm_Description = "";
            alarmsettingrules.Alarm_VehcileCount = int.Parse(DateTime.MinValue.ToString());
            SingleMessage<bool> ret = service.DeleteAlarmSetting(alarmsettingrules.Alarm_RuleID);
            //Assert.IsFalse(ret.IsSuccess);
        }


        /// <summary>
        /// update no exists
        /// </summary>
        [TestMethod]
        public void TestAlarm_IsDefaultDeleteNotExist()
        {
            AlarmSettingRules alarmsettingrules = new AlarmSettingRules();
            CommandManageService service = new CommandManageService();
            alarmsettingrules.Alarm_RuleID = "";
            alarmsettingrules.Alarm_RuleName = "";
            alarmsettingrules.Alarm_Creator = "";
            alarmsettingrules.Alarm_Description = "";
            alarmsettingrules.Alarm_IsDefault = short.Parse(DateTime.MinValue.ToString());
            SingleMessage<bool> ret = service.DeleteAlarmSetting(alarmsettingrules.Alarm_RuleID);
            //Assert.IsFalse(ret.IsSuccess);
        }


        /// <summary>
        /// update no exists
        /// </summary>
        [TestMethod]
        public void TestAlarm_ValidDeleteNotExist()
        {
            AlarmSettingRules alarmsettingrules = new AlarmSettingRules();
            CommandManageService service = new CommandManageService();
            alarmsettingrules.Alarm_RuleID = "";
            alarmsettingrules.Alarm_RuleName = "";
            alarmsettingrules.Alarm_Creator = "";
            alarmsettingrules.Alarm_Description = "";
            alarmsettingrules.Alarm_Valid = short.Parse(DateTime.MinValue.ToString());
            SingleMessage<bool> ret = service.DeleteAlarmSetting(alarmsettingrules.Alarm_RuleID);
            //Assert.IsFalse(ret.IsSuccess);
        }
        #endregion

        # endregion

        #region AbnormalDoor
        /// <summary>
        /// 所有的字段为空,have tested
        /// </summary>
        [TestMethod]
        public void TestStringForAllEmptyAbnormalDoor()
        {
            AbnormalDoorRuleInfo abnormaldoorruleinfo = new AbnormalDoorRuleInfo();
            CommandManageService service = new CommandManageService();
            abnormaldoorruleinfo.ID = null;
            abnormaldoorruleinfo.RuleName = null;
            abnormaldoorruleinfo.UserDescription = null;
            abnormaldoorruleinfo.Creator = null;
            SingleMessage<bool> ret = service.AddAbnormalDoorRule(abnormaldoorruleinfo);
            Assert.IsFalse(ret.IsSuccess);
        }


        /// <summary>
        /// 允许的字段为空,have tested
        /// </summary>
        [TestMethod]
        public void TestStringForAllowEmptyAbnormalDoor()
        {
            AbnormalDoorRuleInfo abnormaldoorruleinfo = new AbnormalDoorRuleInfo();
            CommandManageService service = new CommandManageService();
            StringBuilder builder = new StringBuilder();
            Random random = new Random();
            abnormaldoorruleinfo.ID = "rty6";
            abnormaldoorruleinfo.RuleName = null;
            abnormaldoorruleinfo.UserDescription = null;
            abnormaldoorruleinfo.Creator = null;
            SingleMessage<bool> ret = service.AddAbnormalDoorRule(abnormaldoorruleinfo);
            Assert.IsTrue(ret.IsSuccess);
        }


        /// <summary>
        /// 测试最大中文长度,have tested
        /// </summary>
        [TestMethod]
        public void TestStringForMaxLength_ChineseAbnormalDoor()
        {
            AbnormalDoorRuleInfo abnormaldoorruleinfo = new AbnormalDoorRuleInfo();
            CommandManageService service = new CommandManageService();
            StringBuilder builder = new StringBuilder();
            Random random = new Random();
            for (int i = 0; i < 36; i++)
            {
                builder.Append(chinesecharacter[random.Next(chinesecharacter.Length - 1)]);
            }
            abnormaldoorruleinfo.ID = builder.ToString();
            builder.Length = 0;
            for (int i = 0; i < 512; i++)
            {
                builder.Append(chinesecharacter[random.Next(chinesecharacter.Length - 1)]);
            }
            abnormaldoorruleinfo.RuleName = builder.ToString();
            builder.Length = 0;
            for (int i = 0; i < 512; i++)
            {
                builder.Append(chinesecharacter[random.Next(chinesecharacter.Length - 1)]);
            }
            abnormaldoorruleinfo.Creator = builder.ToString();
            builder.Length = 0;
            for (int i = 0; i < 999; i++)
            {
                builder.Append(chinesecharacter[random.Next(chinesecharacter.Length - 1)]);
            }
            abnormaldoorruleinfo.UserDescription = builder.ToString();
            builder.Length = 0; 
            SingleMessage<bool> ret = service.AddAbnormalDoorRule(abnormaldoorruleinfo);
            Assert.IsTrue(ret.IsSuccess);
        }


        /// <summary>
        /// 测试西文最大长度,have tested
        /// </summary>
        [TestMethod]
        public void TestStringForMaxLength_SpanishAbnormalDoor()
        {
            AbnormalDoorRuleInfo abnormaldoorruleinfo = new AbnormalDoorRuleInfo();
            CommandManageService service = new CommandManageService();
            StringBuilder builder = new StringBuilder();
            Random random = new Random();
            for (int i = 0; i < 36; i++)
            {
                builder.Append(spanishcharacter[random.Next(spanishcharacter.Length - 1)]);
            }
            abnormaldoorruleinfo.ID = builder.ToString();
            builder.Length = 0;
            for (int i = 0; i < 512; i++)
            {
                builder.Append(spanishcharacter[random.Next(spanishcharacter.Length - 1)]);
            }
            abnormaldoorruleinfo.RuleName = builder.ToString();
            builder.Length = 0;
            for (int i = 0; i < 512; i++)
            {
                builder.Append(spanishcharacter[random.Next(spanishcharacter.Length - 1)]);
            }
            abnormaldoorruleinfo.Creator = builder.ToString();
            builder.Length = 0;
            for (int i = 0; i < 2000; i++)
            {
                builder.Append(spanishcharacter[random.Next(spanishcharacter.Length - 1)]);
            }
            abnormaldoorruleinfo.UserDescription = builder.ToString();
            builder.Length = 0; 
            SingleMessage<bool> ret = service.AddAbnormalDoorRule(abnormaldoorruleinfo);
            Assert.IsTrue(ret.IsSuccess);
        }


        /// <summary>
        /// 测试英文最大长度,have tested
        /// </summary>
        [TestMethod]
        public void TestStringForMaxLength_EnglishAbnormalDoor()
        {
            AbnormalDoorRuleInfo abnormaldoorruleinfo = new AbnormalDoorRuleInfo();
            CommandManageService service = new CommandManageService();
            StringBuilder builder = new StringBuilder();
            Random random = new Random();
            for (int i = 0; i < 36; i++)
            {
                builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
            }
            abnormaldoorruleinfo.ID = builder.ToString();
            builder.Length = 0;
            for (int i = 0; i < 512; i++)
            {
                builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
            }
            abnormaldoorruleinfo.RuleName = builder.ToString();
            builder.Length = 0;
            for (int i = 0; i < 512; i++)
            {
                builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
            }
            abnormaldoorruleinfo.Creator = builder.ToString();
            builder.Length = 0;
            for (int i = 0; i < 2000; i++)
            {
                builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
            }
            abnormaldoorruleinfo.UserDescription = builder.ToString();
            builder.Length = 0; 
            SingleMessage<bool> ret = service.AddAbnormalDoorRule(abnormaldoorruleinfo);
            Assert.IsTrue(ret.IsSuccess);
        }


        /// <summary>
        /// 测试Speed中文超长,don`t test
        /// </summary>
        [TestMethod]
        public void TestSpeedStringForOverLength_Chinese()
        {
            AbnormalDoorRuleInfo abnormaldoorruleinfo = new AbnormalDoorRuleInfo();
            CommandManageService service = new CommandManageService();
            StringBuilder builder = new StringBuilder();
            Random random = new Random();
            for (int i = 0; i < 37; i++)
            {
                builder.Append(chinesecharacter[random.Next(chinesecharacter.Length - 1)]);
            }
            abnormaldoorruleinfo.ID = builder.ToString();
            builder.Length = 0;
            for (int i = 0; i < 562; i++)
            {
                builder.Append(chinesecharacter[random.Next(chinesecharacter.Length - 1)]);
            }
            abnormaldoorruleinfo.RuleName = builder.ToString();
            builder.Length = 0;
            for (int i = 0; i < 562; i++)
            {
                builder.Append(chinesecharacter[random.Next(chinesecharacter.Length - 1)]);
            }
            abnormaldoorruleinfo.Creator = builder.ToString();
            builder.Length = 0;
            for (int i = 0; i < 1000; i++)
            {
                builder.Append(chinesecharacter[random.Next(chinesecharacter.Length - 1)]);
            }
            abnormaldoorruleinfo.UserDescription = builder.ToString();
            builder.Length = 0; 
            SingleMessage<bool> ret = service.AddAbnormalDoorRule(abnormaldoorruleinfo);
            Assert.IsFalse(ret.IsSuccess);
        }


        /// <summary>
        /// 测试Speed西文超长,don`t test
        /// </summary>
        [TestMethod]
        public void TestSpeedStringForMaxLength_Spanish()
        {
            AbnormalDoorRuleInfo abnormaldoorruleinfo = new AbnormalDoorRuleInfo();
            CommandManageService service = new CommandManageService();
            StringBuilder builder = new StringBuilder();
            Random random = new Random();
            SingleMessage<bool> ret = service.AddAbnormalDoorRule(abnormaldoorruleinfo);
            Assert.IsFalse(ret.IsSuccess);
        }


        /// <summary>
        /// 测试Speed英文超长,don`t test
        /// </summary>
        [TestMethod]
        public void TestSpeedStringForMaxLength_English()
        {
            AbnormalDoorRuleInfo abnormaldoorruleinfo = new AbnormalDoorRuleInfo();
            CommandManageService service = new CommandManageService();
            StringBuilder builder = new StringBuilder();
            Random random = new Random();
            SingleMessage<bool> ret = service.AddAbnormalDoorRule(abnormaldoorruleinfo);
            Assert.IsFalse(ret.IsSuccess);
        }


        /// <summary>
        /// 测试空格,have tested
        /// </summary>
        [TestMethod]
        public void TestStringForSpaceAbnormalDoor()
        {
            AbnormalDoorRuleInfo abnormaldoorruleinfo = new AbnormalDoorRuleInfo();
            CommandManageService service = new CommandManageService();
            StringBuilder builder = new StringBuilder();
            Random random = new Random();
            builder.AppendLine("			Random random = new Random();");
            //builder.Append(" ");
            //for (int i = 0; i < 3; i++)
            //{
            //    builder.Append(chinesecharacter[random.Next(chinesecharacter.Length - 1)]);
            //}
            //builder.Append(" ");
            //abnormaldoorruleinfo.ID = builder.ToString();
            //builder.Length = 0;
            abnormaldoorruleinfo.ID = "阿嫂   打法";
            builder.Append(" ");
            for (int i = 0; i < 52; i++)
            {
                builder.Append(chinesecharacter[random.Next(chinesecharacter.Length - 1)]);
            }
            builder.Append(" ");
            abnormaldoorruleinfo.RuleName = builder.ToString();
            builder.Length = 0;
            builder.Append(" ");
            for (int i = 0; i < 52; i++)
            {
                builder.Append(chinesecharacter[random.Next(chinesecharacter.Length - 1)]);
            }
            builder.Append(" ");
            abnormaldoorruleinfo.Creator = builder.ToString();
            builder.Length = 0;
            builder.Append(" ");
            for (int i = 0; i < 99; i++)
            {
                builder.Append(chinesecharacter[random.Next(chinesecharacter.Length - 1)]);
            }
            builder.Append(" ");
            abnormaldoorruleinfo.UserDescription = builder.ToString();
            builder.Length = 0; 
            SingleMessage<bool> ret = service.AddAbnormalDoorRule(abnormaldoorruleinfo);
            Assert.IsFalse(ret.IsSuccess);
        }


        /// <summary>
        /// 测试,
        /// </summary>
        [TestMethod]
        public void TestStringForSpecialCharacter_CommaTestAbnormalDoor()
        {
            AbnormalDoorRuleInfo abnormaldoorruleinfo = new AbnormalDoorRuleInfo();
            CommandManageService service = new CommandManageService();
            StringBuilder builder = new StringBuilder();
            Random random = new Random();
            builder.AppendLine(",");
            builder.Append(",");
            for (int i = 0; i < 3; i++)
            {
                builder.Append(chinesecharacter[random.Next(chinesecharacter.Length - 1)]);
            }
            builder.Append(",");
            abnormaldoorruleinfo.ID = builder.ToString();
            builder.Length = 0;
            for (int i = 0; i < 52; i++)
            {
                builder.Append(chinesecharacter[random.Next(chinesecharacter.Length - 1)]);
            }
            builder.Append(",");
            abnormaldoorruleinfo.RuleName = builder.ToString();
            builder.Length = 0;
            builder.Append(",");
            for (int i = 0; i < 52; i++)
            {
                builder.Append(chinesecharacter[random.Next(chinesecharacter.Length - 1)]);
            }
            builder.Append(",");
            abnormaldoorruleinfo.Creator = builder.ToString();
            builder.Length = 0;
            builder.Append(",");
            for (int i = 0; i < 99; i++)
            {
                builder.Append(chinesecharacter[random.Next(chinesecharacter.Length - 1)]);
            }
            builder.Append(",");
            abnormaldoorruleinfo.UserDescription = builder.ToString();
            builder.Length = 0; 
            SingleMessage<bool> ret = service.AddAbnormalDoorRule(abnormaldoorruleinfo);
            Assert.IsTrue(ret.IsSuccess);
        }


        /// <summary>
        /// 测试;
        /// </summary>
        [TestMethod]
        public void TestStringForSpecialCharacter_SemiCommaAbnormalDoor()
        {
            AbnormalDoorRuleInfo abnormaldoorruleinfo = new AbnormalDoorRuleInfo();
            CommandManageService service = new CommandManageService();
            StringBuilder builder = new StringBuilder();
            builder.AppendLine(";");
            Random random = new Random();
            for (int i = 0; i < 3; i++)
            {
                builder.Append(chinesecharacter[random.Next(chinesecharacter.Length - 1)]);
            }
            builder.AppendLine(";");
            abnormaldoorruleinfo.ID = builder.ToString();
            builder.Length = 0;
            for (int i = 0; i < 52; i++)
            {
                builder.Append(chinesecharacter[random.Next(chinesecharacter.Length - 1)]);
            }
            builder.AppendLine(";");
            abnormaldoorruleinfo.RuleName = builder.ToString();
            builder.Length = 0;
            builder.AppendLine(";");
            for (int i = 0; i < 52; i++)
            {
                builder.Append(chinesecharacter[random.Next(chinesecharacter.Length - 1)]);
            }
            builder.AppendLine(";");
            abnormaldoorruleinfo.Creator = builder.ToString();
            builder.Length = 0;
            builder.AppendLine(";");
            for (int i = 0; i < 99; i++)
            {
                builder.Append(chinesecharacter[random.Next(chinesecharacter.Length - 1)]);
            }
            builder.AppendLine(";");
            abnormaldoorruleinfo.UserDescription = builder.ToString();
            builder.Length = 0; 
            SingleMessage<bool> ret = service.AddAbnormalDoorRule(abnormaldoorruleinfo);
            Assert.IsTrue(ret.IsSuccess);
        }


        ///// <summary>
        ///// 测试;
        ///// </summary>
        //[TestMethod]
        //public void TestStringForSpecialCharacter_quote()
        //{
        //    AbnormalDoorRuleInfo abnormaldoorruleinfo = new AbnormalDoorRuleInfo();
        //    CommandManageService service = new CommandManageService();
        //    StringBuilder builder = new StringBuilder();
        //    builder.AppendLine("			Random random = new Random();");
        //    SingleMessage<bool> ret = service.AddAbnormalDoorRule(abnormaldoorruleinfo);
        //    Assert.IsFalse(ret.IsSuccess);
        //}


        /// <summary>
        /// 测试select,have tested
        /// </summary>
        [TestMethod]
        public void TestStringForSpecialCharacter_quoteselectAbnormalDoor()
        {
            AbnormalDoorRuleInfo abnormaldoorruleinfo = new AbnormalDoorRuleInfo();
            CommandManageService service = new CommandManageService();
            abnormaldoorruleinfo.ID = "select";
            abnormaldoorruleinfo.RuleName = "select";
            abnormaldoorruleinfo.UserDescription = "select";
            abnormaldoorruleinfo.Creator = "select";
            SingleMessage<bool> ret = service.AddAbnormalDoorRule(abnormaldoorruleinfo);
            Assert.IsTrue(ret.IsSuccess);
        }


        /// <summary>
        /// 测试update
        /// </summary>
        [TestMethod]
        public void TestStringForSpecialCharacter_quoteupdateAbnormalDoor()
        {
            AbnormalDoorRuleInfo abnormaldoorruleinfo = new AbnormalDoorRuleInfo();
            CommandManageService service = new CommandManageService();
            abnormaldoorruleinfo.ID = "update";
            abnormaldoorruleinfo.RuleName = "update";
            abnormaldoorruleinfo.UserDescription = "update";
            abnormaldoorruleinfo.Creator = "update";
            SingleMessage<bool> ret = service.AddAbnormalDoorRule(abnormaldoorruleinfo);
            Assert.IsTrue(ret.IsSuccess);
        }


        /// <summary>
        /// 测试delete
        /// </summary>
        [TestMethod]
        public void TestStringForSpecialCharacter_quotedeleteAbnormalDoor()
        {
            AbnormalDoorRuleInfo abnormaldoorruleinfo = new AbnormalDoorRuleInfo();
            CommandManageService service = new CommandManageService();
            abnormaldoorruleinfo.ID = "delete";
            abnormaldoorruleinfo.RuleName = "delete";
            abnormaldoorruleinfo.UserDescription = "delete";
            abnormaldoorruleinfo.Creator = "delete";
            SingleMessage<bool> ret = service.AddAbnormalDoorRule(abnormaldoorruleinfo);
            Assert.IsTrue(ret.IsSuccess);
        }


        /// <summary>
        /// 测试最大值,don`t test
        /// </summary>
        [TestMethod]
        public void TestNumberForMaxAbnormalDoor()
        {
            //AbnormalDoorRuleInfo abnormaldoorruleinfo = new AbnormalDoorRuleInfo();
            //CommandManageService service = new CommandManageService();
            //abnormaldoorruleinfo.Speed=;
            //SingleMessage<bool> ret = service.AddAbnormalDoorRule(abnormaldoorruleinfo);
            //Assert.IsFalse(ret.IsSuccess);
        }


        /// <summary>
        /// 测试最小值,don`t test
        /// </summary>
        [TestMethod]
        public void TestNumberForMinAbnormalDoor()
        {
            AbnormalDoorRuleInfo abnormaldoorruleinfo = new AbnormalDoorRuleInfo();
            CommandManageService service = new CommandManageService();

            SingleMessage<bool> ret = service.AddAbnormalDoorRule(abnormaldoorruleinfo);
            Assert.IsFalse(ret.IsSuccess);
        }


        /// <summary>
        /// 正数,have tested
        /// </summary>
        [TestMethod]
        public void TestNumberForPositiveAbnormalDoor()
        {
            AbnormalDoorRuleInfo abnormaldoorruleinfo = new AbnormalDoorRuleInfo();
            CommandManageService service = new CommandManageService();
            abnormaldoorruleinfo.ID = "sdfe";
            abnormaldoorruleinfo.Speed=2;
            SingleMessage<bool> ret = service.AddAbnormalDoorRule(abnormaldoorruleinfo);
            Assert.IsTrue(ret.IsSuccess);
        }


        /// <summary>
        /// 负数,have tested
        /// </summary>
        [TestMethod]
        public void TestNumberForNegativeAbnormalDoor()
        {
            AbnormalDoorRuleInfo abnormaldoorruleinfo = new AbnormalDoorRuleInfo();
            CommandManageService service = new CommandManageService();
            abnormaldoorruleinfo.ID = "mnm";
            abnormaldoorruleinfo.Speed = -2;
            SingleMessage<bool> ret = service.AddAbnormalDoorRule(abnormaldoorruleinfo);
            Assert.IsTrue(ret.IsSuccess);
        }


        /// <summary>
        /// 零,have tested
        /// </summary>
        [TestMethod]
        public void TestNumberForZeroAbnormalDoor()
        {
            AbnormalDoorRuleInfo abnormaldoorruleinfo = new AbnormalDoorRuleInfo();
            CommandManageService service = new CommandManageService();
            abnormaldoorruleinfo.ID = "op";
            abnormaldoorruleinfo.Speed = 0;
            SingleMessage<bool> ret = service.AddAbnormalDoorRule(abnormaldoorruleinfo);
            Assert.IsFalse(ret.IsSuccess);
        }


        /// <summary>
        /// 小数,don`t test
        /// </summary>
        [TestMethod]
        public void TestNumberForPortionAbnormalDoor()
        {
            AbnormalDoorRuleInfo abnormaldoorruleinfo = new AbnormalDoorRuleInfo();
            CommandManageService service = new CommandManageService();

            SingleMessage<bool> ret = service.AddAbnormalDoorRule(abnormaldoorruleinfo);
            Assert.IsFalse(ret.IsSuccess);
        }


        /// <summary>
        /// 正数小数,don`t test
        /// </summary>
        [TestMethod]
        public void TestNumberForPortionIntingAbnormalDoor()
        {
            AbnormalDoorRuleInfo abnormaldoorruleinfo = new AbnormalDoorRuleInfo();
            CommandManageService service = new CommandManageService();
            SingleMessage<bool> ret = service.AddAbnormalDoorRule(abnormaldoorruleinfo);
            //Assert.IsFalse(ret.IsSuccess);
        }


        /// <summary>
        /// 字母,don`t test
        /// </summary>
        [TestMethod]
        public void TestNumberForPortionVVAbnormalDoor()
        {
            AbnormalDoorRuleInfo abnormaldoorruleinfo = new AbnormalDoorRuleInfo();
            CommandManageService service = new CommandManageService();
            SingleMessage<bool> ret = service.AddAbnormalDoorRule(abnormaldoorruleinfo);
            //Assert.IsFalse(ret.IsSuccess);
        }


        /// <summary>
        /// DateTime,have tested
        /// </summary>
        [TestMethod]
        public void TestDateTimeAbnormalDoor()
        {
            AbnormalDoorRuleInfo abnormaldoorruleinfo = new AbnormalDoorRuleInfo();
            CommandManageService service = new CommandManageService();
            abnormaldoorruleinfo.ID = "wser3";
            abnormaldoorruleinfo.CreateTime = DateTime.MaxValue;
            SingleMessage<bool> ret = service.AddAbnormalDoorRule(abnormaldoorruleinfo);
            Assert.IsTrue(ret.IsSuccess);
        }


        /// <summary>
        /// 日期为空,don`t test
        /// </summary>
        [TestMethod]
        public void TestSpeedDateTimeForNullAbnormalDoor()
        {
            AbnormalDoorRuleInfo abnormaldoorruleinfo = new AbnormalDoorRuleInfo();
            CommandManageService service = new CommandManageService();
            //abnormaldoorruleinfo.Speed = null;
            SingleMessage<bool> ret = service.AddAbnormalDoorRule(abnormaldoorruleinfo);
            //Assert.IsFalse(ret.IsSuccess);
        }


        /// <summary>
        /// Speed日期为空,don`t test
        /// </summary>
        [TestMethod]
        public void TestSpeedDateTimeForIncomplete()
		{
			AbnormalDoorRuleInfo abnormaldoorruleinfo = new AbnormalDoorRuleInfo();
			CommandManageService service = new CommandManageService();
            //abnormaldoorruleinfo.Speed=DateTime.Parse("2015/02/12").ToString();
			SingleMessage<bool> ret = service.AddAbnormalDoorRule(abnormaldoorruleinfo);
			//Assert.IsFalse(ret.IsSuccess);
		}


        /// <summary>
        /// 测试Speed日期格式错误,don`t test
        /// </summary>
        [TestMethod]
        public void TestSpeedDateTimeForIncompleteAbnormalDoor()
		{
			AbnormalDoorRuleInfo abnormaldoorruleinfo = new AbnormalDoorRuleInfo();
			CommandManageService service = new CommandManageService();
            //abnormaldoorruleinfo.Speed=DateTime.Parse("2015/33/33");
			SingleMessage<bool> ret = service.AddAbnormalDoorRule(abnormaldoorruleinfo);
			//Assert.IsFalse(ret.IsSuccess);
		}


        /// <summary>
        /// 测试Speed日期最大值,don`t test
        /// </summary>
        [TestMethod]
        public void TestSpeedDateTimeForMax()
        {
            AbnormalDoorRuleInfo abnormaldoorruleinfo = new AbnormalDoorRuleInfo();
            CommandManageService service = new CommandManageService();
            //abnormaldoorruleinfo.Speed = DateTime.MaxValue;
            SingleMessage<bool> ret = service.AddAbnormalDoorRule(abnormaldoorruleinfo);
            //Assert.IsFalse(ret.IsSuccess);
        }


        /// <summary>
        /// 测试Speed日期格式错误,don`t test
        /// </summary>
        [TestMethod]
        public void TestSpeedDateTimeForMin()
        {
            AbnormalDoorRuleInfo abnormaldoorruleinfo = new AbnormalDoorRuleInfo();
            CommandManageService service = new CommandManageService();
            //abnormaldoorruleinfo.Speed = DateTime.MinValue;
            SingleMessage<bool> ret = service.AddAbnormalDoorRule(abnormaldoorruleinfo);
            //Assert.IsFalse(ret.IsSuccess);
        }


        /// <summary>
        /// update no exists
        /// </summary>
        [TestMethod]
        public void TestSpeedUpdateNotExist()
        {
            AbnormalDoorRuleInfo abnormaldoorruleinfo = new AbnormalDoorRuleInfo();
            CommandManageService service = new CommandManageService();
            //abnormaldoorruleinfo.Speed = DateTime.MinValue;
            //SingleMessage<bool> ret = service.UpdateAbnormalDoorRuleInfo(abnormaldoorruleinfo);
            //Assert.IsFalse(ret.IsSuccess);
        }


        /// <summary>
        /// update no exists
        /// </summary>
        [TestMethod]
        public void TestSpeedDeleteNotExist()
        {
            AbnormalDoorRuleInfo abnormaldoorruleinfo = new AbnormalDoorRuleInfo();
            CommandManageService service = new CommandManageService();
            //abnormaldoorruleinfo.Speed = DateTime.MinValue;
            //SingleMessage<bool> ret = service.DeleteAbnormalDoorRuleInfo(abnormaldoorruleinfo);
            //Assert.IsFalse(ret.IsSuccess);
        }
        #endregion

        #region Gps rule
         
         /// <summary>
		/// 所有的字段为空 ,have tested
		/// </summary>
		[TestMethod]
		public void TestStringForAllEmptyGps()
		{
			GpsSettingInfo gpssettinginfo = new GpsSettingInfo();
			CommandManageService service = new CommandManageService();
			gpssettinginfo.Gps_RuleID=null;
			gpssettinginfo.Gps_RuleName=null;
			gpssettinginfo.Gps_Creator=null;
			gpssettinginfo.Gps_Description=null;
			SingleMessage<bool> ret = service.GpsSettingAdd(gpssettinginfo);
			Assert.IsFalse(ret.IsSuccess);
		}


		/// <summary>
		/// 允许的字段为空,have tested 
		/// </summary>
		[TestMethod]
		public void TestStringForAllowEmptyGps()
		{
			GpsSettingInfo gpssettinginfo = new GpsSettingInfo();
			CommandManageService service = new CommandManageService();
			StringBuilder builder = new StringBuilder();
			Random random = new Random();
			for (int i = 0; i < 36; i++)
			{
				builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
			}
			gpssettinginfo.Gps_RuleID=builder.ToString();
			builder.Length=0;

			gpssettinginfo.Gps_RuleName=null;
			gpssettinginfo.Gps_Creator=null;
			gpssettinginfo.Gps_Description=null;
			SingleMessage<bool> ret = service.GpsSettingAdd(gpssettinginfo);
			Assert.IsFalse(ret.IsSuccess);
		}


		/// <summary>
		/// 测试最大中文长度,have tested
		/// </summary>
		[TestMethod]
		public void TestStringForMaxLength_ChineseGps()
		{
			GpsSettingInfo gpssettinginfo = new GpsSettingInfo();
			CommandManageService service = new CommandManageService();
			StringBuilder builder = new StringBuilder();
			Random random = new Random();
			for (int i = 0; i < 36; i++)
			{
				builder.Append(chinesecharacter[random.Next(chinesecharacter.Length - 1)]);
			}
			gpssettinginfo.Gps_RuleID=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 512; i++)
			{
				builder.Append(chinesecharacter[random.Next(chinesecharacter.Length - 1)]);
			}
			gpssettinginfo.Gps_RuleName=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 512; i++)
			{
				builder.Append(chinesecharacter[random.Next(chinesecharacter.Length - 1)]);
			}
			gpssettinginfo.Gps_Creator=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 999; i++)
			{
				builder.Append(chinesecharacter[random.Next(chinesecharacter.Length - 1)]);
			}
			gpssettinginfo.Gps_Description=builder.ToString();
			builder.Length=0;

			SingleMessage<bool> ret = service.GpsSettingAdd(gpssettinginfo);
			Assert.IsTrue(ret.IsSuccess);
		}


		/// <summary>
		/// 测试西文最大长度,have tested
		/// </summary>
		[TestMethod]
		public void TestStringForMaxLength_SpanishGps()
		{
			GpsSettingInfo gpssettinginfo = new GpsSettingInfo();
			CommandManageService service = new CommandManageService();
			StringBuilder builder = new StringBuilder();
			Random random = new Random();
			for (int i = 0; i < 36; i++)
			{
				builder.Append(spanishcharacter[random.Next(spanishcharacter.Length - 1)]);
			}
			gpssettinginfo.Gps_RuleID=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 512; i++)
			{
				builder.Append(spanishcharacter[random.Next(spanishcharacter.Length - 1)]);
			}
			gpssettinginfo.Gps_RuleName=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 512; i++)
			{
				builder.Append(spanishcharacter[random.Next(spanishcharacter.Length - 1)]);
			}
			gpssettinginfo.Gps_Creator=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 2000; i++)
			{
				builder.Append(spanishcharacter[random.Next(spanishcharacter.Length - 1)]);
			}
			gpssettinginfo.Gps_Description=builder.ToString();
			builder.Length=0;

			SingleMessage<bool> ret = service.GpsSettingAdd(gpssettinginfo);
			Assert.IsTrue(ret.IsSuccess);
		}


		/// <summary>
		/// 测试英文最大长度,have tested
		/// </summary>
		[TestMethod]
		public void TestStringForMaxLength_EnglishGps()
		{
			GpsSettingInfo gpssettinginfo = new GpsSettingInfo();
			CommandManageService service = new CommandManageService();
			StringBuilder builder = new StringBuilder();
			Random random = new Random();
			for (int i = 0; i < 36; i++)
			{
				builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
			}
			gpssettinginfo.Gps_RuleID=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 512; i++)
			{
				builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
			}
			gpssettinginfo.Gps_RuleName=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 512; i++)
			{
				builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
			}
			gpssettinginfo.Gps_Creator=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 2000; i++)
			{
				builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
			}
			gpssettinginfo.Gps_Description=builder.ToString();
			builder.Length=0;

			SingleMessage<bool> ret = service.GpsSettingAdd(gpssettinginfo);
			Assert.IsFalse(ret.IsSuccess);
		}


		/// <summary>
		/// 测试Gps_RuleID中文超长,have tested
		/// </summary>
		[TestMethod]
		public void TestGps_RuleIDStringForOverLength_Chinese()
		{
			GpsSettingInfo gpssettinginfo = new GpsSettingInfo();
			CommandManageService service = new CommandManageService();
			StringBuilder builder = new StringBuilder();
			Random random = new Random();
			for (int i = 0; i < 37; i++)
			{
				builder.Append(chinesecharacter[random.Next(chinesecharacter.Length - 1)]);
			}
			gpssettinginfo.Gps_RuleID=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 255; i++)
			{
				builder.Append(chinesecharacter[random.Next(chinesecharacter.Length - 1)]);
			}
			gpssettinginfo.Gps_RuleName=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 255; i++)
			{
				builder.Append(chinesecharacter[random.Next(chinesecharacter.Length - 1)]);
			}
			gpssettinginfo.Gps_Creator=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 999; i++)
			{
				builder.Append(chinesecharacter[random.Next(chinesecharacter.Length - 1)]);
			}
			gpssettinginfo.Gps_Description=builder.ToString();
			builder.Length=0;

			SingleMessage<bool> ret = service.GpsSettingAdd(gpssettinginfo);
			Assert.IsTrue(ret.IsSuccess);
		}


		/// <summary>
		/// 测试Gps_RuleID西文超长,have tested
		/// </summary>
		[TestMethod]
		public void TestGps_RuleIDStringForMaxLength_Spanish()
		{
			GpsSettingInfo gpssettinginfo = new GpsSettingInfo();
			CommandManageService service = new CommandManageService();
			StringBuilder builder = new StringBuilder();
			Random random = new Random();
			for (int i = 0; i < 37; i++)
			{
				builder.Append(spanishcharacter[random.Next(spanishcharacter.Length - 1)]);
			}
			gpssettinginfo.Gps_RuleID=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 255; i++)
			{
				builder.Append(spanishcharacter[random.Next(spanishcharacter.Length - 1)]);
			}
			gpssettinginfo.Gps_RuleName=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 255; i++)
			{
				builder.Append(spanishcharacter[random.Next(spanishcharacter.Length - 1)]);
			}
			gpssettinginfo.Gps_Creator=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 999; i++)
			{
				builder.Append(spanishcharacter[random.Next(spanishcharacter.Length - 1)]);
			}
			gpssettinginfo.Gps_Description=builder.ToString();
			builder.Length=0;

			SingleMessage<bool> ret = service.GpsSettingAdd(gpssettinginfo);
			//Assert.IsFalse(ret.IsSuccess);
		}


		/// <summary>
		/// 测试Gps_RuleID英文超长,have tested
		/// </summary>
		[TestMethod]
		public void TestGps_RuleIDStringForMaxLength_English()
		{
			GpsSettingInfo gpssettinginfo = new GpsSettingInfo();
			CommandManageService service = new CommandManageService();
			StringBuilder builder = new StringBuilder();
			Random random = new Random();
			for (int i = 0; i < 37; i++)
			{
				builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
			}
			gpssettinginfo.Gps_RuleID=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 255; i++)
			{
				builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
			}
			gpssettinginfo.Gps_RuleName=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 255; i++)
			{
				builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
			}
			gpssettinginfo.Gps_Creator=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 999; i++)
			{
				builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
			}
			gpssettinginfo.Gps_Description=builder.ToString();
			builder.Length=0;

			SingleMessage<bool> ret = service.GpsSettingAdd(gpssettinginfo);
			//Assert.IsFalse(ret.IsSuccess);
		}


		/// <summary>
		/// 测试Gps_RuleName中文超长,have tested
		/// </summary>
		[TestMethod]
		public void TestGps_RuleNameStringForOverLength_Chinese()
		{
			GpsSettingInfo gpssettinginfo = new GpsSettingInfo();
			CommandManageService service = new CommandManageService();
			StringBuilder builder = new StringBuilder();
			Random random = new Random();
			for (int i = 0; i < 17; i++)
			{
				builder.Append(chinesecharacter[random.Next(chinesecharacter.Length - 1)]);
			}
			gpssettinginfo.Gps_RuleID=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 513; i++)
			{
				builder.Append(chinesecharacter[random.Next(chinesecharacter.Length - 1)]);
			}
			gpssettinginfo.Gps_RuleName=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 255; i++)
			{
				builder.Append(chinesecharacter[random.Next(chinesecharacter.Length - 1)]);
			}
			gpssettinginfo.Gps_Creator=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 999; i++)
			{
				builder.Append(chinesecharacter[random.Next(chinesecharacter.Length - 1)]);
			}
			gpssettinginfo.Gps_Description=builder.ToString();
			builder.Length=0;

			SingleMessage<bool> ret = service.GpsSettingAdd(gpssettinginfo);
			//Assert.IsFalse(ret.IsSuccess);
		}


		/// <summary>
		/// 测试Gps_RuleName西文超长,have tested
		/// </summary>
		[TestMethod]
		public void TestGps_RuleNameStringForMaxLength_Spanish()
		{
			GpsSettingInfo gpssettinginfo = new GpsSettingInfo();
			CommandManageService service = new CommandManageService();
			StringBuilder builder = new StringBuilder();
			Random random = new Random();
			for (int i = 0; i < 17; i++)
			{
				builder.Append(spanishcharacter[random.Next(spanishcharacter.Length - 1)]);
			}
			gpssettinginfo.Gps_RuleID=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 513; i++)
			{
				builder.Append(spanishcharacter[random.Next(spanishcharacter.Length - 1)]);
			}
			gpssettinginfo.Gps_RuleName=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 255; i++)
			{
				builder.Append(spanishcharacter[random.Next(spanishcharacter.Length - 1)]);
			}
			gpssettinginfo.Gps_Creator=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 999; i++)
			{
				builder.Append(spanishcharacter[random.Next(spanishcharacter.Length - 1)]);
			}
			gpssettinginfo.Gps_Description=builder.ToString();
			builder.Length=0;

			SingleMessage<bool> ret = service.GpsSettingAdd(gpssettinginfo);
			//Assert.IsFalse(ret.IsSuccess);
		}


		/// <summary>
		/// 测试Gps_RuleName英文超长,have tested
		/// </summary>
		[TestMethod]
		public void TestGps_RuleNameStringForMaxLength_English()
		{
			GpsSettingInfo gpssettinginfo = new GpsSettingInfo();
			CommandManageService service = new CommandManageService();
			StringBuilder builder = new StringBuilder();
			Random random = new Random();
			for (int i = 0; i < 17; i++)
			{
				builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
			}
			gpssettinginfo.Gps_RuleID=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 513; i++)
			{
				builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
			}
			gpssettinginfo.Gps_RuleName=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 255; i++)
			{
				builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
			}
			gpssettinginfo.Gps_Creator=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 999; i++)
			{
				builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
			}
			gpssettinginfo.Gps_Description=builder.ToString();
			builder.Length=0;

			SingleMessage<bool> ret = service.GpsSettingAdd(gpssettinginfo);
			//Assert.IsFalse(ret.IsSuccess);
		}


		/// <summary>
		/// 测试Gps_IfMonitor中文超长,have tested
		/// </summary>
		[TestMethod]
		public void TestGps_IfMonitorStringForOverLength_Chinese()
		{
			GpsSettingInfo gpssettinginfo = new GpsSettingInfo();
			CommandManageService service = new CommandManageService();
			StringBuilder builder = new StringBuilder();
			Random random = new Random();
			for (int i = 0; i < 17; i++)
			{
				builder.Append(chinesecharacter[random.Next(chinesecharacter.Length - 1)]);
			}
			gpssettinginfo.Gps_RuleID=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 255; i++)
			{
				builder.Append(chinesecharacter[random.Next(chinesecharacter.Length - 1)]);
			}
			gpssettinginfo.Gps_RuleName=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 255; i++)
			{
				builder.Append(chinesecharacter[random.Next(chinesecharacter.Length - 1)]);
			}
			gpssettinginfo.Gps_Creator=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 999; i++)
			{
				builder.Append(chinesecharacter[random.Next(chinesecharacter.Length - 1)]);
			}
			gpssettinginfo.Gps_Description=builder.ToString();
			builder.Length=0;

			SingleMessage<bool> ret = service.GpsSettingAdd(gpssettinginfo);
			Assert.IsFalse(ret.IsSuccess);
		}


		/// <summary>
		/// 测试Gps_IfMonitor西文超长
		/// </summary>
		[TestMethod]
		public void TestGps_IfMonitorStringForMaxLength_Spanish()
		{
			GpsSettingInfo gpssettinginfo = new GpsSettingInfo();
			CommandManageService service = new CommandManageService();
			StringBuilder builder = new StringBuilder();
			Random random = new Random();
			for (int i = 0; i < 17; i++)
			{
				builder.Append(spanishcharacter[random.Next(spanishcharacter.Length - 1)]);
			}
			gpssettinginfo.Gps_RuleID=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 255; i++)
			{
				builder.Append(spanishcharacter[random.Next(spanishcharacter.Length - 1)]);
			}
			gpssettinginfo.Gps_RuleName=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 255; i++)
			{
				builder.Append(spanishcharacter[random.Next(spanishcharacter.Length - 1)]);
			}
			gpssettinginfo.Gps_Creator=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 999; i++)
			{
				builder.Append(spanishcharacter[random.Next(spanishcharacter.Length - 1)]);
			}
			gpssettinginfo.Gps_Description=builder.ToString();
			builder.Length=0;

			SingleMessage<bool> ret = service.GpsSettingAdd(gpssettinginfo);
			Assert.IsFalse(ret.IsSuccess);
		}


		/// <summary>
		/// 测试Gps_IfMonitor英文超长
		/// </summary>
		[TestMethod]
		public void TestGps_IfMonitorStringForMaxLength_English()
		{
			GpsSettingInfo gpssettinginfo = new GpsSettingInfo();
			CommandManageService service = new CommandManageService();
			StringBuilder builder = new StringBuilder();
			Random random = new Random();
			for (int i = 0; i < 17; i++)
			{
				builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
			}
			gpssettinginfo.Gps_RuleID=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 255; i++)
			{
				builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
			}
			gpssettinginfo.Gps_RuleName=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 255; i++)
			{
				builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
			}
			gpssettinginfo.Gps_Creator=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 999; i++)
			{
				builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
			}
			gpssettinginfo.Gps_Description=builder.ToString();
			builder.Length=0;

			SingleMessage<bool> ret = service.GpsSettingAdd(gpssettinginfo);
			//Assert.IsFalse(ret.IsSuccess);
		}


		/// <summary>
		/// 测试Gps_UploadSum中文超长
		/// </summary>
		[TestMethod]
		public void TestGps_UploadSumStringForOverLength_Chinese()
		{
			GpsSettingInfo gpssettinginfo = new GpsSettingInfo();
			CommandManageService service = new CommandManageService();
			StringBuilder builder = new StringBuilder();
			Random random = new Random();
			for (int i = 0; i < 17; i++)
			{
				builder.Append(chinesecharacter[random.Next(chinesecharacter.Length - 1)]);
			}
			gpssettinginfo.Gps_RuleID=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 255; i++)
			{
				builder.Append(chinesecharacter[random.Next(chinesecharacter.Length - 1)]);
			}
			gpssettinginfo.Gps_RuleName=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 255; i++)
			{
				builder.Append(chinesecharacter[random.Next(chinesecharacter.Length - 1)]);
			}
			gpssettinginfo.Gps_Creator=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 999; i++)
			{
				builder.Append(chinesecharacter[random.Next(chinesecharacter.Length - 1)]);
			}
			gpssettinginfo.Gps_Description=builder.ToString();
			builder.Length=0;

			SingleMessage<bool> ret = service.GpsSettingAdd(gpssettinginfo);
			//Assert.IsFalse(ret.IsSuccess);
		}


		/// <summary>
		/// 测试Gps_UploadSum西文超长
		/// </summary>
		[TestMethod]
		public void TestGps_UploadSumStringForMaxLength_Spanish()
		{
			GpsSettingInfo gpssettinginfo = new GpsSettingInfo();
			CommandManageService service = new CommandManageService();
			StringBuilder builder = new StringBuilder();
			Random random = new Random();
			for (int i = 0; i < 17; i++)
			{
				builder.Append(spanishcharacter[random.Next(spanishcharacter.Length - 1)]);
			}
			gpssettinginfo.Gps_RuleID=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 255; i++)
			{
				builder.Append(spanishcharacter[random.Next(spanishcharacter.Length - 1)]);
			}
			gpssettinginfo.Gps_RuleName=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 255; i++)
			{
				builder.Append(spanishcharacter[random.Next(spanishcharacter.Length - 1)]);
			}
			gpssettinginfo.Gps_Creator=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 999; i++)
			{
				builder.Append(spanishcharacter[random.Next(spanishcharacter.Length - 1)]);
			}
			gpssettinginfo.Gps_Description=builder.ToString();
			builder.Length=0;

			SingleMessage<bool> ret = service.GpsSettingAdd(gpssettinginfo);
			//Assert.IsFalse(ret.IsSuccess);
		}


		/// <summary>
		/// 测试Gps_UploadSum英文超长
		/// </summary>
		[TestMethod]
		public void TestGps_UploadSumStringForMaxLength_English()
		{
			GpsSettingInfo gpssettinginfo = new GpsSettingInfo();
			CommandManageService service = new CommandManageService();
			StringBuilder builder = new StringBuilder();
			Random random = new Random();
			for (int i = 0; i < 17; i++)
			{
				builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
			}
			gpssettinginfo.Gps_RuleID=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 255; i++)
			{
				builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
			}
			gpssettinginfo.Gps_RuleName=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 255; i++)
			{
				builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
			}
			gpssettinginfo.Gps_Creator=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 999; i++)
			{
				builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
			}
			gpssettinginfo.Gps_Description=builder.ToString();
			builder.Length=0;

			SingleMessage<bool> ret = service.GpsSettingAdd(gpssettinginfo);
			//Assert.IsFalse(ret.IsSuccess);
		}


		/// <summary>
		/// 测试Gps_UploadType中文超长
		/// </summary>
		[TestMethod]
		public void TestGps_UploadTypeStringForOverLength_Chinese()
		{
			GpsSettingInfo gpssettinginfo = new GpsSettingInfo();
			CommandManageService service = new CommandManageService();
			StringBuilder builder = new StringBuilder();
			Random random = new Random();
			for (int i = 0; i < 17; i++)
			{
				builder.Append(chinesecharacter[random.Next(chinesecharacter.Length - 1)]);
			}
			gpssettinginfo.Gps_RuleID=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 255; i++)
			{
				builder.Append(chinesecharacter[random.Next(chinesecharacter.Length - 1)]);
			}
			gpssettinginfo.Gps_RuleName=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 255; i++)
			{
				builder.Append(chinesecharacter[random.Next(chinesecharacter.Length - 1)]);
			}
			gpssettinginfo.Gps_Creator=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 999; i++)
			{
				builder.Append(chinesecharacter[random.Next(chinesecharacter.Length - 1)]);
			}
			gpssettinginfo.Gps_Description=builder.ToString();
			builder.Length=0;

			SingleMessage<bool> ret = service.GpsSettingAdd(gpssettinginfo);
			//Assert.IsFalse(ret.IsSuccess);
		}


		/// <summary>
		/// 测试Gps_UploadType西文超长
		/// </summary>
		[TestMethod]
		public void TestGps_UploadTypeStringForMaxLength_Spanish()
		{
			GpsSettingInfo gpssettinginfo = new GpsSettingInfo();
			CommandManageService service = new CommandManageService();
			StringBuilder builder = new StringBuilder();
			Random random = new Random();
			for (int i = 0; i < 17; i++)
			{
				builder.Append(spanishcharacter[random.Next(spanishcharacter.Length - 1)]);
			}
			gpssettinginfo.Gps_RuleID=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 255; i++)
			{
				builder.Append(spanishcharacter[random.Next(spanishcharacter.Length - 1)]);
			}
			gpssettinginfo.Gps_RuleName=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 255; i++)
			{
				builder.Append(spanishcharacter[random.Next(spanishcharacter.Length - 1)]);
			}
			gpssettinginfo.Gps_Creator=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 999; i++)
			{
				builder.Append(spanishcharacter[random.Next(spanishcharacter.Length - 1)]);
			}
			gpssettinginfo.Gps_Description=builder.ToString();
			builder.Length=0;

			SingleMessage<bool> ret = service.GpsSettingAdd(gpssettinginfo);
			//Assert.IsFalse(ret.IsSuccess);
		}


		/// <summary>
		/// 测试Gps_UploadType英文超长
		/// </summary>
		[TestMethod]
		public void TestGps_UploadTypeStringForMaxLength_English()
		{
			GpsSettingInfo gpssettinginfo = new GpsSettingInfo();
			CommandManageService service = new CommandManageService();
			StringBuilder builder = new StringBuilder();
			Random random = new Random();
			for (int i = 0; i < 17; i++)
			{
				builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
			}
			gpssettinginfo.Gps_RuleID=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 255; i++)
			{
				builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
			}
			gpssettinginfo.Gps_RuleName=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 255; i++)
			{
				builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
			}
			gpssettinginfo.Gps_Creator=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 999; i++)
			{
				builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
			}
			gpssettinginfo.Gps_Description=builder.ToString();
			builder.Length=0;

			SingleMessage<bool> ret = service.GpsSettingAdd(gpssettinginfo);
			//Assert.IsFalse(ret.IsSuccess);
		}


		/// <summary>
		/// 测试Gps_CreateTime中文超长
		/// </summary>
		[TestMethod]
		public void TestGps_CreateTimeStringForOverLength_Chinese()
		{
			GpsSettingInfo gpssettinginfo = new GpsSettingInfo();
			CommandManageService service = new CommandManageService();
			StringBuilder builder = new StringBuilder();
			Random random = new Random();
			for (int i = 0; i < 17; i++)
			{
				builder.Append(chinesecharacter[random.Next(chinesecharacter.Length - 1)]);
			}
			gpssettinginfo.Gps_RuleID=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 255; i++)
			{
				builder.Append(chinesecharacter[random.Next(chinesecharacter.Length - 1)]);
			}
			gpssettinginfo.Gps_RuleName=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 255; i++)
			{
				builder.Append(chinesecharacter[random.Next(chinesecharacter.Length - 1)]);
			}
			gpssettinginfo.Gps_Creator=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 999; i++)
			{
				builder.Append(chinesecharacter[random.Next(chinesecharacter.Length - 1)]);
			}
			gpssettinginfo.Gps_Description=builder.ToString();
			builder.Length=0;

			SingleMessage<bool> ret = service.GpsSettingAdd(gpssettinginfo);
			//Assert.IsFalse(ret.IsSuccess);
		}


		/// <summary>
		/// 测试Gps_CreateTime西文超长
		/// </summary>
		[TestMethod]
		public void TestGps_CreateTimeStringForMaxLength_Spanish()
		{
			GpsSettingInfo gpssettinginfo = new GpsSettingInfo();
			CommandManageService service = new CommandManageService();
			StringBuilder builder = new StringBuilder();
			Random random = new Random();
			for (int i = 0; i < 17; i++)
			{
				builder.Append(spanishcharacter[random.Next(spanishcharacter.Length - 1)]);
			}
			gpssettinginfo.Gps_RuleID=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 255; i++)
			{
				builder.Append(spanishcharacter[random.Next(spanishcharacter.Length - 1)]);
			}
			gpssettinginfo.Gps_RuleName=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 255; i++)
			{
				builder.Append(spanishcharacter[random.Next(spanishcharacter.Length - 1)]);
			}
			gpssettinginfo.Gps_Creator=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 999; i++)
			{
				builder.Append(spanishcharacter[random.Next(spanishcharacter.Length - 1)]);
			}
			gpssettinginfo.Gps_Description=builder.ToString();
			builder.Length=0;

			SingleMessage<bool> ret = service.GpsSettingAdd(gpssettinginfo);
			//Assert.IsFalse(ret.IsSuccess);
		}


		/// <summary>
		/// 测试Gps_CreateTime英文超长
		/// </summary>
		[TestMethod]
		public void TestGps_CreateTimeStringForMaxLength_English()
		{
			GpsSettingInfo gpssettinginfo = new GpsSettingInfo();
			CommandManageService service = new CommandManageService();
			StringBuilder builder = new StringBuilder();
			Random random = new Random();
			for (int i = 0; i < 17; i++)
			{
				builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
			}
			gpssettinginfo.Gps_RuleID=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 255; i++)
			{
				builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
			}
			gpssettinginfo.Gps_RuleName=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 255; i++)
			{
				builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
			}
			gpssettinginfo.Gps_Creator=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 999; i++)
			{
				builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
			}
			gpssettinginfo.Gps_Description=builder.ToString();
			builder.Length=0;

			SingleMessage<bool> ret = service.GpsSettingAdd(gpssettinginfo);
			//Assert.IsFalse(ret.IsSuccess);
		}


		/// <summary>
		/// 测试Gps_Creator中文超长
		/// </summary>
		[TestMethod]
		public void TestGps_CreatorStringForOverLength_Chinese()
		{
			GpsSettingInfo gpssettinginfo = new GpsSettingInfo();
			CommandManageService service = new CommandManageService();
			StringBuilder builder = new StringBuilder();
			Random random = new Random();
			for (int i = 0; i < 17; i++)
			{
				builder.Append(chinesecharacter[random.Next(chinesecharacter.Length - 1)]);
			}
			gpssettinginfo.Gps_RuleID=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 255; i++)
			{
				builder.Append(chinesecharacter[random.Next(chinesecharacter.Length - 1)]);
			}
			gpssettinginfo.Gps_RuleName=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 256; i++)
			{
				builder.Append(chinesecharacter[random.Next(chinesecharacter.Length - 1)]);
			}
			gpssettinginfo.Gps_Creator=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 999; i++)
			{
				builder.Append(chinesecharacter[random.Next(chinesecharacter.Length - 1)]);
			}
			gpssettinginfo.Gps_Description=builder.ToString();
			builder.Length=0;

			SingleMessage<bool> ret = service.GpsSettingAdd(gpssettinginfo);
			//Assert.IsFalse(ret.IsSuccess);
		}


		/// <summary>
		/// 测试Gps_Creator西文超长
		/// </summary>
		[TestMethod]
		public void TestGps_CreatorStringForMaxLength_Spanish()
		{
			GpsSettingInfo gpssettinginfo = new GpsSettingInfo();
			CommandManageService service = new CommandManageService();
			StringBuilder builder = new StringBuilder();
			Random random = new Random();
			for (int i = 0; i < 17; i++)
			{
				builder.Append(spanishcharacter[random.Next(spanishcharacter.Length - 1)]);
			}
			gpssettinginfo.Gps_RuleID=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 255; i++)
			{
				builder.Append(spanishcharacter[random.Next(spanishcharacter.Length - 1)]);
			}
			gpssettinginfo.Gps_RuleName=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 256; i++)
			{
				builder.Append(spanishcharacter[random.Next(spanishcharacter.Length - 1)]);
			}
			gpssettinginfo.Gps_Creator=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 999; i++)
			{
				builder.Append(spanishcharacter[random.Next(spanishcharacter.Length - 1)]);
			}
			gpssettinginfo.Gps_Description=builder.ToString();
			builder.Length=0;

			SingleMessage<bool> ret = service.GpsSettingAdd(gpssettinginfo);
			//Assert.IsFalse(ret.IsSuccess);
		}


		/// <summary>
		/// 测试Gps_Creator英文超长
		/// </summary>
		[TestMethod]
		public void TestGps_CreatorStringForMaxLength_English()
		{
			GpsSettingInfo gpssettinginfo = new GpsSettingInfo();
			CommandManageService service = new CommandManageService();
			StringBuilder builder = new StringBuilder();
			Random random = new Random();
			for (int i = 0; i < 17; i++)
			{
				builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
			}
			gpssettinginfo.Gps_RuleID=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 255; i++)
			{
				builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
			}
			gpssettinginfo.Gps_RuleName=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 256; i++)
			{
				builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
			}
			gpssettinginfo.Gps_Creator=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 999; i++)
			{
				builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
			}
			gpssettinginfo.Gps_Description=builder.ToString();
			builder.Length=0;

			SingleMessage<bool> ret = service.GpsSettingAdd(gpssettinginfo);
			//Assert.IsFalse(ret.IsSuccess);
		}


		/// <summary>
		/// 测试Gps_Description中文超长
		/// </summary>
		[TestMethod]
		public void TestGps_DescriptionStringForOverLength_Chinese()
		{
			GpsSettingInfo gpssettinginfo = new GpsSettingInfo();
			CommandManageService service = new CommandManageService();
			StringBuilder builder = new StringBuilder();
			Random random = new Random();
			for (int i = 0; i < 17; i++)
			{
				builder.Append(chinesecharacter[random.Next(chinesecharacter.Length - 1)]);
			}
			gpssettinginfo.Gps_RuleID=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 255; i++)
			{
				builder.Append(chinesecharacter[random.Next(chinesecharacter.Length - 1)]);
			}
			gpssettinginfo.Gps_RuleName=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 255; i++)
			{
				builder.Append(chinesecharacter[random.Next(chinesecharacter.Length - 1)]);
			}
			gpssettinginfo.Gps_Creator=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 1000; i++)
			{
				builder.Append(chinesecharacter[random.Next(chinesecharacter.Length - 1)]);
			}
			gpssettinginfo.Gps_Description=builder.ToString();
			builder.Length=0;

			SingleMessage<bool> ret = service.GpsSettingAdd(gpssettinginfo);
			//Assert.IsFalse(ret.IsSuccess);
		}


		/// <summary>
		/// 测试Gps_Description西文超长
		/// </summary>
		[TestMethod]
		public void TestGps_DescriptionStringForMaxLength_Spanish()
		{
			GpsSettingInfo gpssettinginfo = new GpsSettingInfo();
			CommandManageService service = new CommandManageService();
			StringBuilder builder = new StringBuilder();
			Random random = new Random();
			for (int i = 0; i < 17; i++)
			{
				builder.Append(spanishcharacter[random.Next(spanishcharacter.Length - 1)]);
			}
			gpssettinginfo.Gps_RuleID=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 255; i++)
			{
				builder.Append(spanishcharacter[random.Next(spanishcharacter.Length - 1)]);
			}
			gpssettinginfo.Gps_RuleName=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 255; i++)
			{
				builder.Append(spanishcharacter[random.Next(spanishcharacter.Length - 1)]);
			}
			gpssettinginfo.Gps_Creator=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 1000; i++)
			{
				builder.Append(spanishcharacter[random.Next(spanishcharacter.Length - 1)]);
			}
			gpssettinginfo.Gps_Description=builder.ToString();
			builder.Length=0;

			SingleMessage<bool> ret = service.GpsSettingAdd(gpssettinginfo);
			//Assert.IsFalse(ret.IsSuccess);
		}


		/// <summary>
		/// 测试Gps_Description英文超长
		/// </summary>
		[TestMethod]
		public void TestGps_DescriptionStringForMaxLength_English()
		{
			GpsSettingInfo gpssettinginfo = new GpsSettingInfo();
			CommandManageService service = new CommandManageService();
			StringBuilder builder = new StringBuilder();
			Random random = new Random();
			for (int i = 0; i < 17; i++)
			{
				builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
			}
			gpssettinginfo.Gps_RuleID=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 255; i++)
			{
				builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
			}
			gpssettinginfo.Gps_RuleName=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 255; i++)
			{
				builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
			}
			gpssettinginfo.Gps_Creator=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 1000; i++)
			{
				builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
			}
			gpssettinginfo.Gps_Description=builder.ToString();
			builder.Length=0;

			SingleMessage<bool> ret = service.GpsSettingAdd(gpssettinginfo);
			//Assert.IsFalse(ret.IsSuccess);
		}


		/// <summary>
		/// 测试Gps_Distance中文超长
		/// </summary>
		[TestMethod]
		public void TestGps_DistanceStringForOverLength_Chinese()
		{
			GpsSettingInfo gpssettinginfo = new GpsSettingInfo();
			CommandManageService service = new CommandManageService();
			StringBuilder builder = new StringBuilder();
			Random random = new Random();
			for (int i = 0; i < 17; i++)
			{
				builder.Append(chinesecharacter[random.Next(chinesecharacter.Length - 1)]);
			}
			gpssettinginfo.Gps_RuleID=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 255; i++)
			{
				builder.Append(chinesecharacter[random.Next(chinesecharacter.Length - 1)]);
			}
			gpssettinginfo.Gps_RuleName=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 255; i++)
			{
				builder.Append(chinesecharacter[random.Next(chinesecharacter.Length - 1)]);
			}
			gpssettinginfo.Gps_Creator=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 999; i++)
			{
				builder.Append(chinesecharacter[random.Next(chinesecharacter.Length - 1)]);
			}
			gpssettinginfo.Gps_Description=builder.ToString();
			builder.Length=0;

			SingleMessage<bool> ret = service.GpsSettingAdd(gpssettinginfo);
			//Assert.IsFalse(ret.IsSuccess);
		}


		/// <summary>
		/// 测试Gps_Distance西文超长
		/// </summary>
		[TestMethod]
		public void TestGps_DistanceStringForMaxLength_Spanish()
		{
			GpsSettingInfo gpssettinginfo = new GpsSettingInfo();
			CommandManageService service = new CommandManageService();
			StringBuilder builder = new StringBuilder();
			Random random = new Random();
			for (int i = 0; i < 17; i++)
			{
				builder.Append(spanishcharacter[random.Next(spanishcharacter.Length - 1)]);
			}
			gpssettinginfo.Gps_RuleID=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 255; i++)
			{
				builder.Append(spanishcharacter[random.Next(spanishcharacter.Length - 1)]);
			}
			gpssettinginfo.Gps_RuleName=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 255; i++)
			{
				builder.Append(spanishcharacter[random.Next(spanishcharacter.Length - 1)]);
			}
			gpssettinginfo.Gps_Creator=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 999; i++)
			{
				builder.Append(spanishcharacter[random.Next(spanishcharacter.Length - 1)]);
			}
			gpssettinginfo.Gps_Description=builder.ToString();
			builder.Length=0;

			SingleMessage<bool> ret = service.GpsSettingAdd(gpssettinginfo);
			//Assert.IsFalse(ret.IsSuccess);
		}


		/// <summary>
		/// 测试Gps_Distance英文超长
		/// </summary>
		[TestMethod]
		public void TestGps_DistanceStringForMaxLength_English()
		{
			GpsSettingInfo gpssettinginfo = new GpsSettingInfo();
			CommandManageService service = new CommandManageService();
			StringBuilder builder = new StringBuilder();
			Random random = new Random();
			for (int i = 0; i < 17; i++)
			{
				builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
			}
			gpssettinginfo.Gps_RuleID=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 255; i++)
			{
				builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
			}
			gpssettinginfo.Gps_RuleName=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 255; i++)
			{
				builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
			}
			gpssettinginfo.Gps_Creator=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 999; i++)
			{
				builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
			}
			gpssettinginfo.Gps_Description=builder.ToString();
			builder.Length=0;

			SingleMessage<bool> ret = service.GpsSettingAdd(gpssettinginfo);
			//Assert.IsFalse(ret.IsSuccess);
		}


		/// <summary>
		/// 测试Gps_Time中文超长
		/// </summary>
		[TestMethod]
		public void TestGps_TimeStringForOverLength_Chinese()
		{
			GpsSettingInfo gpssettinginfo = new GpsSettingInfo();
			CommandManageService service = new CommandManageService();
			StringBuilder builder = new StringBuilder();
			Random random = new Random();
			for (int i = 0; i < 17; i++)
			{
				builder.Append(chinesecharacter[random.Next(chinesecharacter.Length - 1)]);
			}
			gpssettinginfo.Gps_RuleID=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 255; i++)
			{
				builder.Append(chinesecharacter[random.Next(chinesecharacter.Length - 1)]);
			}
			gpssettinginfo.Gps_RuleName=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 255; i++)
			{
				builder.Append(chinesecharacter[random.Next(chinesecharacter.Length - 1)]);
			}
			gpssettinginfo.Gps_Creator=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 999; i++)
			{
				builder.Append(chinesecharacter[random.Next(chinesecharacter.Length - 1)]);
			}
			gpssettinginfo.Gps_Description=builder.ToString();
			builder.Length=0;

			SingleMessage<bool> ret = service.GpsSettingAdd(gpssettinginfo);
			//Assert.IsFalse(ret.IsSuccess);
		}


		/// <summary>
		/// 测试Gps_Time西文超长
		/// </summary>
		[TestMethod]
		public void TestGps_TimeStringForMaxLength_Spanish()
		{
			GpsSettingInfo gpssettinginfo = new GpsSettingInfo();
			CommandManageService service = new CommandManageService();
			StringBuilder builder = new StringBuilder();
			Random random = new Random();
			for (int i = 0; i < 17; i++)
			{
				builder.Append(spanishcharacter[random.Next(spanishcharacter.Length - 1)]);
			}
			gpssettinginfo.Gps_RuleID=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 255; i++)
			{
				builder.Append(spanishcharacter[random.Next(spanishcharacter.Length - 1)]);
			}
			gpssettinginfo.Gps_RuleName=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 255; i++)
			{
				builder.Append(spanishcharacter[random.Next(spanishcharacter.Length - 1)]);
			}
			gpssettinginfo.Gps_Creator=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 999; i++)
			{
				builder.Append(spanishcharacter[random.Next(spanishcharacter.Length - 1)]);
			}
			gpssettinginfo.Gps_Description=builder.ToString();
			builder.Length=0;

			SingleMessage<bool> ret = service.GpsSettingAdd(gpssettinginfo);
			//Assert.IsFalse(ret.IsSuccess);
		}


		/// <summary>
		/// 测试Gps_Time英文超长
		/// </summary>
		[TestMethod]
		public void TestGps_TimeStringForMaxLength_English()
		{
			GpsSettingInfo gpssettinginfo = new GpsSettingInfo();
			CommandManageService service = new CommandManageService();
			StringBuilder builder = new StringBuilder();
			Random random = new Random();
			for (int i = 0; i < 17; i++)
			{
				builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
			}
			gpssettinginfo.Gps_RuleID=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 255; i++)
			{
				builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
			}
			gpssettinginfo.Gps_RuleName=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 255; i++)
			{
				builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
			}
			gpssettinginfo.Gps_Creator=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 999; i++)
			{
				builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
			}
			gpssettinginfo.Gps_Description=builder.ToString();
			builder.Length=0;

			SingleMessage<bool> ret = service.GpsSettingAdd(gpssettinginfo);
			//Assert.IsFalse(ret.IsSuccess);
		}


		/// <summary>
		/// 测试Gps_VehicleCount中文超长
		/// </summary>
		[TestMethod]
		public void TestGps_VehicleCountStringForOverLength_Chinese()
		{
			GpsSettingInfo gpssettinginfo = new GpsSettingInfo();
			CommandManageService service = new CommandManageService();
			StringBuilder builder = new StringBuilder();
			Random random = new Random();
			for (int i = 0; i < 17; i++)
			{
				builder.Append(chinesecharacter[random.Next(chinesecharacter.Length - 1)]);
			}
			gpssettinginfo.Gps_RuleID=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 255; i++)
			{
				builder.Append(chinesecharacter[random.Next(chinesecharacter.Length - 1)]);
			}
			gpssettinginfo.Gps_RuleName=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 255; i++)
			{
				builder.Append(chinesecharacter[random.Next(chinesecharacter.Length - 1)]);
			}
			gpssettinginfo.Gps_Creator=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 999; i++)
			{
				builder.Append(chinesecharacter[random.Next(chinesecharacter.Length - 1)]);
			}
			gpssettinginfo.Gps_Description=builder.ToString();
			builder.Length=0;

			SingleMessage<bool> ret = service.GpsSettingAdd(gpssettinginfo);
			//Assert.IsFalse(ret.IsSuccess);
		}


		/// <summary>
		/// 测试Gps_VehicleCount西文超长
		/// </summary>
		[TestMethod]
		public void TestGps_VehicleCountStringForMaxLength_Spanish()
		{
			GpsSettingInfo gpssettinginfo = new GpsSettingInfo();
			CommandManageService service = new CommandManageService();
			StringBuilder builder = new StringBuilder();
			Random random = new Random();
			for (int i = 0; i < 17; i++)
			{
				builder.Append(spanishcharacter[random.Next(spanishcharacter.Length - 1)]);
			}
			gpssettinginfo.Gps_RuleID=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 255; i++)
			{
				builder.Append(spanishcharacter[random.Next(spanishcharacter.Length - 1)]);
			}
			gpssettinginfo.Gps_RuleName=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 255; i++)
			{
				builder.Append(spanishcharacter[random.Next(spanishcharacter.Length - 1)]);
			}
			gpssettinginfo.Gps_Creator=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 999; i++)
			{
				builder.Append(spanishcharacter[random.Next(spanishcharacter.Length - 1)]);
			}
			gpssettinginfo.Gps_Description=builder.ToString();
			builder.Length=0;

			SingleMessage<bool> ret = service.GpsSettingAdd(gpssettinginfo);
			//Assert.IsFalse(ret.IsSuccess);
		}


		/// <summary>
		/// 测试Gps_VehicleCount英文超长
		/// </summary>
		[TestMethod]
		public void TestGps_VehicleCountStringForMaxLength_English()
		{
			GpsSettingInfo gpssettinginfo = new GpsSettingInfo();
			CommandManageService service = new CommandManageService();
			StringBuilder builder = new StringBuilder();
			Random random = new Random();
			for (int i = 0; i < 17; i++)
			{
				builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
			}
			gpssettinginfo.Gps_RuleID=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 255; i++)
			{
				builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
			}
			gpssettinginfo.Gps_RuleName=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 255; i++)
			{
				builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
			}
			gpssettinginfo.Gps_Creator=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 999; i++)
			{
				builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
			}
			gpssettinginfo.Gps_Description=builder.ToString();
			builder.Length=0;

			SingleMessage<bool> ret = service.GpsSettingAdd(gpssettinginfo);
			//Assert.IsFalse(ret.IsSuccess);
		}


		/// <summary>
		/// 测试Gps_IsDefault中文超长
		/// </summary>
		[TestMethod]
		public void TestGps_IsDefaultStringForOverLength_Chinese()
		{
			GpsSettingInfo gpssettinginfo = new GpsSettingInfo();
			CommandManageService service = new CommandManageService();
			StringBuilder builder = new StringBuilder();
			Random random = new Random();
			for (int i = 0; i < 17; i++)
			{
				builder.Append(chinesecharacter[random.Next(chinesecharacter.Length - 1)]);
			}
			gpssettinginfo.Gps_RuleID=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 255; i++)
			{
				builder.Append(chinesecharacter[random.Next(chinesecharacter.Length - 1)]);
			}
			gpssettinginfo.Gps_RuleName=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 255; i++)
			{
				builder.Append(chinesecharacter[random.Next(chinesecharacter.Length - 1)]);
			}
			gpssettinginfo.Gps_Creator=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 999; i++)
			{
				builder.Append(chinesecharacter[random.Next(chinesecharacter.Length - 1)]);
			}
			gpssettinginfo.Gps_Description=builder.ToString();
			builder.Length=0;

			SingleMessage<bool> ret = service.GpsSettingAdd(gpssettinginfo);
			//Assert.IsFalse(ret.IsSuccess);
		}


		/// <summary>
		/// 测试Gps_IsDefault西文超长
		/// </summary>
		[TestMethod]
		public void TestGps_IsDefaultStringForMaxLength_Spanish()
		{
			GpsSettingInfo gpssettinginfo = new GpsSettingInfo();
			CommandManageService service = new CommandManageService();
			StringBuilder builder = new StringBuilder();
			Random random = new Random();
			for (int i = 0; i < 17; i++)
			{
				builder.Append(spanishcharacter[random.Next(spanishcharacter.Length - 1)]);
			}
			gpssettinginfo.Gps_RuleID=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 255; i++)
			{
				builder.Append(spanishcharacter[random.Next(spanishcharacter.Length - 1)]);
			}
			gpssettinginfo.Gps_RuleName=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 255; i++)
			{
				builder.Append(spanishcharacter[random.Next(spanishcharacter.Length - 1)]);
			}
			gpssettinginfo.Gps_Creator=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 999; i++)
			{
				builder.Append(spanishcharacter[random.Next(spanishcharacter.Length - 1)]);
			}
			gpssettinginfo.Gps_Description=builder.ToString();
			builder.Length=0;

			SingleMessage<bool> ret = service.GpsSettingAdd(gpssettinginfo);
			//Assert.IsFalse(ret.IsSuccess);
		}


		/// <summary>
		/// 测试Gps_IsDefault英文超长
		/// </summary>
		[TestMethod]
		public void TestGps_IsDefaultStringForMaxLength_English()
		{
			GpsSettingInfo gpssettinginfo = new GpsSettingInfo();
			CommandManageService service = new CommandManageService();
			StringBuilder builder = new StringBuilder();
			Random random = new Random();
			for (int i = 0; i < 17; i++)
			{
				builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
			}
			gpssettinginfo.Gps_RuleID=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 255; i++)
			{
				builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
			}
			gpssettinginfo.Gps_RuleName=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 255; i++)
			{
				builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
			}
			gpssettinginfo.Gps_Creator=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 999; i++)
			{
				builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
			}
			gpssettinginfo.Gps_Description=builder.ToString();
			builder.Length=0;

			SingleMessage<bool> ret = service.GpsSettingAdd(gpssettinginfo);
			//Assert.IsFalse(ret.IsSuccess);
		}


		/// <summary>
		/// 测试Gps_Valid中文超长
		/// </summary>
		[TestMethod]
		public void TestGps_ValidStringForOverLength_Chinese()
		{
			GpsSettingInfo gpssettinginfo = new GpsSettingInfo();
			CommandManageService service = new CommandManageService();
			StringBuilder builder = new StringBuilder();
			Random random = new Random();
			for (int i = 0; i < 17; i++)
			{
				builder.Append(chinesecharacter[random.Next(chinesecharacter.Length - 1)]);
			}
			gpssettinginfo.Gps_RuleID=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 255; i++)
			{
				builder.Append(chinesecharacter[random.Next(chinesecharacter.Length - 1)]);
			}
			gpssettinginfo.Gps_RuleName=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 255; i++)
			{
				builder.Append(chinesecharacter[random.Next(chinesecharacter.Length - 1)]);
			}
			gpssettinginfo.Gps_Creator=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 999; i++)
			{
				builder.Append(chinesecharacter[random.Next(chinesecharacter.Length - 1)]);
			}
			gpssettinginfo.Gps_Description=builder.ToString();
			builder.Length=0;

			SingleMessage<bool> ret = service.GpsSettingAdd(gpssettinginfo);
			//Assert.IsFalse(ret.IsSuccess);
		}


		/// <summary>
		/// 测试Gps_Valid西文超长
		/// </summary>
		[TestMethod]
		public void TestGps_ValidStringForMaxLength_Spanish()
		{
			GpsSettingInfo gpssettinginfo = new GpsSettingInfo();
			CommandManageService service = new CommandManageService();
			StringBuilder builder = new StringBuilder();
			Random random = new Random();
			for (int i = 0; i < 17; i++)
			{
				builder.Append(spanishcharacter[random.Next(spanishcharacter.Length - 1)]);
			}
			gpssettinginfo.Gps_RuleID=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 255; i++)
			{
				builder.Append(spanishcharacter[random.Next(spanishcharacter.Length - 1)]);
			}
			gpssettinginfo.Gps_RuleName=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 255; i++)
			{
				builder.Append(spanishcharacter[random.Next(spanishcharacter.Length - 1)]);
			}
			gpssettinginfo.Gps_Creator=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 999; i++)
			{
				builder.Append(spanishcharacter[random.Next(spanishcharacter.Length - 1)]);
			}
			gpssettinginfo.Gps_Description=builder.ToString();
			builder.Length=0;

			SingleMessage<bool> ret = service.GpsSettingAdd(gpssettinginfo);
			//Assert.IsFalse(ret.IsSuccess);
		}


		/// <summary>
		/// 测试Gps_Valid英文超长
		/// </summary>
		[TestMethod]
		public void TestGps_ValidStringForMaxLength_English()
		{
			GpsSettingInfo gpssettinginfo = new GpsSettingInfo();
			CommandManageService service = new CommandManageService();
			StringBuilder builder = new StringBuilder();
			Random random = new Random();
			for (int i = 0; i < 17; i++)
			{
				builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
			}
			gpssettinginfo.Gps_RuleID=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 255; i++)
			{
				builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
			}
			gpssettinginfo.Gps_RuleName=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 255; i++)
			{
				builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
			}
			gpssettinginfo.Gps_Creator=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 999; i++)
			{
				builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
			}
			gpssettinginfo.Gps_Description=builder.ToString();
			builder.Length=0;

			SingleMessage<bool> ret = service.GpsSettingAdd(gpssettinginfo);
			//Assert.IsFalse(ret.IsSuccess);
		}


		/// <summary>
		/// 测试空格
		/// </summary>
		[TestMethod]
		public void TestStringForSpaceGps()
		{
			GpsSettingInfo gpssettinginfo = new GpsSettingInfo();
			CommandManageService service = new CommandManageService();
            Random random = new Random();
			StringBuilder builder = new StringBuilder();
            builder.AppendLine("			Random random = new Random();");
builder.Append(" ");
			for (int i = 1; i < 17; i++)
			{
				builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
			}
builder.Append(" ");
			gpssettinginfo.Gps_RuleID=builder.ToString();
			builder.Length=0;

builder.Append(" ");
			for (int i = 1; i < 255; i++)
			{
				builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
			}
builder.Append(" ");
			gpssettinginfo.Gps_RuleName=builder.ToString();
			builder.Length=0;

builder.Append(" ");
			for (int i = 1; i < 255; i++)
			{
				builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
			}
builder.Append(" ");
			gpssettinginfo.Gps_Creator=builder.ToString();
			builder.Length=0;

builder.Append(" ");
			for (int i = 1; i < 999; i++)
			{
				builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
			}
builder.Append(" ");
			gpssettinginfo.Gps_Description=builder.ToString();
			builder.Length=0;

			SingleMessage<bool> ret = service.GpsSettingAdd(gpssettinginfo);
			//Assert.IsFalse(ret.IsSuccess);
		}


		/// <summary>
		/// 测试,
		/// </summary>
		[TestMethod]
		public void TestStringForSpecialCharacter_CommaGps()
		{
			GpsSettingInfo gpssettinginfo = new GpsSettingInfo();
			CommandManageService service = new CommandManageService();
			StringBuilder builder = new StringBuilder();
            Random random = new Random();
            builder.AppendLine("			Random random = new Random();");
builder.Append(",");
			for (int i = 1; i < 17; i++)
			{
				builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
			}
builder.Append(",");
			gpssettinginfo.Gps_RuleID=builder.ToString();
			builder.Length=0;

builder.Append(",");
			for (int i = 1; i < 255; i++)
			{
				builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
			}
builder.Append(",");
			gpssettinginfo.Gps_RuleName=builder.ToString();
			builder.Length=0;

builder.Append(",");
			for (int i = 1; i < 255; i++)
			{
				builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
			}
builder.Append(",");
			gpssettinginfo.Gps_Creator=builder.ToString();
			builder.Length=0;

builder.Append(",");
			for (int i = 1; i < 999; i++)
			{
				builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
			}
builder.Append(",");
			gpssettinginfo.Gps_Description=builder.ToString();
			builder.Length=0;

			SingleMessage<bool> ret = service.GpsSettingAdd(gpssettinginfo);
			//Assert.IsFalse(ret.IsSuccess);
		}


		/// <summary>
		/// 测试;
		/// </summary>
		[TestMethod]
		public void TestStringForSpecialCharacter_SemiCommaGps()
		{
			GpsSettingInfo gpssettinginfo = new GpsSettingInfo();
			CommandManageService service = new CommandManageService();
			StringBuilder builder = new StringBuilder();
            Random random = new Random();
            builder.AppendLine("			Random random = new Random();");
builder.Append(";");
			for (int i = 1; i < 17; i++)
			{
				builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
			}
builder.Append(";");
			gpssettinginfo.Gps_RuleID=builder.ToString();
			builder.Length=0;

builder.Append(";");
			for (int i = 1; i < 255; i++)
			{
				builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
			}
builder.Append(";");
			gpssettinginfo.Gps_RuleName=builder.ToString();
			builder.Length=0;

builder.Append(";");
			for (int i = 1; i < 255; i++)
			{
				builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
			}
builder.Append(";");
			gpssettinginfo.Gps_Creator=builder.ToString();
			builder.Length=0;

builder.Append(";");
			for (int i = 1; i < 999; i++)
			{
				builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
			}
builder.Append(";");
			gpssettinginfo.Gps_Description=builder.ToString();
			builder.Length=0;

			SingleMessage<bool> ret = service.GpsSettingAdd(gpssettinginfo);
			//Assert.IsFalse(ret.IsSuccess);
		}


		/// <summary>
		/// 测试;
		/// </summary>
//        [TestMethod]
//        public void TestStringForSpecialCharacter_quote()
//        {
//            GpsSettingInfo gpssettinginfo = new GpsSettingInfo();
//            CommandManageService service = new CommandManageService();
//            StringBuilder builder = new StringBuilder();
//            builder.AppendLine("			Random random = new Random();");
//builder.Append(""");
//            for (int i = 1; i < 17; i++)
//            {
//                builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
//            }
//builder.Append(""");
//            gpssettinginfo.Gps_RuleID=builder.ToString();
//            builder.Length=0;

//builder.Append(""");
//            for (int i = 1; i < 255; i++)
//            {
//                builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
//            }
//builder.Append(""");
//            gpssettinginfo.Gps_RuleName=builder.ToString();
//            builder.Length=0;

//builder.Append(""");
//            for (int i = 1; i < 255; i++)
//            {
//                builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
//            }
//builder.Append(""");
//            gpssettinginfo.Gps_Creator=builder.ToString();
//            builder.Length=0;

//builder.Append(""");
//            for (int i = 1; i < 999; i++)
//            {
//                builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
//            }
//builder.Append(""");
//            gpssettinginfo.Gps_Description=builder.ToString();
//            builder.Length=0;

//            SingleMessage<bool> ret = service.GpsSettingAdd(gpssettinginfo);
//            //Assert.IsFalse(ret.IsSuccess);
//        }


		/// <summary>
		/// 测试select
		/// </summary>
		[TestMethod]
        public void TestStringForSpecialCharacter_quoteselectGps()
		{
			GpsSettingInfo gpssettinginfo = new GpsSettingInfo();
			CommandManageService service = new CommandManageService();
			gpssettinginfo.Gps_RuleID="yui";

            gpssettinginfo.Gps_RuleName = "select";

            gpssettinginfo.Gps_Creator = "select";

            gpssettinginfo.Gps_Description = "select";

			SingleMessage<bool> ret = service.GpsSettingAdd(gpssettinginfo);
			Assert.IsTrue(ret.IsSuccess);
		}


		/// <summary>
		/// 测试update
		/// </summary>
		[TestMethod]
		public void TestStringForSpecialCharacter_quoteGps()
		{
			GpsSettingInfo gpssettinginfo = new GpsSettingInfo();
			CommandManageService service = new CommandManageService();
			gpssettinginfo.Gps_RuleID="234ad";

			gpssettinginfo.Gps_RuleName="update";

			gpssettinginfo.Gps_Creator="update";

			gpssettinginfo.Gps_Description="update";

			SingleMessage<bool> ret = service.GpsSettingAdd(gpssettinginfo);
			Assert.IsTrue(ret.IsSuccess);
		}


		/// <summary>
		/// 测试delete
		/// </summary>
		[TestMethod]
        public void TestStringForSpecialCharacter_quotedeleteGps()
		{
			GpsSettingInfo gpssettinginfo = new GpsSettingInfo();
			CommandManageService service = new CommandManageService();
			gpssettinginfo.Gps_RuleID="delete";

			gpssettinginfo.Gps_RuleName="delete";

			gpssettinginfo.Gps_Creator="delete";

			gpssettinginfo.Gps_Description="delete";

			SingleMessage<bool> ret = service.GpsSettingAdd(gpssettinginfo);
			//Assert.IsFalse(ret.IsSuccess);
		}


		/// <summary>
		/// 测试最大值
		/// </summary>
		[TestMethod]
		public void TestNumberForMaxGps()
		{
			GpsSettingInfo gpssettinginfo = new GpsSettingInfo();
			CommandManageService service = new CommandManageService();
			gpssettinginfo.Gps_RuleID="";
			gpssettinginfo.Gps_RuleName="";
			gpssettinginfo.Gps_Creator="";
			gpssettinginfo.Gps_Description="";
			SingleMessage<bool> ret = service.GpsSettingAdd(gpssettinginfo);
			//Assert.IsFalse(ret.IsSuccess);
		}


		/// <summary>
		/// 测试最小值
		/// </summary>
		[TestMethod]
		public void TestNumberForMinGps()
		{
			GpsSettingInfo gpssettinginfo = new GpsSettingInfo();
			CommandManageService service = new CommandManageService();
			gpssettinginfo.Gps_RuleID="";
			gpssettinginfo.Gps_RuleName="";
			gpssettinginfo.Gps_Creator="";
			gpssettinginfo.Gps_Description="";
			SingleMessage<bool> ret = service.GpsSettingAdd(gpssettinginfo);
			//Assert.IsFalse(ret.IsSuccess);
		}


		/// <summary>
		/// 正数
		/// </summary>
		[TestMethod]
		public void TestNumberForPositiveGps()
		{
			GpsSettingInfo gpssettinginfo = new GpsSettingInfo();
			CommandManageService service = new CommandManageService();
			gpssettinginfo.Gps_RuleID="";
			gpssettinginfo.Gps_RuleName="";
			gpssettinginfo.Gps_Creator="";
			gpssettinginfo.Gps_Description="";
			SingleMessage<bool> ret = service.GpsSettingAdd(gpssettinginfo);
			//Assert.IsFalse(ret.IsSuccess);
		}


		/// <summary>
		/// 负数
		/// </summary>
		[TestMethod]
		public void TestNumberForNegativeGps()
		{
			GpsSettingInfo gpssettinginfo = new GpsSettingInfo();
			CommandManageService service = new CommandManageService();
			gpssettinginfo.Gps_RuleID="";
			gpssettinginfo.Gps_RuleName="";
			gpssettinginfo.Gps_Creator="";
			gpssettinginfo.Gps_Description="";
			SingleMessage<bool> ret = service.GpsSettingAdd(gpssettinginfo);
			//Assert.IsFalse(ret.IsSuccess);
		}


		/// <summary>
		/// 零
		/// </summary>
		[TestMethod]
		public void TestNumberForZeroGps()
		{
			GpsSettingInfo gpssettinginfo = new GpsSettingInfo();
			CommandManageService service = new CommandManageService();
			gpssettinginfo.Gps_RuleID="";
			gpssettinginfo.Gps_RuleName="";
			gpssettinginfo.Gps_Creator="";
			gpssettinginfo.Gps_Description="";
			SingleMessage<bool> ret = service.GpsSettingAdd(gpssettinginfo);
			//Assert.IsFalse(ret.IsSuccess);
		}


		/// <summary>
		/// 小数
		/// </summary>
		[TestMethod]
		public void TestNumberForPortion()
		{
			GpsSettingInfo gpssettinginfo = new GpsSettingInfo();
			CommandManageService service = new CommandManageService();
			gpssettinginfo.Gps_RuleID="";
			gpssettinginfo.Gps_RuleName="";
			gpssettinginfo.Gps_Creator="";
			gpssettinginfo.Gps_Description="";
			SingleMessage<bool> ret = service.GpsSettingAdd(gpssettinginfo);
			//Assert.IsFalse(ret.IsSuccess);
		}


		/// <summary>
		/// 正数小数
		/// </summary>
		[TestMethod]
		public void TestNumberForPortionGps()
		{
			GpsSettingInfo gpssettinginfo = new GpsSettingInfo();
			CommandManageService service = new CommandManageService();
			gpssettinginfo.Gps_RuleID="";
			gpssettinginfo.Gps_RuleName="";
			gpssettinginfo.Gps_Creator="";
			gpssettinginfo.Gps_Description="";
			SingleMessage<bool> ret = service.GpsSettingAdd(gpssettinginfo);
			//Assert.IsFalse(ret.IsSuccess);
		}


		/// <summary>
		/// 字母
		/// </summary>
		[TestMethod]
		public void TestNumberForPortionVVGps()
		{
			GpsSettingInfo gpssettinginfo = new GpsSettingInfo();
			CommandManageService service = new CommandManageService();
			gpssettinginfo.Gps_RuleID="";
			gpssettinginfo.Gps_RuleName="";
			gpssettinginfo.Gps_Creator="";
			gpssettinginfo.Gps_Description="";
			SingleMessage<bool> ret = service.GpsSettingAdd(gpssettinginfo);
			//Assert.IsFalse(ret.IsSuccess);
		}



		/// <summary>
		/// DateTime
		/// </summary>
		[TestMethod]
		public void TestDateTimeGps()
		{
			GpsSettingInfo gpssettinginfo = new GpsSettingInfo();
			CommandManageService service = new CommandManageService();
			gpssettinginfo.Gps_RuleID="";
			gpssettinginfo.Gps_RuleName="";
			gpssettinginfo.Gps_Creator="";
			gpssettinginfo.Gps_Description="";
			SingleMessage<bool> ret = service.GpsSettingAdd(gpssettinginfo);
			//Assert.IsFalse(ret.IsSuccess);
		}


		/// <summary>
		/// 日期为空
		/// </summary>
		[TestMethod]
		public void TestGps_RuleIDDateTimeForNullGps()
		{
			GpsSettingInfo gpssettinginfo = new GpsSettingInfo();
			CommandManageService service = new CommandManageService();
			gpssettinginfo.Gps_RuleID=null;
			gpssettinginfo.Gps_RuleName="";
			gpssettinginfo.Gps_Creator="";
			gpssettinginfo.Gps_Description="";
			SingleMessage<bool> ret = service.GpsSettingAdd(gpssettinginfo);
			//Assert.IsFalse(ret.IsSuccess);
		}


		/// <summary>
		/// 日期为空
		/// </summary>
		[TestMethod]
		public void TestGps_RuleNameDateTimeForNullGps()
		{
			GpsSettingInfo gpssettinginfo = new GpsSettingInfo();
			CommandManageService service = new CommandManageService();
			gpssettinginfo.Gps_RuleID="";
			gpssettinginfo.Gps_RuleName=null;
			gpssettinginfo.Gps_Creator="";
			gpssettinginfo.Gps_Description="";
			SingleMessage<bool> ret = service.GpsSettingAdd(gpssettinginfo);
			//Assert.IsFalse(ret.IsSuccess);
		}


		/// <summary>
		/// 日期为空
		/// </summary>
		[TestMethod]
		public void TestGps_IfMonitorDateTimeForNullGps()
		{
			GpsSettingInfo gpssettinginfo = new GpsSettingInfo();
			CommandManageService service = new CommandManageService();
			gpssettinginfo.Gps_RuleID="";
			gpssettinginfo.Gps_RuleName="";
			gpssettinginfo.Gps_IfMonitor=null;
			gpssettinginfo.Gps_Creator="";
			gpssettinginfo.Gps_Description="";
			SingleMessage<bool> ret = service.GpsSettingAdd(gpssettinginfo);
			//Assert.IsFalse(ret.IsSuccess);
		}


		/// <summary>
		/// 日期为空
		/// </summary>
		[TestMethod]
		public void TestGps_UploadSumDateTimeForNullGps()
		{
			GpsSettingInfo gpssettinginfo = new GpsSettingInfo();
			CommandManageService service = new CommandManageService();
			gpssettinginfo.Gps_RuleID="";
			gpssettinginfo.Gps_RuleName="";
			gpssettinginfo.Gps_UploadSum=null;
			gpssettinginfo.Gps_Creator="";
			gpssettinginfo.Gps_Description="";
			SingleMessage<bool> ret = service.GpsSettingAdd(gpssettinginfo);
			//Assert.IsFalse(ret.IsSuccess);
		}


		/// <summary>
		/// 日期为空
		/// </summary>
		[TestMethod]
		public void TestGps_UploadTypeDateTimeForNullGps()
		{
			GpsSettingInfo gpssettinginfo = new GpsSettingInfo();
			CommandManageService service = new CommandManageService();
			gpssettinginfo.Gps_RuleID="";
			gpssettinginfo.Gps_RuleName="";
			gpssettinginfo.Gps_UploadType=null;
			gpssettinginfo.Gps_Creator="";
			gpssettinginfo.Gps_Description="";
			SingleMessage<bool> ret = service.GpsSettingAdd(gpssettinginfo);
			//Assert.IsFalse(ret.IsSuccess);
		}


		/// <summary>
		/// 日期为空
		/// </summary>
		[TestMethod]
		public void TestGps_CreateTimeDateTimeForNullGps()
		{
			GpsSettingInfo gpssettinginfo = new GpsSettingInfo();
			CommandManageService service = new CommandManageService();
			gpssettinginfo.Gps_RuleID="";
			gpssettinginfo.Gps_RuleName="";
			gpssettinginfo.Gps_CreateTime=null;
			gpssettinginfo.Gps_Creator="";
			gpssettinginfo.Gps_Description="";
			SingleMessage<bool> ret = service.GpsSettingAdd(gpssettinginfo);
			//Assert.IsFalse(ret.IsSuccess);
		}


		/// <summary>
		/// 日期为空
		/// </summary>
		[TestMethod]
		public void TestGps_CreatorDateTimeForNullGps()
		{
			GpsSettingInfo gpssettinginfo = new GpsSettingInfo();
			CommandManageService service = new CommandManageService();
			gpssettinginfo.Gps_RuleID="";
			gpssettinginfo.Gps_RuleName="";
			gpssettinginfo.Gps_Creator=null;
			gpssettinginfo.Gps_Description="";
			SingleMessage<bool> ret = service.GpsSettingAdd(gpssettinginfo);
			//Assert.IsFalse(ret.IsSuccess);
		}


		/// <summary>
		/// 日期为空
		/// </summary>
		[TestMethod]
		public void TestGps_DescriptionDateTimeForNull()
		{
			GpsSettingInfo gpssettinginfo = new GpsSettingInfo();
			CommandManageService service = new CommandManageService();
			gpssettinginfo.Gps_RuleID="";
			gpssettinginfo.Gps_RuleName="";
			gpssettinginfo.Gps_Creator="";
			gpssettinginfo.Gps_Description=null;
			SingleMessage<bool> ret = service.GpsSettingAdd(gpssettinginfo);
			//Assert.IsFalse(ret.IsSuccess);
		}


		/// <summary>
		/// 日期为空
		/// </summary>
		[TestMethod]
		public void TestGps_DistanceDateTimeForNull()
		{
			GpsSettingInfo gpssettinginfo = new GpsSettingInfo();
			CommandManageService service = new CommandManageService();
			gpssettinginfo.Gps_RuleID="";
			gpssettinginfo.Gps_RuleName="";
			gpssettinginfo.Gps_Creator="";
			gpssettinginfo.Gps_Description="";
			gpssettinginfo.Gps_Distance=null;
			SingleMessage<bool> ret = service.GpsSettingAdd(gpssettinginfo);
			//Assert.IsFalse(ret.IsSuccess);
		}


		/// <summary>
		/// 日期为空
		/// </summary>
		[TestMethod]
		public void TestGps_TimeDateTimeForNull()
		{
			GpsSettingInfo gpssettinginfo = new GpsSettingInfo();
			CommandManageService service = new CommandManageService();
			gpssettinginfo.Gps_RuleID="";
			gpssettinginfo.Gps_RuleName="";
			gpssettinginfo.Gps_Creator="";
			gpssettinginfo.Gps_Description="";
			gpssettinginfo.Gps_Time=null;
			SingleMessage<bool> ret = service.GpsSettingAdd(gpssettinginfo);
			//Assert.IsFalse(ret.IsSuccess);
		}


		/// <summary>
		/// 日期为空,don`t test
		/// </summary>
		[TestMethod]
		public void TestGps_VehicleCountDateTimeForNull()
		{
			GpsSettingInfo gpssettinginfo = new GpsSettingInfo();
			CommandManageService service = new CommandManageService();
			gpssettinginfo.Gps_RuleID="";
			gpssettinginfo.Gps_RuleName="";
			gpssettinginfo.Gps_Creator="";
			gpssettinginfo.Gps_Description="";
            //gpssettinginfo.Gps_VehicleCount=null;
            //SingleMessage<bool> ret = service.GpsSettingAdd(gpssettinginfo);
			//Assert.IsFalse(ret.IsSuccess);
		}


		/// <summary>
		/// 日期为空,don`t test
		/// </summary>
		[TestMethod]
		public void TestGps_IsDefaultDateTimeForNull()
		{
			GpsSettingInfo gpssettinginfo = new GpsSettingInfo();
			CommandManageService service = new CommandManageService();
			gpssettinginfo.Gps_RuleID="";
			gpssettinginfo.Gps_RuleName="";
			gpssettinginfo.Gps_Creator="";
			gpssettinginfo.Gps_Description="";
            //gpssettinginfo.Gps_IsDefault=null;
			SingleMessage<bool> ret = service.GpsSettingAdd(gpssettinginfo);
			//Assert.IsFalse(ret.IsSuccess);
		}


		/// <summary>
		/// 日期为空,don`t test
		/// </summary>
		[TestMethod]
		public void TestGps_ValidDateTimeForNull()
		{
			GpsSettingInfo gpssettinginfo = new GpsSettingInfo();
			CommandManageService service = new CommandManageService();
			gpssettinginfo.Gps_RuleID="";
			gpssettinginfo.Gps_RuleName="";
			gpssettinginfo.Gps_Creator="";
			gpssettinginfo.Gps_Description="";
            //gpssettinginfo.Gps_Valid=null;
			SingleMessage<bool> ret = service.GpsSettingAdd(gpssettinginfo);
			//Assert.IsFalse(ret.IsSuccess);
		}


		/// <summary>
		/// Gps_RuleID输入日期,
		/// </summary>
		[TestMethod]
		public void TestGps_RuleIDDateTimeForIncomplete()
		{
			GpsSettingInfo gpssettinginfo = new GpsSettingInfo();
			CommandManageService service = new CommandManageService();
			gpssettinginfo.Gps_RuleID=DateTime.Parse("2015/02/12").ToString();
			gpssettinginfo.Gps_Creator="";
			gpssettinginfo.Gps_Description="";
			SingleMessage<bool> ret = service.GpsSettingAdd(gpssettinginfo);
			Assert.IsFalse(ret.IsSuccess);
		}


		/// <summary>
		/// Gps_IfMonitor日期为空
		/// </summary>
		[TestMethod]
		public void TestGps_IfMonitorDateTimeForIncomplete()
		{
			GpsSettingInfo gpssettinginfo = new GpsSettingInfo();
			CommandManageService service = new CommandManageService();
			gpssettinginfo.Gps_RuleID="";
			gpssettinginfo.Gps_RuleName="";
            gpssettinginfo.Gps_IfMonitor = short.Parse(DateTime.Parse("2015/02/12").ToString());
			gpssettinginfo.Gps_Creator="";
			gpssettinginfo.Gps_Description="";
			SingleMessage<bool> ret = service.GpsSettingAdd(gpssettinginfo);
			//Assert.IsFalse(ret.IsSuccess);
		}


		/// <summary>
		/// Gps_UploadSum日期为空
		/// </summary>
		[TestMethod]
		public void TestGps_UploadSumDateTimeForIncomplete()
		{
			GpsSettingInfo gpssettinginfo = new GpsSettingInfo();
			CommandManageService service = new CommandManageService();
			gpssettinginfo.Gps_RuleID="";
			gpssettinginfo.Gps_RuleName="";
            gpssettinginfo.Gps_UploadSum = int.Parse(DateTime.Parse("2015/02/12").ToString());
			gpssettinginfo.Gps_Creator="";
			gpssettinginfo.Gps_Description="";
			SingleMessage<bool> ret = service.GpsSettingAdd(gpssettinginfo);
			//Assert.IsFalse(ret.IsSuccess);
		}


		/// <summary>
		/// Gps_UploadType日期为空
		/// </summary>
		[TestMethod]
		public void TestGps_UploadTypeDateTimeForIncomplete()
		{
			GpsSettingInfo gpssettinginfo = new GpsSettingInfo();
			CommandManageService service = new CommandManageService();
			gpssettinginfo.Gps_RuleID="";
			gpssettinginfo.Gps_RuleName="";
            gpssettinginfo.Gps_UploadType = short.Parse(DateTime.Parse("2015/02/12").ToString());
			gpssettinginfo.Gps_Creator="";
			gpssettinginfo.Gps_Description="";
			SingleMessage<bool> ret = service.GpsSettingAdd(gpssettinginfo);
			//Assert.IsFalse(ret.IsSuccess);
		}


		/// <summary>
		/// Gps_CreateTime日期为空
		/// </summary>
		[TestMethod]
		public void TestGps_CreateTimeDateTimeForIncomplete()
		{
			GpsSettingInfo gpssettinginfo = new GpsSettingInfo();
			CommandManageService service = new CommandManageService();
			gpssettinginfo.Gps_RuleID="";
			gpssettinginfo.Gps_RuleName="";
			gpssettinginfo.Gps_CreateTime=DateTime.Parse("2015/02/12");
			gpssettinginfo.Gps_Creator="";
			gpssettinginfo.Gps_Description="";
			SingleMessage<bool> ret = service.GpsSettingAdd(gpssettinginfo);
			//Assert.IsFalse(ret.IsSuccess);
		}


		/// <summary>
		/// Gps_Creator日期为空
		/// </summary>
		[TestMethod]
		public void TestGps_CreatorDateTimeForIncomplete()
		{
			GpsSettingInfo gpssettinginfo = new GpsSettingInfo();
			CommandManageService service = new CommandManageService();
			gpssettinginfo.Gps_RuleID="";
			gpssettinginfo.Gps_RuleName="";
            gpssettinginfo.Gps_Creator = DateTime.Parse("2015/02/12").ToString();
			gpssettinginfo.Gps_Description="";
			SingleMessage<bool> ret = service.GpsSettingAdd(gpssettinginfo);
			//Assert.IsFalse(ret.IsSuccess);
		}


		/// <summary>
		/// Gps_Description日期为空
		/// </summary>
		[TestMethod]
		public void TestGps_DescriptionDateTimeForIncomplete()
		{
			GpsSettingInfo gpssettinginfo = new GpsSettingInfo();
			CommandManageService service = new CommandManageService();
			gpssettinginfo.Gps_RuleID="";
			gpssettinginfo.Gps_RuleName="";
			gpssettinginfo.Gps_Creator="";
            gpssettinginfo.Gps_Description = DateTime.Parse("2015/02/12").ToString();
			SingleMessage<bool> ret = service.GpsSettingAdd(gpssettinginfo);
			//Assert.IsFalse(ret.IsSuccess);
		}


		/// <summary>
		/// Gps_Distance日期为空
		/// </summary>
		[TestMethod]
		public void TestGps_DistanceDateTimeForIncomplete()
		{
			GpsSettingInfo gpssettinginfo = new GpsSettingInfo();
			CommandManageService service = new CommandManageService();
			gpssettinginfo.Gps_RuleID="";
			gpssettinginfo.Gps_RuleName="";
			gpssettinginfo.Gps_Creator="";
			gpssettinginfo.Gps_Description="";
            gpssettinginfo.Gps_Distance = int.Parse(DateTime.Parse("2015/02/12").ToString());
			SingleMessage<bool> ret = service.GpsSettingAdd(gpssettinginfo);
			//Assert.IsFalse(ret.IsSuccess);
		}


		/// <summary>
		/// Gps_Time日期为空
		/// </summary>
		[TestMethod]
		public void TestGps_TimeDateTimeForIncomplete()
		{
			GpsSettingInfo gpssettinginfo = new GpsSettingInfo();
			CommandManageService service = new CommandManageService();
			gpssettinginfo.Gps_RuleID="";
			gpssettinginfo.Gps_RuleName="";
			gpssettinginfo.Gps_Creator="";
			gpssettinginfo.Gps_Description="";
            gpssettinginfo.Gps_Time = int.Parse(DateTime.Parse("2015/02/12").ToString());
			SingleMessage<bool> ret = service.GpsSettingAdd(gpssettinginfo);
			//Assert.IsFalse(ret.IsSuccess);
		}


		/// <summary>
		/// Gps_VehicleCount日期为空
		/// </summary>
		[TestMethod]
		public void TestGps_VehicleCountDateTimeForIncomplete()
		{
			GpsSettingInfo gpssettinginfo = new GpsSettingInfo();
			CommandManageService service = new CommandManageService();
			gpssettinginfo.Gps_RuleID="";
			gpssettinginfo.Gps_RuleName="";
			gpssettinginfo.Gps_Creator="";
			gpssettinginfo.Gps_Description="";
            gpssettinginfo.Gps_VehicleCount = int.Parse(DateTime.Parse("2015/02/12").ToString());
			SingleMessage<bool> ret = service.GpsSettingAdd(gpssettinginfo);
			//Assert.IsFalse(ret.IsSuccess);
		}


		/// <summary>
		/// Gps_IsDefault日期为空
		/// </summary>
		[TestMethod]
		public void TestGps_IsDefaultDateTimeForIncomplete()
		{
			GpsSettingInfo gpssettinginfo = new GpsSettingInfo();
			CommandManageService service = new CommandManageService();
			gpssettinginfo.Gps_RuleID="";
			gpssettinginfo.Gps_RuleName="";
			gpssettinginfo.Gps_Creator="";
			gpssettinginfo.Gps_Description="";
            gpssettinginfo.Gps_IsDefault = short.Parse(DateTime.Parse("2015/02/12").ToString());
			SingleMessage<bool> ret = service.GpsSettingAdd(gpssettinginfo);
			//Assert.IsFalse(ret.IsSuccess);
		}


		/// <summary>
		/// Gps_Valid日期为空
		/// </summary>
		[TestMethod]
		public void TestGps_ValidDateTimeForIncomplete()
		{
			GpsSettingInfo gpssettinginfo = new GpsSettingInfo();
			CommandManageService service = new CommandManageService();
			gpssettinginfo.Gps_RuleID="";
			gpssettinginfo.Gps_RuleName="";
			gpssettinginfo.Gps_Creator="";
			gpssettinginfo.Gps_Description="";
            gpssettinginfo.Gps_Valid = short.Parse(DateTime.Parse("2015/02/12").ToString());
			SingleMessage<bool> ret = service.GpsSettingAdd(gpssettinginfo);
			//Assert.IsFalse(ret.IsSuccess);
		}


		/// <summary>
		/// 测试Gps_RuleID日期格式错误
		/// </summary>
		[TestMethod]
		public void TestGps_RuleIDDateTimeForIncompleteGps()
		{
			GpsSettingInfo gpssettinginfo = new GpsSettingInfo();
			CommandManageService service = new CommandManageService();
            gpssettinginfo.Gps_RuleID = DateTime.Parse("2015/02/12").ToString();
			gpssettinginfo.Gps_RuleName="";
			gpssettinginfo.Gps_Creator="";
			gpssettinginfo.Gps_Description="";
			SingleMessage<bool> ret = service.GpsSettingAdd(gpssettinginfo);
			//Assert.IsFalse(ret.IsSuccess);
		}


		/// <summary>
		/// 测试Gps_RuleName日期格式错误
		/// </summary>
		[TestMethod]
		public void TestGps_RuleNameDateTimeForIncomplete()
		{
			GpsSettingInfo gpssettinginfo = new GpsSettingInfo();
			CommandManageService service = new CommandManageService();
			gpssettinginfo.Gps_RuleID="";
            gpssettinginfo.Gps_RuleName = DateTime.Parse("2015/02/12").ToString();
			gpssettinginfo.Gps_Creator="";
			gpssettinginfo.Gps_Description="";
			SingleMessage<bool> ret = service.GpsSettingAdd(gpssettinginfo);
			//Assert.IsFalse(ret.IsSuccess);
		}


		/// <summary>
		/// 测试Gps_IfMonitor日期格式错误
		/// </summary>
		[TestMethod]
		public void TestGps_IfMonitorDateTimeForIncompleteGps()
		{
			GpsSettingInfo gpssettinginfo = new GpsSettingInfo();
			CommandManageService service = new CommandManageService();
			gpssettinginfo.Gps_RuleID="";
			gpssettinginfo.Gps_RuleName="";
            gpssettinginfo.Gps_IfMonitor = short.Parse(DateTime.Parse("2015/02/12").ToString());
			gpssettinginfo.Gps_Creator="";
			gpssettinginfo.Gps_Description="";
			SingleMessage<bool> ret = service.GpsSettingAdd(gpssettinginfo);
			//Assert.IsFalse(ret.IsSuccess);
		}


		/// <summary>
		/// 测试Gps_UploadSum日期格式错误
		/// </summary>
		[TestMethod]
		public void TestGps_UploadSumDateTimeForIncompleteGps()
		{
			GpsSettingInfo gpssettinginfo = new GpsSettingInfo();
			CommandManageService service = new CommandManageService();
			gpssettinginfo.Gps_RuleID="";
			gpssettinginfo.Gps_RuleName="";
            gpssettinginfo.Gps_UploadSum = int.Parse(DateTime.Parse("2015/02/12").ToString());
			gpssettinginfo.Gps_Creator="";
			gpssettinginfo.Gps_Description="";
			SingleMessage<bool> ret = service.GpsSettingAdd(gpssettinginfo);
			//Assert.IsFalse(ret.IsSuccess);
		}


		/// <summary>
		/// 测试Gps_UploadType日期格式错误
		/// </summary>
		[TestMethod]
		public void TestGps_UploadTypeDateTimeForIncompleteGps()
		{
			GpsSettingInfo gpssettinginfo = new GpsSettingInfo();
			CommandManageService service = new CommandManageService();
			gpssettinginfo.Gps_RuleID="";
			gpssettinginfo.Gps_RuleName="";
            gpssettinginfo.Gps_UploadType = short.Parse(DateTime.Parse("2015/02/12").ToString());
			gpssettinginfo.Gps_Creator="";
			gpssettinginfo.Gps_Description="";
			SingleMessage<bool> ret = service.GpsSettingAdd(gpssettinginfo);
			//Assert.IsFalse(ret.IsSuccess);
		}


		/// <summary>
		/// 测试Gps_CreateTime日期格式错误
		/// </summary>
		[TestMethod]
		public void TestGps_CreateTimeDateTimeForIncompleteGps()
		{
			GpsSettingInfo gpssettinginfo = new GpsSettingInfo();
			CommandManageService service = new CommandManageService();
			gpssettinginfo.Gps_RuleID="";
			gpssettinginfo.Gps_RuleName="";
			gpssettinginfo.Gps_CreateTime=DateTime.Parse("2015/33/33");
			gpssettinginfo.Gps_Creator="";
			gpssettinginfo.Gps_Description="";
			SingleMessage<bool> ret = service.GpsSettingAdd(gpssettinginfo);
			//Assert.IsFalse(ret.IsSuccess);
		}


		/// <summary>
		/// 测试Gps_Creator日期格式错误
		/// </summary>
		[TestMethod]
		public void TestGps_CreatorDateTimeForIncompleteGps()
		{
			GpsSettingInfo gpssettinginfo = new GpsSettingInfo();
			CommandManageService service = new CommandManageService();
			gpssettinginfo.Gps_RuleID="";
			gpssettinginfo.Gps_RuleName="";
            gpssettinginfo.Gps_Creator = DateTime.Parse("2015/02/12").ToString();
			gpssettinginfo.Gps_Description="";
			SingleMessage<bool> ret = service.GpsSettingAdd(gpssettinginfo);
			//Assert.IsFalse(ret.IsSuccess);
		}


		/// <summary>
		/// 测试Gps_Description日期格式错误
		/// </summary>
		[TestMethod]
		public void TestGps_DescriptionDateTimeForIncompleteGps()
		{
			GpsSettingInfo gpssettinginfo = new GpsSettingInfo();
			CommandManageService service = new CommandManageService();
			gpssettinginfo.Gps_RuleID="";
			gpssettinginfo.Gps_RuleName="";
			gpssettinginfo.Gps_Creator="";
			gpssettinginfo.Gps_Description=DateTime.Parse("2015/33/33").ToString();
			SingleMessage<bool> ret = service.GpsSettingAdd(gpssettinginfo);
			//Assert.IsFalse(ret.IsSuccess);
		}


		/// <summary>
		/// 测试Gps_Distance日期格式错误
		/// </summary>
		[TestMethod]
		public void TestGps_DistanceDateTimeForIncompleteGps()
		{
			GpsSettingInfo gpssettinginfo = new GpsSettingInfo();
			CommandManageService service = new CommandManageService();
			gpssettinginfo.Gps_RuleID="";
			gpssettinginfo.Gps_RuleName="";
			gpssettinginfo.Gps_Creator="";
			gpssettinginfo.Gps_Description="";
            gpssettinginfo.Gps_Distance = int.Parse(DateTime.Parse("2015/02/12").ToString());
			SingleMessage<bool> ret = service.GpsSettingAdd(gpssettinginfo);
			//Assert.IsFalse(ret.IsSuccess);
		}


		/// <summary>
		/// 测试Gps_Time日期格式错误
		/// </summary>
		[TestMethod]
		public void TestGps_TimeDateTimeForIncompleteGps()
		{
			GpsSettingInfo gpssettinginfo = new GpsSettingInfo();
			CommandManageService service = new CommandManageService();
			gpssettinginfo.Gps_RuleID="";
			gpssettinginfo.Gps_RuleName="";
			gpssettinginfo.Gps_Creator="";
			gpssettinginfo.Gps_Description="";
            gpssettinginfo.Gps_Time = int.Parse(DateTime.Parse("2015/02/12").ToString());
			SingleMessage<bool> ret = service.GpsSettingAdd(gpssettinginfo);
			//Assert.IsFalse(ret.IsSuccess);
		}


		/// <summary>
		/// 测试Gps_VehicleCount日期格式错误
		/// </summary>
		[TestMethod]
		public void TestGps_VehicleCountDateTimeForIncompleteGps()
		{
			GpsSettingInfo gpssettinginfo = new GpsSettingInfo();
			CommandManageService service = new CommandManageService();
			gpssettinginfo.Gps_RuleID="";
			gpssettinginfo.Gps_RuleName="";
			gpssettinginfo.Gps_Creator="";
			gpssettinginfo.Gps_Description="";
            gpssettinginfo.Gps_VehicleCount = int.Parse(DateTime.Parse("2015/02/12").ToString());
			SingleMessage<bool> ret = service.GpsSettingAdd(gpssettinginfo);
			//Assert.IsFalse(ret.IsSuccess);
		}


		/// <summary>
		/// 测试Gps_IsDefault日期格式错误
		/// </summary>
		[TestMethod]
		public void TestGps_IsDefaultDateTimeForIncompleteGps()
		{
			GpsSettingInfo gpssettinginfo = new GpsSettingInfo();
			CommandManageService service = new CommandManageService();
			gpssettinginfo.Gps_RuleID="";
			gpssettinginfo.Gps_RuleName="";
			gpssettinginfo.Gps_Creator="";
			gpssettinginfo.Gps_Description="";
            gpssettinginfo.Gps_IsDefault = short.Parse(DateTime.Parse("2015/02/12").ToString());
			SingleMessage<bool> ret = service.GpsSettingAdd(gpssettinginfo);
			//Assert.IsFalse(ret.IsSuccess);
		}


		/// <summary>
		/// 测试Gps_Valid日期格式错误
		/// </summary>
		[TestMethod]
        public void TestGps_ValidDateTimeForIncompleteGps_Valid()
		{
			GpsSettingInfo gpssettinginfo = new GpsSettingInfo();
			CommandManageService service = new CommandManageService();
			gpssettinginfo.Gps_RuleID="";
			gpssettinginfo.Gps_RuleName="";
			gpssettinginfo.Gps_Creator="";
			gpssettinginfo.Gps_Description="";
            gpssettinginfo.Gps_Valid = short.Parse(DateTime.Parse("2015/02/12").ToString());
			SingleMessage<bool> ret = service.GpsSettingAdd(gpssettinginfo);
			//Assert.IsFalse(ret.IsSuccess);
		}


		/// <summary>
		/// 测试Gps_RuleID日期最大值
		/// </summary>
		[TestMethod]
		public void TestGps_RuleIDDateTimeForMax()
		{
			GpsSettingInfo gpssettinginfo = new GpsSettingInfo();
			CommandManageService service = new CommandManageService();
            gpssettinginfo.Gps_RuleID = DateTime.Parse("2015/02/12").ToString();
			gpssettinginfo.Gps_RuleName="";
			gpssettinginfo.Gps_Creator="";
			gpssettinginfo.Gps_Description="";
			SingleMessage<bool> ret = service.GpsSettingAdd(gpssettinginfo);
			//Assert.IsFalse(ret.IsSuccess);
		}


		/// <summary>
		/// 测试Gps_RuleName日期最大值
		/// </summary>
		[TestMethod]
		public void TestGps_RuleNameDateTimeForMax()
		{
			GpsSettingInfo gpssettinginfo = new GpsSettingInfo();
			CommandManageService service = new CommandManageService();
			gpssettinginfo.Gps_RuleID="";
            gpssettinginfo.Gps_RuleName = DateTime.Parse("2015/02/12").ToString();
			gpssettinginfo.Gps_Creator="";
			gpssettinginfo.Gps_Description="";
			SingleMessage<bool> ret = service.GpsSettingAdd(gpssettinginfo);
			//Assert.IsFalse(ret.IsSuccess);
		}


		/// <summary>
		/// 测试Gps_IfMonitor日期最大值
		/// </summary>
		[TestMethod]
		public void TestGps_IfMonitorDateTimeForMax()
		{
			GpsSettingInfo gpssettinginfo = new GpsSettingInfo();
			CommandManageService service = new CommandManageService();
			gpssettinginfo.Gps_RuleID="";
			gpssettinginfo.Gps_RuleName="";
            gpssettinginfo.Gps_IfMonitor = short.Parse(DateTime.Parse("2015/02/12").ToString());
			gpssettinginfo.Gps_Creator="";
			gpssettinginfo.Gps_Description="";
			SingleMessage<bool> ret = service.GpsSettingAdd(gpssettinginfo);
			//Assert.IsFalse(ret.IsSuccess);
		}


		/// <summary>
		/// 测试Gps_UploadSum日期最大值
		/// </summary>
		[TestMethod]
		public void TestGps_UploadSumDateTimeForMax()
		{
			GpsSettingInfo gpssettinginfo = new GpsSettingInfo();
			CommandManageService service = new CommandManageService();
			gpssettinginfo.Gps_RuleID="";
			gpssettinginfo.Gps_RuleName="";
            gpssettinginfo.Gps_UploadSum = int.Parse(DateTime.Parse("2015/02/12").ToString());
			gpssettinginfo.Gps_Creator="";
			gpssettinginfo.Gps_Description="";
			SingleMessage<bool> ret = service.GpsSettingAdd(gpssettinginfo);
			//Assert.IsFalse(ret.IsSuccess);
		}


		/// <summary>
		/// 测试Gps_UploadType日期最大值
		/// </summary>
		[TestMethod]
		public void TestGps_UploadTypeDateTimeForMax()
		{
			GpsSettingInfo gpssettinginfo = new GpsSettingInfo();
			CommandManageService service = new CommandManageService();
			gpssettinginfo.Gps_RuleID="";
			gpssettinginfo.Gps_RuleName="";
			gpssettinginfo.Gps_UploadType=short.Parse(DateTime.MaxValue.ToString());
			gpssettinginfo.Gps_Creator="";
			gpssettinginfo.Gps_Description="";
			SingleMessage<bool> ret = service.GpsSettingAdd(gpssettinginfo);
			//Assert.IsFalse(ret.IsSuccess);
		}


		/// <summary>
		/// 测试Gps_CreateTime日期最大值
		/// </summary>
		[TestMethod]
		public void TestGps_CreateTimeDateTimeForMax()
		{
			GpsSettingInfo gpssettinginfo = new GpsSettingInfo();
			CommandManageService service = new CommandManageService();
			gpssettinginfo.Gps_RuleID="";
			gpssettinginfo.Gps_RuleName="";
			gpssettinginfo.Gps_CreateTime=DateTime.MaxValue;
			gpssettinginfo.Gps_Creator="";
			gpssettinginfo.Gps_Description="";
			SingleMessage<bool> ret = service.GpsSettingAdd(gpssettinginfo);
			//Assert.IsFalse(ret.IsSuccess);
		}


		/// <summary>
		/// 测试Gps_Creator日期最大值
		/// </summary>
		[TestMethod]
		public void TestGps_CreatorDateTimeForMax()
		{
			GpsSettingInfo gpssettinginfo = new GpsSettingInfo();
			CommandManageService service = new CommandManageService();
			gpssettinginfo.Gps_RuleID="";
			gpssettinginfo.Gps_RuleName="";
			gpssettinginfo.Gps_Creator=DateTime.MaxValue.ToString();
			gpssettinginfo.Gps_Description="";
			SingleMessage<bool> ret = service.GpsSettingAdd(gpssettinginfo);
			//Assert.IsFalse(ret.IsSuccess);
		}


		/// <summary>
		/// 测试Gps_Description日期最大值
		/// </summary>
		[TestMethod]
		public void TestGps_DescriptionDateTimeForMax()
		{
			GpsSettingInfo gpssettinginfo = new GpsSettingInfo();
			CommandManageService service = new CommandManageService();
			gpssettinginfo.Gps_RuleID="";
			gpssettinginfo.Gps_RuleName="";
			gpssettinginfo.Gps_Creator="";
			gpssettinginfo.Gps_Description=DateTime.MaxValue.ToString();
			SingleMessage<bool> ret = service.GpsSettingAdd(gpssettinginfo);
			//Assert.IsFalse(ret.IsSuccess);
		}


		/// <summary>
		/// 测试Gps_Distance日期最大值
		/// </summary>
		[TestMethod]
		public void TestGps_DistanceDateTimeForMax()
		{
			GpsSettingInfo gpssettinginfo = new GpsSettingInfo();
			CommandManageService service = new CommandManageService();
			gpssettinginfo.Gps_RuleID="";
			gpssettinginfo.Gps_RuleName="";
			gpssettinginfo.Gps_Creator="";
			gpssettinginfo.Gps_Description="";
			gpssettinginfo.Gps_Distance=int.Parse(DateTime.MaxValue.ToString());
			SingleMessage<bool> ret = service.GpsSettingAdd(gpssettinginfo);
			//Assert.IsFalse(ret.IsSuccess);
		}


		/// <summary>
		/// 测试Gps_Time日期最大值
		/// </summary>
		[TestMethod]
		public void TestGps_TimeDateTimeForMax()
		{
			GpsSettingInfo gpssettinginfo = new GpsSettingInfo();
			CommandManageService service = new CommandManageService();
			gpssettinginfo.Gps_RuleID="";
			gpssettinginfo.Gps_RuleName="";
			gpssettinginfo.Gps_Creator="";
			gpssettinginfo.Gps_Description="";
			gpssettinginfo.Gps_Time=int.Parse(DateTime.MaxValue.ToString());
			SingleMessage<bool> ret = service.GpsSettingAdd(gpssettinginfo);
			//Assert.IsFalse(ret.IsSuccess);
		}


		/// <summary>
		/// 测试Gps_VehicleCount日期最大值
		/// </summary>
		[TestMethod]
		public void TestGps_VehicleCountDateTimeForMax()
		{
			GpsSettingInfo gpssettinginfo = new GpsSettingInfo();
			CommandManageService service = new CommandManageService();
			gpssettinginfo.Gps_RuleID="";
			gpssettinginfo.Gps_RuleName="";
			gpssettinginfo.Gps_Creator="";
			gpssettinginfo.Gps_Description="";
            gpssettinginfo.Gps_VehicleCount = int.Parse(DateTime.MaxValue.ToString());
			SingleMessage<bool> ret = service.GpsSettingAdd(gpssettinginfo);
			//Assert.IsFalse(ret.IsSuccess);
		}


		/// <summary>
		/// 测试Gps_IsDefault日期最大值
		/// </summary>
		[TestMethod]
		public void TestGps_IsDefaultDateTimeForMax()
		{
			GpsSettingInfo gpssettinginfo = new GpsSettingInfo();
			CommandManageService service = new CommandManageService();
			gpssettinginfo.Gps_RuleID="";
			gpssettinginfo.Gps_RuleName="";
			gpssettinginfo.Gps_Creator="";
			gpssettinginfo.Gps_Description="";
            gpssettinginfo.Gps_IsDefault = short.Parse(DateTime.MaxValue.ToString());
			SingleMessage<bool> ret = service.GpsSettingAdd(gpssettinginfo);
			//Assert.IsFalse(ret.IsSuccess);
		}


		/// <summary>
		/// 测试Gps_Valid日期最大值
		/// </summary>
		[TestMethod]
		public void TestGps_ValidDateTimeForMax()
		{
			GpsSettingInfo gpssettinginfo = new GpsSettingInfo();
			CommandManageService service = new CommandManageService();
			gpssettinginfo.Gps_RuleID="";
			gpssettinginfo.Gps_RuleName="";
			gpssettinginfo.Gps_Creator="";
			gpssettinginfo.Gps_Description="";
            gpssettinginfo.Gps_Valid = short.Parse(DateTime.MaxValue.ToString());
			SingleMessage<bool> ret = service.GpsSettingAdd(gpssettinginfo);
			//Assert.IsFalse(ret.IsSuccess);
		}


		/// <summary>
		/// 测试Gps_RuleID日期格式错误
		/// </summary>
		[TestMethod]
		public void TestGps_RuleIDDateTimeForMin()
		{
			GpsSettingInfo gpssettinginfo = new GpsSettingInfo();
			CommandManageService service = new CommandManageService();
			gpssettinginfo.Gps_RuleID=DateTime.MinValue.ToString();
			gpssettinginfo.Gps_RuleName="";
			gpssettinginfo.Gps_Creator="";
			gpssettinginfo.Gps_Description="";
			SingleMessage<bool> ret = service.GpsSettingAdd(gpssettinginfo);
			//Assert.IsFalse(ret.IsSuccess);
		}


		/// <summary>
		/// 测试Gps_RuleName日期格式错误
		/// </summary>
		[TestMethod]
		public void TestGps_RuleNameDateTimeForMin()
		{
			GpsSettingInfo gpssettinginfo = new GpsSettingInfo();
			CommandManageService service = new CommandManageService();
			gpssettinginfo.Gps_RuleID="";
			gpssettinginfo.Gps_RuleName=DateTime.MinValue.ToString();
			gpssettinginfo.Gps_Creator="";
			gpssettinginfo.Gps_Description="";
			SingleMessage<bool> ret = service.GpsSettingAdd(gpssettinginfo);
			//Assert.IsFalse(ret.IsSuccess);
		}


		/// <summary>
		/// 测试Gps_IfMonitor日期格式错误
		/// </summary>
		[TestMethod]
		public void TestGps_IfMonitorDateTimeForMin()
		{
			GpsSettingInfo gpssettinginfo = new GpsSettingInfo();
			CommandManageService service = new CommandManageService();
			gpssettinginfo.Gps_RuleID="";
			gpssettinginfo.Gps_RuleName="";
			gpssettinginfo.Gps_IfMonitor=short.Parse(DateTime.MinValue.ToString());
			gpssettinginfo.Gps_Creator="";
			gpssettinginfo.Gps_Description="";
			SingleMessage<bool> ret = service.GpsSettingAdd(gpssettinginfo);
			//Assert.IsFalse(ret.IsSuccess);
		}


		/// <summary>
		/// 测试Gps_UploadSum日期格式错误
		/// </summary>
		[TestMethod]
		public void TestGps_UploadSumDateTimeForMin()
		{
			GpsSettingInfo gpssettinginfo = new GpsSettingInfo();
			CommandManageService service = new CommandManageService();
			gpssettinginfo.Gps_RuleID="";
			gpssettinginfo.Gps_RuleName="";
			gpssettinginfo.Gps_UploadSum=int.Parse(DateTime.MinValue.ToString());
			gpssettinginfo.Gps_Creator="";
			gpssettinginfo.Gps_Description="";
			SingleMessage<bool> ret = service.GpsSettingAdd(gpssettinginfo);
			//Assert.IsFalse(ret.IsSuccess);
		}


		/// <summary>
		/// 测试Gps_UploadType日期格式错误
		/// </summary>
		[TestMethod]
		public void TestGps_UploadTypeDateTimeForMin()
		{
			GpsSettingInfo gpssettinginfo = new GpsSettingInfo();
			CommandManageService service = new CommandManageService();
			gpssettinginfo.Gps_RuleID="";
			gpssettinginfo.Gps_RuleName="";
			gpssettinginfo.Gps_UploadType=short.Parse(DateTime.MinValue.ToString());
			gpssettinginfo.Gps_Creator="";
			gpssettinginfo.Gps_Description="";
			SingleMessage<bool> ret = service.GpsSettingAdd(gpssettinginfo);
			//Assert.IsFalse(ret.IsSuccess);
		}


		/// <summary>
		/// 测试Gps_CreateTime日期格式错误
		/// </summary>
		[TestMethod]
		public void TestGps_CreateTimeDateTimeForMin()
		{
			GpsSettingInfo gpssettinginfo = new GpsSettingInfo();
			CommandManageService service = new CommandManageService();
			gpssettinginfo.Gps_RuleID="";
			gpssettinginfo.Gps_RuleName="";
			gpssettinginfo.Gps_CreateTime=DateTime.MinValue;
			gpssettinginfo.Gps_Creator="";
			gpssettinginfo.Gps_Description="";
			SingleMessage<bool> ret = service.GpsSettingAdd(gpssettinginfo);
			//Assert.IsFalse(ret.IsSuccess);
		}


		/// <summary>
		/// 测试Gps_Creator日期格式错误
		/// </summary>
		[TestMethod]
		public void TestGps_CreatorDateTimeForMin()
		{
			GpsSettingInfo gpssettinginfo = new GpsSettingInfo();
			CommandManageService service = new CommandManageService();
			gpssettinginfo.Gps_RuleID="";
			gpssettinginfo.Gps_RuleName="";
			gpssettinginfo.Gps_Creator=DateTime.MinValue.ToString();
			gpssettinginfo.Gps_Description="";
			SingleMessage<bool> ret = service.GpsSettingAdd(gpssettinginfo);
			//Assert.IsFalse(ret.IsSuccess);
		}


		/// <summary>
		/// 测试Gps_Description日期格式错误
		/// </summary>
		[TestMethod]
		public void TestGps_DescriptionDateTimeForMin()
		{
			GpsSettingInfo gpssettinginfo = new GpsSettingInfo();
			CommandManageService service = new CommandManageService();
			gpssettinginfo.Gps_RuleID="";
			gpssettinginfo.Gps_RuleName="";
			gpssettinginfo.Gps_Creator="";
			gpssettinginfo.Gps_Description=DateTime.MinValue.ToString();
			SingleMessage<bool> ret = service.GpsSettingAdd(gpssettinginfo);
			//Assert.IsFalse(ret.IsSuccess);
		}


		/// <summary>
		/// 测试Gps_Distance日期格式错误
		/// </summary>
		[TestMethod]
		public void TestGps_DistanceDateTimeForMin()
		{
			GpsSettingInfo gpssettinginfo = new GpsSettingInfo();
			CommandManageService service = new CommandManageService();
			gpssettinginfo.Gps_RuleID="";
			gpssettinginfo.Gps_RuleName="";
			gpssettinginfo.Gps_Creator="";
			gpssettinginfo.Gps_Description="";
			gpssettinginfo.Gps_Distance=int.Parse(DateTime.MinValue.ToString());
			SingleMessage<bool> ret = service.GpsSettingAdd(gpssettinginfo);
			//Assert.IsFalse(ret.IsSuccess);
		}


		/// <summary>
		/// 测试Gps_Time日期格式错误
		/// </summary>
		[TestMethod]
		public void TestGps_TimeDateTimeForMin()
		{
			GpsSettingInfo gpssettinginfo = new GpsSettingInfo();
			CommandManageService service = new CommandManageService();
			gpssettinginfo.Gps_RuleID="";
			gpssettinginfo.Gps_RuleName="";
			gpssettinginfo.Gps_Creator="";
			gpssettinginfo.Gps_Description="";
			gpssettinginfo.Gps_Time=int.Parse(DateTime.MinValue.ToString());
			SingleMessage<bool> ret = service.GpsSettingAdd(gpssettinginfo);
			//Assert.IsFalse(ret.IsSuccess);
		}


		/// <summary>
		/// 测试Gps_VehicleCount日期格式错误
		/// </summary>
		[TestMethod]
		public void TestGps_VehicleCountDateTimeForMin()
		{
			GpsSettingInfo gpssettinginfo = new GpsSettingInfo();
			CommandManageService service = new CommandManageService();
			gpssettinginfo.Gps_RuleID="";
			gpssettinginfo.Gps_RuleName="";
			gpssettinginfo.Gps_Creator="";
			gpssettinginfo.Gps_Description="";
			gpssettinginfo.Gps_VehicleCount=int.Parse(DateTime.MinValue.ToString());
			SingleMessage<bool> ret = service.GpsSettingAdd(gpssettinginfo);
			//Assert.IsFalse(ret.IsSuccess);
		}


		/// <summary>
		/// 测试Gps_IsDefault日期格式错误
		/// </summary>
		[TestMethod]
		public void TestGps_IsDefaultDateTimeForMin()
		{
			GpsSettingInfo gpssettinginfo = new GpsSettingInfo();
			CommandManageService service = new CommandManageService();
			gpssettinginfo.Gps_RuleID="";
			gpssettinginfo.Gps_RuleName="";
			gpssettinginfo.Gps_Creator="";
			gpssettinginfo.Gps_Description="";
            gpssettinginfo.Gps_IsDefault = short.Parse(DateTime.MinValue.ToString());
			SingleMessage<bool> ret = service.GpsSettingAdd(gpssettinginfo);
			//Assert.IsFalse(ret.IsSuccess);
		}


		/// <summary>
		/// 测试Gps_Valid日期格式错误
		/// </summary>
		[TestMethod]
		public void TestGps_ValidDateTimeForMin()
		{
			GpsSettingInfo gpssettinginfo = new GpsSettingInfo();
			CommandManageService service = new CommandManageService();
			gpssettinginfo.Gps_RuleID="";
			gpssettinginfo.Gps_RuleName="";
			gpssettinginfo.Gps_Creator="";
			gpssettinginfo.Gps_Description="";
            gpssettinginfo.Gps_Valid = short.Parse(DateTime.MinValue.ToString());
			SingleMessage<bool> ret = service.GpsSettingAdd(gpssettinginfo);
			//Assert.IsFalse(ret.IsSuccess);
		}


		/// <summary>
		/// update no exists
		/// </summary>
		[TestMethod]
		public void TestGps_RuleIDUpdateNotExist()
		{
			GpsSettingInfo gpssettinginfo = new GpsSettingInfo();
			CommandManageService service = new CommandManageService();
            gpssettinginfo.Gps_RuleID = DateTime.MinValue.ToString();
			gpssettinginfo.Gps_RuleName="";
			gpssettinginfo.Gps_Creator="";
			gpssettinginfo.Gps_Description="";
			SingleMessage<bool> ret = service.ModifyGpsSettings(gpssettinginfo);
			//Assert.IsFalse(ret.IsSuccess);
		}


		/// <summary>
		/// update no exists
		/// </summary>
		[TestMethod]
		public void TestGps_RuleNameUpdateNotExist()
		{
			GpsSettingInfo gpssettinginfo = new GpsSettingInfo();
			CommandManageService service = new CommandManageService();
			gpssettinginfo.Gps_RuleID="";
            gpssettinginfo.Gps_RuleName = DateTime.MinValue.ToString();
			gpssettinginfo.Gps_Creator="";
			gpssettinginfo.Gps_Description="";
			SingleMessage<bool> ret = service.ModifyGpsSettings(gpssettinginfo);
			//Assert.IsFalse(ret.IsSuccess);
		}


		/// <summary>
		/// update no exists
		/// </summary>
		[TestMethod]
		public void TestGps_IfMonitorUpdateNotExist()
		{
			GpsSettingInfo gpssettinginfo = new GpsSettingInfo();
			CommandManageService service = new CommandManageService();
			gpssettinginfo.Gps_RuleID="";
			gpssettinginfo.Gps_RuleName="";
            gpssettinginfo.Gps_IfMonitor = short.Parse(DateTime.MinValue.ToString());
			gpssettinginfo.Gps_Creator="";
			gpssettinginfo.Gps_Description="";
			SingleMessage<bool> ret = service.ModifyGpsSettings(gpssettinginfo);
			//Assert.IsFalse(ret.IsSuccess);
		}


		/// <summary>
		/// update no exists
		/// </summary>
		[TestMethod]
		public void TestGps_UploadSumUpdateNotExist()
		{
			GpsSettingInfo gpssettinginfo = new GpsSettingInfo();
			CommandManageService service = new CommandManageService();
			gpssettinginfo.Gps_RuleID="";
			gpssettinginfo.Gps_RuleName="";
            gpssettinginfo.Gps_UploadSum = int.Parse(DateTime.MinValue.ToString());
			gpssettinginfo.Gps_Creator="";
			gpssettinginfo.Gps_Description="";
			SingleMessage<bool> ret = service.ModifyGpsSettings(gpssettinginfo);
			//Assert.IsFalse(ret.IsSuccess);
		}


		/// <summary>
		/// update no exists
		/// </summary>
		[TestMethod]
		public void TestGps_UploadTypeUpdateNotExist()
		{
			GpsSettingInfo gpssettinginfo = new GpsSettingInfo();
			CommandManageService service = new CommandManageService();
			gpssettinginfo.Gps_RuleID="";
			gpssettinginfo.Gps_RuleName="";
            gpssettinginfo.Gps_UploadType = short.Parse(DateTime.MinValue.ToString());
			gpssettinginfo.Gps_Creator="";
			gpssettinginfo.Gps_Description="";
			SingleMessage<bool> ret = service.ModifyGpsSettings(gpssettinginfo);
			//Assert.IsFalse(ret.IsSuccess);
		}


		/// <summary>
		/// update no exists
		/// </summary>
		[TestMethod]
		public void TestGps_CreateTimeUpdateNotExist()
		{
			GpsSettingInfo gpssettinginfo = new GpsSettingInfo();
			CommandManageService service = new CommandManageService();
			gpssettinginfo.Gps_RuleID="";
			gpssettinginfo.Gps_RuleName="";
			gpssettinginfo.Gps_CreateTime=DateTime.MinValue;
			gpssettinginfo.Gps_Creator="";
			gpssettinginfo.Gps_Description="";
			SingleMessage<bool> ret = service.ModifyGpsSettings(gpssettinginfo);
			//Assert.IsFalse(ret.IsSuccess);
		}


		/// <summary>
		/// update no exists
		/// </summary>
		[TestMethod]
		public void TestGps_CreatorUpdateNotExist()
		{
			GpsSettingInfo gpssettinginfo = new GpsSettingInfo();
			CommandManageService service = new CommandManageService();
			gpssettinginfo.Gps_RuleID="";
			gpssettinginfo.Gps_RuleName="";
            gpssettinginfo.Gps_Creator = DateTime.MinValue.ToString();
			gpssettinginfo.Gps_Description="";
			SingleMessage<bool> ret = service.ModifyGpsSettings(gpssettinginfo);
			//Assert.IsFalse(ret.IsSuccess);
		}


		/// <summary>
		/// update no exists
		/// </summary>
		[TestMethod]
		public void TestGps_DescriptionUpdateNotExist()
		{
			GpsSettingInfo gpssettinginfo = new GpsSettingInfo();
			CommandManageService service = new CommandManageService();
			gpssettinginfo.Gps_RuleID="";
			gpssettinginfo.Gps_RuleName="";
			gpssettinginfo.Gps_Creator="";
            gpssettinginfo.Gps_Description = DateTime.MinValue.ToString();
			SingleMessage<bool> ret = service.ModifyGpsSettings(gpssettinginfo);
			//Assert.IsFalse(ret.IsSuccess);
		}


		/// <summary>
		/// update no exists
		/// </summary>
		[TestMethod]
		public void TestGps_DistanceUpdateNotExist()
		{
			GpsSettingInfo gpssettinginfo = new GpsSettingInfo();
			CommandManageService service = new CommandManageService();
			gpssettinginfo.Gps_RuleID="";
			gpssettinginfo.Gps_RuleName="";
			gpssettinginfo.Gps_Creator="";
			gpssettinginfo.Gps_Description="";
            gpssettinginfo.Gps_Distance = int.Parse(DateTime.MinValue.ToString());
			SingleMessage<bool> ret = service.ModifyGpsSettings(gpssettinginfo);
			//Assert.IsFalse(ret.IsSuccess);
		}


		/// <summary>
		/// update no exists
		/// </summary>
		[TestMethod]
		public void TestGps_TimeUpdateNotExist()
		{
			GpsSettingInfo gpssettinginfo = new GpsSettingInfo();
			CommandManageService service = new CommandManageService();
			gpssettinginfo.Gps_RuleID="";
			gpssettinginfo.Gps_RuleName="";
			gpssettinginfo.Gps_Creator="";
			gpssettinginfo.Gps_Description="";
            gpssettinginfo.Gps_Time = int.Parse(DateTime.MinValue.ToString());
			SingleMessage<bool> ret = service.ModifyGpsSettings(gpssettinginfo);
			//Assert.IsFalse(ret.IsSuccess);
		}


		/// <summary>
		/// update no exists
		/// </summary>
		[TestMethod]
		public void TestGps_VehicleCountUpdateNotExist()
		{
			GpsSettingInfo gpssettinginfo = new GpsSettingInfo();
			CommandManageService service = new CommandManageService();
			gpssettinginfo.Gps_RuleID="";
			gpssettinginfo.Gps_RuleName="";
			gpssettinginfo.Gps_Creator="";
			gpssettinginfo.Gps_Description="";
            gpssettinginfo.Gps_VehicleCount = int.Parse(DateTime.MinValue.ToString());
			SingleMessage<bool> ret = service.ModifyGpsSettings(gpssettinginfo);
			//Assert.IsFalse(ret.IsSuccess);
		}


		/// <summary>
		/// update no exists
		/// </summary>
		[TestMethod]
		public void TestGps_IsDefaultUpdateNotExist()
		{
			GpsSettingInfo gpssettinginfo = new GpsSettingInfo();
			CommandManageService service = new CommandManageService();
			gpssettinginfo.Gps_RuleID="";
			gpssettinginfo.Gps_RuleName="";
			gpssettinginfo.Gps_Creator="";
			gpssettinginfo.Gps_Description="";
            gpssettinginfo.Gps_IsDefault = short.Parse(DateTime.MinValue.ToString());
			SingleMessage<bool> ret = service.ModifyGpsSettings(gpssettinginfo);
			//Assert.IsFalse(ret.IsSuccess);
		}


		/// <summary>
		/// update no exists
		/// </summary>
		[TestMethod]
		public void TestGps_ValidUpdateNotExist()
		{
			GpsSettingInfo gpssettinginfo = new GpsSettingInfo();
			CommandManageService service = new CommandManageService();
			gpssettinginfo.Gps_RuleID="";
			gpssettinginfo.Gps_RuleName="";
			gpssettinginfo.Gps_Creator="";
			gpssettinginfo.Gps_Description="";
            gpssettinginfo.Gps_Valid = short.Parse(DateTime.MinValue.ToString());
			SingleMessage<bool> ret = service.ModifyGpsSettings(gpssettinginfo);
			//Assert.IsFalse(ret.IsSuccess);
		}


		/// <summary>
		/// update no exists
		/// </summary>
		[TestMethod]
		public void TestGps_RuleIDDeleteNotExist()
		{
			GpsSettingInfo gpssettinginfo = new GpsSettingInfo();
			CommandManageService service = new CommandManageService();
            gpssettinginfo.Gps_RuleID = DateTime.MinValue.ToString();
			gpssettinginfo.Gps_RuleName="";
			gpssettinginfo.Gps_Creator="";
			gpssettinginfo.Gps_Description="";
			SingleMessage<bool> ret = service.DeleteGpsSetting(gpssettinginfo.Gps_RuleName);
			//Assert.IsFalse(ret.IsSuccess);
		}


		/// <summary>
		/// update no exists
		/// </summary>
		[TestMethod]
		public void TestGps_RuleNameDeleteNotExist()
		{
			GpsSettingInfo gpssettinginfo = new GpsSettingInfo();
			CommandManageService service = new CommandManageService();
			gpssettinginfo.Gps_RuleID="";
            gpssettinginfo.Gps_RuleName = DateTime.MinValue.ToString();
			gpssettinginfo.Gps_Creator="";
			gpssettinginfo.Gps_Description="";
			SingleMessage<bool> ret = service.DeleteGpsSetting(gpssettinginfo.Gps_RuleID);
			//Assert.IsFalse(ret.IsSuccess);
		}


		/// <summary>
		/// update no exists
		/// </summary>
		[TestMethod]
		public void TestGps_IfMonitorDeleteNotExist()
		{
			GpsSettingInfo gpssettinginfo = new GpsSettingInfo();
			CommandManageService service = new CommandManageService();
			gpssettinginfo.Gps_RuleID="";
			gpssettinginfo.Gps_RuleName="";
            gpssettinginfo.Gps_IfMonitor = short.Parse(DateTime.MinValue.ToString());
			gpssettinginfo.Gps_Creator="";
			gpssettinginfo.Gps_Description="";
			SingleMessage<bool> ret = service.DeleteGpsSetting(gpssettinginfo.Gps_RuleName);
			//Assert.IsFalse(ret.IsSuccess);
		}


		/// <summary>
		/// update no exists
		/// </summary>
		[TestMethod]
		public void TestGps_UploadSumDeleteNotExist()
		{
			GpsSettingInfo gpssettinginfo = new GpsSettingInfo();
			CommandManageService service = new CommandManageService();
			gpssettinginfo.Gps_RuleID="";
			gpssettinginfo.Gps_RuleName="";
            gpssettinginfo.Gps_UploadSum = int.Parse(DateTime.MinValue.ToString());
			gpssettinginfo.Gps_Creator="";
			gpssettinginfo.Gps_Description="";
			SingleMessage<bool> ret = service.DeleteGpsSetting(gpssettinginfo.Gps_RuleName);
			//Assert.IsFalse(ret.IsSuccess);
		}


		/// <summary>
		/// update no exists
		/// </summary>
		[TestMethod]
		public void TestGps_UploadTypeDeleteNotExist()
		{
			GpsSettingInfo gpssettinginfo = new GpsSettingInfo();
			CommandManageService service = new CommandManageService();
			gpssettinginfo.Gps_RuleID="";
			gpssettinginfo.Gps_RuleName="";
            gpssettinginfo.Gps_UploadType = short.Parse(DateTime.MinValue.ToString());
			gpssettinginfo.Gps_Creator="";
			gpssettinginfo.Gps_Description="";
			SingleMessage<bool> ret = service.DeleteGpsSetting(gpssettinginfo.Gps_RuleName);
			//Assert.IsFalse(ret.IsSuccess);
		}


		/// <summary>
		/// update no exists
		/// </summary>
		[TestMethod]
		public void TestGps_CreateTimeDeleteNotExist()
		{
			GpsSettingInfo gpssettinginfo = new GpsSettingInfo();
			CommandManageService service = new CommandManageService();
			gpssettinginfo.Gps_RuleID="";
			gpssettinginfo.Gps_RuleName="";
			gpssettinginfo.Gps_CreateTime=DateTime.MinValue;
			gpssettinginfo.Gps_Creator="";
			gpssettinginfo.Gps_Description="";
			SingleMessage<bool> ret = service.DeleteGpsSetting(gpssettinginfo.Gps_RuleName);
			//Assert.IsFalse(ret.IsSuccess);
		}


		/// <summary>
		/// update no exists
		/// </summary>
		[TestMethod]
		public void TestGps_CreatorDeleteNotExist()
		{
			GpsSettingInfo gpssettinginfo = new GpsSettingInfo();
			CommandManageService service = new CommandManageService();
			gpssettinginfo.Gps_RuleID="";
			gpssettinginfo.Gps_RuleName="";
			gpssettinginfo.Gps_Creator=DateTime.MinValue.ToString();
			gpssettinginfo.Gps_Description="";
			SingleMessage<bool> ret = service.DeleteGpsSetting(gpssettinginfo.Gps_RuleName);
			//Assert.IsFalse(ret.IsSuccess);
		}


		/// <summary>
		/// update no exists
		/// </summary>
		[TestMethod]
		public void TestGps_DescriptionDeleteNotExist()
		{
			GpsSettingInfo gpssettinginfo = new GpsSettingInfo();
			CommandManageService service = new CommandManageService();
			gpssettinginfo.Gps_RuleID="";
			gpssettinginfo.Gps_RuleName="";
			gpssettinginfo.Gps_Creator="";
			gpssettinginfo.Gps_Description=DateTime.MinValue.ToString();
			SingleMessage<bool> ret = service.DeleteGpsSetting(gpssettinginfo.Gps_RuleName);
			//Assert.IsFalse(ret.IsSuccess);
		}


		/// <summary>
		/// update no exists
		/// </summary>
		[TestMethod]
		public void TestGps_DistanceDeleteNotExist()
		{
			GpsSettingInfo gpssettinginfo = new GpsSettingInfo();
			CommandManageService service = new CommandManageService();
			gpssettinginfo.Gps_RuleID="";
			gpssettinginfo.Gps_RuleName="";
			gpssettinginfo.Gps_Creator="";
			gpssettinginfo.Gps_Description="";
            gpssettinginfo.Gps_Distance = int.Parse(DateTime.MinValue.ToString());
			SingleMessage<bool> ret = service.DeleteGpsSetting(gpssettinginfo.Gps_RuleName);
			//Assert.IsFalse(ret.IsSuccess);
		}


		/// <summary>
		/// update no exists
		/// </summary>
		[TestMethod]
		public void TestGps_TimeDeleteNotExist()
		{
			GpsSettingInfo gpssettinginfo = new GpsSettingInfo();
			CommandManageService service = new CommandManageService();
			gpssettinginfo.Gps_RuleID="";
			gpssettinginfo.Gps_RuleName="";
			gpssettinginfo.Gps_Creator="";
			gpssettinginfo.Gps_Description="";
            gpssettinginfo.Gps_Time = int.Parse(DateTime.MinValue.ToString());
			SingleMessage<bool> ret = service.DeleteGpsSetting(gpssettinginfo.Gps_RuleName);
			//Assert.IsFalse(ret.IsSuccess);
		}


		/// <summary>
		/// update no exists
		/// </summary>
		[TestMethod]
		public void TestGps_VehicleCountDeleteNotExist()
		{
			GpsSettingInfo gpssettinginfo = new GpsSettingInfo();
			CommandManageService service = new CommandManageService();
			gpssettinginfo.Gps_RuleID="";
			gpssettinginfo.Gps_RuleName="";
			gpssettinginfo.Gps_Creator="";
			gpssettinginfo.Gps_Description="";
            gpssettinginfo.Gps_VehicleCount = int.Parse(DateTime.MinValue.ToString());
			SingleMessage<bool> ret = service.DeleteGpsSetting(gpssettinginfo.Gps_RuleName);
			//Assert.IsFalse(ret.IsSuccess);
		}


		/// <summary>
		/// update no exists
		/// </summary>
		[TestMethod]
		public void TestGps_IsDefaultDeleteNotExist()
		{
			GpsSettingInfo gpssettinginfo = new GpsSettingInfo();
			CommandManageService service = new CommandManageService();
			gpssettinginfo.Gps_RuleID="";
			gpssettinginfo.Gps_RuleName="";
			gpssettinginfo.Gps_Creator="";
			gpssettinginfo.Gps_Description="";
            gpssettinginfo.Gps_IsDefault = short.Parse(DateTime.MinValue.ToString());
			SingleMessage<bool> ret = service.DeleteGpsSetting(gpssettinginfo.Gps_RuleName);
			//Assert.IsFalse(ret.IsSuccess);
		}


		/// <summary>
		/// update no exists
		/// </summary>
		[TestMethod]
		public void TestGps_ValidDeleteNotExist()
		{
			GpsSettingInfo gpssettinginfo = new GpsSettingInfo();
			CommandManageService service = new CommandManageService();
			gpssettinginfo.Gps_RuleID="";
			gpssettinginfo.Gps_RuleName="";
			gpssettinginfo.Gps_Creator="";
			gpssettinginfo.Gps_Description="";
            gpssettinginfo.Gps_Valid = short.Parse(DateTime.MinValue.ToString());
			SingleMessage<bool> ret = service.DeleteGpsSetting(gpssettinginfo.Gps_RuleName);
			//Assert.IsFalse(ret.IsSuccess);
		}
        #endregion

        #region TemperatureRuleInfo
        /// <summary>
        /// 测试最大值,have tested
        /// </summary>
        [TestMethod]
        public void TestNumberForMaxTemperature()
        {
            TemperatureRuleInfo temperatureruleinfo = new TemperatureRuleInfo();
            CommandManageService service = new CommandManageService();
            StringBuilder builder = new StringBuilder();
            Random random = new Random();
            //short a = 327;
            short b = 99;
            //temperatureruleinfo.ID = "uihkbg";
            temperatureruleinfo.TemperatureType = b;
            temperatureruleinfo.LowTemperature = b;
            temperatureruleinfo.HighTemperature = b;
            for (int i = 1; i < 17; i++)
            {
                builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
            }
            builder.Append(";");
            temperatureruleinfo.ID = builder.ToString();
            builder.Length = 0;
            SingleMessage<bool> ret = service.AddTemperatureRule(temperatureruleinfo);
            Assert.IsTrue(ret.IsSuccess);
        }


        /// <summary>
        /// 测试最小值,have tested
        /// </summary>
        [TestMethod]
        public void TestNumberForMinTemperature()
        {
            TemperatureRuleInfo temperatureruleinfo = new TemperatureRuleInfo();
            CommandManageService service = new CommandManageService();
            StringBuilder builder = new StringBuilder();
            Random random = new Random();
            short a = -99;
            for (int i = 1; i < 17; i++)
            {
                builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
            }
            builder.Append(";");
            temperatureruleinfo.ID = builder.ToString();
            builder.Length = 0;
            temperatureruleinfo.TemperatureType = a;
            temperatureruleinfo.LowTemperature = a;
            temperatureruleinfo.HighTemperature = a;
            SingleMessage<bool> ret = service.AddTemperatureRule(temperatureruleinfo);
            Assert.IsTrue(ret.IsSuccess);
        }


        /// <summary>
        /// 正数,have tested
        /// </summary>
        [TestMethod]
        public void TestNumberForPositiveTemperature()
        {
            TemperatureRuleInfo temperatureruleinfo = new TemperatureRuleInfo();
            CommandManageService service = new CommandManageService();
            StringBuilder builder = new StringBuilder();
            Random random = new Random();
            for (int i = 1; i < 17; i++)
            {
                builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
            }
            builder.Append(";");
            temperatureruleinfo.ID = builder.ToString();
            builder.Length = 0;
            temperatureruleinfo.TemperatureType = 1;
            temperatureruleinfo.LowTemperature = 1;
            temperatureruleinfo.HighTemperature = 1;
            SingleMessage<bool> ret = service.AddTemperatureRule(temperatureruleinfo);
            Assert.IsTrue(ret.IsSuccess);
        }


        /// <summary>
        /// 负数,have tested
        /// </summary>
        [TestMethod]
        public void TestNumberForNegativeTemperature()
        {
            TemperatureRuleInfo temperatureruleinfo = new TemperatureRuleInfo();
            CommandManageService service = new CommandManageService();
            StringBuilder builder = new StringBuilder();
            Random random = new Random();
            for (int i = 1; i < 17; i++)
            {
                builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
            }
            builder.Append(";");
            temperatureruleinfo.ID = builder.ToString();
            builder.Length = 0;
            temperatureruleinfo.TemperatureType = -1;
            temperatureruleinfo.LowTemperature = -1;
            temperatureruleinfo.HighTemperature = -1;
            SingleMessage<bool> ret = service.AddTemperatureRule(temperatureruleinfo);
            Assert.IsTrue(ret.IsSuccess);
        }


        /// <summary>
        /// 零,have tested
        /// </summary>
        [TestMethod]
        public void TestNumberForZeroTemperature()
        {
            TemperatureRuleInfo temperatureruleinfo = new TemperatureRuleInfo();
            CommandManageService service = new CommandManageService();
            StringBuilder builder = new StringBuilder();
            Random random = new Random();
            for (int i = 1; i < 17; i++)
            {
                builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
            }
            builder.Append(";");
            temperatureruleinfo.ID = builder.ToString();
            builder.Length = 0;
            temperatureruleinfo.SettingType = 0;
            temperatureruleinfo.TemperatureType = 0;
            temperatureruleinfo.LowTemperature = 0;
            temperatureruleinfo.HighTemperature = 0;
            SingleMessage<bool> ret = service.AddTemperatureRule(temperatureruleinfo);
            Assert.IsTrue(ret.IsSuccess);
        }


        /// <summary>
        /// 小数,don`t test
        /// </summary>
        //[TestMethod]
        //public void TestNumberForPortion()
        //{
        //    //TemperatureRuleInfo temperatureruleinfo = new TemperatureRuleInfo();
        //    //CommandManageService service = new CommandManageService();
        //    //temperatureruleinfo.SettingType = 0.1;
        //    //temperatureruleinfo.TemperatureType = 0.1;
        //    //temperatureruleinfo.LowTemperature = 0.1;
        //    //temperatureruleinfo.HighTemperature = 0.1;
        //    //SingleMessage<bool> ret = service.AddTemperatureRule(temperatureruleinfo);
        //    ////Assert.IsFalse(ret.IsSuccess);
        //}


        /// <summary>
        /// 正数小数,don`t test
        /// </summary>
        //[TestMethod]
        //public void TestNumberForPortion()
        //{
        //    TemperatureRuleInfo temperatureruleinfo = new TemperatureRuleInfo();
        //    CommandManageService service = new CommandManageService();
        //    //temperatureruleinfo.SettingType = 0.1;
        //    //temperatureruleinfo.TemperatureType = short.Parse(0.1);
        //    //temperatureruleinfo.LowTemperature = 0.1;
        //    //temperatureruleinfo.HighTemperature = 0.1;
        //    SingleMessage<bool> ret = service.AddTemperatureRule(temperatureruleinfo);
        //    //Assert.IsFalse(ret.IsSuccess);
        //}


        /// <summary>
        /// 字母
        /// </summary>
        //[TestMethod]
        //public void TestNumberForPortionTemperature()
        //{
        //    TemperatureRuleInfo temperatureruleinfo = new TemperatureRuleInfo();
        //    CommandManageService service = new CommandManageService();
        //    temperatureruleinfo.SettingType = 'a';
        //    temperatureruleinfo.TemperatureType = 'a';
        //    temperatureruleinfo.LowTemperature = 'a';
        //    temperatureruleinfo.HighTemperature = 'a';
        //    SingleMessage<bool> ret = service.AddTemperatureRule(temperatureruleinfo);
        //    //Assert.IsFalse(ret.IsSuccess);
        //}
        #endregion
   

}
}

