using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;
using Gsafety.PTMS.SecuritySuite.Contract;
using Gsafety.PTMS.SecuritySuite.Service;
using Gsafety.PTMS.Base.Contract.Data;
namespace FFF
{
    /// <summary>
    /// VehicleRepository 的摘要说明
    /// </summary>
    [TestClass]
    public class WorkingSuiteServiceTest
    {
        const string chinesecharacter = @"阿啊哀唉挨矮爱碍安岸按案暗昂袄傲奥八巴扒吧疤拔把坝爸罢霸白百柏摆败拜班般斑搬板版办半伴扮拌瓣帮绑榜膀傍棒包胞雹宝饱保堡报抱暴爆杯悲碑北贝备背倍被辈奔本笨蹦逼鼻比彼笔鄙币必毕闭毙弊碧蔽壁避臂边编鞭扁便变遍辨辩辫标表别宾滨冰兵丙柄饼并病拨波玻剥脖菠播伯驳泊博搏膊薄卜补捕不布步怖部擦猜才材财裁采彩睬踩菜参餐残蚕惭惨灿仓苍舱藏操槽草册侧厕测策层叉插查茶察岔差拆柴馋缠产铲颤昌长肠尝偿常厂场敞畅倡唱抄钞超朝潮吵炒车扯彻撤尘臣沉辰陈晨闯衬称趁撑成呈承诚城乘惩程秤吃驰迟持池匙尺齿耻斥赤翅充冲虫崇抽仇绸愁稠筹酬丑臭出初除厨锄础储楚处触畜川穿传船喘串疮窗床创吹炊垂锤春纯唇蠢词慈辞磁此次刺从匆葱聪丛凑粗促醋窜催摧脆翠村存寸错曾搭达答打大呆代带待怠贷袋逮戴丹单担耽胆旦但诞弹淡蛋当挡党荡档刀叨导岛倒蹈到悼盗道稻得德的灯登等凳低堤滴敌笛底抵地弟帝递第颠典点电店垫殿叼雕吊钓调掉爹跌叠蝶丁叮盯钉顶订定丢东冬董懂动冻栋洞都斗抖陡豆逗督毒读独堵赌杜肚度渡端短段断缎锻堆队对吨蹲盾顿多夺朵躲惰鹅蛾额恶饿恩儿而耳二发乏伐罚阀法帆番翻凡烦繁反返犯泛饭范贩方坊芳防妨房仿访纺放飞非肥匪废沸肺费分吩纷芬坟粉份奋愤粪丰风封疯峰锋蜂逢缝讽凤奉佛否夫肤伏扶服俘浮符幅福抚府斧俯辅腐父付妇负附咐复赴副傅富腹覆该改盖溉概干甘杆肝竿秆赶敢感冈刚岗纲缸钢港杠高膏糕搞稿告哥胳鸽割搁歌阁革格葛隔个各给根跟更耕工弓公功攻供宫恭躬巩共贡勾沟钩狗构购够估姑孤辜古谷股骨鼓固故顾瓜刮挂乖拐怪关观官冠馆管贯惯灌罐光广归龟规轨鬼柜贵桂跪滚棍锅国果裹过哈孩海害含寒喊汉汗旱航毫豪好号浩耗喝禾合何和河核荷盒贺黑痕很狠恨恒横衡轰哄烘红宏洪虹喉猴吼后厚候乎呼忽狐胡壶湖糊蝴虎互户护花华哗滑猾化划画话怀槐坏欢还环缓幻唤换患荒慌皇黄煌晃谎灰恢挥辉回悔汇会绘贿惠毁慧昏婚浑魂混活火伙或货获祸惑击饥圾机肌鸡迹积基绩激及吉级即极急疾集籍几己挤脊计记纪忌技际剂季既济继寄加夹佳家嘉甲价驾架假嫁稼奸尖坚歼间肩艰兼监煎拣俭茧捡减剪检简见件建剑荐贱健舰渐践鉴键箭江姜将浆僵疆讲奖桨匠降酱交郊娇浇骄胶椒焦蕉角狡绞饺脚搅缴叫轿较教阶皆接揭街节劫杰洁结捷截竭姐解介戒届界借巾今斤金津筋仅紧谨锦尽劲近进晋浸禁京经茎惊晶睛精井颈景警净径竞竟敬境静镜纠究揪九久酒旧救就舅居拘鞠局菊橘举矩句巨拒具俱剧惧据距锯聚捐卷倦绢决绝觉掘嚼军君均菌俊卡开凯慨刊堪砍做看康糠扛抗炕考烤靠科棵颗壳咳可渴克刻客课肯垦恳坑空孔恐控口扣寇枯哭苦库裤酷夸垮挎跨块快宽款筐狂况旷矿框亏葵愧昆捆困扩括阔垃拉啦喇腊蜡辣来赖兰拦栏蓝篮览懒烂滥郎狼廊朗浪捞劳牢老姥涝乐勒雷垒泪类累冷厘梨狸离犁鹂璃黎礼李里理力历厉立丽利励例隶栗粒俩连帘怜莲联廉镰脸练炼恋链良凉梁粮粱两亮谅辆量辽疗僚了料列劣烈猎裂邻林临淋伶灵岭铃陵零龄领令另溜刘流留榴柳六龙笼聋隆垄拢楼搂漏露芦炉虏鲁陆录鹿滤碌路驴旅屡律虑率绿卵乱掠略轮论罗萝锣箩骡螺络骆落妈麻马码蚂骂吗埋买迈麦卖脉蛮馒瞒满慢漫忙芒盲茫猫毛矛茅茂冒贸帽貌么没眉梅煤霉每美妹门闷们萌盟猛蒙孟梦迷谜米眯秘密蜜眠绵棉免勉面苗描秒妙庙灭蔑民敏名明鸣命摸模膜摩磨魔抹末沫莫漠墨默谋某母亩木目牧墓幕慕暮拿哪内那纳乃奶耐男南难囊挠恼脑闹呢嫩能尼泥你逆年念娘酿鸟尿捏您宁凝牛扭纽农浓弄奴努怒女暖挪欧偶辟趴爬怕拍牌派攀盘判叛盼乓旁胖抛炮袍跑泡陪培赔佩配喷盆朋棚蓬膨捧碰批披劈皮疲脾匹僻片偏篇骗漂飘票撇拼贫品乒平评凭苹瓶萍坡泼婆迫破魄剖仆扑铺葡朴普谱七妻戚期欺漆齐其奇骑棋旗乞企岂启起气弃汽砌器恰洽千迁牵铅谦签前钱钳潜浅遣欠歉枪腔强墙抢悄敲锹乔侨桥瞧巧切茄且窃亲侵芹琴禽勤青轻倾清蜻情晴顷请庆穷丘秋求球区曲驱屈趋渠取去趣圈全权泉拳犬劝券缺却雀确鹊裙群然燃染嚷壤让饶扰绕惹热人仁忍刃认任扔仍日绒荣容熔融柔揉肉如乳辱入软锐瑞润若弱撒洒塞赛三伞散桑嗓丧扫嫂色森杀沙纱傻筛晒山删衫闪陕扇善伤商裳晌赏上尚捎梢烧稍勺少绍哨舌蛇舍设社射涉摄申伸身深神沈审婶肾甚渗慎升生声牲胜绳省圣盛剩尸失师诗施狮湿十什石时识实拾蚀食史使始驶士氏世市示式事侍势视试饰室是柿适逝释誓收手守首寿受兽售授瘦书叔殊梳疏舒输蔬熟暑鼠属薯术束述树竖数刷耍衰摔甩帅拴双霜爽谁水税睡顺说嗽丝司私思斯撕死四寺似饲肆松宋诵送颂搜艘苏俗诉肃素速宿塑酸蒜算虽随岁碎穗孙损笋缩所索锁她他它塌塔踏台抬太态泰贪摊滩坛谈痰坦毯叹炭探汤唐堂塘膛糖倘躺烫趟涛掏滔逃桃陶淘萄讨套特疼腾梯踢提题蹄体剃惕替天添田甜填挑条跳贴铁帖厅听亭庭停挺艇通同桐铜童统桶筒痛偷头投透秃突图徒涂途屠土吐兔团推腿退吞屯托拖脱驼妥娃挖蛙瓦袜歪外弯湾丸完玩顽挽晚碗万汪亡王网往妄忘旺望危威微为围违唯维伟伪尾委卫未位味畏胃喂慰温文纹闻蚊稳问翁窝我沃卧握乌污呜屋无吴五午伍武侮舞勿务物误悟雾夕西吸希析息牺悉惜稀溪锡熄膝习席袭洗喜戏系细隙虾瞎峡狭霞下吓夏厦仙先纤掀鲜闲弦贤咸衔嫌显险县现线限宪陷馅羡献乡相香箱详祥享响想向巷项象像橡削宵消销小晓孝效校笑些歇协邪胁斜携鞋写泄泻卸屑械谢心辛欣新薪信兴星腥刑行形型醒杏姓幸性凶兄胸雄熊休修羞朽秀绣袖锈须虚需徐许序叙绪续絮蓄宣悬旋选穴学雪血寻巡旬询循训讯迅压呀押鸦鸭牙芽崖哑雅亚咽烟淹延严言岩沿炎研盐蜒颜掩眼演厌宴艳验焰雁燕央殃秧扬羊阳杨洋仰养氧痒样妖腰邀窑谣摇遥咬药要耀爷也冶野业叶页夜液一衣医依仪宜姨移遗疑乙已以蚁倚椅义亿忆艺议亦异役译易疫益谊意毅翼因阴姻音银引饮隐印应英樱鹰迎盈营蝇赢影映硬佣拥庸永咏泳勇涌用优忧悠尤由犹邮油游友有又右幼诱于予余鱼娱渔愉愚榆与宇屿羽雨语玉育郁狱浴预域欲御裕遇愈誉冤元员园原圆援缘源远怨院愿约月钥悦阅跃越云匀允孕运晕韵杂灾栽宰载再在咱暂赞脏葬遭糟早枣澡灶皂造燥躁则择泽责贼怎增赠渣扎轧闸眨炸榨摘宅窄债寨沾粘斩展盏崭占战站张章涨掌丈仗帐胀障招找召兆赵照罩遮折哲者这浙贞针侦珍真诊枕阵振镇震争征挣睁筝蒸整正证郑政症之支汁芝枝知织肢脂蜘执侄直值职植殖止只旨址纸指至志制帜治质秩致智置中忠终钟肿种众重州舟周洲粥宙昼皱骤朱株珠诸猪蛛竹烛逐主煮嘱住助注驻柱祝著筑铸抓爪专砖转赚庄装壮状撞追准捉桌浊啄着仔姿资滋子紫字自宗棕踪总纵走奏租足族阻组祖钻嘴最罪醉尊遵昨左作坐座做";
        const string spanishcharacter = "qwértyúíopasdghjklñ´zxcvbnm?¿¡!1234567890 \"";
        const string englishcharacter = "abcdefghigklmnopqrstuvwxyz1234567890 ;*";
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
            WorkingSuite workingsuite = new WorkingSuite();
            WorkingSuiteService service = new WorkingSuiteService();
            workingsuite.Id = null;
            workingsuite.VehicleId = null;
            workingsuite.MdvrCoreId = null;
            workingsuite.AbnormalCause = null;
            workingsuite.Status = null;
            workingsuite.SwitchTime = null;
            workingsuite.OnlineFlag = null;
            workingsuite.AbnormalCause = null;
            workingsuite.SuiteId = null;
            workingsuite.FaultTime = null;
            SingleMessage<int> ret = service.AddWorkingSuite(workingsuite);
            Assert.IsFalse(ret.IsSuccess);
        }


        /// <summary>
        /// 允许的字段为空
        /// </summary>
        [TestMethod]
        public void TestStringForAllowEmpty()
        {
            WorkingSuite workingsuite = new WorkingSuite();
            WorkingSuiteService service = new WorkingSuiteService();
            StringBuilder builder = new StringBuilder();
            Random random = new Random();
            workingsuite.Id = "cd2d49d8-64ab-40c6-9437-2b12e709c123";
            workingsuite.VehicleId = null;
            workingsuite.MdvrCoreId = "";
            workingsuite.AbnormalCause = null;
            workingsuite.VehicleId = null;
            workingsuite.AbnormalCause = null;
            workingsuite.Status = null;
            workingsuite.SwitchTime = null;
            workingsuite.OnlineFlag = null;
            workingsuite.AbnormalCause = null;
            workingsuite.SuiteId = null;
            SingleMessage<int> ret = service.AddWorkingSuite(workingsuite);
            Assert.IsTrue(ret.Result == 1 || ret.Result == 2);
        }
        /// <summary>
        /// 测试空格
        /// </summary>
        [TestMethod]
        public void TestStringForSpace()
        {
            WorkingSuite workingsuite = new WorkingSuite();
            WorkingSuiteService service = new WorkingSuiteService();
            StringBuilder builder = new StringBuilder();
            Random random = new Random();
            workingsuite.Id = "cd2d49d8-64ab-40c6-9437-2b12e709c128";
            builder.Length = 0;

            builder.Append(" ");
            for (int i = 1; i < 9; i++)
            {
                builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
            }
            builder.Append(" ");
            workingsuite.VehicleId = builder.ToString();
            builder.Length = 0;

            builder.Append(" ");
            for (int i = 1; i < 9; i++)
            {
                builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
            }
            builder.Append(" ");
            workingsuite.MdvrCoreId = builder.ToString();
            builder.Length = 0;

            builder.Append(" ");
            for (int i = 1; i < 99; i++)
            {
                builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
            }
            builder.Append(" ");
            workingsuite.AbnormalCause = builder.ToString();
            builder.Length = 0;

            SingleMessage<int> ret = service.AddWorkingSuite(workingsuite);
            Assert.IsTrue(ret.Result == 1 || ret.Result == 2);

        }


        /// <summary>
        /// 测试,
        /// </summary>
        [TestMethod]
        public void TestStringForSpecialCharacter_Comma()
        {
            WorkingSuite workingsuite = new WorkingSuite();
            WorkingSuiteService service = new WorkingSuiteService();
            StringBuilder builder = new StringBuilder();
            Random random = new Random();

            workingsuite.Id = "cd2d49d8-64ab-40c6-9437-2b12e709c127";
            builder.Length = 0;

            builder.Append(",");
            for (int i = 1; i < 9; i++)
            {
                builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
            }
            builder.Append(",");
            workingsuite.VehicleId = builder.ToString();
            builder.Length = 0;

            builder.Append(",");
            for (int i = 1; i < 9; i++)
            {
                builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
            }
            builder.Append(",");
            workingsuite.MdvrCoreId = builder.ToString();
            builder.Length = 0;

            builder.Append(",");
            for (int i = 1; i < 99; i++)
            {
                builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
            }
            builder.Append(",");
            workingsuite.AbnormalCause = builder.ToString();
            builder.Length = 0;

            SingleMessage<int> ret = service.AddWorkingSuite(workingsuite);
            Assert.IsTrue(ret.Result == 1 || ret.Result == 2);
        }


        /// <summary>
        /// 测试;
        /// </summary>
        [TestMethod]
        public void TestStringForSpecialCharacter_SemiComma()
        {
            WorkingSuite workingsuite = new WorkingSuite();
            WorkingSuiteService service = new WorkingSuiteService();
            StringBuilder builder = new StringBuilder();
            Random random = new Random();

            workingsuite.Id = "cd2d49d8-64ab-40c6-9437-2b12e709c129";
            builder.Length = 0;

            builder.Append(";");
            for (int i = 1; i < 9; i++)
            {
                builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
            }
            builder.Append(";");
            workingsuite.VehicleId = builder.ToString();
            builder.Length = 0;

            builder.Append(";");
            for (int i = 1; i < 9; i++)
            {
                builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
            }
            builder.Append(";");
            workingsuite.MdvrCoreId = builder.ToString();
            builder.Length = 0;

            builder.Append(";");
            for (int i = 1; i < 99; i++)
            {
                builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
            }
            builder.Append(";");
            workingsuite.AbnormalCause = builder.ToString();
            builder.Length = 0;

            SingleMessage<int> ret = service.AddWorkingSuite(workingsuite);
            Assert.IsTrue(ret.Result == 1 || ret.Result == 2);

        }


        /// <summary>
        /// 测试"
        /// </summary>
        [TestMethod]
        public void TestStringForSpecialCharacter_quote()
        {
            WorkingSuite workingsuite = new WorkingSuite();
            WorkingSuiteService service = new WorkingSuiteService();
            StringBuilder builder = new StringBuilder();
            Random random = new Random();

            workingsuite.Id = "cd2d49d8-64ab-40c6-9437-2b12e709c131";
            builder.Length = 0;

            builder.Append("\"");
            for (int i = 1; i < 9; i++)
            {
                builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
            }
            builder.Append("\"");
            workingsuite.VehicleId = builder.ToString();
            builder.Length = 0;

            builder.Append("\"");
            for (int i = 1; i < 9; i++)
            {
                builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
            }
            builder.Append("\"");
            workingsuite.MdvrCoreId = builder.ToString();
            builder.Length = 0;

            builder.Append("\"");
            for (int i = 1; i < 99; i++)
            {
                builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
            }
            builder.Append("\"");
            workingsuite.AbnormalCause = builder.ToString();
            builder.Length = 0;

            SingleMessage<int> ret = service.AddWorkingSuite(workingsuite);
            Assert.IsTrue(ret.Result == 1 || ret.Result == 2);

        }



        /// <summary>
        /// 测试最大值
        /// </summary>
        [TestMethod]
        public void TestNumberForMax()
        {
            WorkingSuite workingsuite = new WorkingSuite();
            WorkingSuiteService service = new WorkingSuiteService();
            workingsuite.Id = "cd2d49d8-64ab-40c6-9437-2b12e709c132";
            workingsuite.VehicleId = "";
            workingsuite.MdvrCoreId = "";
            workingsuite.Status = int.MaxValue;
            workingsuite.OnlineFlag = int.MaxValue;
            workingsuite.AbnormalCause = "";
            SingleMessage<int> ret = service.AddWorkingSuite(workingsuite);
            Assert.IsTrue(ret.Result == 1 || ret.Result == 2);
        }


        /// <summary>
        /// 测试最小值
        /// </summary>
        [TestMethod]
        public void TestNumberForMin()
        {
            WorkingSuite workingsuite = new WorkingSuite();
            WorkingSuiteService service = new WorkingSuiteService();
            workingsuite.Id = "cd2d49d8-64ab-40c6-9437-2b12e709c133";
            workingsuite.VehicleId = "";
            workingsuite.MdvrCoreId = "";
            workingsuite.Status = int.MinValue;
            workingsuite.OnlineFlag = int.MinValue;
            workingsuite.AbnormalCause = "";
            SingleMessage<int> ret = service.AddWorkingSuite(workingsuite);
            Assert.IsTrue(ret.Result == 1 || ret.Result == 2);

        }


        /// <summary>
        /// 正数
        /// </summary>
        [TestMethod]
        public void TestNumberForPositive()
        {
            WorkingSuite workingsuite = new WorkingSuite();
            WorkingSuiteService service = new WorkingSuiteService();
            workingsuite.Id = "cd2d49d8-64ab-40c6-9437-2b12e709c134";
            workingsuite.VehicleId = "";
            workingsuite.MdvrCoreId = "";
            workingsuite.Status = 1;
            workingsuite.OnlineFlag = 1;
            workingsuite.AbnormalCause = "";
            SingleMessage<int> ret = service.AddWorkingSuite(workingsuite);
            Assert.IsTrue(ret.Result == 1 || ret.Result == 2);
        }


        /// <summary>
        /// 负数
        /// </summary>
        [TestMethod]
        public void TestNumberForNegative()
        {
            WorkingSuite workingsuite = new WorkingSuite();
            WorkingSuiteService service = new WorkingSuiteService();
            workingsuite.Id = "cd2d49d8-64ab-40c6-9437-2b12e709c135";
            workingsuite.VehicleId = "";
            workingsuite.MdvrCoreId = "";
            workingsuite.Status = -1;
            workingsuite.OnlineFlag = -1;
            workingsuite.AbnormalCause = "";
            SingleMessage<int> ret = service.AddWorkingSuite(workingsuite);
            Assert.IsTrue(ret.Result == 1 || ret.Result == 2);

        }


        /// <summary>
        /// 零
        /// </summary>
        [TestMethod]
        public void TestNumberForZero()
        {
            WorkingSuite workingsuite = new WorkingSuite();
            WorkingSuiteService service = new WorkingSuiteService();
            workingsuite.Id = "cd2d49d8-64ab-40c6-9437-2b12e709c136";
            workingsuite.VehicleId = "";
            workingsuite.MdvrCoreId = "";
            workingsuite.Status = 0;
            workingsuite.OnlineFlag = 0;
            workingsuite.AbnormalCause = "";
            SingleMessage<int> ret = service.AddWorkingSuite(workingsuite);
            Assert.IsTrue(ret.Result == 1 || ret.Result == 2);

        }

        /// <summary>
        /// DateTime
        /// </summary>
        [TestMethod]
        public void TestDateTime()
        {
            WorkingSuite workingsuite = new WorkingSuite();
            WorkingSuiteService service = new WorkingSuiteService();
            workingsuite.Id = "";
            workingsuite.VehicleId = "";
            workingsuite.MdvrCoreId = "";
            workingsuite.AbnormalCause = "";
            SingleMessage<int> ret = service.AddWorkingSuite(workingsuite);
            //Assert.IsFalse(ret.IsSuccess);
        }


        /// <summary>
        /// 日期为空
        /// </summary>
        [TestMethod]
        public void TestIdDateTimeForNull()
        {
            WorkingSuite workingsuite = new WorkingSuite();
            WorkingSuiteService service = new WorkingSuiteService();
            workingsuite.Id = "";
            workingsuite.VehicleId = "";
            workingsuite.MdvrCoreId = "";
            workingsuite.AbnormalCause = "";
            SingleMessage<int> ret = service.AddWorkingSuite(workingsuite);
            //Assert.IsFalse(ret.IsSuccess);
        }


        /// <summary>
        /// 日期为空
        /// </summary>
        [TestMethod]
        public void TestVehicleIdDateTimeForNull()
        {
            WorkingSuite workingsuite = new WorkingSuite();
            WorkingSuiteService service = new WorkingSuiteService();
            workingsuite.Id = "";
            workingsuite.VehicleId = "";
            workingsuite.MdvrCoreId = "";
            workingsuite.AbnormalCause = "";
            SingleMessage<int> ret = service.AddWorkingSuite(workingsuite);
            //Assert.IsFalse(ret.IsSuccess);
        }


        /// <summary>
        /// 日期为空
        /// </summary>
        [TestMethod]
        public void TestMdvrCoreIdDateTimeForNull()
        {
            WorkingSuite workingsuite = new WorkingSuite();
            WorkingSuiteService service = new WorkingSuiteService();
            workingsuite.Id = "";
            workingsuite.VehicleId = "";
            workingsuite.MdvrCoreId = "";
            workingsuite.AbnormalCause = "";
            SingleMessage<int> ret = service.AddWorkingSuite(workingsuite);
            //Assert.IsFalse(ret.IsSuccess);
        }


        /// <summary>
        /// 日期为空
        /// </summary>
        [TestMethod]
        public void TestStatusDateTimeForNull()
        {
            WorkingSuite workingsuite = new WorkingSuite();
            WorkingSuiteService service = new WorkingSuiteService();
            workingsuite.Id = "";
            workingsuite.VehicleId = "";
            workingsuite.MdvrCoreId = "";
            workingsuite.AbnormalCause = "";
            SingleMessage<int> ret = service.AddWorkingSuite(workingsuite);
            //Assert.IsFalse(ret.IsSuccess);
        }


        /// <summary>
        /// 日期为空
        /// </summary>
        [TestMethod]
        public void TestSwitchTimeDateTimeForNull()
        {
            WorkingSuite workingsuite = new WorkingSuite();
            WorkingSuiteService service = new WorkingSuiteService();
            workingsuite.Id = "";
            workingsuite.VehicleId = "";
            workingsuite.MdvrCoreId = "";
            workingsuite.AbnormalCause = "";
            SingleMessage<int> ret = service.AddWorkingSuite(workingsuite);
            //Assert.IsFalse(ret.IsSuccess);
        }


        /// <summary>
        /// 日期为空
        /// </summary>
        [TestMethod]
        public void TestOnlineFlagDateTimeForNull()
        {
            WorkingSuite workingsuite = new WorkingSuite();
            WorkingSuiteService service = new WorkingSuiteService();
            workingsuite.Id = "";
            workingsuite.VehicleId = "";
            workingsuite.MdvrCoreId = "";
            workingsuite.AbnormalCause = "";
            SingleMessage<int> ret = service.AddWorkingSuite(workingsuite);
            //Assert.IsFalse(ret.IsSuccess);
        }


        /// <summary>
        /// 日期为空
        /// </summary>
        [TestMethod]
        public void TestAbnormalCauseDateTimeForNull()
        {
            WorkingSuite workingsuite = new WorkingSuite();
            WorkingSuiteService service = new WorkingSuiteService();
            workingsuite.Id = "";
            workingsuite.VehicleId = "";
            workingsuite.MdvrCoreId = "";
            workingsuite.AbnormalCause = "";
            SingleMessage<int> ret = service.AddWorkingSuite(workingsuite);
            //Assert.IsFalse(ret.IsSuccess);
        }


        /// <summary>
        /// 日期为空
        /// </summary>
        [TestMethod]
        public void TestSuiteIdDateTimeForNull()
        {
            WorkingSuite workingsuite = new WorkingSuite();
            WorkingSuiteService service = new WorkingSuiteService();
            workingsuite.Id = "";
            workingsuite.VehicleId = "";
            workingsuite.MdvrCoreId = "";
            workingsuite.AbnormalCause = "";
            SingleMessage<int> ret = service.AddWorkingSuite(workingsuite);
            //Assert.IsFalse(ret.IsSuccess);
        }


        /// <summary>
        /// 日期为空
        /// </summary>
        [TestMethod]
        public void TestFaultTimeDateTimeForNull()
        {
            WorkingSuite workingsuite = new WorkingSuite();
            WorkingSuiteService service = new WorkingSuiteService();
            workingsuite.Id = "";
            workingsuite.VehicleId = "";
            workingsuite.MdvrCoreId = "";
            workingsuite.AbnormalCause = "";
            SingleMessage<int> ret = service.AddWorkingSuite(workingsuite);
            //Assert.IsFalse(ret.IsSuccess);
        }


        /// <summary>
        /// Id日期为空
        /// </summary>
        [TestMethod]
        public void TestIdDateTimeForIncomplete()
        {
            WorkingSuite workingsuite = new WorkingSuite();
            WorkingSuiteService service = new WorkingSuiteService();
            workingsuite.Id = "";
            workingsuite.VehicleId = "";
            workingsuite.MdvrCoreId = "";
            workingsuite.AbnormalCause = "";
            SingleMessage<int> ret = service.AddWorkingSuite(workingsuite);
            //Assert.IsFalse(ret.IsSuccess);
        }


        /// <summary>
        /// VehicleId日期为空
        /// </summary>
        [TestMethod]
        public void TestVehicleIdDateTimeForIncomplete()
        {
            WorkingSuite workingsuite = new WorkingSuite();
            WorkingSuiteService service = new WorkingSuiteService();
            workingsuite.Id = "";
            workingsuite.VehicleId = "";
            workingsuite.MdvrCoreId = "";
            workingsuite.AbnormalCause = "";
            SingleMessage<int> ret = service.AddWorkingSuite(workingsuite);
            //Assert.IsFalse(ret.IsSuccess);
        }


        /// <summary>
        /// MdvrCoreId日期为空
        /// </summary>
        [TestMethod]
        public void TestMdvrCoreIdDateTimeForIncomplete()
        {
            WorkingSuite workingsuite = new WorkingSuite();
            WorkingSuiteService service = new WorkingSuiteService();
            workingsuite.Id = "";
            workingsuite.VehicleId = "";
            workingsuite.MdvrCoreId = "";
            workingsuite.AbnormalCause = "";
            SingleMessage<int> ret = service.AddWorkingSuite(workingsuite);
            //Assert.IsFalse(ret.IsSuccess);
        }


        /// <summary>
        /// Status日期为空
        /// </summary>
        [TestMethod]
        public void TestStatusDateTimeForIncomplete()
        {
            WorkingSuite workingsuite = new WorkingSuite();
            WorkingSuiteService service = new WorkingSuiteService();
            workingsuite.Id = "";
            workingsuite.VehicleId = "";
            workingsuite.MdvrCoreId = "";
            workingsuite.AbnormalCause = "";
            SingleMessage<int> ret = service.AddWorkingSuite(workingsuite);
            //Assert.IsFalse(ret.IsSuccess);
        }


        /// <summary>
        /// SwitchTime日期为空
        /// </summary>
        [TestMethod]
        public void TestSwitchTimeDateTimeForIncomplete()
        {
            WorkingSuite workingsuite = new WorkingSuite();
            WorkingSuiteService service = new WorkingSuiteService();
            workingsuite.Id = "";
            workingsuite.VehicleId = "";
            workingsuite.MdvrCoreId = "";
            workingsuite.AbnormalCause = "";
            SingleMessage<int> ret = service.AddWorkingSuite(workingsuite);
            //Assert.IsFalse(ret.IsSuccess);
        }


        /// <summary>
        /// OnlineFlag日期为空
        /// </summary>
        [TestMethod]
        public void TestOnlineFlagDateTimeForIncomplete()
        {
            WorkingSuite workingsuite = new WorkingSuite();
            WorkingSuiteService service = new WorkingSuiteService();
            workingsuite.Id = "";
            workingsuite.VehicleId = "";
            workingsuite.MdvrCoreId = "";
            workingsuite.AbnormalCause = "";
            SingleMessage<int> ret = service.AddWorkingSuite(workingsuite);
            //Assert.IsFalse(ret.IsSuccess);
        }


        /// <summary>
        /// AbnormalCause日期为空
        /// </summary>
        [TestMethod]
        public void TestAbnormalCauseDateTimeForIncomplete()
        {
            WorkingSuite workingsuite = new WorkingSuite();
            WorkingSuiteService service = new WorkingSuiteService();
            workingsuite.Id = "";
            workingsuite.VehicleId = "";
            workingsuite.MdvrCoreId = "";
            workingsuite.AbnormalCause = "";
            SingleMessage<int> ret = service.AddWorkingSuite(workingsuite);
            //Assert.IsFalse(ret.IsSuccess);
        }


        /// <summary>
        /// SuiteId日期为空
        /// </summary>
        [TestMethod]
        public void TestSuiteIdDateTimeForIncomplete()
        {
            WorkingSuite workingsuite = new WorkingSuite();
            WorkingSuiteService service = new WorkingSuiteService();
            workingsuite.Id = "";
            workingsuite.VehicleId = "";
            workingsuite.MdvrCoreId = "";
            workingsuite.AbnormalCause = "";
            SingleMessage<int> ret = service.AddWorkingSuite(workingsuite);
            //Assert.IsFalse(ret.IsSuccess);
        }


        /// <summary>
        /// FaultTime日期为空
        /// </summary>
        [TestMethod]
        public void TestFaultTimeDateTimeForIncomplete()
        {
            WorkingSuite workingsuite = new WorkingSuite();
            WorkingSuiteService service = new WorkingSuiteService();
            workingsuite.Id = "";
            workingsuite.VehicleId = "";
            workingsuite.MdvrCoreId = "";
            workingsuite.AbnormalCause = "";
            SingleMessage<int> ret = service.AddWorkingSuite(workingsuite);
            //Assert.IsFalse(ret.IsSuccess);
        }


        /// <summary>
        /// 测试Id日期格式错误
        /// </summary>
        [TestMethod]
        public void TestIdDateTimeForFormatError()
        {
            WorkingSuite workingsuite = new WorkingSuite();
            WorkingSuiteService service = new WorkingSuiteService();
            workingsuite.Id = "";
            workingsuite.VehicleId = "";
            workingsuite.MdvrCoreId = "";
            workingsuite.AbnormalCause = "";
            SingleMessage<int> ret = service.AddWorkingSuite(workingsuite);
            //Assert.IsFalse(ret.IsSuccess);
        }


        /// <summary>
        /// 测试VehicleId日期格式错误
        /// </summary>
        [TestMethod]
        public void TestVehicleIdDateTimeForFormatError()
        {
            WorkingSuite workingsuite = new WorkingSuite();
            WorkingSuiteService service = new WorkingSuiteService();
            workingsuite.Id = "";
            workingsuite.VehicleId = "";
            workingsuite.MdvrCoreId = "";
            workingsuite.AbnormalCause = "";
            SingleMessage<int> ret = service.AddWorkingSuite(workingsuite);
            //Assert.IsFalse(ret.IsSuccess);
        }


        /// <summary>
        /// 测试MdvrCoreId日期格式错误
        /// </summary>
        [TestMethod]
        public void TestMdvrCoreIdDateTimeForFormatError()
        {
            WorkingSuite workingsuite = new WorkingSuite();
            WorkingSuiteService service = new WorkingSuiteService();
            workingsuite.Id = "";
            workingsuite.VehicleId = "";
            workingsuite.MdvrCoreId = "";
            workingsuite.AbnormalCause = "";
            SingleMessage<int> ret = service.AddWorkingSuite(workingsuite);
            //Assert.IsFalse(ret.IsSuccess);
        }


        /// <summary>
        /// 测试Status日期格式错误
        /// </summary>
        [TestMethod]
        public void TestStatusDateTimeForFormatError()
        {
            WorkingSuite workingsuite = new WorkingSuite();
            WorkingSuiteService service = new WorkingSuiteService();
            workingsuite.Id = "";
            workingsuite.VehicleId = "";
            workingsuite.MdvrCoreId = "";
            workingsuite.AbnormalCause = "";
            SingleMessage<int> ret = service.AddWorkingSuite(workingsuite);
            //Assert.IsFalse(ret.IsSuccess);
        }


        /// <summary>
        /// 测试SwitchTime日期格式错误
        /// </summary>
        [TestMethod]
        public void TestSwitchTimeDateTimeForFormatError()
        {
            WorkingSuite workingsuite = new WorkingSuite();
            WorkingSuiteService service = new WorkingSuiteService();
            workingsuite.Id = "";
            workingsuite.VehicleId = "";
            workingsuite.MdvrCoreId = "";
            workingsuite.AbnormalCause = "";
            SingleMessage<int> ret = service.AddWorkingSuite(workingsuite);
            //Assert.IsFalse(ret.IsSuccess);
        }
        /// <summary>
        /// 测试SuiteId日期格式错误
        /// </summary>
        [TestMethod]
        public void TestSuiteIdDateTimeForFormatError()
        {
            WorkingSuite workingsuite = new WorkingSuite();
            WorkingSuiteService service = new WorkingSuiteService();
            workingsuite.Id = "";
            workingsuite.VehicleId = "";
            workingsuite.MdvrCoreId = "";
            workingsuite.AbnormalCause = "";
            SingleMessage<int> ret = service.AddWorkingSuite(workingsuite);
            //Assert.IsFalse(ret.IsSuccess);
        }


        /// <summary>
        /// 测试FaultTime日期格式错误
        /// </summary>
        [TestMethod]
        public void TestFaultTimeDateTimeForFormatError()
        {
            WorkingSuite workingsuite = new WorkingSuite();
            WorkingSuiteService service = new WorkingSuiteService();
            workingsuite.Id = "";
            workingsuite.VehicleId = "";
            workingsuite.MdvrCoreId = "";
            workingsuite.AbnormalCause = "";
            SingleMessage<int> ret = service.AddWorkingSuite(workingsuite);
            //Assert.IsFalse(ret.IsSuccess);
        }


        /// <summary>
        /// 测试Id日期最大值
        /// </summary>
        [TestMethod]
        public void TestIdDateTimeForMax()
        {
            WorkingSuite workingsuite = new WorkingSuite();
            WorkingSuiteService service = new WorkingSuiteService();
            workingsuite.Id = "";
            workingsuite.VehicleId = "";
            workingsuite.MdvrCoreId = "";
            workingsuite.AbnormalCause = "";
            SingleMessage<int> ret = service.AddWorkingSuite(workingsuite);
            //Assert.IsFalse(ret.IsSuccess);
        }


        /// <summary>
        /// 测试VehicleId日期最大值
        /// </summary>
        [TestMethod]
        public void TestVehicleIdDateTimeForMax()
        {
            WorkingSuite workingsuite = new WorkingSuite();
            WorkingSuiteService service = new WorkingSuiteService();
            workingsuite.Id = "";
            workingsuite.VehicleId = "";
            workingsuite.MdvrCoreId = "";
            workingsuite.AbnormalCause = "";
            SingleMessage<int> ret = service.AddWorkingSuite(workingsuite);
            //Assert.IsFalse(ret.IsSuccess);
        }


        /// <summary>
        /// 测试MdvrCoreId日期最大值
        /// </summary>
        [TestMethod]
        public void TestMdvrCoreIdDateTimeForMax()
        {
            WorkingSuite workingsuite = new WorkingSuite();
            WorkingSuiteService service = new WorkingSuiteService();
            workingsuite.Id = "";
            workingsuite.VehicleId = "";
            workingsuite.MdvrCoreId = "";
            workingsuite.AbnormalCause = "";
            SingleMessage<int> ret = service.AddWorkingSuite(workingsuite);
            //Assert.IsFalse(ret.IsSuccess);
        }


        /// <summary>
        /// 测试Status日期最大值
        /// </summary>
        [TestMethod]
        public void TestStatusDateTimeForMax()
        {
            WorkingSuite workingsuite = new WorkingSuite();
            WorkingSuiteService service = new WorkingSuiteService();
            workingsuite.Id = "";
            workingsuite.VehicleId = "";
            workingsuite.MdvrCoreId = "";
            workingsuite.AbnormalCause = "";
            SingleMessage<int> ret = service.AddWorkingSuite(workingsuite);
            //Assert.IsFalse(ret.IsSuccess);
        }


        /// <summary>
        /// 测试SwitchTime日期最大值
        /// </summary>
        [TestMethod]
        public void TestSwitchTimeDateTimeForMax()
        {
            WorkingSuite workingsuite = new WorkingSuite();
            WorkingSuiteService service = new WorkingSuiteService();
            workingsuite.Id = "";
            workingsuite.VehicleId = "";
            workingsuite.MdvrCoreId = "";
            workingsuite.AbnormalCause = "";
            SingleMessage<int> ret = service.AddWorkingSuite(workingsuite);
            //Assert.IsFalse(ret.IsSuccess);
        }


        /// <summary>
        /// 测试OnlineFlag日期最大值
        /// </summary>
        [TestMethod]
        public void TestOnlineFlagDateTimeForMax()
        {
            WorkingSuite workingsuite = new WorkingSuite();
            WorkingSuiteService service = new WorkingSuiteService();
            workingsuite.Id = "";
            workingsuite.VehicleId = "";
            workingsuite.MdvrCoreId = "";
            workingsuite.AbnormalCause = "";
            SingleMessage<int> ret = service.AddWorkingSuite(workingsuite);
            //Assert.IsFalse(ret.IsSuccess);
        }


        /// <summary>
        /// 测试AbnormalCause日期最大值
        /// </summary>
        [TestMethod]
        public void TestAbnormalCauseDateTimeForMax()
        {
            WorkingSuite workingsuite = new WorkingSuite();
            WorkingSuiteService service = new WorkingSuiteService();
            workingsuite.Id = "";
            workingsuite.VehicleId = "";
            workingsuite.MdvrCoreId = "";
            workingsuite.AbnormalCause = "";
            SingleMessage<int> ret = service.AddWorkingSuite(workingsuite);
            //Assert.IsFalse(ret.IsSuccess);
        }


        /// <summary>
        /// 测试SuiteId日期最大值
        /// </summary>
        [TestMethod]
        public void TestSuiteIdDateTimeForMax()
        {
            WorkingSuite workingsuite = new WorkingSuite();
            WorkingSuiteService service = new WorkingSuiteService();
            workingsuite.Id = "";
            workingsuite.VehicleId = "";
            workingsuite.MdvrCoreId = "";
            workingsuite.AbnormalCause = "";
            SingleMessage<int> ret = service.AddWorkingSuite(workingsuite);
            //Assert.IsFalse(ret.IsSuccess);
        }


        /// <summary>
        /// 测试FaultTime日期最大值
        /// </summary>
        [TestMethod]
        public void TestFaultTimeDateTimeForMax()
        {
            WorkingSuite workingsuite = new WorkingSuite();
            WorkingSuiteService service = new WorkingSuiteService();
            workingsuite.Id = "";
            workingsuite.VehicleId = "";
            workingsuite.MdvrCoreId = "";
            workingsuite.AbnormalCause = "";
            SingleMessage<int> ret = service.AddWorkingSuite(workingsuite);
            //Assert.IsFalse(ret.IsSuccess);
        }


        /// <summary>
        /// 测试Id日期格式错误
        /// </summary>
        [TestMethod]
        public void TestIdDateTimeForMin()
        {
            WorkingSuite workingsuite = new WorkingSuite();
            WorkingSuiteService service = new WorkingSuiteService();
            workingsuite.Id = "";
            workingsuite.VehicleId = "";
            workingsuite.MdvrCoreId = "";
            workingsuite.AbnormalCause = "";
            SingleMessage<int> ret = service.AddWorkingSuite(workingsuite);
            //Assert.IsFalse(ret.IsSuccess);
        }


        /// <summary>
        /// 测试VehicleId日期格式错误
        /// </summary>
        [TestMethod]
        public void TestVehicleIdDateTimeForMin()
        {
            WorkingSuite workingsuite = new WorkingSuite();
            WorkingSuiteService service = new WorkingSuiteService();
            workingsuite.Id = "";
            workingsuite.VehicleId = "";
            workingsuite.MdvrCoreId = "";
            workingsuite.AbnormalCause = "";
            SingleMessage<int> ret = service.AddWorkingSuite(workingsuite);
            //Assert.IsFalse(ret.IsSuccess);
        }


        /// <summary>
        /// 测试MdvrCoreId日期格式错误
        /// </summary>
        [TestMethod]
        public void TestMdvrCoreIdDateTimeForMin()
        {
            WorkingSuite workingsuite = new WorkingSuite();
            WorkingSuiteService service = new WorkingSuiteService();
            workingsuite.Id = "";
            workingsuite.VehicleId = "";
            workingsuite.MdvrCoreId = "";
            workingsuite.AbnormalCause = "";
            SingleMessage<int> ret = service.AddWorkingSuite(workingsuite);
            //Assert.IsFalse(ret.IsSuccess);
        }


        /// <summary>
        /// 测试Status日期格式错误
        /// </summary>
        [TestMethod]
        public void TestStatusDateTimeForMin()
        {
            WorkingSuite workingsuite = new WorkingSuite();
            WorkingSuiteService service = new WorkingSuiteService();
            workingsuite.Id = "";
            workingsuite.VehicleId = "";
            workingsuite.MdvrCoreId = "";
            workingsuite.AbnormalCause = "";
            SingleMessage<int> ret = service.AddWorkingSuite(workingsuite);
            //Assert.IsFalse(ret.IsSuccess);
        }


        /// <summary>
        /// 测试SwitchTime日期格式错误
        /// </summary>
        [TestMethod]
        public void TestSwitchTimeDateTimeForMin()
        {
            WorkingSuite workingsuite = new WorkingSuite();
            WorkingSuiteService service = new WorkingSuiteService();
            workingsuite.Id = "";
            workingsuite.VehicleId = "";
            workingsuite.MdvrCoreId = "";
            workingsuite.AbnormalCause = "";
            SingleMessage<int> ret = service.AddWorkingSuite(workingsuite);
            //Assert.IsFalse(ret.IsSuccess);
        }


        /// <summary>
        /// 测试OnlineFlag日期格式错误
        /// </summary>
        [TestMethod]
        public void TestOnlineFlagDateTimeForMin()
        {
            WorkingSuite workingsuite = new WorkingSuite();
            WorkingSuiteService service = new WorkingSuiteService();
            workingsuite.Id = "";
            workingsuite.VehicleId = "";
            workingsuite.MdvrCoreId = "";
            workingsuite.AbnormalCause = "";
            SingleMessage<int> ret = service.AddWorkingSuite(workingsuite);
            //Assert.IsFalse(ret.IsSuccess);
        }


        /// <summary>
        /// 测试AbnormalCause日期格式错误
        /// </summary>
        [TestMethod]
        public void TestAbnormalCauseDateTimeForMin()
        {
            WorkingSuite workingsuite = new WorkingSuite();
            WorkingSuiteService service = new WorkingSuiteService();
            workingsuite.Id = "";
            workingsuite.VehicleId = "";
            workingsuite.MdvrCoreId = "";
            workingsuite.AbnormalCause = "";
            SingleMessage<int> ret = service.AddWorkingSuite(workingsuite);
            //Assert.IsFalse(ret.IsSuccess);
        }


        /// <summary>
        /// 测试SuiteId日期格式错误
        /// </summary>
        [TestMethod]
        public void TestSuiteIdDateTimeForMin()
        {
            WorkingSuite workingsuite = new WorkingSuite();
            WorkingSuiteService service = new WorkingSuiteService();
            workingsuite.Id = "";
            workingsuite.VehicleId = "";
            workingsuite.MdvrCoreId = "";
            workingsuite.AbnormalCause = "";
            SingleMessage<int> ret = service.AddWorkingSuite(workingsuite);
            //Assert.IsFalse(ret.IsSuccess);
        }


        /// <summary>
        /// 测试FaultTime日期格式错误
        /// </summary>
        [TestMethod]
        public void TestFaultTimeDateTimeForMin()
        {
            WorkingSuite workingsuite = new WorkingSuite();
            WorkingSuiteService service = new WorkingSuiteService();
            workingsuite.Id = "";
            workingsuite.VehicleId = "";
            workingsuite.MdvrCoreId = "";
            workingsuite.AbnormalCause = "";
            SingleMessage<int> ret = service.AddWorkingSuite(workingsuite);
            //Assert.IsFalse(ret.IsSuccess);
        }



        /// <summary>
        /// update no exists
        /// </summary>
        [TestMethod]
        public void TestSwitchTimeUpdateNotExist()
        {
            WorkingSuite workingsuite = new WorkingSuite();
            WorkingSuiteService service = new WorkingSuiteService();
            workingsuite.Id = "";
            workingsuite.VehicleId = "";
            workingsuite.MdvrCoreId = "";
            workingsuite.SwitchTime = DateTime.MinValue;
            workingsuite.AbnormalCause = "";
            SingleMessage<bool> ret = service.UpdateWorkingSuite(workingsuite);
            //Assert.IsFalse(ret.IsSuccess);
        }



        /// <summary>
        /// update no exists
        /// </summary>
        [TestMethod]
        public void TestSuiteIdUpdateNotExist()
        {
            WorkingSuite workingsuite = new WorkingSuite();
            WorkingSuiteService service = new WorkingSuiteService();
            workingsuite.Id = "";
            workingsuite.VehicleId = "";
            workingsuite.MdvrCoreId = "";
            workingsuite.AbnormalCause = "";
            workingsuite.SuiteId = DateTime.MinValue.ToString();
            SingleMessage<bool> ret = service.UpdateWorkingSuite(workingsuite);
            //Assert.IsFalse(ret.IsSuccess);
        }


        /// <summary>
        /// update no exists
        /// </summary>
        [TestMethod]
        public void TestSwitchTimeDeleteNotExist()
        {
            WorkingSuite workingsuite = new WorkingSuite();
            WorkingSuiteService service = new WorkingSuiteService();
            workingsuite.Id = "";
            workingsuite.VehicleId = "";
            workingsuite.MdvrCoreId = "";
            workingsuite.SwitchTime = DateTime.MinValue;
            workingsuite.AbnormalCause = "";
            SingleMessage<int> ret = service.AddWorkingSuite(workingsuite);
            //Assert.IsFalse(ret.IsSuccess);
        }
    }
}

