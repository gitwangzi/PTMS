/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: 00c363c6-bdac-4a87-8bcd-d8079133e5c6      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: WANGJINCAI
/////                 Author: TEST(wangjc)
/////======================================================================
/////           Project Name: Gsafety.PTMS.Integration.Test
/////    Project Description:    
/////             Class Name: UnitTest1
/////          Class Version: v1.0.0.0
/////            Create Time: 2013-09-03 15:05:00
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2013-09-03 15:05:00
/////            Modified by:
/////   Modified Description: 
/////======================================================================
using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Gsafety.PTMS.Integration.Service;
using Gsafety.PTMS.Alarm.Contract;
using System.Data.Entity;
using System.Linq;
using System.Collections.Generic;
using Gsafety.PTMS.Integration.Contract;

namespace Gsafety.PTMS.Integration.Test
{
    [TestClass]
    public class VedioTest 
    { 
        private VedioService _srv = new VedioService();        
        private string _mdrid="0202501408";
        private DateTime _startTime=new DateTime(2013, 9, 4,10,0,0);
        private DateTime _endTime=new DateTime(2013, 9, 5, 17, 0, 0);
        private string _alarmId = "12345";

        private PTMS.DBEntity.PTMSEntities _context = new Gsafety.PTMS.DBEntity.PTMSEntities();

        public VedioTest()
        {
            //InitTest();
        }

        private TestContext testContextInstance;

        /// <summary>
        ///获取或设置测试上下文，上下文提供
        ///有关当前测试运行及其功能的信息。
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        
        private  void InitTest()
        {
            var item = _context.ALARM_RECORD.OrderByDescending(x => x.GPS_TIME).FirstOrDefault();
            if (item == null)
            {
                return;
            }
            this._mdrid = item.MDVR_CORE_SN;
            this._startTime = item.GPS_TIME ?? DateTime.Now;
            this._endTime = this._startTime.AddHours(2);
            this._alarmId = item.ALARM_UID;
        }

      
        [TestMethod]
        public void  QueryServerFileList_Test()
        {
            var arg = new QueryServerFileListArgs
            {
                Channel = 0,
                Start_Time = new DateTime(2013,9,5),
                Video_Type = 3,
                Mdvr_Id = _mdrid,
                End_Time = new DateTime(2013, 9,6),
            };
            var result=_srv.QueryServerFileList(arg);
            Assert.IsNotNull(result);
           
        }

        [TestMethod]
        public void QueryMdvrFileList_Test()
        {
            var arg = new QueryMdvrFileListArgs
            {
                Channel = 0,
                Start_Time = new DateTime(2013, 9, 5,01, 0, 0),
                Video_Type = 3,
                Mdvr_Id = _mdrid,
                End_Time = new DateTime(2013, 9, 5, 6, 0, 0),
            };

            var result = _srv.QueryMdvrFileList(arg);
            Assert.IsNotNull(result.Result.Files);
            if (result.Result.Files != null && result.Result.Files.Count > 0)
            {
                DownloadMdvrFile_Test(result.Result.Files[0].Mdvr_File_Id, result.Result.Files[0].Start_Time, result.Result.Files[0].End_Time);
            }
        }

        private void DownloadMdvrFile_Test(string fileId, DateTime startTime, DateTime endTime)
        {
            var offset = (int)(endTime - startTime).TotalSeconds;
            var arg = new DownloadMdvrFileArgs
            {
                Offset_Start_Time = 0,
                Offset_End_Time = offset,
                Mdvr_Id = _mdrid,
                Mdvr_File_Id = fileId,
                Download_Flag = 0,
            };

            arg.Download_Start_Time = startTime.AddSeconds(arg.Offset_Start_Time);
            arg.Download_End_Time = startTime.AddSeconds(arg.Offset_End_Time);

            var result = _srv.DownloadMdvrFile(arg);
            Assert.IsNotNull(result.Result);
            QueryDownloadStatus_Test(fileId, offset);

        }
        
        private  void QueryDownloadStatus_Test(string fileId,int offset)
        {
            
            var arg = new QueryDownloadStatusArgs
            {
                Mdvr_File_Id = fileId,
                Mdvr_Id = _mdrid,
                Offset_End_Time =offset,
                Offset_Start_Time = 0,
            };
           var result=_srv.QueryDownloadStatus(arg);
           Assert.IsNotNull(result.Result);
           string succStat = "4";

           //(0 等待下载,1 指令发送成功,2 下载中,3 下载停止,4 下载完成,10 指令发送失败)

           List<string> comState = new List<string>
           {
               "4",//完成
               "10",               
           };
           while (!comState.Contains(result.Result.Status))
           {
               result = _srv.QueryDownloadStatus(arg);
              
           }
           if (result.Result.Status == succStat)
           {
               DownloadFileToLocal_Test(fileId, offset);
           }
           Assert.IsNotNull(result.Result);
        }


        private void DownloadFileToLocal_Test(string fid, int offset)
        {
            var arg = new DownloadFileToLocalArgs
            {
                Mdvr_File_Id = fid,
                Offset_Start_Time = 0,
                Offset_End_Time = offset,
                Mdvr_Id = _mdrid,

            };
            var result = _srv.DownloadFileToLocal(arg);
            Assert.IsNotNull(result.Result);
        }

        [TestMethod]
        public   void  CheckMediaFileSize_Test()
        {
            var arg = new CheckMediaFileSizeArgs
            {
                Mdvr_Id = _mdrid,
                Date = _startTime,
            };
           var result= _srv.CheckMediaFileSize(arg);
           Assert.IsNotNull(result.Result);
        }

        [TestMethod]
        public void CheckAlarmVideo_Test()
        {
            var arg = new CheckAlarmVideoArgs
            {
                Mdvr_Id ="0202501408", //_mdrid,
                Alarm_Id = "00002",//_alarmId,
                Date = DateTime.Parse("2013-09-05 11:41:00"),//_startTime,
            };
            var result=_srv.CheckAlarmVideo(arg);
            Assert.IsNotNull(result.Result);
        }
    }
}
