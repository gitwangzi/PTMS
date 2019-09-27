/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: a7b2c988-85ff-4447-99a1-642aerertyuv     
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: PC-DENGZL
/////                 Author: TEST(dengzl)
/////======================================================================
/////           Project Name: Gsafety.PTMS.Share
/////    Project Description:    
/////             Class Name: AntLogger
/////          Class Version: v1.0.0.0
/////            Create Time: 7/24/2013 5:33:48 PM
/////      Class Description:  
/////======================================================================
/////          Modified Time: 7/24/2013 5:33:48 PM
/////            Modified by:
/////   Modified Description: 
/////======================================================================
using Jounce.Core.Application;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Diagnostics;
using System.IO.IsolatedStorage;
using System.Net;
using System.Runtime.InteropServices.Automation;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace Gsafety.PTMS.Share
{
    [Export]
    public class AntLogger : ILogger
    {
        readonly string INFOMANTION = "Infomanton.txt";
        readonly string WARNING = "Warning.txt";
        readonly string ERROR = "Error.txt";
        private LogSeverity _SeverityLevel = LogSeverity.Information;
        private readonly Queue<Tuple<LogSeverity, string, string>> _Logs = new Queue<Tuple<LogSeverity, string, string>>();
        object _LockObject = new object();
        public void LogInforMession(string source, string message)
        {
            Log(LogSeverity.Information, source, message);
        }

        public void LogWaring(string source, string waring)
        {
            Log(LogSeverity.Warning, source, waring);
        }

        public void LogError(string source, string error)
        {
            Log(LogSeverity.Error, source, error);
        }

        public void LogException(string source, Exception exception)
        {
            if (exception != null)
            {
                Log(LogSeverity.Critical, source, exception);
            }
        }

        public void Log(LogSeverity serverity, string source, string message)
        {
            try
            {
                //if (serverity.GetHashCode() < _SeverityLevel.GetHashCode())
                if ((int)serverity < (int)_SeverityLevel) return;
                lock (_LockObject)
                {
                    _Logs.Enqueue(new Tuple<LogSeverity, string, string>(serverity, source, string.Format("{0},{1},{2}", source, DateTime.Now.ToString(), message)));
                }
            }
            catch (Exception ex)
            {
                //MessageBoxHelper.ShowDialog(ex.StackTrace);
            }
        }

        public void Log(LogSeverity severity, string source, Exception exception)
        {
            if (!Debugger.IsAttached || (int)severity < (int)_SeverityLevel)
            {
                return;
            }
            var stringbuilder = new StringBuilder();
            stringbuilder.Append(exception);
            var ex = exception.InnerException;
            while (ex != null)
            {
                stringbuilder.AppendFormat("{0}{1}", Environment.NewLine, ex);
                ex = ex.InnerException;
            }
            Log(severity, source, stringbuilder.ToString());
        }

        public void LogFormat(LogSeverity severity, string source, string messageTemplate, params object[] arguments)
        {
        }

        public void SetSeverity(LogSeverity minimumLevel)
        {
            _SeverityLevel = minimumLevel;
        }

        private void CreateDirectory(string DirectoryName)
        {
            using (IsolatedStorageFile isoStore = IsolatedStorageFile.GetUserStoreForApplication())
            {
                if (isoStore.DirectoryExists(DirectoryName))
                    return;
                else
                    isoStore.DeleteDirectory(DirectoryName);
            }
        }

        private void CreateFile(string fileName)
        {
            using (IsolatedStorageFile isoStore = IsolatedStorageFile.GetUserStoreForApplication())
            {
                if (isoStore.FileExists(fileName))
                    return;
                else
                    isoStore.CreateFile(fileName);
            }
        }


        System.Threading.Thread WriteLogThread;
        public AntLogger()
        {
            WriteLogThread = new System.Threading.Thread(WriteLog);
            WriteLogThread.IsBackground = true;
            WriteLogThread.Start();
        }

        public void EndLogger()
        {
            if (WriteLogThread.IsAlive)
            {
                //WriteLogThread.Abort();
            }
        }



        private void WriteLog()
        {
            while (true)
            {
                if (_Logs.Count > 0)
                {
                    Tuple<LogSeverity, string, string>[] log;
                    lock (_LockObject)
                    {
                        log = new Tuple<LogSeverity, string, string>[_Logs.Count];
                        _Logs.CopyTo(log, 0);
                        _Logs.Clear();
                    }

                    if (log.Length > 0)
                    {
                        using (IsolatedStorageFile isoStore = IsolatedStorageFile.GetUserStoreForApplication())
                        {
                            try
                            {
                                Int64 curAvail = isoStore.AvailableFreeSpace;
                                Int64 spaceToAdd = 5242880;
                                if (curAvail < spaceToAdd)
                                    isoStore.IncreaseQuotaTo(isoStore.Quota + spaceToAdd);
                                //if (!isoStore.IncreaseQuotaTo(isoStore.Quota + spaceToAdd))
                                //{
                                //    isoStore.Remove();
                                //}
                                string directoryName = DateTime.Now.Date.ToString("yyyy-MM-dd");
                                if (!isoStore.DirectoryExists(directoryName))
                                    isoStore.CreateDirectory(directoryName);
                                IsolatedStorageFileStream streamInfor = new IsolatedStorageFileStream(string.Format("{0}/{1}", directoryName, INFOMANTION), System.IO.FileMode.OpenOrCreate, System.IO.FileAccess.Write, isoStore);
                                System.IO.TextWriter writerInfor = new System.IO.StreamWriter(streamInfor);
                                IsolatedStorageFileStream streamWaring = new IsolatedStorageFileStream(string.Format("{0}/{1}", directoryName, WARNING), System.IO.FileMode.OpenOrCreate, System.IO.FileAccess.Write, isoStore);
                                System.IO.TextWriter writerWaring = new System.IO.StreamWriter(streamWaring);
                                IsolatedStorageFileStream streamError = new IsolatedStorageFileStream(string.Format("{0}/{1}", directoryName, ERROR), System.IO.FileMode.OpenOrCreate, System.IO.FileAccess.Write, isoStore);
                                System.IO.TextWriter writerError = new System.IO.StreamWriter(streamError);
                                for (int i = 0; i < log.Length; i++)
                                {
                                    switch (log[i].Item1)
                                    {
                                        case LogSeverity.Critical:
                                        case LogSeverity.Information:
                                            writerInfor.WriteLine(log[i].Item3);
                                            break;
                                        case LogSeverity.Warning:
                                            writerWaring.WriteLine(log[i].Item3);
                                            break;
                                        case LogSeverity.Error:
                                        case LogSeverity.Verbose:
                                            writerError.WriteLine(log[i].Item3);
                                            break;
                                        default:
                                            break;
                                    }
                                }
                                writerError.Close();
                                writerInfor.Close();
                                writerWaring.Close();
                                streamError.Close();
                                streamInfor.Close();
                                streamWaring.Close();
                            }
                            catch
                            {
                                isoStore.Remove();
                            }
                        }
                    }
                }
                System.Threading.Thread.Sleep(1000);
            }
        }
    }
}
