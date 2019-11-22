using Gsafety.Common.Util;
/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: 35cda14e-66f2-480d-bf52-64627b2dbea0      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: PC-LIW
/////                 Author: TEST(liw)
/////======================================================================
/////           Project Name: Gsafety.PTMS.Monitor.Contract.Data
/////    Project Description:    
/////             Class Name: TransparentCMD
/////          Class Version: v1.0.0.0
/////            Create Time: 2013/8/26 11:03:26
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2013/8/26 11:03:26
/////            Modified by:
/////   Modified Description: 
/////======================================================================
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Gsafety.PTMS.Message.Contract.Data
{
    /// <summary>
    /// 透传V600
    /// </summary>
    [Serializable]
    [DataContract]
    public class TransparentCMD
    {
        public TransparentCMD() { }

        /// <summary>
        /// 99dc[指令长度],[MDVR芯片号],[消息序列号],指令关键字,[位置和状态],透传信息类型,
        /// 上传透传会话编号,透传信息真实长度,透传信息转义长度,透传信息#
        /// </summary>
        /// <param name="str"></param>
        public TransparentCMD(string str)
        {
            if (!string.IsNullOrEmpty(str))
            {
                string[] array = str.Substring(0, str.LastIndexOf("#")).Split(',');

                if (array[3].ToUpper().Equals("V600"))
                {
                    this.MdvrCoreId = array[1];   ////芯片号

                    var time = ConvertHelper.ConvertStrToDate(array[4], "yyMMdd HHmmss");
                    if (time != null)
                    {
                        this.GpsTime = time.Value;     ////GPS时间
                    }

                    this.GpsValid = array[5];     ////GPS有效性
                    this.Longitude = array[6];    ////经度
                    this.Latitude = array[7];     ////纬度
                    this.Speed = array[8];        ////速度
                    this.Direction = array[9];    ////方向

                    this.TransparentType = int.Parse(array[20]);   ////透传信息类型
                    this.TransparentContext = array[24];           ////透传信息(xml)

                    this.Context = str;
                }
            }
        }

        /// <summary>
        /// 芯片号
        /// </summary>
        [DataMember]
        public string MdvrCoreId { get; set; }

        /// <summary>
        /// 安全套件状态
        /// </summary>
        [DataMember]
        public int SuiteStatus { get; set; }

        /// <summary>
        /// 指令关键字
        /// </summary>
        [DataMember]
        public string CMD { get; set; }

        /// <summary>
        /// 经度
        /// </summary>
        [DataMember]
        public string Longitude { get; set; }

        /// <summary>
        /// 纬度
        /// </summary>
        [DataMember]
        public string Latitude { get; set; }

        /// <summary>
        /// 速度
        /// </summary>
        [DataMember]
        public string Speed { get; set; }

        /// <summary>
        /// 方向
        /// </summary>
        [DataMember]
        public string Direction { get; set; }

        /// <summary>
        /// GPS时间
        /// </summary>
        [DataMember]
        public Nullable<DateTime> GpsTime { get; set; }

        /// <summary>
        /// GPS有效性
        /// </summary>
        [DataMember]
        public string GpsValid { get; set; }

        /// <summary>
        /// ASCII码整数，1：计价器，2：广告屏，3安装验收检测，4：运维信息 5：升级结果
        /// </summary>
        [DataMember]
        public int TransparentType { get; set; }

        /// <summary>
        /// 回话ID
        /// </summary>
        [DataMember]
        public string SuiteRunintStatusID { get; set; }

        /// <summary>
        /// 透传信息
        /// </summary>
        [DataMember]
        public string TransparentContext { get; set; }

        /// <summary>
        /// 原始文本信息
        /// </summary>
        [DataMember]
        public string Context { get; set; }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            if (!string.IsNullOrEmpty(Convert.ToString(MdvrCoreId)))
            {
                builder.AppendLine("MdvrCoreId:" + MdvrCoreId.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(SuiteStatus)))
            {
                builder.AppendLine("SuiteStatus:" + SuiteStatus.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(CMD)))
            {
                builder.AppendLine("CMD:" + CMD.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(Longitude)))
            {
                builder.AppendLine("Longitude:" + Longitude.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(Latitude)))
            {
                builder.AppendLine("Latitude:" + Latitude.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(Speed)))
            {
                builder.AppendLine("Speed:" + Speed.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(Direction)))
            {
                builder.AppendLine("Direction:" + Direction.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(GpsTime)))
            {
                builder.AppendLine("GpsTime:" + GpsTime.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(GpsValid)))
            {
                builder.AppendLine("GpsValid:" + GpsValid.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(TransparentType)))
            {
                builder.AppendLine("TransparentType:" + TransparentType.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(SuiteRunintStatusID)))
            {
                builder.AppendLine("SuiteRunintStatusID:" + SuiteRunintStatusID.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(TransparentContext)))
            {
                builder.AppendLine("TransparentContext:" + TransparentContext.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(Context)))
            {
                builder.AppendLine("Context:" + Context.ToString());
            }
            return builder.ToString();
        }

    }
}
