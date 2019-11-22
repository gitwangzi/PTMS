using System;
using System.Text;
using System.Collections.Generic;
using Gsafety.PTMS.Alert.Contract.Data;
using Gsafety.PTMS.Alert.Repository;
using Gsafety.PTMS.Base.Contract.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;
namespace Gsafety.PTMS.Monitor.Test
{
	/// <summary>
	/// VehicleRepository 的摘要说明
	/// </summary>
	[TestClass]
	public class DeviceAlertServiceTest
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
			DeviceAlert devicealert = new DeviceAlert();
			DeviceAlertRespository service = new DeviceAlertRespository();
			devicealert.Id=null;
			devicealert.MdvrCoreId=null;
			devicealert.SuiteInfoId=null;
			devicealert.VehicleId=null;
			devicealert.Cmd=null;
			devicealert.Longitude=null;
			devicealert.Latitude=null;
			devicealert.Speed=null;
			devicealert.Direction=null;
			devicealert.GpsValid=null;
			devicealert.TagValue=null;
			SingleMessage<DeviceAlert> ret = service.AddDeviceAlert(devicealert);
			//Assert.IsFalse(ret.IsSuccess);
		}


		/// <summary>
		/// 允许的字段为空
		/// </summary>
		[TestMethod]
		public void TestStringForAllowEmpty()
		{
			DeviceAlert devicealert = new DeviceAlert();
			DeviceAlertRespository service = new DeviceAlertRespository();
			StringBuilder builder = new StringBuilder();
			Random random = new Random();
			devicealert.Id=null;
			devicealert.MdvrCoreId=null;
			devicealert.SuiteInfoId=null;
			devicealert.VehicleId=null;
			devicealert.Cmd=null;
			devicealert.Longitude=null;
			devicealert.Latitude=null;
			devicealert.Speed=null;
			devicealert.Direction=null;
			devicealert.GpsValid=null;
			devicealert.TagValue=null;
			SingleMessage<DeviceAlert> ret = service.AddDeviceAlert(devicealert);
			//Assert.IsFalse(ret.IsSuccess);
		}


		/// <summary>
		/// 测试最大中文长度
		/// </summary>
		[TestMethod]
		public void TestStringForMaxLength_Chinese()
		{
			DeviceAlert devicealert = new DeviceAlert();
			DeviceAlertRespository service = new DeviceAlertRespository();
			StringBuilder builder = new StringBuilder();
			Random random = new Random();
			for (int i = 0; i < 9; i++)
			{
				builder.Append(chinesecharacter[random.Next(chinesecharacter.Length - 1)]);
			}
			devicealert.Id=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 9; i++)
			{
				builder.Append(chinesecharacter[random.Next(chinesecharacter.Length - 1)]);
			}
			devicealert.MdvrCoreId=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 17; i++)
			{
				builder.Append(chinesecharacter[random.Next(chinesecharacter.Length - 1)]);
			}
			devicealert.SuiteInfoId=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 9; i++)
			{
				builder.Append(chinesecharacter[random.Next(chinesecharacter.Length - 1)]);
			}
			devicealert.VehicleId=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 9; i++)
			{
				builder.Append(chinesecharacter[random.Next(chinesecharacter.Length - 1)]);
			}
			devicealert.Cmd=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 9; i++)
			{
				builder.Append(chinesecharacter[random.Next(chinesecharacter.Length - 1)]);
			}
			devicealert.Longitude=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 9; i++)
			{
				builder.Append(chinesecharacter[random.Next(chinesecharacter.Length - 1)]);
			}
			devicealert.Latitude=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 9; i++)
			{
				builder.Append(chinesecharacter[random.Next(chinesecharacter.Length - 1)]);
			}
			devicealert.Speed=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 9; i++)
			{
				builder.Append(chinesecharacter[random.Next(chinesecharacter.Length - 1)]);
			}
			devicealert.Direction=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 0; i++)
			{
				builder.Append(chinesecharacter[random.Next(chinesecharacter.Length - 1)]);
			}
			devicealert.GpsValid=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 127; i++)
			{
				builder.Append(chinesecharacter[random.Next(chinesecharacter.Length - 1)]);
			}
			devicealert.TagValue=builder.ToString();
			builder.Length=0;

			SingleMessage<DeviceAlert> ret = service.AddDeviceAlert(devicealert);
			//Assert.IsFalse(ret.IsSuccess);
		}


		/// <summary>
		/// 测试西文最大长度
		/// </summary>
		[TestMethod]
		public void TestStringForMaxLength_Spanish()
		{
			DeviceAlert devicealert = new DeviceAlert();
			DeviceAlertRespository service = new DeviceAlertRespository();
			StringBuilder builder = new StringBuilder();
			Random random = new Random();
			for (int i = 0; i < 9; i++)
			{
				builder.Append(spanishcharacter[random.Next(spanishcharacter.Length - 1)]);
			}
			devicealert.Id=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 9; i++)
			{
				builder.Append(spanishcharacter[random.Next(spanishcharacter.Length - 1)]);
			}
			devicealert.MdvrCoreId=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 17; i++)
			{
				builder.Append(spanishcharacter[random.Next(spanishcharacter.Length - 1)]);
			}
			devicealert.SuiteInfoId=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 9; i++)
			{
				builder.Append(spanishcharacter[random.Next(spanishcharacter.Length - 1)]);
			}
			devicealert.VehicleId=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 9; i++)
			{
				builder.Append(spanishcharacter[random.Next(spanishcharacter.Length - 1)]);
			}
			devicealert.Cmd=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 9; i++)
			{
				builder.Append(spanishcharacter[random.Next(spanishcharacter.Length - 1)]);
			}
			devicealert.Longitude=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 9; i++)
			{
				builder.Append(spanishcharacter[random.Next(spanishcharacter.Length - 1)]);
			}
			devicealert.Latitude=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 9; i++)
			{
				builder.Append(spanishcharacter[random.Next(spanishcharacter.Length - 1)]);
			}
			devicealert.Speed=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 9; i++)
			{
				builder.Append(spanishcharacter[random.Next(spanishcharacter.Length - 1)]);
			}
			devicealert.Direction=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 0; i++)
			{
				builder.Append(spanishcharacter[random.Next(spanishcharacter.Length - 1)]);
			}
			devicealert.GpsValid=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 127; i++)
			{
				builder.Append(spanishcharacter[random.Next(spanishcharacter.Length - 1)]);
			}
			devicealert.TagValue=builder.ToString();
			builder.Length=0;

			SingleMessage<DeviceAlert> ret = service.AddDeviceAlert(devicealert);
			//Assert.IsFalse(ret.IsSuccess);
		}


		/// <summary>
		/// 测试英文最大长度
		/// </summary>
		[TestMethod]
		public void TestStringForMaxLength_English()
		{
			DeviceAlert devicealert = new DeviceAlert();
			DeviceAlertRespository service = new DeviceAlertRespository();
			StringBuilder builder = new StringBuilder();
			Random random = new Random();
			for (int i = 0; i < 9; i++)
			{
				builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
			}
			devicealert.Id=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 9; i++)
			{
				builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
			}
			devicealert.MdvrCoreId=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 17; i++)
			{
				builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
			}
			devicealert.SuiteInfoId=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 9; i++)
			{
				builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
			}
			devicealert.VehicleId=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 9; i++)
			{
				builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
			}
			devicealert.Cmd=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 9; i++)
			{
				builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
			}
			devicealert.Longitude=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 9; i++)
			{
				builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
			}
			devicealert.Latitude=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 9; i++)
			{
				builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
			}
			devicealert.Speed=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 9; i++)
			{
				builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
			}
			devicealert.Direction=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 0; i++)
			{
				builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
			}
			devicealert.GpsValid=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 127; i++)
			{
				builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
			}
			devicealert.TagValue=builder.ToString();
			builder.Length=0;

			SingleMessage<DeviceAlert> ret = service.AddDeviceAlert(devicealert);
			//Assert.IsFalse(ret.IsSuccess);
		}


		/// <summary>
		/// 测试Id中文超长
		/// </summary>
		[TestMethod]
		public void TestIdStringForOverLength_Chinese()
		{
			DeviceAlert devicealert = new DeviceAlert();
			DeviceAlertRespository service = new DeviceAlertRespository();
			StringBuilder builder = new StringBuilder();
			Random random = new Random();
			for (int i = 0; i < 10; i++)
			{
				builder.Append(chinesecharacter[random.Next(chinesecharacter.Length - 1)]);
			}
			devicealert.Id=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 9; i++)
			{
				builder.Append(chinesecharacter[random.Next(chinesecharacter.Length - 1)]);
			}
			devicealert.MdvrCoreId=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 17; i++)
			{
				builder.Append(chinesecharacter[random.Next(chinesecharacter.Length - 1)]);
			}
			devicealert.SuiteInfoId=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 9; i++)
			{
				builder.Append(chinesecharacter[random.Next(chinesecharacter.Length - 1)]);
			}
			devicealert.VehicleId=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 9; i++)
			{
				builder.Append(chinesecharacter[random.Next(chinesecharacter.Length - 1)]);
			}
			devicealert.Cmd=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 9; i++)
			{
				builder.Append(chinesecharacter[random.Next(chinesecharacter.Length - 1)]);
			}
			devicealert.Longitude=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 9; i++)
			{
				builder.Append(chinesecharacter[random.Next(chinesecharacter.Length - 1)]);
			}
			devicealert.Latitude=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 9; i++)
			{
				builder.Append(chinesecharacter[random.Next(chinesecharacter.Length - 1)]);
			}
			devicealert.Speed=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 9; i++)
			{
				builder.Append(chinesecharacter[random.Next(chinesecharacter.Length - 1)]);
			}
			devicealert.Direction=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 0; i++)
			{
				builder.Append(chinesecharacter[random.Next(chinesecharacter.Length - 1)]);
			}
			devicealert.GpsValid=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 127; i++)
			{
				builder.Append(chinesecharacter[random.Next(chinesecharacter.Length - 1)]);
			}
			devicealert.TagValue=builder.ToString();
			builder.Length=0;

			SingleMessage<DeviceAlert> ret = service.AddDeviceAlert(devicealert);
			//Assert.IsFalse(ret.IsSuccess);
		}


		/// <summary>
		/// 测试Id西文超长
		/// </summary>
		[TestMethod]
		public void TestIdStringForMaxLength_Spanish()
		{
			DeviceAlert devicealert = new DeviceAlert();
			DeviceAlertRespository service = new DeviceAlertRespository();
			StringBuilder builder = new StringBuilder();
			Random random = new Random();
			for (int i = 0; i < 10; i++)
			{
				builder.Append(spanishcharacter[random.Next(spanishcharacter.Length - 1)]);
			}
			devicealert.Id=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 9; i++)
			{
				builder.Append(spanishcharacter[random.Next(spanishcharacter.Length - 1)]);
			}
			devicealert.MdvrCoreId=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 17; i++)
			{
				builder.Append(spanishcharacter[random.Next(spanishcharacter.Length - 1)]);
			}
			devicealert.SuiteInfoId=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 9; i++)
			{
				builder.Append(spanishcharacter[random.Next(spanishcharacter.Length - 1)]);
			}
			devicealert.VehicleId=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 9; i++)
			{
				builder.Append(spanishcharacter[random.Next(spanishcharacter.Length - 1)]);
			}
			devicealert.Cmd=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 9; i++)
			{
				builder.Append(spanishcharacter[random.Next(spanishcharacter.Length - 1)]);
			}
			devicealert.Longitude=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 9; i++)
			{
				builder.Append(spanishcharacter[random.Next(spanishcharacter.Length - 1)]);
			}
			devicealert.Latitude=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 9; i++)
			{
				builder.Append(spanishcharacter[random.Next(spanishcharacter.Length - 1)]);
			}
			devicealert.Speed=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 9; i++)
			{
				builder.Append(spanishcharacter[random.Next(spanishcharacter.Length - 1)]);
			}
			devicealert.Direction=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 0; i++)
			{
				builder.Append(spanishcharacter[random.Next(spanishcharacter.Length - 1)]);
			}
			devicealert.GpsValid=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 127; i++)
			{
				builder.Append(spanishcharacter[random.Next(spanishcharacter.Length - 1)]);
			}
			devicealert.TagValue=builder.ToString();
			builder.Length=0;

			SingleMessage<DeviceAlert> ret = service.AddDeviceAlert(devicealert);
			//Assert.IsFalse(ret.IsSuccess);
		}


		/// <summary>
		/// 测试Id英文超长
		/// </summary>
		[TestMethod]
		public void TestIdStringForMaxLength_English()
		{
			DeviceAlert devicealert = new DeviceAlert();
			DeviceAlertRespository service = new DeviceAlertRespository();
			StringBuilder builder = new StringBuilder();
			Random random = new Random();
			for (int i = 0; i < 10; i++)
			{
				builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
			}
			devicealert.Id=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 9; i++)
			{
				builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
			}
			devicealert.MdvrCoreId=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 17; i++)
			{
				builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
			}
			devicealert.SuiteInfoId=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 9; i++)
			{
				builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
			}
			devicealert.VehicleId=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 9; i++)
			{
				builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
			}
			devicealert.Cmd=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 9; i++)
			{
				builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
			}
			devicealert.Longitude=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 9; i++)
			{
				builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
			}
			devicealert.Latitude=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 9; i++)
			{
				builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
			}
			devicealert.Speed=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 9; i++)
			{
				builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
			}
			devicealert.Direction=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 0; i++)
			{
				builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
			}
			devicealert.GpsValid=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 127; i++)
			{
				builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
			}
			devicealert.TagValue=builder.ToString();
			builder.Length=0;

			SingleMessage<DeviceAlert> ret = service.AddDeviceAlert(devicealert);
			//Assert.IsFalse(ret.IsSuccess);
		}


		/// <summary>
		/// 测试MdvrCoreId中文超长
		/// </summary>
		[TestMethod]
		public void TestMdvrCoreIdStringForOverLength_Chinese()
		{
			DeviceAlert devicealert = new DeviceAlert();
			DeviceAlertRespository service = new DeviceAlertRespository();
			StringBuilder builder = new StringBuilder();
			Random random = new Random();
			for (int i = 0; i < 9; i++)
			{
				builder.Append(chinesecharacter[random.Next(chinesecharacter.Length - 1)]);
			}
			devicealert.Id=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 10; i++)
			{
				builder.Append(chinesecharacter[random.Next(chinesecharacter.Length - 1)]);
			}
			devicealert.MdvrCoreId=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 17; i++)
			{
				builder.Append(chinesecharacter[random.Next(chinesecharacter.Length - 1)]);
			}
			devicealert.SuiteInfoId=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 9; i++)
			{
				builder.Append(chinesecharacter[random.Next(chinesecharacter.Length - 1)]);
			}
			devicealert.VehicleId=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 9; i++)
			{
				builder.Append(chinesecharacter[random.Next(chinesecharacter.Length - 1)]);
			}
			devicealert.Cmd=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 9; i++)
			{
				builder.Append(chinesecharacter[random.Next(chinesecharacter.Length - 1)]);
			}
			devicealert.Longitude=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 9; i++)
			{
				builder.Append(chinesecharacter[random.Next(chinesecharacter.Length - 1)]);
			}
			devicealert.Latitude=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 9; i++)
			{
				builder.Append(chinesecharacter[random.Next(chinesecharacter.Length - 1)]);
			}
			devicealert.Speed=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 9; i++)
			{
				builder.Append(chinesecharacter[random.Next(chinesecharacter.Length - 1)]);
			}
			devicealert.Direction=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 0; i++)
			{
				builder.Append(chinesecharacter[random.Next(chinesecharacter.Length - 1)]);
			}
			devicealert.GpsValid=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 127; i++)
			{
				builder.Append(chinesecharacter[random.Next(chinesecharacter.Length - 1)]);
			}
			devicealert.TagValue=builder.ToString();
			builder.Length=0;

			SingleMessage<DeviceAlert> ret = service.AddDeviceAlert(devicealert);
			//Assert.IsFalse(ret.IsSuccess);
		}


		/// <summary>
		/// 测试MdvrCoreId西文超长
		/// </summary>
		[TestMethod]
		public void TestMdvrCoreIdStringForMaxLength_Spanish()
		{
			DeviceAlert devicealert = new DeviceAlert();
			DeviceAlertRespository service = new DeviceAlertRespository();
			StringBuilder builder = new StringBuilder();
			Random random = new Random();
			for (int i = 0; i < 9; i++)
			{
				builder.Append(spanishcharacter[random.Next(spanishcharacter.Length - 1)]);
			}
			devicealert.Id=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 10; i++)
			{
				builder.Append(spanishcharacter[random.Next(spanishcharacter.Length - 1)]);
			}
			devicealert.MdvrCoreId=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 17; i++)
			{
				builder.Append(spanishcharacter[random.Next(spanishcharacter.Length - 1)]);
			}
			devicealert.SuiteInfoId=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 9; i++)
			{
				builder.Append(spanishcharacter[random.Next(spanishcharacter.Length - 1)]);
			}
			devicealert.VehicleId=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 9; i++)
			{
				builder.Append(spanishcharacter[random.Next(spanishcharacter.Length - 1)]);
			}
			devicealert.Cmd=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 9; i++)
			{
				builder.Append(spanishcharacter[random.Next(spanishcharacter.Length - 1)]);
			}
			devicealert.Longitude=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 9; i++)
			{
				builder.Append(spanishcharacter[random.Next(spanishcharacter.Length - 1)]);
			}
			devicealert.Latitude=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 9; i++)
			{
				builder.Append(spanishcharacter[random.Next(spanishcharacter.Length - 1)]);
			}
			devicealert.Speed=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 9; i++)
			{
				builder.Append(spanishcharacter[random.Next(spanishcharacter.Length - 1)]);
			}
			devicealert.Direction=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 0; i++)
			{
				builder.Append(spanishcharacter[random.Next(spanishcharacter.Length - 1)]);
			}
			devicealert.GpsValid=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 127; i++)
			{
				builder.Append(spanishcharacter[random.Next(spanishcharacter.Length - 1)]);
			}
			devicealert.TagValue=builder.ToString();
			builder.Length=0;

			SingleMessage<DeviceAlert> ret = service.AddDeviceAlert(devicealert);
			//Assert.IsFalse(ret.IsSuccess);
		}


		/// <summary>
		/// 测试MdvrCoreId英文超长
		/// </summary>
		[TestMethod]
		public void TestMdvrCoreIdStringForMaxLength_English()
		{
			DeviceAlert devicealert = new DeviceAlert();
			DeviceAlertRespository service = new DeviceAlertRespository();
			StringBuilder builder = new StringBuilder();
			Random random = new Random();
			for (int i = 0; i < 9; i++)
			{
				builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
			}
			devicealert.Id=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 10; i++)
			{
				builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
			}
			devicealert.MdvrCoreId=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 17; i++)
			{
				builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
			}
			devicealert.SuiteInfoId=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 9; i++)
			{
				builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
			}
			devicealert.VehicleId=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 9; i++)
			{
				builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
			}
			devicealert.Cmd=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 9; i++)
			{
				builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
			}
			devicealert.Longitude=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 9; i++)
			{
				builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
			}
			devicealert.Latitude=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 9; i++)
			{
				builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
			}
			devicealert.Speed=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 9; i++)
			{
				builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
			}
			devicealert.Direction=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 0; i++)
			{
				builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
			}
			devicealert.GpsValid=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 127; i++)
			{
				builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
			}
			devicealert.TagValue=builder.ToString();
			builder.Length=0;

			SingleMessage<DeviceAlert> ret = service.AddDeviceAlert(devicealert);
			//Assert.IsFalse(ret.IsSuccess);
		}


		/// <summary>
		/// 测试SuiteId中文超长
		/// </summary>
		[TestMethod]
		public void TestSuiteIdStringForOverLength_Chinese()
		{
			DeviceAlert devicealert = new DeviceAlert();
			DeviceAlertRespository service = new DeviceAlertRespository();
			StringBuilder builder = new StringBuilder();
			Random random = new Random();
			for (int i = 0; i < 9; i++)
			{
				builder.Append(chinesecharacter[random.Next(chinesecharacter.Length - 1)]);
			}
			devicealert.Id=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 9; i++)
			{
				builder.Append(chinesecharacter[random.Next(chinesecharacter.Length - 1)]);
			}
			devicealert.MdvrCoreId=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 17; i++)
			{
				builder.Append(chinesecharacter[random.Next(chinesecharacter.Length - 1)]);
			}
			devicealert.SuiteInfoId=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 9; i++)
			{
				builder.Append(chinesecharacter[random.Next(chinesecharacter.Length - 1)]);
			}
			devicealert.VehicleId=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 9; i++)
			{
				builder.Append(chinesecharacter[random.Next(chinesecharacter.Length - 1)]);
			}
			devicealert.Cmd=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 9; i++)
			{
				builder.Append(chinesecharacter[random.Next(chinesecharacter.Length - 1)]);
			}
			devicealert.Longitude=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 9; i++)
			{
				builder.Append(chinesecharacter[random.Next(chinesecharacter.Length - 1)]);
			}
			devicealert.Latitude=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 9; i++)
			{
				builder.Append(chinesecharacter[random.Next(chinesecharacter.Length - 1)]);
			}
			devicealert.Speed=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 9; i++)
			{
				builder.Append(chinesecharacter[random.Next(chinesecharacter.Length - 1)]);
			}
			devicealert.Direction=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 0; i++)
			{
				builder.Append(chinesecharacter[random.Next(chinesecharacter.Length - 1)]);
			}
			devicealert.GpsValid=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 127; i++)
			{
				builder.Append(chinesecharacter[random.Next(chinesecharacter.Length - 1)]);
			}
			devicealert.TagValue=builder.ToString();
			builder.Length=0;

			SingleMessage<DeviceAlert> ret = service.AddDeviceAlert(devicealert);
			//Assert.IsFalse(ret.IsSuccess);
		}


		/// <summary>
		/// 测试SuiteId西文超长
		/// </summary>
		[TestMethod]
		public void TestSuiteIdStringForMaxLength_Spanish()
		{
			DeviceAlert devicealert = new DeviceAlert();
			DeviceAlertRespository service = new DeviceAlertRespository();
			StringBuilder builder = new StringBuilder();
			Random random = new Random();
			for (int i = 0; i < 9; i++)
			{
				builder.Append(spanishcharacter[random.Next(spanishcharacter.Length - 1)]);
			}
			devicealert.Id=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 9; i++)
			{
				builder.Append(spanishcharacter[random.Next(spanishcharacter.Length - 1)]);
			}
			devicealert.MdvrCoreId=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 17; i++)
			{
				builder.Append(spanishcharacter[random.Next(spanishcharacter.Length - 1)]);
			}
			devicealert.SuiteInfoId=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 9; i++)
			{
				builder.Append(spanishcharacter[random.Next(spanishcharacter.Length - 1)]);
			}
			devicealert.VehicleId=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 9; i++)
			{
				builder.Append(spanishcharacter[random.Next(spanishcharacter.Length - 1)]);
			}
			devicealert.Cmd=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 9; i++)
			{
				builder.Append(spanishcharacter[random.Next(spanishcharacter.Length - 1)]);
			}
			devicealert.Longitude=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 9; i++)
			{
				builder.Append(spanishcharacter[random.Next(spanishcharacter.Length - 1)]);
			}
			devicealert.Latitude=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 9; i++)
			{
				builder.Append(spanishcharacter[random.Next(spanishcharacter.Length - 1)]);
			}
			devicealert.Speed=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 9; i++)
			{
				builder.Append(spanishcharacter[random.Next(spanishcharacter.Length - 1)]);
			}
			devicealert.Direction=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 0; i++)
			{
				builder.Append(spanishcharacter[random.Next(spanishcharacter.Length - 1)]);
			}
			devicealert.GpsValid=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 127; i++)
			{
				builder.Append(spanishcharacter[random.Next(spanishcharacter.Length - 1)]);
			}
			devicealert.TagValue=builder.ToString();
			builder.Length=0;

			SingleMessage<DeviceAlert> ret = service.AddDeviceAlert(devicealert);
			//Assert.IsFalse(ret.IsSuccess);
		}


		/// <summary>
		/// 测试SuiteId英文超长
		/// </summary>
		[TestMethod]
		public void TestSuiteIdStringForMaxLength_English()
		{
			DeviceAlert devicealert = new DeviceAlert();
			DeviceAlertRespository service = new DeviceAlertRespository();
			StringBuilder builder = new StringBuilder();
			Random random = new Random();
			for (int i = 0; i < 9; i++)
			{
				builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
			}
			devicealert.Id=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 9; i++)
			{
				builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
			}
			devicealert.MdvrCoreId=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 17; i++)
			{
				builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
			}
			devicealert.SuiteInfoId=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 9; i++)
			{
				builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
			}
			devicealert.VehicleId=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 9; i++)
			{
				builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
			}
			devicealert.Cmd=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 9; i++)
			{
				builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
			}
			devicealert.Longitude=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 9; i++)
			{
				builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
			}
			devicealert.Latitude=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 9; i++)
			{
				builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
			}
			devicealert.Speed=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 9; i++)
			{
				builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
			}
			devicealert.Direction=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 0; i++)
			{
				builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
			}
			devicealert.GpsValid=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 127; i++)
			{
				builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
			}
			devicealert.TagValue=builder.ToString();
			builder.Length=0;

			SingleMessage<DeviceAlert> ret = service.AddDeviceAlert(devicealert);
			//Assert.IsFalse(ret.IsSuccess);
		}


		/// <summary>
		/// 测试SuiteInfoId中文超长
		/// </summary>
		[TestMethod]
		public void TestSuiteInfoIdStringForOverLength_Chinese()
		{
			DeviceAlert devicealert = new DeviceAlert();
			DeviceAlertRespository service = new DeviceAlertRespository();
			StringBuilder builder = new StringBuilder();
			Random random = new Random();
			for (int i = 0; i < 9; i++)
			{
				builder.Append(chinesecharacter[random.Next(chinesecharacter.Length - 1)]);
			}
			devicealert.Id=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 9; i++)
			{
				builder.Append(chinesecharacter[random.Next(chinesecharacter.Length - 1)]);
			}
			devicealert.MdvrCoreId=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 18; i++)
			{
				builder.Append(chinesecharacter[random.Next(chinesecharacter.Length - 1)]);
			}
			devicealert.SuiteInfoId=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 9; i++)
			{
				builder.Append(chinesecharacter[random.Next(chinesecharacter.Length - 1)]);
			}
			devicealert.VehicleId=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 9; i++)
			{
				builder.Append(chinesecharacter[random.Next(chinesecharacter.Length - 1)]);
			}
			devicealert.Cmd=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 9; i++)
			{
				builder.Append(chinesecharacter[random.Next(chinesecharacter.Length - 1)]);
			}
			devicealert.Longitude=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 9; i++)
			{
				builder.Append(chinesecharacter[random.Next(chinesecharacter.Length - 1)]);
			}
			devicealert.Latitude=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 9; i++)
			{
				builder.Append(chinesecharacter[random.Next(chinesecharacter.Length - 1)]);
			}
			devicealert.Speed=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 9; i++)
			{
				builder.Append(chinesecharacter[random.Next(chinesecharacter.Length - 1)]);
			}
			devicealert.Direction=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 0; i++)
			{
				builder.Append(chinesecharacter[random.Next(chinesecharacter.Length - 1)]);
			}
			devicealert.GpsValid=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 127; i++)
			{
				builder.Append(chinesecharacter[random.Next(chinesecharacter.Length - 1)]);
			}
			devicealert.TagValue=builder.ToString();
			builder.Length=0;

			SingleMessage<DeviceAlert> ret = service.AddDeviceAlert(devicealert);
			//Assert.IsFalse(ret.IsSuccess);
		}


		/// <summary>
		/// 测试SuiteInfoId西文超长
		/// </summary>
		[TestMethod]
		public void TestSuiteInfoIdStringForMaxLength_Spanish()
		{
			DeviceAlert devicealert = new DeviceAlert();
			DeviceAlertRespository service = new DeviceAlertRespository();
			StringBuilder builder = new StringBuilder();
			Random random = new Random();
			for (int i = 0; i < 9; i++)
			{
				builder.Append(spanishcharacter[random.Next(spanishcharacter.Length - 1)]);
			}
			devicealert.Id=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 9; i++)
			{
				builder.Append(spanishcharacter[random.Next(spanishcharacter.Length - 1)]);
			}
			devicealert.MdvrCoreId=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 18; i++)
			{
				builder.Append(spanishcharacter[random.Next(spanishcharacter.Length - 1)]);
			}
			devicealert.SuiteInfoId=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 9; i++)
			{
				builder.Append(spanishcharacter[random.Next(spanishcharacter.Length - 1)]);
			}
			devicealert.VehicleId=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 9; i++)
			{
				builder.Append(spanishcharacter[random.Next(spanishcharacter.Length - 1)]);
			}
			devicealert.Cmd=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 9; i++)
			{
				builder.Append(spanishcharacter[random.Next(spanishcharacter.Length - 1)]);
			}
			devicealert.Longitude=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 9; i++)
			{
				builder.Append(spanishcharacter[random.Next(spanishcharacter.Length - 1)]);
			}
			devicealert.Latitude=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 9; i++)
			{
				builder.Append(spanishcharacter[random.Next(spanishcharacter.Length - 1)]);
			}
			devicealert.Speed=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 9; i++)
			{
				builder.Append(spanishcharacter[random.Next(spanishcharacter.Length - 1)]);
			}
			devicealert.Direction=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 0; i++)
			{
				builder.Append(spanishcharacter[random.Next(spanishcharacter.Length - 1)]);
			}
			devicealert.GpsValid=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 127; i++)
			{
				builder.Append(spanishcharacter[random.Next(spanishcharacter.Length - 1)]);
			}
			devicealert.TagValue=builder.ToString();
			builder.Length=0;

			SingleMessage<DeviceAlert> ret = service.AddDeviceAlert(devicealert);
			//Assert.IsFalse(ret.IsSuccess);
		}


		/// <summary>
		/// 测试SuiteInfoId英文超长
		/// </summary>
		[TestMethod]
		public void TestSuiteInfoIdStringForMaxLength_English()
		{
			DeviceAlert devicealert = new DeviceAlert();
			DeviceAlertRespository service = new DeviceAlertRespository();
			StringBuilder builder = new StringBuilder();
			Random random = new Random();
			for (int i = 0; i < 9; i++)
			{
				builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
			}
			devicealert.Id=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 9; i++)
			{
				builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
			}
			devicealert.MdvrCoreId=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 18; i++)
			{
				builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
			}
			devicealert.SuiteInfoId=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 9; i++)
			{
				builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
			}
			devicealert.VehicleId=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 9; i++)
			{
				builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
			}
			devicealert.Cmd=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 9; i++)
			{
				builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
			}
			devicealert.Longitude=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 9; i++)
			{
				builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
			}
			devicealert.Latitude=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 9; i++)
			{
				builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
			}
			devicealert.Speed=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 9; i++)
			{
				builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
			}
			devicealert.Direction=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 0; i++)
			{
				builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
			}
			devicealert.GpsValid=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 127; i++)
			{
				builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
			}
			devicealert.TagValue=builder.ToString();
			builder.Length=0;

			SingleMessage<DeviceAlert> ret = service.AddDeviceAlert(devicealert);
			//Assert.IsFalse(ret.IsSuccess);
		}


		/// <summary>
		/// 测试VehicleId中文超长
		/// </summary>
		[TestMethod]
		public void TestVehicleIdStringForOverLength_Chinese()
		{
			DeviceAlert devicealert = new DeviceAlert();
			DeviceAlertRespository service = new DeviceAlertRespository();
			StringBuilder builder = new StringBuilder();
			Random random = new Random();
			for (int i = 0; i < 9; i++)
			{
				builder.Append(chinesecharacter[random.Next(chinesecharacter.Length - 1)]);
			}
			devicealert.Id=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 9; i++)
			{
				builder.Append(chinesecharacter[random.Next(chinesecharacter.Length - 1)]);
			}
			devicealert.MdvrCoreId=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 17; i++)
			{
				builder.Append(chinesecharacter[random.Next(chinesecharacter.Length - 1)]);
			}
			devicealert.SuiteInfoId=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 10; i++)
			{
				builder.Append(chinesecharacter[random.Next(chinesecharacter.Length - 1)]);
			}
			devicealert.VehicleId=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 9; i++)
			{
				builder.Append(chinesecharacter[random.Next(chinesecharacter.Length - 1)]);
			}
			devicealert.Cmd=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 9; i++)
			{
				builder.Append(chinesecharacter[random.Next(chinesecharacter.Length - 1)]);
			}
			devicealert.Longitude=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 9; i++)
			{
				builder.Append(chinesecharacter[random.Next(chinesecharacter.Length - 1)]);
			}
			devicealert.Latitude=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 9; i++)
			{
				builder.Append(chinesecharacter[random.Next(chinesecharacter.Length - 1)]);
			}
			devicealert.Speed=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 9; i++)
			{
				builder.Append(chinesecharacter[random.Next(chinesecharacter.Length - 1)]);
			}
			devicealert.Direction=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 0; i++)
			{
				builder.Append(chinesecharacter[random.Next(chinesecharacter.Length - 1)]);
			}
			devicealert.GpsValid=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 127; i++)
			{
				builder.Append(chinesecharacter[random.Next(chinesecharacter.Length - 1)]);
			}
			devicealert.TagValue=builder.ToString();
			builder.Length=0;

			SingleMessage<DeviceAlert> ret = service.AddDeviceAlert(devicealert);
			//Assert.IsFalse(ret.IsSuccess);
		}


		/// <summary>
		/// 测试VehicleId西文超长
		/// </summary>
		[TestMethod]
		public void TestVehicleIdStringForMaxLength_Spanish()
		{
			DeviceAlert devicealert = new DeviceAlert();
			DeviceAlertRespository service = new DeviceAlertRespository();
			StringBuilder builder = new StringBuilder();
			Random random = new Random();
			for (int i = 0; i < 9; i++)
			{
				builder.Append(spanishcharacter[random.Next(spanishcharacter.Length - 1)]);
			}
			devicealert.Id=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 9; i++)
			{
				builder.Append(spanishcharacter[random.Next(spanishcharacter.Length - 1)]);
			}
			devicealert.MdvrCoreId=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 17; i++)
			{
				builder.Append(spanishcharacter[random.Next(spanishcharacter.Length - 1)]);
			}
			devicealert.SuiteInfoId=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 10; i++)
			{
				builder.Append(spanishcharacter[random.Next(spanishcharacter.Length - 1)]);
			}
			devicealert.VehicleId=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 9; i++)
			{
				builder.Append(spanishcharacter[random.Next(spanishcharacter.Length - 1)]);
			}
			devicealert.Cmd=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 9; i++)
			{
				builder.Append(spanishcharacter[random.Next(spanishcharacter.Length - 1)]);
			}
			devicealert.Longitude=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 9; i++)
			{
				builder.Append(spanishcharacter[random.Next(spanishcharacter.Length - 1)]);
			}
			devicealert.Latitude=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 9; i++)
			{
				builder.Append(spanishcharacter[random.Next(spanishcharacter.Length - 1)]);
			}
			devicealert.Speed=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 9; i++)
			{
				builder.Append(spanishcharacter[random.Next(spanishcharacter.Length - 1)]);
			}
			devicealert.Direction=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 0; i++)
			{
				builder.Append(spanishcharacter[random.Next(spanishcharacter.Length - 1)]);
			}
			devicealert.GpsValid=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 127; i++)
			{
				builder.Append(spanishcharacter[random.Next(spanishcharacter.Length - 1)]);
			}
			devicealert.TagValue=builder.ToString();
			builder.Length=0;

			SingleMessage<DeviceAlert> ret = service.AddDeviceAlert(devicealert);
			//Assert.IsFalse(ret.IsSuccess);
		}


		/// <summary>
		/// 测试VehicleId英文超长
		/// </summary>
		[TestMethod]
		public void TestVehicleIdStringForMaxLength_English()
		{
			DeviceAlert devicealert = new DeviceAlert();
			DeviceAlertRespository service = new DeviceAlertRespository();
			StringBuilder builder = new StringBuilder();
			Random random = new Random();
			for (int i = 0; i < 9; i++)
			{
				builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
			}
			devicealert.Id=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 9; i++)
			{
				builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
			}
			devicealert.MdvrCoreId=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 17; i++)
			{
				builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
			}
			devicealert.SuiteInfoId=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 10; i++)
			{
				builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
			}
			devicealert.VehicleId=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 9; i++)
			{
				builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
			}
			devicealert.Cmd=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 9; i++)
			{
				builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
			}
			devicealert.Longitude=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 9; i++)
			{
				builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
			}
			devicealert.Latitude=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 9; i++)
			{
				builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
			}
			devicealert.Speed=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 9; i++)
			{
				builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
			}
			devicealert.Direction=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 0; i++)
			{
				builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
			}
			devicealert.GpsValid=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 127; i++)
			{
				builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
			}
			devicealert.TagValue=builder.ToString();
			builder.Length=0;

			SingleMessage<DeviceAlert> ret = service.AddDeviceAlert(devicealert);
			//Assert.IsFalse(ret.IsSuccess);
		}


		/// <summary>
		/// 测试SuiteStatus中文超长
		/// </summary>
		[TestMethod]
		public void TestSuiteStatusStringForOverLength_Chinese()
		{
			DeviceAlert devicealert = new DeviceAlert();
			DeviceAlertRespository service = new DeviceAlertRespository();
			StringBuilder builder = new StringBuilder();
			Random random = new Random();
			for (int i = 0; i < 9; i++)
			{
				builder.Append(chinesecharacter[random.Next(chinesecharacter.Length - 1)]);
			}
			devicealert.Id=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 9; i++)
			{
				builder.Append(chinesecharacter[random.Next(chinesecharacter.Length - 1)]);
			}
			devicealert.MdvrCoreId=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 17; i++)
			{
				builder.Append(chinesecharacter[random.Next(chinesecharacter.Length - 1)]);
			}
			devicealert.SuiteInfoId=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 9; i++)
			{
				builder.Append(chinesecharacter[random.Next(chinesecharacter.Length - 1)]);
			}
			devicealert.VehicleId=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 9; i++)
			{
				builder.Append(chinesecharacter[random.Next(chinesecharacter.Length - 1)]);
			}
			devicealert.Cmd=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 9; i++)
			{
				builder.Append(chinesecharacter[random.Next(chinesecharacter.Length - 1)]);
			}
			devicealert.Longitude=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 9; i++)
			{
				builder.Append(chinesecharacter[random.Next(chinesecharacter.Length - 1)]);
			}
			devicealert.Latitude=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 9; i++)
			{
				builder.Append(chinesecharacter[random.Next(chinesecharacter.Length - 1)]);
			}
			devicealert.Speed=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 9; i++)
			{
				builder.Append(chinesecharacter[random.Next(chinesecharacter.Length - 1)]);
			}
			devicealert.Direction=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 0; i++)
			{
				builder.Append(chinesecharacter[random.Next(chinesecharacter.Length - 1)]);
			}
			devicealert.GpsValid=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 127; i++)
			{
				builder.Append(chinesecharacter[random.Next(chinesecharacter.Length - 1)]);
			}
			devicealert.TagValue=builder.ToString();
			builder.Length=0;

			SingleMessage<DeviceAlert> ret = service.AddDeviceAlert(devicealert);
			//Assert.IsFalse(ret.IsSuccess);
		}


		/// <summary>
		/// 测试SuiteStatus西文超长
		/// </summary>
		[TestMethod]
		public void TestSuiteStatusStringForMaxLength_Spanish()
		{
			DeviceAlert devicealert = new DeviceAlert();
			DeviceAlertRespository service = new DeviceAlertRespository();
			StringBuilder builder = new StringBuilder();
			Random random = new Random();
			for (int i = 0; i < 9; i++)
			{
				builder.Append(spanishcharacter[random.Next(spanishcharacter.Length - 1)]);
			}
			devicealert.Id=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 9; i++)
			{
				builder.Append(spanishcharacter[random.Next(spanishcharacter.Length - 1)]);
			}
			devicealert.MdvrCoreId=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 17; i++)
			{
				builder.Append(spanishcharacter[random.Next(spanishcharacter.Length - 1)]);
			}
			devicealert.SuiteInfoId=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 9; i++)
			{
				builder.Append(spanishcharacter[random.Next(spanishcharacter.Length - 1)]);
			}
			devicealert.VehicleId=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 9; i++)
			{
				builder.Append(spanishcharacter[random.Next(spanishcharacter.Length - 1)]);
			}
			devicealert.Cmd=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 9; i++)
			{
				builder.Append(spanishcharacter[random.Next(spanishcharacter.Length - 1)]);
			}
			devicealert.Longitude=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 9; i++)
			{
				builder.Append(spanishcharacter[random.Next(spanishcharacter.Length - 1)]);
			}
			devicealert.Latitude=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 9; i++)
			{
				builder.Append(spanishcharacter[random.Next(spanishcharacter.Length - 1)]);
			}
			devicealert.Speed=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 9; i++)
			{
				builder.Append(spanishcharacter[random.Next(spanishcharacter.Length - 1)]);
			}
			devicealert.Direction=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 0; i++)
			{
				builder.Append(spanishcharacter[random.Next(spanishcharacter.Length - 1)]);
			}
			devicealert.GpsValid=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 127; i++)
			{
				builder.Append(spanishcharacter[random.Next(spanishcharacter.Length - 1)]);
			}
			devicealert.TagValue=builder.ToString();
			builder.Length=0;

			SingleMessage<DeviceAlert> ret = service.AddDeviceAlert(devicealert);
			//Assert.IsFalse(ret.IsSuccess);
		}


		/// <summary>
		/// 测试SuiteStatus英文超长
		/// </summary>
		[TestMethod]
		public void TestSuiteStatusStringForMaxLength_English()
		{
			DeviceAlert devicealert = new DeviceAlert();
			DeviceAlertRespository service = new DeviceAlertRespository();
			StringBuilder builder = new StringBuilder();
			Random random = new Random();
			for (int i = 0; i < 9; i++)
			{
				builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
			}
			devicealert.Id=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 9; i++)
			{
				builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
			}
			devicealert.MdvrCoreId=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 17; i++)
			{
				builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
			}
			devicealert.SuiteInfoId=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 9; i++)
			{
				builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
			}
			devicealert.VehicleId=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 9; i++)
			{
				builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
			}
			devicealert.Cmd=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 9; i++)
			{
				builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
			}
			devicealert.Longitude=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 9; i++)
			{
				builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
			}
			devicealert.Latitude=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 9; i++)
			{
				builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
			}
			devicealert.Speed=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 9; i++)
			{
				builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
			}
			devicealert.Direction=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 0; i++)
			{
				builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
			}
			devicealert.GpsValid=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 127; i++)
			{
				builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
			}
			devicealert.TagValue=builder.ToString();
			builder.Length=0;

			SingleMessage<DeviceAlert> ret = service.AddDeviceAlert(devicealert);
			//Assert.IsFalse(ret.IsSuccess);
		}


		/// <summary>
		/// 测试AlertType中文超长
		/// </summary>
		[TestMethod]
		public void TestAlertTypeStringForOverLength_Chinese()
		{
			DeviceAlert devicealert = new DeviceAlert();
			DeviceAlertRespository service = new DeviceAlertRespository();
			StringBuilder builder = new StringBuilder();
			Random random = new Random();
			for (int i = 0; i < 9; i++)
			{
				builder.Append(chinesecharacter[random.Next(chinesecharacter.Length - 1)]);
			}
			devicealert.Id=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 9; i++)
			{
				builder.Append(chinesecharacter[random.Next(chinesecharacter.Length - 1)]);
			}
			devicealert.MdvrCoreId=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 17; i++)
			{
				builder.Append(chinesecharacter[random.Next(chinesecharacter.Length - 1)]);
			}
			devicealert.SuiteInfoId=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 9; i++)
			{
				builder.Append(chinesecharacter[random.Next(chinesecharacter.Length - 1)]);
			}
			devicealert.VehicleId=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 9; i++)
			{
				builder.Append(chinesecharacter[random.Next(chinesecharacter.Length - 1)]);
			}
			devicealert.Cmd=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 9; i++)
			{
				builder.Append(chinesecharacter[random.Next(chinesecharacter.Length - 1)]);
			}
			devicealert.Longitude=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 9; i++)
			{
				builder.Append(chinesecharacter[random.Next(chinesecharacter.Length - 1)]);
			}
			devicealert.Latitude=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 9; i++)
			{
				builder.Append(chinesecharacter[random.Next(chinesecharacter.Length - 1)]);
			}
			devicealert.Speed=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 9; i++)
			{
				builder.Append(chinesecharacter[random.Next(chinesecharacter.Length - 1)]);
			}
			devicealert.Direction=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 0; i++)
			{
				builder.Append(chinesecharacter[random.Next(chinesecharacter.Length - 1)]);
			}
			devicealert.GpsValid=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 127; i++)
			{
				builder.Append(chinesecharacter[random.Next(chinesecharacter.Length - 1)]);
			}
			devicealert.TagValue=builder.ToString();
			builder.Length=0;

			SingleMessage<DeviceAlert> ret = service.AddDeviceAlert(devicealert);
			//Assert.IsFalse(ret.IsSuccess);
		}


		/// <summary>
		/// 测试AlertType西文超长
		/// </summary>
		[TestMethod]
		public void TestAlertTypeStringForMaxLength_Spanish()
		{
			DeviceAlert devicealert = new DeviceAlert();
			DeviceAlertRespository service = new DeviceAlertRespository();
			StringBuilder builder = new StringBuilder();
			Random random = new Random();
			for (int i = 0; i < 9; i++)
			{
				builder.Append(spanishcharacter[random.Next(spanishcharacter.Length - 1)]);
			}
			devicealert.Id=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 9; i++)
			{
				builder.Append(spanishcharacter[random.Next(spanishcharacter.Length - 1)]);
			}
			devicealert.MdvrCoreId=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 17; i++)
			{
				builder.Append(spanishcharacter[random.Next(spanishcharacter.Length - 1)]);
			}
			devicealert.SuiteInfoId=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 9; i++)
			{
				builder.Append(spanishcharacter[random.Next(spanishcharacter.Length - 1)]);
			}
			devicealert.VehicleId=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 9; i++)
			{
				builder.Append(spanishcharacter[random.Next(spanishcharacter.Length - 1)]);
			}
			devicealert.Cmd=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 9; i++)
			{
				builder.Append(spanishcharacter[random.Next(spanishcharacter.Length - 1)]);
			}
			devicealert.Longitude=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 9; i++)
			{
				builder.Append(spanishcharacter[random.Next(spanishcharacter.Length - 1)]);
			}
			devicealert.Latitude=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 9; i++)
			{
				builder.Append(spanishcharacter[random.Next(spanishcharacter.Length - 1)]);
			}
			devicealert.Speed=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 9; i++)
			{
				builder.Append(spanishcharacter[random.Next(spanishcharacter.Length - 1)]);
			}
			devicealert.Direction=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 0; i++)
			{
				builder.Append(spanishcharacter[random.Next(spanishcharacter.Length - 1)]);
			}
			devicealert.GpsValid=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 127; i++)
			{
				builder.Append(spanishcharacter[random.Next(spanishcharacter.Length - 1)]);
			}
			devicealert.TagValue=builder.ToString();
			builder.Length=0;

			SingleMessage<DeviceAlert> ret = service.AddDeviceAlert(devicealert);
			//Assert.IsFalse(ret.IsSuccess);
		}


		/// <summary>
		/// 测试AlertType英文超长
		/// </summary>
		[TestMethod]
		public void TestAlertTypeStringForMaxLength_English()
		{
			DeviceAlert devicealert = new DeviceAlert();
			DeviceAlertRespository service = new DeviceAlertRespository();
			StringBuilder builder = new StringBuilder();
			Random random = new Random();
			for (int i = 0; i < 9; i++)
			{
				builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
			}
			devicealert.Id=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 9; i++)
			{
				builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
			}
			devicealert.MdvrCoreId=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 17; i++)
			{
				builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
			}
			devicealert.SuiteInfoId=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 9; i++)
			{
				builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
			}
			devicealert.VehicleId=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 9; i++)
			{
				builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
			}
			devicealert.Cmd=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 9; i++)
			{
				builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
			}
			devicealert.Longitude=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 9; i++)
			{
				builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
			}
			devicealert.Latitude=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 9; i++)
			{
				builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
			}
			devicealert.Speed=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 9; i++)
			{
				builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
			}
			devicealert.Direction=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 0; i++)
			{
				builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
			}
			devicealert.GpsValid=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 127; i++)
			{
				builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
			}
			devicealert.TagValue=builder.ToString();
			builder.Length=0;

			SingleMessage<DeviceAlert> ret = service.AddDeviceAlert(devicealert);
			//Assert.IsFalse(ret.IsSuccess);
		}


		/// <summary>
		/// 测试AlertTypeName中文超长
		/// </summary>
		[TestMethod]
		public void TestAlertTypeNameStringForOverLength_Chinese()
		{
			DeviceAlert devicealert = new DeviceAlert();
			DeviceAlertRespository service = new DeviceAlertRespository();
			StringBuilder builder = new StringBuilder();
			Random random = new Random();
			for (int i = 0; i < 9; i++)
			{
				builder.Append(chinesecharacter[random.Next(chinesecharacter.Length - 1)]);
			}
			devicealert.Id=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 9; i++)
			{
				builder.Append(chinesecharacter[random.Next(chinesecharacter.Length - 1)]);
			}
			devicealert.MdvrCoreId=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 17; i++)
			{
				builder.Append(chinesecharacter[random.Next(chinesecharacter.Length - 1)]);
			}
			devicealert.SuiteInfoId=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 9; i++)
			{
				builder.Append(chinesecharacter[random.Next(chinesecharacter.Length - 1)]);
			}
			devicealert.VehicleId=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 9; i++)
			{
				builder.Append(chinesecharacter[random.Next(chinesecharacter.Length - 1)]);
			}
			devicealert.Cmd=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 9; i++)
			{
				builder.Append(chinesecharacter[random.Next(chinesecharacter.Length - 1)]);
			}
			devicealert.Longitude=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 9; i++)
			{
				builder.Append(chinesecharacter[random.Next(chinesecharacter.Length - 1)]);
			}
			devicealert.Latitude=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 9; i++)
			{
				builder.Append(chinesecharacter[random.Next(chinesecharacter.Length - 1)]);
			}
			devicealert.Speed=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 9; i++)
			{
				builder.Append(chinesecharacter[random.Next(chinesecharacter.Length - 1)]);
			}
			devicealert.Direction=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 0; i++)
			{
				builder.Append(chinesecharacter[random.Next(chinesecharacter.Length - 1)]);
			}
			devicealert.GpsValid=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 127; i++)
			{
				builder.Append(chinesecharacter[random.Next(chinesecharacter.Length - 1)]);
			}
			devicealert.TagValue=builder.ToString();
			builder.Length=0;

			SingleMessage<DeviceAlert> ret = service.AddDeviceAlert(devicealert);
			//Assert.IsFalse(ret.IsSuccess);
		}


		/// <summary>
		/// 测试AlertTypeName西文超长
		/// </summary>
		[TestMethod]
		public void TestAlertTypeNameStringForMaxLength_Spanish()
		{
			DeviceAlert devicealert = new DeviceAlert();
			DeviceAlertRespository service = new DeviceAlertRespository();
			StringBuilder builder = new StringBuilder();
			Random random = new Random();
			for (int i = 0; i < 9; i++)
			{
				builder.Append(spanishcharacter[random.Next(spanishcharacter.Length - 1)]);
			}
			devicealert.Id=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 9; i++)
			{
				builder.Append(spanishcharacter[random.Next(spanishcharacter.Length - 1)]);
			}
			devicealert.MdvrCoreId=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 17; i++)
			{
				builder.Append(spanishcharacter[random.Next(spanishcharacter.Length - 1)]);
			}
			devicealert.SuiteInfoId=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 9; i++)
			{
				builder.Append(spanishcharacter[random.Next(spanishcharacter.Length - 1)]);
			}
			devicealert.VehicleId=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 9; i++)
			{
				builder.Append(spanishcharacter[random.Next(spanishcharacter.Length - 1)]);
			}
			devicealert.Cmd=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 9; i++)
			{
				builder.Append(spanishcharacter[random.Next(spanishcharacter.Length - 1)]);
			}
			devicealert.Longitude=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 9; i++)
			{
				builder.Append(spanishcharacter[random.Next(spanishcharacter.Length - 1)]);
			}
			devicealert.Latitude=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 9; i++)
			{
				builder.Append(spanishcharacter[random.Next(spanishcharacter.Length - 1)]);
			}
			devicealert.Speed=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 9; i++)
			{
				builder.Append(spanishcharacter[random.Next(spanishcharacter.Length - 1)]);
			}
			devicealert.Direction=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 0; i++)
			{
				builder.Append(spanishcharacter[random.Next(spanishcharacter.Length - 1)]);
			}
			devicealert.GpsValid=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 127; i++)
			{
				builder.Append(spanishcharacter[random.Next(spanishcharacter.Length - 1)]);
			}
			devicealert.TagValue=builder.ToString();
			builder.Length=0;

			SingleMessage<DeviceAlert> ret = service.AddDeviceAlert(devicealert);
			//Assert.IsFalse(ret.IsSuccess);
		}


		/// <summary>
		/// 测试AlertTypeName英文超长
		/// </summary>
		[TestMethod]
		public void TestAlertTypeNameStringForMaxLength_English()
		{
			DeviceAlert devicealert = new DeviceAlert();
			DeviceAlertRespository service = new DeviceAlertRespository();
			StringBuilder builder = new StringBuilder();
			Random random = new Random();
			for (int i = 0; i < 9; i++)
			{
				builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
			}
			devicealert.Id=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 9; i++)
			{
				builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
			}
			devicealert.MdvrCoreId=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 17; i++)
			{
				builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
			}
			devicealert.SuiteInfoId=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 9; i++)
			{
				builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
			}
			devicealert.VehicleId=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 9; i++)
			{
				builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
			}
			devicealert.Cmd=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 9; i++)
			{
				builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
			}
			devicealert.Longitude=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 9; i++)
			{
				builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
			}
			devicealert.Latitude=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 9; i++)
			{
				builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
			}
			devicealert.Speed=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 9; i++)
			{
				builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
			}
			devicealert.Direction=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 0; i++)
			{
				builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
			}
			devicealert.GpsValid=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 127; i++)
			{
				builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
			}
			devicealert.TagValue=builder.ToString();
			builder.Length=0;

			SingleMessage<DeviceAlert> ret = service.AddDeviceAlert(devicealert);
			//Assert.IsFalse(ret.IsSuccess);
		}


		/// <summary>
		/// 测试AlertTime中文超长
		/// </summary>
		[TestMethod]
		public void TestAlertTimeStringForOverLength_Chinese()
		{
			DeviceAlert devicealert = new DeviceAlert();
			DeviceAlertRespository service = new DeviceAlertRespository();
			StringBuilder builder = new StringBuilder();
			Random random = new Random();
			for (int i = 0; i < 9; i++)
			{
				builder.Append(chinesecharacter[random.Next(chinesecharacter.Length - 1)]);
			}
			devicealert.Id=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 9; i++)
			{
				builder.Append(chinesecharacter[random.Next(chinesecharacter.Length - 1)]);
			}
			devicealert.MdvrCoreId=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 17; i++)
			{
				builder.Append(chinesecharacter[random.Next(chinesecharacter.Length - 1)]);
			}
			devicealert.SuiteInfoId=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 9; i++)
			{
				builder.Append(chinesecharacter[random.Next(chinesecharacter.Length - 1)]);
			}
			devicealert.VehicleId=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 9; i++)
			{
				builder.Append(chinesecharacter[random.Next(chinesecharacter.Length - 1)]);
			}
			devicealert.Cmd=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 9; i++)
			{
				builder.Append(chinesecharacter[random.Next(chinesecharacter.Length - 1)]);
			}
			devicealert.Longitude=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 9; i++)
			{
				builder.Append(chinesecharacter[random.Next(chinesecharacter.Length - 1)]);
			}
			devicealert.Latitude=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 9; i++)
			{
				builder.Append(chinesecharacter[random.Next(chinesecharacter.Length - 1)]);
			}
			devicealert.Speed=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 9; i++)
			{
				builder.Append(chinesecharacter[random.Next(chinesecharacter.Length - 1)]);
			}
			devicealert.Direction=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 0; i++)
			{
				builder.Append(chinesecharacter[random.Next(chinesecharacter.Length - 1)]);
			}
			devicealert.GpsValid=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 127; i++)
			{
				builder.Append(chinesecharacter[random.Next(chinesecharacter.Length - 1)]);
			}
			devicealert.TagValue=builder.ToString();
			builder.Length=0;

			SingleMessage<DeviceAlert> ret = service.AddDeviceAlert(devicealert);
			//Assert.IsFalse(ret.IsSuccess);
		}


		/// <summary>
		/// 测试AlertTime西文超长
		/// </summary>
		[TestMethod]
		public void TestAlertTimeStringForMaxLength_Spanish()
		{
			DeviceAlert devicealert = new DeviceAlert();
			DeviceAlertRespository service = new DeviceAlertRespository();
			StringBuilder builder = new StringBuilder();
			Random random = new Random();
			for (int i = 0; i < 9; i++)
			{
				builder.Append(spanishcharacter[random.Next(spanishcharacter.Length - 1)]);
			}
			devicealert.Id=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 9; i++)
			{
				builder.Append(spanishcharacter[random.Next(spanishcharacter.Length - 1)]);
			}
			devicealert.MdvrCoreId=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 17; i++)
			{
				builder.Append(spanishcharacter[random.Next(spanishcharacter.Length - 1)]);
			}
			devicealert.SuiteInfoId=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 9; i++)
			{
				builder.Append(spanishcharacter[random.Next(spanishcharacter.Length - 1)]);
			}
			devicealert.VehicleId=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 9; i++)
			{
				builder.Append(spanishcharacter[random.Next(spanishcharacter.Length - 1)]);
			}
			devicealert.Cmd=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 9; i++)
			{
				builder.Append(spanishcharacter[random.Next(spanishcharacter.Length - 1)]);
			}
			devicealert.Longitude=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 9; i++)
			{
				builder.Append(spanishcharacter[random.Next(spanishcharacter.Length - 1)]);
			}
			devicealert.Latitude=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 9; i++)
			{
				builder.Append(spanishcharacter[random.Next(spanishcharacter.Length - 1)]);
			}
			devicealert.Speed=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 9; i++)
			{
				builder.Append(spanishcharacter[random.Next(spanishcharacter.Length - 1)]);
			}
			devicealert.Direction=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 0; i++)
			{
				builder.Append(spanishcharacter[random.Next(spanishcharacter.Length - 1)]);
			}
			devicealert.GpsValid=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 127; i++)
			{
				builder.Append(spanishcharacter[random.Next(spanishcharacter.Length - 1)]);
			}
			devicealert.TagValue=builder.ToString();
			builder.Length=0;

			SingleMessage<DeviceAlert> ret = service.AddDeviceAlert(devicealert);
			//Assert.IsFalse(ret.IsSuccess);
		}


		/// <summary>
		/// 测试AlertTime英文超长
		/// </summary>
		[TestMethod]
		public void TestAlertTimeStringForMaxLength_English()
		{
			DeviceAlert devicealert = new DeviceAlert();
			DeviceAlertRespository service = new DeviceAlertRespository();
			StringBuilder builder = new StringBuilder();
			Random random = new Random();
			for (int i = 0; i < 9; i++)
			{
				builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
			}
			devicealert.Id=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 9; i++)
			{
				builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
			}
			devicealert.MdvrCoreId=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 17; i++)
			{
				builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
			}
			devicealert.SuiteInfoId=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 9; i++)
			{
				builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
			}
			devicealert.VehicleId=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 9; i++)
			{
				builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
			}
			devicealert.Cmd=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 9; i++)
			{
				builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
			}
			devicealert.Longitude=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 9; i++)
			{
				builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
			}
			devicealert.Latitude=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 9; i++)
			{
				builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
			}
			devicealert.Speed=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 9; i++)
			{
				builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
			}
			devicealert.Direction=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 0; i++)
			{
				builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
			}
			devicealert.GpsValid=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 127; i++)
			{
				builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
			}
			devicealert.TagValue=builder.ToString();
			builder.Length=0;

			SingleMessage<DeviceAlert> ret = service.AddDeviceAlert(devicealert);
			//Assert.IsFalse(ret.IsSuccess);
		}


		/// <summary>
		/// 测试Cmd中文超长
		/// </summary>
		[TestMethod]
		public void TestCmdStringForOverLength_Chinese()
		{
			DeviceAlert devicealert = new DeviceAlert();
			DeviceAlertRespository service = new DeviceAlertRespository();
			StringBuilder builder = new StringBuilder();
			Random random = new Random();
			for (int i = 0; i < 9; i++)
			{
				builder.Append(chinesecharacter[random.Next(chinesecharacter.Length - 1)]);
			}
			devicealert.Id=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 9; i++)
			{
				builder.Append(chinesecharacter[random.Next(chinesecharacter.Length - 1)]);
			}
			devicealert.MdvrCoreId=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 17; i++)
			{
				builder.Append(chinesecharacter[random.Next(chinesecharacter.Length - 1)]);
			}
			devicealert.SuiteInfoId=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 9; i++)
			{
				builder.Append(chinesecharacter[random.Next(chinesecharacter.Length - 1)]);
			}
			devicealert.VehicleId=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 10; i++)
			{
				builder.Append(chinesecharacter[random.Next(chinesecharacter.Length - 1)]);
			}
			devicealert.Cmd=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 9; i++)
			{
				builder.Append(chinesecharacter[random.Next(chinesecharacter.Length - 1)]);
			}
			devicealert.Longitude=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 9; i++)
			{
				builder.Append(chinesecharacter[random.Next(chinesecharacter.Length - 1)]);
			}
			devicealert.Latitude=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 9; i++)
			{
				builder.Append(chinesecharacter[random.Next(chinesecharacter.Length - 1)]);
			}
			devicealert.Speed=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 9; i++)
			{
				builder.Append(chinesecharacter[random.Next(chinesecharacter.Length - 1)]);
			}
			devicealert.Direction=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 0; i++)
			{
				builder.Append(chinesecharacter[random.Next(chinesecharacter.Length - 1)]);
			}
			devicealert.GpsValid=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 127; i++)
			{
				builder.Append(chinesecharacter[random.Next(chinesecharacter.Length - 1)]);
			}
			devicealert.TagValue=builder.ToString();
			builder.Length=0;

			SingleMessage<DeviceAlert> ret = service.AddDeviceAlert(devicealert);
			//Assert.IsFalse(ret.IsSuccess);
		}


		/// <summary>
		/// 测试Cmd西文超长
		/// </summary>
		[TestMethod]
		public void TestCmdStringForMaxLength_Spanish()
		{
			DeviceAlert devicealert = new DeviceAlert();
			DeviceAlertRespository service = new DeviceAlertRespository();
			StringBuilder builder = new StringBuilder();
			Random random = new Random();
			for (int i = 0; i < 9; i++)
			{
				builder.Append(spanishcharacter[random.Next(spanishcharacter.Length - 1)]);
			}
			devicealert.Id=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 9; i++)
			{
				builder.Append(spanishcharacter[random.Next(spanishcharacter.Length - 1)]);
			}
			devicealert.MdvrCoreId=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 17; i++)
			{
				builder.Append(spanishcharacter[random.Next(spanishcharacter.Length - 1)]);
			}
			devicealert.SuiteInfoId=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 9; i++)
			{
				builder.Append(spanishcharacter[random.Next(spanishcharacter.Length - 1)]);
			}
			devicealert.VehicleId=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 10; i++)
			{
				builder.Append(spanishcharacter[random.Next(spanishcharacter.Length - 1)]);
			}
			devicealert.Cmd=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 9; i++)
			{
				builder.Append(spanishcharacter[random.Next(spanishcharacter.Length - 1)]);
			}
			devicealert.Longitude=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 9; i++)
			{
				builder.Append(spanishcharacter[random.Next(spanishcharacter.Length - 1)]);
			}
			devicealert.Latitude=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 9; i++)
			{
				builder.Append(spanishcharacter[random.Next(spanishcharacter.Length - 1)]);
			}
			devicealert.Speed=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 9; i++)
			{
				builder.Append(spanishcharacter[random.Next(spanishcharacter.Length - 1)]);
			}
			devicealert.Direction=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 0; i++)
			{
				builder.Append(spanishcharacter[random.Next(spanishcharacter.Length - 1)]);
			}
			devicealert.GpsValid=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 127; i++)
			{
				builder.Append(spanishcharacter[random.Next(spanishcharacter.Length - 1)]);
			}
			devicealert.TagValue=builder.ToString();
			builder.Length=0;

			SingleMessage<DeviceAlert> ret = service.AddDeviceAlert(devicealert);
			//Assert.IsFalse(ret.IsSuccess);
		}


		/// <summary>
		/// 测试Cmd英文超长
		/// </summary>
		[TestMethod]
		public void TestCmdStringForMaxLength_English()
		{
			DeviceAlert devicealert = new DeviceAlert();
			DeviceAlertRespository service = new DeviceAlertRespository();
			StringBuilder builder = new StringBuilder();
			Random random = new Random();
			for (int i = 0; i < 9; i++)
			{
				builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
			}
			devicealert.Id=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 9; i++)
			{
				builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
			}
			devicealert.MdvrCoreId=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 17; i++)
			{
				builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
			}
			devicealert.SuiteInfoId=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 9; i++)
			{
				builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
			}
			devicealert.VehicleId=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 10; i++)
			{
				builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
			}
			devicealert.Cmd=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 9; i++)
			{
				builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
			}
			devicealert.Longitude=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 9; i++)
			{
				builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
			}
			devicealert.Latitude=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 9; i++)
			{
				builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
			}
			devicealert.Speed=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 9; i++)
			{
				builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
			}
			devicealert.Direction=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 0; i++)
			{
				builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
			}
			devicealert.GpsValid=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 127; i++)
			{
				builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
			}
			devicealert.TagValue=builder.ToString();
			builder.Length=0;

			SingleMessage<DeviceAlert> ret = service.AddDeviceAlert(devicealert);
			//Assert.IsFalse(ret.IsSuccess);
		}


		/// <summary>
		/// 测试Longitude中文超长
		/// </summary>
		[TestMethod]
		public void TestLongitudeStringForOverLength_Chinese()
		{
			DeviceAlert devicealert = new DeviceAlert();
			DeviceAlertRespository service = new DeviceAlertRespository();
			StringBuilder builder = new StringBuilder();
			Random random = new Random();
			for (int i = 0; i < 9; i++)
			{
				builder.Append(chinesecharacter[random.Next(chinesecharacter.Length - 1)]);
			}
			devicealert.Id=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 9; i++)
			{
				builder.Append(chinesecharacter[random.Next(chinesecharacter.Length - 1)]);
			}
			devicealert.MdvrCoreId=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 17; i++)
			{
				builder.Append(chinesecharacter[random.Next(chinesecharacter.Length - 1)]);
			}
			devicealert.SuiteInfoId=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 9; i++)
			{
				builder.Append(chinesecharacter[random.Next(chinesecharacter.Length - 1)]);
			}
			devicealert.VehicleId=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 9; i++)
			{
				builder.Append(chinesecharacter[random.Next(chinesecharacter.Length - 1)]);
			}
			devicealert.Cmd=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 10; i++)
			{
				builder.Append(chinesecharacter[random.Next(chinesecharacter.Length - 1)]);
			}
			devicealert.Longitude=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 9; i++)
			{
				builder.Append(chinesecharacter[random.Next(chinesecharacter.Length - 1)]);
			}
			devicealert.Latitude=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 9; i++)
			{
				builder.Append(chinesecharacter[random.Next(chinesecharacter.Length - 1)]);
			}
			devicealert.Speed=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 9; i++)
			{
				builder.Append(chinesecharacter[random.Next(chinesecharacter.Length - 1)]);
			}
			devicealert.Direction=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 0; i++)
			{
				builder.Append(chinesecharacter[random.Next(chinesecharacter.Length - 1)]);
			}
			devicealert.GpsValid=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 127; i++)
			{
				builder.Append(chinesecharacter[random.Next(chinesecharacter.Length - 1)]);
			}
			devicealert.TagValue=builder.ToString();
			builder.Length=0;

			SingleMessage<DeviceAlert> ret = service.AddDeviceAlert(devicealert);
			//Assert.IsFalse(ret.IsSuccess);
		}


		/// <summary>
		/// 测试Longitude西文超长
		/// </summary>
		[TestMethod]
		public void TestLongitudeStringForMaxLength_Spanish()
		{
			DeviceAlert devicealert = new DeviceAlert();
			DeviceAlertRespository service = new DeviceAlertRespository();
			StringBuilder builder = new StringBuilder();
			Random random = new Random();
			for (int i = 0; i < 9; i++)
			{
				builder.Append(spanishcharacter[random.Next(spanishcharacter.Length - 1)]);
			}
			devicealert.Id=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 9; i++)
			{
				builder.Append(spanishcharacter[random.Next(spanishcharacter.Length - 1)]);
			}
			devicealert.MdvrCoreId=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 17; i++)
			{
				builder.Append(spanishcharacter[random.Next(spanishcharacter.Length - 1)]);
			}
			devicealert.SuiteInfoId=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 9; i++)
			{
				builder.Append(spanishcharacter[random.Next(spanishcharacter.Length - 1)]);
			}
			devicealert.VehicleId=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 9; i++)
			{
				builder.Append(spanishcharacter[random.Next(spanishcharacter.Length - 1)]);
			}
			devicealert.Cmd=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 10; i++)
			{
				builder.Append(spanishcharacter[random.Next(spanishcharacter.Length - 1)]);
			}
			devicealert.Longitude=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 9; i++)
			{
				builder.Append(spanishcharacter[random.Next(spanishcharacter.Length - 1)]);
			}
			devicealert.Latitude=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 9; i++)
			{
				builder.Append(spanishcharacter[random.Next(spanishcharacter.Length - 1)]);
			}
			devicealert.Speed=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 9; i++)
			{
				builder.Append(spanishcharacter[random.Next(spanishcharacter.Length - 1)]);
			}
			devicealert.Direction=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 0; i++)
			{
				builder.Append(spanishcharacter[random.Next(spanishcharacter.Length - 1)]);
			}
			devicealert.GpsValid=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 127; i++)
			{
				builder.Append(spanishcharacter[random.Next(spanishcharacter.Length - 1)]);
			}
			devicealert.TagValue=builder.ToString();
			builder.Length=0;

			SingleMessage<DeviceAlert> ret = service.AddDeviceAlert(devicealert);
			//Assert.IsFalse(ret.IsSuccess);
		}


		/// <summary>
		/// 测试Longitude英文超长
		/// </summary>
		[TestMethod]
		public void TestLongitudeStringForMaxLength_English()
		{
			DeviceAlert devicealert = new DeviceAlert();
			DeviceAlertRespository service = new DeviceAlertRespository();
			StringBuilder builder = new StringBuilder();
			Random random = new Random();
			for (int i = 0; i < 9; i++)
			{
				builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
			}
			devicealert.Id=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 9; i++)
			{
				builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
			}
			devicealert.MdvrCoreId=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 17; i++)
			{
				builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
			}
			devicealert.SuiteInfoId=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 9; i++)
			{
				builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
			}
			devicealert.VehicleId=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 9; i++)
			{
				builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
			}
			devicealert.Cmd=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 10; i++)
			{
				builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
			}
			devicealert.Longitude=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 9; i++)
			{
				builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
			}
			devicealert.Latitude=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 9; i++)
			{
				builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
			}
			devicealert.Speed=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 9; i++)
			{
				builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
			}
			devicealert.Direction=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 0; i++)
			{
				builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
			}
			devicealert.GpsValid=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 127; i++)
			{
				builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
			}
			devicealert.TagValue=builder.ToString();
			builder.Length=0;

			SingleMessage<DeviceAlert> ret = service.AddDeviceAlert(devicealert);
			//Assert.IsFalse(ret.IsSuccess);
		}


		/// <summary>
		/// 测试Latitude中文超长
		/// </summary>
		[TestMethod]
		public void TestLatitudeStringForOverLength_Chinese()
		{
			DeviceAlert devicealert = new DeviceAlert();
			DeviceAlertRespository service = new DeviceAlertRespository();
			StringBuilder builder = new StringBuilder();
			Random random = new Random();
			for (int i = 0; i < 9; i++)
			{
				builder.Append(chinesecharacter[random.Next(chinesecharacter.Length - 1)]);
			}
			devicealert.Id=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 9; i++)
			{
				builder.Append(chinesecharacter[random.Next(chinesecharacter.Length - 1)]);
			}
			devicealert.MdvrCoreId=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 17; i++)
			{
				builder.Append(chinesecharacter[random.Next(chinesecharacter.Length - 1)]);
			}
			devicealert.SuiteInfoId=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 9; i++)
			{
				builder.Append(chinesecharacter[random.Next(chinesecharacter.Length - 1)]);
			}
			devicealert.VehicleId=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 9; i++)
			{
				builder.Append(chinesecharacter[random.Next(chinesecharacter.Length - 1)]);
			}
			devicealert.Cmd=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 9; i++)
			{
				builder.Append(chinesecharacter[random.Next(chinesecharacter.Length - 1)]);
			}
			devicealert.Longitude=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 10; i++)
			{
				builder.Append(chinesecharacter[random.Next(chinesecharacter.Length - 1)]);
			}
			devicealert.Latitude=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 9; i++)
			{
				builder.Append(chinesecharacter[random.Next(chinesecharacter.Length - 1)]);
			}
			devicealert.Speed=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 9; i++)
			{
				builder.Append(chinesecharacter[random.Next(chinesecharacter.Length - 1)]);
			}
			devicealert.Direction=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 0; i++)
			{
				builder.Append(chinesecharacter[random.Next(chinesecharacter.Length - 1)]);
			}
			devicealert.GpsValid=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 127; i++)
			{
				builder.Append(chinesecharacter[random.Next(chinesecharacter.Length - 1)]);
			}
			devicealert.TagValue=builder.ToString();
			builder.Length=0;

			SingleMessage<DeviceAlert> ret = service.AddDeviceAlert(devicealert);
			//Assert.IsFalse(ret.IsSuccess);
		}


		/// <summary>
		/// 测试Latitude西文超长
		/// </summary>
		[TestMethod]
		public void TestLatitudeStringForMaxLength_Spanish()
		{
			DeviceAlert devicealert = new DeviceAlert();
			DeviceAlertRespository service = new DeviceAlertRespository();
			StringBuilder builder = new StringBuilder();
			Random random = new Random();
			for (int i = 0; i < 9; i++)
			{
				builder.Append(spanishcharacter[random.Next(spanishcharacter.Length - 1)]);
			}
			devicealert.Id=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 9; i++)
			{
				builder.Append(spanishcharacter[random.Next(spanishcharacter.Length - 1)]);
			}
			devicealert.MdvrCoreId=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 17; i++)
			{
				builder.Append(spanishcharacter[random.Next(spanishcharacter.Length - 1)]);
			}
			devicealert.SuiteInfoId=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 9; i++)
			{
				builder.Append(spanishcharacter[random.Next(spanishcharacter.Length - 1)]);
			}
			devicealert.VehicleId=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 9; i++)
			{
				builder.Append(spanishcharacter[random.Next(spanishcharacter.Length - 1)]);
			}
			devicealert.Cmd=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 9; i++)
			{
				builder.Append(spanishcharacter[random.Next(spanishcharacter.Length - 1)]);
			}
			devicealert.Longitude=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 10; i++)
			{
				builder.Append(spanishcharacter[random.Next(spanishcharacter.Length - 1)]);
			}
			devicealert.Latitude=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 9; i++)
			{
				builder.Append(spanishcharacter[random.Next(spanishcharacter.Length - 1)]);
			}
			devicealert.Speed=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 9; i++)
			{
				builder.Append(spanishcharacter[random.Next(spanishcharacter.Length - 1)]);
			}
			devicealert.Direction=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 0; i++)
			{
				builder.Append(spanishcharacter[random.Next(spanishcharacter.Length - 1)]);
			}
			devicealert.GpsValid=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 127; i++)
			{
				builder.Append(spanishcharacter[random.Next(spanishcharacter.Length - 1)]);
			}
			devicealert.TagValue=builder.ToString();
			builder.Length=0;

			SingleMessage<DeviceAlert> ret = service.AddDeviceAlert(devicealert);
			//Assert.IsFalse(ret.IsSuccess);
		}


		/// <summary>
		/// 测试Latitude英文超长
		/// </summary>
		[TestMethod]
		public void TestLatitudeStringForMaxLength_English()
		{
			DeviceAlert devicealert = new DeviceAlert();
			DeviceAlertRespository service = new DeviceAlertRespository();
			StringBuilder builder = new StringBuilder();
			Random random = new Random();
			for (int i = 0; i < 9; i++)
			{
				builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
			}
			devicealert.Id=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 9; i++)
			{
				builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
			}
			devicealert.MdvrCoreId=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 17; i++)
			{
				builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
			}
			devicealert.SuiteInfoId=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 9; i++)
			{
				builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
			}
			devicealert.VehicleId=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 9; i++)
			{
				builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
			}
			devicealert.Cmd=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 9; i++)
			{
				builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
			}
			devicealert.Longitude=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 10; i++)
			{
				builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
			}
			devicealert.Latitude=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 9; i++)
			{
				builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
			}
			devicealert.Speed=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 9; i++)
			{
				builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
			}
			devicealert.Direction=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 0; i++)
			{
				builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
			}
			devicealert.GpsValid=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 127; i++)
			{
				builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
			}
			devicealert.TagValue=builder.ToString();
			builder.Length=0;

			SingleMessage<DeviceAlert> ret = service.AddDeviceAlert(devicealert);
			//Assert.IsFalse(ret.IsSuccess);
		}


		/// <summary>
		/// 测试GpsTime中文超长
		/// </summary>
		[TestMethod]
		public void TestGpsTimeStringForOverLength_Chinese()
		{
			DeviceAlert devicealert = new DeviceAlert();
			DeviceAlertRespository service = new DeviceAlertRespository();
			StringBuilder builder = new StringBuilder();
			Random random = new Random();
			for (int i = 0; i < 9; i++)
			{
				builder.Append(chinesecharacter[random.Next(chinesecharacter.Length - 1)]);
			}
			devicealert.Id=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 9; i++)
			{
				builder.Append(chinesecharacter[random.Next(chinesecharacter.Length - 1)]);
			}
			devicealert.MdvrCoreId=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 17; i++)
			{
				builder.Append(chinesecharacter[random.Next(chinesecharacter.Length - 1)]);
			}
			devicealert.SuiteInfoId=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 9; i++)
			{
				builder.Append(chinesecharacter[random.Next(chinesecharacter.Length - 1)]);
			}
			devicealert.VehicleId=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 9; i++)
			{
				builder.Append(chinesecharacter[random.Next(chinesecharacter.Length - 1)]);
			}
			devicealert.Cmd=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 9; i++)
			{
				builder.Append(chinesecharacter[random.Next(chinesecharacter.Length - 1)]);
			}
			devicealert.Longitude=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 9; i++)
			{
				builder.Append(chinesecharacter[random.Next(chinesecharacter.Length - 1)]);
			}
			devicealert.Latitude=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 9; i++)
			{
				builder.Append(chinesecharacter[random.Next(chinesecharacter.Length - 1)]);
			}
			devicealert.Speed=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 9; i++)
			{
				builder.Append(chinesecharacter[random.Next(chinesecharacter.Length - 1)]);
			}
			devicealert.Direction=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 0; i++)
			{
				builder.Append(chinesecharacter[random.Next(chinesecharacter.Length - 1)]);
			}
			devicealert.GpsValid=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 127; i++)
			{
				builder.Append(chinesecharacter[random.Next(chinesecharacter.Length - 1)]);
			}
			devicealert.TagValue=builder.ToString();
			builder.Length=0;

			SingleMessage<DeviceAlert> ret = service.AddDeviceAlert(devicealert);
			//Assert.IsFalse(ret.IsSuccess);
		}


		/// <summary>
		/// 测试GpsTime西文超长
		/// </summary>
		[TestMethod]
		public void TestGpsTimeStringForMaxLength_Spanish()
		{
			DeviceAlert devicealert = new DeviceAlert();
			DeviceAlertRespository service = new DeviceAlertRespository();
			StringBuilder builder = new StringBuilder();
			Random random = new Random();
			for (int i = 0; i < 9; i++)
			{
				builder.Append(spanishcharacter[random.Next(spanishcharacter.Length - 1)]);
			}
			devicealert.Id=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 9; i++)
			{
				builder.Append(spanishcharacter[random.Next(spanishcharacter.Length - 1)]);
			}
			devicealert.MdvrCoreId=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 17; i++)
			{
				builder.Append(spanishcharacter[random.Next(spanishcharacter.Length - 1)]);
			}
			devicealert.SuiteInfoId=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 9; i++)
			{
				builder.Append(spanishcharacter[random.Next(spanishcharacter.Length - 1)]);
			}
			devicealert.VehicleId=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 9; i++)
			{
				builder.Append(spanishcharacter[random.Next(spanishcharacter.Length - 1)]);
			}
			devicealert.Cmd=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 9; i++)
			{
				builder.Append(spanishcharacter[random.Next(spanishcharacter.Length - 1)]);
			}
			devicealert.Longitude=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 9; i++)
			{
				builder.Append(spanishcharacter[random.Next(spanishcharacter.Length - 1)]);
			}
			devicealert.Latitude=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 9; i++)
			{
				builder.Append(spanishcharacter[random.Next(spanishcharacter.Length - 1)]);
			}
			devicealert.Speed=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 9; i++)
			{
				builder.Append(spanishcharacter[random.Next(spanishcharacter.Length - 1)]);
			}
			devicealert.Direction=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 0; i++)
			{
				builder.Append(spanishcharacter[random.Next(spanishcharacter.Length - 1)]);
			}
			devicealert.GpsValid=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 127; i++)
			{
				builder.Append(spanishcharacter[random.Next(spanishcharacter.Length - 1)]);
			}
			devicealert.TagValue=builder.ToString();
			builder.Length=0;

			SingleMessage<DeviceAlert> ret = service.AddDeviceAlert(devicealert);
			//Assert.IsFalse(ret.IsSuccess);
		}


		/// <summary>
		/// 测试GpsTime英文超长
		/// </summary>
		[TestMethod]
		public void TestGpsTimeStringForMaxLength_English()
		{
			DeviceAlert devicealert = new DeviceAlert();
			DeviceAlertRespository service = new DeviceAlertRespository();
			StringBuilder builder = new StringBuilder();
			Random random = new Random();
			for (int i = 0; i < 9; i++)
			{
				builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
			}
			devicealert.Id=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 9; i++)
			{
				builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
			}
			devicealert.MdvrCoreId=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 17; i++)
			{
				builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
			}
			devicealert.SuiteInfoId=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 9; i++)
			{
				builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
			}
			devicealert.VehicleId=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 9; i++)
			{
				builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
			}
			devicealert.Cmd=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 9; i++)
			{
				builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
			}
			devicealert.Longitude=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 9; i++)
			{
				builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
			}
			devicealert.Latitude=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 9; i++)
			{
				builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
			}
			devicealert.Speed=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 9; i++)
			{
				builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
			}
			devicealert.Direction=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 0; i++)
			{
				builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
			}
			devicealert.GpsValid=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 127; i++)
			{
				builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
			}
			devicealert.TagValue=builder.ToString();
			builder.Length=0;

			SingleMessage<DeviceAlert> ret = service.AddDeviceAlert(devicealert);
			//Assert.IsFalse(ret.IsSuccess);
		}


		/// <summary>
		/// 测试Speed中文超长
		/// </summary>
		[TestMethod]
		public void TestSpeedStringForOverLength_Chinese()
		{
			DeviceAlert devicealert = new DeviceAlert();
			DeviceAlertRespository service = new DeviceAlertRespository();
			StringBuilder builder = new StringBuilder();
			Random random = new Random();
			for (int i = 0; i < 9; i++)
			{
				builder.Append(chinesecharacter[random.Next(chinesecharacter.Length - 1)]);
			}
			devicealert.Id=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 9; i++)
			{
				builder.Append(chinesecharacter[random.Next(chinesecharacter.Length - 1)]);
			}
			devicealert.MdvrCoreId=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 17; i++)
			{
				builder.Append(chinesecharacter[random.Next(chinesecharacter.Length - 1)]);
			}
			devicealert.SuiteInfoId=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 9; i++)
			{
				builder.Append(chinesecharacter[random.Next(chinesecharacter.Length - 1)]);
			}
			devicealert.VehicleId=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 9; i++)
			{
				builder.Append(chinesecharacter[random.Next(chinesecharacter.Length - 1)]);
			}
			devicealert.Cmd=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 9; i++)
			{
				builder.Append(chinesecharacter[random.Next(chinesecharacter.Length - 1)]);
			}
			devicealert.Longitude=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 9; i++)
			{
				builder.Append(chinesecharacter[random.Next(chinesecharacter.Length - 1)]);
			}
			devicealert.Latitude=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 10; i++)
			{
				builder.Append(chinesecharacter[random.Next(chinesecharacter.Length - 1)]);
			}
			devicealert.Speed=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 9; i++)
			{
				builder.Append(chinesecharacter[random.Next(chinesecharacter.Length - 1)]);
			}
			devicealert.Direction=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 0; i++)
			{
				builder.Append(chinesecharacter[random.Next(chinesecharacter.Length - 1)]);
			}
			devicealert.GpsValid=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 127; i++)
			{
				builder.Append(chinesecharacter[random.Next(chinesecharacter.Length - 1)]);
			}
			devicealert.TagValue=builder.ToString();
			builder.Length=0;

			SingleMessage<DeviceAlert> ret = service.AddDeviceAlert(devicealert);
			//Assert.IsFalse(ret.IsSuccess);
		}


		/// <summary>
		/// 测试Speed西文超长
		/// </summary>
		[TestMethod]
		public void TestSpeedStringForMaxLength_Spanish()
		{
			DeviceAlert devicealert = new DeviceAlert();
			DeviceAlertRespository service = new DeviceAlertRespository();
			StringBuilder builder = new StringBuilder();
			Random random = new Random();
			for (int i = 0; i < 9; i++)
			{
				builder.Append(spanishcharacter[random.Next(spanishcharacter.Length - 1)]);
			}
			devicealert.Id=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 9; i++)
			{
				builder.Append(spanishcharacter[random.Next(spanishcharacter.Length - 1)]);
			}
			devicealert.MdvrCoreId=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 17; i++)
			{
				builder.Append(spanishcharacter[random.Next(spanishcharacter.Length - 1)]);
			}
			devicealert.SuiteInfoId=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 9; i++)
			{
				builder.Append(spanishcharacter[random.Next(spanishcharacter.Length - 1)]);
			}
			devicealert.VehicleId=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 9; i++)
			{
				builder.Append(spanishcharacter[random.Next(spanishcharacter.Length - 1)]);
			}
			devicealert.Cmd=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 9; i++)
			{
				builder.Append(spanishcharacter[random.Next(spanishcharacter.Length - 1)]);
			}
			devicealert.Longitude=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 9; i++)
			{
				builder.Append(spanishcharacter[random.Next(spanishcharacter.Length - 1)]);
			}
			devicealert.Latitude=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 10; i++)
			{
				builder.Append(spanishcharacter[random.Next(spanishcharacter.Length - 1)]);
			}
			devicealert.Speed=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 9; i++)
			{
				builder.Append(spanishcharacter[random.Next(spanishcharacter.Length - 1)]);
			}
			devicealert.Direction=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 0; i++)
			{
				builder.Append(spanishcharacter[random.Next(spanishcharacter.Length - 1)]);
			}
			devicealert.GpsValid=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 127; i++)
			{
				builder.Append(spanishcharacter[random.Next(spanishcharacter.Length - 1)]);
			}
			devicealert.TagValue=builder.ToString();
			builder.Length=0;

			SingleMessage<DeviceAlert> ret = service.AddDeviceAlert(devicealert);
			//Assert.IsFalse(ret.IsSuccess);
		}


		/// <summary>
		/// 测试Speed英文超长
		/// </summary>
		[TestMethod]
		public void TestSpeedStringForMaxLength_English()
		{
			DeviceAlert devicealert = new DeviceAlert();
			DeviceAlertRespository service = new DeviceAlertRespository();
			StringBuilder builder = new StringBuilder();
			Random random = new Random();
			for (int i = 0; i < 9; i++)
			{
				builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
			}
			devicealert.Id=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 9; i++)
			{
				builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
			}
			devicealert.MdvrCoreId=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 17; i++)
			{
				builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
			}
			devicealert.SuiteInfoId=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 9; i++)
			{
				builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
			}
			devicealert.VehicleId=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 9; i++)
			{
				builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
			}
			devicealert.Cmd=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 9; i++)
			{
				builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
			}
			devicealert.Longitude=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 9; i++)
			{
				builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
			}
			devicealert.Latitude=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 10; i++)
			{
				builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
			}
			devicealert.Speed=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 9; i++)
			{
				builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
			}
			devicealert.Direction=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 0; i++)
			{
				builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
			}
			devicealert.GpsValid=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 127; i++)
			{
				builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
			}
			devicealert.TagValue=builder.ToString();
			builder.Length=0;

			SingleMessage<DeviceAlert> ret = service.AddDeviceAlert(devicealert);
			//Assert.IsFalse(ret.IsSuccess);
		}


		/// <summary>
		/// 测试Direction中文超长
		/// </summary>
		[TestMethod]
		public void TestDirectionStringForOverLength_Chinese()
		{
			DeviceAlert devicealert = new DeviceAlert();
			DeviceAlertRespository service = new DeviceAlertRespository();
			StringBuilder builder = new StringBuilder();
			Random random = new Random();
			for (int i = 0; i < 9; i++)
			{
				builder.Append(chinesecharacter[random.Next(chinesecharacter.Length - 1)]);
			}
			devicealert.Id=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 9; i++)
			{
				builder.Append(chinesecharacter[random.Next(chinesecharacter.Length - 1)]);
			}
			devicealert.MdvrCoreId=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 17; i++)
			{
				builder.Append(chinesecharacter[random.Next(chinesecharacter.Length - 1)]);
			}
			devicealert.SuiteInfoId=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 9; i++)
			{
				builder.Append(chinesecharacter[random.Next(chinesecharacter.Length - 1)]);
			}
			devicealert.VehicleId=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 9; i++)
			{
				builder.Append(chinesecharacter[random.Next(chinesecharacter.Length - 1)]);
			}
			devicealert.Cmd=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 9; i++)
			{
				builder.Append(chinesecharacter[random.Next(chinesecharacter.Length - 1)]);
			}
			devicealert.Longitude=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 9; i++)
			{
				builder.Append(chinesecharacter[random.Next(chinesecharacter.Length - 1)]);
			}
			devicealert.Latitude=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 9; i++)
			{
				builder.Append(chinesecharacter[random.Next(chinesecharacter.Length - 1)]);
			}
			devicealert.Speed=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 10; i++)
			{
				builder.Append(chinesecharacter[random.Next(chinesecharacter.Length - 1)]);
			}
			devicealert.Direction=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 0; i++)
			{
				builder.Append(chinesecharacter[random.Next(chinesecharacter.Length - 1)]);
			}
			devicealert.GpsValid=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 127; i++)
			{
				builder.Append(chinesecharacter[random.Next(chinesecharacter.Length - 1)]);
			}
			devicealert.TagValue=builder.ToString();
			builder.Length=0;

			SingleMessage<DeviceAlert> ret = service.AddDeviceAlert(devicealert);
			//Assert.IsFalse(ret.IsSuccess);
		}


		/// <summary>
		/// 测试Direction西文超长
		/// </summary>
		[TestMethod]
		public void TestDirectionStringForMaxLength_Spanish()
		{
			DeviceAlert devicealert = new DeviceAlert();
			DeviceAlertRespository service = new DeviceAlertRespository();
			StringBuilder builder = new StringBuilder();
			Random random = new Random();
			for (int i = 0; i < 9; i++)
			{
				builder.Append(spanishcharacter[random.Next(spanishcharacter.Length - 1)]);
			}
			devicealert.Id=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 9; i++)
			{
				builder.Append(spanishcharacter[random.Next(spanishcharacter.Length - 1)]);
			}
			devicealert.MdvrCoreId=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 17; i++)
			{
				builder.Append(spanishcharacter[random.Next(spanishcharacter.Length - 1)]);
			}
			devicealert.SuiteInfoId=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 9; i++)
			{
				builder.Append(spanishcharacter[random.Next(spanishcharacter.Length - 1)]);
			}
			devicealert.VehicleId=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 9; i++)
			{
				builder.Append(spanishcharacter[random.Next(spanishcharacter.Length - 1)]);
			}
			devicealert.Cmd=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 9; i++)
			{
				builder.Append(spanishcharacter[random.Next(spanishcharacter.Length - 1)]);
			}
			devicealert.Longitude=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 9; i++)
			{
				builder.Append(spanishcharacter[random.Next(spanishcharacter.Length - 1)]);
			}
			devicealert.Latitude=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 9; i++)
			{
				builder.Append(spanishcharacter[random.Next(spanishcharacter.Length - 1)]);
			}
			devicealert.Speed=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 10; i++)
			{
				builder.Append(spanishcharacter[random.Next(spanishcharacter.Length - 1)]);
			}
			devicealert.Direction=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 0; i++)
			{
				builder.Append(spanishcharacter[random.Next(spanishcharacter.Length - 1)]);
			}
			devicealert.GpsValid=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 127; i++)
			{
				builder.Append(spanishcharacter[random.Next(spanishcharacter.Length - 1)]);
			}
			devicealert.TagValue=builder.ToString();
			builder.Length=0;

			SingleMessage<DeviceAlert> ret = service.AddDeviceAlert(devicealert);
			//Assert.IsFalse(ret.IsSuccess);
		}


		/// <summary>
		/// 测试Direction英文超长
		/// </summary>
		[TestMethod]
		public void TestDirectionStringForMaxLength_English()
		{
			DeviceAlert devicealert = new DeviceAlert();
			DeviceAlertRespository service = new DeviceAlertRespository();
			StringBuilder builder = new StringBuilder();
			Random random = new Random();
			for (int i = 0; i < 9; i++)
			{
				builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
			}
			devicealert.Id=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 9; i++)
			{
				builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
			}
			devicealert.MdvrCoreId=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 17; i++)
			{
				builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
			}
			devicealert.SuiteInfoId=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 9; i++)
			{
				builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
			}
			devicealert.VehicleId=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 9; i++)
			{
				builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
			}
			devicealert.Cmd=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 9; i++)
			{
				builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
			}
			devicealert.Longitude=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 9; i++)
			{
				builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
			}
			devicealert.Latitude=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 9; i++)
			{
				builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
			}
			devicealert.Speed=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 10; i++)
			{
				builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
			}
			devicealert.Direction=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 0; i++)
			{
				builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
			}
			devicealert.GpsValid=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 127; i++)
			{
				builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
			}
			devicealert.TagValue=builder.ToString();
			builder.Length=0;

			SingleMessage<DeviceAlert> ret = service.AddDeviceAlert(devicealert);
			//Assert.IsFalse(ret.IsSuccess);
		}


		/// <summary>
		/// 测试GpsValid中文超长
		/// </summary>
		[TestMethod]
		public void TestGpsValidStringForOverLength_Chinese()
		{
			DeviceAlert devicealert = new DeviceAlert();
			DeviceAlertRespository service = new DeviceAlertRespository();
			StringBuilder builder = new StringBuilder();
			Random random = new Random();
			for (int i = 0; i < 9; i++)
			{
				builder.Append(chinesecharacter[random.Next(chinesecharacter.Length - 1)]);
			}
			devicealert.Id=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 9; i++)
			{
				builder.Append(chinesecharacter[random.Next(chinesecharacter.Length - 1)]);
			}
			devicealert.MdvrCoreId=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 17; i++)
			{
				builder.Append(chinesecharacter[random.Next(chinesecharacter.Length - 1)]);
			}
			devicealert.SuiteInfoId=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 9; i++)
			{
				builder.Append(chinesecharacter[random.Next(chinesecharacter.Length - 1)]);
			}
			devicealert.VehicleId=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 9; i++)
			{
				builder.Append(chinesecharacter[random.Next(chinesecharacter.Length - 1)]);
			}
			devicealert.Cmd=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 9; i++)
			{
				builder.Append(chinesecharacter[random.Next(chinesecharacter.Length - 1)]);
			}
			devicealert.Longitude=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 9; i++)
			{
				builder.Append(chinesecharacter[random.Next(chinesecharacter.Length - 1)]);
			}
			devicealert.Latitude=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 9; i++)
			{
				builder.Append(chinesecharacter[random.Next(chinesecharacter.Length - 1)]);
			}
			devicealert.Speed=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 9; i++)
			{
				builder.Append(chinesecharacter[random.Next(chinesecharacter.Length - 1)]);
			}
			devicealert.Direction=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 1; i++)
			{
				builder.Append(chinesecharacter[random.Next(chinesecharacter.Length - 1)]);
			}
			devicealert.GpsValid=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 127; i++)
			{
				builder.Append(chinesecharacter[random.Next(chinesecharacter.Length - 1)]);
			}
			devicealert.TagValue=builder.ToString();
			builder.Length=0;

			SingleMessage<DeviceAlert> ret = service.AddDeviceAlert(devicealert);
			//Assert.IsFalse(ret.IsSuccess);
		}


		/// <summary>
		/// 测试GpsValid西文超长
		/// </summary>
		[TestMethod]
		public void TestGpsValidStringForMaxLength_Spanish()
		{
			DeviceAlert devicealert = new DeviceAlert();
			DeviceAlertRespository service = new DeviceAlertRespository();
			StringBuilder builder = new StringBuilder();
			Random random = new Random();
			for (int i = 0; i < 9; i++)
			{
				builder.Append(spanishcharacter[random.Next(spanishcharacter.Length - 1)]);
			}
			devicealert.Id=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 9; i++)
			{
				builder.Append(spanishcharacter[random.Next(spanishcharacter.Length - 1)]);
			}
			devicealert.MdvrCoreId=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 17; i++)
			{
				builder.Append(spanishcharacter[random.Next(spanishcharacter.Length - 1)]);
			}
			devicealert.SuiteInfoId=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 9; i++)
			{
				builder.Append(spanishcharacter[random.Next(spanishcharacter.Length - 1)]);
			}
			devicealert.VehicleId=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 9; i++)
			{
				builder.Append(spanishcharacter[random.Next(spanishcharacter.Length - 1)]);
			}
			devicealert.Cmd=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 9; i++)
			{
				builder.Append(spanishcharacter[random.Next(spanishcharacter.Length - 1)]);
			}
			devicealert.Longitude=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 9; i++)
			{
				builder.Append(spanishcharacter[random.Next(spanishcharacter.Length - 1)]);
			}
			devicealert.Latitude=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 9; i++)
			{
				builder.Append(spanishcharacter[random.Next(spanishcharacter.Length - 1)]);
			}
			devicealert.Speed=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 9; i++)
			{
				builder.Append(spanishcharacter[random.Next(spanishcharacter.Length - 1)]);
			}
			devicealert.Direction=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 1; i++)
			{
				builder.Append(spanishcharacter[random.Next(spanishcharacter.Length - 1)]);
			}
			devicealert.GpsValid=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 127; i++)
			{
				builder.Append(spanishcharacter[random.Next(spanishcharacter.Length - 1)]);
			}
			devicealert.TagValue=builder.ToString();
			builder.Length=0;

			SingleMessage<DeviceAlert> ret = service.AddDeviceAlert(devicealert);
			//Assert.IsFalse(ret.IsSuccess);
		}


		/// <summary>
		/// 测试GpsValid英文超长
		/// </summary>
		[TestMethod]
		public void TestGpsValidStringForMaxLength_English()
		{
			DeviceAlert devicealert = new DeviceAlert();
			DeviceAlertRespository service = new DeviceAlertRespository();
			StringBuilder builder = new StringBuilder();
			Random random = new Random();
			for (int i = 0; i < 9; i++)
			{
				builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
			}
			devicealert.Id=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 9; i++)
			{
				builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
			}
			devicealert.MdvrCoreId=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 17; i++)
			{
				builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
			}
			devicealert.SuiteInfoId=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 9; i++)
			{
				builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
			}
			devicealert.VehicleId=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 9; i++)
			{
				builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
			}
			devicealert.Cmd=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 9; i++)
			{
				builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
			}
			devicealert.Longitude=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 9; i++)
			{
				builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
			}
			devicealert.Latitude=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 9; i++)
			{
				builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
			}
			devicealert.Speed=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 9; i++)
			{
				builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
			}
			devicealert.Direction=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 1; i++)
			{
				builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
			}
			devicealert.GpsValid=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 127; i++)
			{
				builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
			}
			devicealert.TagValue=builder.ToString();
			builder.Length=0;

			SingleMessage<DeviceAlert> ret = service.AddDeviceAlert(devicealert);
			//Assert.IsFalse(ret.IsSuccess);
		}


		/// <summary>
		/// 测试TagValue中文超长
		/// </summary>
		[TestMethod]
		public void TestTagValueStringForOverLength_Chinese()
		{
			DeviceAlert devicealert = new DeviceAlert();
			DeviceAlertRespository service = new DeviceAlertRespository();
			StringBuilder builder = new StringBuilder();
			Random random = new Random();
			for (int i = 0; i < 9; i++)
			{
				builder.Append(chinesecharacter[random.Next(chinesecharacter.Length - 1)]);
			}
			devicealert.Id=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 9; i++)
			{
				builder.Append(chinesecharacter[random.Next(chinesecharacter.Length - 1)]);
			}
			devicealert.MdvrCoreId=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 17; i++)
			{
				builder.Append(chinesecharacter[random.Next(chinesecharacter.Length - 1)]);
			}
			devicealert.SuiteInfoId=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 9; i++)
			{
				builder.Append(chinesecharacter[random.Next(chinesecharacter.Length - 1)]);
			}
			devicealert.VehicleId=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 9; i++)
			{
				builder.Append(chinesecharacter[random.Next(chinesecharacter.Length - 1)]);
			}
			devicealert.Cmd=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 9; i++)
			{
				builder.Append(chinesecharacter[random.Next(chinesecharacter.Length - 1)]);
			}
			devicealert.Longitude=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 9; i++)
			{
				builder.Append(chinesecharacter[random.Next(chinesecharacter.Length - 1)]);
			}
			devicealert.Latitude=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 9; i++)
			{
				builder.Append(chinesecharacter[random.Next(chinesecharacter.Length - 1)]);
			}
			devicealert.Speed=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 9; i++)
			{
				builder.Append(chinesecharacter[random.Next(chinesecharacter.Length - 1)]);
			}
			devicealert.Direction=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 0; i++)
			{
				builder.Append(chinesecharacter[random.Next(chinesecharacter.Length - 1)]);
			}
			devicealert.GpsValid=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 128; i++)
			{
				builder.Append(chinesecharacter[random.Next(chinesecharacter.Length - 1)]);
			}
			devicealert.TagValue=builder.ToString();
			builder.Length=0;

			SingleMessage<DeviceAlert> ret = service.AddDeviceAlert(devicealert);
			//Assert.IsFalse(ret.IsSuccess);
		}


		/// <summary>
		/// 测试TagValue西文超长
		/// </summary>
		[TestMethod]
		public void TestTagValueStringForMaxLength_Spanish()
		{
			DeviceAlert devicealert = new DeviceAlert();
			DeviceAlertRespository service = new DeviceAlertRespository();
			StringBuilder builder = new StringBuilder();
			Random random = new Random();
			for (int i = 0; i < 9; i++)
			{
				builder.Append(spanishcharacter[random.Next(spanishcharacter.Length - 1)]);
			}
			devicealert.Id=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 9; i++)
			{
				builder.Append(spanishcharacter[random.Next(spanishcharacter.Length - 1)]);
			}
			devicealert.MdvrCoreId=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 17; i++)
			{
				builder.Append(spanishcharacter[random.Next(spanishcharacter.Length - 1)]);
			}
			devicealert.SuiteInfoId=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 9; i++)
			{
				builder.Append(spanishcharacter[random.Next(spanishcharacter.Length - 1)]);
			}
			devicealert.VehicleId=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 9; i++)
			{
				builder.Append(spanishcharacter[random.Next(spanishcharacter.Length - 1)]);
			}
			devicealert.Cmd=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 9; i++)
			{
				builder.Append(spanishcharacter[random.Next(spanishcharacter.Length - 1)]);
			}
			devicealert.Longitude=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 9; i++)
			{
				builder.Append(spanishcharacter[random.Next(spanishcharacter.Length - 1)]);
			}
			devicealert.Latitude=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 9; i++)
			{
				builder.Append(spanishcharacter[random.Next(spanishcharacter.Length - 1)]);
			}
			devicealert.Speed=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 9; i++)
			{
				builder.Append(spanishcharacter[random.Next(spanishcharacter.Length - 1)]);
			}
			devicealert.Direction=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 0; i++)
			{
				builder.Append(spanishcharacter[random.Next(spanishcharacter.Length - 1)]);
			}
			devicealert.GpsValid=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 128; i++)
			{
				builder.Append(spanishcharacter[random.Next(spanishcharacter.Length - 1)]);
			}
			devicealert.TagValue=builder.ToString();
			builder.Length=0;

			SingleMessage<DeviceAlert> ret = service.AddDeviceAlert(devicealert);
			//Assert.IsFalse(ret.IsSuccess);
		}


		/// <summary>
		/// 测试TagValue英文超长
		/// </summary>
		[TestMethod]
		public void TestTagValueStringForMaxLength_English()
		{
			DeviceAlert devicealert = new DeviceAlert();
			DeviceAlertRespository service = new DeviceAlertRespository();
			StringBuilder builder = new StringBuilder();
			Random random = new Random();
			for (int i = 0; i < 9; i++)
			{
				builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
			}
			devicealert.Id=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 9; i++)
			{
				builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
			}
			devicealert.MdvrCoreId=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 17; i++)
			{
				builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
			}
			devicealert.SuiteInfoId=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 9; i++)
			{
				builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
			}
			devicealert.VehicleId=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 9; i++)
			{
				builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
			}
			devicealert.Cmd=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 9; i++)
			{
				builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
			}
			devicealert.Longitude=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 9; i++)
			{
				builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
			}
			devicealert.Latitude=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 9; i++)
			{
				builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
			}
			devicealert.Speed=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 9; i++)
			{
				builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
			}
			devicealert.Direction=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 0; i++)
			{
				builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
			}
			devicealert.GpsValid=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 128; i++)
			{
				builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
			}
			devicealert.TagValue=builder.ToString();
			builder.Length=0;

			SingleMessage<DeviceAlert> ret = service.AddDeviceAlert(devicealert);
			//Assert.IsFalse(ret.IsSuccess);
		}


		/// <summary>
		/// 测试Status中文超长
		/// </summary>
		[TestMethod]
		public void TestStatusStringForOverLength_Chinese()
		{
			DeviceAlert devicealert = new DeviceAlert();
			DeviceAlertRespository service = new DeviceAlertRespository();
			StringBuilder builder = new StringBuilder();
			Random random = new Random();
			for (int i = 0; i < 9; i++)
			{
				builder.Append(chinesecharacter[random.Next(chinesecharacter.Length - 1)]);
			}
			devicealert.Id=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 9; i++)
			{
				builder.Append(chinesecharacter[random.Next(chinesecharacter.Length - 1)]);
			}
			devicealert.MdvrCoreId=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 17; i++)
			{
				builder.Append(chinesecharacter[random.Next(chinesecharacter.Length - 1)]);
			}
			devicealert.SuiteInfoId=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 9; i++)
			{
				builder.Append(chinesecharacter[random.Next(chinesecharacter.Length - 1)]);
			}
			devicealert.VehicleId=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 9; i++)
			{
				builder.Append(chinesecharacter[random.Next(chinesecharacter.Length - 1)]);
			}
			devicealert.Cmd=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 9; i++)
			{
				builder.Append(chinesecharacter[random.Next(chinesecharacter.Length - 1)]);
			}
			devicealert.Longitude=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 9; i++)
			{
				builder.Append(chinesecharacter[random.Next(chinesecharacter.Length - 1)]);
			}
			devicealert.Latitude=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 9; i++)
			{
				builder.Append(chinesecharacter[random.Next(chinesecharacter.Length - 1)]);
			}
			devicealert.Speed=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 9; i++)
			{
				builder.Append(chinesecharacter[random.Next(chinesecharacter.Length - 1)]);
			}
			devicealert.Direction=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 0; i++)
			{
				builder.Append(chinesecharacter[random.Next(chinesecharacter.Length - 1)]);
			}
			devicealert.GpsValid=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 127; i++)
			{
				builder.Append(chinesecharacter[random.Next(chinesecharacter.Length - 1)]);
			}
			devicealert.TagValue=builder.ToString();
			builder.Length=0;

			SingleMessage<DeviceAlert> ret = service.AddDeviceAlert(devicealert);
			//Assert.IsFalse(ret.IsSuccess);
		}


		/// <summary>
		/// 测试Status西文超长
		/// </summary>
		[TestMethod]
		public void TestStatusStringForMaxLength_Spanish()
		{
			DeviceAlert devicealert = new DeviceAlert();
			DeviceAlertRespository service = new DeviceAlertRespository();
			StringBuilder builder = new StringBuilder();
			Random random = new Random();
			for (int i = 0; i < 9; i++)
			{
				builder.Append(spanishcharacter[random.Next(spanishcharacter.Length - 1)]);
			}
			devicealert.Id=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 9; i++)
			{
				builder.Append(spanishcharacter[random.Next(spanishcharacter.Length - 1)]);
			}
			devicealert.MdvrCoreId=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 17; i++)
			{
				builder.Append(spanishcharacter[random.Next(spanishcharacter.Length - 1)]);
			}
			devicealert.SuiteInfoId=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 9; i++)
			{
				builder.Append(spanishcharacter[random.Next(spanishcharacter.Length - 1)]);
			}
			devicealert.VehicleId=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 9; i++)
			{
				builder.Append(spanishcharacter[random.Next(spanishcharacter.Length - 1)]);
			}
			devicealert.Cmd=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 9; i++)
			{
				builder.Append(spanishcharacter[random.Next(spanishcharacter.Length - 1)]);
			}
			devicealert.Longitude=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 9; i++)
			{
				builder.Append(spanishcharacter[random.Next(spanishcharacter.Length - 1)]);
			}
			devicealert.Latitude=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 9; i++)
			{
				builder.Append(spanishcharacter[random.Next(spanishcharacter.Length - 1)]);
			}
			devicealert.Speed=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 9; i++)
			{
				builder.Append(spanishcharacter[random.Next(spanishcharacter.Length - 1)]);
			}
			devicealert.Direction=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 0; i++)
			{
				builder.Append(spanishcharacter[random.Next(spanishcharacter.Length - 1)]);
			}
			devicealert.GpsValid=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 127; i++)
			{
				builder.Append(spanishcharacter[random.Next(spanishcharacter.Length - 1)]);
			}
			devicealert.TagValue=builder.ToString();
			builder.Length=0;

			SingleMessage<DeviceAlert> ret = service.AddDeviceAlert(devicealert);
			//Assert.IsFalse(ret.IsSuccess);
		}


		/// <summary>
		/// 测试Status英文超长
		/// </summary>
		[TestMethod]
		public void TestStatusStringForMaxLength_English()
		{
			DeviceAlert devicealert = new DeviceAlert();
			DeviceAlertRespository service = new DeviceAlertRespository();
			StringBuilder builder = new StringBuilder();
			Random random = new Random();
			for (int i = 0; i < 9; i++)
			{
				builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
			}
			devicealert.Id=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 9; i++)
			{
				builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
			}
			devicealert.MdvrCoreId=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 17; i++)
			{
				builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
			}
			devicealert.SuiteInfoId=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 9; i++)
			{
				builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
			}
			devicealert.VehicleId=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 9; i++)
			{
				builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
			}
			devicealert.Cmd=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 9; i++)
			{
				builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
			}
			devicealert.Longitude=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 9; i++)
			{
				builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
			}
			devicealert.Latitude=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 9; i++)
			{
				builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
			}
			devicealert.Speed=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 9; i++)
			{
				builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
			}
			devicealert.Direction=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 0; i++)
			{
				builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
			}
			devicealert.GpsValid=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 127; i++)
			{
				builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
			}
			devicealert.TagValue=builder.ToString();
			builder.Length=0;

			SingleMessage<DeviceAlert> ret = service.AddDeviceAlert(devicealert);
			//Assert.IsFalse(ret.IsSuccess);
		}


		/// <summary>
		/// 测试HandleId中文超长
		/// </summary>
		[TestMethod]
		public void TestHandleIdStringForOverLength_Chinese()
		{
			DeviceAlert devicealert = new DeviceAlert();
			DeviceAlertRespository service = new DeviceAlertRespository();
			StringBuilder builder = new StringBuilder();
			Random random = new Random();
			for (int i = 0; i < 9; i++)
			{
				builder.Append(chinesecharacter[random.Next(chinesecharacter.Length - 1)]);
			}
			devicealert.Id=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 9; i++)
			{
				builder.Append(chinesecharacter[random.Next(chinesecharacter.Length - 1)]);
			}
			devicealert.MdvrCoreId=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 17; i++)
			{
				builder.Append(chinesecharacter[random.Next(chinesecharacter.Length - 1)]);
			}
			devicealert.SuiteInfoId=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 9; i++)
			{
				builder.Append(chinesecharacter[random.Next(chinesecharacter.Length - 1)]);
			}
			devicealert.VehicleId=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 9; i++)
			{
				builder.Append(chinesecharacter[random.Next(chinesecharacter.Length - 1)]);
			}
			devicealert.Cmd=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 9; i++)
			{
				builder.Append(chinesecharacter[random.Next(chinesecharacter.Length - 1)]);
			}
			devicealert.Longitude=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 9; i++)
			{
				builder.Append(chinesecharacter[random.Next(chinesecharacter.Length - 1)]);
			}
			devicealert.Latitude=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 9; i++)
			{
				builder.Append(chinesecharacter[random.Next(chinesecharacter.Length - 1)]);
			}
			devicealert.Speed=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 9; i++)
			{
				builder.Append(chinesecharacter[random.Next(chinesecharacter.Length - 1)]);
			}
			devicealert.Direction=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 0; i++)
			{
				builder.Append(chinesecharacter[random.Next(chinesecharacter.Length - 1)]);
			}
			devicealert.GpsValid=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 127; i++)
			{
				builder.Append(chinesecharacter[random.Next(chinesecharacter.Length - 1)]);
			}
			devicealert.TagValue=builder.ToString();
			builder.Length=0;

			SingleMessage<DeviceAlert> ret = service.AddDeviceAlert(devicealert);
			//Assert.IsFalse(ret.IsSuccess);
		}


		/// <summary>
		/// 测试HandleId西文超长
		/// </summary>
		[TestMethod]
		public void TestHandleIdStringForMaxLength_Spanish()
		{
			DeviceAlert devicealert = new DeviceAlert();
			DeviceAlertRespository service = new DeviceAlertRespository();
			StringBuilder builder = new StringBuilder();
			Random random = new Random();
			for (int i = 0; i < 9; i++)
			{
				builder.Append(spanishcharacter[random.Next(spanishcharacter.Length - 1)]);
			}
			devicealert.Id=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 9; i++)
			{
				builder.Append(spanishcharacter[random.Next(spanishcharacter.Length - 1)]);
			}
			devicealert.MdvrCoreId=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 17; i++)
			{
				builder.Append(spanishcharacter[random.Next(spanishcharacter.Length - 1)]);
			}
			devicealert.SuiteInfoId=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 9; i++)
			{
				builder.Append(spanishcharacter[random.Next(spanishcharacter.Length - 1)]);
			}
			devicealert.VehicleId=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 9; i++)
			{
				builder.Append(spanishcharacter[random.Next(spanishcharacter.Length - 1)]);
			}
			devicealert.Cmd=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 9; i++)
			{
				builder.Append(spanishcharacter[random.Next(spanishcharacter.Length - 1)]);
			}
			devicealert.Longitude=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 9; i++)
			{
				builder.Append(spanishcharacter[random.Next(spanishcharacter.Length - 1)]);
			}
			devicealert.Latitude=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 9; i++)
			{
				builder.Append(spanishcharacter[random.Next(spanishcharacter.Length - 1)]);
			}
			devicealert.Speed=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 9; i++)
			{
				builder.Append(spanishcharacter[random.Next(spanishcharacter.Length - 1)]);
			}
			devicealert.Direction=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 0; i++)
			{
				builder.Append(spanishcharacter[random.Next(spanishcharacter.Length - 1)]);
			}
			devicealert.GpsValid=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 127; i++)
			{
				builder.Append(spanishcharacter[random.Next(spanishcharacter.Length - 1)]);
			}
			devicealert.TagValue=builder.ToString();
			builder.Length=0;

			SingleMessage<DeviceAlert> ret = service.AddDeviceAlert(devicealert);
			//Assert.IsFalse(ret.IsSuccess);
		}


		/// <summary>
		/// 测试HandleId英文超长
		/// </summary>
		[TestMethod]
		public void TestHandleIdStringForMaxLength_English()
		{
			DeviceAlert devicealert = new DeviceAlert();
			DeviceAlertRespository service = new DeviceAlertRespository();
			StringBuilder builder = new StringBuilder();
			Random random = new Random();
			for (int i = 0; i < 9; i++)
			{
				builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
			}
			devicealert.Id=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 9; i++)
			{
				builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
			}
			devicealert.MdvrCoreId=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 17; i++)
			{
				builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
			}
			devicealert.SuiteInfoId=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 9; i++)
			{
				builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
			}
			devicealert.VehicleId=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 9; i++)
			{
				builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
			}
			devicealert.Cmd=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 9; i++)
			{
				builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
			}
			devicealert.Longitude=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 9; i++)
			{
				builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
			}
			devicealert.Latitude=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 9; i++)
			{
				builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
			}
			devicealert.Speed=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 9; i++)
			{
				builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
			}
			devicealert.Direction=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 0; i++)
			{
				builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
			}
			devicealert.GpsValid=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 127; i++)
			{
				builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
			}
			devicealert.TagValue=builder.ToString();
			builder.Length=0;

			SingleMessage<DeviceAlert> ret = service.AddDeviceAlert(devicealert);
			//Assert.IsFalse(ret.IsSuccess);
		}


		/// <summary>
		/// 测试RuleId中文超长
		/// </summary>
		[TestMethod]
		public void TestRuleIdStringForOverLength_Chinese()
		{
			DeviceAlert devicealert = new DeviceAlert();
			DeviceAlertRespository service = new DeviceAlertRespository();
			StringBuilder builder = new StringBuilder();
			Random random = new Random();
			for (int i = 0; i < 9; i++)
			{
				builder.Append(chinesecharacter[random.Next(chinesecharacter.Length - 1)]);
			}
			devicealert.Id=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 9; i++)
			{
				builder.Append(chinesecharacter[random.Next(chinesecharacter.Length - 1)]);
			}
			devicealert.MdvrCoreId=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 17; i++)
			{
				builder.Append(chinesecharacter[random.Next(chinesecharacter.Length - 1)]);
			}
			devicealert.SuiteInfoId=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 9; i++)
			{
				builder.Append(chinesecharacter[random.Next(chinesecharacter.Length - 1)]);
			}
			devicealert.VehicleId=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 9; i++)
			{
				builder.Append(chinesecharacter[random.Next(chinesecharacter.Length - 1)]);
			}
			devicealert.Cmd=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 9; i++)
			{
				builder.Append(chinesecharacter[random.Next(chinesecharacter.Length - 1)]);
			}
			devicealert.Longitude=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 9; i++)
			{
				builder.Append(chinesecharacter[random.Next(chinesecharacter.Length - 1)]);
			}
			devicealert.Latitude=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 9; i++)
			{
				builder.Append(chinesecharacter[random.Next(chinesecharacter.Length - 1)]);
			}
			devicealert.Speed=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 9; i++)
			{
				builder.Append(chinesecharacter[random.Next(chinesecharacter.Length - 1)]);
			}
			devicealert.Direction=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 0; i++)
			{
				builder.Append(chinesecharacter[random.Next(chinesecharacter.Length - 1)]);
			}
			devicealert.GpsValid=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 127; i++)
			{
				builder.Append(chinesecharacter[random.Next(chinesecharacter.Length - 1)]);
			}
			devicealert.TagValue=builder.ToString();
			builder.Length=0;

			SingleMessage<DeviceAlert> ret = service.AddDeviceAlert(devicealert);
			//Assert.IsFalse(ret.IsSuccess);
		}


		/// <summary>
		/// 测试RuleId西文超长
		/// </summary>
		[TestMethod]
		public void TestRuleIdStringForMaxLength_Spanish()
		{
			DeviceAlert devicealert = new DeviceAlert();
			DeviceAlertRespository service = new DeviceAlertRespository();
			StringBuilder builder = new StringBuilder();
			Random random = new Random();
			for (int i = 0; i < 9; i++)
			{
				builder.Append(spanishcharacter[random.Next(spanishcharacter.Length - 1)]);
			}
			devicealert.Id=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 9; i++)
			{
				builder.Append(spanishcharacter[random.Next(spanishcharacter.Length - 1)]);
			}
			devicealert.MdvrCoreId=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 17; i++)
			{
				builder.Append(spanishcharacter[random.Next(spanishcharacter.Length - 1)]);
			}
			devicealert.SuiteInfoId=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 9; i++)
			{
				builder.Append(spanishcharacter[random.Next(spanishcharacter.Length - 1)]);
			}
			devicealert.VehicleId=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 9; i++)
			{
				builder.Append(spanishcharacter[random.Next(spanishcharacter.Length - 1)]);
			}
			devicealert.Cmd=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 9; i++)
			{
				builder.Append(spanishcharacter[random.Next(spanishcharacter.Length - 1)]);
			}
			devicealert.Longitude=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 9; i++)
			{
				builder.Append(spanishcharacter[random.Next(spanishcharacter.Length - 1)]);
			}
			devicealert.Latitude=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 9; i++)
			{
				builder.Append(spanishcharacter[random.Next(spanishcharacter.Length - 1)]);
			}
			devicealert.Speed=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 9; i++)
			{
				builder.Append(spanishcharacter[random.Next(spanishcharacter.Length - 1)]);
			}
			devicealert.Direction=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 0; i++)
			{
				builder.Append(spanishcharacter[random.Next(spanishcharacter.Length - 1)]);
			}
			devicealert.GpsValid=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 127; i++)
			{
				builder.Append(spanishcharacter[random.Next(spanishcharacter.Length - 1)]);
			}
			devicealert.TagValue=builder.ToString();
			builder.Length=0;

			SingleMessage<DeviceAlert> ret = service.AddDeviceAlert(devicealert);
			//Assert.IsFalse(ret.IsSuccess);
		}


		/// <summary>
		/// 测试RuleId英文超长
		/// </summary>
		[TestMethod]
		public void TestRuleIdStringForMaxLength_English()
		{
			DeviceAlert devicealert = new DeviceAlert();
			DeviceAlertRespository service = new DeviceAlertRespository();
			StringBuilder builder = new StringBuilder();
			Random random = new Random();
			for (int i = 0; i < 9; i++)
			{
				builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
			}
			devicealert.Id=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 9; i++)
			{
				builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
			}
			devicealert.MdvrCoreId=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 17; i++)
			{
				builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
			}
			devicealert.SuiteInfoId=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 9; i++)
			{
				builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
			}
			devicealert.VehicleId=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 9; i++)
			{
				builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
			}
			devicealert.Cmd=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 9; i++)
			{
				builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
			}
			devicealert.Longitude=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 9; i++)
			{
				builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
			}
			devicealert.Latitude=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 9; i++)
			{
				builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
			}
			devicealert.Speed=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 9; i++)
			{
				builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
			}
			devicealert.Direction=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 0; i++)
			{
				builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
			}
			devicealert.GpsValid=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 127; i++)
			{
				builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
			}
			devicealert.TagValue=builder.ToString();
			builder.Length=0;

			SingleMessage<DeviceAlert> ret = service.AddDeviceAlert(devicealert);
			//Assert.IsFalse(ret.IsSuccess);
		}
        Random random=new Random();
		/// <summary>
		/// 测试空格
		/// </summary>
		[TestMethod]
		public void TestStringForSpace()
		{
			DeviceAlert devicealert = new DeviceAlert();
			DeviceAlertRespository service = new DeviceAlertRespository();
			StringBuilder builder = new StringBuilder();
			builder.AppendLine("			Random random = new Random();");
builder.Append(" ");
			for (int i = 1; i < 9; i++)
			{
				builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
			}
builder.Append(" ");
			devicealert.Id=builder.ToString();
			builder.Length=0;

builder.Append(" ");
			for (int i = 1; i < 9; i++)
			{
				builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
			}
builder.Append(" ");
			devicealert.MdvrCoreId=builder.ToString();
			builder.Length=0;

builder.Append(" ");
			for (int i = 1; i < 17; i++)
			{
				builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
			}
builder.Append(" ");
			devicealert.SuiteInfoId=builder.ToString();
			builder.Length=0;

builder.Append(" ");
			for (int i = 1; i < 9; i++)
			{
				builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
			}
builder.Append(" ");
			devicealert.VehicleId=builder.ToString();
			builder.Length=0;

builder.Append(" ");
			for (int i = 1; i < 9; i++)
			{
				builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
			}
builder.Append(" ");
			devicealert.Cmd=builder.ToString();
			builder.Length=0;

builder.Append(" ");
			for (int i = 1; i < 9; i++)
			{
				builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
			}
builder.Append(" ");
			devicealert.Longitude=builder.ToString();
			builder.Length=0;

builder.Append(" ");
			for (int i = 1; i < 9; i++)
			{
				builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
			}
builder.Append(" ");
			devicealert.Latitude=builder.ToString();
			builder.Length=0;

builder.Append(" ");
			for (int i = 1; i < 9; i++)
			{
				builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
			}
builder.Append(" ");
			devicealert.Speed=builder.ToString();
			builder.Length=0;

builder.Append(" ");
			for (int i = 1; i < 9; i++)
			{
				builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
			}
builder.Append(" ");
			devicealert.Direction=builder.ToString();
			builder.Length=0;

builder.Append(" ");
			for (int i = 1; i < 0; i++)
			{
				builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
			}
builder.Append(" ");
			devicealert.GpsValid=builder.ToString();
			builder.Length=0;

builder.Append(" ");
			for (int i = 1; i < 127; i++)
			{
				builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
			}
builder.Append(" ");
			devicealert.TagValue=builder.ToString();
			builder.Length=0;

			SingleMessage<DeviceAlert> ret = service.AddDeviceAlert(devicealert);
			//Assert.IsFalse(ret.IsSuccess);
		}


		/// <summary>
		/// 测试,
		/// </summary>
		[TestMethod]
		public void TestStringForSpecialCharacter_Comma()
		{
			DeviceAlert devicealert = new DeviceAlert();
			DeviceAlertRespository service = new DeviceAlertRespository();
			StringBuilder builder = new StringBuilder();
			builder.AppendLine("			Random random = new Random();");
builder.Append(",");
			for (int i = 1; i < 9; i++)
			{
				builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
			}
builder.Append(",");
			devicealert.Id=builder.ToString();
			builder.Length=0;

builder.Append(",");
			for (int i = 1; i < 9; i++)
			{
				builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
			}
builder.Append(",");
			devicealert.MdvrCoreId=builder.ToString();
			builder.Length=0;

builder.Append(",");
			for (int i = 1; i < 17; i++)
			{
				builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
			}
builder.Append(",");
			devicealert.SuiteInfoId=builder.ToString();
			builder.Length=0;

builder.Append(",");
			for (int i = 1; i < 9; i++)
			{
				builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
			}
builder.Append(",");
			devicealert.VehicleId=builder.ToString();
			builder.Length=0;

builder.Append(",");
			for (int i = 1; i < 9; i++)
			{
				builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
			}
builder.Append(",");
			devicealert.Cmd=builder.ToString();
			builder.Length=0;

builder.Append(",");
			for (int i = 1; i < 9; i++)
			{
				builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
			}
builder.Append(",");
			devicealert.Longitude=builder.ToString();
			builder.Length=0;

builder.Append(",");
			for (int i = 1; i < 9; i++)
			{
				builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
			}
builder.Append(",");
			devicealert.Latitude=builder.ToString();
			builder.Length=0;

builder.Append(",");
			for (int i = 1; i < 9; i++)
			{
				builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
			}
builder.Append(",");
			devicealert.Speed=builder.ToString();
			builder.Length=0;

builder.Append(",");
			for (int i = 1; i < 9; i++)
			{
				builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
			}
builder.Append(",");
			devicealert.Direction=builder.ToString();
			builder.Length=0;

builder.Append(",");
			for (int i = 1; i < 0; i++)
			{
				builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
			}
builder.Append(",");
			devicealert.GpsValid=builder.ToString();
			builder.Length=0;

builder.Append(",");
			for (int i = 1; i < 127; i++)
			{
				builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
			}
builder.Append(",");
			devicealert.TagValue=builder.ToString();
			builder.Length=0;

			SingleMessage<DeviceAlert> ret = service.AddDeviceAlert(devicealert);
			//Assert.IsFalse(ret.IsSuccess);
		}


		/// <summary>
		/// 测试;
		/// </summary>
		[TestMethod]
		public void TestStringForSpecialCharacter_SemiComma()
		{
			DeviceAlert devicealert = new DeviceAlert();
			DeviceAlertRespository service = new DeviceAlertRespository();
			StringBuilder builder = new StringBuilder();
			builder.AppendLine("			Random random = new Random();");
builder.Append(";");
			for (int i = 1; i < 9; i++)
			{
				builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
			}
builder.Append(";");
			devicealert.Id=builder.ToString();
			builder.Length=0;

builder.Append(";");
			for (int i = 1; i < 9; i++)
			{
				builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
			}
builder.Append(";");
			devicealert.MdvrCoreId=builder.ToString();
			builder.Length=0;

builder.Append(";");
			for (int i = 1; i < 17; i++)
			{
				builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
			}
builder.Append(";");
			devicealert.SuiteInfoId=builder.ToString();
			builder.Length=0;

builder.Append(";");
			for (int i = 1; i < 9; i++)
			{
				builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
			}
builder.Append(";");
			devicealert.VehicleId=builder.ToString();
			builder.Length=0;

builder.Append(";");
			for (int i = 1; i < 9; i++)
			{
				builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
			}
builder.Append(";");
			devicealert.Cmd=builder.ToString();
			builder.Length=0;

builder.Append(";");
			for (int i = 1; i < 9; i++)
			{
				builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
			}
builder.Append(";");
			devicealert.Longitude=builder.ToString();
			builder.Length=0;

builder.Append(";");
			for (int i = 1; i < 9; i++)
			{
				builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
			}
builder.Append(";");
			devicealert.Latitude=builder.ToString();
			builder.Length=0;

builder.Append(";");
			for (int i = 1; i < 9; i++)
			{
				builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
			}
builder.Append(";");
			devicealert.Speed=builder.ToString();
			builder.Length=0;

builder.Append(";");
			for (int i = 1; i < 9; i++)
			{
				builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
			}
builder.Append(";");
			devicealert.Direction=builder.ToString();
			builder.Length=0;

builder.Append(";");
			for (int i = 1; i < 0; i++)
			{
				builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
			}
builder.Append(";");
			devicealert.GpsValid=builder.ToString();
			builder.Length=0;

builder.Append(";");
			for (int i = 1; i < 127; i++)
			{
				builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
			}
builder.Append(";");
			devicealert.TagValue=builder.ToString();
			builder.Length=0;

			SingleMessage<DeviceAlert> ret = service.AddDeviceAlert(devicealert);
			//Assert.IsFalse(ret.IsSuccess);
		}


		/// <summary>
		/// 测试;
		/// </summary>
		[TestMethod]
		public void TestStringForSpecialCharacter()
		{
			DeviceAlert devicealert = new DeviceAlert();
			DeviceAlertRespository service = new DeviceAlertRespository();
			StringBuilder builder = new StringBuilder();
			builder.AppendLine("			Random random = new Random();");
builder.Append("\"");
			for (int i = 1; i < 9; i++)
			{
				builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
			}
builder.Append("\"");
			devicealert.Id=builder.ToString();
			builder.Length=0;

builder.Append("\"");
			for (int i = 1; i < 9; i++)
			{
				builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
			}
builder.Append("\"");
			devicealert.MdvrCoreId=builder.ToString();
			builder.Length=0;

builder.Append("\"");
			for (int i = 1; i < 17; i++)
			{
				builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
			}
builder.Append("\"");
			devicealert.SuiteInfoId=builder.ToString();
			builder.Length=0;

builder.Append("\"");
			for (int i = 1; i < 9; i++)
			{
				builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
			}
builder.Append("\"");
			devicealert.VehicleId=builder.ToString();
			builder.Length=0;

builder.Append("\"");
			for (int i = 1; i < 9; i++)
			{
				builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
			}
builder.Append("\"");
			devicealert.Cmd=builder.ToString();
			builder.Length=0;

builder.Append("\"");
			for (int i = 1; i < 9; i++)
			{
				builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
			}
builder.Append("\"");
			devicealert.Longitude=builder.ToString();
			builder.Length=0;

builder.Append("\"");
			for (int i = 1; i < 9; i++)
			{
				builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
			}
builder.Append("\"");
			devicealert.Latitude=builder.ToString();
			builder.Length=0;

builder.Append("\"");
			for (int i = 1; i < 9; i++)
			{
				builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
			}
builder.Append("\"");
			devicealert.Speed=builder.ToString();
			builder.Length=0;

builder.Append("\"");
			for (int i = 1; i < 9; i++)
			{
				builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
			}
builder.Append("\"");
			devicealert.Direction=builder.ToString();
			builder.Length=0;

builder.Append("\"");
			for (int i = 1; i < 0; i++)
			{
				builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
			}
builder.Append("\"");
			devicealert.GpsValid=builder.ToString();
			builder.Length=0;

builder.Append("\"");
			for (int i = 1; i < 127; i++)
			{
				builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
			}
builder.Append("\"");
			devicealert.TagValue=builder.ToString();
			builder.Length=0;

			SingleMessage<DeviceAlert> ret = service.AddDeviceAlert(devicealert);
			//Assert.IsFalse(ret.IsSuccess);
		}


		/// <summary>
		/// 测试select
		/// </summary>
		[TestMethod]
		public void TestStringForSpecialCharacterSelect()
		{
			DeviceAlert devicealert = new DeviceAlert();
			DeviceAlertRespository service = new DeviceAlertRespository();
			devicealert.Id="select";

			devicealert.MdvrCoreId="select";

			devicealert.SuiteInfoId="select";

			devicealert.VehicleId="select";

			devicealert.Cmd="select";

			devicealert.Longitude="select";

			devicealert.Latitude="select";

			devicealert.Speed="select";

			devicealert.Direction="select";

			devicealert.GpsValid="";

			devicealert.TagValue="select";

			SingleMessage<DeviceAlert> ret = service.AddDeviceAlert(devicealert);
			//Assert.IsFalse(ret.IsSuccess);
		}


		/// <summary>
		/// 测试update
		/// </summary>
		[TestMethod]
		public void TestStringForSpecialCharacterUpdate()
		{
			DeviceAlert devicealert = new DeviceAlert();
			DeviceAlertRespository service = new DeviceAlertRespository();
			devicealert.Id="update";

			devicealert.MdvrCoreId="update";

			devicealert.SuiteInfoId="update";

			devicealert.VehicleId="update";

			devicealert.Cmd="update";

			devicealert.Longitude="update";

			devicealert.Latitude="update";

			devicealert.Speed="update";

			devicealert.Direction="update";

			devicealert.GpsValid="";

			devicealert.TagValue="update";

			SingleMessage<DeviceAlert> ret = service.AddDeviceAlert(devicealert);
			//Assert.IsFalse(ret.IsSuccess);
		}


		/// <summary>
		/// 测试delete
		/// </summary>
		[TestMethod]
		public void TestStringForSpecialCharacterDelete()
		{
			DeviceAlert devicealert = new DeviceAlert();
			DeviceAlertRespository service = new DeviceAlertRespository();
			devicealert.Id="delete";

			devicealert.MdvrCoreId="delete";

			devicealert.SuiteInfoId="delete";

			devicealert.VehicleId="delete";

			devicealert.Cmd="delete";

			devicealert.Longitude="delete";

			devicealert.Latitude="delete";

			devicealert.Speed="delete";

			devicealert.Direction="delete";

			devicealert.GpsValid="";

			devicealert.TagValue="delete";

			SingleMessage<DeviceAlert> ret = service.AddDeviceAlert(devicealert);
			//Assert.IsFalse(ret.IsSuccess);
		}



		/// <summary>
		/// 正数
		/// </summary>
		[TestMethod]
		public void TestNumberForPositive()
		{
			DeviceAlert devicealert = new DeviceAlert();
			DeviceAlertRespository service = new DeviceAlertRespository();
			devicealert.Id="";
			devicealert.MdvrCoreId="";
			devicealert.SuiteInfoId="";
			devicealert.VehicleId="";
			devicealert.SuiteStatus=1;
			devicealert.Cmd="";
			devicealert.Longitude="";
			devicealert.Latitude="";
			devicealert.Speed="";
			devicealert.Direction="";
			devicealert.GpsValid="";
			devicealert.TagValue="";
			devicealert.Status=1;
			SingleMessage<DeviceAlert> ret = service.AddDeviceAlert(devicealert);
			//Assert.IsFalse(ret.IsSuccess);
		}


		/// <summary>
		/// 负数
		/// </summary>
		[TestMethod]
		public void TestNumberForNegative()
		{
			DeviceAlert devicealert = new DeviceAlert();
			DeviceAlertRespository service = new DeviceAlertRespository();
			devicealert.Id="";
			devicealert.MdvrCoreId="";
			devicealert.SuiteInfoId="";
			devicealert.VehicleId="";
			devicealert.SuiteStatus=-1;
			devicealert.Cmd="";
			devicealert.Longitude="";
			devicealert.Latitude="";
			devicealert.Speed="";
			devicealert.Direction="";
			devicealert.GpsValid="";
			devicealert.TagValue="";
			devicealert.Status=-1;
			SingleMessage<DeviceAlert> ret = service.AddDeviceAlert(devicealert);
			//Assert.IsFalse(ret.IsSuccess);
		}


		/// <summary>
		/// 零
		/// </summary>
		[TestMethod]
		public void TestNumberForZero()
		{
			DeviceAlert devicealert = new DeviceAlert();
			DeviceAlertRespository service = new DeviceAlertRespository();
			devicealert.Id="";
			devicealert.MdvrCoreId="";
			devicealert.SuiteInfoId="";
			devicealert.VehicleId="";
			devicealert.SuiteStatus=0;
			devicealert.Cmd="";
			devicealert.Longitude="";
			devicealert.Latitude="";
			devicealert.Speed="";
			devicealert.Direction="";
			devicealert.GpsValid="";
			devicealert.TagValue="";
			devicealert.Status=0;
			SingleMessage<DeviceAlert> ret = service.AddDeviceAlert(devicealert);
			//Assert.IsFalse(ret.IsSuccess);
		}





		/// <summary>
		/// DateTime
		/// </summary>
		[TestMethod]
		public void TestDateTime()
		{
			DeviceAlert devicealert = new DeviceAlert();
			DeviceAlertRespository service = new DeviceAlertRespository();
			devicealert.Id="";
			devicealert.MdvrCoreId="";
			devicealert.SuiteInfoId="";
			devicealert.VehicleId="";
			devicealert.Cmd="";
			devicealert.Longitude="";
			devicealert.Latitude="";
			devicealert.Speed="";
			devicealert.Direction="";
			devicealert.GpsValid="";
			devicealert.TagValue="";
			SingleMessage<DeviceAlert> ret = service.AddDeviceAlert(devicealert);
			//Assert.IsFalse(ret.IsSuccess);
		}


		/// <summary>
		/// 日期为空
		/// </summary>
		[TestMethod]
		public void TestIdDateTimeForNull()
		{
			DeviceAlert devicealert = new DeviceAlert();
			DeviceAlertRespository service = new DeviceAlertRespository();
			devicealert.Id=null;
			devicealert.MdvrCoreId="";
			devicealert.SuiteInfoId="";
			devicealert.VehicleId="";
			devicealert.Cmd="";
			devicealert.Longitude="";
			devicealert.Latitude="";
			devicealert.Speed="";
			devicealert.Direction="";
			devicealert.GpsValid="";
			devicealert.TagValue="";
			SingleMessage<DeviceAlert> ret = service.AddDeviceAlert(devicealert);
			//Assert.IsFalse(ret.IsSuccess);
		}


		/// <summary>
		/// 日期为空
		/// </summary>
		[TestMethod]
		public void TestMdvrCoreIdDateTimeForNull()
		{
			DeviceAlert devicealert = new DeviceAlert();
			DeviceAlertRespository service = new DeviceAlertRespository();
			devicealert.Id="";
			devicealert.MdvrCoreId=null;
			devicealert.SuiteInfoId="";
			devicealert.VehicleId="";
			devicealert.Cmd="";
			devicealert.Longitude="";
			devicealert.Latitude="";
			devicealert.Speed="";
			devicealert.Direction="";
			devicealert.GpsValid="";
			devicealert.TagValue="";
			SingleMessage<DeviceAlert> ret = service.AddDeviceAlert(devicealert);
			//Assert.IsFalse(ret.IsSuccess);
		}


		/// <summary>
		/// 日期为空
		/// </summary>
		[TestMethod]
		public void TestSuiteIdDateTimeForNull()
		{
			DeviceAlert devicealert = new DeviceAlert();
			DeviceAlertRespository service = new DeviceAlertRespository();
			devicealert.Id="";
			devicealert.MdvrCoreId="";
			devicealert.SuiteId=null;
			devicealert.SuiteInfoId="";
			devicealert.VehicleId="";
			devicealert.Cmd="";
			devicealert.Longitude="";
			devicealert.Latitude="";
			devicealert.Speed="";
			devicealert.Direction="";
			devicealert.GpsValid="";
			devicealert.TagValue="";
			SingleMessage<DeviceAlert> ret = service.AddDeviceAlert(devicealert);
			//Assert.IsFalse(ret.IsSuccess);
		}


		/// <summary>
		/// 日期为空
		/// </summary>
		[TestMethod]
		public void TestSuiteInfoIdDateTimeForNull()
		{
			DeviceAlert devicealert = new DeviceAlert();
			DeviceAlertRespository service = new DeviceAlertRespository();
			devicealert.Id="";
			devicealert.MdvrCoreId="";
			devicealert.SuiteInfoId=null;
			devicealert.VehicleId="";
			devicealert.Cmd="";
			devicealert.Longitude="";
			devicealert.Latitude="";
			devicealert.Speed="";
			devicealert.Direction="";
			devicealert.GpsValid="";
			devicealert.TagValue="";
			SingleMessage<DeviceAlert> ret = service.AddDeviceAlert(devicealert);
			//Assert.IsFalse(ret.IsSuccess);
		}


		/// <summary>
		/// 日期为空
		/// </summary>
		[TestMethod]
		public void TestVehicleIdDateTimeForNull()
		{
			DeviceAlert devicealert = new DeviceAlert();
			DeviceAlertRespository service = new DeviceAlertRespository();
			devicealert.Id="";
			devicealert.MdvrCoreId="";
			devicealert.SuiteInfoId="";
			devicealert.VehicleId=null;
			devicealert.Cmd="";
			devicealert.Longitude="";
			devicealert.Latitude="";
			devicealert.Speed="";
			devicealert.Direction="";
			devicealert.GpsValid="";
			devicealert.TagValue="";
			SingleMessage<DeviceAlert> ret = service.AddDeviceAlert(devicealert);
			//Assert.IsFalse(ret.IsSuccess);
		}


		/// <summary>
		/// 日期为空
		/// </summary>
		[TestMethod]
		public void TestSuiteStatusDateTimeForNull()
		{
			DeviceAlert devicealert = new DeviceAlert();
			DeviceAlertRespository service = new DeviceAlertRespository();
			devicealert.Id="";
			devicealert.MdvrCoreId="";
			devicealert.SuiteInfoId="";
			devicealert.VehicleId="";
			devicealert.SuiteStatus=null;
			devicealert.Cmd="";
			devicealert.Longitude="";
			devicealert.Latitude="";
			devicealert.Speed="";
			devicealert.Direction="";
			devicealert.GpsValid="";
			devicealert.TagValue="";
			SingleMessage<DeviceAlert> ret = service.AddDeviceAlert(devicealert);
			//Assert.IsFalse(ret.IsSuccess);
		}


		/// <summary>
		/// 日期为空
		/// </summary>
		[TestMethod]
		public void TestAlertTypeDateTimeForNull()
		{
			DeviceAlert devicealert = new DeviceAlert();
			DeviceAlertRespository service = new DeviceAlertRespository();
			devicealert.Id="";
			devicealert.MdvrCoreId="";
			devicealert.SuiteInfoId="";
			devicealert.VehicleId="";
			devicealert.AlertType=null;
			devicealert.Cmd="";
			devicealert.Longitude="";
			devicealert.Latitude="";
			devicealert.Speed="";
			devicealert.Direction="";
			devicealert.GpsValid="";
			devicealert.TagValue="";
			SingleMessage<DeviceAlert> ret = service.AddDeviceAlert(devicealert);
			//Assert.IsFalse(ret.IsSuccess);
		}


		/// <summary>
		/// 日期为空
		/// </summary>
		[TestMethod]
		public void TestAlertTypeNameDateTimeForNull()
		{
			DeviceAlert devicealert = new DeviceAlert();
			DeviceAlertRespository service = new DeviceAlertRespository();
			devicealert.Id="";
			devicealert.MdvrCoreId="";
			devicealert.SuiteInfoId="";
			devicealert.VehicleId="";
			devicealert.AlertTypeName=null;
			devicealert.Cmd="";
			devicealert.Longitude="";
			devicealert.Latitude="";
			devicealert.Speed="";
			devicealert.Direction="";
			devicealert.GpsValid="";
			devicealert.TagValue="";
			SingleMessage<DeviceAlert> ret = service.AddDeviceAlert(devicealert);
			//Assert.IsFalse(ret.IsSuccess);
		}


		/// <summary>
		/// 日期为空
		/// </summary>
		[TestMethod]
		public void TestAlertTimeDateTimeForNull()
		{
			DeviceAlert devicealert = new DeviceAlert();
			DeviceAlertRespository service = new DeviceAlertRespository();
			devicealert.Id="";
			devicealert.MdvrCoreId="";
			devicealert.SuiteInfoId="";
			devicealert.VehicleId="";
			devicealert.AlertTime=null;
			devicealert.Cmd="";
			devicealert.Longitude="";
			devicealert.Latitude="";
			devicealert.Speed="";
			devicealert.Direction="";
			devicealert.GpsValid="";
			devicealert.TagValue="";
			SingleMessage<DeviceAlert> ret = service.AddDeviceAlert(devicealert);
			//Assert.IsFalse(ret.IsSuccess);
		}


		/// <summary>
		/// 日期为空
		/// </summary>
		[TestMethod]
		public void TestCmdDateTimeForNull()
		{
			DeviceAlert devicealert = new DeviceAlert();
			DeviceAlertRespository service = new DeviceAlertRespository();
			devicealert.Id="";
			devicealert.MdvrCoreId="";
			devicealert.SuiteInfoId="";
			devicealert.VehicleId="";
			devicealert.Cmd=null;
			devicealert.Longitude="";
			devicealert.Latitude="";
			devicealert.Speed="";
			devicealert.Direction="";
			devicealert.GpsValid="";
			devicealert.TagValue="";
			SingleMessage<DeviceAlert> ret = service.AddDeviceAlert(devicealert);
			//Assert.IsFalse(ret.IsSuccess);
		}


		/// <summary>
		/// 日期为空
		/// </summary>
		[TestMethod]
		public void TestLongitudeDateTimeForNull()
		{
			DeviceAlert devicealert = new DeviceAlert();
			DeviceAlertRespository service = new DeviceAlertRespository();
			devicealert.Id="";
			devicealert.MdvrCoreId="";
			devicealert.SuiteInfoId="";
			devicealert.VehicleId="";
			devicealert.Cmd="";
			devicealert.Longitude=null;
			devicealert.Latitude="";
			devicealert.Speed="";
			devicealert.Direction="";
			devicealert.GpsValid="";
			devicealert.TagValue="";
			SingleMessage<DeviceAlert> ret = service.AddDeviceAlert(devicealert);
			//Assert.IsFalse(ret.IsSuccess);
		}


		/// <summary>
		/// 日期为空
		/// </summary>
		[TestMethod]
		public void TestLatitudeDateTimeForNull()
		{
			DeviceAlert devicealert = new DeviceAlert();
			DeviceAlertRespository service = new DeviceAlertRespository();
			devicealert.Id="";
			devicealert.MdvrCoreId="";
			devicealert.SuiteInfoId="";
			devicealert.VehicleId="";
			devicealert.Cmd="";
			devicealert.Longitude="";
			devicealert.Latitude=null;
			devicealert.Speed="";
			devicealert.Direction="";
			devicealert.GpsValid="";
			devicealert.TagValue="";
			SingleMessage<DeviceAlert> ret = service.AddDeviceAlert(devicealert);
			//Assert.IsFalse(ret.IsSuccess);
		}


		/// <summary>
		/// 日期为空
		/// </summary>
		[TestMethod]
		public void TestGpsTimeDateTimeForNull()
		{
			DeviceAlert devicealert = new DeviceAlert();
			DeviceAlertRespository service = new DeviceAlertRespository();
			devicealert.Id="";
			devicealert.MdvrCoreId="";
			devicealert.SuiteInfoId="";
			devicealert.VehicleId="";
			devicealert.Cmd="";
			devicealert.Longitude="";
			devicealert.Latitude="";
			devicealert.GpsTime=null;
			devicealert.Speed="";
			devicealert.Direction="";
			devicealert.GpsValid="";
			devicealert.TagValue="";
			SingleMessage<DeviceAlert> ret = service.AddDeviceAlert(devicealert);
			//Assert.IsFalse(ret.IsSuccess);
		}


		/// <summary>
		/// 日期为空
		/// </summary>
		[TestMethod]
		public void TestSpeedDateTimeForNull()
		{
			DeviceAlert devicealert = new DeviceAlert();
			DeviceAlertRespository service = new DeviceAlertRespository();
			devicealert.Id="";
			devicealert.MdvrCoreId="";
			devicealert.SuiteInfoId="";
			devicealert.VehicleId="";
			devicealert.Cmd="";
			devicealert.Longitude="";
			devicealert.Latitude="";
			devicealert.Speed=null;
			devicealert.Direction="";
			devicealert.GpsValid="";
			devicealert.TagValue="";
			SingleMessage<DeviceAlert> ret = service.AddDeviceAlert(devicealert);
			//Assert.IsFalse(ret.IsSuccess);
		}


		/// <summary>
		/// 日期为空
		/// </summary>
		[TestMethod]
		public void TestDirectionDateTimeForNull()
		{
			DeviceAlert devicealert = new DeviceAlert();
			DeviceAlertRespository service = new DeviceAlertRespository();
			devicealert.Id="";
			devicealert.MdvrCoreId="";
			devicealert.SuiteInfoId="";
			devicealert.VehicleId="";
			devicealert.Cmd="";
			devicealert.Longitude="";
			devicealert.Latitude="";
			devicealert.Speed="";
			devicealert.Direction=null;
			devicealert.GpsValid="";
			devicealert.TagValue="";
			SingleMessage<DeviceAlert> ret = service.AddDeviceAlert(devicealert);
			//Assert.IsFalse(ret.IsSuccess);
		}


		/// <summary>
		/// 日期为空
		/// </summary>
		[TestMethod]
		public void TestGpsValidDateTimeForNull()
		{
			DeviceAlert devicealert = new DeviceAlert();
			DeviceAlertRespository service = new DeviceAlertRespository();
			devicealert.Id="";
			devicealert.MdvrCoreId="";
			devicealert.SuiteInfoId="";
			devicealert.VehicleId="";
			devicealert.Cmd="";
			devicealert.Longitude="";
			devicealert.Latitude="";
			devicealert.Speed="";
			devicealert.Direction="";
			devicealert.GpsValid=null;
			devicealert.TagValue="";
			SingleMessage<DeviceAlert> ret = service.AddDeviceAlert(devicealert);
			//Assert.IsFalse(ret.IsSuccess);
		}


		/// <summary>
		/// 日期为空
		/// </summary>
		[TestMethod]
		public void TestTagValueDateTimeForNull()
		{
			DeviceAlert devicealert = new DeviceAlert();
			DeviceAlertRespository service = new DeviceAlertRespository();
			devicealert.Id="";
			devicealert.MdvrCoreId="";
			devicealert.SuiteInfoId="";
			devicealert.VehicleId="";
			devicealert.Cmd="";
			devicealert.Longitude="";
			devicealert.Latitude="";
			devicealert.Speed="";
			devicealert.Direction="";
			devicealert.GpsValid="";
			devicealert.TagValue=null;
			SingleMessage<DeviceAlert> ret = service.AddDeviceAlert(devicealert);
			//Assert.IsFalse(ret.IsSuccess);
		}


		/// <summary>
		/// 日期为空
		/// </summary>
		[TestMethod]
		public void TestStatusDateTimeForNull()
		{
			DeviceAlert devicealert = new DeviceAlert();
			DeviceAlertRespository service = new DeviceAlertRespository();
			devicealert.Id="";
			devicealert.MdvrCoreId="";
			devicealert.SuiteInfoId="";
			devicealert.VehicleId="";
			devicealert.Cmd="";
			devicealert.Longitude="";
			devicealert.Latitude="";
			devicealert.Speed="";
			devicealert.Direction="";
			devicealert.GpsValid="";
			devicealert.TagValue="";
			devicealert.Status=null;
			SingleMessage<DeviceAlert> ret = service.AddDeviceAlert(devicealert);
			//Assert.IsFalse(ret.IsSuccess);
		}


		/// <summary>
		/// 日期为空
		/// </summary>
		[TestMethod]
		public void TestHandleIdDateTimeForNull()
		{
			DeviceAlert devicealert = new DeviceAlert();
			DeviceAlertRespository service = new DeviceAlertRespository();
			devicealert.Id="";
			devicealert.MdvrCoreId="";
			devicealert.SuiteInfoId="";
			devicealert.VehicleId="";
			devicealert.Cmd="";
			devicealert.Longitude="";
			devicealert.Latitude="";
			devicealert.Speed="";
			devicealert.Direction="";
			devicealert.GpsValid="";
			devicealert.TagValue="";
			devicealert.HandleId=null;
			SingleMessage<DeviceAlert> ret = service.AddDeviceAlert(devicealert);
			//Assert.IsFalse(ret.IsSuccess);
		}


		/// <summary>
		/// 日期为空
		/// </summary>
		[TestMethod]
		public void TestRuleIdDateTimeForNull()
		{
			DeviceAlert devicealert = new DeviceAlert();
			DeviceAlertRespository service = new DeviceAlertRespository();
			devicealert.Id="";
			devicealert.MdvrCoreId="";
			devicealert.SuiteInfoId="";
			devicealert.VehicleId="";
			devicealert.Cmd="";
			devicealert.Longitude="";
			devicealert.Latitude="";
			devicealert.Speed="";
			devicealert.Direction="";
			devicealert.GpsValid="";
			devicealert.TagValue="";
			devicealert.RuleId=0;
			SingleMessage<DeviceAlert> ret = service.AddDeviceAlert(devicealert);
			//Assert.IsFalse(ret.IsSuccess);
		}


		/// <summary>
		/// Id日期为空
		/// </summary>
		[TestMethod]
		public void TestIdDateTimeForIncomplete()
		{
			DeviceAlert devicealert = new DeviceAlert();
			DeviceAlertRespository service = new DeviceAlertRespository();
			devicealert.Id=DateTime.Parse("2015/02/12").ToString();
			devicealert.MdvrCoreId="";
			devicealert.SuiteInfoId="";
			devicealert.VehicleId="";
			devicealert.Cmd="";
			devicealert.Longitude="";
			devicealert.Latitude="";
			devicealert.Speed="";
			devicealert.Direction="";
			devicealert.GpsValid="";
			devicealert.TagValue="";
			SingleMessage<DeviceAlert> ret = service.AddDeviceAlert(devicealert);
			//Assert.IsFalse(ret.IsSuccess);
		}


		/// <summary>
		/// MdvrCoreId日期为空
		/// </summary>
		[TestMethod]
		public void TestMdvrCoreIdDateTimeForIncomplete()
		{
			DeviceAlert devicealert = new DeviceAlert();
			DeviceAlertRespository service = new DeviceAlertRespository();
			devicealert.Id="";
			devicealert.MdvrCoreId=DateTime.Parse("2015/02/12").ToString();
			devicealert.SuiteInfoId="";
			devicealert.VehicleId="";
			devicealert.Cmd="";
			devicealert.Longitude="";
			devicealert.Latitude="";
			devicealert.Speed="";
			devicealert.Direction="";
			devicealert.GpsValid="";
			devicealert.TagValue="";
			SingleMessage<DeviceAlert> ret = service.AddDeviceAlert(devicealert);
			//Assert.IsFalse(ret.IsSuccess);
		}


		/// <summary>
		/// SuiteId日期为空
		/// </summary>
		[TestMethod]
		public void TestSuiteIdDateTimeForIncomplete()
		{
			DeviceAlert devicealert = new DeviceAlert();
			DeviceAlertRespository service = new DeviceAlertRespository();
			devicealert.Id="";
			devicealert.MdvrCoreId="";
			devicealert.SuiteId=DateTime.Parse("2015/02/12").ToString();
			devicealert.SuiteInfoId="";
			devicealert.VehicleId="";
			devicealert.Cmd="";
			devicealert.Longitude="";
			devicealert.Latitude="";
			devicealert.Speed="";
			devicealert.Direction="";
			devicealert.GpsValid="";
			devicealert.TagValue="";
			SingleMessage<DeviceAlert> ret = service.AddDeviceAlert(devicealert);
			//Assert.IsFalse(ret.IsSuccess);
		}


		/// <summary>
		/// SuiteInfoId日期为空
		/// </summary>
		[TestMethod]
		public void TestSuiteInfoIdDateTimeForIncomplete()
		{
			DeviceAlert devicealert = new DeviceAlert();
			DeviceAlertRespository service = new DeviceAlertRespository();
			devicealert.Id="";
			devicealert.MdvrCoreId="";
			devicealert.SuiteInfoId=DateTime.Parse("2015/02/12").ToString();
			devicealert.VehicleId="";
			devicealert.Cmd="";
			devicealert.Longitude="";
			devicealert.Latitude="";
			devicealert.Speed="";
			devicealert.Direction="";
			devicealert.GpsValid="";
			devicealert.TagValue="";
			SingleMessage<DeviceAlert> ret = service.AddDeviceAlert(devicealert);
			//Assert.IsFalse(ret.IsSuccess);
		}


		/// <summary>
		/// VehicleId日期为空
		/// </summary>
		[TestMethod]
		public void TestVehicleIdDateTimeForIncomplete()
		{
			DeviceAlert devicealert = new DeviceAlert();
			DeviceAlertRespository service = new DeviceAlertRespository();
			devicealert.Id="";
			devicealert.MdvrCoreId="";
			devicealert.SuiteInfoId="";
			devicealert.VehicleId=DateTime.Parse("2015/02/12").ToString();
			devicealert.Cmd="";
			devicealert.Longitude="";
			devicealert.Latitude="";
			devicealert.Speed="";
			devicealert.Direction="";
			devicealert.GpsValid="";
			devicealert.TagValue="";
			SingleMessage<DeviceAlert> ret = service.AddDeviceAlert(devicealert);
			//Assert.IsFalse(ret.IsSuccess);
		}



		/// <summary>
		/// AlertTypeName日期为空
		/// </summary>
		[TestMethod]
		public void TestAlertTypeNameDateTimeForIncomplete()
		{
			DeviceAlert devicealert = new DeviceAlert();
			DeviceAlertRespository service = new DeviceAlertRespository();
			devicealert.Id="";
			devicealert.MdvrCoreId="";
			devicealert.SuiteInfoId="";
			devicealert.VehicleId="";
			devicealert.AlertTypeName=DateTime.Parse("2015/02/12").ToString();
			devicealert.Cmd="";
			devicealert.Longitude="";
			devicealert.Latitude="";
			devicealert.Speed="";
			devicealert.Direction="";
			devicealert.GpsValid="";
			devicealert.TagValue="";
			SingleMessage<DeviceAlert> ret = service.AddDeviceAlert(devicealert);
			//Assert.IsFalse(ret.IsSuccess);
		}


		/// <summary>
		/// AlertTime日期为空
		/// </summary>
		[TestMethod]
		public void TestAlertTimeDateTimeForIncomplete()
		{
			DeviceAlert devicealert = new DeviceAlert();
			DeviceAlertRespository service = new DeviceAlertRespository();
			devicealert.Id="";
			devicealert.MdvrCoreId="";
			devicealert.SuiteInfoId="";
			devicealert.VehicleId="";
			devicealert.AlertTime=DateTime.Parse("2015/02/12");
			devicealert.Cmd="";
			devicealert.Longitude="";
			devicealert.Latitude="";
			devicealert.Speed="";
			devicealert.Direction="";
			devicealert.GpsValid="";
			devicealert.TagValue="";
			SingleMessage<DeviceAlert> ret = service.AddDeviceAlert(devicealert);
			//Assert.IsFalse(ret.IsSuccess);
		}


		/// <summary>
		/// Cmd日期为空
		/// </summary>
		[TestMethod]
		public void TestCmdDateTimeForIncomplete()
		{
			DeviceAlert devicealert = new DeviceAlert();
			DeviceAlertRespository service = new DeviceAlertRespository();
			devicealert.Id="";
			devicealert.MdvrCoreId="";
			devicealert.SuiteInfoId="";
			devicealert.VehicleId="";
			devicealert.Cmd=DateTime.Parse("2015/02/12").ToLongDateString();
			devicealert.Longitude="";
			devicealert.Latitude="";
			devicealert.Speed="";
			devicealert.Direction="";
			devicealert.GpsValid="";
			devicealert.TagValue="";
			SingleMessage<DeviceAlert> ret = service.AddDeviceAlert(devicealert);
			//Assert.IsFalse(ret.IsSuccess);
		}


		/// <summary>
		/// Longitude日期为空
		/// </summary>
		[TestMethod]
		public void TestLongitudeDateTimeForIncomplete()
		{
			DeviceAlert devicealert = new DeviceAlert();
			DeviceAlertRespository service = new DeviceAlertRespository();
			devicealert.Id="";
			devicealert.MdvrCoreId="";
			devicealert.SuiteInfoId="";
			devicealert.VehicleId="";
			devicealert.Cmd="";
			devicealert.Longitude=DateTime.Parse("2015/02/12").ToLongDateString();
			devicealert.Latitude="";
			devicealert.Speed="";
			devicealert.Direction="";
			devicealert.GpsValid="";
			devicealert.TagValue="";
			SingleMessage<DeviceAlert> ret = service.AddDeviceAlert(devicealert);
			//Assert.IsFalse(ret.IsSuccess);
		}


		/// <summary>
		/// Latitude日期为空
		/// </summary>
		[TestMethod]
		public void TestLatitudeDateTimeForIncomplete()
		{
			DeviceAlert devicealert = new DeviceAlert();
			DeviceAlertRespository service = new DeviceAlertRespository();
			devicealert.Id="";
			devicealert.MdvrCoreId="";
			devicealert.SuiteInfoId="";
			devicealert.VehicleId="";
			devicealert.Cmd="";
			devicealert.Longitude="";
			devicealert.Latitude=DateTime.Parse("2015/02/12").ToString();
			devicealert.Speed="";
			devicealert.Direction="";
			devicealert.GpsValid="";
			devicealert.TagValue="";
			SingleMessage<DeviceAlert> ret = service.AddDeviceAlert(devicealert);
			//Assert.IsFalse(ret.IsSuccess);
		}


		/// <summary>
		/// GpsTime日期为空
		/// </summary>
		[TestMethod]
		public void TestGpsTimeDateTimeForIncomplete()
		{
			DeviceAlert devicealert = new DeviceAlert();
			DeviceAlertRespository service = new DeviceAlertRespository();
			devicealert.Id="";
			devicealert.MdvrCoreId="";
			devicealert.SuiteInfoId="";
			devicealert.VehicleId="";
			devicealert.Cmd="";
			devicealert.Longitude="";
			devicealert.Latitude="";
			devicealert.GpsTime=DateTime.Parse("2015/02/12");
			devicealert.Speed="";
			devicealert.Direction="";
			devicealert.GpsValid="";
			devicealert.TagValue="";
			SingleMessage<DeviceAlert> ret = service.AddDeviceAlert(devicealert);
			//Assert.IsFalse(ret.IsSuccess);
		}


		/// <summary>
		/// Speed日期为空
		/// </summary>
		[TestMethod]
		public void TestSpeedDateTimeForIncomplete()
		{
			DeviceAlert devicealert = new DeviceAlert();
			DeviceAlertRespository service = new DeviceAlertRespository();
			devicealert.Id="";
			devicealert.MdvrCoreId="";
			devicealert.SuiteInfoId="";
			devicealert.VehicleId="";
			devicealert.Cmd="";
			devicealert.Longitude="";
			devicealert.Latitude="";
			devicealert.Speed=DateTime.Parse("2015/02/12").ToString();
			devicealert.Direction="";
			devicealert.GpsValid="";
			devicealert.TagValue="";
			SingleMessage<DeviceAlert> ret = service.AddDeviceAlert(devicealert);
			//Assert.IsFalse(ret.IsSuccess);
		}


		/// <summary>
		/// Direction日期为空
		/// </summary>
		[TestMethod]
		public void TestDirectionDateTimeForIncomplete()
		{
			DeviceAlert devicealert = new DeviceAlert();
			DeviceAlertRespository service = new DeviceAlertRespository();
			devicealert.Id="";
			devicealert.MdvrCoreId="";
			devicealert.SuiteInfoId="";
			devicealert.VehicleId="";
			devicealert.Cmd="";
			devicealert.Longitude="";
			devicealert.Latitude="";
			devicealert.Speed="";
			devicealert.Direction=DateTime.Parse("2015/02/12").ToString();
			devicealert.GpsValid="";
			devicealert.TagValue="";
			SingleMessage<DeviceAlert> ret = service.AddDeviceAlert(devicealert);
			//Assert.IsFalse(ret.IsSuccess);
		}


		/// <summary>
		/// GpsValid日期为空
		/// </summary>
		[TestMethod]
		public void TestGpsValidDateTimeForIncomplete()
		{
			DeviceAlert devicealert = new DeviceAlert();
			DeviceAlertRespository service = new DeviceAlertRespository();
			devicealert.Id="";
			devicealert.MdvrCoreId="";
			devicealert.SuiteInfoId="";
			devicealert.VehicleId="";
			devicealert.Cmd="";
			devicealert.Longitude="";
			devicealert.Latitude="";
			devicealert.Speed="";
			devicealert.Direction="";
			devicealert.GpsValid=DateTime.Parse("2015/02/12").ToString();
			devicealert.TagValue="";
			SingleMessage<DeviceAlert> ret = service.AddDeviceAlert(devicealert);
			//Assert.IsFalse(ret.IsSuccess);
		}


		/// <summary>
		/// TagValue日期为空
		/// </summary>
		[TestMethod]
		public void TestTagValueDateTimeForIncomplete()
		{
			DeviceAlert devicealert = new DeviceAlert();
			DeviceAlertRespository service = new DeviceAlertRespository();
			devicealert.Id="";
			devicealert.MdvrCoreId="";
			devicealert.SuiteInfoId="";
			devicealert.VehicleId="";
			devicealert.Cmd="";
			devicealert.Longitude="";
			devicealert.Latitude="";
			devicealert.Speed="";
			devicealert.Direction="";
			devicealert.GpsValid="";
			devicealert.TagValue=DateTime.Parse("2015/02/12").ToString();
			SingleMessage<DeviceAlert> ret = service.AddDeviceAlert(devicealert);
			//Assert.IsFalse(ret.IsSuccess);
		}




		/// <summary>
		/// HandleId日期为空
		/// </summary>
		[TestMethod]
		public void TestHandleIdDateTimeForIncomplete()
		{
			DeviceAlert devicealert = new DeviceAlert();
			DeviceAlertRespository service = new DeviceAlertRespository();
			devicealert.Id="";
			devicealert.MdvrCoreId="";
			devicealert.SuiteInfoId="";
			devicealert.VehicleId="";
			devicealert.Cmd="";
			devicealert.Longitude="";
			devicealert.Latitude="";
			devicealert.Speed="";
			devicealert.Direction="";
			devicealert.GpsValid="";
			devicealert.TagValue="";
			devicealert.HandleId=DateTime.Parse("2015/02/12").ToString();
			SingleMessage<DeviceAlert> ret = service.AddDeviceAlert(devicealert);
			//Assert.IsFalse(ret.IsSuccess);
		}


		/// <summary>
		/// RuleId日期为空
		/// </summary>
		[TestMethod]
		public void TestRuleIdDateTimeForIncomplete()
		{
			DeviceAlert devicealert = new DeviceAlert();
			DeviceAlertRespository service = new DeviceAlertRespository();
			devicealert.Id="";
			devicealert.MdvrCoreId="";
			devicealert.SuiteInfoId="";
			devicealert.VehicleId="";
			devicealert.Cmd="";
			devicealert.Longitude="";
			devicealert.Latitude="";
			devicealert.Speed="";
			devicealert.Direction="";
			devicealert.GpsValid="";
			devicealert.TagValue="";
			devicealert.RuleId=DateTime.Parse("2015/02/12").Day;
			SingleMessage<DeviceAlert> ret = service.AddDeviceAlert(devicealert);
			//Assert.IsFalse(ret.IsSuccess);
		}


		/// <summary>
		/// 测试Id日期格式错误
		/// </summary>
		[TestMethod]
		public void TestIdDateTimeForFormatError()
		{
			DeviceAlert devicealert = new DeviceAlert();
			DeviceAlertRespository service = new DeviceAlertRespository();
			devicealert.Id="";
			devicealert.MdvrCoreId="";
			devicealert.SuiteInfoId="";
			devicealert.VehicleId="";
			devicealert.Cmd="";
			devicealert.Longitude="";
			devicealert.Latitude="";
			devicealert.Speed="";
			devicealert.Direction="";
			devicealert.GpsValid="";
			devicealert.TagValue="";
			SingleMessage<DeviceAlert> ret = service.AddDeviceAlert(devicealert);
			//Assert.IsFalse(ret.IsSuccess);
		}


		/// <summary>
		/// 测试MdvrCoreId日期格式错误
		/// </summary>
		[TestMethod]
		public void TestMdvrCoreIdDateTimeForFormatError()
		{
			DeviceAlert devicealert = new DeviceAlert();
			DeviceAlertRespository service = new DeviceAlertRespository();
			devicealert.Id="";
			devicealert.MdvrCoreId="";
			devicealert.SuiteInfoId="";
			devicealert.VehicleId="";
			devicealert.Cmd="";
			devicealert.Longitude="";
			devicealert.Latitude="";
			devicealert.Speed="";
			devicealert.Direction="";
			devicealert.GpsValid="";
			devicealert.TagValue="";
			SingleMessage<DeviceAlert> ret = service.AddDeviceAlert(devicealert);
			//Assert.IsFalse(ret.IsSuccess);
		}


		/// <summary>
		/// 测试SuiteId日期格式错误
		/// </summary>
		[TestMethod]
		public void TestSuiteIdDateTimeForFormatError()
		{
			DeviceAlert devicealert = new DeviceAlert();
			DeviceAlertRespository service = new DeviceAlertRespository();
			devicealert.Id="";
			devicealert.MdvrCoreId="";
			devicealert.SuiteInfoId="";
			devicealert.VehicleId="";
			devicealert.Cmd="";
			devicealert.Longitude="";
			devicealert.Latitude="";
			devicealert.Speed="";
			devicealert.Direction="";
			devicealert.GpsValid="";
			devicealert.TagValue="";
			SingleMessage<DeviceAlert> ret = service.AddDeviceAlert(devicealert);
			//Assert.IsFalse(ret.IsSuccess);
		}


		/// <summary>
		/// 测试SuiteInfoId日期格式错误
		/// </summary>
		[TestMethod]
		public void TestSuiteInfoIdDateTimeForFormatError()
		{
			DeviceAlert devicealert = new DeviceAlert();
			DeviceAlertRespository service = new DeviceAlertRespository();
			devicealert.Id="";
			devicealert.MdvrCoreId="";
			devicealert.SuiteInfoId="";
			devicealert.VehicleId="";
			devicealert.Cmd="";
			devicealert.Longitude="";
			devicealert.Latitude="";
			devicealert.Speed="";
			devicealert.Direction="";
			devicealert.GpsValid="";
			devicealert.TagValue="";
			SingleMessage<DeviceAlert> ret = service.AddDeviceAlert(devicealert);
			//Assert.IsFalse(ret.IsSuccess);
		}


		/// <summary>
		/// 测试VehicleId日期格式错误
		/// </summary>
		[TestMethod]
		public void TestVehicleIdDateTimeForFormatError()
		{
			DeviceAlert devicealert = new DeviceAlert();
			DeviceAlertRespository service = new DeviceAlertRespository();
			devicealert.Id="";
			devicealert.MdvrCoreId="";
			devicealert.SuiteInfoId="";
			devicealert.VehicleId="";
			devicealert.Cmd="";
			devicealert.Longitude="";
			devicealert.Latitude="";
			devicealert.Speed="";
			devicealert.Direction="";
			devicealert.GpsValid="";
			devicealert.TagValue="";
			SingleMessage<DeviceAlert> ret = service.AddDeviceAlert(devicealert);
			//Assert.IsFalse(ret.IsSuccess);
		}


		/// <summary>
		/// 测试SuiteStatus日期格式错误
		/// </summary>
		[TestMethod]
		public void TestSuiteStatusDateTimeForFormatError()
		{
			DeviceAlert devicealert = new DeviceAlert();
			DeviceAlertRespository service = new DeviceAlertRespository();
			devicealert.Id="";
			devicealert.MdvrCoreId="";
			devicealert.SuiteInfoId="";
			devicealert.VehicleId="";
			devicealert.Cmd="";
			devicealert.Longitude="";
			devicealert.Latitude="";
			devicealert.Speed="";
			devicealert.Direction="";
			devicealert.GpsValid="";
			devicealert.TagValue="";
			SingleMessage<DeviceAlert> ret = service.AddDeviceAlert(devicealert);
			//Assert.IsFalse(ret.IsSuccess);
		}


		/// <summary>
		/// 测试AlertType日期格式错误
		/// </summary>
		[TestMethod]
		public void TestAlertTypeDateTimeForFormatError()
		{
			DeviceAlert devicealert = new DeviceAlert();
			DeviceAlertRespository service = new DeviceAlertRespository();
			devicealert.Id="";
			devicealert.MdvrCoreId="";
			devicealert.SuiteInfoId="";
			devicealert.VehicleId="";
			devicealert.Cmd="";
			devicealert.Longitude="";
			devicealert.Latitude="";
			devicealert.Speed="";
			devicealert.Direction="";
			devicealert.GpsValid="";
			devicealert.TagValue="";
			SingleMessage<DeviceAlert> ret = service.AddDeviceAlert(devicealert);
			//Assert.IsFalse(ret.IsSuccess);
		}


		/// <summary>
		/// 测试AlertTypeName日期格式错误
		/// </summary>
		[TestMethod]
		public void TestAlertTypeNameDateTimeForFormatError()
		{
			DeviceAlert devicealert = new DeviceAlert();
			DeviceAlertRespository service = new DeviceAlertRespository();
			devicealert.Id="";
			devicealert.MdvrCoreId="";
			devicealert.SuiteInfoId="";
			devicealert.VehicleId="";
			devicealert.Cmd="";
			devicealert.Longitude="";
			devicealert.Latitude="";
			devicealert.Speed="";
			devicealert.Direction="";
			devicealert.GpsValid="";
			devicealert.TagValue="";
			SingleMessage<DeviceAlert> ret = service.AddDeviceAlert(devicealert);
			//Assert.IsFalse(ret.IsSuccess);
		}


		/// <summary>
		/// 测试AlertTime日期格式错误
		/// </summary>
		[TestMethod]
		public void TestAlertTimeDateTimeForFormatError()
		{
			DeviceAlert devicealert = new DeviceAlert();
			DeviceAlertRespository service = new DeviceAlertRespository();
			devicealert.Id="";
			devicealert.MdvrCoreId="";
			devicealert.SuiteInfoId="";
			devicealert.VehicleId="";
			devicealert.Cmd="";
			devicealert.Longitude="";
			devicealert.Latitude="";
			devicealert.Speed="";
			devicealert.Direction="";
			devicealert.GpsValid="";
			devicealert.TagValue="";
			SingleMessage<DeviceAlert> ret = service.AddDeviceAlert(devicealert);
			//Assert.IsFalse(ret.IsSuccess);
		}


		/// <summary>
		/// 测试Cmd日期格式错误
		/// </summary>
		[TestMethod]
		public void TestCmdDateTimeForFormatError()
		{
			DeviceAlert devicealert = new DeviceAlert();
			DeviceAlertRespository service = new DeviceAlertRespository();
			devicealert.Id="";
			devicealert.MdvrCoreId="";
			devicealert.SuiteInfoId="";
			devicealert.VehicleId="";
			devicealert.Cmd="";
			devicealert.Longitude="";
			devicealert.Latitude="";
			devicealert.Speed="";
			devicealert.Direction="";
			devicealert.GpsValid="";
			devicealert.TagValue="";
			SingleMessage<DeviceAlert> ret = service.AddDeviceAlert(devicealert);
			//Assert.IsFalse(ret.IsSuccess);
		}


		/// <summary>
		/// 测试Longitude日期格式错误
		/// </summary>
		[TestMethod]
		public void TestLongitudeDateTimeForFormatError()
		{
			DeviceAlert devicealert = new DeviceAlert();
			DeviceAlertRespository service = new DeviceAlertRespository();
			devicealert.Id="";
			devicealert.MdvrCoreId="";
			devicealert.SuiteInfoId="";
			devicealert.VehicleId="";
			devicealert.Cmd="";
			devicealert.Longitude="";
			devicealert.Latitude="";
			devicealert.Speed="";
			devicealert.Direction="";
			devicealert.GpsValid="";
			devicealert.TagValue="";
			SingleMessage<DeviceAlert> ret = service.AddDeviceAlert(devicealert);
			//Assert.IsFalse(ret.IsSuccess);
		}


		/// <summary>
		/// 测试Latitude日期格式错误
		/// </summary>
		[TestMethod]
		public void TestLatitudeDateTimeForFormatError()
		{
			DeviceAlert devicealert = new DeviceAlert();
			DeviceAlertRespository service = new DeviceAlertRespository();
			devicealert.Id="";
			devicealert.MdvrCoreId="";
			devicealert.SuiteInfoId="";
			devicealert.VehicleId="";
			devicealert.Cmd="";
			devicealert.Longitude="";
			devicealert.Latitude="";
			devicealert.Speed="";
			devicealert.Direction="";
			devicealert.GpsValid="";
			devicealert.TagValue="";
			SingleMessage<DeviceAlert> ret = service.AddDeviceAlert(devicealert);
			//Assert.IsFalse(ret.IsSuccess);
		}


		/// <summary>
		/// 测试GpsTime日期格式错误
		/// </summary>
		[TestMethod]
		public void TestGpsTimeDateTimeForFormatError()
		{
			DeviceAlert devicealert = new DeviceAlert();
			DeviceAlertRespository service = new DeviceAlertRespository();
			devicealert.Id="";
			devicealert.MdvrCoreId="";
			devicealert.SuiteInfoId="";
			devicealert.VehicleId="";
			devicealert.Cmd="";
			devicealert.Longitude="";
			devicealert.Latitude="";
			devicealert.Speed="";
			devicealert.Direction="";
			devicealert.GpsValid="";
			devicealert.TagValue="";
			SingleMessage<DeviceAlert> ret = service.AddDeviceAlert(devicealert);
			//Assert.IsFalse(ret.IsSuccess);
		}


		/// <summary>
		/// 测试Speed日期格式错误
		/// </summary>
		[TestMethod]
		public void TestSpeedDateTimeForFormatError()
		{
			DeviceAlert devicealert = new DeviceAlert();
			DeviceAlertRespository service = new DeviceAlertRespository();
			devicealert.Id="";
			devicealert.MdvrCoreId="";
			devicealert.SuiteInfoId="";
			devicealert.VehicleId="";
			devicealert.Cmd="";
			devicealert.Longitude="";
			devicealert.Latitude="";
			devicealert.Speed="";
			devicealert.Direction="";
			devicealert.GpsValid="";
			devicealert.TagValue="";
			SingleMessage<DeviceAlert> ret = service.AddDeviceAlert(devicealert);
			//Assert.IsFalse(ret.IsSuccess);
		}


		/// <summary>
		/// 测试Direction日期格式错误
		/// </summary>
		[TestMethod]
		public void TestDirectionDateTimeForFormatError()
		{
			DeviceAlert devicealert = new DeviceAlert();
			DeviceAlertRespository service = new DeviceAlertRespository();
			devicealert.Id="";
			devicealert.MdvrCoreId="";
			devicealert.SuiteInfoId="";
			devicealert.VehicleId="";
			devicealert.Cmd="";
			devicealert.Longitude="";
			devicealert.Latitude="";
			devicealert.Speed="";
			devicealert.Direction="";
			devicealert.GpsValid="";
			devicealert.TagValue="";
			SingleMessage<DeviceAlert> ret = service.AddDeviceAlert(devicealert);
			//Assert.IsFalse(ret.IsSuccess);
		}


		/// <summary>
		/// 测试GpsValid日期格式错误
		/// </summary>
		[TestMethod]
		public void TestGpsValidDateTimeForFormatError()
		{
			DeviceAlert devicealert = new DeviceAlert();
			DeviceAlertRespository service = new DeviceAlertRespository();
			devicealert.Id="";
			devicealert.MdvrCoreId="";
			devicealert.SuiteInfoId="";
			devicealert.VehicleId="";
			devicealert.Cmd="";
			devicealert.Longitude="";
			devicealert.Latitude="";
			devicealert.Speed="";
			devicealert.Direction="";
			devicealert.GpsValid="";
			devicealert.TagValue="";
			SingleMessage<DeviceAlert> ret = service.AddDeviceAlert(devicealert);
			//Assert.IsFalse(ret.IsSuccess);
		}


		/// <summary>
		/// 测试TagValue日期格式错误
		/// </summary>
		[TestMethod]
		public void TestTagValueDateTimeForFormatError()
		{
			DeviceAlert devicealert = new DeviceAlert();
			DeviceAlertRespository service = new DeviceAlertRespository();
			devicealert.Id="";
			devicealert.MdvrCoreId="";
			devicealert.SuiteInfoId="";
			devicealert.VehicleId="";
			devicealert.Cmd="";
			devicealert.Longitude="";
			devicealert.Latitude="";
			devicealert.Speed="";
			devicealert.Direction="";
			devicealert.GpsValid="";
			devicealert.TagValue="";
			SingleMessage<DeviceAlert> ret = service.AddDeviceAlert(devicealert);
			//Assert.IsFalse(ret.IsSuccess);
		}


		/// <summary>
		/// 测试Status日期格式错误
		/// </summary>
		[TestMethod]
		public void TestStatusDateTimeForFormatError()
		{
			DeviceAlert devicealert = new DeviceAlert();
			DeviceAlertRespository service = new DeviceAlertRespository();
			devicealert.Id="";
			devicealert.MdvrCoreId="";
			devicealert.SuiteInfoId="";
			devicealert.VehicleId="";
			devicealert.Cmd="";
			devicealert.Longitude="";
			devicealert.Latitude="";
			devicealert.Speed="";
			devicealert.Direction="";
			devicealert.GpsValid="";
			devicealert.TagValue="";
			SingleMessage<DeviceAlert> ret = service.AddDeviceAlert(devicealert);
			//Assert.IsFalse(ret.IsSuccess);
		}


		/// <summary>
		/// 测试HandleId日期格式错误
		/// </summary>
		[TestMethod]
		public void TestHandleIdDateTimeForFormatError()
		{
			DeviceAlert devicealert = new DeviceAlert();
			DeviceAlertRespository service = new DeviceAlertRespository();
			devicealert.Id="";
			devicealert.MdvrCoreId="";
			devicealert.SuiteInfoId="";
			devicealert.VehicleId="";
			devicealert.Cmd="";
			devicealert.Longitude="";
			devicealert.Latitude="";
			devicealert.Speed="";
			devicealert.Direction="";
			devicealert.GpsValid="";
			devicealert.TagValue="";
			SingleMessage<DeviceAlert> ret = service.AddDeviceAlert(devicealert);
			//Assert.IsFalse(ret.IsSuccess);
		}


		/// <summary>
		/// 测试RuleId日期格式错误
		/// </summary>
		[TestMethod]
		public void TestRuleIdDateTimeForFormatError()
		{
			DeviceAlert devicealert = new DeviceAlert();
			DeviceAlertRespository service = new DeviceAlertRespository();
			devicealert.Id="";
			devicealert.MdvrCoreId="";
			devicealert.SuiteInfoId="";
			devicealert.VehicleId="";
			devicealert.Cmd="";
			devicealert.Longitude="";
			devicealert.Latitude="";
			devicealert.Speed="";
			devicealert.Direction="";
			devicealert.GpsValid="";
			devicealert.TagValue="";
			SingleMessage<DeviceAlert> ret = service.AddDeviceAlert(devicealert);
			//Assert.IsFalse(ret.IsSuccess);
		}


		/// <summary>
		/// 测试Id日期最大值
		/// </summary>
		[TestMethod]
		public void TestIdDateTimeForMax()
		{
			DeviceAlert devicealert = new DeviceAlert();
			DeviceAlertRespository service = new DeviceAlertRespository();
			devicealert.Id="";
			devicealert.MdvrCoreId="";
			devicealert.SuiteInfoId="";
			devicealert.VehicleId="";
			devicealert.Cmd="";
			devicealert.Longitude="";
			devicealert.Latitude="";
			devicealert.Speed="";
			devicealert.Direction="";
			devicealert.GpsValid="";
			devicealert.TagValue="";
			SingleMessage<DeviceAlert> ret = service.AddDeviceAlert(devicealert);
			//Assert.IsFalse(ret.IsSuccess);
		}


		/// <summary>
		/// 测试MdvrCoreId日期最大值
		/// </summary>
		[TestMethod]
		public void TestMdvrCoreIdDateTimeForMax()
		{
			DeviceAlert devicealert = new DeviceAlert();
			DeviceAlertRespository service = new DeviceAlertRespository();
			devicealert.Id="";
			devicealert.MdvrCoreId="";
			devicealert.SuiteInfoId="";
			devicealert.VehicleId="";
			devicealert.Cmd="";
			devicealert.Longitude="";
			devicealert.Latitude="";
			devicealert.Speed="";
			devicealert.Direction="";
			devicealert.GpsValid="";
			devicealert.TagValue="";
			SingleMessage<DeviceAlert> ret = service.AddDeviceAlert(devicealert);
			//Assert.IsFalse(ret.IsSuccess);
		}


		/// <summary>
		/// 测试SuiteId日期最大值
		/// </summary>
		[TestMethod]
		public void TestSuiteIdDateTimeForMax()
		{
			DeviceAlert devicealert = new DeviceAlert();
			DeviceAlertRespository service = new DeviceAlertRespository();
			devicealert.Id="";
			devicealert.MdvrCoreId="";
			devicealert.SuiteInfoId="";
			devicealert.VehicleId="";
			devicealert.Cmd="";
			devicealert.Longitude="";
			devicealert.Latitude="";
			devicealert.Speed="";
			devicealert.Direction="";
			devicealert.GpsValid="";
			devicealert.TagValue="";
			SingleMessage<DeviceAlert> ret = service.AddDeviceAlert(devicealert);
			//Assert.IsFalse(ret.IsSuccess);
		}


		/// <summary>
		/// 测试SuiteInfoId日期最大值
		/// </summary>
		[TestMethod]
		public void TestSuiteInfoIdDateTimeForMax()
		{
			DeviceAlert devicealert = new DeviceAlert();
			DeviceAlertRespository service = new DeviceAlertRespository();
			devicealert.Id="";
			devicealert.MdvrCoreId="";
			devicealert.SuiteInfoId="";
			devicealert.VehicleId="";
			devicealert.Cmd="";
			devicealert.Longitude="";
			devicealert.Latitude="";
			devicealert.Speed="";
			devicealert.Direction="";
			devicealert.GpsValid="";
			devicealert.TagValue="";
			SingleMessage<DeviceAlert> ret = service.AddDeviceAlert(devicealert);
			//Assert.IsFalse(ret.IsSuccess);
		}


		/// <summary>
		/// 测试VehicleId日期最大值
		/// </summary>
		[TestMethod]
		public void TestVehicleIdDateTimeForMax()
		{
			DeviceAlert devicealert = new DeviceAlert();
			DeviceAlertRespository service = new DeviceAlertRespository();
			devicealert.Id="";
			devicealert.MdvrCoreId="";
			devicealert.SuiteInfoId="";
			devicealert.VehicleId="";
			devicealert.Cmd="";
			devicealert.Longitude="";
			devicealert.Latitude="";
			devicealert.Speed="";
			devicealert.Direction="";
			devicealert.GpsValid="";
			devicealert.TagValue="";
			SingleMessage<DeviceAlert> ret = service.AddDeviceAlert(devicealert);
			//Assert.IsFalse(ret.IsSuccess);
		}


		/// <summary>
		/// 测试SuiteStatus日期最大值
		/// </summary>
		[TestMethod]
		public void TestSuiteStatusDateTimeForMax()
		{
			DeviceAlert devicealert = new DeviceAlert();
			DeviceAlertRespository service = new DeviceAlertRespository();
			devicealert.Id="";
			devicealert.MdvrCoreId="";
			devicealert.SuiteInfoId="";
			devicealert.VehicleId="";
			devicealert.Cmd="";
			devicealert.Longitude="";
			devicealert.Latitude="";
			devicealert.Speed="";
			devicealert.Direction="";
			devicealert.GpsValid="";
			devicealert.TagValue="";
			SingleMessage<DeviceAlert> ret = service.AddDeviceAlert(devicealert);
			//Assert.IsFalse(ret.IsSuccess);
		}


		/// <summary>
		/// 测试AlertType日期最大值
		/// </summary>
		[TestMethod]
		public void TestAlertTypeDateTimeForMax()
		{
			DeviceAlert devicealert = new DeviceAlert();
			DeviceAlertRespository service = new DeviceAlertRespository();
			devicealert.Id="";
			devicealert.MdvrCoreId="";
			devicealert.SuiteInfoId="";
			devicealert.VehicleId="";
			devicealert.Cmd="";
			devicealert.Longitude="";
			devicealert.Latitude="";
			devicealert.Speed="";
			devicealert.Direction="";
			devicealert.GpsValid="";
			devicealert.TagValue="";
			SingleMessage<DeviceAlert> ret = service.AddDeviceAlert(devicealert);
			//Assert.IsFalse(ret.IsSuccess);
		}


		/// <summary>
		/// 测试AlertTypeName日期最大值
		/// </summary>
		[TestMethod]
		public void TestAlertTypeNameDateTimeForMax()
		{
			DeviceAlert devicealert = new DeviceAlert();
			DeviceAlertRespository service = new DeviceAlertRespository();
			devicealert.Id="";
			devicealert.MdvrCoreId="";
			devicealert.SuiteInfoId="";
			devicealert.VehicleId="";
			devicealert.Cmd="";
			devicealert.Longitude="";
			devicealert.Latitude="";
			devicealert.Speed="";
			devicealert.Direction="";
			devicealert.GpsValid="";
			devicealert.TagValue="";
			SingleMessage<DeviceAlert> ret = service.AddDeviceAlert(devicealert);
			//Assert.IsFalse(ret.IsSuccess);
		}


		/// <summary>
		/// 测试AlertTime日期最大值
		/// </summary>
		[TestMethod]
		public void TestAlertTimeDateTimeForMax()
		{
			DeviceAlert devicealert = new DeviceAlert();
			DeviceAlertRespository service = new DeviceAlertRespository();
			devicealert.Id="";
			devicealert.MdvrCoreId="";
			devicealert.SuiteInfoId="";
			devicealert.VehicleId="";
			devicealert.Cmd="";
			devicealert.Longitude="";
			devicealert.Latitude="";
			devicealert.Speed="";
			devicealert.Direction="";
			devicealert.GpsValid="";
			devicealert.TagValue="";
			SingleMessage<DeviceAlert> ret = service.AddDeviceAlert(devicealert);
			//Assert.IsFalse(ret.IsSuccess);
		}


		/// <summary>
		/// 测试Cmd日期最大值
		/// </summary>
		[TestMethod]
		public void TestCmdDateTimeForMax()
		{
			DeviceAlert devicealert = new DeviceAlert();
			DeviceAlertRespository service = new DeviceAlertRespository();
			devicealert.Id="";
			devicealert.MdvrCoreId="";
			devicealert.SuiteInfoId="";
			devicealert.VehicleId="";
			devicealert.Cmd="";
			devicealert.Longitude="";
			devicealert.Latitude="";
			devicealert.Speed="";
			devicealert.Direction="";
			devicealert.GpsValid="";
			devicealert.TagValue="";
			SingleMessage<DeviceAlert> ret = service.AddDeviceAlert(devicealert);
			//Assert.IsFalse(ret.IsSuccess);
		}


		/// <summary>
		/// 测试Longitude日期最大值
		/// </summary>
		[TestMethod]
		public void TestLongitudeDateTimeForMax()
		{
			DeviceAlert devicealert = new DeviceAlert();
			DeviceAlertRespository service = new DeviceAlertRespository();
			devicealert.Id="";
			devicealert.MdvrCoreId="";
			devicealert.SuiteInfoId="";
			devicealert.VehicleId="";
			devicealert.Cmd="";
			devicealert.Longitude="";
			devicealert.Latitude="";
			devicealert.Speed="";
			devicealert.Direction="";
			devicealert.GpsValid="";
			devicealert.TagValue="";
			SingleMessage<DeviceAlert> ret = service.AddDeviceAlert(devicealert);
			//Assert.IsFalse(ret.IsSuccess);
		}


		/// <summary>
		/// 测试Latitude日期最大值
		/// </summary>
		[TestMethod]
		public void TestLatitudeDateTimeForMax()
		{
			DeviceAlert devicealert = new DeviceAlert();
			DeviceAlertRespository service = new DeviceAlertRespository();
			devicealert.Id="";
			devicealert.MdvrCoreId="";
			devicealert.SuiteInfoId="";
			devicealert.VehicleId="";
			devicealert.Cmd="";
			devicealert.Longitude="";
			devicealert.Latitude="";
			devicealert.Speed="";
			devicealert.Direction="";
			devicealert.GpsValid="";
			devicealert.TagValue="";
			SingleMessage<DeviceAlert> ret = service.AddDeviceAlert(devicealert);
			//Assert.IsFalse(ret.IsSuccess);
		}


		/// <summary>
		/// 测试GpsTime日期最大值
		/// </summary>
		[TestMethod]
		public void TestGpsTimeDateTimeForMax()
		{
			DeviceAlert devicealert = new DeviceAlert();
			DeviceAlertRespository service = new DeviceAlertRespository();
			devicealert.Id="";
			devicealert.MdvrCoreId="";
			devicealert.SuiteInfoId="";
			devicealert.VehicleId="";
			devicealert.Cmd="";
			devicealert.Longitude="";
			devicealert.Latitude="";
			devicealert.Speed="";
			devicealert.Direction="";
			devicealert.GpsValid="";
			devicealert.TagValue="";
			SingleMessage<DeviceAlert> ret = service.AddDeviceAlert(devicealert);
			//Assert.IsFalse(ret.IsSuccess);
		}


		/// <summary>
		/// 测试Speed日期最大值
		/// </summary>
		[TestMethod]
		public void TestSpeedDateTimeForMax()
		{
			DeviceAlert devicealert = new DeviceAlert();
			DeviceAlertRespository service = new DeviceAlertRespository();
			devicealert.Id="";
			devicealert.MdvrCoreId="";
			devicealert.SuiteInfoId="";
			devicealert.VehicleId="";
			devicealert.Cmd="";
			devicealert.Longitude="";
			devicealert.Latitude="";
			devicealert.Speed="";
			devicealert.Direction="";
			devicealert.GpsValid="";
			devicealert.TagValue="";
			SingleMessage<DeviceAlert> ret = service.AddDeviceAlert(devicealert);
			//Assert.IsFalse(ret.IsSuccess);
		}


		/// <summary>
		/// 测试Direction日期最大值
		/// </summary>
		[TestMethod]
		public void TestDirectionDateTimeForMax()
		{
			DeviceAlert devicealert = new DeviceAlert();
			DeviceAlertRespository service = new DeviceAlertRespository();
			devicealert.Id="";
			devicealert.MdvrCoreId="";
			devicealert.SuiteInfoId="";
			devicealert.VehicleId="";
			devicealert.Cmd="";
			devicealert.Longitude="";
			devicealert.Latitude="";
			devicealert.Speed="";
			devicealert.Direction="";
			devicealert.GpsValid="";
			devicealert.TagValue="";
			SingleMessage<DeviceAlert> ret = service.AddDeviceAlert(devicealert);
			//Assert.IsFalse(ret.IsSuccess);
		}


		/// <summary>
		/// 测试GpsValid日期最大值
		/// </summary>
		[TestMethod]
		public void TestGpsValidDateTimeForMax()
		{
			DeviceAlert devicealert = new DeviceAlert();
			DeviceAlertRespository service = new DeviceAlertRespository();
			devicealert.Id="";
			devicealert.MdvrCoreId="";
			devicealert.SuiteInfoId="";
			devicealert.VehicleId="";
			devicealert.Cmd="";
			devicealert.Longitude="";
			devicealert.Latitude="";
			devicealert.Speed="";
			devicealert.Direction="";
			devicealert.GpsValid="";
			devicealert.TagValue="";
			SingleMessage<DeviceAlert> ret = service.AddDeviceAlert(devicealert);
			//Assert.IsFalse(ret.IsSuccess);
		}


		/// <summary>
		/// 测试TagValue日期最大值
		/// </summary>
		[TestMethod]
		public void TestTagValueDateTimeForMax()
		{
			DeviceAlert devicealert = new DeviceAlert();
			DeviceAlertRespository service = new DeviceAlertRespository();
			devicealert.Id="";
			devicealert.MdvrCoreId="";
			devicealert.SuiteInfoId="";
			devicealert.VehicleId="";
			devicealert.Cmd="";
			devicealert.Longitude="";
			devicealert.Latitude="";
			devicealert.Speed="";
			devicealert.Direction="";
			devicealert.GpsValid="";
			devicealert.TagValue="";
			SingleMessage<DeviceAlert> ret = service.AddDeviceAlert(devicealert);
			//Assert.IsFalse(ret.IsSuccess);
		}


		/// <summary>
		/// 测试Status日期最大值
		/// </summary>
		[TestMethod]
		public void TestStatusDateTimeForMax()
		{
			DeviceAlert devicealert = new DeviceAlert();
			DeviceAlertRespository service = new DeviceAlertRespository();
			devicealert.Id="";
			devicealert.MdvrCoreId="";
			devicealert.SuiteInfoId="";
			devicealert.VehicleId="";
			devicealert.Cmd="";
			devicealert.Longitude="";
			devicealert.Latitude="";
			devicealert.Speed="";
			devicealert.Direction="";
			devicealert.GpsValid="";
			devicealert.TagValue="";
			SingleMessage<DeviceAlert> ret = service.AddDeviceAlert(devicealert);
			//Assert.IsFalse(ret.IsSuccess);
		}


		/// <summary>
		/// 测试HandleId日期最大值
		/// </summary>
		[TestMethod]
		public void TestHandleIdDateTimeForMax()
		{
			DeviceAlert devicealert = new DeviceAlert();
			DeviceAlertRespository service = new DeviceAlertRespository();
			devicealert.Id="";
			devicealert.MdvrCoreId="";
			devicealert.SuiteInfoId="";
			devicealert.VehicleId="";
			devicealert.Cmd="";
			devicealert.Longitude="";
			devicealert.Latitude="";
			devicealert.Speed="";
			devicealert.Direction="";
			devicealert.GpsValid="";
			devicealert.TagValue="";
			SingleMessage<DeviceAlert> ret = service.AddDeviceAlert(devicealert);
			//Assert.IsFalse(ret.IsSuccess);
		}


		/// <summary>
		/// 测试RuleId日期最大值
		/// </summary>
		[TestMethod]
		public void TestRuleIdDateTimeForMax()
		{
			DeviceAlert devicealert = new DeviceAlert();
			DeviceAlertRespository service = new DeviceAlertRespository();
			devicealert.Id="";
			devicealert.MdvrCoreId="";
			devicealert.SuiteInfoId="";
			devicealert.VehicleId="";
			devicealert.Cmd="";
			devicealert.Longitude="";
			devicealert.Latitude="";
			devicealert.Speed="";
			devicealert.Direction="";
			devicealert.GpsValid="";
			devicealert.TagValue="";
			SingleMessage<DeviceAlert> ret = service.AddDeviceAlert(devicealert);
			//Assert.IsFalse(ret.IsSuccess);
		}


		/// <summary>
		/// 测试Id日期格式错误
		/// </summary>
		[TestMethod]
		public void TestIdDateTimeForMin()
		{
			DeviceAlert devicealert = new DeviceAlert();
			DeviceAlertRespository service = new DeviceAlertRespository();
			devicealert.Id="";
			devicealert.MdvrCoreId="";
			devicealert.SuiteInfoId="";
			devicealert.VehicleId="";
			devicealert.Cmd="";
			devicealert.Longitude="";
			devicealert.Latitude="";
			devicealert.Speed="";
			devicealert.Direction="";
			devicealert.GpsValid="";
			devicealert.TagValue="";
			SingleMessage<DeviceAlert> ret = service.AddDeviceAlert(devicealert);
			//Assert.IsFalse(ret.IsSuccess);
		}


		/// <summary>
		/// 测试MdvrCoreId日期格式错误
		/// </summary>
		[TestMethod]
		public void TestMdvrCoreIdDateTimeForMin()
		{
			DeviceAlert devicealert = new DeviceAlert();
			DeviceAlertRespository service = new DeviceAlertRespository();
			devicealert.Id="";
			devicealert.MdvrCoreId="";
			devicealert.SuiteInfoId="";
			devicealert.VehicleId="";
			devicealert.Cmd="";
			devicealert.Longitude="";
			devicealert.Latitude="";
			devicealert.Speed="";
			devicealert.Direction="";
			devicealert.GpsValid="";
			devicealert.TagValue="";
			SingleMessage<DeviceAlert> ret = service.AddDeviceAlert(devicealert);
			//Assert.IsFalse(ret.IsSuccess);
		}


		/// <summary>
		/// 测试SuiteId日期格式错误
		/// </summary>
		[TestMethod]
		public void TestSuiteIdDateTimeForMin()
		{
			DeviceAlert devicealert = new DeviceAlert();
			DeviceAlertRespository service = new DeviceAlertRespository();
			devicealert.Id="";
			devicealert.MdvrCoreId="";
			devicealert.SuiteInfoId="";
			devicealert.VehicleId="";
			devicealert.Cmd="";
			devicealert.Longitude="";
			devicealert.Latitude="";
			devicealert.Speed="";
			devicealert.Direction="";
			devicealert.GpsValid="";
			devicealert.TagValue="";
			SingleMessage<DeviceAlert> ret = service.AddDeviceAlert(devicealert);
			//Assert.IsFalse(ret.IsSuccess);
		}


		/// <summary>
		/// 测试SuiteInfoId日期格式错误
		/// </summary>
		[TestMethod]
		public void TestSuiteInfoIdDateTimeForMin()
		{
			DeviceAlert devicealert = new DeviceAlert();
			DeviceAlertRespository service = new DeviceAlertRespository();
			devicealert.Id="";
			devicealert.MdvrCoreId="";
			devicealert.SuiteInfoId="";
			devicealert.VehicleId="";
			devicealert.Cmd="";
			devicealert.Longitude="";
			devicealert.Latitude="";
			devicealert.Speed="";
			devicealert.Direction="";
			devicealert.GpsValid="";
			devicealert.TagValue="";
			SingleMessage<DeviceAlert> ret = service.AddDeviceAlert(devicealert);
			//Assert.IsFalse(ret.IsSuccess);
		}


		/// <summary>
		/// 测试VehicleId日期格式错误
		/// </summary>
		[TestMethod]
		public void TestVehicleIdDateTimeForMin()
		{
			DeviceAlert devicealert = new DeviceAlert();
			DeviceAlertRespository service = new DeviceAlertRespository();
			devicealert.Id="";
			devicealert.MdvrCoreId="";
			devicealert.SuiteInfoId="";
			devicealert.VehicleId="";
			devicealert.Cmd="";
			devicealert.Longitude="";
			devicealert.Latitude="";
			devicealert.Speed="";
			devicealert.Direction="";
			devicealert.GpsValid="";
			devicealert.TagValue="";
			SingleMessage<DeviceAlert> ret = service.AddDeviceAlert(devicealert);
			//Assert.IsFalse(ret.IsSuccess);
		}


		/// <summary>
		/// 测试SuiteStatus日期格式错误
		/// </summary>
		[TestMethod]
		public void TestSuiteStatusDateTimeForMin()
		{
			DeviceAlert devicealert = new DeviceAlert();
			DeviceAlertRespository service = new DeviceAlertRespository();
			devicealert.Id="";
			devicealert.MdvrCoreId="";
			devicealert.SuiteInfoId="";
			devicealert.VehicleId="";
			devicealert.Cmd="";
			devicealert.Longitude="";
			devicealert.Latitude="";
			devicealert.Speed="";
			devicealert.Direction="";
			devicealert.GpsValid="";
			devicealert.TagValue="";
			SingleMessage<DeviceAlert> ret = service.AddDeviceAlert(devicealert);
			//Assert.IsFalse(ret.IsSuccess);
		}


		/// <summary>
		/// 测试AlertType日期格式错误
		/// </summary>
		[TestMethod]
		public void TestAlertTypeDateTimeForMin()
		{
			DeviceAlert devicealert = new DeviceAlert();
			DeviceAlertRespository service = new DeviceAlertRespository();
			devicealert.Id="";
			devicealert.MdvrCoreId="";
			devicealert.SuiteInfoId="";
			devicealert.VehicleId="";
			devicealert.Cmd="";
			devicealert.Longitude="";
			devicealert.Latitude="";
			devicealert.Speed="";
			devicealert.Direction="";
			devicealert.GpsValid="";
			devicealert.TagValue="";
			SingleMessage<DeviceAlert> ret = service.AddDeviceAlert(devicealert);
			//Assert.IsFalse(ret.IsSuccess);
		}


		/// <summary>
		/// 测试AlertTypeName日期格式错误
		/// </summary>
		[TestMethod]
		public void TestAlertTypeNameDateTimeForMin()
		{
			DeviceAlert devicealert = new DeviceAlert();
			DeviceAlertRespository service = new DeviceAlertRespository();
			devicealert.Id="";
			devicealert.MdvrCoreId="";
			devicealert.SuiteInfoId="";
			devicealert.VehicleId="";
			devicealert.Cmd="";
			devicealert.Longitude="";
			devicealert.Latitude="";
			devicealert.Speed="";
			devicealert.Direction="";
			devicealert.GpsValid="";
			devicealert.TagValue="";
			SingleMessage<DeviceAlert> ret = service.AddDeviceAlert(devicealert);
			//Assert.IsFalse(ret.IsSuccess);
		}


		/// <summary>
		/// 测试AlertTime日期格式错误
		/// </summary>
		[TestMethod]
		public void TestAlertTimeDateTimeForMin()
		{
			DeviceAlert devicealert = new DeviceAlert();
			DeviceAlertRespository service = new DeviceAlertRespository();
			devicealert.Id="";
			devicealert.MdvrCoreId="";
			devicealert.SuiteInfoId="";
			devicealert.VehicleId="";
			devicealert.Cmd="";
			devicealert.Longitude="";
			devicealert.Latitude="";
			devicealert.Speed="";
			devicealert.Direction="";
			devicealert.GpsValid="";
			devicealert.TagValue="";
			SingleMessage<DeviceAlert> ret = service.AddDeviceAlert(devicealert);
			//Assert.IsFalse(ret.IsSuccess);
		}


		/// <summary>
		/// 测试Cmd日期格式错误
		/// </summary>
		[TestMethod]
		public void TestCmdDateTimeForMin()
		{
			DeviceAlert devicealert = new DeviceAlert();
			DeviceAlertRespository service = new DeviceAlertRespository();
			devicealert.Id="";
			devicealert.MdvrCoreId="";
			devicealert.SuiteInfoId="";
			devicealert.VehicleId="";
			devicealert.Cmd="";
			devicealert.Longitude="";
			devicealert.Latitude="";
			devicealert.Speed="";
			devicealert.Direction="";
			devicealert.GpsValid="";
			devicealert.TagValue="";
			SingleMessage<DeviceAlert> ret = service.AddDeviceAlert(devicealert);
			//Assert.IsFalse(ret.IsSuccess);
		}


		/// <summary>
		/// 测试Longitude日期格式错误
		/// </summary>
		[TestMethod]
		public void TestLongitudeDateTimeForMin()
		{
			DeviceAlert devicealert = new DeviceAlert();
			DeviceAlertRespository service = new DeviceAlertRespository();
			devicealert.Id="";
			devicealert.MdvrCoreId="";
			devicealert.SuiteInfoId="";
			devicealert.VehicleId="";
			devicealert.Cmd="";
			devicealert.Longitude="";
			devicealert.Latitude="";
			devicealert.Speed="";
			devicealert.Direction="";
			devicealert.GpsValid="";
			devicealert.TagValue="";
			SingleMessage<DeviceAlert> ret = service.AddDeviceAlert(devicealert);
			//Assert.IsFalse(ret.IsSuccess);
		}


		/// <summary>
		/// 测试Latitude日期格式错误
		/// </summary>
		[TestMethod]
		public void TestLatitudeDateTimeForMin()
		{
			DeviceAlert devicealert = new DeviceAlert();
			DeviceAlertRespository service = new DeviceAlertRespository();
			devicealert.Id="";
			devicealert.MdvrCoreId="";
			devicealert.SuiteInfoId="";
			devicealert.VehicleId="";
			devicealert.Cmd="";
			devicealert.Longitude="";
			devicealert.Latitude="";
			devicealert.Speed="";
			devicealert.Direction="";
			devicealert.GpsValid="";
			devicealert.TagValue="";
			SingleMessage<DeviceAlert> ret = service.AddDeviceAlert(devicealert);
			//Assert.IsFalse(ret.IsSuccess);
		}


		/// <summary>
		/// 测试GpsTime日期格式错误
		/// </summary>
		[TestMethod]
		public void TestGpsTimeDateTimeForMin()
		{
			DeviceAlert devicealert = new DeviceAlert();
			DeviceAlertRespository service = new DeviceAlertRespository();
			devicealert.Id="";
			devicealert.MdvrCoreId="";
			devicealert.SuiteInfoId="";
			devicealert.VehicleId="";
			devicealert.Cmd="";
			devicealert.Longitude="";
			devicealert.Latitude="";
			devicealert.Speed="";
			devicealert.Direction="";
			devicealert.GpsValid="";
			devicealert.TagValue="";
			SingleMessage<DeviceAlert> ret = service.AddDeviceAlert(devicealert);
			//Assert.IsFalse(ret.IsSuccess);
		}


		/// <summary>
		/// 测试Speed日期格式错误
		/// </summary>
		[TestMethod]
		public void TestSpeedDateTimeForMin()
		{
			DeviceAlert devicealert = new DeviceAlert();
			DeviceAlertRespository service = new DeviceAlertRespository();
			devicealert.Id="";
			devicealert.MdvrCoreId="";
			devicealert.SuiteInfoId="";
			devicealert.VehicleId="";
			devicealert.Cmd="";
			devicealert.Longitude="";
			devicealert.Latitude="";
			devicealert.Speed="";
			devicealert.Direction="";
			devicealert.GpsValid="";
			devicealert.TagValue="";
			SingleMessage<DeviceAlert> ret = service.AddDeviceAlert(devicealert);
			//Assert.IsFalse(ret.IsSuccess);
		}


		/// <summary>
		/// 测试Direction日期格式错误
		/// </summary>
		[TestMethod]
		public void TestDirectionDateTimeForMin()
		{
			DeviceAlert devicealert = new DeviceAlert();
			DeviceAlertRespository service = new DeviceAlertRespository();
			devicealert.Id="";
			devicealert.MdvrCoreId="";
			devicealert.SuiteInfoId="";
			devicealert.VehicleId="";
			devicealert.Cmd="";
			devicealert.Longitude="";
			devicealert.Latitude="";
			devicealert.Speed="";
			devicealert.Direction="";
			devicealert.GpsValid="";
			devicealert.TagValue="";
			SingleMessage<DeviceAlert> ret = service.AddDeviceAlert(devicealert);
			//Assert.IsFalse(ret.IsSuccess);
		}


		/// <summary>
		/// 测试GpsValid日期格式错误
		/// </summary>
		[TestMethod]
		public void TestGpsValidDateTimeForMin()
		{
			DeviceAlert devicealert = new DeviceAlert();
			DeviceAlertRespository service = new DeviceAlertRespository();
			devicealert.Id="";
			devicealert.MdvrCoreId="";
			devicealert.SuiteInfoId="";
			devicealert.VehicleId="";
			devicealert.Cmd="";
			devicealert.Longitude="";
			devicealert.Latitude="";
			devicealert.Speed="";
			devicealert.Direction="";
			devicealert.GpsValid="";
			devicealert.TagValue="";
			SingleMessage<DeviceAlert> ret = service.AddDeviceAlert(devicealert);
			//Assert.IsFalse(ret.IsSuccess);
		}


		/// <summary>
		/// 测试TagValue日期格式错误
		/// </summary>
		[TestMethod]
		public void TestTagValueDateTimeForMin()
		{
			DeviceAlert devicealert = new DeviceAlert();
			DeviceAlertRespository service = new DeviceAlertRespository();
			devicealert.Id="";
			devicealert.MdvrCoreId="";
			devicealert.SuiteInfoId="";
			devicealert.VehicleId="";
			devicealert.Cmd="";
			devicealert.Longitude="";
			devicealert.Latitude="";
			devicealert.Speed="";
			devicealert.Direction="";
			devicealert.GpsValid="";
			devicealert.TagValue="";
			SingleMessage<DeviceAlert> ret = service.AddDeviceAlert(devicealert);
			//Assert.IsFalse(ret.IsSuccess);
		}


		/// <summary>
		/// 测试Status日期格式错误
		/// </summary>
		[TestMethod]
		public void TestStatusDateTimeForMin()
		{
			DeviceAlert devicealert = new DeviceAlert();
			DeviceAlertRespository service = new DeviceAlertRespository();
			devicealert.Id="";
			devicealert.MdvrCoreId="";
			devicealert.SuiteInfoId="";
			devicealert.VehicleId="";
			devicealert.Cmd="";
			devicealert.Longitude="";
			devicealert.Latitude="";
			devicealert.Speed="";
			devicealert.Direction="";
			devicealert.GpsValid="";
			devicealert.TagValue="";
			SingleMessage<DeviceAlert> ret = service.AddDeviceAlert(devicealert);
			//Assert.IsFalse(ret.IsSuccess);
		}


		/// <summary>
		/// 测试HandleId日期格式错误
		/// </summary>
		[TestMethod]
		public void TestHandleIdDateTimeForMin()
		{
			DeviceAlert devicealert = new DeviceAlert();
			DeviceAlertRespository service = new DeviceAlertRespository();
			devicealert.Id="";
			devicealert.MdvrCoreId="";
			devicealert.SuiteInfoId="";
			devicealert.VehicleId="";
			devicealert.Cmd="";
			devicealert.Longitude="";
			devicealert.Latitude="";
			devicealert.Speed="";
			devicealert.Direction="";
			devicealert.GpsValid="";
			devicealert.TagValue="";
			SingleMessage<DeviceAlert> ret = service.AddDeviceAlert(devicealert);
			//Assert.IsFalse(ret.IsSuccess);
		}


		/// <summary>
		/// 测试RuleId日期格式错误
		/// </summary>
		[TestMethod]
		public void TestRuleIdDateTimeForMin()
		{
			DeviceAlert devicealert = new DeviceAlert();
			DeviceAlertRespository service = new DeviceAlertRespository();
			devicealert.Id="";
			devicealert.MdvrCoreId="";
			devicealert.SuiteInfoId="";
			devicealert.VehicleId="";
			devicealert.Cmd="";
			devicealert.Longitude="";
			devicealert.Latitude="";
			devicealert.Speed="";
			devicealert.Direction="";
			devicealert.GpsValid="";
			devicealert.TagValue="";
			SingleMessage<DeviceAlert> ret = service.AddDeviceAlert(devicealert);
			//Assert.IsFalse(ret.IsSuccess);
		}
	}
}

