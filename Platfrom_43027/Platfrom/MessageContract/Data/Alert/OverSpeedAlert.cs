/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: 886cba45-50f0-4315-85ce-d2a04c2f7424      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: PC-GUOH
/////                 Author: TEST(guoh)
/////======================================================================
/////           Project Name: Gsafety.PTMS.Alert.Contract.Data
/////    Project Description:    
/////             Class Name: OverSpeedAlert
/////          Class Version: v1.0.0.0
/////            Create Time: 8/26/2013 2:38:42 PM
/////      Class Description:  
/////======================================================================
/////          Modified Time: 8/26/2013 2:38:42 PM
/////            Modified by:
/////   Modified Description: 
/////======================================================================
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Gsafety.PTMS.Base.Contract.Data;
using Gsafety.Common.Util;


namespace Gsafety.PTMS.Message.Contract.Data
{
    /// <summary>
    /// OverSpeed Alert
    /// </summary>
    [Serializable]
    [DataContract]
    public class OverSpeedAlert : AlertBaseModel
    {

        /// <summary>
        /// Current Speed
        /// </summary>
        [DataMember]
        public string CurrentSpeed { get; set; }

        /// <summary>
        /// Min Speed
        /// </summary>
        [DataMember]
        public string MinSpeed { get; set; }

        /// <summary>
        /// Max Speed
        /// </summary>
        [DataMember]
        public string MaxSpeed { get; set; }

        /// <summary>
        /// Continue Time
        /// </summary>
        [DataMember]
        public string ContinueTime { get; set; }

        /// <summary>
        /// SpeedAlert Type。
        /// Including,0:The Standard Low Speed ;1:The Standard High Speed;2:The History Low Speed;3:History High Speed
        /// </summary>
        [DataMember]
        public Int16 SpeedAlertType { get; set; }

        /// <summary>
        /// History Min Speed
        /// </summary>
        [DataMember]
        public string HistoryMinSpeed { get; set; }

        /// <summary>
        /// History Max Speed
        /// </summary>
        [DataMember]
        public string HistoryMaxSpeed { get; set; }

        /// <summary>
        /// OverSpeed Alert
        /// </summary>
        /// <param name="str">Alert Original Cmd Context
        /// Cmd Format:99dc[Cmd Length],[MDVR Card ID],[Message ID],Cmd Key,[Location And State],Location And State When Happen,Alerm UID,Send Count ID,Current Voltage,Mix Voltage,Max Voltage#
        /// </param>
        public OverSpeedAlert(string str)
        {

            #region Location And State

            //
            //Location And State
            //
            //070729 220859,A,11329.9818,-2234.5670,120.98,15.80,0104003100000000,00050000000F0000,35.00,,0041C01F.E0C0,324,1,5,0,41
            //
            //Explain：
            //ID	Item	Comment
            //1	Cmd Send Time 070729 220859:2007 year 7 month 29 day 22 hour  8 mintue 59 second
            //2	Location State A:Location Is Reliable
            //3	Longitude 11329.9818:E113degree29.9818cent
            //4	Latitude -2234.5670:S22degree34.5670cent
            //5	Speed on Land 120.98:Speed 120.98Km/H
            //6	Direction on Land 15.80:Direction 15.80Degree
            //7	Unit Status and Alerm Status 0104003100000000:Has emergency alarm button,No Temperature Alerm,No Disk Alerm,No OverSpeed Alerm,Pressed the Brake,Middle Door Close,Back Door Close,Front Door Close,Upgoing,No Operation,No Exception of Open or Close Door Alerm,Channel1~2 Video Lose,Channel3~4 Video Close
            //8	Power Supply State and GPS Module State 00050000000F0000:Main Power Connecting Normal,Security Suite Voltage Normal,GPS Module Exist,Connect Right,Signal Strange
            //9	Security Suite Temperatury 35.00:Security Suite Temperatury is 35℃
            //10	Temperature In Vehicle  Null
            //11	IO Alert State and Web State 0041C01F.E0C0:IO port 7 and IO1 Exist Alerm,Dail Module Exist,Card Exist,Signal Strength(+)31,Security Suite In Fire Mode,WIFI Module Exist,Signal Strength(-)192
            //12	Route Number	324:Route Number is 324
            //13	Driver ID 1:Driver ID is 1
            //14	Next Station ID	5:Next Station is 5
            //15	Stop Flag	0:Stop In The Station
            //16	Number of People In Car	41:Number of People In Car is 41
            //
            //Comment:The Handle of Component State and Alerm Flag Should Take Mask Field Which equal 1,then Decide The Meaning of it(Alerm);Remove the part which equal 0 in Mask Field
            //

            #endregion

            try
            {
                if (!string.IsNullOrEmpty(str))
                {
                    //Remove Last '#' Sign From Cmd
                    str = str.Substring(0, str.LastIndexOf("#"));
                    //Spilt String
                    string[] alertArray = str.Split(',');
                    if (alertArray != null && alertArray.Length > 0)
                    {
                        if (alertArray[3].Trim().ToUpper().Equals("V70"))
                        {
                            this.ID = Guid.NewGuid().ToString();
                            //MdvrCore SN
                            this.MdvrCoreSN = alertArray[1].Trim();

                            //TODO:According MDVR Card ID ,Select Database Get Security Suite ID,Card ID,Security Suite State
                            //SuitInfoID
                            //this.SuitInfoID
                            //VehicleID
                            //this.VehicleID
                            //SuitStatus
                            //this.SuitStatus

                            ////--------------------------------------------------------------
                            ////TODO:Need The Upload Function , Note From Here
                            //Security Suite Time
                            //var time = ConvertHelper.ConvertStrToDate(alertArray[4], "yyMMdd HHmmss");
                            //if (time != null)
                            //{
                            //    //AlertTime
                            //    this.AlertTime = time.Value;
                            //    //GpsTime
                            //    this.GpsTime = time.Value;
                            //}
                            ////GpsValid
                            //this.GpsValid = alertArray[5];
                            ////Longitude
                            //this.Longitude = alertArray[6];
                            ////Latitude
                            //this.Latitude = alertArray[7];
                            ////Speed
                            //this.Speed = alertArray[8];
                            ////Direction
                            //this.Direction = alertArray[9];
                            ////--------------------------------------------------------------

                            ////--------------------------------------------------------------
                            //TODO:The Make-up Message
                            if (!string.IsNullOrEmpty(alertArray[20]))
                            {
                                //time in Security Suite
                                var time = ConvertHelper.ConvertStrToDate(alertArray[20], "yyMMdd HHmmss");
                                if (time != null)
                                {
                                    //AlertTime
                                    this.AlertTime = time.Value;
                                    //GpsTime
                                    this.GpsTime = time.Value;
                                }
                                //GpsValid
                                this.GpsValid = alertArray[21];
                                //Longitude
                                this.Longitude = alertArray[22];
                                //Latitude
                                this.Latitude = alertArray[23];
                                //Speed
                                this.Speed = alertArray[24];
                                //Direction
                                this.Direction = alertArray[25];
                            }
                            else
                            {
                                //time
                                var time = ConvertHelper.ConvertStrToDate(alertArray[4], "yyMMdd HHmmss");
                                if (time != null)
                                {
                                    //AlertTime
                                    this.AlertTime = time.Value;
                                    //GpsTime
                                    this.GpsTime = time.Value;
                                }
                                //GpsValid
                                this.GpsValid = alertArray[5];
                                //Longitude
                                this.Longitude = alertArray[6];
                                //Latitude
                                this.Latitude = alertArray[7];
                                //Speed
                                this.Speed = alertArray[8];
                                //Direction
                                this.Direction = alertArray[9];
                            }
                            //--------------------------------------------------------------

                            //Cmd
                            this.Cmd = "V70";
                            //Status
                            this.Status = 1;
                            //CreateTime
                            this.CreateTime = DateTime.Now;
                            //Context
                            this.Context = str;

                            //CurrentSpeed
                            this.CurrentSpeed = alertArray[38];
                            //MinSpeed
                            this.MinSpeed = alertArray[39];
                            //MaxSpeed
                            this.MaxSpeed = alertArray[40];
                            //ContinueTime
                            this.ContinueTime = alertArray[41];

                            //SpeedAlertType
                            this.SpeedAlertType = Convert.ToInt16(alertArray[43]);
                            //According Alert Category, Setting Alert Type
                            switch (this.SpeedAlertType)
                            {
                                case 0:
                                case 2:
                                    //When Alert Type Is Standard LowSpeed Alert And History LowSpeed Alert,Alert Type Is Unified Common LowSpeed Alert.
                                   // this.AlertType = (short)BusinessAlertType.UnderSpeed;
                                  //  break;
                                case 1:
                                case 3:
                                    //When Alert Type Is Standard OverSpeed Alert And History OverSpeed Alert,Alert Type Is Unified Common OverSpeed Alert.
                                //    this.AlertType = (short)BusinessAlertType.OverSpeed;
                               //     break;
                                default:
                                    //Default Is Common Overspeed Alert 
                                    this.AlertType = (short)BusinessAlertType.OverSpeed;
                                    break;
                            }

                            //History Min Speed
                            this.HistoryMinSpeed = alertArray[44];
                            //History Max Speed
                            this.HistoryMaxSpeed = alertArray[45];
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(DateTime.Now + "--Gsafety.Ant.Alert.Contract.Data.OverSpeedAlert.OverSpeedAlert(string str) --" + ex.ToString());
            }
        }

    }
}
