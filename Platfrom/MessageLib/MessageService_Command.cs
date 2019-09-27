using Gsafety.Common.Logging;
using Gsafety.Common.Util;
using Gsafety.MQ;
using Gsafety.PTMS.Analysis.Helper;
using Gsafety.PTMS.Common.Data;
using Gsafety.PTMS.Message.Contract;
using Gsafety.PTMS.Message.Contract.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Gsafety.PTMS.Common.Data.Data.Video;

namespace Gsafety.PTMS.MessageLib
{
    public partial class MessageService
    {
        public void SendGetVideoListCMD(QueryMdvrFileList model)
        {
            try
            {
                byte[] msg = ConvertHelper.ObjectToBytes(model);
                string routeKey = VideoRoute.QueryMdvrFileListAppKey + model.Mdvr_Id;
                LoggerManager.Logger.Info("Client:" + _queue + "  VideoListCMDKey ,route:" + routeKey + ",data:" + ConvertHelper.ConvertModelToJson(model));
                SendMessage(Constdefine.APPEXCHANGE, routeKey, msg);
            }
            catch (Exception ex)
            {
                LoggerManager.Logger.Error(ex);
            }
        }

        public void SendDownloadMdvrFile(DownloadFile model)
        {
            try
            {
                byte[] msg = ConvertHelper.ObjectToBytes(model);
                string routeKey = VideoRoute.DownloadMdvrFileAppKey + model.MdvrCoreSn;
                LoggerManager.Logger.Info("Client:" + _queue + "  DownloadMdvrFile ,route:" + routeKey + ",data:" + ConvertHelper.ConvertModelToJson(model));
                SendMessage(Constdefine.APPEXCHANGE, routeKey, msg);
            }
            catch (Exception ex)
            {
                LoggerManager.Logger.Error(ex);
            }
        }

        public void SendTakePictureCMD(TakePictureArgs model)
        {
            try
            {
                byte[] msg = ConvertHelper.ObjectToBytes(model);
                string routeKey = VideoRoute.TakePictureAppKey + model.Mdvr_Core_Sn;
                LoggerManager.Logger.Info("Client:" + _queue + "  TakePictureAppKey ,route:" + routeKey + ",data:" + ConvertHelper.ConvertModelToJson(model));
                SendMessage(Constdefine.APPEXCHANGE, routeKey, msg);
            }
            catch (Exception ex)
            {
                LoggerManager.Logger.Error(ex);
            }
        }

        private void OnVideoList(string key, byte[] bytes)
        {
            try
            {
                LoggerManager.Logger.Info("OnVideoList");
                var model = ConvertHelper.BytesToObject(bytes) as QueryServerFileListMessageResponse;

                if (DataManager.Users.ContainsKey(model.UserID))
                {
                    Dictionary<string, IMessageCallBackExt> dictionary = DataManager.Users[model.UserID];
                    List<string> sessions = new List<string>();
                    foreach (var sessionid in dictionary.Keys)
                    {
                        try
                        {
                            dictionary[sessionid].MessageCallBack(model);
                        }
                        catch (Exception)
                        {
                            sessions.Add(sessionid);
                        }
                    }

                    if (sessions.Count != 0)
                    {
                        Thread.Sleep(1000);
                        foreach (var item in sessions)
                        {
                            if (dictionary.ContainsKey(item))
                            {
                                try
                                {
                                    dictionary[item].MessageCallBack(model);
                                }
                                catch (Exception)
                                {

                                }
                            }
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                LoggerManager.Logger.Error(ex);
            }
        }

        private void OnTakePicture(string key, byte[] bytes)
        {
            try
            {
                LoggerManager.Logger.Info("OnTakePicture");
                var model = ConvertHelper.BytesToObject(bytes) as TakePictureMessageResponse;
                if (DataManager.Users.ContainsKey(model.UserID))
                {
                    Dictionary<string, IMessageCallBackExt> dictionary = DataManager.Users[model.UserID];
                    List<string> sessions = new List<string>();
                    foreach (var sessionid in dictionary.Keys)
                    {
                        try
                        {
                            dictionary[sessionid].MessageCallBack(model);
                        }
                        catch (Exception)
                        {
                            sessions.Add(sessionid);
                        }
                    }

                    if (sessions.Count != 0)
                    {
                        Thread.Sleep(1000);
                        foreach (var item in sessions)
                        {
                            if (dictionary.ContainsKey(item))
                            {
                                try
                                {
                                    dictionary[item].MessageCallBack(model);
                                }
                                catch (Exception)
                                {

                                }
                            }
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                LoggerManager.Logger.Error(ex);
            }
        }
    }
}
