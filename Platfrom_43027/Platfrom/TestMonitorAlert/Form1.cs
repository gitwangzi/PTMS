using Gsafety.PTMS.Message.Contract.Data;
using Gsafety.PTMS.MonitorAlert;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace TestMonitorAlert
{
    public partial class Form1 : Form
    {
        private MonitorAlertGenerator _monitorAlertGenerator = null;
        public Form1()
        {
            InitializeComponent();
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //_monitorAlertGenerator = new MonitorAlertGenerator();
            MonitorAlertMessage.Start();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string gpsmessage = "$GPRMC,070311.00,A,250.8734,S,7902.2353,W,200.0,0.0,250913,5.9,E,N*2E,012605000046302";

            System.Diagnostics.Stopwatch stopwatch = new System.Diagnostics.Stopwatch();
            stopwatch.Start();
            for (int i = 0; i < 100000; i++)
            {
                PTMSGPS antgps = new PTMSGPS(gpsmessage, 8);
                _monitorAlertGenerator.HandleAntGPS(antgps);
            }
            stopwatch.Stop();

            MessageBox.Show((stopwatch.ElapsedMilliseconds).ToString());
        }

        //测试编号2
        private void routeSendMessage()
        {
            string gpsmessage = "$GPRMC,070311.00,A,112.183289,S,7851.212808,W,0.0,0.0,250913,5.9,E,N*2E,012605000046302";
            string gpsmessageOut = "$GPRMC,070311.00,A,112.183289,S,7851.212808,E,0.0,0.0,250913,5.9,E,N*2E,012605000046302";

            System.Diagnostics.Stopwatch stopwatch = new System.Diagnostics.Stopwatch();
            stopwatch.Start();
            for (int i = 0; i < 50000; i++)
            {
                PTMSGPS antgps = new PTMSGPS(gpsmessage, 8);
                _monitorAlertGenerator.HandleAntGPS(antgps);

                antgps = new PTMSGPS(gpsmessageOut, 8);
                _monitorAlertGenerator.HandleAntGPS(antgps);
            }
            stopwatch.Stop();

            MessageBox.Show((stopwatch.ElapsedMilliseconds).ToString());
        }
    }
}
