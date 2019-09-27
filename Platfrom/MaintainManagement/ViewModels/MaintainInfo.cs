using Gsafety.Common.Localization.Resource;
using System.ComponentModel.DataAnnotations;

/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////Guid: 
///// clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
///// Machine Name: PC-SHIHS
///// Author: (Shihs)
/////======================================================================
///// Project Name: Gsafety.PTMS.Manager.ViewModels.LogManageViewModel
///// Project Description:    
/////Class Name: CarAlertLogViewModel
///// Class Version: v1.0.0.0
///// Create Time: 2013/11/22 00:00:00
/////Class Description:  
/////======================================================================
/////Modified Time: 2013/11/27 00:00:00
/////Modified by:
/////Modified Description: 
/////======================================================================

namespace Gsafety.PTMS.Maintain.ViewModels
{
    public class MaintainInfo
    {
        private string maintainer;
        private string vihcleId;
        private string suiteID;

        private string maintainTime;///维修时间
        [Display(Name = "Tiempo de último mantenimiento", GroupName = "Name", AutoGenerateField = false)]
        public string MaintainTime
        {
            get
            {
                string wxr = StringResource.ResourceManager.GetString("SUITE_Maintainer");
                string tjh = StringResource.ResourceManager.GetString("ALERT_SecuritySuitID");
                return string.Format("{0}     {1}：{2}     {3}：{4}     ", maintainTime, wxr, maintainer, tjh, suiteID);
            }
            set { maintainTime = value; }
        }

        [Display(Name = "Personal de mantenimiento", GroupName = "Name", AutoGenerateField = false)]
        public string Maintainer///维修人
        {
            get { return maintainer; }
            set { maintainer = value; }
        }

        [Display(Name = "Número de Kit de seguridad", GroupName = "Name", AutoGenerateField = false)]
        public string SuiteID///安全套件号
        {
            get { return suiteID; }
            set { suiteID = value; }
        }

        /*******************************************************************************************************/

        private string deviceName;///设备名称
        [Display(Name = "Nombre del dispositivo", GroupName = "Name", AutoGenerateField = false)]
        public string DeviceName
        {
            get { return deviceName; }
            set { deviceName = value; }
        }

        //private string cEIEC;
        //[Display(Name = "CEIEC主机号", GroupName = "Name", AutoGenerateField = false)]
        //public string CEIEC
        //{
        //    get { return cEIEC; }
        //    set { cEIEC = value; }
        //}

        //private string mDVR;
        //[Display(Name = "MDVR芯片号", GroupName = "Name", AutoGenerateField = false)]
        //public string MDVR
        //{
        //    get { return mDVR; }
        //    set { mDVR = value; }
        //}

        private string oldCode;///维修前编号
        [Display(Name = "No. Antes de mantenimiento", GroupName = "Name", AutoGenerateField = false)]
        public string OldCode
        {
            get { return oldCode; }
            set { oldCode = value; }
        }

        private string newCode;///维修后编号
        [Display(Name = "Numero de reparación", GroupName = "Name", AutoGenerateField = false)]
        public string NewCode
        {
            get { return newCode; }
            set { newCode = value; }
        }

        private bool isMaintained;///是否维修
        [Display(Name = "Si va a operar", GroupName = "Name", AutoGenerateField = false)]
        public bool IsMaintained
        {
            get { return isMaintained; }
            set { isMaintained = value; }
        }
        /// <summary>
        /// 车牌号
        /// </summary>
        [Display(Name = "Placa", GroupName = "Name", AutoGenerateField = false)]
        public string VihcleId
        {
            get { return vihcleId; }
            set { vihcleId = value; }
        }

        private string note;
        [Display(Name = "Note", GroupName = "Name", AutoGenerateField = false)]
        public string Note
        {
            get { return note; }
            set { note = value; }
        }
    }
}
