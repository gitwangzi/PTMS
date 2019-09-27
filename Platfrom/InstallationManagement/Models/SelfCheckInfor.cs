/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: 8f43ef4c-5986-4a0a-8a52-f81d62df0cb0      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: GAOZITIAN-PC
/////                 Author: TEST(gaozt)
/////======================================================================
/////           Project Name: Gsafety.PTMS.Installation.Models
/////    Project Description:    
/////             Class Name: SelfCheckInfor1
/////          Class Version: v1.0.0.0
/////            Create Time: 2013/8/19 16:30:29
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2013/8/19 16:30:29
/////            Modified by:
/////   Modified Description: 
/////======================================================================
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
using Gsafety.PTMS.Share;

namespace Gsafety.PTMS.Installation.Models
{
    public class SelfCheckInfor
    {
        public string DeviceSN { get; set; }
        public string CheckTime { get; set; }
        public string RecHDD { get; set; }
        public string GPSINFO { get; set; }
        public string Sensor1 { get; set; }
        public string Sensor2 { get; set; }
        public string Sensor3 { get; set; }
        public string StandbyPower { get; set; }
        public string Module3G { get; set; }
        public string Channel1 { get; set; }
        public string Channel2 { get; set; }
        public string Channel3 { get; set; }
        public string Channel4 { get; set; }
        public string CurInTemperature { get; set; }
        public string SIM { get; set; }
        public string CurVoltage { get; set; }
        public string SdCapacity { get; set; }

        public string DeviceSN_Result { get; set; }
        public string CheckTime_Result { get; set; }
        public string RecHDD_Result { get; set; }
        public string GPSINFO_Result { get; set; }
        public string Sensor1_Result { get; set; }
        public string Sensor2_Result { get; set; }
        public string Sensor3_Result { get; set; }
        public string StandbyPower_Result { get; set; }
        public string Module3G_Result { get; set; }
        public string Channel1_Result { get; set; }
        public string Channel2_Result { get; set; }
        public string Channel3_Result { get; set; }
        public string Channel4_Result { get; set; }
        public string CurInTemperature_Result { get; set; }
        public string SIM_Result { get; set; }
        public string CurVoltage_Result { get; set; }
        public string SdCapacity_Result { get; set; }
        public bool IsSucc { get; set; }

        public bool IsError;
        public bool Check()
        {
            
            bool tem = false;
            tem = this.RecHDD == "E";

            this.IsSucc = tem;
            GetCheckIndex(tem);
            this.RecHDD_Result = tem ? ApplicationContext.Instance.StringResourceReader.GetString("ID_INSTALL_SelfCheck_Result_Success") : ApplicationContext.Instance.StringResourceReader.GetString("ID_INSTALL_SelfCheck_Result_Falid");

            tem = !(this.GPSINFO == "N" || this.GPSINFO == "V" || (this.GPSINFO ?? "").ToUpper().IndexOf("INVALID") > -1);
            this.GPSINFO_Result = tem ? ApplicationContext.Instance.StringResourceReader.GetString("ID_INSTALL_SelfCheck_Result_Success") : ApplicationContext.Instance.StringResourceReader.GetString("ID_INSTALL_SelfCheck_Result_Falid");
            this.IsSucc = this.IsSucc && tem;
            GetCheckIndex(tem);



            this.StandbyPower_Result = ApplicationContext.Instance.StringResourceReader.GetString("ID_INSTALL_SelfCheck_Ingore");

            tem = this.Module3G == "OK";
            this.Module3G_Result = tem ? ApplicationContext.Instance.StringResourceReader.GetString("ID_INSTALL_SelfCheck_Result_Success") : ApplicationContext.Instance.StringResourceReader.GetString("ID_INSTALL_SelfCheck_Result_Falid");
            this.IsSucc = this.IsSucc && tem;
            GetCheckIndex(tem);
            tem = this.Channel1 == "OK";
            this.Channel1_Result = tem ? ApplicationContext.Instance.StringResourceReader.GetString("ID_INSTALL_SelfCheck_Result_Success") : ApplicationContext.Instance.StringResourceReader.GetString("ID_INSTALL_SelfCheck_Result_Falid");
            this.IsSucc = this.IsSucc && tem;
            GetCheckIndex(tem);
            tem = this.Channel2 == "OK";
            this.Channel2_Result = tem ? ApplicationContext.Instance.StringResourceReader.GetString("ID_INSTALL_SelfCheck_Result_Success") : ApplicationContext.Instance.StringResourceReader.GetString("ID_INSTALL_SelfCheck_Result_Falid");
            this.IsSucc = this.IsSucc && tem;
            GetCheckIndex(tem);
            tem = this.Channel3 == "OK";
            this.Channel3_Result = tem ? ApplicationContext.Instance.StringResourceReader.GetString("ID_INSTALL_SelfCheck_Result_Success") : ApplicationContext.Instance.StringResourceReader.GetString("ID_INSTALL_SelfCheck_Result_Falid");
            this.IsSucc = this.IsSucc && tem;
            GetCheckIndex(tem);
            tem = this.Channel4 == "OK";
            this.Channel4_Result = tem ? ApplicationContext.Instance.StringResourceReader.GetString("ID_INSTALL_SelfCheck_Result_Success") : ApplicationContext.Instance.StringResourceReader.GetString("ID_INSTALL_SelfCheck_Result_Falid");
            this.IsSucc = this.IsSucc && tem;
            GetCheckIndex(tem);
            tem = this.Sensor1 == "OK";
            this.Sensor1_Result = tem ? ApplicationContext.Instance.StringResourceReader.GetString("ID_INSTALL_SelfCheck_Result_Success") : ApplicationContext.Instance.StringResourceReader.GetString("ID_INSTALL_SelfCheck_Result_Falid");
            this.IsSucc = this.IsSucc && tem;
            GetCheckIndex(tem);
            tem = this.Sensor2 == "OK";
            this.Sensor2_Result = tem ? ApplicationContext.Instance.StringResourceReader.GetString("ID_INSTALL_SelfCheck_Result_Success") : ApplicationContext.Instance.StringResourceReader.GetString("ID_INSTALL_SelfCheck_Result_Falid");
            this.IsSucc = this.IsSucc && tem;
            GetCheckIndex(tem);
            tem = this.Sensor3 == "OK";
            this.Sensor3_Result = tem ? ApplicationContext.Instance.StringResourceReader.GetString("ID_INSTALL_SelfCheck_Result_Success") : ApplicationContext.Instance.StringResourceReader.GetString("ID_INSTALL_SelfCheck_Result_Falid");
            this.IsSucc = this.IsSucc && tem;
            GetCheckIndex(tem);
            float numTem;
            tem = float.TryParse(this.CurInTemperature.Trim(), System.Globalization.NumberStyles.Any, System.Globalization.NumberFormatInfo.InvariantInfo, out numTem);
            tem = tem && numTem < 70 && numTem > -20;
            this.CurInTemperature_Result = tem ? ApplicationContext.Instance.StringResourceReader.GetString("ID_INSTALL_SelfCheck_Result_Success") : ApplicationContext.Instance.StringResourceReader.GetString("ID_INSTALL_SelfCheck_Result_Falid");
            this.IsSucc = this.IsSucc && tem;
            GetCheckIndex(tem);
            this.DeviceSN_Result = !string.IsNullOrWhiteSpace(this.DeviceSN) ? ApplicationContext.Instance.StringResourceReader.GetString("ID_INSTALL_SelfCheck_Result_Success") : ApplicationContext.Instance.StringResourceReader.GetString("ID_INSTALL_SelfCheck_Result_Falid");
            GetCheckIndex(tem);
            this.CheckTime_Result = !string.IsNullOrWhiteSpace(this.CheckTime) ? ApplicationContext.Instance.StringResourceReader.GetString("ID_INSTALL_SelfCheck_Result_Success") : ApplicationContext.Instance.StringResourceReader.GetString("ID_INSTALL_SelfCheck_Result_Falid");
            GetCheckIndex(tem);

            return IsError;
        }

        public void GetCheckIndex(bool isSuccess)
        {
            
            if (!isSuccess)
            {
                IsError = true;
            }

        }
    }

}
