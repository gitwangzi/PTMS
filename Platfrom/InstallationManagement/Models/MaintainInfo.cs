/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: b25366ca-d7f5-4559-b71a-ddff3a7b505f      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: GAOZITIAN-PC
/////                 Author: TEST(gaozt)
/////======================================================================
/////           Project Name: Gsafety.PTMS.Installation.Models
/////    Project Description:    
/////             Class Name: MaintainInfo
/////          Class Version: v1.0.0.0
/////            Create Time: 2013/9/6 16:53:05
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2013/9/6 16:53:05
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

namespace Gsafety.PTMS.Installation.Models
{
    public class MaintainInfo
    {
        public string CarNumber { get; set; }
        public string SuiteId { get; set; }
        public string Maintainer { get; set; }
        public string Note { get; set; }

        public bool IsRepair1 { get; set; }
        public bool IsRepair2 { get; set; }
        public bool IsRepair3 { get; set; }
        public bool IsRepair4 { get; set; }
        public bool IsRepair5 { get; set; }
        public bool IsRepair6 { get; set; }
        public bool IsRepair7 { get; set; }
        public bool IsRepair8 { get; set; }
        public bool IsRepair9 { get; set; }
        public bool IsRepair10 { get; set; }

        public string OldCamera1 { get; set; }
        public string OldCamera2 { get; set; }
        public string OldAlarm1 { get; set; }
        public string OldAlarm2 { get; set; }
        public string OldAlarm3 { get; set; }
        public string OldUps { get; set; }
        public string OldSdCard { get; set; }
        public string OldOpenDoor { get; set; }
        public string OldMdvrSn { get; set; }
        public string OldMdvrCoreSn { get; set; }

        public string NewCamera1 { get; set; }
        public string NewCamera2 { get; set; }
        public string NewAlarm1 { get; set; }
        public string NewAlarm2 { get; set; }
        public string NewAlarm3 { get; set; }
        public string NewUps { get; set; }
        public string NewSdCard { get; set; }
        public string NewOpenDoor { get; set; }
        public string NewMdvrSn { get; set; }
        public string NewMdvrCoreSn { get; set; }

        public void isrepairinit()
        {
            if ((OldCamera1 != null) && (NewCamera1 != null))
            {
                if (OldCamera1.Equals(NewCamera1))
                {
                    IsRepair1 = false;
                }
                else
                {
                    IsRepair1 = true;
                }
            }
            if ((OldCamera2 != null) && (NewCamera2 != null))
            {
                if (OldCamera2.Equals(NewCamera2))
                {
                    IsRepair2 = false;
                }
                else
                {
                    IsRepair2 = true;
                }
            }
            if ((OldAlarm1 != null) && (NewAlarm1 != null))
            {
                if (OldAlarm1.Equals(NewAlarm1))
                {
                    IsRepair3 = false;
                }
                else
                {
                    IsRepair3 = true;
                }
            }
            if ((OldAlarm2 != null) && (NewAlarm2 != null))
            {
                if (OldAlarm2.Equals(NewAlarm2))
                {
                    IsRepair4 = false;
                }
                else
                {
                    IsRepair4 = true;
                }
            }
            if ((OldAlarm3 != null) && (NewAlarm3 != null))
            {
                if (OldAlarm3.Equals(NewAlarm3))
                {
                    IsRepair5 = false;
                }
                else
                {
                    IsRepair5 = true;
                }
            }
            if ((OldUps != null) && (NewUps != null))
            {
                if (OldUps.Equals(NewUps))
                {
                    IsRepair6 = false;
                }
                else
                {
                    IsRepair6 = true;
                }
            }
            if ((OldSdCard != null) && (NewSdCard != null))
            {
                if (OldSdCard.Equals(NewSdCard))
                {
                    IsRepair7 = false;
                }
                else
                {
                    IsRepair7 = true;
                }
            }
            if ((OldOpenDoor != null) && (NewOpenDoor != null))
            {
                if (OldOpenDoor.Equals(NewOpenDoor))
                {
                    IsRepair8 = false;
                }
                else
                {
                    IsRepair8 = true;
                }
            }
            if ((OldMdvrSn != null) && (NewMdvrSn != null))
            {
                if (OldMdvrSn.Equals(NewMdvrSn))
                {
                    IsRepair9 = false;
                }
                else
                {
                    IsRepair9 = true;
                }
            }
            if ((OldMdvrCoreSn != null) && (NewMdvrCoreSn != null))
            {
                if (OldMdvrCoreSn.Equals(NewMdvrCoreSn))
                {
                    IsRepair10 = false;
                }
                else
                {
                    IsRepair10 = true;
                }
            }
        }

    }
}
