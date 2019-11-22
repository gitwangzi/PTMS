using Gsafety.Common.Logging;
using Gsafety.Common.Util;
using Gsafety.PTMS.Common.Data;
using Gsafety.PTMS.Common.Enum;
using Gsafety.PTMS.Message.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gsafety.PTMS.MessageLib
{
    public partial class MessageService
    {
        public void MonitorVehicle(string usertoken, List<string> vechiles)
        {
            try
            {
                LoggerManager.Logger.Info("MonitorVehicle");
                IMessageCallBackExt callback = DataManager.SocketClients[usertoken];
                if (DataManager.Vehicles == null)
                {
                    DataManager.Vehicles = new Dictionary<string, Dictionary<string, IMessageCallBackExt>>();
                }
                lock (DataManager.Vehicles)
                {
                    foreach (string item in vechiles)
                    {
                        if (!DataManager.Vehicles.ContainsKey(item))
                        {
                            DataManager.Vehicles.Add(item, new Dictionary<string, IMessageCallBackExt>());
                        }

                        Dictionary<string, IMessageCallBackExt> dicvehicles = DataManager.Vehicles[item];

                        dicvehicles.Add(usertoken, callback);
                    }
                }
            }
            catch (Exception ex)
            {
                LoggerManager.Logger.Error(ex);
            }
        }

        public void UnMonitorVehicle(string usertoken, List<string> vechiles)
        {
            try
            {
                LoggerManager.Logger.Info("UnMonitorVehicle");
                lock (DataManager.Vehicles)
                {
                    foreach (string item in vechiles)
                    {
                        Dictionary<string, IMessageCallBackExt> dicvehicles = DataManager.Vehicles[item];

                        dicvehicles.Remove(usertoken);
                    }
                }
            }
            catch (Exception ex)
            {
                LoggerManager.Logger.Error(ex);
            }
        }

        private void OnSuiteGPS(byte[] bytes, string key)
        {
            try
            {
                LoggerManager.Logger.Info("OnSuiteGPS");
                string str = System.Text.UnicodeEncoding.UTF8.GetString(bytes);
                LoggerManager.Logger.Info(str);
                //json -> entity
                GpsInfo gpsinfo = JsonHelper.FromJsonString<Gsafety.PTMS.Common.Data.GpsInfo>(str);

                if (gpsinfo != null)
                {
                    GPS gps = new GPS();
                    gps.AlarmFlag = gpsinfo.AlarmFlag;
                    gps.Direction = gpsinfo.Direction;
                    gps.Status = gpsinfo.Status;
                    gps.Valid = gpsinfo.Valid;
                    gps.Height = gpsinfo.Height;
                    gps.Latitude = gpsinfo.Latitude;
                    gps.Longitude = gpsinfo.Longitude;
                    gps.Speed = gpsinfo.Speed;
                    gps.UID = gpsinfo.UID;
                    gps.VehicleId = gpsinfo.VehicleId;
                    gps.Source = (short)GPSSourceEnum.Suite;
                    gps.GpsTime = DateTime.Parse(gpsinfo.GpsTime);


                    if (DataManager.Vehicles != null && DataManager.Vehicles.ContainsKey(gps.VehicleId))
                    {
                        Dictionary<string, IMessageCallBackExt> dictionary = DataManager.Vehicles[gps.VehicleId];

                        List<string> keys = new List<string>();
                        foreach (var item in dictionary.Keys)
                        {
                            try
                            {
                                dictionary[item].MessageCallBack(gps);
                                LoggerManager.Logger.Info("Send GPS Info To :" + item);
                            }
                            catch (Exception ex)
                            {
                                LoggerManager.Logger.Error(item);
                                LoggerManager.Logger.Error(ex);
                                keys.Add(item);
                            }
                        }

                        if (keys.Count != 0)
                        {
                            lock (DataManager.Vehicles)
                            {
                                dictionary = DataManager.Vehicles[gps.VehicleId];
                                foreach (string item in keys)
                                {
                                    dictionary.Remove(item);
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

        private void OnGPSGPS(byte[] bytes, string key)
        {
            try
            {
                LoggerManager.Logger.Info("OnGPSGPS");
                string str = System.Text.UnicodeEncoding.UTF8.GetString(bytes);
                LoggerManager.Logger.Info(str);
                //json -> entity
                GpsInfo gpsinfo = JsonHelper.FromJsonString<Gsafety.PTMS.Common.Data.GpsInfo>(str);

                if (gpsinfo != null)
                {
                    GPS gps = new GPS();
                    gps.AlarmFlag = gpsinfo.AlarmFlag;
                    gps.Direction = gpsinfo.Direction;
                    gps.Status = gpsinfo.Status;
                    gps.Valid = gpsinfo.Valid;
                    gps.Height = gpsinfo.Height;
                    gps.Latitude = gpsinfo.Latitude;
                    gps.Longitude = gpsinfo.Longitude;
                    gps.Speed = gpsinfo.Speed;
                    gps.UID = gpsinfo.UID;
                    gps.VehicleId = gpsinfo.VehicleId;
                    gps.Source = (short)GPSSourceEnum.GPS;
                    gps.GpsTime = DateTime.Parse(gpsinfo.GpsTime);


                    if (DataManager.Vehicles != null && DataManager.Vehicles.ContainsKey(gps.VehicleId))
                    {
                        Dictionary<string, IMessageCallBackExt> dictionary = DataManager.Vehicles[gps.VehicleId];

                        List<string> keys = new List<string>();
                        foreach (var item in dictionary.Keys)
                        {
                            try
                            {
                                dictionary[item].MessageCallBack(gps);
                            }
                            catch (Exception ex)
                            {
                                LoggerManager.Logger.Error(item);
                                LoggerManager.Logger.Error(ex);
                                keys.Add(item);
                            }
                        }

                        if (keys.Count != 0)
                        {
                            lock (DataManager.Vehicles)
                            {
                                dictionary = DataManager.Vehicles[gps.VehicleId];
                                foreach (string item in keys)
                                {
                                    dictionary.Remove(item);
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

        private void OnMobileGPS(byte[] bytes, string key)
        {
            try
            {
                LoggerManager.Logger.Info("OnMobileGPS");
                string str = System.Text.UnicodeEncoding.UTF8.GetString(bytes);
                //json -> entity
                GpsInfo gpsinfo = JsonHelper.FromJsonString<Gsafety.PTMS.Common.Data.GpsInfo>(str);
                LoggerManager.Logger.Info(str);
                if (gpsinfo != null)
                {
                    GPS gps = new GPS();
                    gps.AlarmFlag = gpsinfo.AlarmFlag;
                    gps.Direction = gpsinfo.Direction;
                    gps.Status = gpsinfo.Status;
                    gps.Valid = gpsinfo.Valid;
                    gps.Height = gpsinfo.Height;
                    gps.Latitude = gpsinfo.Latitude;
                    gps.Longitude = gpsinfo.Longitude;
                    gps.Speed = gpsinfo.Speed;
                    gps.UID = gpsinfo.UID;
                    gps.VehicleId = gpsinfo.VehicleId;
                    gps.Source = (short)GPSSourceEnum.Mobile;
                    gps.GpsTime = DateTime.Parse(gpsinfo.GpsTime);


                    if (DataManager.Vehicles != null && DataManager.Vehicles.ContainsKey(gps.VehicleId))
                    {
                        Dictionary<string, IMessageCallBackExt> dictionary = DataManager.Vehicles[gps.VehicleId];

                        List<string> keys = new List<string>();
                        foreach (var item in dictionary.Keys)
                        {
                            try
                            {
                                dictionary[item].MessageCallBack(gps);
                            }
                            catch (Exception ex)
                            {
                                LoggerManager.Logger.Error(item);
                                LoggerManager.Logger.Error(ex);
                                keys.Add(item);
                            }
                        }

                        if (keys.Count != 0)
                        {
                            lock (DataManager.Vehicles)
                            {
                                dictionary = DataManager.Vehicles[gps.VehicleId];
                                foreach (string item in keys)
                                {
                                    dictionary.Remove(item);
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
