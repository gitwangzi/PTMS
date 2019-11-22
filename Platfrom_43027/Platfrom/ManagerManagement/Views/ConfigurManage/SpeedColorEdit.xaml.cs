using Gsafety.Common.Controls;
using Gsafety.PTMS.ServiceReference.RunMonitorGroupService;
using Gsafety.PTMS.Share;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Gsafety.Common.CommMessage;
using Gsafety.PTMS.Share;
using Jounce.Core.Event;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace Gsafety.Ant.Monitor.Views
{
    public partial class SpeedColorEdit : ChildWindow
    {
        public SpeedColorEdit()
        {
            InitializeComponent();
            Initialize_Colors();
            this.DataContext = this;
            this.MouseRightButtonDown += ChildWindow_MouseRightButtonDown;
        }
        /// <summary>
        /// 颜色
        /// </summary>
        private Color _fillcolor = Colors.Red;
        public Color FillColorParm
        {
            get { return _fillcolor; }
            set { _fillcolor = value; }
        }
        public ObservableCollection<PredefinedColor> PreDefinedColors = null;
        private void Initialize_Colors()
        {
            if (PreDefinedColors != null)
            {
                return;
            }
            PreDefinedColors = PredefinedColors.PredefinedColorCollection;

            FillColor.DataContext = PreDefinedColors;
            //默认红色
            if (PredefinedColors.PredefinedColorCollection.Count > 1)
            {
                FillColor.SelectedValue = PredefinedColors.PredefinedColorCollection[1];
            }
          
        }
        private SpeedColorData _EditSpeedColorData;
        public SpeedColorData EditSpeedColorData
        {
            get
            {
                return _EditSpeedColorData;
            }
            set
            {
                _EditSpeedColorData = value;
            }
        }

        public void Edit(SpeedColorData runSpeedColorData)
        {
            EditSpeedColorData = runSpeedColorData;


            FillColor.SelectedValue = PreDefinedColors.Where(x => x.Value == runSpeedColorData.FillColorParm).FirstOrDefault();
            
            Show();
        }

        

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            
            if ((FillColor.SelectedItem as PredefinedColor) == null)
            {
                MessageBoxHelper.ShowDialog(ApplicationContext.Instance.StringResourceReader.GetString("GIS_Notice"), ApplicationContext.Instance.StringResourceReader.GetString("GIS_Symbol_SelectColor"), MessageDialogButton.Ok);
                return;
            }           
            FillColorParm = (FillColor.SelectedItem as PredefinedColor).Value;
            EditSpeedColorData.FillColorParm = FillColorParm;
            if (EditSpeedColorData.MinSpeed!=null)
            {
                if (EditSpeedColorData.MinSpeed > 200 || EditSpeedColorData.MinSpeed < 0)
                {
                    MessageBoxHelper.ShowDialog(ApplicationContext.Instance.StringResourceReader.GetString("Tip"), ApplicationContext.Instance.StringResourceReader.GetString("TRAFFIC_InputValidMinSpeed"), MessageDialogButton.Ok);
                    return;
                }
            }
            if (EditSpeedColorData.MaxSpeed != null)
            {
                if (EditSpeedColorData.MaxSpeed > 200 || EditSpeedColorData.MaxSpeed < 1)
                {
                    MessageBoxHelper.ShowDialog(ApplicationContext.Instance.StringResourceReader.GetString("Tip"), ApplicationContext.Instance.StringResourceReader.GetString("TRAFFIC_InputValidMaxSpeed"), MessageDialogButton.Ok);
                    return;
                }
            }

            if (EditSpeedColorData.MaxSpeed <= EditSpeedColorData.MinSpeed)
            {
                MessageBoxHelper.ShowDialog(ApplicationContext.Instance.StringResourceReader.GetString("Tip"), ApplicationContext.Instance.StringResourceReader.GetString("TRAFFIC_SpeedMaxMinError"), MessageDialogButton.Ok);
                return;
            
            }
            this.DialogResult = true;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        private void ChildWindow_MouseRightButtonDown(object sender,System.Windows.Input.MouseButtonEventArgs e)
        {
            e.Handled = true;
        }
    }

    
}

public class SpeedColorData
{
    string _id;
    ///<summary>
    ///主键
    ///</summary>

    public string ID
    {
        get
        {
            return _id;
        }
        set
        {
            _id = value;
        }
    }


    int _minspeed;


    public int MinSpeed
    {
        get
        {
            return _minspeed;
        }
        set
        {
            _minspeed = value;
        }
    }


    int _maxspeed;


    public int MaxSpeed
    {
        get
        {
            return _maxspeed;
        }
        set
        {
            _maxspeed = value;
        }
    }

    Color _fillcolor = Colors.Red;

    public Color FillColorParm
    {
        get
        {
            return _fillcolor;
        }
        set
        {
            _fillcolor = value;
        }
    }


}

