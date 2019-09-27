using Gsafety.PTMS.Base.Entity;
/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: 961b004a-256f-4052-a1bb-9d78f0371b76      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: PC-LIW
/////                 Author: TEST(liw)
/////======================================================================
/////           Project Name: Gsafety.PTMS.Alarm.Entity
/////    Project Description:    
/////             Class Name: C70
/////          Class Version: v1.0.0.0
/////            Create Time: 2013/8/20 17:13:14
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2013/8/20 17:13:14
/////            Modified by:
/////   Modified Description: 
/////======================================================================
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gsafety.PTMS.Alarm.Entity
{
    /// <summary>
    /// 解除一键报警
    /// </summary>
    [Serializable]
    public class C70 : DownwardBase, IConvertModel
    {
        /// <summary>
        /// 转换为字符串
        /// 99dc[指令长度],[MDVR芯片号],[消息序列号],指令关键字,
        /// [指令发送时间],报警类型,自定义报警编号,停报时间,一键报警状态#
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public string ConvertToString(object obj)
        {
            var model = obj as C70;
            StringBuilder sb = new StringBuilder();
            sb.Append("99dcXXX,")  ////99dc[指令长度]
              .Append(model.DvId)  ////,[MDVR芯片号]
              .Append(",")
              .Append(model.MsgId) ////[消息序列号]
              .Append(",")
              .Append(model.Cmd)   ////指令关键字
              .Append(",")
              .Append(model.GpsTime.ToString("yyMMdd HHmmss")) ////指令发送时间
              .Append("02000000")  ////报警类型
              .Append(",")         ////自定义报警编号
              .Append(",")
              .Append("0")        ////停报时间
              .Append(",")
              .Append("0#");       ////一键报警状态
            return sb.ToString();
        }

        public object ConvertToModel(string str)
        {
            return null;
        }
    }
}
