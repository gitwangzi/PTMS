/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////Guid: 0a68efed-aa84-4309-93e6-0d7cd1749664      
///// clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
///// Machine Name: PC-SHIHS
///// Author: (ShiHongsheng)
/////======================================================================
///// Project Name: Gsafety.PTMS.Manager.ViewModels.LogManageViewModel
///// Project Description:    
/////Class Name: 
///// Class Version: v1.0.0.0
///// Create Time: 2013/10/09 00:00:00
/////Class Description:  
/////======================================================================
/////Modified Time:
/////Modified by:
/////Modified Description: 
/////======================================================================
using Gsafety.Common.Localization.Resource;
using System;
using System.Windows;

namespace Gsafety.PTMS.Manager.ViewModels.LogManageViewModel
{
    public class LogHelper
    {
        internal bool SeearchConditionValid(DateTime sd, DateTime ed)
        {
            bool result = false;
            if (sd.CompareTo(ed) > 0)
            {
                string timeError = StringResource.ResourceManager.GetString("TimeError");
                timeError = string.IsNullOrEmpty(timeError) ? "StrartTime can not be greater than EndTime !" : timeError;

                string warning = StringResource.ResourceManager.GetString("Warning");
                MessageBox.Show(timeError, string.IsNullOrEmpty(warning) ? "Warning !" : warning, MessageBoxButton.OK);
                result = false;
            }
            else
            {
                result = true;
            }
            return result;
        }

        internal void RomoteError()
        {
            string remoteServerError = StringResource.ResourceManager.GetString("RemoteServerError");
            remoteServerError = string.IsNullOrEmpty(remoteServerError) ? "Remote service errors !" : remoteServerError;
            string error = StringResource.ResourceManager.GetString("Error");
            global::System.Windows.MessageBox.Show(remoteServerError, string.IsNullOrEmpty(error) ? "Error !" : error, System.Windows.MessageBoxButton.OK);
        }

        /// <summary>
        /// Export excel
        /// </summary>
        internal void ExportExcel()
        {
            string tip = StringResource.ResourceManager.GetString("MONITOR_Notice");
            tip = string.IsNullOrEmpty(tip) ? "Tip !" : tip;
            string noprinter = StringResource.ResourceManager.GetString("NoPrinter");
            noprinter = string.IsNullOrEmpty(noprinter) ? "No printer available !" : noprinter;

            global::System.Windows.MessageBox.Show(noprinter, tip, System.Windows.MessageBoxButton.OK);
        }

    }
}
