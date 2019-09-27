/////Copyright (C) Gsafety 2014 .All Rights Reserved.
/////======================================================================
/////                   Guid: 1f6839e7-ce83-4b9f-807f-1df9066ad7e7      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: PC-ZHANGY
/////                 Author: GJSY(zhangy)
/////======================================================================
/////           Project Name: Gsafety.PTMS.Video
/////    Project Description:    
/////             Class Name: MessageProcessing
/////          Class Version: v1.0.0.0
/////            Create Time: 2014/12/15 10:42:46
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2014/12/15 10:42:46
/////            Modified by:
/////   Modified Description: 
/////======================================================================
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gsafety.PTMS.Analysis.Helper;
using Gsafety.Common.Logging;

namespace Gsafety.PTMS.Video
{
    public class MessageProcessing
    {
        [Business(typeName: SettingRoute.VideoListCMDKey)]
        public void ProcessVideoListSendup(byte[] bytes, string key)
        {
            try
            {
                LoggerManager.Logger.Info("Get Information:" + SettingRoute.VideoListCMDKey);
                VideoWorker.CommandSend(bytes, key);
            }
            catch (Exception ex)
            {
                LoggerManager.Logger.Error(ex);
            }
        }

        [Business(typeName: ReplyRoute.HandleVideoListReplyKey)]
        public void ProcessVideoListReply(byte[] bytes, string key)
        {
            try
            {
                string str = System.Text.UnicodeEncoding.UTF8.GetString(bytes);
            string[] array = str.Substring(0, str.LastIndexOf("#")).Split(new char[] { ',' }, 30);
           
            string guid = array[2];
            VideoWorker.CommandReply(bytes, key, guid);
            }
            catch (Exception ex)
            {
                LoggerManager.Logger.Error(ex);
            }
        }

        [Business(typeName: SettingRoute.DownloadMdvrFile)]
        public void ProcessDownloadMavrFile(byte[] bytes, string key)
        {
             LoggerManager.Logger.Info("Get Information:" + SettingRoute.DownloadMdvrFile);
                DownloadFileWorker.CommandSend(bytes,key);
        }

        [Business(typeName: ReplyRoute.HandleDownloadFileV23)]
        public void ProcessV23Reply(byte[] bytes, string key)
        {
            LoggerManager.Logger.Info("Get Information:" + ReplyRoute.HandleDownloadFileV23);
            DownloadFileWorker.CommandV23Reply(bytes, key);

        }

        [Business(typeName:  ReplyRoute.HandleDownloadMdvrFile)]
        public void ProcessDownloadMavrFileReply(byte[] bytes, string key)
        {
            LoggerManager.Logger.Info("Get Information:" + SettingRoute.DownloadMdvrFile);
            DownloadFileWorker.CommandReply(bytes, key);
        }
    }
}
