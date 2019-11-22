/////Copyright (C) Gsafety 2014 .All Rights Reserved.
/////======================================================================
/////                   Guid: 55fbde17-92fc-49ae-86ae-c2f38c123e0d      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: PC-ZHANGY
/////                 Author: GJSY(zhangy)
/////======================================================================
/////           Project Name: Gsafety.PTMS.Video
/////    Project Description:    
/////             Class Name: VideoWorker
/////          Class Version: v1.0.0.0
/////            Create Time: 2014/12/15 14:22:37
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2014/12/15 14:22:37
/////            Modified by:
/////   Modified Description: 
/////======================================================================
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gsafety.Common.Util;
using Gsafety.PTMS.Analysis.Helper;
using Gsafety.Common.Logging;
using Gsafety.PTMS.Message.Contract.Data;
using Gsafety.MQ;
using Gsafety.PTMS.Message.Contract.Data.Video;

namespace Gsafety.PTMS.Video
{
    public class VideoWorker
    {
        static Dictionary<string, List<MdvrFileListResult>> dicResult;
        static Dictionary<string, int> dicChannel;
        static VideoWorker()
        {
            dicResult = new Dictionary<string, List<MdvrFileListResult>>();
            dicChannel = new Dictionary<string, int>();
        }

        public static void CommandSend(byte[] bytes, string key)
        {
            string guid = Guid.NewGuid().ToString();
            dicResult.Add(guid, new List<MdvrFileListResult>() { });
            var model = ConvertHelper.BytesToObject(bytes) as QueryMdvrFileList;
        //    model.Start_Time = model.Start_Time.AddHours(12.5);
          //  model.End_Time = model.End_Time.AddHours(12.5);
            dicChannel.Add(guid, model.Channel.Count);
            string cmd = GetCMD(model, guid);
            var commandContent = System.Text.UTF8Encoding.UTF8.GetBytes(cmd);
            VideoMessage.PublishMessage(Constdefine.MDVREXCHANGE, string.Format("{0}{1}", MonitorRoute.HandleVideoListKey, model.Mdvr_Id), commandContent);
            #region test
    //       CommandReply(bytes, key, guid);
            #endregion
        }

        public static void CommandReply(byte[] bytes, string key, string oldguid)
        {
            #region test
          //  string str = @"99dc1323,006A00BCF7,{0},V0,150522 181123,A,11619.6785,3959.0766,0.00,275.00,0000000000000000,0000000000000000,43.00,999.00,00000000.0000,,,0,0,0,C110,150522 181151,0,1,,,10,0,10,418185216,389283840,418316288,389283840,418054144,388956160,418119680,388890624,419102720,390004736,000000,000000,003000,003000,010000,010000,013000,013000,020000,020000,003000,003000,010000,010000,013000,013000,020000,020000,023000,023000,4,4,4,4,4,4,4,4,4,4,0001,0002,0001,0002,0001,0002,0001,0002,0001,0002,/stm/disk/0/p1/2015-05-22/00MQ000200000000-150522-000000-003000-00p401100000.nvr|/stm/disk/0/p1/2015-05-22/00MQ000200000000-150522-000000-003000-00p402100000.nvr|/stm/disk/0/p1/2015-05-22/00MQ000200000000-150522-003000-010000-00p401100000.nvr|/stm/disk/0/p1/2015-05-22/00MQ000200000000-150522-003000-010000-00p402100000.nvr|/stm/disk/0/p1/2015-05-22/00MQ000200000000-150522-010000-013000-00p401100000.nvr|/stm/disk/0/p1/2015-05-22/00MQ000200000000-150522-010000-013000-00p402100000.nvr|/stm/disk/0/p1/2015-05-22/00MQ000200000000-150522-013000-020000-00p401100000.nvr|/stm/disk/0/p1/2015-05-22/00MQ000200000000-150522-013000-020000-00p402100000.nvr|/stm/disk/0/p1/2015-05-22/00MQ000200000000-150522-020000-023000-00p401100000.nvr|/stm/disk/0/p1/2015-05-22/00MQ000200000000-150522-020000-023000-00p402100000.nvr#";
      //    str = string.Format(str, oldguid);
            #endregion            
            string str = System.Text.UnicodeEncoding.UTF8.GetString(bytes);
            string[] array = str.Substring(0, str.LastIndexOf("#")).Split(new char[] { ',' }, 30);
            string mdvrid = array[1];
            string guid = array[2];
            int fileTotalCount = int.Parse(array[26]);
            int unfinishFileCount = int.Parse(array[27]);
            int finishFileCount = int.Parse(array[28]);
            List<MdvrFileListResult> lstResult = GetResult(array[29], finishFileCount, dicChannel[guid]);
            if (dicResult.ContainsKey(guid))
                dicResult[guid].AddRange(lstResult);
            if (unfinishFileCount.Equals(0))
            {
                VideoListResult result = new VideoListResult();
                result.lstResult = lstResult;
                var cmd = ConvertHelper.ObjectToBytes(result);
                //sendcommand
                VideoMessage.PublishMessage(Constdefine.APPEXCHANGE, string.Format("{0}*", ReplyRoute.HandleBusinessVideoListReplyKey), cmd);
                dicResult.Remove(guid);
                dicChannel.Remove(guid);
            }
        }

        private static string GetCMD(QueryMdvrFileList args, string guid)
        {
            StringBuilder strCmd = new StringBuilder();
            strCmd.Append("99dcXXXX,")
                .Append(args.Mdvr_Id)
                .Append(",")
                .Append(guid)
                .Append(",")
                .Append("C110")
                .Append(",")
                .Append(DateTime.Now.ToString("yyMMdd HHmmss"))
                .Append(",")
                .Append(args.Start_Time.ToString("yyMMdd HHmmss"))
                .Append(",")
                .Append(args.End_Time.ToString("HHmmss"))
                .Append(",")
                .Append(args.Video_Type)
                .Append(",")
                .Append(GetVideoChannel(args.Channel))
                .Append("#");
            strCmd.Replace("XXXX", (strCmd.Length - 8).ToString("D5").Substring(1));
            return strCmd.ToString();
        }

        private static string GetVideoChannel(List<int> lstChannel)
        {
            string strChannel = "0{0}00";
            int channel = 0;
            foreach(int i in lstChannel)
            {
                channel += Convert.ToInt32(Math.Pow(2, i - 1));
            }
            string str = Convert.ToString(channel, 16).ToUpper();
            return string.Format(strChannel, str);
        }

        private static List<MdvrFileListResult> GetResult(string str, int finishFileCount, int channelCount)
        {
            //finishFileCount 8 channelCount 4
            //448200704,420085760,474284032,406847488,448659456,417071104,478019584,407502848,
            //000000,000000,000000,000000,003000,003000,003000,003000,003000,003000,003001,003001,010000,010000,010001,010001,4,4,4,4,4,4,4,4,0001,0002,0004,0008,0001,0002,0004,0008,/stm/disk/0/p1/2015-06-30/0NNN000100000000-150630-000000-003000-10u401000000.nvr|/stm/disk/0/p1/2015-06-30/0NNN000100000000-150630-000000-003000-10u402000000.nvr|/stm/disk/0/p1/2015-06-30/0NNN000100000000-150630-000000-003001-10u403000000.nvr|/stm/disk/0/p1/2015-06-30/0NNN000100000000-150630-000000-003001-10u404000000.nvr|/stm/disk/0/p1/2015-06-30/0NNN000100000000-150630-003000-010000-10u401000000.nvr|/stm/disk/0/p1/2015-06-30/0NNN000100000000-150630-003000-010000-10u402000000.nvr|/stm/disk/0/p1/2015-06-30/0NNN000100000000-150630-003000-010001-10u403000000.nvr|/stm/disk/0/p1/2015-06-30/0NNN000100000000-150630-003000-010001-10u404000000.nvr
            string[] lstFileSize = str.Split(new char[] { ',' }, finishFileCount + 1);
            string[] lstTime = lstFileSize[finishFileCount].Split(new char[] { ',' }, finishFileCount * 2 + 1);
            string[] lstVideoType = lstTime[finishFileCount * 2].Split(new char[] { ',' }, finishFileCount + 1);
            string[] lstChannel = lstVideoType[finishFileCount].Split(new char[] { ',' }, finishFileCount + 1);
            string[] lstFileName = lstChannel[finishFileCount].Split(new char[] { '|' });
            List<MdvrFileListResult> result = new List<MdvrFileListResult>();
            for (int i = 0; i < finishFileCount; i++)
            {
                MdvrFileListResult fileItem = new MdvrFileListResult();
                fileItem.Mdvr_File_Id = lstFileName[i];
                fileItem.Channel = GetChannel(lstChannel[i]);
                fileItem.Mdvr_File_Size = lstFileSize[i];
                string[] dateTime = fileItem.Mdvr_File_Id.Split('-');
                fileItem.Start_Time = GetDateTimeByName(dateTime[3],dateTime[4]);
                fileItem.End_Time = GetDateTimeByName(dateTime[3],dateTime[5]);
                result.Add(fileItem);
            }
            return result;
        }
        //150701-163000-170000
        private static DateTime GetDateTimeByName(string Ymd, string Hmm)
        {
            DateTime dt ;
         
            if (Ymd.Length.Equals(6) && Hmm.Length.Equals(6))
            {
               dt  = new DateTime(int.Parse("20" + Ymd.Substring(0, 2)), 
                   int.Parse(Ymd.Substring(2, 2)), int.Parse(Ymd.Substring(4, 2)), int.Parse(Hmm.Substring(0, 2)),
                   int.Parse(Hmm.Substring(2, 2)), int.Parse(Hmm.Substring(4, 2)));
               return dt;
            }
            else
            {

                return DateTime.Now;
            }
      
        }



        private static DateTime GetDateTime(string time)
        {
            DateTime dt = new DateTime();
            if (time.Length.Equals(6))
                dt = DateTime.Parse(time.Substring(0, 2) + ":" + time.Substring(2, 2) + ":" + time.Substring(4, 2));
            return dt;
        }

        private static int GetChannel(string channel)
        { 
            switch(channel)
            {
                case "0001":
                    return 0;
                case "0002":
                    return 1;
                case "0004":
                    return 2;
                case "0008":
                    return 3;
                default:
                    return -1;
            }
        }
    }
}
