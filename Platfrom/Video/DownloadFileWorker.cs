/////Copyright (C) Gsafety 2014 .All Rights Reserved.
/////======================================================================
/////                   Guid: fe53e639-cd6c-4d84-8f35-4f5b78e3ab50      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: PC-ZHANGY
/////                 Author: GJSY(zhangy)
/////======================================================================
/////           Project Name: Gsafety.PTMS.Video
/////    Project Description:    
/////             Class Name: DownloadFileWorker
/////          Class Version: v1.0.0.0
/////            Create Time: 2014/12/25 14:37:05
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2014/12/25 14:37:05
/////            Modified by:
/////   Modified Description: 
/////======================================================================
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gsafety.Common.Util;
using Gsafety.PTMS.Message.Contract.Data;
using Gsafety.PTMS.Analysis.Helper;
using Gsafety.MQ;
using Gsafety.PTMS.Message.Contract.Data.Video;

namespace Gsafety.PTMS.Video
{
    public class DownloadFileWorker
    {
        static Dictionary<string, int> dicDownloadStatus;
        private static Dictionary<string, int> MdvrUid;
        static DownloadFileWorker()
        {
            dicDownloadStatus = new Dictionary<string, int>();
            MdvrUid=new Dictionary<string, int>();
        }

        public static void CommandSend(byte[] bytes, string key)
        {
            var model = ConvertHelper.BytesToObject(bytes) as DownloadFile;
            if (!dicDownloadStatus.ContainsKey(model.Uid))
                dicDownloadStatus.Add(model.Uid, 0);
            if(!MdvrUid.ContainsKey(model.Uid))
                MdvrUid.Add(model.Uid,0);
            if (MdvrUid[model.Uid].Equals(65535))
            {
                MdvrUid[model.Uid] = 0;
            }
            string cmd = GetCMD(model);
            var commandContent = System.Text.UTF8Encoding.UTF8.GetBytes(cmd);
            VideoMessage.PublishMessage(Constdefine.MDVREXCHANGE, string.Format("{0}{1}", MonitorRoute.DownloadFileKey, model.Mdvr_Id), commandContent);
            MdvrUid[model.Uid]++;
        }

        public static void CommandReply(byte[] bytes, string key)
        {
            string str = System.Text.UnicodeEncoding.UTF8.GetString(bytes);
          //  string str = @"99dc0165,006A00BDD0,Johnny,V0,141225 104520,V,-00000.0000,-0000.0000,0.00,0.00,8000000000000000,0000000000000000,50.00,999.00,00000000.0000,,,0,0,0,C114,141225 101840,0,1,,#";
            string[] array = str.Substring(0, str.LastIndexOf("#")).Split(new char[] { ',' });
            string mdvrid = array[1];
            string guid = array[2];
            int ReplyResult = int.Parse(array[23]);
            if (dicDownloadStatus.ContainsKey(guid))
                dicDownloadStatus[guid] = ReplyResult;
        }

        public static void CommandV23Reply(byte[] bytes, string key)
        {
            string str = System.Text.UnicodeEncoding.UTF8.GetString(bytes);
            //string str = @"99dc0205,006A00BDD0,10,V23,141225 104520,V,-00000.0000,-0000.0000,0.00,0.00,8000000000000000,0000000000000000,50.00,999.00,00000000.0000,,,0,0,0,6d9d2436-02ad-419b-91f5-bd0deab6469d,,,00010003,Connection overtime#";
            string[] array = str.Substring(0, str.LastIndexOf("#")).Split(new char[] { ',' });
            string mdvrid = array[1];
            string guid = array[20];
            string ReplyResult = array[23];
            DownloadFileReply reply = new DownloadFileReply();
            reply.Mdvr_Id = mdvrid;
            reply.Uid = guid;
            reply.ReplyResult = ReplyResult;
            var cmd = ConvertHelper.ObjectToBytes(reply);
            VideoMessage.PublishMessage(Constdefine.APPEXCHANGE, string.Format("{0}*", ReplyRoute.HandleDownloadFileReplyKey), cmd);
        }

        private static string GetCMD(DownloadFile args)
        {
            StringBuilder strCmd = new StringBuilder();
        //    args.DownloadType = 0;
            strCmd.Append("99dcXXXX,")
                .Append(args.Mdvr_Id)
                .Append(",")
                .Append(MdvrUid[args.Uid])
                .Append(",")
                .Append("C114")
                .Append(",")
                .Append(DateTime.Now.ToString("yyMMdd HHmmss"))
                .Append(",")
                .Append(args.Uid)
                .Append(",")
                .Append(args.DownloadType)
                .Append(",")
                 .Append(args.BeginFileBackforward)
                .Append(",")
                 .Append(args.EndFileBackforward)
                .Append(",")
                .Append(args.BeginTimeBackforward)
                .Append(",")
                .Append(args.EndTimeBackforward)
                .Append(",")
                .Append("")
                .Append(",")
                .Append(args.File_Id)
                .Append("#");
            strCmd.Replace("XXXX", (strCmd.Length - 8).ToString("D5").Substring(1));
            return strCmd.ToString();
        }
    }
}
