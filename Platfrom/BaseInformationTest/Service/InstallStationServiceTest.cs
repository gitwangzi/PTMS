using System;
using System.Text;
using System.Collections.Generic;
using Gsafety.PTMS.Base.Contract.Data;
using Gsafety.PTMS.BaseInformation.Contract.Data;
using Gsafety.PTMS.BaseInformation.Service;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;
namespace Gsafety.PTMS.Message.Test{
	/// <summary>
	/// VehicleRepository 的摘要说明
	/// </summary>
	[TestClass]
	public class InstallStationServiceTest
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
			InstallStation installstation = new InstallStation();
			InstallStationService service = new InstallStationService();
			installstation.Id=null;
			installstation.Name=null;
			installstation.Address=null;
			installstation.Director=null;
			installstation.DirectorPhone=null;
			installstation.Contact=null;
			installstation.ContactPhone=null;
			installstation.Email=null;
			installstation.CityCode=null;
			installstation.Note=null;
			SingleMessage<bool> ret = service.AddInstallStation(installstation);
			//Assert.IsFalse(ret.IsSuccess);
		}


		/// <summary>
		/// 允许的字段为空
		/// </summary>
		[TestMethod]
		public void TestStringForAllowEmpty()
		{
			InstallStation installstation = new InstallStation();
			InstallStationService service = new InstallStationService();
			StringBuilder builder = new StringBuilder();
			Random random = new Random();
			for (int i = 0; i < 36; i++)
			{
				builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
			}
			installstation.Id=builder.ToString();
			builder.Length=0;

			installstation.Name=null;
			installstation.Address=null;
			installstation.Director=null;
			installstation.DirectorPhone=null;
			installstation.Contact=null;
			installstation.ContactPhone=null;
			installstation.Email=null;
			installstation.CityCode=null;
			installstation.Note=null;
			SingleMessage<bool> ret = service.AddInstallStation(installstation);
			//Assert.IsFalse(ret.IsSuccess);
		}


		/// <summary>
		/// 测试最大中文长度
		/// </summary>
		[TestMethod]
		public void TestStringForMaxLength_Chinese()
		{
			InstallStation installstation = new InstallStation();
			InstallStationService service = new InstallStationService();
			StringBuilder builder = new StringBuilder();
			Random random = new Random();
			for (int i = 0; i < 17; i++)
			{
				builder.Append(chinesecharacter[random.Next(chinesecharacter.Length - 1)]);
			}
			installstation.Id=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 255; i++)
			{
				builder.Append(chinesecharacter[random.Next(chinesecharacter.Length - 1)]);
			}
			installstation.Name=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 255; i++)
			{
				builder.Append(chinesecharacter[random.Next(chinesecharacter.Length - 1)]);
			}
			installstation.Address=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 24; i++)
			{
				builder.Append(chinesecharacter[random.Next(chinesecharacter.Length - 1)]);
			}
			installstation.Director=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 24; i++)
			{
				builder.Append(chinesecharacter[random.Next(chinesecharacter.Length - 1)]);
			}
			installstation.DirectorPhone=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 9; i++)
			{
				builder.Append(chinesecharacter[random.Next(chinesecharacter.Length - 1)]);
			}
			installstation.Contact=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 9; i++)
			{
				builder.Append(chinesecharacter[random.Next(chinesecharacter.Length - 1)]);
			}
			installstation.ContactPhone=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 49; i++)
			{
				builder.Append(chinesecharacter[random.Next(chinesecharacter.Length - 1)]);
			}
			installstation.Email=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 6; i++)
			{
				builder.Append(chinesecharacter[random.Next(chinesecharacter.Length - 1)]);
			}
			installstation.CityCode=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 999; i++)
			{
				builder.Append(chinesecharacter[random.Next(chinesecharacter.Length - 1)]);
			}
			installstation.Note=builder.ToString();
			builder.Length=0;

			SingleMessage<bool> ret = service.AddInstallStation(installstation);
			//Assert.IsFalse(ret.IsSuccess);
		}


		/// <summary>
		/// 测试西文最大长度
		/// </summary>
		[TestMethod]
		public void TestStringForMaxLength_Spanish()
		{
			InstallStation installstation = new InstallStation();
			InstallStationService service = new InstallStationService();
			StringBuilder builder = new StringBuilder();
			Random random = new Random();
			for (int i = 0; i < 17; i++)
			{
				builder.Append(spanishcharacter[random.Next(spanishcharacter.Length - 1)]);
			}
			installstation.Id=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 255; i++)
			{
				builder.Append(spanishcharacter[random.Next(spanishcharacter.Length - 1)]);
			}
			installstation.Name=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 255; i++)
			{
				builder.Append(spanishcharacter[random.Next(spanishcharacter.Length - 1)]);
			}
			installstation.Address=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 24; i++)
			{
				builder.Append(spanishcharacter[random.Next(spanishcharacter.Length - 1)]);
			}
			installstation.Director=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 24; i++)
			{
				builder.Append(spanishcharacter[random.Next(spanishcharacter.Length - 1)]);
			}
			installstation.DirectorPhone=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 9; i++)
			{
				builder.Append(spanishcharacter[random.Next(spanishcharacter.Length - 1)]);
			}
			installstation.Contact=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 9; i++)
			{
				builder.Append(spanishcharacter[random.Next(spanishcharacter.Length - 1)]);
			}
			installstation.ContactPhone=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 49; i++)
			{
				builder.Append(spanishcharacter[random.Next(spanishcharacter.Length - 1)]);
			}
			installstation.Email=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 6; i++)
			{
				builder.Append(spanishcharacter[random.Next(spanishcharacter.Length - 1)]);
			}
			installstation.CityCode=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 999; i++)
			{
				builder.Append(spanishcharacter[random.Next(spanishcharacter.Length - 1)]);
			}
			installstation.Note=builder.ToString();
			builder.Length=0;

			SingleMessage<bool> ret = service.AddInstallStation(installstation);
			//Assert.IsFalse(ret.IsSuccess);
		}


		/// <summary>
		/// 测试英文最大长度
		/// </summary>
		[TestMethod]
		public void TestStringForMaxLength_English()
		{
			InstallStation installstation = new InstallStation();
			InstallStationService service = new InstallStationService();
			StringBuilder builder = new StringBuilder();
			Random random = new Random();
			for (int i = 0; i < 17; i++)
			{
				builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
			}
			installstation.Id=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 255; i++)
			{
				builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
			}
			installstation.Name=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 255; i++)
			{
				builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
			}
			installstation.Address=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 24; i++)
			{
				builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
			}
			installstation.Director=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 24; i++)
			{
				builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
			}
			installstation.DirectorPhone=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 9; i++)
			{
				builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
			}
			installstation.Contact=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 9; i++)
			{
				builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
			}
			installstation.ContactPhone=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 49; i++)
			{
				builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
			}
			installstation.Email=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 6; i++)
			{
				builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
			}
			installstation.CityCode=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 999; i++)
			{
				builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
			}
			installstation.Note=builder.ToString();
			builder.Length=0;

			SingleMessage<bool> ret = service.AddInstallStation(installstation);
			//Assert.IsFalse(ret.IsSuccess);
		}


		/// <summary>
		/// 测试Id中文超长
		/// </summary>
		[TestMethod]
		public void TestIdStringForOverLength_Chinese()
		{
			InstallStation installstation = new InstallStation();
			InstallStationService service = new InstallStationService();
			StringBuilder builder = new StringBuilder();
			Random random = new Random();
			for (int i = 0; i < 18; i++)
			{
				builder.Append(chinesecharacter[random.Next(chinesecharacter.Length - 1)]);
			}
			installstation.Id=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 255; i++)
			{
				builder.Append(chinesecharacter[random.Next(chinesecharacter.Length - 1)]);
			}
			installstation.Name=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 255; i++)
			{
				builder.Append(chinesecharacter[random.Next(chinesecharacter.Length - 1)]);
			}
			installstation.Address=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 24; i++)
			{
				builder.Append(chinesecharacter[random.Next(chinesecharacter.Length - 1)]);
			}
			installstation.Director=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 24; i++)
			{
				builder.Append(chinesecharacter[random.Next(chinesecharacter.Length - 1)]);
			}
			installstation.DirectorPhone=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 9; i++)
			{
				builder.Append(chinesecharacter[random.Next(chinesecharacter.Length - 1)]);
			}
			installstation.Contact=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 9; i++)
			{
				builder.Append(chinesecharacter[random.Next(chinesecharacter.Length - 1)]);
			}
			installstation.ContactPhone=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 49; i++)
			{
				builder.Append(chinesecharacter[random.Next(chinesecharacter.Length - 1)]);
			}
			installstation.Email=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 6; i++)
			{
				builder.Append(chinesecharacter[random.Next(chinesecharacter.Length - 1)]);
			}
			installstation.CityCode=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 999; i++)
			{
				builder.Append(chinesecharacter[random.Next(chinesecharacter.Length - 1)]);
			}
			installstation.Note=builder.ToString();
			builder.Length=0;

			SingleMessage<bool> ret = service.AddInstallStation(installstation);
			//Assert.IsFalse(ret.IsSuccess);
		}


		/// <summary>
		/// 测试Id西文超长
		/// </summary>
		[TestMethod]
		public void TestIdStringForMaxLength_Spanish()
		{
			InstallStation installstation = new InstallStation();
			InstallStationService service = new InstallStationService();
			StringBuilder builder = new StringBuilder();
			Random random = new Random();
			for (int i = 0; i < 18; i++)
			{
				builder.Append(spanishcharacter[random.Next(spanishcharacter.Length - 1)]);
			}
			installstation.Id=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 255; i++)
			{
				builder.Append(spanishcharacter[random.Next(spanishcharacter.Length - 1)]);
			}
			installstation.Name=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 255; i++)
			{
				builder.Append(spanishcharacter[random.Next(spanishcharacter.Length - 1)]);
			}
			installstation.Address=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 24; i++)
			{
				builder.Append(spanishcharacter[random.Next(spanishcharacter.Length - 1)]);
			}
			installstation.Director=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 24; i++)
			{
				builder.Append(spanishcharacter[random.Next(spanishcharacter.Length - 1)]);
			}
			installstation.DirectorPhone=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 9; i++)
			{
				builder.Append(spanishcharacter[random.Next(spanishcharacter.Length - 1)]);
			}
			installstation.Contact=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 9; i++)
			{
				builder.Append(spanishcharacter[random.Next(spanishcharacter.Length - 1)]);
			}
			installstation.ContactPhone=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 49; i++)
			{
				builder.Append(spanishcharacter[random.Next(spanishcharacter.Length - 1)]);
			}
			installstation.Email=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 6; i++)
			{
				builder.Append(spanishcharacter[random.Next(spanishcharacter.Length - 1)]);
			}
			installstation.CityCode=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 999; i++)
			{
				builder.Append(spanishcharacter[random.Next(spanishcharacter.Length - 1)]);
			}
			installstation.Note=builder.ToString();
			builder.Length=0;

			SingleMessage<bool> ret = service.AddInstallStation(installstation);
			//Assert.IsFalse(ret.IsSuccess);
		}


		/// <summary>
		/// 测试Id英文超长
		/// </summary>
		[TestMethod]
		public void TestIdStringForMaxLength_English()
		{
			InstallStation installstation = new InstallStation();
			InstallStationService service = new InstallStationService();
			StringBuilder builder = new StringBuilder();
			Random random = new Random();
			for (int i = 0; i < 18; i++)
			{
				builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
			}
			installstation.Id=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 255; i++)
			{
				builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
			}
			installstation.Name=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 255; i++)
			{
				builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
			}
			installstation.Address=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 24; i++)
			{
				builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
			}
			installstation.Director=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 24; i++)
			{
				builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
			}
			installstation.DirectorPhone=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 9; i++)
			{
				builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
			}
			installstation.Contact=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 9; i++)
			{
				builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
			}
			installstation.ContactPhone=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 49; i++)
			{
				builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
			}
			installstation.Email=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 6; i++)
			{
				builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
			}
			installstation.CityCode=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 999; i++)
			{
				builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
			}
			installstation.Note=builder.ToString();
			builder.Length=0;

			SingleMessage<bool> ret = service.AddInstallStation(installstation);
			//Assert.IsFalse(ret.IsSuccess);
		}


		/// <summary>
		/// 测试Name中文超长
		/// </summary>
		[TestMethod]
		public void TestNameStringForOverLength_Chinese()
		{
			InstallStation installstation = new InstallStation();
			InstallStationService service = new InstallStationService();
			StringBuilder builder = new StringBuilder();
			Random random = new Random();
			for (int i = 0; i < 17; i++)
			{
				builder.Append(chinesecharacter[random.Next(chinesecharacter.Length - 1)]);
			}
			installstation.Id=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 256; i++)
			{
				builder.Append(chinesecharacter[random.Next(chinesecharacter.Length - 1)]);
			}
			installstation.Name=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 255; i++)
			{
				builder.Append(chinesecharacter[random.Next(chinesecharacter.Length - 1)]);
			}
			installstation.Address=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 24; i++)
			{
				builder.Append(chinesecharacter[random.Next(chinesecharacter.Length - 1)]);
			}
			installstation.Director=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 24; i++)
			{
				builder.Append(chinesecharacter[random.Next(chinesecharacter.Length - 1)]);
			}
			installstation.DirectorPhone=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 9; i++)
			{
				builder.Append(chinesecharacter[random.Next(chinesecharacter.Length - 1)]);
			}
			installstation.Contact=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 9; i++)
			{
				builder.Append(chinesecharacter[random.Next(chinesecharacter.Length - 1)]);
			}
			installstation.ContactPhone=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 49; i++)
			{
				builder.Append(chinesecharacter[random.Next(chinesecharacter.Length - 1)]);
			}
			installstation.Email=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 6; i++)
			{
				builder.Append(chinesecharacter[random.Next(chinesecharacter.Length - 1)]);
			}
			installstation.CityCode=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 999; i++)
			{
				builder.Append(chinesecharacter[random.Next(chinesecharacter.Length - 1)]);
			}
			installstation.Note=builder.ToString();
			builder.Length=0;

			SingleMessage<bool> ret = service.AddInstallStation(installstation);
			//Assert.IsFalse(ret.IsSuccess);
		}


		/// <summary>
		/// 测试Name西文超长
		/// </summary>
		[TestMethod]
		public void TestNameStringForMaxLength_Spanish()
		{
			InstallStation installstation = new InstallStation();
			InstallStationService service = new InstallStationService();
			StringBuilder builder = new StringBuilder();
			Random random = new Random();
			for (int i = 0; i < 17; i++)
			{
				builder.Append(spanishcharacter[random.Next(spanishcharacter.Length - 1)]);
			}
			installstation.Id=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 256; i++)
			{
				builder.Append(spanishcharacter[random.Next(spanishcharacter.Length - 1)]);
			}
			installstation.Name=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 255; i++)
			{
				builder.Append(spanishcharacter[random.Next(spanishcharacter.Length - 1)]);
			}
			installstation.Address=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 24; i++)
			{
				builder.Append(spanishcharacter[random.Next(spanishcharacter.Length - 1)]);
			}
			installstation.Director=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 24; i++)
			{
				builder.Append(spanishcharacter[random.Next(spanishcharacter.Length - 1)]);
			}
			installstation.DirectorPhone=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 9; i++)
			{
				builder.Append(spanishcharacter[random.Next(spanishcharacter.Length - 1)]);
			}
			installstation.Contact=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 9; i++)
			{
				builder.Append(spanishcharacter[random.Next(spanishcharacter.Length - 1)]);
			}
			installstation.ContactPhone=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 49; i++)
			{
				builder.Append(spanishcharacter[random.Next(spanishcharacter.Length - 1)]);
			}
			installstation.Email=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 6; i++)
			{
				builder.Append(spanishcharacter[random.Next(spanishcharacter.Length - 1)]);
			}
			installstation.CityCode=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 999; i++)
			{
				builder.Append(spanishcharacter[random.Next(spanishcharacter.Length - 1)]);
			}
			installstation.Note=builder.ToString();
			builder.Length=0;

			SingleMessage<bool> ret = service.AddInstallStation(installstation);
			//Assert.IsFalse(ret.IsSuccess);
		}


		/// <summary>
		/// 测试Name英文超长
		/// </summary>
		[TestMethod]
		public void TestNameStringForMaxLength_English()
		{
			InstallStation installstation = new InstallStation();
			InstallStationService service = new InstallStationService();
			StringBuilder builder = new StringBuilder();
			Random random = new Random();
			for (int i = 0; i < 17; i++)
			{
				builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
			}
			installstation.Id=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 256; i++)
			{
				builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
			}
			installstation.Name=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 255; i++)
			{
				builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
			}
			installstation.Address=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 24; i++)
			{
				builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
			}
			installstation.Director=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 24; i++)
			{
				builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
			}
			installstation.DirectorPhone=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 9; i++)
			{
				builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
			}
			installstation.Contact=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 9; i++)
			{
				builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
			}
			installstation.ContactPhone=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 49; i++)
			{
				builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
			}
			installstation.Email=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 6; i++)
			{
				builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
			}
			installstation.CityCode=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 999; i++)
			{
				builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
			}
			installstation.Note=builder.ToString();
			builder.Length=0;

			SingleMessage<bool> ret = service.AddInstallStation(installstation);
			//Assert.IsFalse(ret.IsSuccess);
		}


		/// <summary>
		/// 测试Address中文超长
		/// </summary>
		[TestMethod]
		public void TestAddressStringForOverLength_Chinese()
		{
			InstallStation installstation = new InstallStation();
			InstallStationService service = new InstallStationService();
			StringBuilder builder = new StringBuilder();
			Random random = new Random();
			for (int i = 0; i < 17; i++)
			{
				builder.Append(chinesecharacter[random.Next(chinesecharacter.Length - 1)]);
			}
			installstation.Id=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 255; i++)
			{
				builder.Append(chinesecharacter[random.Next(chinesecharacter.Length - 1)]);
			}
			installstation.Name=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 256; i++)
			{
				builder.Append(chinesecharacter[random.Next(chinesecharacter.Length - 1)]);
			}
			installstation.Address=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 24; i++)
			{
				builder.Append(chinesecharacter[random.Next(chinesecharacter.Length - 1)]);
			}
			installstation.Director=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 24; i++)
			{
				builder.Append(chinesecharacter[random.Next(chinesecharacter.Length - 1)]);
			}
			installstation.DirectorPhone=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 9; i++)
			{
				builder.Append(chinesecharacter[random.Next(chinesecharacter.Length - 1)]);
			}
			installstation.Contact=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 9; i++)
			{
				builder.Append(chinesecharacter[random.Next(chinesecharacter.Length - 1)]);
			}
			installstation.ContactPhone=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 49; i++)
			{
				builder.Append(chinesecharacter[random.Next(chinesecharacter.Length - 1)]);
			}
			installstation.Email=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 6; i++)
			{
				builder.Append(chinesecharacter[random.Next(chinesecharacter.Length - 1)]);
			}
			installstation.CityCode=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 999; i++)
			{
				builder.Append(chinesecharacter[random.Next(chinesecharacter.Length - 1)]);
			}
			installstation.Note=builder.ToString();
			builder.Length=0;

			SingleMessage<bool> ret = service.AddInstallStation(installstation);
			//Assert.IsFalse(ret.IsSuccess);
		}


		/// <summary>
		/// 测试Address西文超长
		/// </summary>
		[TestMethod]
		public void TestAddressStringForMaxLength_Spanish()
		{
			InstallStation installstation = new InstallStation();
			InstallStationService service = new InstallStationService();
			StringBuilder builder = new StringBuilder();
			Random random = new Random();
			for (int i = 0; i < 17; i++)
			{
				builder.Append(spanishcharacter[random.Next(spanishcharacter.Length - 1)]);
			}
			installstation.Id=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 255; i++)
			{
				builder.Append(spanishcharacter[random.Next(spanishcharacter.Length - 1)]);
			}
			installstation.Name=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 256; i++)
			{
				builder.Append(spanishcharacter[random.Next(spanishcharacter.Length - 1)]);
			}
			installstation.Address=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 24; i++)
			{
				builder.Append(spanishcharacter[random.Next(spanishcharacter.Length - 1)]);
			}
			installstation.Director=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 24; i++)
			{
				builder.Append(spanishcharacter[random.Next(spanishcharacter.Length - 1)]);
			}
			installstation.DirectorPhone=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 9; i++)
			{
				builder.Append(spanishcharacter[random.Next(spanishcharacter.Length - 1)]);
			}
			installstation.Contact=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 9; i++)
			{
				builder.Append(spanishcharacter[random.Next(spanishcharacter.Length - 1)]);
			}
			installstation.ContactPhone=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 49; i++)
			{
				builder.Append(spanishcharacter[random.Next(spanishcharacter.Length - 1)]);
			}
			installstation.Email=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 6; i++)
			{
				builder.Append(spanishcharacter[random.Next(spanishcharacter.Length - 1)]);
			}
			installstation.CityCode=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 999; i++)
			{
				builder.Append(spanishcharacter[random.Next(spanishcharacter.Length - 1)]);
			}
			installstation.Note=builder.ToString();
			builder.Length=0;

			SingleMessage<bool> ret = service.AddInstallStation(installstation);
			//Assert.IsFalse(ret.IsSuccess);
		}


		/// <summary>
		/// 测试Address英文超长
		/// </summary>
		[TestMethod]
		public void TestAddressStringForMaxLength_English()
		{
			InstallStation installstation = new InstallStation();
			InstallStationService service = new InstallStationService();
			StringBuilder builder = new StringBuilder();
			Random random = new Random();
			for (int i = 0; i < 17; i++)
			{
				builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
			}
			installstation.Id=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 255; i++)
			{
				builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
			}
			installstation.Name=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 256; i++)
			{
				builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
			}
			installstation.Address=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 24; i++)
			{
				builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
			}
			installstation.Director=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 24; i++)
			{
				builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
			}
			installstation.DirectorPhone=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 9; i++)
			{
				builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
			}
			installstation.Contact=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 9; i++)
			{
				builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
			}
			installstation.ContactPhone=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 49; i++)
			{
				builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
			}
			installstation.Email=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 6; i++)
			{
				builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
			}
			installstation.CityCode=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 999; i++)
			{
				builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
			}
			installstation.Note=builder.ToString();
			builder.Length=0;

			SingleMessage<bool> ret = service.AddInstallStation(installstation);
			//Assert.IsFalse(ret.IsSuccess);
		}


		/// <summary>
		/// 测试Director中文超长
		/// </summary>
		[TestMethod]
		public void TestDirectorStringForOverLength_Chinese()
		{
			InstallStation installstation = new InstallStation();
			InstallStationService service = new InstallStationService();
			StringBuilder builder = new StringBuilder();
			Random random = new Random();
			for (int i = 0; i < 17; i++)
			{
				builder.Append(chinesecharacter[random.Next(chinesecharacter.Length - 1)]);
			}
			installstation.Id=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 255; i++)
			{
				builder.Append(chinesecharacter[random.Next(chinesecharacter.Length - 1)]);
			}
			installstation.Name=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 255; i++)
			{
				builder.Append(chinesecharacter[random.Next(chinesecharacter.Length - 1)]);
			}
			installstation.Address=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 25; i++)
			{
				builder.Append(chinesecharacter[random.Next(chinesecharacter.Length - 1)]);
			}
			installstation.Director=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 24; i++)
			{
				builder.Append(chinesecharacter[random.Next(chinesecharacter.Length - 1)]);
			}
			installstation.DirectorPhone=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 9; i++)
			{
				builder.Append(chinesecharacter[random.Next(chinesecharacter.Length - 1)]);
			}
			installstation.Contact=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 9; i++)
			{
				builder.Append(chinesecharacter[random.Next(chinesecharacter.Length - 1)]);
			}
			installstation.ContactPhone=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 49; i++)
			{
				builder.Append(chinesecharacter[random.Next(chinesecharacter.Length - 1)]);
			}
			installstation.Email=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 6; i++)
			{
				builder.Append(chinesecharacter[random.Next(chinesecharacter.Length - 1)]);
			}
			installstation.CityCode=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 999; i++)
			{
				builder.Append(chinesecharacter[random.Next(chinesecharacter.Length - 1)]);
			}
			installstation.Note=builder.ToString();
			builder.Length=0;

			SingleMessage<bool> ret = service.AddInstallStation(installstation);
			//Assert.IsFalse(ret.IsSuccess);
		}


		/// <summary>
		/// 测试Director西文超长
		/// </summary>
		[TestMethod]
		public void TestDirectorStringForMaxLength_Spanish()
		{
			InstallStation installstation = new InstallStation();
			InstallStationService service = new InstallStationService();
			StringBuilder builder = new StringBuilder();
			Random random = new Random();
			for (int i = 0; i < 17; i++)
			{
				builder.Append(spanishcharacter[random.Next(spanishcharacter.Length - 1)]);
			}
			installstation.Id=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 255; i++)
			{
				builder.Append(spanishcharacter[random.Next(spanishcharacter.Length - 1)]);
			}
			installstation.Name=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 255; i++)
			{
				builder.Append(spanishcharacter[random.Next(spanishcharacter.Length - 1)]);
			}
			installstation.Address=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 25; i++)
			{
				builder.Append(spanishcharacter[random.Next(spanishcharacter.Length - 1)]);
			}
			installstation.Director=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 24; i++)
			{
				builder.Append(spanishcharacter[random.Next(spanishcharacter.Length - 1)]);
			}
			installstation.DirectorPhone=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 9; i++)
			{
				builder.Append(spanishcharacter[random.Next(spanishcharacter.Length - 1)]);
			}
			installstation.Contact=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 9; i++)
			{
				builder.Append(spanishcharacter[random.Next(spanishcharacter.Length - 1)]);
			}
			installstation.ContactPhone=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 49; i++)
			{
				builder.Append(spanishcharacter[random.Next(spanishcharacter.Length - 1)]);
			}
			installstation.Email=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 6; i++)
			{
				builder.Append(spanishcharacter[random.Next(spanishcharacter.Length - 1)]);
			}
			installstation.CityCode=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 999; i++)
			{
				builder.Append(spanishcharacter[random.Next(spanishcharacter.Length - 1)]);
			}
			installstation.Note=builder.ToString();
			builder.Length=0;

			SingleMessage<bool> ret = service.AddInstallStation(installstation);
			//Assert.IsFalse(ret.IsSuccess);
		}


		/// <summary>
		/// 测试Director英文超长
		/// </summary>
		[TestMethod]
		public void TestDirectorStringForMaxLength_English()
		{
			InstallStation installstation = new InstallStation();
			InstallStationService service = new InstallStationService();
			StringBuilder builder = new StringBuilder();
			Random random = new Random();
			for (int i = 0; i < 17; i++)
			{
				builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
			}
			installstation.Id=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 255; i++)
			{
				builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
			}
			installstation.Name=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 255; i++)
			{
				builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
			}
			installstation.Address=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 25; i++)
			{
				builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
			}
			installstation.Director=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 24; i++)
			{
				builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
			}
			installstation.DirectorPhone=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 9; i++)
			{
				builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
			}
			installstation.Contact=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 9; i++)
			{
				builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
			}
			installstation.ContactPhone=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 49; i++)
			{
				builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
			}
			installstation.Email=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 6; i++)
			{
				builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
			}
			installstation.CityCode=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 999; i++)
			{
				builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
			}
			installstation.Note=builder.ToString();
			builder.Length=0;

			SingleMessage<bool> ret = service.AddInstallStation(installstation);
			//Assert.IsFalse(ret.IsSuccess);
		}


		/// <summary>
		/// 测试DirectorPhone中文超长
		/// </summary>
		[TestMethod]
		public void TestDirectorPhoneStringForOverLength_Chinese()
		{
			InstallStation installstation = new InstallStation();
			InstallStationService service = new InstallStationService();
			StringBuilder builder = new StringBuilder();
			Random random = new Random();
			for (int i = 0; i < 17; i++)
			{
				builder.Append(chinesecharacter[random.Next(chinesecharacter.Length - 1)]);
			}
			installstation.Id=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 255; i++)
			{
				builder.Append(chinesecharacter[random.Next(chinesecharacter.Length - 1)]);
			}
			installstation.Name=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 255; i++)
			{
				builder.Append(chinesecharacter[random.Next(chinesecharacter.Length - 1)]);
			}
			installstation.Address=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 24; i++)
			{
				builder.Append(chinesecharacter[random.Next(chinesecharacter.Length - 1)]);
			}
			installstation.Director=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 25; i++)
			{
				builder.Append(chinesecharacter[random.Next(chinesecharacter.Length - 1)]);
			}
			installstation.DirectorPhone=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 9; i++)
			{
				builder.Append(chinesecharacter[random.Next(chinesecharacter.Length - 1)]);
			}
			installstation.Contact=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 9; i++)
			{
				builder.Append(chinesecharacter[random.Next(chinesecharacter.Length - 1)]);
			}
			installstation.ContactPhone=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 49; i++)
			{
				builder.Append(chinesecharacter[random.Next(chinesecharacter.Length - 1)]);
			}
			installstation.Email=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 6; i++)
			{
				builder.Append(chinesecharacter[random.Next(chinesecharacter.Length - 1)]);
			}
			installstation.CityCode=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 999; i++)
			{
				builder.Append(chinesecharacter[random.Next(chinesecharacter.Length - 1)]);
			}
			installstation.Note=builder.ToString();
			builder.Length=0;

			SingleMessage<bool> ret = service.AddInstallStation(installstation);
			//Assert.IsFalse(ret.IsSuccess);
		}


		/// <summary>
		/// 测试DirectorPhone西文超长
		/// </summary>
		[TestMethod]
		public void TestDirectorPhoneStringForMaxLength_Spanish()
		{
			InstallStation installstation = new InstallStation();
			InstallStationService service = new InstallStationService();
			StringBuilder builder = new StringBuilder();
			Random random = new Random();
			for (int i = 0; i < 17; i++)
			{
				builder.Append(spanishcharacter[random.Next(spanishcharacter.Length - 1)]);
			}
			installstation.Id=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 255; i++)
			{
				builder.Append(spanishcharacter[random.Next(spanishcharacter.Length - 1)]);
			}
			installstation.Name=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 255; i++)
			{
				builder.Append(spanishcharacter[random.Next(spanishcharacter.Length - 1)]);
			}
			installstation.Address=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 24; i++)
			{
				builder.Append(spanishcharacter[random.Next(spanishcharacter.Length - 1)]);
			}
			installstation.Director=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 25; i++)
			{
				builder.Append(spanishcharacter[random.Next(spanishcharacter.Length - 1)]);
			}
			installstation.DirectorPhone=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 9; i++)
			{
				builder.Append(spanishcharacter[random.Next(spanishcharacter.Length - 1)]);
			}
			installstation.Contact=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 9; i++)
			{
				builder.Append(spanishcharacter[random.Next(spanishcharacter.Length - 1)]);
			}
			installstation.ContactPhone=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 49; i++)
			{
				builder.Append(spanishcharacter[random.Next(spanishcharacter.Length - 1)]);
			}
			installstation.Email=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 6; i++)
			{
				builder.Append(spanishcharacter[random.Next(spanishcharacter.Length - 1)]);
			}
			installstation.CityCode=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 999; i++)
			{
				builder.Append(spanishcharacter[random.Next(spanishcharacter.Length - 1)]);
			}
			installstation.Note=builder.ToString();
			builder.Length=0;

			SingleMessage<bool> ret = service.AddInstallStation(installstation);
			//Assert.IsFalse(ret.IsSuccess);
		}


		/// <summary>
		/// 测试DirectorPhone英文超长
		/// </summary>
		[TestMethod]
		public void TestDirectorPhoneStringForMaxLength_English()
		{
			InstallStation installstation = new InstallStation();
			InstallStationService service = new InstallStationService();
			StringBuilder builder = new StringBuilder();
			Random random = new Random();
			for (int i = 0; i < 17; i++)
			{
				builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
			}
			installstation.Id=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 255; i++)
			{
				builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
			}
			installstation.Name=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 255; i++)
			{
				builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
			}
			installstation.Address=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 24; i++)
			{
				builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
			}
			installstation.Director=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 25; i++)
			{
				builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
			}
			installstation.DirectorPhone=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 9; i++)
			{
				builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
			}
			installstation.Contact=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 9; i++)
			{
				builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
			}
			installstation.ContactPhone=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 49; i++)
			{
				builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
			}
			installstation.Email=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 6; i++)
			{
				builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
			}
			installstation.CityCode=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 999; i++)
			{
				builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
			}
			installstation.Note=builder.ToString();
			builder.Length=0;

			SingleMessage<bool> ret = service.AddInstallStation(installstation);
			//Assert.IsFalse(ret.IsSuccess);
		}


		/// <summary>
		/// 测试Contact中文超长
		/// </summary>
		[TestMethod]
		public void TestContactStringForOverLength_Chinese()
		{
			InstallStation installstation = new InstallStation();
			InstallStationService service = new InstallStationService();
			StringBuilder builder = new StringBuilder();
			Random random = new Random();
			for (int i = 0; i < 17; i++)
			{
				builder.Append(chinesecharacter[random.Next(chinesecharacter.Length - 1)]);
			}
			installstation.Id=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 255; i++)
			{
				builder.Append(chinesecharacter[random.Next(chinesecharacter.Length - 1)]);
			}
			installstation.Name=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 255; i++)
			{
				builder.Append(chinesecharacter[random.Next(chinesecharacter.Length - 1)]);
			}
			installstation.Address=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 24; i++)
			{
				builder.Append(chinesecharacter[random.Next(chinesecharacter.Length - 1)]);
			}
			installstation.Director=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 24; i++)
			{
				builder.Append(chinesecharacter[random.Next(chinesecharacter.Length - 1)]);
			}
			installstation.DirectorPhone=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 10; i++)
			{
				builder.Append(chinesecharacter[random.Next(chinesecharacter.Length - 1)]);
			}
			installstation.Contact=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 9; i++)
			{
				builder.Append(chinesecharacter[random.Next(chinesecharacter.Length - 1)]);
			}
			installstation.ContactPhone=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 49; i++)
			{
				builder.Append(chinesecharacter[random.Next(chinesecharacter.Length - 1)]);
			}
			installstation.Email=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 6; i++)
			{
				builder.Append(chinesecharacter[random.Next(chinesecharacter.Length - 1)]);
			}
			installstation.CityCode=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 999; i++)
			{
				builder.Append(chinesecharacter[random.Next(chinesecharacter.Length - 1)]);
			}
			installstation.Note=builder.ToString();
			builder.Length=0;

			SingleMessage<bool> ret = service.AddInstallStation(installstation);
			//Assert.IsFalse(ret.IsSuccess);
		}


		/// <summary>
		/// 测试Contact西文超长
		/// </summary>
		[TestMethod]
		public void TestContactStringForMaxLength_Spanish()
		{
			InstallStation installstation = new InstallStation();
			InstallStationService service = new InstallStationService();
			StringBuilder builder = new StringBuilder();
			Random random = new Random();
			for (int i = 0; i < 17; i++)
			{
				builder.Append(spanishcharacter[random.Next(spanishcharacter.Length - 1)]);
			}
			installstation.Id=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 255; i++)
			{
				builder.Append(spanishcharacter[random.Next(spanishcharacter.Length - 1)]);
			}
			installstation.Name=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 255; i++)
			{
				builder.Append(spanishcharacter[random.Next(spanishcharacter.Length - 1)]);
			}
			installstation.Address=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 24; i++)
			{
				builder.Append(spanishcharacter[random.Next(spanishcharacter.Length - 1)]);
			}
			installstation.Director=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 24; i++)
			{
				builder.Append(spanishcharacter[random.Next(spanishcharacter.Length - 1)]);
			}
			installstation.DirectorPhone=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 10; i++)
			{
				builder.Append(spanishcharacter[random.Next(spanishcharacter.Length - 1)]);
			}
			installstation.Contact=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 9; i++)
			{
				builder.Append(spanishcharacter[random.Next(spanishcharacter.Length - 1)]);
			}
			installstation.ContactPhone=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 49; i++)
			{
				builder.Append(spanishcharacter[random.Next(spanishcharacter.Length - 1)]);
			}
			installstation.Email=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 6; i++)
			{
				builder.Append(spanishcharacter[random.Next(spanishcharacter.Length - 1)]);
			}
			installstation.CityCode=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 999; i++)
			{
				builder.Append(spanishcharacter[random.Next(spanishcharacter.Length - 1)]);
			}
			installstation.Note=builder.ToString();
			builder.Length=0;

			SingleMessage<bool> ret = service.AddInstallStation(installstation);
			//Assert.IsFalse(ret.IsSuccess);
		}


		/// <summary>
		/// 测试Contact英文超长
		/// </summary>
		[TestMethod]
		public void TestContactStringForMaxLength_English()
		{
			InstallStation installstation = new InstallStation();
			InstallStationService service = new InstallStationService();
			StringBuilder builder = new StringBuilder();
			Random random = new Random();
			for (int i = 0; i < 17; i++)
			{
				builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
			}
			installstation.Id=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 255; i++)
			{
				builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
			}
			installstation.Name=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 255; i++)
			{
				builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
			}
			installstation.Address=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 24; i++)
			{
				builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
			}
			installstation.Director=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 24; i++)
			{
				builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
			}
			installstation.DirectorPhone=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 10; i++)
			{
				builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
			}
			installstation.Contact=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 9; i++)
			{
				builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
			}
			installstation.ContactPhone=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 49; i++)
			{
				builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
			}
			installstation.Email=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 6; i++)
			{
				builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
			}
			installstation.CityCode=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 999; i++)
			{
				builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
			}
			installstation.Note=builder.ToString();
			builder.Length=0;

			SingleMessage<bool> ret = service.AddInstallStation(installstation);
			//Assert.IsFalse(ret.IsSuccess);
		}


		/// <summary>
		/// 测试ContactPhone中文超长
		/// </summary>
		[TestMethod]
		public void TestContactPhoneStringForOverLength_Chinese()
		{
			InstallStation installstation = new InstallStation();
			InstallStationService service = new InstallStationService();
			StringBuilder builder = new StringBuilder();
			Random random = new Random();
			for (int i = 0; i < 17; i++)
			{
				builder.Append(chinesecharacter[random.Next(chinesecharacter.Length - 1)]);
			}
			installstation.Id=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 255; i++)
			{
				builder.Append(chinesecharacter[random.Next(chinesecharacter.Length - 1)]);
			}
			installstation.Name=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 255; i++)
			{
				builder.Append(chinesecharacter[random.Next(chinesecharacter.Length - 1)]);
			}
			installstation.Address=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 24; i++)
			{
				builder.Append(chinesecharacter[random.Next(chinesecharacter.Length - 1)]);
			}
			installstation.Director=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 24; i++)
			{
				builder.Append(chinesecharacter[random.Next(chinesecharacter.Length - 1)]);
			}
			installstation.DirectorPhone=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 9; i++)
			{
				builder.Append(chinesecharacter[random.Next(chinesecharacter.Length - 1)]);
			}
			installstation.Contact=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 10; i++)
			{
				builder.Append(chinesecharacter[random.Next(chinesecharacter.Length - 1)]);
			}
			installstation.ContactPhone=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 49; i++)
			{
				builder.Append(chinesecharacter[random.Next(chinesecharacter.Length - 1)]);
			}
			installstation.Email=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 6; i++)
			{
				builder.Append(chinesecharacter[random.Next(chinesecharacter.Length - 1)]);
			}
			installstation.CityCode=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 999; i++)
			{
				builder.Append(chinesecharacter[random.Next(chinesecharacter.Length - 1)]);
			}
			installstation.Note=builder.ToString();
			builder.Length=0;

			SingleMessage<bool> ret = service.AddInstallStation(installstation);
			//Assert.IsFalse(ret.IsSuccess);
		}


		/// <summary>
		/// 测试ContactPhone西文超长
		/// </summary>
		[TestMethod]
		public void TestContactPhoneStringForMaxLength_Spanish()
		{
			InstallStation installstation = new InstallStation();
			InstallStationService service = new InstallStationService();
			StringBuilder builder = new StringBuilder();
			Random random = new Random();
			for (int i = 0; i < 17; i++)
			{
				builder.Append(spanishcharacter[random.Next(spanishcharacter.Length - 1)]);
			}
			installstation.Id=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 255; i++)
			{
				builder.Append(spanishcharacter[random.Next(spanishcharacter.Length - 1)]);
			}
			installstation.Name=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 255; i++)
			{
				builder.Append(spanishcharacter[random.Next(spanishcharacter.Length - 1)]);
			}
			installstation.Address=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 24; i++)
			{
				builder.Append(spanishcharacter[random.Next(spanishcharacter.Length - 1)]);
			}
			installstation.Director=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 24; i++)
			{
				builder.Append(spanishcharacter[random.Next(spanishcharacter.Length - 1)]);
			}
			installstation.DirectorPhone=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 9; i++)
			{
				builder.Append(spanishcharacter[random.Next(spanishcharacter.Length - 1)]);
			}
			installstation.Contact=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 10; i++)
			{
				builder.Append(spanishcharacter[random.Next(spanishcharacter.Length - 1)]);
			}
			installstation.ContactPhone=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 49; i++)
			{
				builder.Append(spanishcharacter[random.Next(spanishcharacter.Length - 1)]);
			}
			installstation.Email=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 6; i++)
			{
				builder.Append(spanishcharacter[random.Next(spanishcharacter.Length - 1)]);
			}
			installstation.CityCode=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 999; i++)
			{
				builder.Append(spanishcharacter[random.Next(spanishcharacter.Length - 1)]);
			}
			installstation.Note=builder.ToString();
			builder.Length=0;

			SingleMessage<bool> ret = service.AddInstallStation(installstation);
			//Assert.IsFalse(ret.IsSuccess);
		}


		/// <summary>
		/// 测试ContactPhone英文超长
		/// </summary>
		[TestMethod]
		public void TestContactPhoneStringForMaxLength_English()
		{
			InstallStation installstation = new InstallStation();
			InstallStationService service = new InstallStationService();
			StringBuilder builder = new StringBuilder();
			Random random = new Random();
			for (int i = 0; i < 17; i++)
			{
				builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
			}
			installstation.Id=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 255; i++)
			{
				builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
			}
			installstation.Name=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 255; i++)
			{
				builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
			}
			installstation.Address=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 24; i++)
			{
				builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
			}
			installstation.Director=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 24; i++)
			{
				builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
			}
			installstation.DirectorPhone=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 9; i++)
			{
				builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
			}
			installstation.Contact=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 10; i++)
			{
				builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
			}
			installstation.ContactPhone=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 49; i++)
			{
				builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
			}
			installstation.Email=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 6; i++)
			{
				builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
			}
			installstation.CityCode=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 999; i++)
			{
				builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
			}
			installstation.Note=builder.ToString();
			builder.Length=0;

			SingleMessage<bool> ret = service.AddInstallStation(installstation);
			//Assert.IsFalse(ret.IsSuccess);
		}


		/// <summary>
		/// 测试Email中文超长
		/// </summary>
		[TestMethod]
		public void TestEmailStringForOverLength_Chinese()
		{
			InstallStation installstation = new InstallStation();
			InstallStationService service = new InstallStationService();
			StringBuilder builder = new StringBuilder();
			Random random = new Random();
			for (int i = 0; i < 17; i++)
			{
				builder.Append(chinesecharacter[random.Next(chinesecharacter.Length - 1)]);
			}
			installstation.Id=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 255; i++)
			{
				builder.Append(chinesecharacter[random.Next(chinesecharacter.Length - 1)]);
			}
			installstation.Name=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 255; i++)
			{
				builder.Append(chinesecharacter[random.Next(chinesecharacter.Length - 1)]);
			}
			installstation.Address=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 24; i++)
			{
				builder.Append(chinesecharacter[random.Next(chinesecharacter.Length - 1)]);
			}
			installstation.Director=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 24; i++)
			{
				builder.Append(chinesecharacter[random.Next(chinesecharacter.Length - 1)]);
			}
			installstation.DirectorPhone=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 9; i++)
			{
				builder.Append(chinesecharacter[random.Next(chinesecharacter.Length - 1)]);
			}
			installstation.Contact=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 9; i++)
			{
				builder.Append(chinesecharacter[random.Next(chinesecharacter.Length - 1)]);
			}
			installstation.ContactPhone=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 50; i++)
			{
				builder.Append(chinesecharacter[random.Next(chinesecharacter.Length - 1)]);
			}
			installstation.Email=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 6; i++)
			{
				builder.Append(chinesecharacter[random.Next(chinesecharacter.Length - 1)]);
			}
			installstation.CityCode=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 999; i++)
			{
				builder.Append(chinesecharacter[random.Next(chinesecharacter.Length - 1)]);
			}
			installstation.Note=builder.ToString();
			builder.Length=0;

			SingleMessage<bool> ret = service.AddInstallStation(installstation);
			//Assert.IsFalse(ret.IsSuccess);
		}


		/// <summary>
		/// 测试Email西文超长
		/// </summary>
		[TestMethod]
		public void TestEmailStringForMaxLength_Spanish()
		{
			InstallStation installstation = new InstallStation();
			InstallStationService service = new InstallStationService();
			StringBuilder builder = new StringBuilder();
			Random random = new Random();
			for (int i = 0; i < 17; i++)
			{
				builder.Append(spanishcharacter[random.Next(spanishcharacter.Length - 1)]);
			}
			installstation.Id=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 255; i++)
			{
				builder.Append(spanishcharacter[random.Next(spanishcharacter.Length - 1)]);
			}
			installstation.Name=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 255; i++)
			{
				builder.Append(spanishcharacter[random.Next(spanishcharacter.Length - 1)]);
			}
			installstation.Address=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 24; i++)
			{
				builder.Append(spanishcharacter[random.Next(spanishcharacter.Length - 1)]);
			}
			installstation.Director=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 24; i++)
			{
				builder.Append(spanishcharacter[random.Next(spanishcharacter.Length - 1)]);
			}
			installstation.DirectorPhone=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 9; i++)
			{
				builder.Append(spanishcharacter[random.Next(spanishcharacter.Length - 1)]);
			}
			installstation.Contact=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 9; i++)
			{
				builder.Append(spanishcharacter[random.Next(spanishcharacter.Length - 1)]);
			}
			installstation.ContactPhone=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 50; i++)
			{
				builder.Append(spanishcharacter[random.Next(spanishcharacter.Length - 1)]);
			}
			installstation.Email=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 6; i++)
			{
				builder.Append(spanishcharacter[random.Next(spanishcharacter.Length - 1)]);
			}
			installstation.CityCode=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 999; i++)
			{
				builder.Append(spanishcharacter[random.Next(spanishcharacter.Length - 1)]);
			}
			installstation.Note=builder.ToString();
			builder.Length=0;

			SingleMessage<bool> ret = service.AddInstallStation(installstation);
			//Assert.IsFalse(ret.IsSuccess);
		}


		/// <summary>
		/// 测试Email英文超长
		/// </summary>
		[TestMethod]
		public void TestEmailStringForMaxLength_English()
		{
			InstallStation installstation = new InstallStation();
			InstallStationService service = new InstallStationService();
			StringBuilder builder = new StringBuilder();
			Random random = new Random();
			for (int i = 0; i < 17; i++)
			{
				builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
			}
			installstation.Id=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 255; i++)
			{
				builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
			}
			installstation.Name=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 255; i++)
			{
				builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
			}
			installstation.Address=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 24; i++)
			{
				builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
			}
			installstation.Director=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 24; i++)
			{
				builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
			}
			installstation.DirectorPhone=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 9; i++)
			{
				builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
			}
			installstation.Contact=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 9; i++)
			{
				builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
			}
			installstation.ContactPhone=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 50; i++)
			{
				builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
			}
			installstation.Email=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 6; i++)
			{
				builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
			}
			installstation.CityCode=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 999; i++)
			{
				builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
			}
			installstation.Note=builder.ToString();
			builder.Length=0;

			SingleMessage<bool> ret = service.AddInstallStation(installstation);
			//Assert.IsFalse(ret.IsSuccess);
		}


		/// <summary>
		/// 测试ProvinceCode中文超长
		/// </summary>
		[TestMethod]
		public void TestProvinceCodeStringForOverLength_Chinese()
		{
			InstallStation installstation = new InstallStation();
			InstallStationService service = new InstallStationService();
			StringBuilder builder = new StringBuilder();
			Random random = new Random();
			for (int i = 0; i < 17; i++)
			{
				builder.Append(chinesecharacter[random.Next(chinesecharacter.Length - 1)]);
			}
			installstation.Id=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 255; i++)
			{
				builder.Append(chinesecharacter[random.Next(chinesecharacter.Length - 1)]);
			}
			installstation.Name=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 255; i++)
			{
				builder.Append(chinesecharacter[random.Next(chinesecharacter.Length - 1)]);
			}
			installstation.Address=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 24; i++)
			{
				builder.Append(chinesecharacter[random.Next(chinesecharacter.Length - 1)]);
			}
			installstation.Director=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 24; i++)
			{
				builder.Append(chinesecharacter[random.Next(chinesecharacter.Length - 1)]);
			}
			installstation.DirectorPhone=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 9; i++)
			{
				builder.Append(chinesecharacter[random.Next(chinesecharacter.Length - 1)]);
			}
			installstation.Contact=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 9; i++)
			{
				builder.Append(chinesecharacter[random.Next(chinesecharacter.Length - 1)]);
			}
			installstation.ContactPhone=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 49; i++)
			{
				builder.Append(chinesecharacter[random.Next(chinesecharacter.Length - 1)]);
			}
			installstation.Email=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 6; i++)
			{
				builder.Append(chinesecharacter[random.Next(chinesecharacter.Length - 1)]);
			}
			installstation.CityCode=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 999; i++)
			{
				builder.Append(chinesecharacter[random.Next(chinesecharacter.Length - 1)]);
			}
			installstation.Note=builder.ToString();
			builder.Length=0;

			SingleMessage<bool> ret = service.AddInstallStation(installstation);
			//Assert.IsFalse(ret.IsSuccess);
		}


		/// <summary>
		/// 测试ProvinceCode西文超长
		/// </summary>
		[TestMethod]
		public void TestProvinceCodeStringForMaxLength_Spanish()
		{
			InstallStation installstation = new InstallStation();
			InstallStationService service = new InstallStationService();
			StringBuilder builder = new StringBuilder();
			Random random = new Random();
			for (int i = 0; i < 17; i++)
			{
				builder.Append(spanishcharacter[random.Next(spanishcharacter.Length - 1)]);
			}
			installstation.Id=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 255; i++)
			{
				builder.Append(spanishcharacter[random.Next(spanishcharacter.Length - 1)]);
			}
			installstation.Name=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 255; i++)
			{
				builder.Append(spanishcharacter[random.Next(spanishcharacter.Length - 1)]);
			}
			installstation.Address=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 24; i++)
			{
				builder.Append(spanishcharacter[random.Next(spanishcharacter.Length - 1)]);
			}
			installstation.Director=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 24; i++)
			{
				builder.Append(spanishcharacter[random.Next(spanishcharacter.Length - 1)]);
			}
			installstation.DirectorPhone=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 9; i++)
			{
				builder.Append(spanishcharacter[random.Next(spanishcharacter.Length - 1)]);
			}
			installstation.Contact=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 9; i++)
			{
				builder.Append(spanishcharacter[random.Next(spanishcharacter.Length - 1)]);
			}
			installstation.ContactPhone=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 49; i++)
			{
				builder.Append(spanishcharacter[random.Next(spanishcharacter.Length - 1)]);
			}
			installstation.Email=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 6; i++)
			{
				builder.Append(spanishcharacter[random.Next(spanishcharacter.Length - 1)]);
			}
			installstation.CityCode=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 999; i++)
			{
				builder.Append(spanishcharacter[random.Next(spanishcharacter.Length - 1)]);
			}
			installstation.Note=builder.ToString();
			builder.Length=0;

			SingleMessage<bool> ret = service.AddInstallStation(installstation);
			//Assert.IsFalse(ret.IsSuccess);
		}


		/// <summary>
		/// 测试ProvinceCode英文超长
		/// </summary>
		[TestMethod]
		public void TestProvinceCodeStringForMaxLength_English()
		{
			InstallStation installstation = new InstallStation();
			InstallStationService service = new InstallStationService();
			StringBuilder builder = new StringBuilder();
			Random random = new Random();
			for (int i = 0; i < 17; i++)
			{
				builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
			}
			installstation.Id=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 255; i++)
			{
				builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
			}
			installstation.Name=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 255; i++)
			{
				builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
			}
			installstation.Address=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 24; i++)
			{
				builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
			}
			installstation.Director=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 24; i++)
			{
				builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
			}
			installstation.DirectorPhone=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 9; i++)
			{
				builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
			}
			installstation.Contact=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 9; i++)
			{
				builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
			}
			installstation.ContactPhone=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 49; i++)
			{
				builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
			}
			installstation.Email=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 6; i++)
			{
				builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
			}
			installstation.CityCode=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 999; i++)
			{
				builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
			}
			installstation.Note=builder.ToString();
			builder.Length=0;

			SingleMessage<bool> ret = service.AddInstallStation(installstation);
			//Assert.IsFalse(ret.IsSuccess);
		}


		/// <summary>
		/// 测试ProvinceName中文超长
		/// </summary>
		[TestMethod]
		public void TestProvinceNameStringForOverLength_Chinese()
		{
			InstallStation installstation = new InstallStation();
			InstallStationService service = new InstallStationService();
			StringBuilder builder = new StringBuilder();
			Random random = new Random();
			for (int i = 0; i < 17; i++)
			{
				builder.Append(chinesecharacter[random.Next(chinesecharacter.Length - 1)]);
			}
			installstation.Id=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 255; i++)
			{
				builder.Append(chinesecharacter[random.Next(chinesecharacter.Length - 1)]);
			}
			installstation.Name=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 255; i++)
			{
				builder.Append(chinesecharacter[random.Next(chinesecharacter.Length - 1)]);
			}
			installstation.Address=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 24; i++)
			{
				builder.Append(chinesecharacter[random.Next(chinesecharacter.Length - 1)]);
			}
			installstation.Director=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 24; i++)
			{
				builder.Append(chinesecharacter[random.Next(chinesecharacter.Length - 1)]);
			}
			installstation.DirectorPhone=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 9; i++)
			{
				builder.Append(chinesecharacter[random.Next(chinesecharacter.Length - 1)]);
			}
			installstation.Contact=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 9; i++)
			{
				builder.Append(chinesecharacter[random.Next(chinesecharacter.Length - 1)]);
			}
			installstation.ContactPhone=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 49; i++)
			{
				builder.Append(chinesecharacter[random.Next(chinesecharacter.Length - 1)]);
			}
			installstation.Email=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 6; i++)
			{
				builder.Append(chinesecharacter[random.Next(chinesecharacter.Length - 1)]);
			}
			installstation.CityCode=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 999; i++)
			{
				builder.Append(chinesecharacter[random.Next(chinesecharacter.Length - 1)]);
			}
			installstation.Note=builder.ToString();
			builder.Length=0;

			SingleMessage<bool> ret = service.AddInstallStation(installstation);
			//Assert.IsFalse(ret.IsSuccess);
		}


		/// <summary>
		/// 测试ProvinceName西文超长
		/// </summary>
		[TestMethod]
		public void TestProvinceNameStringForMaxLength_Spanish()
		{
			InstallStation installstation = new InstallStation();
			InstallStationService service = new InstallStationService();
			StringBuilder builder = new StringBuilder();
			Random random = new Random();
			for (int i = 0; i < 17; i++)
			{
				builder.Append(spanishcharacter[random.Next(spanishcharacter.Length - 1)]);
			}
			installstation.Id=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 255; i++)
			{
				builder.Append(spanishcharacter[random.Next(spanishcharacter.Length - 1)]);
			}
			installstation.Name=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 255; i++)
			{
				builder.Append(spanishcharacter[random.Next(spanishcharacter.Length - 1)]);
			}
			installstation.Address=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 24; i++)
			{
				builder.Append(spanishcharacter[random.Next(spanishcharacter.Length - 1)]);
			}
			installstation.Director=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 24; i++)
			{
				builder.Append(spanishcharacter[random.Next(spanishcharacter.Length - 1)]);
			}
			installstation.DirectorPhone=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 9; i++)
			{
				builder.Append(spanishcharacter[random.Next(spanishcharacter.Length - 1)]);
			}
			installstation.Contact=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 9; i++)
			{
				builder.Append(spanishcharacter[random.Next(spanishcharacter.Length - 1)]);
			}
			installstation.ContactPhone=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 49; i++)
			{
				builder.Append(spanishcharacter[random.Next(spanishcharacter.Length - 1)]);
			}
			installstation.Email=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 6; i++)
			{
				builder.Append(spanishcharacter[random.Next(spanishcharacter.Length - 1)]);
			}
			installstation.CityCode=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 999; i++)
			{
				builder.Append(spanishcharacter[random.Next(spanishcharacter.Length - 1)]);
			}
			installstation.Note=builder.ToString();
			builder.Length=0;

			SingleMessage<bool> ret = service.AddInstallStation(installstation);
			//Assert.IsFalse(ret.IsSuccess);
		}


		/// <summary>
		/// 测试ProvinceName英文超长
		/// </summary>
		[TestMethod]
		public void TestProvinceNameStringForMaxLength_English()
		{
			InstallStation installstation = new InstallStation();
			InstallStationService service = new InstallStationService();
			StringBuilder builder = new StringBuilder();
			Random random = new Random();
			for (int i = 0; i < 17; i++)
			{
				builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
			}
			installstation.Id=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 255; i++)
			{
				builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
			}
			installstation.Name=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 255; i++)
			{
				builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
			}
			installstation.Address=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 24; i++)
			{
				builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
			}
			installstation.Director=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 24; i++)
			{
				builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
			}
			installstation.DirectorPhone=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 9; i++)
			{
				builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
			}
			installstation.Contact=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 9; i++)
			{
				builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
			}
			installstation.ContactPhone=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 49; i++)
			{
				builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
			}
			installstation.Email=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 6; i++)
			{
				builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
			}
			installstation.CityCode=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 999; i++)
			{
				builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
			}
			installstation.Note=builder.ToString();
			builder.Length=0;

			SingleMessage<bool> ret = service.AddInstallStation(installstation);
			//Assert.IsFalse(ret.IsSuccess);
		}


		/// <summary>
		/// 测试CityCode中文超长
		/// </summary>
		[TestMethod]
		public void TestCityCodeStringForOverLength_Chinese()
		{
			InstallStation installstation = new InstallStation();
			InstallStationService service = new InstallStationService();
			StringBuilder builder = new StringBuilder();
			Random random = new Random();
			for (int i = 0; i < 17; i++)
			{
				builder.Append(chinesecharacter[random.Next(chinesecharacter.Length - 1)]);
			}
			installstation.Id=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 255; i++)
			{
				builder.Append(chinesecharacter[random.Next(chinesecharacter.Length - 1)]);
			}
			installstation.Name=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 255; i++)
			{
				builder.Append(chinesecharacter[random.Next(chinesecharacter.Length - 1)]);
			}
			installstation.Address=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 24; i++)
			{
				builder.Append(chinesecharacter[random.Next(chinesecharacter.Length - 1)]);
			}
			installstation.Director=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 24; i++)
			{
				builder.Append(chinesecharacter[random.Next(chinesecharacter.Length - 1)]);
			}
			installstation.DirectorPhone=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 9; i++)
			{
				builder.Append(chinesecharacter[random.Next(chinesecharacter.Length - 1)]);
			}
			installstation.Contact=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 9; i++)
			{
				builder.Append(chinesecharacter[random.Next(chinesecharacter.Length - 1)]);
			}
			installstation.ContactPhone=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 49; i++)
			{
				builder.Append(chinesecharacter[random.Next(chinesecharacter.Length - 1)]);
			}
			installstation.Email=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 7; i++)
			{
				builder.Append(chinesecharacter[random.Next(chinesecharacter.Length - 1)]);
			}
			installstation.CityCode=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 999; i++)
			{
				builder.Append(chinesecharacter[random.Next(chinesecharacter.Length - 1)]);
			}
			installstation.Note=builder.ToString();
			builder.Length=0;

			SingleMessage<bool> ret = service.AddInstallStation(installstation);
			//Assert.IsFalse(ret.IsSuccess);
		}


		/// <summary>
		/// 测试CityCode西文超长
		/// </summary>
		[TestMethod]
		public void TestCityCodeStringForMaxLength_Spanish()
		{
			InstallStation installstation = new InstallStation();
			InstallStationService service = new InstallStationService();
			StringBuilder builder = new StringBuilder();
			Random random = new Random();
			for (int i = 0; i < 17; i++)
			{
				builder.Append(spanishcharacter[random.Next(spanishcharacter.Length - 1)]);
			}
			installstation.Id=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 255; i++)
			{
				builder.Append(spanishcharacter[random.Next(spanishcharacter.Length - 1)]);
			}
			installstation.Name=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 255; i++)
			{
				builder.Append(spanishcharacter[random.Next(spanishcharacter.Length - 1)]);
			}
			installstation.Address=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 24; i++)
			{
				builder.Append(spanishcharacter[random.Next(spanishcharacter.Length - 1)]);
			}
			installstation.Director=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 24; i++)
			{
				builder.Append(spanishcharacter[random.Next(spanishcharacter.Length - 1)]);
			}
			installstation.DirectorPhone=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 9; i++)
			{
				builder.Append(spanishcharacter[random.Next(spanishcharacter.Length - 1)]);
			}
			installstation.Contact=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 9; i++)
			{
				builder.Append(spanishcharacter[random.Next(spanishcharacter.Length - 1)]);
			}
			installstation.ContactPhone=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 49; i++)
			{
				builder.Append(spanishcharacter[random.Next(spanishcharacter.Length - 1)]);
			}
			installstation.Email=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 7; i++)
			{
				builder.Append(spanishcharacter[random.Next(spanishcharacter.Length - 1)]);
			}
			installstation.CityCode=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 999; i++)
			{
				builder.Append(spanishcharacter[random.Next(spanishcharacter.Length - 1)]);
			}
			installstation.Note=builder.ToString();
			builder.Length=0;

			SingleMessage<bool> ret = service.AddInstallStation(installstation);
			//Assert.IsFalse(ret.IsSuccess);
		}


		/// <summary>
		/// 测试CityCode英文超长
		/// </summary>
		[TestMethod]
		public void TestCityCodeStringForMaxLength_English()
		{
			InstallStation installstation = new InstallStation();
			InstallStationService service = new InstallStationService();
			StringBuilder builder = new StringBuilder();
			Random random = new Random();
			for (int i = 0; i < 17; i++)
			{
				builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
			}
			installstation.Id=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 255; i++)
			{
				builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
			}
			installstation.Name=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 255; i++)
			{
				builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
			}
			installstation.Address=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 24; i++)
			{
				builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
			}
			installstation.Director=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 24; i++)
			{
				builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
			}
			installstation.DirectorPhone=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 9; i++)
			{
				builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
			}
			installstation.Contact=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 9; i++)
			{
				builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
			}
			installstation.ContactPhone=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 49; i++)
			{
				builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
			}
			installstation.Email=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 7; i++)
			{
				builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
			}
			installstation.CityCode=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 999; i++)
			{
				builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
			}
			installstation.Note=builder.ToString();
			builder.Length=0;

			SingleMessage<bool> ret = service.AddInstallStation(installstation);
			//Assert.IsFalse(ret.IsSuccess);
		}


		/// <summary>
		/// 测试CityName中文超长
		/// </summary>
		[TestMethod]
		public void TestCityNameStringForOverLength_Chinese()
		{
			InstallStation installstation = new InstallStation();
			InstallStationService service = new InstallStationService();
			StringBuilder builder = new StringBuilder();
			Random random = new Random();
			for (int i = 0; i < 17; i++)
			{
				builder.Append(chinesecharacter[random.Next(chinesecharacter.Length - 1)]);
			}
			installstation.Id=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 255; i++)
			{
				builder.Append(chinesecharacter[random.Next(chinesecharacter.Length - 1)]);
			}
			installstation.Name=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 255; i++)
			{
				builder.Append(chinesecharacter[random.Next(chinesecharacter.Length - 1)]);
			}
			installstation.Address=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 24; i++)
			{
				builder.Append(chinesecharacter[random.Next(chinesecharacter.Length - 1)]);
			}
			installstation.Director=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 24; i++)
			{
				builder.Append(chinesecharacter[random.Next(chinesecharacter.Length - 1)]);
			}
			installstation.DirectorPhone=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 9; i++)
			{
				builder.Append(chinesecharacter[random.Next(chinesecharacter.Length - 1)]);
			}
			installstation.Contact=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 9; i++)
			{
				builder.Append(chinesecharacter[random.Next(chinesecharacter.Length - 1)]);
			}
			installstation.ContactPhone=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 49; i++)
			{
				builder.Append(chinesecharacter[random.Next(chinesecharacter.Length - 1)]);
			}
			installstation.Email=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 6; i++)
			{
				builder.Append(chinesecharacter[random.Next(chinesecharacter.Length - 1)]);
			}
			installstation.CityCode=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 999; i++)
			{
				builder.Append(chinesecharacter[random.Next(chinesecharacter.Length - 1)]);
			}
			installstation.Note=builder.ToString();
			builder.Length=0;

			SingleMessage<bool> ret = service.AddInstallStation(installstation);
			//Assert.IsFalse(ret.IsSuccess);
		}


		/// <summary>
		/// 测试CityName西文超长
		/// </summary>
		[TestMethod]
		public void TestCityNameStringForMaxLength_Spanish()
		{
			InstallStation installstation = new InstallStation();
			InstallStationService service = new InstallStationService();
			StringBuilder builder = new StringBuilder();
			Random random = new Random();
			for (int i = 0; i < 17; i++)
			{
				builder.Append(spanishcharacter[random.Next(spanishcharacter.Length - 1)]);
			}
			installstation.Id=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 255; i++)
			{
				builder.Append(spanishcharacter[random.Next(spanishcharacter.Length - 1)]);
			}
			installstation.Name=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 255; i++)
			{
				builder.Append(spanishcharacter[random.Next(spanishcharacter.Length - 1)]);
			}
			installstation.Address=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 24; i++)
			{
				builder.Append(spanishcharacter[random.Next(spanishcharacter.Length - 1)]);
			}
			installstation.Director=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 24; i++)
			{
				builder.Append(spanishcharacter[random.Next(spanishcharacter.Length - 1)]);
			}
			installstation.DirectorPhone=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 9; i++)
			{
				builder.Append(spanishcharacter[random.Next(spanishcharacter.Length - 1)]);
			}
			installstation.Contact=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 9; i++)
			{
				builder.Append(spanishcharacter[random.Next(spanishcharacter.Length - 1)]);
			}
			installstation.ContactPhone=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 49; i++)
			{
				builder.Append(spanishcharacter[random.Next(spanishcharacter.Length - 1)]);
			}
			installstation.Email=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 6; i++)
			{
				builder.Append(spanishcharacter[random.Next(spanishcharacter.Length - 1)]);
			}
			installstation.CityCode=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 999; i++)
			{
				builder.Append(spanishcharacter[random.Next(spanishcharacter.Length - 1)]);
			}
			installstation.Note=builder.ToString();
			builder.Length=0;

			SingleMessage<bool> ret = service.AddInstallStation(installstation);
			//Assert.IsFalse(ret.IsSuccess);
		}


		/// <summary>
		/// 测试CityName英文超长
		/// </summary>
		[TestMethod]
		public void TestCityNameStringForMaxLength_English()
		{
			InstallStation installstation = new InstallStation();
			InstallStationService service = new InstallStationService();
			StringBuilder builder = new StringBuilder();
			Random random = new Random();
			for (int i = 0; i < 17; i++)
			{
				builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
			}
			installstation.Id=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 255; i++)
			{
				builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
			}
			installstation.Name=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 255; i++)
			{
				builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
			}
			installstation.Address=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 24; i++)
			{
				builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
			}
			installstation.Director=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 24; i++)
			{
				builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
			}
			installstation.DirectorPhone=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 9; i++)
			{
				builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
			}
			installstation.Contact=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 9; i++)
			{
				builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
			}
			installstation.ContactPhone=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 49; i++)
			{
				builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
			}
			installstation.Email=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 6; i++)
			{
				builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
			}
			installstation.CityCode=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 999; i++)
			{
				builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
			}
			installstation.Note=builder.ToString();
			builder.Length=0;

			SingleMessage<bool> ret = service.AddInstallStation(installstation);
			//Assert.IsFalse(ret.IsSuccess);
		}


		/// <summary>
		/// 测试Note中文超长
		/// </summary>
		[TestMethod]
		public void TestNoteStringForOverLength_Chinese()
		{
			InstallStation installstation = new InstallStation();
			InstallStationService service = new InstallStationService();
			StringBuilder builder = new StringBuilder();
			Random random = new Random();
			for (int i = 0; i < 17; i++)
			{
				builder.Append(chinesecharacter[random.Next(chinesecharacter.Length - 1)]);
			}
			installstation.Id=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 255; i++)
			{
				builder.Append(chinesecharacter[random.Next(chinesecharacter.Length - 1)]);
			}
			installstation.Name=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 255; i++)
			{
				builder.Append(chinesecharacter[random.Next(chinesecharacter.Length - 1)]);
			}
			installstation.Address=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 24; i++)
			{
				builder.Append(chinesecharacter[random.Next(chinesecharacter.Length - 1)]);
			}
			installstation.Director=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 24; i++)
			{
				builder.Append(chinesecharacter[random.Next(chinesecharacter.Length - 1)]);
			}
			installstation.DirectorPhone=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 9; i++)
			{
				builder.Append(chinesecharacter[random.Next(chinesecharacter.Length - 1)]);
			}
			installstation.Contact=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 9; i++)
			{
				builder.Append(chinesecharacter[random.Next(chinesecharacter.Length - 1)]);
			}
			installstation.ContactPhone=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 49; i++)
			{
				builder.Append(chinesecharacter[random.Next(chinesecharacter.Length - 1)]);
			}
			installstation.Email=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 6; i++)
			{
				builder.Append(chinesecharacter[random.Next(chinesecharacter.Length - 1)]);
			}
			installstation.CityCode=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 1000; i++)
			{
				builder.Append(chinesecharacter[random.Next(chinesecharacter.Length - 1)]);
			}
			installstation.Note=builder.ToString();
			builder.Length=0;

			SingleMessage<bool> ret = service.AddInstallStation(installstation);
			//Assert.IsFalse(ret.IsSuccess);
		}


		/// <summary>
		/// 测试Note西文超长
		/// </summary>
		[TestMethod]
		public void TestNoteStringForMaxLength_Spanish()
		{
			InstallStation installstation = new InstallStation();
			InstallStationService service = new InstallStationService();
			StringBuilder builder = new StringBuilder();
			Random random = new Random();
			for (int i = 0; i < 17; i++)
			{
				builder.Append(spanishcharacter[random.Next(spanishcharacter.Length - 1)]);
			}
			installstation.Id=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 255; i++)
			{
				builder.Append(spanishcharacter[random.Next(spanishcharacter.Length - 1)]);
			}
			installstation.Name=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 255; i++)
			{
				builder.Append(spanishcharacter[random.Next(spanishcharacter.Length - 1)]);
			}
			installstation.Address=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 24; i++)
			{
				builder.Append(spanishcharacter[random.Next(spanishcharacter.Length - 1)]);
			}
			installstation.Director=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 24; i++)
			{
				builder.Append(spanishcharacter[random.Next(spanishcharacter.Length - 1)]);
			}
			installstation.DirectorPhone=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 9; i++)
			{
				builder.Append(spanishcharacter[random.Next(spanishcharacter.Length - 1)]);
			}
			installstation.Contact=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 9; i++)
			{
				builder.Append(spanishcharacter[random.Next(spanishcharacter.Length - 1)]);
			}
			installstation.ContactPhone=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 49; i++)
			{
				builder.Append(spanishcharacter[random.Next(spanishcharacter.Length - 1)]);
			}
			installstation.Email=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 6; i++)
			{
				builder.Append(spanishcharacter[random.Next(spanishcharacter.Length - 1)]);
			}
			installstation.CityCode=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 1000; i++)
			{
				builder.Append(spanishcharacter[random.Next(spanishcharacter.Length - 1)]);
			}
			installstation.Note=builder.ToString();
			builder.Length=0;

			SingleMessage<bool> ret = service.AddInstallStation(installstation);
			//Assert.IsFalse(ret.IsSuccess);
		}


		/// <summary>
		/// 测试Note英文超长
		/// </summary>
		[TestMethod]
		public void TestNoteStringForMaxLength_English()
		{
			InstallStation installstation = new InstallStation();
			InstallStationService service = new InstallStationService();
			StringBuilder builder = new StringBuilder();
			Random random = new Random();
			for (int i = 0; i < 17; i++)
			{
				builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
			}
			installstation.Id=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 255; i++)
			{
				builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
			}
			installstation.Name=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 255; i++)
			{
				builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
			}
			installstation.Address=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 24; i++)
			{
				builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
			}
			installstation.Director=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 24; i++)
			{
				builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
			}
			installstation.DirectorPhone=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 9; i++)
			{
				builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
			}
			installstation.Contact=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 9; i++)
			{
				builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
			}
			installstation.ContactPhone=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 49; i++)
			{
				builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
			}
			installstation.Email=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 6; i++)
			{
				builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
			}
			installstation.CityCode=builder.ToString();
			builder.Length=0;

			for (int i = 0; i < 1000; i++)
			{
				builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
			}
			installstation.Note=builder.ToString();
			builder.Length=0;

			SingleMessage<bool> ret = service.AddInstallStation(installstation);
			//Assert.IsFalse(ret.IsSuccess);
		}
        

		/// <summary>
		/// 测试空格
		/// </summary>
		[TestMethod]
		public void TestStringForSpace()
		{
			InstallStation installstation = new InstallStation();
			InstallStationService service = new InstallStationService();
            Random random=new Random();
			StringBuilder builder = new StringBuilder();
            builder.AppendLine("			Random random = new Random();");
builder.Append(" ");
			for (int i = 1; i < 17; i++)
			{
				builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
			}
builder.Append(" ");
			installstation.Id=builder.ToString();
			builder.Length=0;

builder.Append(" ");
			for (int i = 1; i < 255; i++)
			{
				builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
			}
builder.Append(" ");
			installstation.Name=builder.ToString();
			builder.Length=0;

builder.Append(" ");
			for (int i = 1; i < 255; i++)
			{
				builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
			}
builder.Append(" ");
			installstation.Address=builder.ToString();
			builder.Length=0;

builder.Append(" ");
			for (int i = 1; i < 24; i++)
			{
				builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
			}
builder.Append(" ");
			installstation.Director=builder.ToString();
			builder.Length=0;

builder.Append(" ");
			for (int i = 1; i < 24; i++)
			{
				builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
			}
builder.Append(" ");
			installstation.DirectorPhone=builder.ToString();
			builder.Length=0;

builder.Append(" ");
			for (int i = 1; i < 9; i++)
			{
				builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
			}
builder.Append(" ");
			installstation.Contact=builder.ToString();
			builder.Length=0;

builder.Append(" ");
			for (int i = 1; i < 9; i++)
			{
				builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
			}
builder.Append(" ");
			installstation.ContactPhone=builder.ToString();
			builder.Length=0;

builder.Append(" ");
			for (int i = 1; i < 49; i++)
			{
				builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
			}
builder.Append(" ");
			installstation.Email=builder.ToString();
			builder.Length=0;

builder.Append(" ");
			for (int i = 1; i < 6; i++)
			{
				builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
			}
builder.Append(" ");
			installstation.CityCode=builder.ToString();
			builder.Length=0;

builder.Append(" ");
			for (int i = 1; i < 999; i++)
			{
				builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
			}
builder.Append(" ");
			installstation.Note=builder.ToString();
			builder.Length=0;

			SingleMessage<bool> ret = service.AddInstallStation(installstation);
			//Assert.IsFalse(ret.IsSuccess);
		}


		/// <summary>
		/// 测试,
		/// </summary>
		[TestMethod]
		public void TestStringForSpecialCharacter_Comma()
		{
			InstallStation installstation = new InstallStation();
			InstallStationService service = new InstallStationService();
            Random random=new Random();
			StringBuilder builder = new StringBuilder();
            builder.AppendLine("			Random random = new Random();");
builder.Append(",");
			for (int i = 1; i < 17; i++)
			{
				builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
			}
builder.Append(",");
			installstation.Id=builder.ToString();
			builder.Length=0;

builder.Append(",");
			for (int i = 1; i < 255; i++)
			{
				builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
			}
builder.Append(",");
			installstation.Name=builder.ToString();
			builder.Length=0;

builder.Append(",");
			for (int i = 1; i < 255; i++)
			{
				builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
			}
builder.Append(",");
			installstation.Address=builder.ToString();
			builder.Length=0;

builder.Append(",");
			for (int i = 1; i < 24; i++)
			{
				builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
			}
builder.Append(",");
			installstation.Director=builder.ToString();
			builder.Length=0;

builder.Append(",");
			for (int i = 1; i < 24; i++)
			{
				builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
			}
builder.Append(",");
			installstation.DirectorPhone=builder.ToString();
			builder.Length=0;

builder.Append(",");
			for (int i = 1; i < 9; i++)
			{
				builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
			}
builder.Append(",");
			installstation.Contact=builder.ToString();
			builder.Length=0;

builder.Append(",");
			for (int i = 1; i < 9; i++)
			{
				builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
			}
builder.Append(",");
			installstation.ContactPhone=builder.ToString();
			builder.Length=0;

builder.Append(",");
			for (int i = 1; i < 49; i++)
			{
				builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
			}
builder.Append(",");
			installstation.Email=builder.ToString();
			builder.Length=0;

builder.Append(",");
			for (int i = 1; i < 6; i++)
			{
				builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
			}
builder.Append(",");
			installstation.CityCode=builder.ToString();
			builder.Length=0;

builder.Append(",");
			for (int i = 1; i < 999; i++)
			{
				builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
			}
builder.Append(",");
			installstation.Note=builder.ToString();
			builder.Length=0;

			SingleMessage<bool> ret = service.AddInstallStation(installstation);
			//Assert.IsFalse(ret.IsSuccess);
		}


		/// <summary>
		/// 测试;
		/// </summary>
		[TestMethod]
		public void TestStringForSpecialCharacter_SemiComma()
		{
			InstallStation installstation = new InstallStation();
			InstallStationService service = new InstallStationService();
			StringBuilder builder = new StringBuilder();
			Random random = new Random();
builder.Append(";");
			for (int i = 1; i < 17; i++)
			{
				builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
			}
builder.Append(";");
			installstation.Id=builder.ToString();
			builder.Length=0;

builder.Append(";");
			for (int i = 1; i < 255; i++)
			{
				builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
			}
builder.Append(";");
			installstation.Name=builder.ToString();
			builder.Length=0;

builder.Append(";");
			for (int i = 1; i < 255; i++)
			{
				builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
			}
builder.Append(";");
			installstation.Address=builder.ToString();
			builder.Length=0;

builder.Append(";");
			for (int i = 1; i < 24; i++)
			{
				builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
			}
builder.Append(";");
			installstation.Director=builder.ToString();
			builder.Length=0;

builder.Append(";");
			for (int i = 1; i < 24; i++)
			{
				builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
			}
builder.Append(";");
			installstation.DirectorPhone=builder.ToString();
			builder.Length=0;

builder.Append(";");
			for (int i = 1; i < 9; i++)
			{
				builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
			}
builder.Append(";");
			installstation.Contact=builder.ToString();
			builder.Length=0;

builder.Append(";");
			for (int i = 1; i < 9; i++)
			{
				builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
			}
builder.Append(";");
			installstation.ContactPhone=builder.ToString();
			builder.Length=0;

builder.Append(";");
			for (int i = 1; i < 49; i++)
			{
				builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
			}
builder.Append(";");
			installstation.Email=builder.ToString();
			builder.Length=0;

builder.Append(";");
			for (int i = 1; i < 6; i++)
			{
				builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
			}
builder.Append(";");
			installstation.CityCode=builder.ToString();
			builder.Length=0;

builder.Append(";");
			for (int i = 1; i < 999; i++)
			{
				builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
			}
builder.Append(";");
			installstation.Note=builder.ToString();
			builder.Length=0;

			SingleMessage<bool> ret = service.AddInstallStation(installstation);
			//Assert.IsFalse(ret.IsSuccess);
		}


		/// <summary>
		/// 测试"
		/// </summary>
		[TestMethod]
		public void TestStringForSpecialCharacter()
		{
			InstallStation installstation = new InstallStation();
			InstallStationService service = new InstallStationService();
			StringBuilder builder = new StringBuilder();
			Random random = new Random();
builder.Append("\"");
			for (int i = 1; i < 17; i++)
			{
				builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
			}
builder.Append("\"");
			installstation.Id=builder.ToString();
			builder.Length=0;

builder.Append("\"");
			for (int i = 1; i < 255; i++)
			{
				builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
			}
builder.Append("\"");
			installstation.Name=builder.ToString();
			builder.Length=0;

builder.Append("\"");
			for (int i = 1; i < 255; i++)
			{
				builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
			}
builder.Append("\"");
			installstation.Address=builder.ToString();
			builder.Length=0;

builder.Append("\"");
			for (int i = 1; i < 24; i++)
			{
				builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
			}
builder.Append("\"");
			installstation.Director=builder.ToString();
			builder.Length=0;

builder.Append("\"");
			for (int i = 1; i < 24; i++)
			{
				builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
			}
builder.Append("\"");
			installstation.DirectorPhone=builder.ToString();
			builder.Length=0;

builder.Append("\"");
			for (int i = 1; i < 9; i++)
			{
				builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
			}
builder.Append("\"");
			installstation.Contact=builder.ToString();
			builder.Length=0;

builder.Append("\"");
			for (int i = 1; i < 9; i++)
			{
				builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
			}
builder.Append("\"");
			installstation.ContactPhone=builder.ToString();
			builder.Length=0;

builder.Append("\"");
			for (int i = 1; i < 49; i++)
			{
				builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
			}
builder.Append("\"");
			installstation.Email=builder.ToString();
			builder.Length=0;

builder.Append("\"");
			for (int i = 1; i < 6; i++)
			{
				builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
			}
builder.Append("\"");
			installstation.CityCode=builder.ToString();
			builder.Length=0;

builder.Append("\"");
			for (int i = 1; i < 999; i++)
			{
				builder.Append(englishcharacter[random.Next(englishcharacter.Length - 1)]);
			}
builder.Append("\"");
			installstation.Note=builder.ToString();
			builder.Length=0;

			SingleMessage<bool> ret = service.AddInstallStation(installstation);
			//Assert.IsFalse(ret.IsSuccess);
		}


		/// <summary>
		/// 测试select
		/// </summary>
		[TestMethod]
		public void TestStringForSpecialCharacterSelect()
		{
			InstallStation installstation = new InstallStation();
			InstallStationService service = new InstallStationService();
			installstation.Id="select";

			installstation.Name="select";

			installstation.Address="select";

			installstation.Director="select";

			installstation.DirectorPhone="select";

			installstation.Contact="select";

			installstation.ContactPhone="select";

			installstation.Email="select";

			installstation.CityCode="select";

			installstation.Note="select";

			SingleMessage<bool> ret = service.AddInstallStation(installstation);
			//Assert.IsFalse(ret.IsSuccess);
		}


		/// <summary>
		/// 测试update
		/// </summary>
		[TestMethod]
		public void TestStringForSpecialCharacterUpdate()
		{
			InstallStation installstation = new InstallStation();
			InstallStationService service = new InstallStationService();
			installstation.Id="update";

			installstation.Name="update";

			installstation.Address="update";

			installstation.Director="update";

			installstation.DirectorPhone="update";

			installstation.Contact="update";

			installstation.ContactPhone="update";

			installstation.Email="update";

			installstation.CityCode="update";

			installstation.Note="update";

			SingleMessage<bool> ret = service.AddInstallStation(installstation);
			//Assert.IsFalse(ret.IsSuccess);
		}


		/// <summary>
		/// 测试delete
		/// </summary>
		[TestMethod]
		public void TestStringForSpecialCharacter_quote()
		{
			InstallStation installstation = new InstallStation();
			InstallStationService service = new InstallStationService();
			installstation.Id="delete";

			installstation.Name="delete";

			installstation.Address="delete";

			installstation.Director="delete";

			installstation.DirectorPhone="delete";

			installstation.Contact="delete";

			installstation.ContactPhone="delete";

			installstation.Email="delete";

			installstation.CityCode="delete";

			installstation.Note="delete";

			SingleMessage<bool> ret = service.AddInstallStation(installstation);
			//Assert.IsFalse(ret.IsSuccess);
		}


		/// <summary>
		/// 测试最大值
		/// </summary>
		[TestMethod]
		public void TestNumberForMax()
		{
			InstallStation installstation = new InstallStation();
			InstallStationService service = new InstallStationService();
			installstation.Id="";
			installstation.Name="";
			installstation.Address="";
			installstation.Director="";
			installstation.DirectorPhone="";
			installstation.Contact="";
			installstation.ContactPhone="";
			installstation.Email="";
			installstation.CityCode="";
			installstation.Note="";
			SingleMessage<bool> ret = service.AddInstallStation(installstation);
			//Assert.IsFalse(ret.IsSuccess);
		}


		/// <summary>
		/// 测试最小值
		/// </summary>
		[TestMethod]
		public void TestNumberForMin()
		{
			InstallStation installstation = new InstallStation();
			InstallStationService service = new InstallStationService();
			installstation.Id="";
			installstation.Name="";
			installstation.Address="";
			installstation.Director="";
			installstation.DirectorPhone="";
			installstation.Contact="";
			installstation.ContactPhone="";
			installstation.Email="";
			installstation.CityCode="";
			installstation.Note="";
			SingleMessage<bool> ret = service.AddInstallStation(installstation);
			//Assert.IsFalse(ret.IsSuccess);
		}


		/// <summary>
		/// 正数
		/// </summary>
		[TestMethod]
		public void TestNumberForPositive()
		{
			InstallStation installstation = new InstallStation();
			InstallStationService service = new InstallStationService();
			installstation.Id="";
			installstation.Name="";
			installstation.Address="";
			installstation.Director="";
			installstation.DirectorPhone="";
			installstation.Contact="";
			installstation.ContactPhone="";
			installstation.Email="";
			installstation.CityCode="";
			installstation.Note="";
			SingleMessage<bool> ret = service.AddInstallStation(installstation);
			//Assert.IsFalse(ret.IsSuccess);
		}


		/// <summary>
		/// 负数
		/// </summary>
		[TestMethod]
		public void TestNumberForNegative()
		{
			InstallStation installstation = new InstallStation();
			InstallStationService service = new InstallStationService();
			installstation.Id="";
			installstation.Name="";
			installstation.Address="";
			installstation.Director="";
			installstation.DirectorPhone="";
			installstation.Contact="";
			installstation.ContactPhone="";
			installstation.Email="";
			installstation.CityCode="";
			installstation.Note="";
			SingleMessage<bool> ret = service.AddInstallStation(installstation);
			//Assert.IsFalse(ret.IsSuccess);
		}


		/// <summary>
		/// 零
		/// </summary>
		[TestMethod]
		public void TestNumberForZero()
		{
			InstallStation installstation = new InstallStation();
			InstallStationService service = new InstallStationService();
			installstation.Id="";
			installstation.Name="";
			installstation.Address="";
			installstation.Director="";
			installstation.DirectorPhone="";
			installstation.Contact="";
			installstation.ContactPhone="";
			installstation.Email="";
			installstation.CityCode="";
			installstation.Note="";
			SingleMessage<bool> ret = service.AddInstallStation(installstation);
			//Assert.IsFalse(ret.IsSuccess);
		}


		/// <summary>
		/// 小数
		/// </summary>
		[TestMethod]
		public void TestNumberForPortionA()
		{
			InstallStation installstation = new InstallStation();
			InstallStationService service = new InstallStationService();
			installstation.Id="";
			installstation.Name="";
			installstation.Address="";
			installstation.Director="";
			installstation.DirectorPhone="";
			installstation.Contact="";
			installstation.ContactPhone="";
			installstation.Email="";
			installstation.CityCode="";
			installstation.Note="";
			SingleMessage<bool> ret = service.AddInstallStation(installstation);
			//Assert.IsFalse(ret.IsSuccess);
		}


		/// <summary>
		/// 正数小数
		/// </summary>
		[TestMethod]
		public void TestNumberForPortionB()
		{
			InstallStation installstation = new InstallStation();
			InstallStationService service = new InstallStationService();
			installstation.Id="";
			installstation.Name="";
			installstation.Address="";
			installstation.Director="";
			installstation.DirectorPhone="";
			installstation.Contact="";
			installstation.ContactPhone="";
			installstation.Email="";
			installstation.CityCode="";
			installstation.Note="";
			SingleMessage<bool> ret = service.AddInstallStation(installstation);
			//Assert.IsFalse(ret.IsSuccess);
		}


		/// <summary>
		/// 字母
		/// </summary>
		[TestMethod]
		public void TestNumberForPortion()
		{
			InstallStation installstation = new InstallStation();
			InstallStationService service = new InstallStationService();
			installstation.Id="";
			installstation.Name="";
			installstation.Address="";
			installstation.Director="";
			installstation.DirectorPhone="";
			installstation.Contact="";
			installstation.ContactPhone="";
			installstation.Email="";
			installstation.CityCode="";
			installstation.Note="";
			SingleMessage<bool> ret = service.AddInstallStation(installstation);
			//Assert.IsFalse(ret.IsSuccess);
		}


		/// <summary>
		/// DateTime
		/// </summary>
		[TestMethod]
		public void TestDateTime()
		{
			InstallStation installstation = new InstallStation();
			InstallStationService service = new InstallStationService();
			installstation.Id="";
			installstation.Name="";
			installstation.Address="";
			installstation.Director="";
			installstation.DirectorPhone="";
			installstation.Contact="";
			installstation.ContactPhone="";
			installstation.Email="";
			installstation.CityCode="";
			installstation.Note="";
			SingleMessage<bool> ret = service.AddInstallStation(installstation);
			//Assert.IsFalse(ret.IsSuccess);
		}


		/// <summary>
		/// 日期为空
		/// </summary>
		[TestMethod]
		public void TestIdDateTimeForNull()
		{
			InstallStation installstation = new InstallStation();
			InstallStationService service = new InstallStationService();
			installstation.Id="";
			installstation.Name="";
			installstation.Address="";
			installstation.Director="";
			installstation.DirectorPhone="";
			installstation.Contact="";
			installstation.ContactPhone="";
			installstation.Email="";
			installstation.CityCode="";
			installstation.Note="";
			SingleMessage<bool> ret = service.AddInstallStation(installstation);
			//Assert.IsFalse(ret.IsSuccess);
		}


		/// <summary>
		/// 日期为空
		/// </summary>
		[TestMethod]
		public void TestNameDateTimeForNull()
		{
			InstallStation installstation = new InstallStation();
			InstallStationService service = new InstallStationService();
			installstation.Id="";
			installstation.Name="";
			installstation.Address="";
			installstation.Director="";
			installstation.DirectorPhone="";
			installstation.Contact="";
			installstation.ContactPhone="";
			installstation.Email="";
			installstation.CityCode="";
			installstation.Note="";
			SingleMessage<bool> ret = service.AddInstallStation(installstation);
			//Assert.IsFalse(ret.IsSuccess);
		}


		/// <summary>
		/// 日期为空
		/// </summary>
		[TestMethod]
		public void TestAddressDateTimeForNull()
		{
			InstallStation installstation = new InstallStation();
			InstallStationService service = new InstallStationService();
			installstation.Id="";
			installstation.Name="";
			installstation.Address="";
			installstation.Director="";
			installstation.DirectorPhone="";
			installstation.Contact="";
			installstation.ContactPhone="";
			installstation.Email="";
			installstation.CityCode="";
			installstation.Note="";
			SingleMessage<bool> ret = service.AddInstallStation(installstation);
			//Assert.IsFalse(ret.IsSuccess);
		}


		/// <summary>
		/// 日期为空
		/// </summary>
		[TestMethod]
		public void TestDirectorDateTimeForNull()
		{
			InstallStation installstation = new InstallStation();
			InstallStationService service = new InstallStationService();
			installstation.Id="";
			installstation.Name="";
			installstation.Address="";
			installstation.Director="";
			installstation.DirectorPhone="";
			installstation.Contact="";
			installstation.ContactPhone="";
			installstation.Email="";
			installstation.CityCode="";
			installstation.Note="";
			SingleMessage<bool> ret = service.AddInstallStation(installstation);
			//Assert.IsFalse(ret.IsSuccess);
		}


		/// <summary>
		/// 日期为空
		/// </summary>
		[TestMethod]
		public void TestDirectorPhoneDateTimeForNull()
		{
			InstallStation installstation = new InstallStation();
			InstallStationService service = new InstallStationService();
			installstation.Id="";
			installstation.Name="";
			installstation.Address="";
			installstation.Director="";
			installstation.DirectorPhone="";
			installstation.Contact="";
			installstation.ContactPhone="";
			installstation.Email="";
			installstation.CityCode="";
			installstation.Note="";
			SingleMessage<bool> ret = service.AddInstallStation(installstation);
			//Assert.IsFalse(ret.IsSuccess);
		}


		/// <summary>
		/// 日期为空
		/// </summary>
		[TestMethod]
		public void TestContactDateTimeForNull()
		{
			InstallStation installstation = new InstallStation();
			InstallStationService service = new InstallStationService();
			installstation.Id="";
			installstation.Name="";
			installstation.Address="";
			installstation.Director="";
			installstation.DirectorPhone="";
			installstation.Contact="";
			installstation.ContactPhone="";
			installstation.Email="";
			installstation.CityCode="";
			installstation.Note="";
			SingleMessage<bool> ret = service.AddInstallStation(installstation);
			//Assert.IsFalse(ret.IsSuccess);
		}


		/// <summary>
		/// 日期为空
		/// </summary>
		[TestMethod]
		public void TestContactPhoneDateTimeForNull()
		{
			InstallStation installstation = new InstallStation();
			InstallStationService service = new InstallStationService();
			installstation.Id="";
			installstation.Name="";
			installstation.Address="";
			installstation.Director="";
			installstation.DirectorPhone="";
			installstation.Contact="";
			installstation.ContactPhone="";
			installstation.Email="";
			installstation.CityCode="";
			installstation.Note="";
			SingleMessage<bool> ret = service.AddInstallStation(installstation);
			//Assert.IsFalse(ret.IsSuccess);
		}


		/// <summary>
		/// 日期为空
		/// </summary>
		[TestMethod]
		public void TestEmailDateTimeForNull()
		{
			InstallStation installstation = new InstallStation();
			InstallStationService service = new InstallStationService();
			installstation.Id="";
			installstation.Name="";
			installstation.Address="";
			installstation.Director="";
			installstation.DirectorPhone="";
			installstation.Contact="";
			installstation.ContactPhone="";
			installstation.Email="";
			installstation.CityCode="";
			installstation.Note="";
			SingleMessage<bool> ret = service.AddInstallStation(installstation);
			//Assert.IsFalse(ret.IsSuccess);
		}


		/// <summary>
		/// 日期为空
		/// </summary>
		[TestMethod]
		public void TestProvinceCodeDateTimeForNull()
		{
			InstallStation installstation = new InstallStation();
			InstallStationService service = new InstallStationService();
			installstation.Id="";
			installstation.Name="";
			installstation.Address="";
			installstation.Director="";
			installstation.DirectorPhone="";
			installstation.Contact="";
			installstation.ContactPhone="";
			installstation.Email="";
			installstation.CityCode="";
			installstation.Note="";
			SingleMessage<bool> ret = service.AddInstallStation(installstation);
			//Assert.IsFalse(ret.IsSuccess);
		}


		/// <summary>
		/// 日期为空
		/// </summary>
		[TestMethod]
		public void TestProvinceNameDateTimeForNull()
		{
			InstallStation installstation = new InstallStation();
			InstallStationService service = new InstallStationService();
			installstation.Id="";
			installstation.Name="";
			installstation.Address="";
			installstation.Director="";
			installstation.DirectorPhone="";
			installstation.Contact="";
			installstation.ContactPhone="";
			installstation.Email="";
			installstation.CityCode="";
			installstation.Note="";
			SingleMessage<bool> ret = service.AddInstallStation(installstation);
			//Assert.IsFalse(ret.IsSuccess);
		}


		/// <summary>
		/// 日期为空
		/// </summary>
		[TestMethod]
		public void TestCityCodeDateTimeForNull()
		{
			InstallStation installstation = new InstallStation();
			InstallStationService service = new InstallStationService();
			installstation.Id="";
			installstation.Name="";
			installstation.Address="";
			installstation.Director="";
			installstation.DirectorPhone="";
			installstation.Contact="";
			installstation.ContactPhone="";
			installstation.Email="";
			installstation.CityCode="";
			installstation.Note="";
			SingleMessage<bool> ret = service.AddInstallStation(installstation);
			//Assert.IsFalse(ret.IsSuccess);
		}


		/// <summary>
		/// 日期为空
		/// </summary>
		[TestMethod]
		public void TestCityNameDateTimeForNull()
		{
			InstallStation installstation = new InstallStation();
			InstallStationService service = new InstallStationService();
			installstation.Id="";
			installstation.Name="";
			installstation.Address="";
			installstation.Director="";
			installstation.DirectorPhone="";
			installstation.Contact="";
			installstation.ContactPhone="";
			installstation.Email="";
			installstation.CityCode="";
			installstation.Note="";
			SingleMessage<bool> ret = service.AddInstallStation(installstation);
			//Assert.IsFalse(ret.IsSuccess);
		}


		/// <summary>
		/// 日期为空
		/// </summary>
		[TestMethod]
		public void TestNoteDateTimeForNull()
		{
			InstallStation installstation = new InstallStation();
			InstallStationService service = new InstallStationService();
			installstation.Id="";
			installstation.Name="";
			installstation.Address="";
			installstation.Director="";
			installstation.DirectorPhone="";
			installstation.Contact="";
			installstation.ContactPhone="";
			installstation.Email="";
			installstation.CityCode="";
			installstation.Note="";
			SingleMessage<bool> ret = service.AddInstallStation(installstation);
			//Assert.IsFalse(ret.IsSuccess);
		}


		/// <summary>
		/// Id日期为空
		/// </summary>
		[TestMethod]
		public void TestIdDateTimeForIncomplete()
		{
			InstallStation installstation = new InstallStation();
			InstallStationService service = new InstallStationService();
			installstation.Id="";
			installstation.Name="";
			installstation.Address="";
			installstation.Director="";
			installstation.DirectorPhone="";
			installstation.Contact="";
			installstation.ContactPhone="";
			installstation.Email="";
			installstation.CityCode="";
			installstation.Note="";
			SingleMessage<bool> ret = service.AddInstallStation(installstation);
			//Assert.IsFalse(ret.IsSuccess);
		}


		/// <summary>
		/// Name日期为空
		/// </summary>
		[TestMethod]
		public void TestNameDateTimeForIncomplete()
		{
			InstallStation installstation = new InstallStation();
			InstallStationService service = new InstallStationService();
			installstation.Id="";
			installstation.Name="";
			installstation.Address="";
			installstation.Director="";
			installstation.DirectorPhone="";
			installstation.Contact="";
			installstation.ContactPhone="";
			installstation.Email="";
			installstation.CityCode="";
			installstation.Note="";
			SingleMessage<bool> ret = service.AddInstallStation(installstation);
			//Assert.IsFalse(ret.IsSuccess);
		}


		/// <summary>
		/// Address日期为空
		/// </summary>
		[TestMethod]
		public void TestAddressDateTimeForIncomplete()
		{
			InstallStation installstation = new InstallStation();
			InstallStationService service = new InstallStationService();
			installstation.Id="";
			installstation.Name="";
			installstation.Address="";
			installstation.Director="";
			installstation.DirectorPhone="";
			installstation.Contact="";
			installstation.ContactPhone="";
			installstation.Email="";
			installstation.CityCode="";
			installstation.Note="";
			SingleMessage<bool> ret = service.AddInstallStation(installstation);
			//Assert.IsFalse(ret.IsSuccess);
		}


		/// <summary>
		/// Director日期为空
		/// </summary>
		[TestMethod]
		public void TestDirectorDateTimeForIncomplete()
		{
			InstallStation installstation = new InstallStation();
			InstallStationService service = new InstallStationService();
			installstation.Id="";
			installstation.Name="";
			installstation.Address="";
			installstation.Director="";
			installstation.DirectorPhone="";
			installstation.Contact="";
			installstation.ContactPhone="";
			installstation.Email="";
			installstation.CityCode="";
			installstation.Note="";
			SingleMessage<bool> ret = service.AddInstallStation(installstation);
			//Assert.IsFalse(ret.IsSuccess);
		}


		/// <summary>
		/// DirectorPhone日期为空
		/// </summary>
		[TestMethod]
		public void TestDirectorPhoneDateTimeForIncomplete()
		{
			InstallStation installstation = new InstallStation();
			InstallStationService service = new InstallStationService();
			installstation.Id="";
			installstation.Name="";
			installstation.Address="";
			installstation.Director="";
			installstation.DirectorPhone="";
			installstation.Contact="";
			installstation.ContactPhone="";
			installstation.Email="";
			installstation.CityCode="";
			installstation.Note="";
			SingleMessage<bool> ret = service.AddInstallStation(installstation);
			//Assert.IsFalse(ret.IsSuccess);
		}


		/// <summary>
		/// Contact日期为空
		/// </summary>
		[TestMethod]
		public void TestContactDateTimeForIncomplete()
		{
			InstallStation installstation = new InstallStation();
			InstallStationService service = new InstallStationService();
			installstation.Id="";
			installstation.Name="";
			installstation.Address="";
			installstation.Director="";
			installstation.DirectorPhone="";
			installstation.Contact="";
			installstation.ContactPhone="";
			installstation.Email="";
			installstation.CityCode="";
			installstation.Note="";
			SingleMessage<bool> ret = service.AddInstallStation(installstation);
			//Assert.IsFalse(ret.IsSuccess);
		}


		/// <summary>
		/// ContactPhone日期为空
		/// </summary>
		[TestMethod]
		public void TestContactPhoneDateTimeForIncomplete()
		{
			InstallStation installstation = new InstallStation();
			InstallStationService service = new InstallStationService();
			installstation.Id="";
			installstation.Name="";
			installstation.Address="";
			installstation.Director="";
			installstation.DirectorPhone="";
			installstation.Contact="";
			installstation.ContactPhone="";
			installstation.Email="";
			installstation.CityCode="";
			installstation.Note="";
			SingleMessage<bool> ret = service.AddInstallStation(installstation);
			//Assert.IsFalse(ret.IsSuccess);
		}


		/// <summary>
		/// Email日期为空
		/// </summary>
		[TestMethod]
		public void TestEmailDateTimeForIncomplete()
		{
			InstallStation installstation = new InstallStation();
			InstallStationService service = new InstallStationService();
			installstation.Id="";
			installstation.Name="";
			installstation.Address="";
			installstation.Director="";
			installstation.DirectorPhone="";
			installstation.Contact="";
			installstation.ContactPhone="";
			installstation.Email="";
			installstation.CityCode="";
			installstation.Note="";
			SingleMessage<bool> ret = service.AddInstallStation(installstation);
			//Assert.IsFalse(ret.IsSuccess);
		}


		/// <summary>
		/// ProvinceCode日期为空
		/// </summary>
		[TestMethod]
		public void TestProvinceCodeDateTimeForIncomplete()
		{
			InstallStation installstation = new InstallStation();
			InstallStationService service = new InstallStationService();
			installstation.Id="";
			installstation.Name="";
			installstation.Address="";
			installstation.Director="";
			installstation.DirectorPhone="";
			installstation.Contact="";
			installstation.ContactPhone="";
			installstation.Email="";
			installstation.CityCode="";
			installstation.Note="";
			SingleMessage<bool> ret = service.AddInstallStation(installstation);
			//Assert.IsFalse(ret.IsSuccess);
		}


		/// <summary>
		/// ProvinceName日期为空
		/// </summary>
		[TestMethod]
		public void TestProvinceNameDateTimeForIncomplete()
		{
			InstallStation installstation = new InstallStation();
			InstallStationService service = new InstallStationService();
			installstation.Id="";
			installstation.Name="";
			installstation.Address="";
			installstation.Director="";
			installstation.DirectorPhone="";
			installstation.Contact="";
			installstation.ContactPhone="";
			installstation.Email="";
			installstation.CityCode="";
			installstation.Note="";
			SingleMessage<bool> ret = service.AddInstallStation(installstation);
			//Assert.IsFalse(ret.IsSuccess);
		}


		/// <summary>
		/// CityCode日期为空
		/// </summary>
		[TestMethod]
		public void TestCityCodeDateTimeForIncomplete()
		{
			InstallStation installstation = new InstallStation();
			InstallStationService service = new InstallStationService();
			installstation.Id="";
			installstation.Name="";
			installstation.Address="";
			installstation.Director="";
			installstation.DirectorPhone="";
			installstation.Contact="";
			installstation.ContactPhone="";
			installstation.Email="";
			installstation.CityCode="";
			installstation.Note="";
			SingleMessage<bool> ret = service.AddInstallStation(installstation);
			//Assert.IsFalse(ret.IsSuccess);
		}


		/// <summary>
		/// CityName日期为空
		/// </summary>
		[TestMethod]
		public void TestCityNameDateTimeForIncomplete()
		{
			InstallStation installstation = new InstallStation();
			InstallStationService service = new InstallStationService();
			installstation.Id="";
			installstation.Name="";
			installstation.Address="";
			installstation.Director="";
			installstation.DirectorPhone="";
			installstation.Contact="";
			installstation.ContactPhone="";
			installstation.Email="";
			installstation.CityCode="";
			installstation.Note="";
			SingleMessage<bool> ret = service.AddInstallStation(installstation);
			//Assert.IsFalse(ret.IsSuccess);
		}


		/// <summary>
		/// Note日期为空
		/// </summary>
		[TestMethod]
		public void TestNoteDateTimeForIncomplete()
		{
			InstallStation installstation = new InstallStation();
			InstallStationService service = new InstallStationService();
			installstation.Id="";
			installstation.Name="";
			installstation.Address="";
			installstation.Director="";
			installstation.DirectorPhone="";
			installstation.Contact="";
			installstation.ContactPhone="";
			installstation.Email="";
			installstation.CityCode="";
			installstation.Note="";
			SingleMessage<bool> ret = service.AddInstallStation(installstation);
			//Assert.IsFalse(ret.IsSuccess);
		}


		/// <summary>
		/// 测试Id日期格式错误
		/// </summary>
		[TestMethod]
		public void TestIdDateTimeForFormatError()
		{
			InstallStation installstation = new InstallStation();
			InstallStationService service = new InstallStationService();
			installstation.Id="";
			installstation.Name="";
			installstation.Address="";
			installstation.Director="";
			installstation.DirectorPhone="";
			installstation.Contact="";
			installstation.ContactPhone="";
			installstation.Email="";
			installstation.CityCode="";
			installstation.Note="";
			SingleMessage<bool> ret = service.AddInstallStation(installstation);
			//Assert.IsFalse(ret.IsSuccess);
		}


		/// <summary>
		/// 测试Name日期格式错误
		/// </summary>
		[TestMethod]
		public void TestNameDateTimeForFormatError()
		{
			InstallStation installstation = new InstallStation();
			InstallStationService service = new InstallStationService();
			installstation.Id="";
			installstation.Name="";
			installstation.Address="";
			installstation.Director="";
			installstation.DirectorPhone="";
			installstation.Contact="";
			installstation.ContactPhone="";
			installstation.Email="";
			installstation.CityCode="";
			installstation.Note="";
			SingleMessage<bool> ret = service.AddInstallStation(installstation);
			//Assert.IsFalse(ret.IsSuccess);
		}


		/// <summary>
		/// 测试Address日期格式错误
		/// </summary>
		[TestMethod]
		public void TestAddressDateTimeForFormatError()
		{
			InstallStation installstation = new InstallStation();
			InstallStationService service = new InstallStationService();
			installstation.Id="";
			installstation.Name="";
			installstation.Address="";
			installstation.Director="";
			installstation.DirectorPhone="";
			installstation.Contact="";
			installstation.ContactPhone="";
			installstation.Email="";
			installstation.CityCode="";
			installstation.Note="";
			SingleMessage<bool> ret = service.AddInstallStation(installstation);
			//Assert.IsFalse(ret.IsSuccess);
		}


		/// <summary>
		/// 测试Director日期格式错误
		/// </summary>
		[TestMethod]
		public void TestDirectorDateTimeForFormatError()
		{
			InstallStation installstation = new InstallStation();
			InstallStationService service = new InstallStationService();
			installstation.Id="";
			installstation.Name="";
			installstation.Address="";
			installstation.Director="";
			installstation.DirectorPhone="";
			installstation.Contact="";
			installstation.ContactPhone="";
			installstation.Email="";
			installstation.CityCode="";
			installstation.Note="";
			SingleMessage<bool> ret = service.AddInstallStation(installstation);
			//Assert.IsFalse(ret.IsSuccess);
		}


		/// <summary>
		/// 测试DirectorPhone日期格式错误
		/// </summary>
		[TestMethod]
		public void TestDirectorPhoneDateTimeForFormatError()
		{
			InstallStation installstation = new InstallStation();
			InstallStationService service = new InstallStationService();
			installstation.Id="";
			installstation.Name="";
			installstation.Address="";
			installstation.Director="";
			installstation.DirectorPhone="";
			installstation.Contact="";
			installstation.ContactPhone="";
			installstation.Email="";
			installstation.CityCode="";
			installstation.Note="";
			SingleMessage<bool> ret = service.AddInstallStation(installstation);
			//Assert.IsFalse(ret.IsSuccess);
		}


		/// <summary>
		/// 测试Contact日期格式错误
		/// </summary>
		[TestMethod]
		public void TestContactDateTimeForFormatError()
		{
			InstallStation installstation = new InstallStation();
			InstallStationService service = new InstallStationService();
			installstation.Id="";
			installstation.Name="";
			installstation.Address="";
			installstation.Director="";
			installstation.DirectorPhone="";
			installstation.Contact="";
			installstation.ContactPhone="";
			installstation.Email="";
			installstation.CityCode="";
			installstation.Note="";
			SingleMessage<bool> ret = service.AddInstallStation(installstation);
			//Assert.IsFalse(ret.IsSuccess);
		}


		/// <summary>
		/// 测试ContactPhone日期格式错误
		/// </summary>
		[TestMethod]
		public void TestContactPhoneDateTimeForFormatError()
		{
			InstallStation installstation = new InstallStation();
			InstallStationService service = new InstallStationService();
			installstation.Id="";
			installstation.Name="";
			installstation.Address="";
			installstation.Director="";
			installstation.DirectorPhone="";
			installstation.Contact="";
			installstation.ContactPhone="";
			installstation.Email="";
			installstation.CityCode="";
			installstation.Note="";
			SingleMessage<bool> ret = service.AddInstallStation(installstation);
			//Assert.IsFalse(ret.IsSuccess);
		}


		/// <summary>
		/// 测试Email日期格式错误
		/// </summary>
		[TestMethod]
		public void TestEmailDateTimeForFormatError()
		{
			InstallStation installstation = new InstallStation();
			InstallStationService service = new InstallStationService();
			installstation.Id="";
			installstation.Name="";
			installstation.Address="";
			installstation.Director="";
			installstation.DirectorPhone="";
			installstation.Contact="";
			installstation.ContactPhone="";
			installstation.Email="";
			installstation.CityCode="";
			installstation.Note="";
			SingleMessage<bool> ret = service.AddInstallStation(installstation);
			//Assert.IsFalse(ret.IsSuccess);
		}


		/// <summary>
		/// 测试ProvinceCode日期格式错误
		/// </summary>
		[TestMethod]
		public void TestProvinceCodeDateTimeForFormatError()
		{
			InstallStation installstation = new InstallStation();
			InstallStationService service = new InstallStationService();
			installstation.Id="";
			installstation.Name="";
			installstation.Address="";
			installstation.Director="";
			installstation.DirectorPhone="";
			installstation.Contact="";
			installstation.ContactPhone="";
			installstation.Email="";
			installstation.CityCode="";
			installstation.Note="";
			SingleMessage<bool> ret = service.AddInstallStation(installstation);
			//Assert.IsFalse(ret.IsSuccess);
		}


		/// <summary>
		/// 测试ProvinceName日期格式错误
		/// </summary>
		[TestMethod]
		public void TestProvinceNameDateTimeForFormatError()
		{
			InstallStation installstation = new InstallStation();
			InstallStationService service = new InstallStationService();
			installstation.Id="";
			installstation.Name="";
			installstation.Address="";
			installstation.Director="";
			installstation.DirectorPhone="";
			installstation.Contact="";
			installstation.ContactPhone="";
			installstation.Email="";
			installstation.CityCode="";
			installstation.Note="";
			SingleMessage<bool> ret = service.AddInstallStation(installstation);
			//Assert.IsFalse(ret.IsSuccess);
		}


		/// <summary>
		/// 测试CityCode日期格式错误
		/// </summary>
		[TestMethod]
		public void TestCityCodeDateTimeForFormatError()
		{
			InstallStation installstation = new InstallStation();
			InstallStationService service = new InstallStationService();
			installstation.Id="";
			installstation.Name="";
			installstation.Address="";
			installstation.Director="";
			installstation.DirectorPhone="";
			installstation.Contact="";
			installstation.ContactPhone="";
			installstation.Email="";
			installstation.CityCode="";
			installstation.Note="";
			SingleMessage<bool> ret = service.AddInstallStation(installstation);
			//Assert.IsFalse(ret.IsSuccess);
		}


		/// <summary>
		/// 测试CityName日期格式错误
		/// </summary>
		[TestMethod]
		public void TestCityNameDateTimeForFormatError()
		{
			InstallStation installstation = new InstallStation();
			InstallStationService service = new InstallStationService();
			installstation.Id="";
			installstation.Name="";
			installstation.Address="";
			installstation.Director="";
			installstation.DirectorPhone="";
			installstation.Contact="";
			installstation.ContactPhone="";
			installstation.Email="";
			installstation.CityCode="";
			installstation.Note="";
			SingleMessage<bool> ret = service.AddInstallStation(installstation);
			//Assert.IsFalse(ret.IsSuccess);
		}


		/// <summary>
		/// 测试Note日期格式错误
		/// </summary>
		[TestMethod]
		public void TestNoteDateTimeForFormatError()
		{
			InstallStation installstation = new InstallStation();
			InstallStationService service = new InstallStationService();
			installstation.Id="";
			installstation.Name="";
			installstation.Address="";
			installstation.Director="";
			installstation.DirectorPhone="";
			installstation.Contact="";
			installstation.ContactPhone="";
			installstation.Email="";
			installstation.CityCode="";
			installstation.Note="";
			SingleMessage<bool> ret = service.AddInstallStation(installstation);
			//Assert.IsFalse(ret.IsSuccess);
		}


		/// <summary>
		/// 测试Id日期最大值
		/// </summary>
		[TestMethod]
		public void TestIdDateTimeForMax()
		{
			InstallStation installstation = new InstallStation();
			InstallStationService service = new InstallStationService();
			installstation.Id="";
			installstation.Name="";
			installstation.Address="";
			installstation.Director="";
			installstation.DirectorPhone="";
			installstation.Contact="";
			installstation.ContactPhone="";
			installstation.Email="";
			installstation.CityCode="";
			installstation.Note="";
			SingleMessage<bool> ret = service.AddInstallStation(installstation);
			//Assert.IsFalse(ret.IsSuccess);
		}


		/// <summary>
		/// 测试Name日期最大值
		/// </summary>
		[TestMethod]
		public void TestNameDateTimeForMax()
		{
			InstallStation installstation = new InstallStation();
			InstallStationService service = new InstallStationService();
			installstation.Id="";
			installstation.Name="";
			installstation.Address="";
			installstation.Director="";
			installstation.DirectorPhone="";
			installstation.Contact="";
			installstation.ContactPhone="";
			installstation.Email="";
			installstation.CityCode="";
			installstation.Note="";
			SingleMessage<bool> ret = service.AddInstallStation(installstation);
			//Assert.IsFalse(ret.IsSuccess);
		}


		/// <summary>
		/// 测试Address日期最大值
		/// </summary>
		[TestMethod]
		public void TestAddressDateTimeForMax()
		{
			InstallStation installstation = new InstallStation();
			InstallStationService service = new InstallStationService();
			installstation.Id="";
			installstation.Name="";
			installstation.Address="";
			installstation.Director="";
			installstation.DirectorPhone="";
			installstation.Contact="";
			installstation.ContactPhone="";
			installstation.Email="";
			installstation.CityCode="";
			installstation.Note="";
			SingleMessage<bool> ret = service.AddInstallStation(installstation);
			//Assert.IsFalse(ret.IsSuccess);
		}


		/// <summary>
		/// 测试Director日期最大值
		/// </summary>
		[TestMethod]
		public void TestDirectorDateTimeForMax()
		{
			InstallStation installstation = new InstallStation();
			InstallStationService service = new InstallStationService();
			installstation.Id="";
			installstation.Name="";
			installstation.Address="";
			installstation.Director="";
			installstation.DirectorPhone="";
			installstation.Contact="";
			installstation.ContactPhone="";
			installstation.Email="";
			installstation.CityCode="";
			installstation.Note="";
			SingleMessage<bool> ret = service.AddInstallStation(installstation);
			//Assert.IsFalse(ret.IsSuccess);
		}


		/// <summary>
		/// 测试DirectorPhone日期最大值
		/// </summary>
		[TestMethod]
		public void TestDirectorPhoneDateTimeForMax()
		{
			InstallStation installstation = new InstallStation();
			InstallStationService service = new InstallStationService();
			installstation.Id="";
			installstation.Name="";
			installstation.Address="";
			installstation.Director="";
			installstation.DirectorPhone="";
			installstation.Contact="";
			installstation.ContactPhone="";
			installstation.Email="";
			installstation.CityCode="";
			installstation.Note="";
			SingleMessage<bool> ret = service.AddInstallStation(installstation);
			//Assert.IsFalse(ret.IsSuccess);
		}


		/// <summary>
		/// 测试Contact日期最大值
		/// </summary>
		[TestMethod]
		public void TestContactDateTimeForMax()
		{
			InstallStation installstation = new InstallStation();
			InstallStationService service = new InstallStationService();
			installstation.Id="";
			installstation.Name="";
			installstation.Address="";
			installstation.Director="";
			installstation.DirectorPhone="";
			installstation.Contact="";
			installstation.ContactPhone="";
			installstation.Email="";
			installstation.CityCode="";
			installstation.Note="";
			SingleMessage<bool> ret = service.AddInstallStation(installstation);
			//Assert.IsFalse(ret.IsSuccess);
		}


		/// <summary>
		/// 测试ContactPhone日期最大值
		/// </summary>
		[TestMethod]
		public void TestContactPhoneDateTimeForMax()
		{
			InstallStation installstation = new InstallStation();
			InstallStationService service = new InstallStationService();
			installstation.Id="";
			installstation.Name="";
			installstation.Address="";
			installstation.Director="";
			installstation.DirectorPhone="";
			installstation.Contact="";
			installstation.ContactPhone="";
			installstation.Email="";
			installstation.CityCode="";
			installstation.Note="";
			SingleMessage<bool> ret = service.AddInstallStation(installstation);
			//Assert.IsFalse(ret.IsSuccess);
		}


		/// <summary>
		/// 测试Email日期最大值
		/// </summary>
		[TestMethod]
		public void TestEmailDateTimeForMax()
		{
			InstallStation installstation = new InstallStation();
			InstallStationService service = new InstallStationService();
			installstation.Id="";
			installstation.Name="";
			installstation.Address="";
			installstation.Director="";
			installstation.DirectorPhone="";
			installstation.Contact="";
			installstation.ContactPhone="";
			installstation.Email="";
			installstation.CityCode="";
			installstation.Note="";
			SingleMessage<bool> ret = service.AddInstallStation(installstation);
			//Assert.IsFalse(ret.IsSuccess);
		}


		/// <summary>
		/// 测试ProvinceCode日期最大值
		/// </summary>
		[TestMethod]
		public void TestProvinceCodeDateTimeForMax()
		{
			InstallStation installstation = new InstallStation();
			InstallStationService service = new InstallStationService();
			installstation.Id="";
			installstation.Name="";
			installstation.Address="";
			installstation.Director="";
			installstation.DirectorPhone="";
			installstation.Contact="";
			installstation.ContactPhone="";
			installstation.Email="";
			installstation.CityCode="";
			installstation.Note="";
			SingleMessage<bool> ret = service.AddInstallStation(installstation);
			//Assert.IsFalse(ret.IsSuccess);
		}


		/// <summary>
		/// 测试ProvinceName日期最大值
		/// </summary>
		[TestMethod]
		public void TestProvinceNameDateTimeForMax()
		{
			InstallStation installstation = new InstallStation();
			InstallStationService service = new InstallStationService();
			installstation.Id="";
			installstation.Name="";
			installstation.Address="";
			installstation.Director="";
			installstation.DirectorPhone="";
			installstation.Contact="";
			installstation.ContactPhone="";
			installstation.Email="";
			installstation.CityCode="";
			installstation.Note="";
			SingleMessage<bool> ret = service.AddInstallStation(installstation);
			//Assert.IsFalse(ret.IsSuccess);
		}


		/// <summary>
		/// 测试CityCode日期最大值
		/// </summary>
		[TestMethod]
		public void TestCityCodeDateTimeForMax()
		{
			InstallStation installstation = new InstallStation();
			InstallStationService service = new InstallStationService();
			installstation.Id="";
			installstation.Name="";
			installstation.Address="";
			installstation.Director="";
			installstation.DirectorPhone="";
			installstation.Contact="";
			installstation.ContactPhone="";
			installstation.Email="";
			installstation.CityCode="";
			installstation.Note="";
			SingleMessage<bool> ret = service.AddInstallStation(installstation);
			//Assert.IsFalse(ret.IsSuccess);
		}


		/// <summary>
		/// 测试CityName日期最大值
		/// </summary>
		[TestMethod]
		public void TestCityNameDateTimeForMax()
		{
			InstallStation installstation = new InstallStation();
			InstallStationService service = new InstallStationService();
			installstation.Id="";
			installstation.Name="";
			installstation.Address="";
			installstation.Director="";
			installstation.DirectorPhone="";
			installstation.Contact="";
			installstation.ContactPhone="";
			installstation.Email="";
			installstation.CityCode="";
			installstation.Note="";
			SingleMessage<bool> ret = service.AddInstallStation(installstation);
			//Assert.IsFalse(ret.IsSuccess);
		}


		/// <summary>
		/// 测试Note日期最大值
		/// </summary>
		[TestMethod]
		public void TestNoteDateTimeForMax()
		{
			InstallStation installstation = new InstallStation();
			InstallStationService service = new InstallStationService();
			installstation.Id="";
			installstation.Name="";
			installstation.Address="";
			installstation.Director="";
			installstation.DirectorPhone="";
			installstation.Contact="";
			installstation.ContactPhone="";
			installstation.Email="";
			installstation.CityCode="";
			installstation.Note="";
			SingleMessage<bool> ret = service.AddInstallStation(installstation);
			//Assert.IsFalse(ret.IsSuccess);
		}


		/// <summary>
		/// 测试Id日期格式错误
		/// </summary>
		[TestMethod]
		public void TestIdDateTimeForMin()
		{
			InstallStation installstation = new InstallStation();
			InstallStationService service = new InstallStationService();
			installstation.Id="";
			installstation.Name="";
			installstation.Address="";
			installstation.Director="";
			installstation.DirectorPhone="";
			installstation.Contact="";
			installstation.ContactPhone="";
			installstation.Email="";
			installstation.CityCode="";
			installstation.Note="";
			SingleMessage<bool> ret = service.AddInstallStation(installstation);
			//Assert.IsFalse(ret.IsSuccess);
		}


		/// <summary>
		/// 测试Name日期格式错误
		/// </summary>
		[TestMethod]
		public void TestNameDateTimeForMin()
		{
			InstallStation installstation = new InstallStation();
			InstallStationService service = new InstallStationService();
			installstation.Id="";
			installstation.Name="";
			installstation.Address="";
			installstation.Director="";
			installstation.DirectorPhone="";
			installstation.Contact="";
			installstation.ContactPhone="";
			installstation.Email="";
			installstation.CityCode="";
			installstation.Note="";
			SingleMessage<bool> ret = service.AddInstallStation(installstation);
			//Assert.IsFalse(ret.IsSuccess);
		}


		/// <summary>
		/// 测试Address日期格式错误
		/// </summary>
		[TestMethod]
		public void TestAddressDateTimeForMin()
		{
			InstallStation installstation = new InstallStation();
			InstallStationService service = new InstallStationService();
			installstation.Id="";
			installstation.Name="";
			installstation.Address="";
			installstation.Director="";
			installstation.DirectorPhone="";
			installstation.Contact="";
			installstation.ContactPhone="";
			installstation.Email="";
			installstation.CityCode="";
			installstation.Note="";
			SingleMessage<bool> ret = service.AddInstallStation(installstation);
			//Assert.IsFalse(ret.IsSuccess);
		}


		/// <summary>
		/// 测试Director日期格式错误
		/// </summary>
		[TestMethod]
		public void TestDirectorDateTimeForMin()
		{
			InstallStation installstation = new InstallStation();
			InstallStationService service = new InstallStationService();
			installstation.Id="";
			installstation.Name="";
			installstation.Address="";
			installstation.Director="";
			installstation.DirectorPhone="";
			installstation.Contact="";
			installstation.ContactPhone="";
			installstation.Email="";
			installstation.CityCode="";
			installstation.Note="";
			SingleMessage<bool> ret = service.AddInstallStation(installstation);
			//Assert.IsFalse(ret.IsSuccess);
		}


		/// <summary>
		/// 测试DirectorPhone日期格式错误
		/// </summary>
		[TestMethod]
		public void TestDirectorPhoneDateTimeForMin()
		{
			InstallStation installstation = new InstallStation();
			InstallStationService service = new InstallStationService();
			installstation.Id="";
			installstation.Name="";
			installstation.Address="";
			installstation.Director="";
			installstation.DirectorPhone="";
			installstation.Contact="";
			installstation.ContactPhone="";
			installstation.Email="";
			installstation.CityCode="";
			installstation.Note="";
			SingleMessage<bool> ret = service.AddInstallStation(installstation);
			//Assert.IsFalse(ret.IsSuccess);
		}


		/// <summary>
		/// 测试Contact日期格式错误
		/// </summary>
		[TestMethod]
		public void TestContactDateTimeForMin()
		{
			InstallStation installstation = new InstallStation();
			InstallStationService service = new InstallStationService();
			installstation.Id="";
			installstation.Name="";
			installstation.Address="";
			installstation.Director="";
			installstation.DirectorPhone="";
			installstation.Contact="";
			installstation.ContactPhone="";
			installstation.Email="";
			installstation.CityCode="";
			installstation.Note="";
			SingleMessage<bool> ret = service.AddInstallStation(installstation);
			//Assert.IsFalse(ret.IsSuccess);
		}


		/// <summary>
		/// 测试ContactPhone日期格式错误
		/// </summary>
		[TestMethod]
		public void TestContactPhoneDateTimeForMin()
		{
			InstallStation installstation = new InstallStation();
			InstallStationService service = new InstallStationService();
			installstation.Id="";
			installstation.Name="";
			installstation.Address="";
			installstation.Director="";
			installstation.DirectorPhone="";
			installstation.Contact="";
			installstation.ContactPhone="";
			installstation.Email="";
			installstation.CityCode="";
			installstation.Note="";
			SingleMessage<bool> ret = service.AddInstallStation(installstation);
			//Assert.IsFalse(ret.IsSuccess);
		}


		/// <summary>
		/// 测试Email日期格式错误
		/// </summary>
		[TestMethod]
		public void TestEmailDateTimeForMin()
		{
			InstallStation installstation = new InstallStation();
			InstallStationService service = new InstallStationService();
			installstation.Id="";
			installstation.Name="";
			installstation.Address="";
			installstation.Director="";
			installstation.DirectorPhone="";
			installstation.Contact="";
			installstation.ContactPhone="";
			installstation.Email="";
			installstation.CityCode="";
			installstation.Note="";
			SingleMessage<bool> ret = service.AddInstallStation(installstation);
			//Assert.IsFalse(ret.IsSuccess);
		}


		/// <summary>
		/// 测试ProvinceCode日期格式错误
		/// </summary>
		[TestMethod]
		public void TestProvinceCodeDateTimeForMin()
		{
			InstallStation installstation = new InstallStation();
			InstallStationService service = new InstallStationService();
			installstation.Id="";
			installstation.Name="";
			installstation.Address="";
			installstation.Director="";
			installstation.DirectorPhone="";
			installstation.Contact="";
			installstation.ContactPhone="";
			installstation.Email="";
			installstation.CityCode="";
			installstation.Note="";
			SingleMessage<bool> ret = service.AddInstallStation(installstation);
			//Assert.IsFalse(ret.IsSuccess);
		}


		/// <summary>
		/// 测试ProvinceName日期格式错误
		/// </summary>
		[TestMethod]
		public void TestProvinceNameDateTimeForMin()
		{
			InstallStation installstation = new InstallStation();
			InstallStationService service = new InstallStationService();
			installstation.Id="";
			installstation.Name="";
			installstation.Address="";
			installstation.Director="";
			installstation.DirectorPhone="";
			installstation.Contact="";
			installstation.ContactPhone="";
			installstation.Email="";
			installstation.CityCode="";
			installstation.Note="";
			SingleMessage<bool> ret = service.AddInstallStation(installstation);
			//Assert.IsFalse(ret.IsSuccess);
		}


		/// <summary>
		/// 测试CityCode日期格式错误
		/// </summary>
		[TestMethod]
		public void TestCityCodeDateTimeForMin()
		{
			InstallStation installstation = new InstallStation();
			InstallStationService service = new InstallStationService();
			installstation.Id="";
			installstation.Name="";
			installstation.Address="";
			installstation.Director="";
			installstation.DirectorPhone="";
			installstation.Contact="";
			installstation.ContactPhone="";
			installstation.Email="";
			installstation.CityCode="";
			installstation.Note="";
			SingleMessage<bool> ret = service.AddInstallStation(installstation);
			//Assert.IsFalse(ret.IsSuccess);
		}


		/// <summary>
		/// 测试CityName日期格式错误
		/// </summary>
		[TestMethod]
		public void TestCityNameDateTimeForMin()
		{
			InstallStation installstation = new InstallStation();
			InstallStationService service = new InstallStationService();
			installstation.Id="";
			installstation.Name="";
			installstation.Address="";
			installstation.Director="";
			installstation.DirectorPhone="";
			installstation.Contact="";
			installstation.ContactPhone="";
			installstation.Email="";
			installstation.CityCode="";
			installstation.Note="";
			SingleMessage<bool> ret = service.AddInstallStation(installstation);
			//Assert.IsFalse(ret.IsSuccess);
		}


		/// <summary>
		/// 测试Note日期格式错误
		/// </summary>
		[TestMethod]
		public void TestNoteDateTimeForMin()
		{
			InstallStation installstation = new InstallStation();
			InstallStationService service = new InstallStationService();
			installstation.Id="";
			installstation.Name="";
			installstation.Address="";
			installstation.Director="";
			installstation.DirectorPhone="";
			installstation.Contact="";
			installstation.ContactPhone="";
			installstation.Email="";
			installstation.CityCode="";
			installstation.Note="";
			SingleMessage<bool> ret = service.AddInstallStation(installstation);
			//Assert.IsFalse(ret.IsSuccess);
		}


		/// <summary>
		/// update no exists
		/// </summary>
		[TestMethod]
		public void TestIdUpdateNotExist()
		{
			InstallStation installstation = new InstallStation();
			InstallStationService service = new InstallStationService();
			installstation.Id=DateTime.MinValue.ToLongDateString();
			installstation.Name="";
			installstation.Address="";
			installstation.Director="";
			installstation.DirectorPhone="";
			installstation.Contact="";
			installstation.ContactPhone="";
			installstation.Email="";
			installstation.CityCode="";
			installstation.Note="";
			SingleMessage<bool> ret = service.UpdateInstallStation(installstation);
			//Assert.IsFalse(ret.IsSuccess);
		}


		/// <summary>
		/// update no exists
		/// </summary>
		[TestMethod]
		public void TestNameUpdateNotExist()
		{
			InstallStation installstation = new InstallStation();
			InstallStationService service = new InstallStationService();
			installstation.Id="";
			installstation.Name=DateTime.MinValue.ToLongDateString();
			installstation.Address="";
			installstation.Director="";
			installstation.DirectorPhone="";
			installstation.Contact="";
			installstation.ContactPhone="";
			installstation.Email="";
			installstation.CityCode="";
			installstation.Note="";
			SingleMessage<bool> ret = service.UpdateInstallStation(installstation);
			//Assert.IsFalse(ret.IsSuccess);
		}


		/// <summary>
		/// update no exists
		/// </summary>
		[TestMethod]
		public void TestAddressUpdateNotExist()
		{
			InstallStation installstation = new InstallStation();
			InstallStationService service = new InstallStationService();
			installstation.Id="";
			installstation.Name="";
			installstation.Address=DateTime.MinValue.ToLongDateString();
			installstation.Director="";
			installstation.DirectorPhone="";
			installstation.Contact="";
			installstation.ContactPhone="";
			installstation.Email="";
			installstation.CityCode="";
			installstation.Note="";
			SingleMessage<bool> ret = service.UpdateInstallStation(installstation);
			//Assert.IsFalse(ret.IsSuccess);
		}


		/// <summary>
		/// update no exists
		/// </summary>
		[TestMethod]
		public void TestDirectorUpdateNotExist()
		{
			InstallStation installstation = new InstallStation();
			InstallStationService service = new InstallStationService();
			installstation.Id="";
			installstation.Name="";
			installstation.Address="";
			installstation.Director=DateTime.MinValue.ToString();
			installstation.DirectorPhone="";
			installstation.Contact="";
			installstation.ContactPhone="";
			installstation.Email="";
			installstation.CityCode="";
			installstation.Note="";
			SingleMessage<bool> ret = service.UpdateInstallStation(installstation);
			//Assert.IsFalse(ret.IsSuccess);
		}


		/// <summary>
		/// update no exists
		/// </summary>
		[TestMethod]
		public void TestDirectorPhoneUpdateNotExist()
		{
			InstallStation installstation = new InstallStation();
			InstallStationService service = new InstallStationService();
			installstation.Id="";
			installstation.Name="";
			installstation.Address="";
			installstation.Director="";
			installstation.DirectorPhone=DateTime.MinValue.ToString();
			installstation.Contact="";
			installstation.ContactPhone="";
			installstation.Email="";
			installstation.CityCode="";
			installstation.Note="";
			SingleMessage<bool> ret = service.UpdateInstallStation(installstation);
			//Assert.IsFalse(ret.IsSuccess);
		}


		/// <summary>
		/// update no exists
		/// </summary>
		[TestMethod]
		public void TestContactUpdateNotExist()
		{
			InstallStation installstation = new InstallStation();
			InstallStationService service = new InstallStationService();
			installstation.Id="";
			installstation.Name="";
			installstation.Address="";
			installstation.Director="";
			installstation.DirectorPhone="";
			installstation.Contact=DateTime.MinValue.ToString();
			installstation.ContactPhone="";
			installstation.Email="";
			installstation.CityCode="";
			installstation.Note="";
			SingleMessage<bool> ret = service.UpdateInstallStation(installstation);
			//Assert.IsFalse(ret.IsSuccess);
		}


		/// <summary>
		/// update no exists
		/// </summary>
		[TestMethod]
		public void TestContactPhoneUpdateNotExist()
		{
			InstallStation installstation = new InstallStation();
			InstallStationService service = new InstallStationService();
			installstation.Id="";
			installstation.Name="";
			installstation.Address="";
			installstation.Director="";
			installstation.DirectorPhone="";
			installstation.Contact="";
			installstation.ContactPhone=DateTime.MinValue.ToString();
			installstation.Email="";
			installstation.CityCode="";
			installstation.Note="";
			SingleMessage<bool> ret = service.UpdateInstallStation(installstation);
			//Assert.IsFalse(ret.IsSuccess);
		}


		/// <summary>
		/// update no exists
		/// </summary>
		[TestMethod]
		public void TestEmailUpdateNotExist()
		{
			InstallStation installstation = new InstallStation();
			InstallStationService service = new InstallStationService();
			installstation.Id="";
			installstation.Name="";
			installstation.Address="";
			installstation.Director="";
			installstation.DirectorPhone="";
			installstation.Contact="";
			installstation.ContactPhone="";
			installstation.Email=DateTime.MinValue.ToString();
			installstation.CityCode="";
			installstation.Note="";
			SingleMessage<bool> ret = service.UpdateInstallStation(installstation);
			//Assert.IsFalse(ret.IsSuccess);
		}


		/// <summary>
		/// update no exists
		/// </summary>
		[TestMethod]
		public void TestProvinceCodeUpdateNotExist()
		{
			InstallStation installstation = new InstallStation();
			InstallStationService service = new InstallStationService();
			installstation.Id="";
			installstation.Name="";
			installstation.Address="";
			installstation.Director="";
			installstation.DirectorPhone="";
			installstation.Contact="";
			installstation.ContactPhone="";
			installstation.Email="";
			installstation.ProvinceCode=DateTime.MinValue.ToString();
			installstation.CityCode="";
			installstation.Note="";
			SingleMessage<bool> ret = service.UpdateInstallStation(installstation);
			//Assert.IsFalse(ret.IsSuccess);
		}


		/// <summary>
		/// update no exists
		/// </summary>
		[TestMethod]
		public void TestProvinceNameUpdateNotExist()
		{
			InstallStation installstation = new InstallStation();
			InstallStationService service = new InstallStationService();
			installstation.Id="";
			installstation.Name="";
			installstation.Address="";
			installstation.Director="";
			installstation.DirectorPhone="";
			installstation.Contact="";
			installstation.ContactPhone="";
			installstation.Email="";
			installstation.ProvinceName=DateTime.MinValue.ToString();
			installstation.CityCode="";
			installstation.Note="";
			SingleMessage<bool> ret = service.UpdateInstallStation(installstation);
			//Assert.IsFalse(ret.IsSuccess);
		}


		/// <summary>
		/// update no exists
		/// </summary>
		[TestMethod]
		public void TestCityCodeUpdateNotExist()
		{
			InstallStation installstation = new InstallStation();
			InstallStationService service = new InstallStationService();
			installstation.Id="";
			installstation.Name="";
			installstation.Address="";
			installstation.Director="";
			installstation.DirectorPhone="";
			installstation.Contact="";
			installstation.ContactPhone="";
			installstation.Email="";
			installstation.CityCode=DateTime.MinValue.ToString();
			installstation.Note="";
			SingleMessage<bool> ret = service.UpdateInstallStation(installstation);
			//Assert.IsFalse(ret.IsSuccess);
		}


		/// <summary>
		/// update no exists
		/// </summary>
		[TestMethod]
		public void TestCityNameUpdateNotExist()
		{
			InstallStation installstation = new InstallStation();
			InstallStationService service = new InstallStationService();
			installstation.Id="";
			installstation.Name="";
			installstation.Address="";
			installstation.Director="";
			installstation.DirectorPhone="";
			installstation.Contact="";
			installstation.ContactPhone="";
			installstation.Email="";
			installstation.CityCode="";
			installstation.CityName=DateTime.MinValue.ToString();
			installstation.Note="";
			SingleMessage<bool> ret = service.UpdateInstallStation(installstation);
			//Assert.IsFalse(ret.IsSuccess);
		}


		/// <summary>
		/// update no exists
		/// </summary>
		[TestMethod]
		public void TestNoteUpdateNotExist()
		{
			InstallStation installstation = new InstallStation();
			InstallStationService service = new InstallStationService();
			installstation.Id="";
			installstation.Name="";
			installstation.Address="";
			installstation.Director="";
			installstation.DirectorPhone="";
			installstation.Contact="";
			installstation.ContactPhone="";
			installstation.Email="";
			installstation.CityCode="";
			installstation.Note=DateTime.MinValue.ToString();
			SingleMessage<bool> ret = service.UpdateInstallStation(installstation);
			//Assert.IsFalse(ret.IsSuccess);
		}


		/// <summary>
		/// update no exists
		/// </summary>
		[TestMethod]
		public void TestIdDeleteNotExist()
		{
			InstallStation installstation = new InstallStation();
			InstallStationService service = new InstallStationService();
			installstation.Id=DateTime.MinValue.ToString();
			installstation.Name="";
			installstation.Address="";
			installstation.Director="";
			installstation.DirectorPhone="";
			installstation.Contact="";
			installstation.ContactPhone="";
			installstation.Email="";
			installstation.CityCode="";
			installstation.Note="";
			SingleMessage<bool> ret = service.DeleteInstallStation(installstation.Id);
			//Assert.IsFalse(ret.IsSuccess);
		}


		/// <summary>
		/// update no exists
		/// </summary>
		[TestMethod]
		public void TestNameDeleteNotExist()
		{
			InstallStation installstation = new InstallStation();
			InstallStationService service = new InstallStationService();
			installstation.Id="";
			installstation.Name=DateTime.MinValue.ToString();
			installstation.Address="";
			installstation.Director="";
			installstation.DirectorPhone="";
			installstation.Contact="";
			installstation.ContactPhone="";
			installstation.Email="";
			installstation.CityCode="";
			installstation.Note="";
			SingleMessage<bool> ret = service.DeleteInstallStation(installstation.Id);
			//Assert.IsFalse(ret.IsSuccess);
		}


		/// <summary>
		/// update no exists
		/// </summary>
		[TestMethod]
		public void TestAddressDeleteNotExist()
		{
			InstallStation installstation = new InstallStation();
			InstallStationService service = new InstallStationService();
			installstation.Id="";
			installstation.Name="";
			installstation.Address=DateTime.MinValue.ToString();
			installstation.Director="";
			installstation.DirectorPhone="";
			installstation.Contact="";
			installstation.ContactPhone="";
			installstation.Email="";
			installstation.CityCode="";
			installstation.Note="";
			SingleMessage<bool> ret = service.DeleteInstallStation(installstation.Id);
			//Assert.IsFalse(ret.IsSuccess);
		}


		/// <summary>
		/// update no exists
		/// </summary>
		[TestMethod]
		public void TestDirectorDeleteNotExist()
		{
			InstallStation installstation = new InstallStation();
			InstallStationService service = new InstallStationService();
			installstation.Id="";
			installstation.Name="";
			installstation.Address="";
			installstation.Director=DateTime.MinValue.ToString();
			installstation.DirectorPhone="";
			installstation.Contact="";
			installstation.ContactPhone="";
			installstation.Email="";
			installstation.CityCode="";
			installstation.Note="";
			SingleMessage<bool> ret = service.DeleteInstallStation(installstation.Id);
			//Assert.IsFalse(ret.IsSuccess);
		}


		/// <summary>
		/// update no exists
		/// </summary>
		[TestMethod]
		public void TestDirectorPhoneDeleteNotExist()
		{
			InstallStation installstation = new InstallStation();
			InstallStationService service = new InstallStationService();
			installstation.Id="";
			installstation.Name="";
			installstation.Address="";
			installstation.Director="";
			installstation.DirectorPhone=DateTime.MinValue.ToString();
			installstation.Contact="";
			installstation.ContactPhone="";
			installstation.Email="";
			installstation.CityCode="";
			installstation.Note="";
			SingleMessage<bool> ret = service.DeleteInstallStation(installstation.Id);
			//Assert.IsFalse(ret.IsSuccess);
		}


		/// <summary>
		/// update no exists
		/// </summary>
		[TestMethod]
		public void TestContactDeleteNotExist()
		{
			InstallStation installstation = new InstallStation();
			InstallStationService service = new InstallStationService();
			installstation.Id="";
			installstation.Name="";
			installstation.Address="";
			installstation.Director="";
			installstation.DirectorPhone="";
			installstation.Contact=DateTime.MinValue.ToString();
			installstation.ContactPhone="";
			installstation.Email="";
			installstation.CityCode="";
			installstation.Note="";
			SingleMessage<bool> ret = service.DeleteInstallStation(installstation.Id);
			//Assert.IsFalse(ret.IsSuccess);
		}


		/// <summary>
		/// update no exists
		/// </summary>
		[TestMethod]
		public void TestContactPhoneDeleteNotExist()
		{
			InstallStation installstation = new InstallStation();
			InstallStationService service = new InstallStationService();
			installstation.Id="";
			installstation.Name="";
			installstation.Address="";
			installstation.Director="";
			installstation.DirectorPhone="";
			installstation.Contact="";
			installstation.ContactPhone=DateTime.MinValue.ToString();
			installstation.Email="";
			installstation.CityCode="";
			installstation.Note="";
			SingleMessage<bool> ret = service.DeleteInstallStation(installstation.Id);
			//Assert.IsFalse(ret.IsSuccess);
		}


		/// <summary>
		/// update no exists
		/// </summary>
		[TestMethod]
		public void TestEmailDeleteNotExist()
		{
			InstallStation installstation = new InstallStation();
			InstallStationService service = new InstallStationService();
			installstation.Id="";
			installstation.Name="";
			installstation.Address="";
			installstation.Director="";
			installstation.DirectorPhone="";
			installstation.Contact="";
			installstation.ContactPhone="";
			installstation.Email=DateTime.MinValue.ToString();
			installstation.CityCode="";
			installstation.Note="";
			SingleMessage<bool> ret = service.DeleteInstallStation(installstation.Id);
			//Assert.IsFalse(ret.IsSuccess);
		}


		/// <summary>
		/// update no exists
		/// </summary>
		[TestMethod]
		public void TestProvinceCodeDeleteNotExist()
		{
			InstallStation installstation = new InstallStation();
			InstallStationService service = new InstallStationService();
			installstation.Id="";
			installstation.Name="";
			installstation.Address="";
			installstation.Director="";
			installstation.DirectorPhone="";
			installstation.Contact="";
			installstation.ContactPhone="";
			installstation.Email="";
			installstation.ProvinceCode=DateTime.MinValue.ToString();
			installstation.CityCode="";
			installstation.Note="";
			SingleMessage<bool> ret = service.DeleteInstallStation(installstation.Id);
			//Assert.IsFalse(ret.IsSuccess);
		}


		/// <summary>
		/// update no exists
		/// </summary>
		[TestMethod]
		public void TestProvinceNameDeleteNotExist()
		{
			InstallStation installstation = new InstallStation();
			InstallStationService service = new InstallStationService();
			installstation.Id="";
			installstation.Name="";
			installstation.Address="";
			installstation.Director="";
			installstation.DirectorPhone="";
			installstation.Contact="";
			installstation.ContactPhone="";
			installstation.Email="";
			installstation.ProvinceName=DateTime.MinValue.ToString();
			installstation.CityCode="";
			installstation.Note="";
			SingleMessage<bool> ret = service.DeleteInstallStation(installstation.Id);
			//Assert.IsFalse(ret.IsSuccess);
		}


		/// <summary>
		/// update no exists
		/// </summary>
		[TestMethod]
		public void TestCityCodeDeleteNotExist()
		{
			InstallStation installstation = new InstallStation();
			InstallStationService service = new InstallStationService();
			installstation.Id="";
			installstation.Name="";
			installstation.Address="";
			installstation.Director="";
			installstation.DirectorPhone="";
			installstation.Contact="";
			installstation.ContactPhone="";
			installstation.Email="";
			installstation.CityCode=DateTime.MinValue.ToString();
			installstation.Note="";
			SingleMessage<bool> ret = service.DeleteInstallStation(installstation.Id);
			//Assert.IsFalse(ret.IsSuccess);
		}


		/// <summary>
		/// update no exists
		/// </summary>
		[TestMethod]
		public void TestCityNameDeleteNotExist()
		{
			InstallStation installstation = new InstallStation();
			InstallStationService service = new InstallStationService();
			installstation.Id="";
			installstation.Name="";
			installstation.Address="";
			installstation.Director="";
			installstation.DirectorPhone="";
			installstation.Contact="";
			installstation.ContactPhone="";
			installstation.Email="";
			installstation.CityCode="";
			installstation.CityName=DateTime.MinValue.ToString();
			installstation.Note="";
			SingleMessage<bool> ret = service.DeleteInstallStation(installstation.Id);
			//Assert.IsFalse(ret.IsSuccess);
		}


		/// <summary>
		/// update no exists
		/// </summary>
		[TestMethod]
		public void TestNoteDeleteNotExist()
		{
			InstallStation installstation = new InstallStation();
			InstallStationService service = new InstallStationService();
			installstation.Id="";
			installstation.Name="";
			installstation.Address="";
			installstation.Director="";
			installstation.DirectorPhone="";
			installstation.Contact="";
			installstation.ContactPhone="";
			installstation.Email="";
			installstation.CityCode="";
			installstation.Note=DateTime.MinValue.ToString();
			SingleMessage<bool> ret = service.DeleteInstallStation(installstation.Id);
			//Assert.IsFalse(ret.IsSuccess);
		}

	}
}

