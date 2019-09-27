/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: 41ef3923-a7da-4deb-bc4e-90098dd78402      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: PC-LIW
/////                 Author: TEST(liw)
/////======================================================================
/////           Project Name: Gsafety.PTMS.Alarm.Contract
/////    Project Description:    
/////             Class Name: IVehicleAlarmService
/////          Class Version: v1.0.0.0
/////            Create Time: 2013/8/8 11:03:41
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2013/8/23 11:03:41
/////            Modified by: BilongLiu
/////   Modified Description: 
/////======================================================================
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using Gs.PTMS.Common.Data.Enum;
using Gsafety.PTMS.Integration.Contract.Data;
using Gsafety.PTMS.Base.Contract.Data;
using Gsafety.PTMS.Common.Data;
using Gsafety.PTMS.Common.Data.Data.Video;
using Gsafety.PTMS.Common.Data.Enum;
using Gsafety.PTMS.Integration.Contract;

namespace Gsafety.PTMS.Alarm.Contract
{
    [ServiceContract]
    public interface IVedioService
    {
        /// <summary>
        /// 查询已有监测视频文件列表
        /// </summary>
        /// <param name="arg"></param>
        /// <returns></returns>
        [OperationContract]
        MultiMessage<QueryServerFileListMessage> QueryServerFileList(QueryServerFileListArgs arg);

        [OperationContract]
        MultiMessage<QueryServerFileListMessage> QueryHistoryVideoList(string vehicleid, DateTime startTime, DateTime endTime, int pageSize, int pageValue);

        /// <summary>
        /// 查询文件的下载进度
        /// </summary>
        /// <param name="arg"></param>
        /// <returns></returns>
        [OperationContract]
        MultiMessage<QueryDownloadStatusMessage> QueryDownloadStatus(QueryDownloadStatusArgs arg);

        /// <summary>
        /// 验证报警视频是否存在
        /// </summary>
        /// <param name="arg"></param>
        /// <returns></returns>
        [OperationContract]
        SingleMessage<Dictionary<int, bool>> CheckAlarmVideo(CheckAlarmVideoArgs arg);

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        MultiMessage<HistoryItemForVideoAppeal> GetHistoryItemForVideoAppeal(string starttime, string endtime);

        [OperationContract]
        MultiMessage<QueryServerFileListMessage> GetAlarmFiftyVideoAppeal(string AlarmId);

        [OperationContract]
        SingleMessage<bool> UpdateVideoNote(string videoID, string note);

        [OperationContract]
        MultiMessage<Photo> GetPhotoList(QueryPhotoFileListArgs arg);

        [OperationContract]
        SingleMessage<bool> SetPhotoMark(List<string> list, int status, string note);

        [OperationContract]
        SingleMessage<bool> DeletePhoto(List<string> list);

        /// <summary>
        ///根据车牌号获取通道号
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        MultiMessage<RealTimeChannelInfo> GetChannelByVehicleSN(string vehcilesn);
    }
}

