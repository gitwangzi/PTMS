using System;
using System.Text;
using System.Collections.Generic;
using Gsafety.PTMS.BaseInformation.Service;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Gsafety.PTMS.BaseInformation.Repository;
using Gsafety.PTMS.BaseInformation.Contract.Data;
using System.Threading.Tasks;
using Gsafety.PTMS.Base.Contract.Data;


namespace Gsafety.PTMS.Monitor.Test
{
    /// <summary>
    /// VehicleRepository 的摘要说明
    /// </summary>
    [TestClass]
    public class VehicleServiceTest
    {
        const string chinesecharacter = @"阿啊哀唉挨矮爱碍安岸按案暗昂袄傲奥八巴扒吧疤拔把坝爸罢霸白百柏摆败拜班般斑搬板版办半伴扮拌瓣帮绑榜膀傍棒包胞雹宝饱保堡报抱暴爆杯悲碑北贝备背倍被辈奔本笨蹦逼鼻比彼笔鄙币必毕闭毙弊碧蔽壁避臂边编鞭扁便变遍辨辩辫标表别宾滨冰兵丙柄饼并病拨波玻剥脖菠播伯驳泊博搏膊薄卜补捕不布步怖部擦猜才材财裁采彩睬踩菜参餐残蚕惭惨灿仓苍舱藏操槽草册侧厕测策层叉插查茶察岔差拆柴馋缠产铲颤昌长肠尝偿常厂场敞畅倡唱抄钞超朝潮吵炒车扯彻撤尘臣沉辰陈晨闯衬称趁撑成呈承诚城乘惩程秤吃驰迟持池匙尺齿耻斥赤翅充冲虫崇抽仇绸愁稠筹酬丑臭出初除厨锄础储楚处触畜川穿传船喘串疮窗床创吹炊垂锤春纯唇蠢词慈辞磁此次刺从匆葱聪丛凑粗促醋窜催摧脆翠村存寸错曾搭达答打大呆代带待怠贷袋逮戴丹单担耽胆旦但诞弹淡蛋当挡党荡档刀叨导岛倒蹈到悼盗道稻得德的灯登等凳低堤滴敌笛底抵地弟帝递第颠典点电店垫殿叼雕吊钓调掉爹跌叠蝶丁叮盯钉顶订定丢东冬董懂动冻栋洞都斗抖陡豆逗督毒读独堵赌杜肚度渡端短段断缎锻堆队对吨蹲盾顿多夺朵躲惰鹅蛾额恶饿恩儿而耳二发乏伐罚阀法帆番翻凡烦繁反返犯泛饭范贩方坊芳防妨房仿访纺放飞非肥匪废沸肺费分吩纷芬坟粉份奋愤粪丰风封疯峰锋蜂逢缝讽凤奉佛否夫肤伏扶服俘浮符幅福抚府斧俯辅腐父付妇负附咐复赴副傅富腹覆该改盖溉概干甘杆肝竿秆赶敢感冈刚岗纲缸钢港杠高膏糕搞稿告哥胳鸽割搁歌阁革格葛隔个各给根跟更耕工弓公功攻供宫恭躬巩共贡勾沟钩狗构购够估姑孤辜古谷股骨鼓固故顾瓜刮挂乖拐怪关观官冠馆管贯惯灌罐光广归龟规轨鬼柜贵桂跪滚棍锅国果裹过哈孩海害含寒喊汉汗旱航毫豪好号浩耗喝禾合何和河核荷盒贺黑痕很狠恨恒横衡轰哄烘红宏洪虹喉猴吼后厚候乎呼忽狐胡壶湖糊蝴虎互户护花华哗滑猾化划画话怀槐坏欢还环缓幻唤换患荒慌皇黄煌晃谎灰恢挥辉回悔汇会绘贿惠毁慧昏婚浑魂混活火伙或货获祸惑击饥圾机肌鸡迹积基绩激及吉级即极急疾集籍几己挤脊计记纪忌技际剂季既济继寄加夹佳家嘉甲价驾架假嫁稼奸尖坚歼间肩艰兼监煎拣俭茧捡减剪检简见件建剑荐贱健舰渐践鉴键箭江姜将浆僵疆讲奖桨匠降酱交郊娇浇骄胶椒焦蕉角狡绞饺脚搅缴叫轿较教阶皆接揭街节劫杰洁结捷截竭姐解介戒届界借巾今斤金津筋仅紧谨锦尽劲近进晋浸禁京经茎惊晶睛精井颈景警净径竞竟敬境静镜纠究揪九久酒旧救就舅居拘鞠局菊橘举矩句巨拒具俱剧惧据距锯聚捐卷倦绢决绝觉掘嚼军君均菌俊卡开凯慨刊堪砍做看康糠扛抗炕考烤靠科棵颗壳咳可渴克刻客课肯垦恳坑空孔恐控口扣寇枯哭苦库裤酷夸垮挎跨块快宽款筐狂况旷矿框亏葵愧昆捆困扩括阔垃拉啦喇腊蜡辣来赖兰拦栏蓝篮览懒烂滥郎狼廊朗浪捞劳牢老姥涝乐勒雷垒泪类累冷厘梨狸离犁鹂璃黎礼李里理力历厉立丽利励例隶栗粒俩连帘怜莲联廉镰脸练炼恋链良凉梁粮粱两亮谅辆量辽疗僚了料列劣烈猎裂邻林临淋伶灵岭铃陵零龄领令另溜刘流留榴柳六龙笼聋隆垄拢楼搂漏露芦炉虏鲁陆录鹿滤碌路驴旅屡律虑率绿卵乱掠略轮论罗萝锣箩骡螺络骆落妈麻马码蚂骂吗埋买迈麦卖脉蛮馒瞒满慢漫忙芒盲茫猫毛矛茅茂冒贸帽貌么没眉梅煤霉每美妹门闷们萌盟猛蒙孟梦迷谜米眯秘密蜜眠绵棉免勉面苗描秒妙庙灭蔑民敏名明鸣命摸模膜摩磨魔抹末沫莫漠墨默谋某母亩木目牧墓幕慕暮拿哪内那纳乃奶耐男南难囊挠恼脑闹呢嫩能尼泥你逆年念娘酿鸟尿捏您宁凝牛扭纽农浓弄奴努怒女暖挪欧偶辟趴爬怕拍牌派攀盘判叛盼乓旁胖抛炮袍跑泡陪培赔佩配喷盆朋棚蓬膨捧碰批披劈皮疲脾匹僻片偏篇骗漂飘票撇拼贫品乒平评凭苹瓶萍坡泼婆迫破魄剖仆扑铺葡朴普谱七妻戚期欺漆齐其奇骑棋旗乞企岂启起气弃汽砌器恰洽千迁牵铅谦签前钱钳潜浅遣欠歉枪腔强墙抢悄敲锹乔侨桥瞧巧切茄且窃亲侵芹琴禽勤青轻倾清蜻情晴顷请庆穷丘秋求球区曲驱屈趋渠取去趣圈全权泉拳犬劝券缺却雀确鹊裙群然燃染嚷壤让饶扰绕惹热人仁忍刃认任扔仍日绒荣容熔融柔揉肉如乳辱入软锐瑞润若弱撒洒塞赛三伞散桑嗓丧扫嫂色森杀沙纱傻筛晒山删衫闪陕扇善伤商裳晌赏上尚捎梢烧稍勺少绍哨舌蛇舍设社射涉摄申伸身深神沈审婶肾甚渗慎升生声牲胜绳省圣盛剩尸失师诗施狮湿十什石时识实拾蚀食史使始驶士氏世市示式事侍势视试饰室是柿适逝释誓收手守首寿受兽售授瘦书叔殊梳疏舒输蔬熟暑鼠属薯术束述树竖数刷耍衰摔甩帅拴双霜爽谁水税睡顺说嗽丝司私思斯撕死四寺似饲肆松宋诵送颂搜艘苏俗诉肃素速宿塑酸蒜算虽随岁碎穗孙损笋缩所索锁她他它塌塔踏台抬太态泰贪摊滩坛谈痰坦毯叹炭探汤唐堂塘膛糖倘躺烫趟涛掏滔逃桃陶淘萄讨套特疼腾梯踢提题蹄体剃惕替天添田甜填挑条跳贴铁帖厅听亭庭停挺艇通同桐铜童统桶筒痛偷头投透秃突图徒涂途屠土吐兔团推腿退吞屯托拖脱驼妥娃挖蛙瓦袜歪外弯湾丸完玩顽挽晚碗万汪亡王网往妄忘旺望危威微为围违唯维伟伪尾委卫未位味畏胃喂慰温文纹闻蚊稳问翁窝我沃卧握乌污呜屋无吴五午伍武侮舞勿务物误悟雾夕西吸希析息牺悉惜稀溪锡熄膝习席袭洗喜戏系细隙虾瞎峡狭霞下吓夏厦仙先纤掀鲜闲弦贤咸衔嫌显险县现线限宪陷馅羡献乡相香箱详祥享响想向巷项象像橡削宵消销小晓孝效校笑些歇协邪胁斜携鞋写泄泻卸屑械谢心辛欣新薪信兴星腥刑行形型醒杏姓幸性凶兄胸雄熊休修羞朽秀绣袖锈须虚需徐许序叙绪续絮蓄宣悬旋选穴学雪血寻巡旬询循训讯迅压呀押鸦鸭牙芽崖哑雅亚咽烟淹延严言岩沿炎研盐蜒颜掩眼演厌宴艳验焰雁燕央殃秧扬羊阳杨洋仰养氧痒样妖腰邀窑谣摇遥咬药要耀爷也冶野业叶页夜液一衣医依仪宜姨移遗疑乙已以蚁倚椅义亿忆艺议亦异役译易疫益谊意毅翼因阴姻音银引饮隐印应英樱鹰迎盈营蝇赢影映硬佣拥庸永咏泳勇涌用优忧悠尤由犹邮油游友有又右幼诱于予余鱼娱渔愉愚榆与宇屿羽雨语玉育郁狱浴预域欲御裕遇愈誉冤元员园原圆援缘源远怨院愿约月钥悦阅跃越云匀允孕运晕韵杂灾栽宰载再在咱暂赞脏葬遭糟早枣澡灶皂造燥躁则择泽责贼怎增赠渣扎轧闸眨炸榨摘宅窄债寨沾粘斩展盏崭占战站张章涨掌丈仗帐胀障招找召兆赵照罩遮折哲者这浙贞针侦珍真诊枕阵振镇震争征挣睁筝蒸整正证郑政症之支汁芝枝知织肢脂蜘执侄直值职植殖止只旨址纸指至志制帜治质秩致智置中忠终钟肿种众重州舟周洲粥宙昼皱骤朱株珠诸猪蛛竹烛逐主煮嘱住助注驻柱祝著筑铸抓爪专砖转赚庄装壮状撞追准捉桌浊啄着仔姿资滋子紫字自宗棕踪总纵走奏租足族阻组祖钻嘴最罪醉尊遵昨左作坐座做";

        const string spanishcharacter = "qwértyúíopasdghjklñ´zxcvbnm?¿¡!1234567890 \"";

        const string englishcharacter = "abcdefghigklmnopqrstuvwxyz1234567890 ;*";

        public VehicleServiceTest()
        {

        }

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

        /// <summary>
        /// 所有的字段为空
        /// </summary>
        [TestMethod]
        public void TestStringForAllEmpty()
        {
            Vehicle vehicle = new Vehicle();
            vehicle.VehicleId = null;
            vehicle.VehicleSn = null;
            vehicle.BrandModel = null;
            vehicle.CityCode = null;
            vehicle.StartYear = null;
            vehicle.Note = null;
            vehicle.Region = null;
            vehicle.OperatingLicense = null;
            vehicle.EngineId = null;
            vehicle.Owner = null;
            vehicle.OwnerId = null;
            vehicle.OwnerPhone = null;
            vehicle.OwnerEmail = null;
            vehicle.OwnerAddress = null;

            VehicleService service = new VehicleService();
            SingleMessage<bool> ret = service.AddVehicle(vehicle);
            Assert.IsFalse(ret.IsSuccess);
        }

        /// <summary>
        /// 字符可空的为空
        /// </summary>
        [TestMethod]
        public void TestStringForAllowEmpty()
        {
            Vehicle vehicle = new Vehicle();
            StringBuilder builder = new StringBuilder();
            Random random = new Random();
            for (int i = 0; i < 20; i++)
            {
                builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
            }

            vehicle.VehicleId = builder.ToString();
            vehicle.VehicleSn = null;
            vehicle.BrandModel = null;
            vehicle.CityCode = null;
            vehicle.StartYear = null;
            vehicle.Note = null;
            vehicle.Region = null;
            vehicle.OperatingLicense = null;
            vehicle.EngineId = null;
            vehicle.Owner = null;
            vehicle.OwnerId = null;
            vehicle.OwnerPhone = null;
            vehicle.OwnerEmail = null;
            vehicle.OwnerAddress = null;
            VehicleService service = new VehicleService();
            SingleMessage<bool> ret = service.AddVehicle(vehicle);
            Assert.IsFalse(ret.IsSuccess);
        }

        /// <summary>
        /// 字符最大长度(中文)
        /// </summary>
        [TestMethod]
        public void TestStringForMaxLength_Chinese()
        {
            Vehicle vehicle = new Vehicle();
            //vehicle 20
            StringBuilder builder = new StringBuilder();
            Random random = new Random();
            for (int i = 0; i < 20; i++)
            {
                builder.Append(chinesecharacter[random.Next(englishcharacter.Length - 1)]);
            }
            vehicle.VehicleId = builder.ToString();
            //vehiclesn 50
            vehicle.VehicleSn = "阿嫂登记佛阿嫂看到法律瓦萨基地理范围可聘任阿净嫂计";
            //BrandModel 200
            vehicle.BrandModel = "";
            string temp = string.Empty;
            vehicle.Note = string.Empty;
            for (int i = 0; i < 50; i++)
            {
                string a = "啊的";
                temp = temp + a;
                vehicle.Note = temp;
            }
            vehicle.CityCode = null;
            //startyear 10
            vehicle.StartYear = "三单放机三";
            //note 2000
            vehicle.Note = null;
            for (int i = 0; i < 1000; i++)
            {
                string a = "啊";
                temp = temp + a;
                vehicle.Note = temp;
            }
            //region 512
            vehicle.Region = "";
            for (int i = 0; i < 256; i++)
            {
                string a = "啊";
                temp = temp + a;
                vehicle.Region = temp;
            }
            //operating lisences 100
            vehicle.OperatingLicense = "阿嫂登记佛阿嫂看到法律瓦萨基地理范围可聘任阿净嫂计阿嫂登记佛阿嫂看到法律瓦萨基地理范围可聘任阿净嫂计";
            //Engine ID 50
            vehicle.EngineId = "阿嫂登记佛阿嫂看到法律瓦萨基地理范围可聘任阿净嫂计";
            //vehicle.ServerType = null;
            //owner 50
            vehicle.Owner = "阿嫂登记佛阿嫂看到法律瓦萨基地理范围可聘任阿净嫂计";
            //ownerid 50
            vehicle.OwnerId = "阿嫂登记佛阿嫂看到法律瓦萨基地理范围可聘任阿净嫂计";
            //ownerphone 50
            vehicle.OwnerPhone = "阿嫂登记佛阿嫂看到法律瓦萨基地理范围可聘任阿净嫂计";

            vehicle.OwnerEmail = null;
            //owneraddress 512
            vehicle.OwnerAddress = null;
            for (int i = 0; i < 256; i++)
            {
                string a = "啊";
                temp = temp + a;
                vehicle.OwnerAddress = temp;
            }
            VehicleService service = new VehicleService();
            SingleMessage<bool> ret = service.AddVehicle(vehicle);
            Assert.IsFalse(ret.IsSuccess);

        }

        /// <summary>
        /// 字符最大长度(西文)
        /// </summary>
        [TestMethod]
        public void TestStringForMaxLength_Spanish()
        {
            Vehicle vehicle = new Vehicle();
            //vehicleid 20
            StringBuilder builder = new StringBuilder();
            Random random = new Random();
            for (int i = 0; i < 20; i++)
            {
                builder.Append(spanishcharacter[random.Next(englishcharacter.Length - 1)]);
            }
            vehicle.VehicleId = builder.ToString();
            //vehiclesn 50
            vehicle.VehicleSn = "ApertúradePuñrtaAnónApertúradePuñrtaAnónApertúrade";
            //BrandModel 200
            vehicle.BrandModel = "";
            string temp = null;
            for (int i = 0; i < 40; i++)
            {
                string a = "túóuñ";
                temp = temp + a;
                vehicle.Region = temp;
            }
            vehicle.CityCode = null;
            //startyear 10
            vehicle.StartYear = "rtúdñrnónj";
            //note 2000
            vehicle.Note = null;
            for (int i = 0; i < 100; i++)
            {
                string a = "ApertúradePuñrtaAnón";
                temp = temp + a;
                vehicle.Note = temp;
            }
            //region 512
            vehicle.Region = "";
            for (int i = 0; i < 64; i++)
            {
                string a = "tfúdóuñg";
                temp = temp + a;
                vehicle.Region = temp;
            }
            //operating lisences 100
            vehicle.OperatingLicense = "ApertúradePuñrtaAnónApertúradePuñrtaAnónApertúradeApertúradePuñrtaAnónApertúradePuñrtaAnónApertúrade";
            //Engine ID 50
            vehicle.EngineId = "ApertúradePuñrtaAnónApertúradePuñrtaAnónApertúrade";
            //vehicle.ServerType = null;
            //owner 50
            vehicle.Owner = "ApertúradePuñrtaAnónApertúradePuñrtaAnónApertúrade";
            //ownerid 50
            vehicle.OwnerId = "ApertúradePuñrtaAnónApertúradePuñrtaAnónApertúrade";
            //ownerphone 50
            vehicle.OwnerPhone = "ApertúradePuñrtaAnónApertúradePuñrtaAnónApertúrade";

            vehicle.OwnerEmail = null;
            //owneraddress 512
            vehicle.OwnerAddress = null;
            for (int i = 0; i < 64; i++)
            {
                string a = "tfúdóuñg";
                temp = temp + a;
                vehicle.OwnerAddress = temp;
            }
            VehicleService service = new VehicleService();
            SingleMessage<bool> ret = service.AddVehicle(vehicle);
            Assert.IsFalse(ret.IsSuccess);
        }

        /// <summary>
        /// 字符最大长度（英文）
        /// </summary>
        [TestMethod]
        public void TestStringForMaxLength_English()
        {
            Vehicle vehicle = new Vehicle();
            //vehicleid 20
            StringBuilder builder = new StringBuilder();
            Random random = new Random();
            for (int i = 0; i < 20; i++)
            {
                builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
            }

            vehicle.VehicleId = builder.ToString();
            //vehiclesn 50
            vehicle.VehicleSn = "VehicleCheckingSuiteVehicleCheckingSuiteVehicle";
            //BrandModel 100
            vehicle.BrandModel = "";
            //region  512
            string temp = null;
            for (int i = 0; i < 40; i++)
            {
                string a = "Chec";
                temp = temp + a;
                vehicle.Region = temp;
            }
            vehicle.CityCode = null;
            //startyear 10
            vehicle.StartYear = "Vehicle";
            //note 2000
            vehicle.Note = null;
            for (int i = 0; i < 100; i++)
            {
                string a = "VehicleCheVehicleCh";
                temp = temp + a;
                vehicle.Note = temp;
            }
            //region 512
            vehicle.Region = "";
            for (int i = 0; i < 64; i++)
            {
                string a = "Vehicle";
                temp = temp + a;
                vehicle.Region = temp;
            }
            //operating lisences 100
            vehicle.OperatingLicense = "VehicleCheckingSuiteVehicleCheckinSuiteVehicleCheVehicleCheckingSuiteVehicleCheckingSuiteVehicleChe";
            //Engine ID 50
            vehicle.EngineId = "VehicleCheckingSuiteVehicleCheckingSuiteVehi";
            //vehicle.ServerType = null;
            //owner 50
            vehicle.Owner = "VehicleCheckingSuiteVehicleCheckingSuiteVehi";
            //ownerid 50
            vehicle.OwnerId = "VehicleCheckingSuiteVehicleCheckingSuiteVehi";
            //ownerphone 50
            vehicle.OwnerPhone = "VehicleCheckingSuiteVehicleCheckingSuiteVehi";

            vehicle.OwnerEmail = null;
            //owneraddress 512
            vehicle.OwnerAddress = null;
            for (int i = 0; i < 64; i++)
            {
                string a = "Vehicle";
                temp = temp + a;
                vehicle.OwnerAddress = temp;
            }
            vehicle.Type = VehicleType.Bus;
            vehicle.Status = VehicleConditionType.Available;
            vehicle.ServerType = VehicleSeviceType.Private;
            VehicleService service = new VehicleService();
            SingleMessage<bool> ret = service.AddVehicle(vehicle);
            Assert.IsFalse(ret.IsSuccess);
        }

        /// <summary>
        /// 超过字符最大长度
        /// </summary>
        [TestMethod]
        public void TestStringForOverMaxLength_Chinese()
        {
            Vehicle vehicle = new Vehicle();
            //vehicleid 20
            StringBuilder builder = new StringBuilder();
            Random random = new Random();
            for (int i = 0; i < 40; i++)
            {
                builder.Append(chinesecharacter[random.Next(englishcharacter.Length - 1)]);
            }
            vehicle.VehicleId = builder.ToString();
            //vehiclesn 50
            vehicle.VehicleSn = "浇洒地噢发挖掘三卡阿阿嫂打法浇洒地噢发挖掘三卡阿阿嫂打法浇洒地噢发挖掘三卡阿阿嫂打法";
            //BrandModel 200
            vehicle.BrandModel = "";
            string temp = null;
            for (int i = 0; i < 50; i++)
            {
                string a = "啊的大";
                temp = temp + a;
                vehicle.Note = temp;
            }
            vehicle.CityCode = null;
            //startyear 10
            vehicle.StartYear = "三单放机三";
            //note 2000
            vehicle.Note = "阿嫂基地佛阿嫂放倒阿嫂打法未遂犯坡坎普阿嫂打法考虑买劳动局开发送风机思考阿嫂打法萨发拉夫桑可么大连三木砸碎打法走私大麻法律阿看破速度飞快速度考拉夫马拉松分";
            //region 512
            vehicle.Region = "";
            for (int i = 0; i < 64; i++)
            {
                string a = "豆腐干丝放的速度飞送豆腐";
                temp = temp + a;
                vehicle.Region = temp;
            }
            //operating lisences 100
            vehicle.OperatingLicense = "阿嫂登记佛阿嫂看到法律瓦萨基地理范围可聘任阿净嫂计阿嫂登记佛阿嫂看到法律瓦萨基地理范围可聘任阿净嫂计地方官";
            //Engine ID 50
            vehicle.EngineId = "浇洒地噢发挖掘三卡阿阿嫂打法浇洒地噢发挖掘三卡阿阿嫂打法浇洒地噢发挖掘三卡阿阿嫂打法";
            //vehicle.ServerType = null;
            //owner 50
            vehicle.Owner = "浇洒地噢发挖掘三卡阿阿嫂打法浇洒地噢发挖掘三卡阿阿嫂打法浇洒地噢发挖掘三卡阿阿嫂打法";
            //ownerid 50
            vehicle.OwnerId = "浇洒地噢发挖掘三卡阿阿嫂打法浇洒地噢发挖掘三卡阿阿嫂打法浇洒地噢发挖掘三卡阿阿嫂打法";
            //ownerphone 50
            vehicle.OwnerPhone = "浇洒地噢发挖掘三卡阿阿嫂打法浇洒地噢发挖掘三卡阿阿嫂打法浇洒地噢发挖掘三卡阿阿嫂打法";

            vehicle.OwnerEmail = null;
            //owneraddress 512
            vehicle.OwnerAddress = "";
            for (int i = 0; i < 64; i++)
            {
                string a = "豆腐干丝放的速度飞送豆腐";
                temp = temp + a;
                vehicle.OwnerAddress = temp;
            }
            VehicleService service = new VehicleService();
            SingleMessage<bool> ret = service.AddVehicle(vehicle);
            Assert.IsFalse(ret.IsSuccess);
        }


        /// <summary>
        /// 超过字符最大长度(西语)
        /// </summary>
        [TestMethod]
        public void TestStringForOverMaxLength_Spanish()
        {
            Vehicle vehicle = new Vehicle();
            //vehicleid 20
            StringBuilder builder = new StringBuilder();
            Random random = new Random();
            for (int i = 0; i < 20; i++)
            {
                builder.Append(spanishcharacter[random.Next(englishcharacter.Length - 1)]);
            }
            vehicle.VehicleId = builder.ToString();
            //vehiclesn 50
            vehicle.VehicleSn = "ApertúradePuñrtaAnónApertúradePuñrtaAnónApertúradeaúsdAnónApe";
            //BrandModel 200
            vehicle.BrandModel = "";
            string temp = null;
            for (int i = 0; i < 40; i++)
            {
                string a = "túpeóuñ";
                temp = temp + a;
                vehicle.Region = temp;
            }
            vehicle.CityCode = null;
            //startyear 10
            vehicle.StartYear = "rtúdpeñrnónj";
            //note 2000
            vehicle.Note = null;
            for (int i = 0; i < 100; i++)
            {
                string a = "ApertúradepePuñrtaAnón";
                temp = temp + a;
                vehicle.Note = temp;
            }
            //region 512
            vehicle.Region = "";
            for (int i = 0; i < 64; i++)
            {
                string a = "tfúdóuuñrñg";
                temp = temp + a;
                vehicle.Region = temp;
            }
            //operating lisences 100
            vehicle.OperatingLicense = "ApertúruñruñruñradePuñrtaAnónApertúradePuñrtaAnónApertúradeApertúradePuñrtaAnónApertúradePuñrtaAnónApertúrade";
            //Engine ID 50
            vehicle.EngineId = "ApertúradePuñrtuñraAnónApertúradePuñrtaAnónApertúrade";
            //vehicle.ServerType = null;
            //owner 50
            vehicle.Owner = "ApertúradePuñrtaAnónuñrApertúradePuñrtaAnónApertúrade";
            //ownerid 50
            vehicle.OwnerId = "ApertúradePuñrtaAnuñrónApertúradePuñrtaAnónApertúrade";
            //ownerphone 50
            vehicle.OwnerPhone = "ApertúradePuñruñrtaAnónApertúradePuñrtaAnónApertúrade";

            vehicle.OwnerEmail = null;
            //owneraddress 512
            vehicle.OwnerAddress = null;
            for (int i = 0; i < 64; i++)
            {
                string a = "tfúdóuuñrñg";
                temp = temp + a;
                vehicle.OwnerAddress = temp;
            }
            VehicleService service = new VehicleService();
            SingleMessage<bool> ret = service.AddVehicle(vehicle);
            Assert.IsFalse(ret.IsSuccess);
        }

        /// <summary>
        /// 超过字符最大长度
        /// </summary>
        [TestMethod]
        public void TestStringForOverMaxLength_English()
        {
            Vehicle vehicle = new Vehicle();
            //vehicleid 20
            StringBuilder builder = new StringBuilder();
            Random random = new Random();
            for (int i = 0; i < 20; i++)
            {
                builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
            }
            vehicle.VehicleId = builder.ToString();
            //vehiclesn 50
            vehicle.VehicleSn = "VehicleCheckingSuiteVehicleCheckingSuiteVehicleCheasertfasdfa";
            //BrandModel 200
            vehicle.BrandModel = "";
            string temp = null;
            for (int i = 0; i < 40; i++)
            {
                string a = "Checking";
                temp = temp + a;
                vehicle.Region = temp;
            }
            //vehicle.Type = null;
            //vehicle.Status = null;
            vehicle.CityCode = null;
            //startyear 10
            vehicle.StartYear = "VehicleCheching";
            //note 2000
            vehicle.Note = null;
            for (int i = 0; i < 100; i++)
            {
                string a = "VehicleCheVehicleCheasdf";
                temp = temp + a;
                vehicle.Note = temp;
            }
            //region 512
            vehicle.Region = "";
            for (int i = 0; i < 64; i++)
            {
                string a = "VehicleCasdf";
                temp = temp + a;
                vehicle.Region = temp;
            }
            //operating lisences 100
            vehicle.OperatingLicense = "VehicleCheckingSuiteVehicleCheckingSuiteVehicleCheVehicleCheckingSuiteVehicleCheckingSuiteVehicleCheasdfaw";
            //Engine ID 50
            vehicle.EngineId = "VehicleCheckingSuiteVehicleCheckingSuiteVehicleCheasdfas";
            //vehicle.ServerType = null;
            //owner 50
            vehicle.Owner = "VehicleCheckingSuiteVehicleCheckingSuiteVehicleChesadfsa";
            //ownerid 50
            vehicle.OwnerId = "VehicleCheckingSuiteVehicleCheckingSuiteVehicleCheasdfas";
            //ownerphone 50
            vehicle.OwnerPhone = "VehicleCheckingSuiteVehicleCheckingSuiteVehicleChe";

            vehicle.OwnerEmail = null;
            //owneraddress 512
            vehicle.OwnerAddress = null;
            for (int i = 0; i < 64; i++)
            {
                string a = "VehicleCheckin";
                temp = temp + a;
                vehicle.OwnerAddress = temp;
            }
            VehicleService service = new VehicleService();
            SingleMessage<bool> ret = service.AddVehicle(vehicle);
            Assert.IsFalse(ret.IsSuccess);
        }

        /// <summary>
        /// 空格
        /// </summary>
        [TestMethod]
        public void TestStringForSpace()
        {
            Vehicle vehicle = new Vehicle();
            //vehicleid 20
            vehicle.VehicleId = "Apertú   ñrtaAnón";
            //vehiclesn 50
            vehicle.VehicleSn = "AtúradePuñ   óúuñ   óntúrade";
            //BrandModel 200
            vehicle.BrandModel = "";
            string temp = null;
            for (int i = 0; i < 40; i++)
            {
                string a = " ú uñ";
                temp = temp + a;
                vehicle.Region = temp;
            }
            vehicle.CityCode = null;
            //startyear 10
            vehicle.StartYear = "úñ  ó";
            //note 2000
            vehicle.Note = null;
            for (int i = 0; i < 100; i++)
            {
                string a = "ú  Puñ Anón";
                temp = temp + a;
                vehicle.Note = temp;
            }
            //region 512
            vehicle.Region = "";
            for (int i = 0; i < 64; i++)
            {
                string a = " úóuñ ";
                temp = temp + a;
                vehicle.Region = temp;
            }
            //operating lisences 100
            vehicle.OperatingLicense = "Apertú  ñrtaAnónApertúradePuñrtaAnón rtúradeApertúr uñrtaAnón túradePuñrtaAnónApertúrade";
            //Engine ID 50
            vehicle.EngineId = " túradePuñrtaAnónApertúr ñrtaAnónApertúrade";
            //vehicle.ServerType = null;
            //owner 50
            vehicle.Owner = " úradePuñr ónApertúradePuñrtaAnónApertúrade";
            //ownerid 50
            vehicle.OwnerId = "Apertú uñrtaAnó úradePuñrtaAnónApertúrade";
            //ownerphone 50
            vehicle.OwnerPhone = "Apertúr uñrtaAnó úradePuñrtaAnónApertúrade";

            vehicle.OwnerEmail = null;
            //owneraddress 512
            vehicle.OwnerAddress = null;
            for (int i = 0; i < 64; i++)
            {
                string a = " údó ñg";
                temp = temp + a;
                vehicle.OwnerAddress = temp;
            }
            VehicleService service = new VehicleService();
            SingleMessage<bool> ret = service.AddVehicle(vehicle);
            Assert.IsFalse(ret.IsSuccess);
        }

        /// <summary>
        /// ,
        /// </summary>
        [TestMethod]
        public void TestStringForSpecialCharacter_Comma()
        {
            Vehicle vehicle = new Vehicle();
            //vehicleid 20
            vehicle.VehicleId = "Apertú,ñrta,Anón";
            //vehiclesn 50
            vehicle.VehicleSn = "AtúradePuñ,óúuñ,óntúrade";
            //BrandModel 200
            vehicle.BrandModel = "";
            string temp = null;
            for (int i = 0; i < 40; i++)
            {
                string a = "ú,uñ";
                temp = temp + a;
                vehicle.Region = temp;
            }
            vehicle.CityCode = null;
            //startyear 10
            vehicle.StartYear = "úñ,ó";
            //note 2000
            vehicle.Note = null;
            for (int i = 0; i < 100; i++)
            {
                string a = "ú,Puñ,nón";
                temp = temp + a;
                vehicle.Note = temp;
            }
            //region 512
            vehicle.Region = "";
            for (int i = 0; i < 64; i++)
            {
                string a = "úó,ñ ";
                temp = temp + a;
                vehicle.Region = temp;
            }
            //operating lisences 100
            vehicle.OperatingLicense = "ApertúñrtaAnónApertúradePuñrtaAnón,rtúradeApertúr,uñrtaAnón,túradePuñrtaAnónApertúrade";
            //Engine ID 50
            vehicle.EngineId = "túradePuñrta,nónApertúr,ñrtaAnónApertúrade";
            //vehicle.ServerType = null;
            //owner 50
            vehicle.Owner = ",úradePuñr,ónApertúradePuñrtaAnónApertúrade";
            //ownerid 50
            vehicle.OwnerId = "Apertú,uñrtaAnó,úradePuñrtaAnón,ertúrade";
            //ownerphone 50
            vehicle.OwnerPhone = "Apertúr,uñrtaAnó,úradePuñrtaAnónA,rtúrade";

            vehicle.OwnerEmail = null;
            //owneraddress 512
            vehicle.OwnerAddress = null;
            for (int i = 0; i < 64; i++)
            {
                string a = "údó,ñg";
                temp = temp + a;
                vehicle.OwnerAddress = temp;
            }
            VehicleService service = new VehicleService();
            SingleMessage<bool> ret = service.AddVehicle(vehicle);
            Assert.IsFalse(ret.IsSuccess);
        }

        /// <summary>
        /// ;
        /// </summary>
        [TestMethod]
        public void TestStringForSpecialCharacter_SemiComma()
        {
            Vehicle vehicle = new Vehicle();
            //vehicleid 20
            vehicle.VehicleId = "Apertú;ñrta;Anón";
            //vehiclesn 50
            vehicle.VehicleSn = "AtúradePuñ;óúuñ;óntúrade";
            //BrandModel 200
            vehicle.BrandModel = "";
            string temp = null;
            for (int i = 0; i < 40; i++)
            {
                string a = "ú;uñ";
                temp = temp + a;
                vehicle.Region = temp;
            }
            vehicle.CityCode = null;
            //startyear 10
            vehicle.StartYear = "úñ;ó";
            //note 2000
            vehicle.Note = null;
            for (int i = 0; i < 100; i++)
            {
                string a = "ú;Puñ;nón";
                temp = temp + a;
                vehicle.Note = temp;
            }
            //region 512
            vehicle.Region = "";
            for (int i = 0; i < 64; i++)
            {
                string a = "úó;ñ";
                temp = temp + a;
                vehicle.Region = temp;
            }
            //operating lisences 100
            vehicle.OperatingLicense = "ApertúñrtaAnónApertúradePuñrtaAnón;rtúradeApertúr;uñrtaAnón;túradePuñrtaAnónApertúrade";
            //Engine ID 50
            vehicle.EngineId = "túradePuñrta;nónApertúr;ñrtaAnónApertúrade";
            //vehicle.ServerType = null;
            //owner 50
            vehicle.Owner = "úradePuñr;ónApertúradePuñrtaAnónApertúrade";
            //ownerid 50
            vehicle.OwnerId = "Apertú;uñrtaAnó;úradePuñrtaAnón;ertúrade";
            //ownerphone 50
            vehicle.OwnerPhone = "Apertúr;uñrtaAnó;úradePuñrtaAnónA;rtúrade";

            vehicle.OwnerEmail = null;
            //owneraddress 512
            vehicle.OwnerAddress = null;
            for (int i = 0; i < 64; i++)
            {
                string a = "údó;ñg";
                temp = temp + a;
                vehicle.OwnerAddress = temp;
            }
            VehicleService service = new VehicleService();
            SingleMessage<bool> ret = service.AddVehicle(vehicle);
            Assert.IsFalse(ret.IsSuccess);
        }

        /// <summary>
        /// '
        /// </summary>
        [TestMethod]
        public void TestStringForSpecialCharacter_quote()
        {
            Vehicle vehicle = new Vehicle();
            //vehicleid 20
            vehicle.VehicleId = "Apertú'ñrta'Anón";
            //vehiclesn 50
            vehicle.VehicleSn = "AtúradePuñ'óúuñ'óntúrade";
            //BrandModel 200
            vehicle.BrandModel = "";
            string temp = null;
            for (int i = 0; i < 40; i++)
            {
                string a = "ú'uñ";
                temp = temp + a;
                vehicle.Region = temp;
            }
            vehicle.CityCode = null;
            //startyear 10
            vehicle.StartYear = "úñ'ó";
            //note 2000
            vehicle.Note = null;
            for (int i = 0; i < 100; i++)
            {
                string a = "ú'Puñ'nón";
                temp = temp + a;
                vehicle.Note = temp;
            }
            //region 512
            vehicle.Region = "";
            for (int i = 0; i < 64; i++)
            {
                string a = "úó'ñ";
                temp = temp + a;
                vehicle.Region = temp;
            }
            //operating lisences 100
            vehicle.OperatingLicense = "ApertúñrtaAnónApertúradePuñrtaAnón'rtúradeApertúr'uñrtaAnón'túradePuñrtaAnónApertúrade";
            //Engine ID 50
            vehicle.EngineId = "túradePuñrta'nónApertúr'ñrtaAnónApertúrade";
            //vehicle.ServerType = null;
            //owner 50
            vehicle.Owner = "úradePuñr'ónApertúradePuñrtaAnónApertúrade";
            //ownerid 50
            vehicle.OwnerId = "Apertú'uñrtaAnó'úradePuñrtaAnónertúrade";
            //ownerphone 50
            vehicle.OwnerPhone = "Apertúr'uñrtaAnó'úradePuñrtaAnónArtúrade";

            vehicle.OwnerEmail = null;
            //owneraddress 512
            vehicle.OwnerAddress = null;
            for (int i = 0; i < 64; i++)
            {
                string a = "údó'ñg";
                temp = temp + a;
                vehicle.OwnerAddress = temp;
            }
            VehicleService service = new VehicleService();
            SingleMessage<bool> ret = service.AddVehicle(vehicle);
            Assert.IsFalse(ret.IsSuccess);
        }

        /// <summary>
        /// update 
        /// </summary>
        [TestMethod]
        public void TestStringForSpecialCharacter_update()
        {
            Vehicle vehicle = new Vehicle();
            //vehicleid 20
            vehicle.VehicleId = "updateúñnón";
            //vehiclesn 50
            vehicle.VehicleSn = "updatepdateóntú";
            //BrandModel 200
            vehicle.BrandModel = "";
            string temp = null;
            for (int i = 0; i < 4; i++)
            {
                string a = "update";
                temp = temp + a;
                vehicle.Region = temp;
            }
            vehicle.CityCode = null;
            //startyear 10
            vehicle.StartYear = "update";
            //note 2000
            vehicle.Note = null;
            for (int i = 0; i < 10; i++)
            {
                string a = "update";
                temp = temp + a;
                vehicle.Note = temp;
            }
            //region 512
            vehicle.Region = "";
            for (int i = 0; i < 4; i++)
            {
                string a = "update";
                temp = temp + a;
                vehicle.Region = temp;
            }
            //operating lisences 100
            vehicle.OperatingLicense = "update";
            //Engine ID 50
            vehicle.EngineId = "túradeupdateúrade";
            //vehicle.ServerType = null;
            //owner 50
            vehicle.Owner = "úradePuupdateertúrade";
            //ownerid 50
            vehicle.OwnerId = "Apeupdatertúrade";
            //ownerphone 50
            vehicle.OwnerPhone = "Apertupdaterade";

            vehicle.OwnerEmail = null;
            //owneraddress 512
            vehicle.OwnerAddress = null;
            for (int i = 0; i < 4; i++)
            {
                string a = "update";
                temp = temp + a;
                vehicle.OwnerAddress = temp;
            }
            VehicleService service = new VehicleService();
            SingleMessage<bool> ret = service.AddVehicle(vehicle);
            Assert.IsFalse(ret.IsSuccess);
        }

        /// <summary>
        /// delete 
        /// </summary>
        [TestMethod]
        public void TestStringForSpecialCharacter_delete()
        {
            Vehicle vehicle = new Vehicle();
            //vehicleid 20
            vehicle.VehicleId = "Apedeleten";
            //vehiclesn 50
            vehicle.VehicleSn = "Atúradeleteade";
            //BrandModel 200
            vehicle.BrandModel = "";
            string temp = null;
            for (int i = 0; i < 4; i++)
            {
                string a = "delete";
                temp = temp + a;
                vehicle.Region = temp;
            }
            vehicle.CityCode = null;
            //startyear 10
            vehicle.StartYear = "delete";
            //note 2000
            vehicle.Note = null;
            for (int i = 0; i < 10; i++)
            {
                string a = "delete";
                temp = temp + a;
                vehicle.Note = temp;
            }
            //region 512
            vehicle.Region = "";
            for (int i = 0; i < 6; i++)
            {
                string a = "delete";
                temp = temp + a;
                vehicle.Region = temp;
            }
            //operating lisences 100
            vehicle.OperatingLicense = "ApertúñrdeleteónApertúrade";
            //Engine ID 50
            vehicle.EngineId = "túradePdeletepertúrade";
            //vehicle.ServerType = null;
            //owner 50
            vehicle.Owner = "úradePdeleteAnónApertúrade";
            //ownerid 50
            vehicle.OwnerId = "Aperdeletertúrade";
            //ownerphone 50
            vehicle.OwnerPhone = "Apertdeleterade";

            vehicle.OwnerEmail = null;
            //owneraddress 512
            vehicle.OwnerAddress = null;
            for (int i = 0; i < 4; i++)
            {
                string a = "delete";
                temp = temp + a;
                vehicle.OwnerAddress = temp;
            }
            VehicleService service = new VehicleService();
            SingleMessage<bool> ret = service.AddVehicle(vehicle);
            Assert.IsFalse(ret.IsSuccess);
        }

        /// <summary>
        /// select 
        /// </summary>
        [TestMethod]
        public void TestStringForSpecialCharacter_selete()
        {
            Vehicle vehicle = new Vehicle();
            //vehicleid 20
            vehicle.VehicleId = "Apselecten";
            //vehiclesn 50
            vehicle.VehicleSn = "Atselectde";
            //BrandModel 200
            vehicle.BrandModel = "";
            string temp = null;
            for (int i = 0; i < 4; i++)
            {
                string a = "select";
                temp = temp + a;
                vehicle.Region = temp;
            }
            vehicle.CityCode = null;
            //startyear 10
            vehicle.StartYear = "select";
            //note 2000
            vehicle.Note = null;
            for (int i = 0; i < 10; i++)
            {
                string a = "select";
                temp = temp + a;
                vehicle.Note = temp;
            }
            //region 512
            vehicle.Region = "";
            for (int i = 0; i < 6; i++)
            {
                string a = "select";
                temp = temp + a;
                vehicle.Region = temp;
            }
            //operating lisences 100
            vehicle.OperatingLicense = "AperselectónApertúrade";
            //Engine ID 50
            vehicle.EngineId = "túradePdselectúrade";
            //vehicle.ServerType = null;
            //owner 50
            vehicle.Owner = "úradePselectpertúrade";
            //ownerid 50
            vehicle.OwnerId = "Apeselecttúrade";
            //ownerphone 50
            vehicle.OwnerPhone = "Aperselectrade";

            vehicle.OwnerEmail = null;
            //owneraddress 512
            vehicle.OwnerAddress = null;
            for (int i = 0; i < 4; i++)
            {
                string a = "select";
                temp = temp + a;
                vehicle.OwnerAddress = temp;
            }
            VehicleService service = new VehicleService();
            SingleMessage<bool> ret = service.AddVehicle(vehicle);
            Assert.IsFalse(ret.IsSuccess);
        }

    }
}
