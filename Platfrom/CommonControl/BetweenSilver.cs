using Gsafety.PTMS.Share;
using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace Gsafety.Common.Controls
{
    public class BetweenSilver : ContentControl
    {
        #region 成员
        private Canvas canvasTop = null;
        private Canvas canvasTopTextBlock = null;
        private Canvas canvansSubScript = null;
        private Canvas canvansSubScriptText = null;
        private Canvas canvasMain = null;
        private Grid gridTop = null;
        private Rectangle regBarBg = null;
        private Border borBarBetween = null;
        private Rectangle regBarMiddleHandle = null;
        private SilverPointer silverPointerStart = null;
        private SilverPointer silverPointerEnd = null;
        private Button btnPrev = null;
        private Button btnNext = null;

        private SilverSlecetCycleEnum silverSelectCycle = SilverSlecetCycleEnum.Date_Time;
        private int silverStep = 60;
        private DateTime maxDateTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 23, 59, 59);
        private DateTime minDateTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0);
        private DateTime startDateTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 7, 15, 0);
        private DateTime endDateTime = DateTime.Now;

        private string dateFormat = "HH:mm";
        private Visibility silverPointerToolTipVisility = Visibility.Visible;

        private bool isMouseDowned = false;
        private Point oldPoint;
        private double subPointX = 0;
        private double borBarLeft = 0;
        private double silverPStartLeft = 0;
        private double silverPEndLeft = 0;
        private double oldBorBarBetweenWidth = 0;

        private double oneMinutesStep = 0;

        /// <summary>
        /// 标刻尺间隔
        /// </summary>
        private double interval = 0;
        private double borBarMagin = 7;//控制条的左右间距

        private bool isSelfArrange = false;
        private bool isFirstLoaded = true;
        #endregion

        #region 属性
        /// <summary>
        /// 选择范围类型
        /// </summary>
        public SilverSlecetCycleEnum SilverSlecetCycle
        {
            get
            {
                return this.silverSelectCycle;
            }
            private set
            {
                this.silverSelectCycle = value;
            }
        }

        /// <summary>
        /// 每次切换步长,单位分钟
        /// </summary>
        public int SilverStep
        {
            get { return silverStep; }
            private set
            {
                silverStep = value;
            }
        }

        /// <summary>
        /// 可以选择的最大时间值
        /// </summary>
        public DateTime MaxDateTime
        {
            get { return maxDateTime; }
            private set { maxDateTime = value; }
        }

        /// <summary>
        /// 可以选择的最小时间值
        /// </summary>
        public DateTime MinDateTime
        {
            get { return minDateTime; }
            private set { minDateTime = value; }
        }

        /// <summary>
        /// 选中的开始时间值
        /// </summary>
        public DateTime StartDateTime
        {
            get { return startDateTime; }
            set
            {
                if (value < minDateTime)
                {
                    MessageBoxHelper.ShowDialog(ApplicationContext.Instance.StringResourceReader.GetString("TimeError"));
                    return;
                }

                if (value >= EndDateTime)
                {
                    MessageBoxHelper.ShowDialog(ApplicationContext.Instance.StringResourceReader.GetString("TimeError"));
                    return;
                }

                this.isSelfArrange = true;
                startDateTime = value;
                this.InvalidateArrange();
            }
        }

        /// <summary>
        /// 选中的结束时间值
        /// </summary>
        public DateTime EndDateTime
        {
            get { return endDateTime; }
            set
            {
                if (value > maxDateTime)
                {
                    MessageBoxHelper.ShowDialog(ApplicationContext.Instance.StringResourceReader.GetString("EndTimeMinMaxTime"));
                    return;
                }

                if (value <= StartDateTime)
                {
                    MessageBoxHelper.ShowDialog(ApplicationContext.Instance.StringResourceReader.GetString("TimeError"));
                    return;
                }

                endDateTime = value;
                this.isSelfArrange = true;
                this.InvalidateArrange();
            }
        }

        /// <summary>
        /// 显示或隐藏划动锚点的提示，默认显示
        /// </summary>
        public Visibility SilverPointerToolTipVisility
        {
            get
            {
                return this.silverPointerToolTipVisility;
            }
            set
            {
                this.silverPointerToolTipVisility = value;
                if (this.silverPointerStart != null && this.silverPointerEnd != null)
                {
                    this.silverPointerStart.ToolTipVisibility = this.silverPointerToolTipVisility;
                    this.silverPointerEnd.ToolTipVisibility = this.silverPointerToolTipVisility;
                }
            }
        }
        #endregion

        #region 注册事件
        public delegate void DateTimerChanged(object sender, BetweenSilverEventArgs e);
        public event DateTimerChanged BetweenDateTimeChanged;
        #endregion

        #region 构造函数
        public BetweenSilver()
        {
            this.DefaultStyleKey = typeof(BetweenSilver);
        }
        #endregion

        #region 方法重写
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            this.canvasTop = this.GetTemplateChild("canvasTop") as Canvas;
            this.gridTop = this.GetTemplateChild("gridTop") as Grid;
            this.canvasTopTextBlock = this.GetTemplateChild("canvasTopTextBlock") as Canvas;
            this.canvansSubScript = this.GetTemplateChild("canvansSubScript") as Canvas;
            this.canvansSubScriptText = this.GetTemplateChild("canvansSubScriptText") as Canvas;
            this.canvasMain = this.GetTemplateChild("canvasMain") as Canvas;
            this.regBarBg = this.GetTemplateChild("regBarBg") as Rectangle;
            this.borBarBetween = this.GetTemplateChild("borBarBetween") as Border;
            this.regBarMiddleHandle = this.GetTemplateChild("regBarMiddleHandle") as Rectangle;
            this.silverPointerStart = this.GetTemplateChild("silverPointerStart") as SilverPointer;
            this.silverPointerEnd = this.GetTemplateChild("silverPointerEnd") as SilverPointer;
            this.btnPrev = this.GetTemplateChild("btnPrev") as Button;
            this.btnNext = this.GetTemplateChild("btnNext") as Button;

            if (this.silverPointerStart != null && this.canvasMain != null)
            {
                this.silverPointerStart.ToolTipVisibility = this.silverPointerToolTipVisility;
                this.silverPointerStart.MouseLeftButtonDown += new MouseButtonEventHandler(silverPointerStart_MouseLeftButtonDown);
                this.silverPointerStart.MouseLeftButtonUp += new MouseButtonEventHandler(silverPointerStart_MouseLeftButtonUp);
                this.silverPointerStart.MouseMove += new MouseEventHandler(silverPointerStart_MouseMove);
            }

            if (this.silverPointerEnd != null && this.canvasMain != null)
            {
                this.silverPointerEnd.ToolTipVisibility = this.silverPointerToolTipVisility;
                this.silverPointerEnd.MouseLeftButtonDown += new MouseButtonEventHandler(silverPointerEnd_MouseLeftButtonDown);
                this.silverPointerEnd.MouseLeftButtonUp += new MouseButtonEventHandler(silverPointerEnd_MouseLeftButtonUp);
                this.silverPointerEnd.MouseMove += new MouseEventHandler(silverPointerEnd_MouseMove);
            }

            if (this.regBarMiddleHandle != null && this.canvasMain != null && this.borBarBetween != null)
            {
                this.regBarMiddleHandle.MouseLeftButtonDown += new MouseButtonEventHandler(regBarMiddleHandle_MouseLeftButtonDown);
                this.regBarMiddleHandle.MouseLeftButtonUp += new MouseButtonEventHandler(regBarMiddleHandle_MouseLeftButtonUp);
                this.regBarMiddleHandle.MouseMove += new MouseEventHandler(regBarMiddleHandle_MouseMove);
            }

            if (this.btnPrev != null)
            {
                this.btnPrev.Click += new RoutedEventHandler(btnPrev_Click);
            }

            if (this.btnNext != null)
            {
                this.btnNext.Click += new RoutedEventHandler(btnNext_Click);
            }

            this.CreateSubScript();
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            this.SetSubScriptPosition(finalSize.Width);
            return base.ArrangeOverride(finalSize);
        }

        protected override Size MeasureOverride(Size availableSize)
        {
            return base.MeasureOverride(availableSize);
        }
        #endregion

        #region 初始化

        #endregion

        #region 事件处理
        #region silverPointerStart
        void silverPointerStart_MouseMove(object sender, MouseEventArgs e)
        {
            if (this.isMouseDowned)
            {
                SilverPointer tempSp = sender as SilverPointer;
                Point newPoint = e.GetPosition(this.canvasMain);
                double tempSub = (newPoint.X - oldPoint.X) - this.subPointX;
                double tempActualSub = newPoint.X - oldPoint.X;
                double tempSilverStartLeft = this.oldPoint.X + tempSub;
                double tempBorBarLeft = this.borBarLeft + (newPoint.X - oldPoint.X);
                double tempBorBarWidth = this.oldBorBarBetweenWidth - (newPoint.X - oldPoint.X);
                bool isStop = false;
                if (tempSilverStartLeft >= this.silverPEndLeft - this.interval)
                {
                    tempSilverStartLeft = this.silverPEndLeft - this.interval;
                    tempBorBarLeft = tempSilverStartLeft + borBarMagin;
                    tempBorBarWidth = this.interval;
                    isStop = true;
                }

                if (tempSilverStartLeft <= 0)
                {
                    tempSilverStartLeft = 0;
                    tempBorBarLeft = borBarMagin;
                    tempBorBarWidth = this.silverPEndLeft;
                    isStop = true;
                }

                if (this.oneMinutesStep != 0 && !isStop)
                {
                    double tempSubMinute = tempActualSub / this.oneMinutesStep;
                    DateTime tempTime = this.GetSubDateTime(this.startDateTime, (int)tempSubMinute);
                    tempSp.ToolTipText = tempTime.ToString(this.dateFormat);
                }
                Canvas.SetLeft(tempSp, tempSilverStartLeft);
                Canvas.SetLeft(this.borBarBetween, tempBorBarLeft);
                this.borBarBetween.Width = tempBorBarWidth;
            }
        }

        void silverPointerStart_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            SilverPointer tempSp = sender as SilverPointer;
            tempSp.ReleaseMouseCapture();
            this.isMouseDowned = false;
            SetMouseUpNearPointer(tempSp, ChangedPositionEnum.Start);
            this.DateTimeCallBack();

            tempSp.ToolTipText = this.startDateTime.ToString(this.dateFormat);
        }

        void silverPointerStart_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            SilverPointer tempSp = sender as SilverPointer;
            tempSp.CaptureMouse();
            this.isMouseDowned = true;
            this.oldPoint = e.GetPosition(this.canvasMain);
            this.subPointX = e.GetPosition(tempSp).X;
            this.borBarLeft = Canvas.GetLeft(this.borBarBetween);
            this.silverPEndLeft = Canvas.GetLeft(this.silverPointerEnd);
            this.oldBorBarBetweenWidth = this.borBarBetween.ActualWidth;
        }
        #endregion

        #region silverPointerEnd
        void silverPointerEnd_MouseMove(object sender, MouseEventArgs e)
        {
            if (this.isMouseDowned)
            {
                SilverPointer tempSp = sender as SilverPointer;
                Point newPoint = e.GetPosition(this.canvasMain);
                double tempActualSub = newPoint.X - oldPoint.X;
                double tempSub = (newPoint.X - oldPoint.X) - this.subPointX;
                double tempSilverEndLeft = this.oldPoint.X + tempSub;
                double tempBorBarWidth = this.oldBorBarBetweenWidth + (newPoint.X - oldPoint.X);
                bool isStop = false;
                if (tempSilverEndLeft <= this.silverPStartLeft + this.interval)
                {
                    tempSilverEndLeft = this.silverPStartLeft + this.interval;
                    tempBorBarWidth = this.interval;
                    isStop = true;
                }

                if (tempSilverEndLeft >= (this.canvasMain.ActualWidth - this.silverPointerEnd.ActualWidth))
                {
                    tempSilverEndLeft = this.canvasMain.ActualWidth - this.silverPointerEnd.ActualWidth;
                    tempBorBarWidth = tempSilverEndLeft - this.silverPStartLeft;
                    isStop = true;
                }

                if (this.oneMinutesStep != 0 && !isStop)
                {
                    double tempSubMinute = tempActualSub / this.oneMinutesStep;
                    DateTime tempTime = this.GetSubDateTime(this.endDateTime, (int)tempSubMinute);
                    tempSp.ToolTipText = tempTime.ToString(this.dateFormat);
                }

                Canvas.SetLeft(tempSp, tempSilverEndLeft);
                this.borBarBetween.Width = tempBorBarWidth;
            }
        }

        void silverPointerEnd_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            SilverPointer tempSp = sender as SilverPointer;
            tempSp.ReleaseMouseCapture();
            this.isMouseDowned = false;
            SetMouseUpNearPointer(tempSp, ChangedPositionEnum.End);
            this.DateTimeCallBack();
            tempSp.ToolTipText = this.endDateTime.ToString(this.dateFormat);
        }

        void silverPointerEnd_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (Canvas.GetLeft(this.silverPointerStart) == Canvas.GetLeft(this.silverPointerEnd) && (Canvas.GetLeft(this.silverPointerStart) + this.silverPointerStart.ActualWidth / 2) == Canvas.GetLeft(this.canvansSubScript.Children[this.canvansSubScript.Children.Count - 1]))
            {
                this.silverPointerStart_MouseLeftButtonDown(this.silverPointerStart, e);
            }
            else
            {
                SilverPointer tempSp = sender as SilverPointer;
                tempSp.CaptureMouse();
                this.isMouseDowned = true;
                this.oldPoint = e.GetPosition(this.canvasMain);
                this.subPointX = e.GetPosition(tempSp).X;
                this.oldBorBarBetweenWidth = this.borBarBetween.ActualWidth;
                this.silverPStartLeft = Canvas.GetLeft(this.silverPointerStart);
            }
        }
        #endregion

        #region regBarMiddleHandle
        void regBarMiddleHandle_MouseMove(object sender, MouseEventArgs e)
        {
            if (this.isMouseDowned)
            {
                Point newPoint = e.GetPosition(this.canvasMain);
                double tempSub = (newPoint.X - oldPoint.X);
                double tempBorBarLeft = this.borBarLeft + tempSub;
                double tempSilverStartLeft = this.silverPStartLeft + tempSub;
                double tempSilverEndLeft = this.silverPEndLeft + tempSub;
                bool isStop = false;
                if (tempSilverStartLeft <= 0)
                {
                    tempSilverStartLeft = 0;
                    tempSilverEndLeft = this.borBarBetween.ActualWidth;
                    tempBorBarLeft = this.borBarMagin;
                    isStop = true;
                }

                if (tempSilverEndLeft >= (this.canvasMain.ActualWidth - this.silverPointerEnd.ActualWidth))
                {
                    tempSilverStartLeft = this.canvasMain.ActualWidth - this.silverPointerEnd.ActualWidth - this.borBarBetween.ActualWidth;
                    tempSilverEndLeft = this.canvasMain.ActualWidth - this.silverPointerEnd.ActualWidth;
                    tempBorBarLeft = tempSilverStartLeft + this.borBarMagin;
                    isStop = true;
                }

                if (this.oneMinutesStep != 0 && !isStop)
                {
                    double tempSubMinute = tempSub / this.oneMinutesStep;
                    DateTime tempTime = this.GetSubDateTime(this.startDateTime, (int)tempSubMinute);
                    this.silverPointerStart.ToolTipText = tempTime.ToString(this.dateFormat);


                    tempTime = this.GetSubDateTime(this.endDateTime, (int)tempSubMinute);
                    this.silverPointerEnd.ToolTipText = tempTime.ToString(this.dateFormat);

                }

                Canvas.SetLeft(this.borBarBetween, tempBorBarLeft);
                Canvas.SetLeft(this.silverPointerStart, tempSilverStartLeft);
                Canvas.SetLeft(this.silverPointerEnd, tempSilverEndLeft);
            }
        }

        void regBarMiddleHandle_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            Rectangle tempRect = sender as Rectangle;
            tempRect.ReleaseMouseCapture();
            this.isMouseDowned = false;
            SetMouseUpNearPointer(this.silverPointerStart, ChangedPositionEnum.Handle);
            this.DateTimeCallBack();

            this.silverPointerStart.ToolTipText = this.startDateTime.ToString(this.dateFormat);
            this.silverPointerEnd.ToolTipText = this.endDateTime.ToString(this.dateFormat);
        }

        void regBarMiddleHandle_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Rectangle tempRect = sender as Rectangle;
            tempRect.CaptureMouse();
            this.isMouseDowned = true;
            this.oldPoint = e.GetPosition(this.canvasMain);
            this.subPointX = e.GetPosition(tempRect).X;
            this.borBarLeft = Canvas.GetLeft(this.borBarBetween);
            this.silverPStartLeft = Canvas.GetLeft(this.silverPointerStart);
            this.silverPEndLeft = Canvas.GetLeft(this.silverPointerEnd);
        }
        #endregion

        #region 向上、向下按钮
        void btnNext_Click(object sender, RoutedEventArgs e)
        {
            if (this.silverSelectCycle == SilverSlecetCycleEnum.Month)
            {
                this.MoveDateTimeForMonth(1);
            }
            else if (this.silverSelectCycle == SilverSlecetCycleEnum.Day)
            {
                this.MoveDateTimeForDay(1);
            }
            this.silverPointerStart.ToolTipText = this.startDateTime.ToString(this.dateFormat);
            this.silverPointerEnd.ToolTipText = this.endDateTime.ToString(this.dateFormat);
        }

        void btnPrev_Click(object sender, RoutedEventArgs e)
        {
            if (this.silverSelectCycle == SilverSlecetCycleEnum.Month)
            {
                this.MoveDateTimeForMonth(-1);
            }
            else if (this.silverSelectCycle == SilverSlecetCycleEnum.Day)
            {
                this.MoveDateTimeForDay(-1);
            }
            this.silverPointerStart.ToolTipText = this.startDateTime.ToString(this.dateFormat);
            this.silverPointerEnd.ToolTipText = this.endDateTime.ToString(this.dateFormat);
        }
        #endregion
        #endregion

        #region 公共方法
        /// <summary>
        /// 初始化控件
        /// </summary>
        /// <param name="silverSelectCycle">日期类型</param>
        /// <param name="silverStep">步长，除Date_Time类型，其他默认为1就可以了</param>
        /// <param name="maxDateTime">最大时间</param>
        /// <param name="minDateTime">最小时间，除Date_Time类型，其他类型设置该字段无效</param>
        public void SetBetweenSilverPropertys(SilverSlecetCycleEnum silverSelectCycle, int silverStep, DateTime maxDateTime, DateTime minDateTime)
        {
            if (silverStep <= 0)
            {
                MessageBoxHelper.ShowDialog(ApplicationContext.Instance.StringResourceReader.GetString("LengUpZero"));
                return;
            }

            if (silverSelectCycle == SilverSlecetCycleEnum.Date_Time)
            {
                if (maxDateTime <= minDateTime)
                {
                    MessageBoxHelper.ShowDialog(ApplicationContext.Instance.StringResourceReader.GetString("MaxUpMin"));
                    return;
                }
            }

            this.silverSelectCycle = silverSelectCycle;
            this.silverStep = silverStep;
            this.maxDateTime = maxDateTime;
            this.minDateTime = minDateTime;

            this.CreateSubScript();
            this.isSelfArrange = true;
            this.InvalidateArrange();
        }

        /// <summary>
        /// 初始化控件
        /// </summary>
        /// <param name="silverSelectCycle">日期类型</param>
        /// <param name="silverStep">步长，除Date_Time类型，其他默认为1就可以了</param>
        /// <param name="maxDateTime">最大时间</param>
        /// <param name="minDateTime">最小时间，除Date_Time类型，其他类型设置该字段无效</param>
        /// <param name="startDateTime">选择的开始时间</param>
        /// <param name="endDateTime">选择的结束时间</param>
        public void SetBetweenSilverPropertys(SilverSlecetCycleEnum silverSelectCycle, int silverStep, DateTime maxDateTime, DateTime minDateTime,
            DateTime startDateTime, DateTime endDateTime)
        {
            if (startDateTime >= endDateTime)
            {
                MessageBoxHelper.ShowDialog(ApplicationContext.Instance.StringResourceReader.GetString("TimeError"));
                return;
            }

            if (silverSelectCycle == SilverSlecetCycleEnum.Date_Time)
            {
                if (startDateTime < minDateTime)
                {
                    MessageBoxHelper.ShowDialog(ApplicationContext.Instance.StringResourceReader.GetString("BegTimeUpMinTime"));
                    return;
                }
            }

            if (endDateTime > maxDateTime)
            {
                MessageBoxHelper.ShowDialog(ApplicationContext.Instance.StringResourceReader.GetString("EndTimeMinMaxTime"));
                return;
            }

            this.startDateTime = startDateTime;
            this.endDateTime = endDateTime;

            this.SetBetweenSilverPropertys(silverSelectCycle, silverStep, maxDateTime, minDateTime);
        }
        #endregion

        #region 私有方法
        private Rectangle CreateRectangle(double height)
        {
            Rectangle rect = new Rectangle();
            rect.Fill = new SolidColorBrush(Colors.Black);
            rect.Height = height;
            rect.Width = 1;

            return rect;
        }

        private TextBlock CreateTextBlock(string text, double fontSize = 10)
        {
            TextBlock tb = new TextBlock();
            tb.Foreground = new SolidColorBrush(Colors.Black);
            tb.Text = text;
            tb.FontSize = fontSize;

            return tb;
        }

        private void CreateSubScript()
        {
            switch (this.silverSelectCycle)
            {
                case SilverSlecetCycleEnum.Date_Time:
                    this.dateFormat = "HH:mm";
                    if (this.gridTop != null)
                    {
                        this.gridTop.Visibility = Visibility.Collapsed;
                    }
                    this.CreateWithDateTime();
                    break;
                case SilverSlecetCycleEnum.Day:
                    this.dateFormat = "yyyyMMdd";
                    if (this.gridTop != null)
                    {
                        this.gridTop.Visibility = Visibility.Visible;
                    }
                    this.CreateWithDay();
                    break;
                case SilverSlecetCycleEnum.Month:
                    this.dateFormat = "yyyyMM";
                    if (this.gridTop != null)
                    {
                        this.gridTop.Visibility = Visibility.Visible;
                    }
                    this.CreateWithMonth();
                    break;
                default:
                    if (this.gridTop != null)
                    {
                        this.gridTop.Visibility = Visibility.Collapsed;
                    }
                    this.CreateWithDateTime();
                    this.dateFormat = "HH:mm";
                    break;
            }
        }

        private void CreateWithDateTime()
        {
            if (this.canvansSubScript == null || this.canvansSubScriptText == null)
                return;

            this.canvansSubScript.Children.Clear();
            this.canvansSubScriptText.Children.Clear();
            int index = 0;
            for (DateTime dt = this.minDateTime; dt <= this.maxDateTime; dt = dt.AddMinutes(1))
            {
                if (dt.Minute % this.silverStep == 0 || dt == this.minDateTime || dt == this.maxDateTime)
                {
                    int height = 4;
                    if (dt.ToString("mm:ss") == "00:00")
                    {
                        height = 8;
                        string text = dt.Hour.ToString();
                        if (index != 0 && dt.Hour == 0)
                        {
                            text = "24";
                        }
                        this.canvansSubScriptText.Children.Add(this.CreateTextBlock(text));
                    }
                    Rectangle rect = this.CreateRectangle(height);
                    rect.Tag = dt;
                    this.canvansSubScript.Children.Add(rect);
                }
                index++;
            }

            this.silverPointerStart.ToolTipText = this.startDateTime.ToString(this.dateFormat);
            this.silverPointerEnd.ToolTipText = this.endDateTime.ToString(this.dateFormat);
        }

        private void CreateWithDay()
        {
            if (this.canvansSubScript == null || this.canvansSubScriptText == null || this.canvasTop == null || this.canvasTopTextBlock == null)
                return;
            this.canvansSubScript.Children.Clear();
            this.canvansSubScriptText.Children.Clear();
            this.canvasTop.Children.Clear();
            this.canvasTopTextBlock.Children.Clear();

            this.minDateTime = this.maxDateTime.AddDays(-45);
            for (DateTime dt = this.minDateTime; dt <= this.maxDateTime; dt = dt.AddDays(1))
            {
                this.canvansSubScriptText.Children.Add(this.CreateTextBlock(dt.Day.ToString()));
                Rectangle rect = this.CreateRectangle(4);
                rect.Tag = dt;
                this.canvansSubScript.Children.Add(rect);
                if (dt.Day == 1)
                {
                    rect = this.CreateRectangle(30);
                    this.canvasTop.Children.Add(rect);
                    this.canvasTopTextBlock.Children.Add(this.CreateTextBlock(dt.ToString("yyyy年MM月"), 16));
                }
            }

            if (this.canvasTop.Children.Count <= 1)
            {
                Rectangle rect = this.CreateRectangle(30);
                this.canvasTop.Children.Insert(0, rect);
                this.canvasTopTextBlock.Children.Insert(0, this.CreateTextBlock(this.minDateTime.ToString("yyyy年MM月"), 16));
                TextBlock tx = this.CreateTextBlock(this.maxDateTime.ToString("yyyy年MM月"), 16);
                this.canvasTopTextBlock.Children.Add(tx);
            }
            else if (this.canvasTop.Children.Count <= 2)
            {
                this.canvasTopTextBlock.Children.Insert(0, this.CreateTextBlock(this.minDateTime.ToString("yyyy年MM月"), 16));
            }
        }

        private void CreateWithMonth()
        {
            if (this.canvansSubScript == null || this.canvansSubScriptText == null || this.canvasTop == null || this.canvasTopTextBlock == null)
                return;
            this.canvansSubScript.Children.Clear();
            this.canvansSubScriptText.Children.Clear();
            this.canvasTop.Children.Clear();
            this.canvasTopTextBlock.Children.Clear();

            this.minDateTime = this.maxDateTime.AddMonths(-26);
            for (DateTime dt = this.minDateTime; dt <= this.maxDateTime; dt = dt.AddMonths(1))
            {
                this.canvansSubScriptText.Children.Add(this.CreateTextBlock(dt.Month.ToString()));
                Rectangle rect = this.CreateRectangle(4);
                rect.Tag = dt;
                this.canvansSubScript.Children.Add(rect);
                if (dt.Month == 1)
                {
                    rect = this.CreateRectangle(30);
                    this.canvasTop.Children.Add(rect);
                    this.canvasTopTextBlock.Children.Add(this.CreateTextBlock(dt.Year.ToString(), 16));
                }
            }

            if (this.canvasTop.Children.Count <= 2)
            {
                Rectangle rect = this.CreateRectangle(30);
                this.canvasTop.Children.Insert(0, rect);
                this.canvasTopTextBlock.Children.Insert(0, this.CreateTextBlock(this.minDateTime.Year.ToString(), 16));
            }
        }

        private void SetSubScriptPosition(double canvansSubScriptWidth)
        {
            if (this.canvansSubScript != null && canvansSubScriptWidth > 0)
            {
                if (this.regBarBg != null)
                {
                    this.regBarBg.Width = canvansSubScriptWidth;
                }

                switch (this.silverSelectCycle)
                {
                    case SilverSlecetCycleEnum.Date_Time:
                        this.SetPositionWithDateTime(canvansSubScriptWidth);
                        break;
                    case SilverSlecetCycleEnum.Day:
                        this.SetPositionWithDay(canvansSubScriptWidth);
                        break;
                    case SilverSlecetCycleEnum.Month:
                        this.SetPositionWithMonth(canvansSubScriptWidth);
                        break;
                    default:
                        this.SetPositionWithDateTime(canvansSubScriptWidth);
                        break;
                }
            }
        }

        private void SetPositionWithDateTime(double canvansSubScriptWidth)
        {
            TimeSpan subDateTime = this.maxDateTime.Subtract(this.minDateTime);
            double minutesCount = subDateTime.TotalMinutes;
            canvansSubScriptWidth = canvansSubScriptWidth - this.borBarMagin * 2;
            this.oneMinutesStep = canvansSubScriptWidth / minutesCount;

            //this.interval = canvansSubScriptWidth / (minutesCount / this.silverStep);
            int index = 0;
            int textCount = this.canvansSubScriptText.Children.Count;
            foreach (Rectangle rect in this.canvansSubScript.Children)
            {
                DateTime temp = DateTime.Parse(rect.Tag.ToString());
                subDateTime = temp.Subtract(this.minDateTime);

                double tempRectLeft = this.borBarMagin + (oneMinutesStep * subDateTime.TotalMinutes);
                Canvas.SetTop(rect, 15);
                Canvas.SetLeft(rect, tempRectLeft);

                if (temp.ToString("mm:ss") == "00:00")
                {
                    if (index < textCount)
                    {
                        double sub = 7;
                        TextBlock tb = this.canvansSubScriptText.Children[index] as TextBlock;
                        if (tb.Text.Length == 1)
                        {
                            sub = 3.5;
                        }
                        Canvas.SetLeft(tb, tempRectLeft - sub);
                        index++;
                    }
                }
            }

            if (this.startDateTime < this.minDateTime)
            {
                this.startDateTime = this.minDateTime;
            }

            if (this.endDateTime > this.maxDateTime)
            {
                this.endDateTime = this.maxDateTime;
            }

            subDateTime = this.startDateTime.Subtract(this.minDateTime);
            minutesCount = subDateTime.TotalMinutes;
            Canvas.SetLeft(this.silverPointerStart, oneMinutesStep * minutesCount);

            subDateTime = this.endDateTime.Subtract(this.minDateTime);
            minutesCount = subDateTime.TotalMinutes;
            Canvas.SetLeft(this.silverPointerEnd, oneMinutesStep * minutesCount);

            SetMouseUpNearPointer(this.silverPointerStart, ChangedPositionEnum.Handle);
        }

        private void SetPositionWithMonth(double canvansSubScriptWidth)
        {
            double monthCount = (this.maxDateTime.Year * 12 + this.maxDateTime.Month) - (this.minDateTime.Year * 12 + this.minDateTime.Month);
            canvansSubScriptWidth = canvansSubScriptWidth - this.borBarMagin * 2;
            this.oneMinutesStep = canvansSubScriptWidth / monthCount;

            //this.interval = canvansSubScriptWidth / monthCount;
            int index = 0;
            int topIndex = 0;
            int textCount = this.canvansSubScriptText.Children.Count;
            int rectCount = this.canvasTop.Children.Count;

            if (this.minDateTime.Month == 12)
            {
                this.canvasTop.Children[0].Visibility = Visibility.Visible;
                this.canvasTopTextBlock.Children[0].Visibility = Visibility.Visible;
                this.canvasTopTextBlock.Children[2].Visibility = Visibility.Visible;
            }
            else if (this.minDateTime.Month == 11)
            {
                this.canvasTop.Children[0].Visibility = Visibility.Visible;
                this.canvasTopTextBlock.Children[0].Visibility = Visibility.Visible;
                this.canvasTopTextBlock.Children[2].Visibility = Visibility.Collapsed;
            }
            else if (this.minDateTime.Month == 1)
            {
                this.canvasTop.Children[0].Visibility = Visibility.Collapsed;
                this.canvasTopTextBlock.Children[0].Visibility = Visibility.Visible;
                this.canvasTopTextBlock.Children[2].Visibility = Visibility.Visible;
            }
            else
            {
                topIndex = 1;
                this.canvasTop.Children[0].Visibility = Visibility.Collapsed;
                this.canvasTopTextBlock.Children[0].Visibility = Visibility.Visible;
                this.canvasTopTextBlock.Children[2].Visibility = Visibility.Visible;
                Canvas.SetTop(this.canvasTopTextBlock.Children[0], 6);
                Canvas.SetLeft(this.canvasTopTextBlock.Children[0], 0);
            }

            foreach (Rectangle rect in this.canvansSubScript.Children)
            {
                DateTime temp = DateTime.Parse(rect.Tag.ToString());

                double tempRectLeft = this.borBarMagin + (oneMinutesStep * index);
                Canvas.SetTop(rect, 15);
                Canvas.SetLeft(rect, tempRectLeft);

                if (temp.Month == 1)
                {
                    if (topIndex < rectCount)
                    {
                        Canvas.SetTop(this.canvasTop.Children[topIndex], 6);
                        Canvas.SetLeft(this.canvasTop.Children[topIndex], tempRectLeft - (this.oneMinutesStep / 2));

                        Canvas.SetTop(this.canvasTopTextBlock.Children[topIndex], 6);
                        Canvas.SetLeft(this.canvasTopTextBlock.Children[topIndex], tempRectLeft - (this.oneMinutesStep / 2) + 4);

                        topIndex++;
                    }
                }

                if (index < textCount)
                {
                    double sub = 7;
                    TextBlock tb = this.canvansSubScriptText.Children[index] as TextBlock;
                    if (tb.Text.Length == 1)
                    {
                        sub = 3.5;
                    }
                    Canvas.SetLeft(tb, tempRectLeft - sub);
                }
                index++;
            }

            if (this.startDateTime < this.minDateTime)
            {
                this.startDateTime = this.minDateTime;
            }

            if (this.endDateTime > this.maxDateTime)
            {
                this.endDateTime = this.maxDateTime;
            }

            monthCount = (this.startDateTime.Year * 12 + this.startDateTime.Month) - (this.minDateTime.Year * 12 + this.minDateTime.Month);
            Canvas.SetLeft(this.silverPointerStart, oneMinutesStep * monthCount);

            monthCount = (this.endDateTime.Year * 12 + this.endDateTime.Month) - (this.minDateTime.Year * 12 + this.minDateTime.Month);
            Canvas.SetLeft(this.silverPointerEnd, oneMinutesStep * monthCount);

            SetMouseUpNearPointer(this.silverPointerStart, ChangedPositionEnum.Handle);
        }

        private void SetPositionWithDay(double canvansSubScriptWidth)
        {
            double dayCount = Math.Ceiling(this.maxDateTime.Subtract(this.minDateTime).TotalDays);
            canvansSubScriptWidth = canvansSubScriptWidth - this.borBarMagin * 2;
            this.oneMinutesStep = canvansSubScriptWidth / dayCount;

            //this.interval = canvansSubScriptWidth / dayCount;
            int index = 0;
            int topIndex = 0;
            int topTextIndex = 0;
            int day1Count = 0;
            int textCount = this.canvansSubScriptText.Children.Count;
            int rectCount = this.canvasTop.Children.Count;
            int textTopCount = this.canvasTopTextBlock.Children.Count;

            (this.canvasTopTextBlock.Children[0] as TextBlock).Text = this.minDateTime.ToString("yyyy年MM月");
            Canvas.SetTop(this.canvasTopTextBlock.Children[0], 6);
            Canvas.SetLeft(this.canvasTopTextBlock.Children[0], 0);

            if (this.minDateTime.Day > 1)
            {
                topTextIndex = 1;
            }

            foreach (Rectangle rect in this.canvansSubScript.Children)
            {
                DateTime temp = DateTime.Parse(rect.Tag.ToString());

                double tempRectLeft = this.borBarMagin + (oneMinutesStep * index);
                Canvas.SetTop(rect, 15);
                Canvas.SetLeft(rect, tempRectLeft);

                if (temp.Day == 1)
                {
                    day1Count++;
                    if (topIndex < rectCount)
                    {
                        Canvas.SetTop(this.canvasTop.Children[topIndex], 6);
                        Canvas.SetLeft(this.canvasTop.Children[topIndex], tempRectLeft - (this.oneMinutesStep / 2));

                        topIndex++;
                    }
                    if (topTextIndex < textTopCount)
                    {
                        Canvas.SetTop(this.canvasTopTextBlock.Children[topTextIndex], 6);
                        Canvas.SetLeft(this.canvasTopTextBlock.Children[topTextIndex], tempRectLeft - (this.oneMinutesStep / 2) + 4);
                        topTextIndex++;
                    }
                }

                if (index < textCount)
                {
                    double sub = 7;
                    TextBlock tb = this.canvansSubScriptText.Children[index] as TextBlock;
                    if (tb.Text.Length == 1)
                    {
                        sub = 3.5;
                    }
                    Canvas.SetLeft(tb, tempRectLeft - sub);
                }
                index++;
            }

            int subMinCount = new DateTime(this.minDateTime.AddMonths(1).Year, this.minDateTime.AddMonths(1).Month, 1).Subtract(this.minDateTime).Days;
            if (day1Count == 1)
            {
                this.canvasTop.Children[1].Visibility = Visibility.Collapsed;
                this.canvasTop.Children[0].Visibility = Visibility.Visible;
                this.canvasTopTextBlock.Children[1].Visibility = Visibility.Visible;

                if (subMinCount < 5)
                {
                    this.canvasTopTextBlock.Children[0].Visibility = Visibility.Collapsed;
                    this.canvasTopTextBlock.Children[2].Visibility = Visibility.Visible;
                }
                else
                {
                    this.canvasTopTextBlock.Children[0].Visibility = Visibility.Visible;
                    this.canvasTopTextBlock.Children[2].Visibility = Visibility.Collapsed;
                }
            }
            else
            {
                this.canvasTop.Children[1].Visibility = Visibility.Visible;
                if (this.maxDateTime.Day < 5)
                {
                    this.canvasTopTextBlock.Children[2].Visibility = Visibility.Collapsed;
                }
                else
                {
                    this.canvasTopTextBlock.Children[2].Visibility = Visibility.Visible;
                }

                if (this.minDateTime.Day == 1)
                {
                    this.canvasTop.Children[0].Visibility = Visibility.Collapsed;
                    this.canvasTopTextBlock.Children[0].Visibility = Visibility.Visible;
                    this.canvasTopTextBlock.Children[2].Visibility = Visibility.Collapsed;
                }
                else
                {
                    this.canvasTop.Children[0].Visibility = Visibility.Visible;


                    if (subMinCount < 5)
                    {
                        this.canvasTopTextBlock.Children[0].Visibility = Visibility.Collapsed;
                    }
                    else
                    {
                        this.canvasTopTextBlock.Children[0].Visibility = Visibility.Visible;
                    }
                }
            }

            if (this.startDateTime < this.minDateTime)
            {
                this.startDateTime = this.minDateTime;
            }

            if (this.endDateTime > this.maxDateTime)
            {
                this.endDateTime = this.maxDateTime;
            }

            dayCount = Math.Ceiling(this.startDateTime.Subtract(this.minDateTime).TotalDays);
            Canvas.SetLeft(this.silverPointerStart, oneMinutesStep * dayCount);

            dayCount = Math.Ceiling(this.endDateTime.Subtract(this.minDateTime).TotalDays);
            Canvas.SetLeft(this.silverPointerEnd, oneMinutesStep * dayCount);

            SetMouseUpNearPointer(this.silverPointerStart, ChangedPositionEnum.Handle);
        }

        private void SetMouseUpNearPointer(SilverPointer silverPointer, ChangedPositionEnum changedPosition)
        {
            double tempSilverStartLeft = Canvas.GetLeft(silverPointer) + this.borBarMagin;
            if (this.canvansSubScript != null)
            {
                Rectangle absMinRect = null;
                foreach (Rectangle rect in this.canvansSubScript.Children)
                {
                    double tempRectLeft = Canvas.GetLeft(rect);
                    if (absMinRect == null)
                    {
                        absMinRect = rect;
                    }
                    else
                    {
                        if (Math.Abs(tempSilverStartLeft - tempRectLeft) < Math.Abs(tempSilverStartLeft - Canvas.GetLeft(absMinRect)))
                        {
                            absMinRect = rect;
                        }
                    }
                }

                if (absMinRect != null)
                {
                    tempSilverStartLeft = Canvas.GetLeft(absMinRect);
                    DateTime tempTime = DateTime.Parse(absMinRect.Tag.ToString());
                    switch (changedPosition)
                    {
                        case ChangedPositionEnum.Start:
                            Canvas.SetLeft(silverPointer, tempSilverStartLeft - this.borBarMagin);
                            Canvas.SetLeft(this.borBarBetween, tempSilverStartLeft);
                            this.startDateTime = tempTime;
                            break;
                        case ChangedPositionEnum.Handle:
                            Canvas.SetLeft(silverPointer, tempSilverStartLeft - this.borBarMagin);
                            Canvas.SetLeft(this.borBarBetween, tempSilverStartLeft);
                            this.startDateTime = tempTime;
                            this.SetMouseUpNearPointer(this.silverPointerEnd, ChangedPositionEnum.End);
                            break;
                        case ChangedPositionEnum.End:
                            this.endDateTime = tempTime;
                            Canvas.SetLeft(silverPointer, tempSilverStartLeft - this.borBarMagin);
                            break;
                        default:
                            Canvas.SetLeft(silverPointer, tempSilverStartLeft - this.borBarMagin);
                            Canvas.SetLeft(this.borBarBetween, tempSilverStartLeft);
                            this.startDateTime = tempTime;
                            break;
                    }
                    this.borBarBetween.Width = Canvas.GetLeft(this.silverPointerEnd) - Canvas.GetLeft(this.silverPointerStart);

                    if (this.isFirstLoaded)
                    {
                        this.isFirstLoaded = false;

                        this.silverPointerStart.ToolTipText = this.startDateTime.ToString(this.dateFormat);
                        this.silverPointerEnd.ToolTipText = this.endDateTime.ToString(this.dateFormat);
                    }
                    if (this.isSelfArrange)
                    {
                        this.isSelfArrange = false;
                        this.silverPointerStart.ToolTipText = this.startDateTime.ToString(this.dateFormat);
                        this.silverPointerEnd.ToolTipText = this.endDateTime.ToString(this.dateFormat);
                        this.DateTimeCallBack();
                    }
                }
            }
        }

        private void DateTimeCallBack()
        {
            if (this.BetweenDateTimeChanged != null)
            {
                BetweenSilverEventArgs bsArg = new BetweenSilverEventArgs();
                bsArg.StartDateTime = this.startDateTime;
                bsArg.EndDateTime = this.endDateTime;
                this.BetweenDateTimeChanged(this, bsArg);
            }
        }

        private DateTime GetSubDateTime(DateTime dateTime, int tempSubCount)
        {
            DateTime tempTime;
            if (this.silverSelectCycle == SilverSlecetCycleEnum.Month)
            {
                tempTime = dateTime.AddMonths(tempSubCount);
            }
            else if (this.silverSelectCycle == SilverSlecetCycleEnum.Day)
            {
                tempTime = dateTime.AddDays(tempSubCount);
            }
            else
            {
                tempTime = dateTime.AddMinutes(tempSubCount);
            }

            return tempTime;
        }

        private void MoveDateTimeForMonth(int num)
        {
            this.minDateTime = this.minDateTime.AddMonths(num);
            this.maxDateTime = this.maxDateTime.AddMonths(num);
            this.startDateTime = this.startDateTime.AddMonths(num);
            this.endDateTime = this.endDateTime.AddMonths(num);

            int index = 0;
            int topIndex = 0;
            int rectCount = this.canvasTop.Children.Count;

            if (this.minDateTime.Month == 12)
            {
                this.canvasTop.Children[0].Visibility = Visibility.Visible;
                this.canvasTopTextBlock.Children[0].Visibility = Visibility.Visible;
                this.canvasTopTextBlock.Children[2].Visibility = Visibility.Visible;
            }
            else if (this.minDateTime.Month == 11)
            {
                this.canvasTop.Children[0].Visibility = Visibility.Visible;
                this.canvasTopTextBlock.Children[0].Visibility = Visibility.Visible;
                this.canvasTopTextBlock.Children[2].Visibility = Visibility.Collapsed;
            }
            else if (this.minDateTime.Month == 1)
            {
                this.canvasTop.Children[0].Visibility = Visibility.Collapsed;
                this.canvasTopTextBlock.Children[0].Visibility = Visibility.Visible;
                this.canvasTopTextBlock.Children[2].Visibility = Visibility.Visible;
            }
            else
            {
                topIndex = 1;
                this.canvasTop.Children[0].Visibility = Visibility.Collapsed;
                this.canvasTopTextBlock.Children[0].Visibility = Visibility.Visible;
                this.canvasTopTextBlock.Children[2].Visibility = Visibility.Visible;
                Canvas.SetLeft(this.canvasTopTextBlock.Children[0], 0);

                (this.canvasTopTextBlock.Children[0] as TextBlock).Text = this.minDateTime.Year.ToString();
            }

            for (DateTime dt = this.minDateTime; dt <= this.maxDateTime; dt = dt.AddMonths(1))
            {
                double tempRectLeft = this.borBarMagin + (oneMinutesStep * index);
                if (index < this.canvansSubScriptText.Children.Count)
                {
                    (this.canvansSubScriptText.Children[index] as TextBlock).Text = dt.Month.ToString();
                    (this.canvansSubScript.Children[index] as Rectangle).Tag = dt;
                }
                if (dt.Month == 1)
                {
                    Canvas.SetTop(this.canvasTop.Children[topIndex], 6);
                    Canvas.SetLeft(this.canvasTop.Children[topIndex], tempRectLeft - (this.oneMinutesStep / 2));

                    Canvas.SetTop(this.canvasTopTextBlock.Children[topIndex], 6);
                    Canvas.SetLeft(this.canvasTopTextBlock.Children[topIndex], tempRectLeft - (this.oneMinutesStep / 2) + 4);
                    (this.canvasTopTextBlock.Children[topIndex] as TextBlock).Text = dt.Year.ToString();

                    topIndex++;
                }
                index++;
            }

            this.DateTimeCallBack();
        }

        private void MoveDateTimeForDay(int num)
        {
            this.minDateTime = this.minDateTime.AddDays(num);
            this.maxDateTime = this.maxDateTime.AddDays(num);
            this.startDateTime = this.startDateTime.AddDays(num);
            this.endDateTime = this.endDateTime.AddDays(num);

            int index = 0;
            int topIndex = 0;
            int topTextIndex = 0;
            int rectCount = this.canvasTop.Children.Count;

            (this.canvasTopTextBlock.Children[0] as TextBlock).Text = this.minDateTime.ToString("yyyy年MM月");
            if (this.minDateTime.Day > 1)
            {
                topTextIndex = 1;
            }

            int day1Count = 0;
            for (DateTime dt = this.minDateTime; dt <= this.maxDateTime; dt = dt.AddDays(1))
            {
                double tempRectLeft = this.borBarMagin + (oneMinutesStep * index);
                if (index < this.canvansSubScriptText.Children.Count)
                {
                    (this.canvansSubScriptText.Children[index] as TextBlock).Text = dt.Day.ToString();
                    (this.canvansSubScript.Children[index] as Rectangle).Tag = dt;
                }
                if (dt.Day == 1)
                {
                    day1Count++;

                    Canvas.SetTop(this.canvasTop.Children[topIndex], 6);
                    Canvas.SetLeft(this.canvasTop.Children[topIndex], tempRectLeft - (this.oneMinutesStep / 2));

                    Canvas.SetTop(this.canvasTopTextBlock.Children[topTextIndex], 6);
                    Canvas.SetLeft(this.canvasTopTextBlock.Children[topTextIndex], tempRectLeft - (this.oneMinutesStep / 2) + 4);
                    (this.canvasTopTextBlock.Children[topTextIndex] as TextBlock).Text = dt.ToString("yyyy年MM月");

                    topIndex++;
                    topTextIndex++;
                }
                index++;
            }

            int subMinCount = new DateTime(this.minDateTime.AddMonths(1).Year, this.minDateTime.AddMonths(1).Month, 1).Subtract(this.minDateTime).Days;
            if (day1Count == 1)
            {
                this.canvasTop.Children[1].Visibility = Visibility.Collapsed;
                this.canvasTop.Children[0].Visibility = Visibility.Visible;
                this.canvasTopTextBlock.Children[1].Visibility = Visibility.Visible;

                if (subMinCount < 5)
                {
                    this.canvasTopTextBlock.Children[0].Visibility = Visibility.Collapsed;
                    this.canvasTopTextBlock.Children[2].Visibility = Visibility.Visible;
                }
                else
                {
                    this.canvasTopTextBlock.Children[0].Visibility = Visibility.Visible;
                    this.canvasTopTextBlock.Children[2].Visibility = Visibility.Collapsed;
                }
            }
            else
            {
                this.canvasTop.Children[1].Visibility = Visibility.Visible;
                if (this.maxDateTime.Day < 5)
                {
                    this.canvasTopTextBlock.Children[2].Visibility = Visibility.Collapsed;
                }
                else
                {
                    this.canvasTopTextBlock.Children[2].Visibility = Visibility.Visible;
                }

                if (this.minDateTime.Day == 1)
                {
                    this.canvasTop.Children[0].Visibility = Visibility.Collapsed;
                    this.canvasTopTextBlock.Children[0].Visibility = Visibility.Visible;
                    this.canvasTopTextBlock.Children[2].Visibility = Visibility.Collapsed;
                }
                else
                {
                    this.canvasTop.Children[0].Visibility = Visibility.Visible;


                    if (subMinCount < 5)
                    {
                        this.canvasTopTextBlock.Children[0].Visibility = Visibility.Collapsed;
                    }
                    else
                    {
                        this.canvasTopTextBlock.Children[0].Visibility = Visibility.Visible;
                    }

                }
            }

            this.DateTimeCallBack();
        }
        #endregion

        #region 内部类
        enum ChangedPositionEnum
        {
            Start,
            Handle,
            End
        }
        #endregion
    }

    public enum SilverSlecetCycleEnum
    {
        /// <summary>
        /// 时间
        /// </summary>
        Date_Time,
        /// <summary>
        /// 日
        /// </summary>
        Day,
        /// <summary>
        /// 月
        /// </summary>
        Month,
    }

    public class BetweenSilverEventArgs : EventArgs
    {
        public DateTime StartDateTime { get; set; }

        public DateTime EndDateTime { get; set; }
    }

    public class SilverPointer : ContentControl
    {
        private TextBlock txtTip = null;
        private string toolTipText = "";
        private Visibility toolTipVisility = Visibility.Visible;

        public string ToolTipText
        {
            get
            {
                return this.toolTipText;
            }
            set
            {
                this.toolTipText = value;
                if (this.txtTip != null)
                {
                    this.txtTip.Text = value;
                }
            }
        }

        public Visibility ToolTipVisibility
        {
            get
            {
                return this.toolTipVisility;
            }
            set
            {
                this.toolTipVisility = value;
                if (this.txtTip != null)
                {
                    this.txtTip.Visibility = value;
                }
            }
        }

        public SilverPointer()
        {
            this.DefaultStyleKey = typeof(SilverPointer);
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            this.txtTip = this.GetTemplateChild("txtTip") as TextBlock;
            if (this.txtTip != null)
            {
                this.txtTip.Text = this.toolTipText;
                this.txtTip.Visibility = this.toolTipVisility;
            }
        }
    }
}
