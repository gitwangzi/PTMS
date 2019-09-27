/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: b08673ec-206e-48a7-8ca1-8f91a8eb269d      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: PC-GUOH
/////                 Author: TEST(guoh)
/////======================================================================
/////           Project Name: Gsafety.PTMS.Message.Contract.Data.Alert
/////    Project Description:    
/////             Class Name: RemoveDeviceSuiteAlertNotify
/////          Class Version: v1.0.0.0
/////            Create Time: 8/29/2013 2:05:42 PM
/////      Class Description:  
/////======================================================================
/////          Modified Time: 8/29/2013 2:05:42 PM
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
    /// Remove Device Suite Alert Notify
    /// </summary>
    [Serializable]
    [DataContract]
    public class RemoveDeviceSuiteAlertNotify : AlertBaseModel
    {
        /// <summary>
        /// remove Business Alert Type
        /// </summary>
        [DataMember]
        public string removeBusinessAlertType { get; set; }

        /// <summary>
        /// remove Device Alert Type
        /// </summary>
        [DataMember]
        public string removeDeviceAlertType { get; set; }

        /// <summary>
        /// remove User Defined Alert Type
        /// </summary>
        [DataMember]
        public string removeUserDefinedAlertType { get; set; }

        /// <summary>
        /// User Defined Alert Id
        /// </summary>
        [DataMember]
        public string UserDefinedAlertId { get; set; }

        /// <summary>
        /// User Defined Alert Name
        /// </summary>
        [DataMember]
        public string UserDefinedAlertName { get; set; }

        /// <summary>
        /// Removed Alarm Flag
        /// </summary>
        [DataMember]
        public bool RemovedAlarmFlag { get; set; }

        /// <summary>
        /// Remove Device Suite Alert Notify
        /// </summary>
        /// <param name="str">Alert Original Cmd Context
        /// Cmd Format:99dc[Cmd Length],[MDVR Card ID],[Message ID],Cmd Key,[Location And State],Location And State When Happen,Alerm UID,Send Count ID,Current Voltage,Mix Voltage,Max Voltage#
        /// </param>
        public RemoveDeviceSuiteAlertNotify(string str)
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
                        if (alertArray[3].Trim().ToUpper().Equals("V77"))
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
                                //time in Security Suite
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
                            this.Cmd = "V77";
                            //Status
                            this.Status = 1;
                            //CreateTime
                            this.CreateTime = DateTime.Now;
                            //Context
                            this.Context = str;

                            //
                            //TODO：According Alert Type Get Remove The Business Alert Type,Device Alert Type,Define Alert Type
                            //
                            this.GetRemoveAlertTypeInfo(alertArray);

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(DateTime.Now + "--Gsafety.Ant.Alert.Contract.Data.RemoveDeviceSuiteAlertNotify.RemoveDeviceSuiteAlertNotify(string str) --" + ex.ToString());
            }
        }

        /// <summary>
        /// According Alert Type Get Remove The Business Alert Type,Device Alert Type,Define Alert Type
        /// </summary>
        /// <param name="alertArray">Alert Array</param>
        private void GetRemoveAlertTypeInfo(string[] alertArray)
        {
            try
            {
                //Get Alert Type
                string tempHex = alertArray[20];
                if (tempHex != null && tempHex.Length > 0)
                {
                    //Sixteen Hexadecimal Conversion to Binary
                    string binaryString = ConvertHelper.ConvertHexadecimalToBinary(tempHex);
                    if (binaryString != null & binaryString.Length > 0)
                    {
                        char[] chars = binaryString.ToCharArray();
                        if (chars != null && chars.Length > 0)
                        {
                            for (int i = 0; i < chars.Length; i++)
                            {
                                string bitString = chars[i].ToString();
                                // 1 is Remove Alert Type
                                if (bitString == "1")
                                {
                                    switch (i)
                                    {
                                        //Illegal Fire Alert
                                        case 0:
                                            this.removeDeviceAlertType += (short)DeviceAlertType.AbnormalFire + ",";
                                            break;
                                        //Three Times Password Alert
                                        case 1:
                                            this.removeDeviceAlertType += (short)DeviceAlertType.PasswordFault + ",";
                                            break;
                                        //Illegal Open Close Door Alert 
                                        case 2:
                                            this.removeBusinessAlertType += (short)BusinessAlertType.AbnormalDoor + ",";
                                            break;
                                        //Camera Block Alert 
                                        case 3:
                                            this.removeDeviceAlertType += (short)DeviceAlertType.VedioShelter + ",";
                                            break;
                                        //Camera No Signal Alert 
                                        case 4:
                                            this.removeDeviceAlertType += (short)DeviceAlertType.VedioNoSignal + ",";
                                            break;
                                        //Shake Alert 
                                        case 5:
                                            break;
                                        //Emergency Button Alert	
                                        case 6:
                                            RemovedAlarmFlag = true;
                                            break;
                                        //Define Alert
                                        case 7:
                                            this.removeUserDefinedAlertType = "UserDefinedAlert";
                                            break;
                                        //Voltage Exception Alert	
                                        case 8:
                                            this.removeDeviceAlertType += (short)DeviceAlertType.AbnormalValtage + ",";
                                            break;
                                        //Acceleration Alert
                                        case 9:
                                            break;
                                        //GPS Receive Error Alert
                                        case 10:
                                            this.removeDeviceAlertType += (short)DeviceAlertType.GpsFault + ",";
                                            break;
                                        //Open Close Error Alert
                                        case 11:
                                            this.removeBusinessAlertType += (short)BusinessAlertType.AbnormalDoor + ",";
                                            break;
                                        //Cross Border Alert
                                        case 12:
                                            this.removeBusinessAlertType += (short)BusinessAlertType.OutFence + ",";
                                            break;
                                        //Overspeed Alert
                                        case 13:
                                            this.removeBusinessAlertType += (short)BusinessAlertType.OverSpeed + ",";
                                            break;
                                        //Disk Error Alert
                                        case 14:
                                            this.removeDeviceAlertType += (short)DeviceAlertType.SdFault + ",";
                                            break;
                                        //Temperature Alert
                                        case 15:
                                            this.removeDeviceAlertType += (short)DeviceAlertType.OverTemperature + ",";
                                            break;
                                        //keep
                                        default:
                                            break;
                                    }
                                }
                            }
                        }
                    }
                }
                // removeUserDefinedAlertType
                if (this.removeBusinessAlertType != null && this.removeBusinessAlertType.Length > 0)
                {
                    this.removeBusinessAlertType = this.removeBusinessAlertType.Substring(0, this.removeBusinessAlertType.Length - 1);
                }
                // removeUserDefinedAlertType
                if (this.removeDeviceAlertType != null && this.removeDeviceAlertType.Length > 0)
                {
                    this.removeDeviceAlertType = this.removeDeviceAlertType.Substring(0, this.removeDeviceAlertType.Length - 1);
                }
                // removeUserDefinedAlertType
                if (this.removeUserDefinedAlertType != null && this.removeUserDefinedAlertType.Length > 0)
                {
                    // UserDefinedAlertId
                    this.UserDefinedAlertId = alertArray[21];
                    // UserDefinedAlertName
                    this.UserDefinedAlertName = alertArray[22];
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(DateTime.Now + "--Gsafety.Ant.Alert.Contract.Data.RemoveDeviceSuiteAlertNotify.GetRemoveAlertTypeInfo(string str) --" + ex.ToString());
            }
        }
        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            if (!string.IsNullOrEmpty(Convert.ToString(removeBusinessAlertType)))
            {
                builder.AppendLine("removeBusinessAlertType:" + removeBusinessAlertType.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(removeDeviceAlertType)))
            {
                builder.AppendLine("removeDeviceAlertType:" + removeDeviceAlertType.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(removeUserDefinedAlertType)))
            {
                builder.AppendLine("removeUserDefinedAlertType:" + removeUserDefinedAlertType.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(UserDefinedAlertId)))
            {
                builder.AppendLine("UserDefinedAlertId:" + UserDefinedAlertId.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(UserDefinedAlertName)))
            {
                builder.AppendLine("UserDefinedAlertName:" + UserDefinedAlertName.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(RemovedAlarmFlag)))
            {
                builder.AppendLine("RemovedAlarmFlag:" + RemovedAlarmFlag.ToString());
            }
            return builder.ToString();
        }

    }
}
