/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: 4cd538e7-c55e-4399-ac29-8df8d224b95b      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: GAOZITIAN-PC
/////                 Author: TEST(gaozt)
/////======================================================================
/////           Project Name: Gsafety.PTMS.Maintain.Models
/////    Project Description:    
/////             Class Name: DeviceAlertType
/////          Class Version: v1.0.0.0
/////            Create Time: 2013/9/12 16:23:58
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2013/9/12 16:23:58
/////            Modified by:
/////   Modified Description: 
/////======================================================================
using System;
using System.Net;
using System.Windows;
using System.Windows.Input;

namespace Gsafety.Ant.Maintain.Models
{
    public enum DeviceAlertType
    {
        ////超温报警
        OverTemperature = 11,
        ////GPS接收机故障
        GpsFault = 12,
        ////摄像头遮挡
        VedioShelter =13,
        ////摄像头无信号
        VedioNoSignal = 14,
        ////非法点火报警
        AbnormalFire = 15,
        ////MDVR存储卡错误报警
        SdFault = 16,
        ////三次密码错误报警
        PasswordFault = 17,
        ////电压异常报警
        AbnormalValtage = 18,
        ////设备72小时未上线
        Offline72 = 19,
        ////意外损坏
        Damage = 20
    }
}
