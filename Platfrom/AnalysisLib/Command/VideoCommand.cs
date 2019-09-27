using Gsafety.Common.Util;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;
using Gsafety.PTMS.Common.Data;
using Gsafety.PTMS.DBEntity;
using Gsafety.Common.Logging;
using System.Reflection;
using Gsafety.MQ;
using Gsafety.PTMS.Analysis.Helper;
using Gsafety.PTMS.Common.Data.Data.Video;

namespace Gsafety.PTMS.AnalysisLib.Command
{
    public class VideoCommand : CommandBase
    {
        private const string _dateFormat = "yyyy-MM-dd HH:mm:ss";

        public static void SendQueryFile(PTMSEntities context, byte[] bytes, string key)
        {
            var model = ConvertHelper.BytesToObject(bytes) as QueryMdvrFileList;
            var time = ConvertHelper.DateTimeNow();

            var jsonModel = new QueryFileListJson()
            {
                Channel = model.Channel,
                StartTime = model.Start_Time.ToString(_dateFormat),
                EndTime = model.End_Time.ToString(_dateFormat),
                FileType = model.FileType,
                StreamType = model.StreamType,
                UID = model.Mdvr_Id
            };

            var entity = new RUN_VIDEO_QUERY()
            {
                ID = Guid.NewGuid().ToString(),
                USER_ID = model.UserID,
                CREATE_TIME = time,
                SEND_TIME = time,
                STREAM_TYPE = (short)model.StreamType,
                FILET_YPE = (short)model.FileType,
                CHANNEL = string.Join(",", model.Channel),
                START_TIME = model.Start_Time.ToString(_dateFormat),
                END_TIME = model.End_Time.ToString(_dateFormat),
                MDVR_CORE_SN = model.Mdvr_Id,
                PACKAGE_SEQ = jsonModel.SerialNo,
            };

            context.RUN_VIDEO_QUERY.Add(entity);

            if (context.SaveChanges() <= 0)
            {
                LoggerManager.Logger.Error(MethodBase.GetCurrentMethod().ToString() + " : Insert RUN_VIDEO_QUERY Error");
                return;
            }

            var json = ConvertHelper.ConvertModelToJson(jsonModel);
            var sendbytes = UnicodeEncoding.UTF8.GetBytes(json);

            string routeKey = VideoRoute.QueryMdvrFileListMDVRKey + model.Mdvr_Id;
            LoggerManager.Logger.Info("QueryMdvrFileList:" + "route:" + routeKey + ",data:" + json);

            TransforMessage.PublishMessage(Constdefine.MDVREXCHANGE, routeKey, sendbytes);
        }

        public static void OnReplyQueryFile(PTMSEntities context, byte[] bytes, string key)
        {
            var json = UnicodeEncoding.UTF8.GetString(bytes);
            var responseModel = ConvertHelper.ConvertJsonToModel<QueryFileListResponseJson>(json);

            LoggerManager.Logger.Info("OnReplyQueryFile:" + " data:" + json);

            var targetQuery = context.RUN_VIDEO_QUERY.FirstOrDefault(t => t.PACKAGE_SEQ == responseModel.SerialNo && t.MDVR_CORE_SN == responseModel.UID);
            if (targetQuery == null)
            {
                LoggerManager.Logger.Error(MethodBase.GetCurrentMethod().ToString() + " : Get RUN_VIDEO_QUERY Error");
                return;
            }

            var queryServerFileListMessages = new List<QueryServerFileListMessage>();

            if (responseModel.FileItems.Count > 0)
            {
                //var minTime = responseModel.FileItems.Min(t => DateTime.Parse(t.RecordStartTime));
                //var maxTime = responseModel.FileItems.Max(t => DateTime.Parse(t.RecordEndTime));

                //var existMdvrFiles = context.MDI_DOWNLOAD_VIDEO.
                //      Where(t => t.MDVR_CORE_SN == responseModel.UID && t.START_TIME >= minTime && t.END_TIME <= maxTime)
                //      .Select(t => new
                //      {
                //          CHANNELID = t.CHANNEL_ID,
                //          StartTime = t.START_TIME,
                //          EndTime = t.END_TIME
                //      }).ToList();

                foreach (var item in responseModel.FileItems)
                {
                    var startTime = DateTime.Parse(item.RecordStartTime);
                    var endTime = DateTime.Parse(item.RecordEndTime);

                    //if (existMdvrFiles.Any(t => t.CHANNELID == item.RecordChannel && t.StartTime == startTime && t.EndTime == endTime))
                    //{
                    //    continue;
                    //}

                    var fileInfo = new QueryServerFileListMessage()
                    {
                        Channel = item.RecordChannel,
                        StartTime = startTime,
                        EndTime = endTime,
                        UUID = responseModel.UID,
                        FileID = item.RecordId,
                        FileSize = (decimal)item.RecordSize,
                        VideoType = item.FileType
                    };

                    queryServerFileListMessages.Add(fileInfo);
                }
            }

            if (queryServerFileListMessages.Count > 0)
            {
                var appResponse = new QueryServerFileListMessageResponse();
                appResponse.QueryServerFileListMessages = queryServerFileListMessages;
                appResponse.UserID = targetQuery.USER_ID;

                var message = ConvertHelper.ObjectToBytes(appResponse);
                TransforMessage.PublishMessage(Constdefine.APPEXCHANGE, VideoRoute.QueryMdvrFileListAppResponseKey + responseModel.UID, message);
            }
            else
            {
                LoggerManager.Logger.Info("OnReplyQueryFile:" + " queryServerFileListMessages.Count=" + queryServerFileListMessages.Count);
            }

            context.RUN_VIDEO_QUERY.Remove(targetQuery);
            LoggerManager.Logger.Info("OnReplyQueryFile:" + " RUN_VIDEO_QUERY.Remove " + targetQuery.PACKAGE_SEQ);
            context.SaveChanges();
        }

        public static void SendDownloadMdvrFile(PTMSEntities context, byte[] bytes, string key)
        {
            var model = ConvertHelper.BytesToObject(bytes) as DownloadFile;

            var currentTime = ConvertHelper.DateTimeNow();
            var streamName = model.MdvrCoreSn + "_" + model.ChannelID + "_" + currentTime.ToString("yyyyMMddHHmmss");
            var entity = new MDI_DOWNLOAD_VIDEO()
            {
                UUID = model.UUID,
                CHANNEL_ID = int.Parse(model.ChannelID),
                MDVR_CORE_SN = model.MdvrCoreSn,
                STREAM_ID = 1,
                START_TIME = model.StartTime,
                END_TIME = model.EndTime,
                MDVR_FILE = model.FileId,
                OFFSET_FLAG = model.OffSetFlag,
                OFFSET_START_TIME = 0,
                OFFSET_END_TIME = 0,
                DOWNLOAD_STATUS = (int)DownloadStatus.Downloading,//正在下载
                CREATE_TIME = ConvertHelper.DateTimeNow(),
                VIDEO_TYPE = model.FileType,
                VIDEO_URL = streamName,
                VIDEO_NAME = streamName,
                SOURCE_SIZE = (int)model.FileSize
            };

            context.MDI_DOWNLOAD_VIDEO.Add(entity);

            if (context.SaveChanges() <= 0)
            {
                LoggerManager.Logger.Error(MethodBase.GetCurrentMethod().ToString() + " : Insert MDI_DOWNLOAD_VIDEO Error");
                return;
            }

            var jsonModel = new DownloadFileJson()
            {
                EndTime = model.EndTime.ToString(_dateFormat),
                OffSet = model.OffSet,
                OffSetFlag = model.OffSetFlag,
                RecordId = model.FileId,
                StartTime = model.StartTime.ToString(_dateFormat),
                StreamName = streamName,
                UID = model.MdvrCoreSn
            };


            var json = ConvertHelper.ConvertModelToJson(jsonModel);
            var sendbytes = UnicodeEncoding.UTF8.GetBytes(json);

            string routeKey = VideoRoute.DownloadMdvrFileMDVRKey + model.MdvrCoreSn;
            LoggerManager.Logger.Info("QueryMdvrFileList:" + "route:" + routeKey + ",data:" + json);

            TransforMessage.PublishMessage(Constdefine.MDVREXCHANGE, routeKey, sendbytes);
        }

        public static void SendTakePicture(PTMSEntities context, byte[] bytes, string key)
        {
            var model = ConvertHelper.BytesToObject(bytes) as TakePictureArgs;
            var time = ConvertHelper.DateTimeNow();
            for (int i = 0; i < model.Channel.Count; i++)
            {
                var jsonModel = new TakePictureJson()
                {
                    UID = model.Mdvr_Core_Sn,
                    Channel = model.Channel[i],
                    Cmd = model.Cmd,
                    Interval = model.Interval,
                    Resolution = model.Resolution,
                    Quality = model.Quality,
                    Brightness = model.Brightness,
                    Saturation = model.Saturation,
                    Contrast = model.Contrast,
                    Color = model.Color
                };

                var entity = new RUN_TAKEPICTURE()
                {
                    ID = Guid.NewGuid().ToString(),
                    USER_ID = model.UserID,
                    MDVR_CORE_SN = model.Mdvr_Core_Sn,
                    CREATE_TIME = time,
                    SEND_TIME = time,
                    CHANNEL = (short)model.Channel[i],
                    CMD = model.Cmd,
                    INTERVAL = model.Interval,
                    RESOLUTION = model.Resolution,
                    BRIGHTNESS = (short)model.Brightness,
                    QUALITY = (short)model.Quality,
                    SATURATION = (short)model.Saturation,
                    CONSTRAST = (short)model.Contrast,
                    COLOR = (short)model.Color,
                    RECEIVECOUNT = 0,
                    PACKAGE_SEQ = jsonModel.SerialNo,
                };

                context.RUN_TAKEPICTURE.Add(entity);
                var json = ConvertHelper.ConvertModelToJson(jsonModel);
                var sendbytes = UnicodeEncoding.UTF8.GetBytes(json);

                string routeKey = VideoRoute.TakePictureMDVRKey + model.Mdvr_Core_Sn;
                LoggerManager.Logger.Info("TakePicture:" + "route:" + routeKey + ",data:" + json);

                TransforMessage.PublishMessage(Constdefine.MDVREXCHANGE, routeKey, sendbytes);
            }

            context.SaveChanges();
            if (context.SaveChanges() <= 0)
            {
                LoggerManager.Logger.Error(MethodBase.GetCurrentMethod().ToString() + " : Insert RUN_TAKEPICTURE Error");
                return;
            }
        }

        public static void OnReplyTakePicture(PTMSEntities context, byte[] bytes, string key)
        {
            var json = UnicodeEncoding.UTF8.GetString(bytes);
            var responseModel = ConvertHelper.ConvertJsonToModel<TakePictureResponseJson>(json);

            LoggerManager.Logger.Info("OnReplyTakePicture:" + " data:" + json);

            var targetQuery = context.RUN_TAKEPICTURE.FirstOrDefault(t => t.PACKAGE_SEQ == responseModel.SerialNo && t.MDVR_CORE_SN == responseModel.UID);
            if (targetQuery == null)
            {
                LoggerManager.Logger.Error(MethodBase.GetCurrentMethod().ToString() + " : Get RUN_TAKEPICTURE Error");
                return;
            }

            #region SavePicture
            //byte[] datas = Convert.FromBase64String(responseModel.Data);
            //string fileName = string.Empty;
            //string miniFileName = string.Empty;
            //using (MemoryStream ms = new MemoryStream(datas))
            //{
            //    Image img = Image.FromStream(ms);
            //    string path = ConfigHelper.PicturePath;
            //    string folderName = DateTime.UtcNow.Date.ToString("yyyyMMdd");
            //    if (!Directory.Exists("@" + path + folderName))
            //    {
            //        Directory.CreateDirectory("@" + path + folderName);
            //    }

            //    fileName = path + folderName + DateTime.UtcNow.ToString("yyyyMMdd_hhmmssfff");
            //    miniFileName = path + folderName + DateTime.UtcNow.ToString("yyyyMMdd_hhmmssfff");
            //    if (responseModel.Format == 0)
            //    {
            //        fileName += ".jpeg";
            //        miniFileName += "mini.jpeg";
            //    }
            //    else
            //    {
            //        fileName +=  ".tif";
            //        miniFileName += "mini.tif";
            //    }

            //    img.Save(fileName);

            //    //创建缩略图
            //    float srcWidth = img.Width;
            //    float srcHeight = img.Height;
            //    float scale = srcHeight / srcWidth;
            //    int miniWidth = 180;
            //    int miniHeight = (int)(scale * miniWidth);
            //    //decimal imgSize = img.

            //    Bitmap bmp = new Bitmap(miniWidth, miniHeight);
            //    Graphics gr = Graphics.FromImage(bmp);
            //    gr.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            //    gr.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
            //    gr.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.High;
            //    Rectangle rt = new Rectangle(0, 0, miniWidth, miniHeight);
            //    gr.DrawImage(img, rt, 0, 0, srcWidth, srcHeight, GraphicsUnit.Pixel);
            //    bmp.Save(miniFileName);
            //    bmp.Dispose();
            //    img.Dispose();
            //}

            #endregion

            string photoID = Guid.NewGuid().ToString();
            string clientID =
                context.RUN_SUITE_WORKING.Where(t => t.MDVR_CORE_SN == responseModel.UID).FirstOrDefault().CLIENT_ID;
            var entity = new MDI_PHOTOGRAPH()
            {
                ID = photoID,
                CLIENT_ID = clientID,
                DEVICE_SN = responseModel.UID,
                CHANNEL_ID = responseModel.Channel,
                DEVICE_TYPE = 0,
                CREATE_TIME = Convert.ToDateTime(responseModel.Time),
                LONGITUDE = responseModel.Longitude,
                LATITUDE = responseModel.Latitude,
                IMG_URL = responseModel.Path,
                MINIIMG_URL = responseModel.Path,
                IMGSIZE = 0,
                NOTE = "",
                STATUS = 0
            };

            context.MDI_PHOTOGRAPH.Add(entity);

            targetQuery.RECEIVECOUNT += 1;

            TakePictureMessageResponse appResponse = new TakePictureMessageResponse();
            Photo photo = new Photo();
            photo.ID = photoID;
            photo.ClientID = clientID;
            photo.DeviceSn = responseModel.UID;
            photo.DeviceType = 0;
            photo.ChannelID = responseModel.Channel;
            photo.Img_Url = responseModel.Path;
            photo.MiniImg_Url = responseModel.Path;
            photo.Img_Size = 0;
            photo.Note = string.Empty;
            photo.Status = false;
            photo.Create_Time = Convert.ToDateTime(responseModel.Time);
            photo.Longitude = responseModel.Longitude;
            photo.Latitude = responseModel.Latitude;
            photo.IsChecked = false;
            appResponse.Photo = photo;
            appResponse.UserID = targetQuery.USER_ID;

            var message = ConvertHelper.ObjectToBytes(appResponse);
            TransforMessage.PublishMessage(Constdefine.APPEXCHANGE, VideoRoute.TakePictureAppResponseKey + responseModel.UID, message);
            if (targetQuery.CMD == targetQuery.RECEIVECOUNT)
            {
                context.RUN_TAKEPICTURE.Remove(targetQuery);
                LoggerManager.Logger.Info("OnReplyTakePicture:" + " RUN_TAKEPICTURE.Remove " + targetQuery.PACKAGE_SEQ);
            }
            context.SaveChanges();
        }
    }
}
