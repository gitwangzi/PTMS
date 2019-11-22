/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: 129ad430-b1b0-43f8-aaa4-79345e4ebfbd      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: PC-GUOH
/////                 Author: TEST(guoh)
/////======================================================================
/////           Project Name: Gsafety.PTMS.Alert.Repository
/////    Project Description:    
/////             Class Name: BusinessAlertRespository
/////          Class Version: v1.0.0.0
/////            Create Time: 8/31/2013 14:40:56 
/////      Class Description:  
/////======================================================================
/////          Modified Time: 8/31/2013 14:40:56 
/////            Modified by:
/////   Modified Description: 
/////======================================================================
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gsafety.PTMS.Base.Contract.Data;
using Gsafety.PTMS.BaseInformation.Contract.Data;
using Gsafety.PTMS.DBEntity;
using Gsafety.PTMS.Message.Contract.Data;
using Gsafety.Common.Logging;
using System.Transactions;
using Gsafety.PTMS.Common.Data;

namespace Gsafety.PTMS.Alert.Repository
{
    public class BusinessAlertRespository
    {
        public BusinessAlertRespository()
        { }

        public bool AddBusinessAlert(PTMSEntities context, BusinessAlertEx model)
        {
            bool ret = false;
            TransactionOptions optons = new TransactionOptions();
            optons.IsolationLevel = IsolationLevel.ReadUncommitted;
            using (var scope = new TransactionScope(TransactionScopeOption.Required, optons))
            {
                try
                {
                    ALT_BUSINESS_ALERT entity = new ALT_BUSINESS_ALERT();
                    entity.ID = model.Id;
                    entity.VEHICLE_ID = model.VehicleId;
                    entity.MDVR_CORE_SN = model.MdvrCoreId;
                    entity.DISTRICT_CODE = model.DistrictCode;
                    entity.SUITE_INFO_ID = model.SuiteInfoID;
                    entity.ALERT_TYPE = (short)model.AlertType;
                    entity.LONGITUDE = model.Longitude;
                    entity.LATITUDE = model.Latitude;
                    entity.DIRECTION = model.Direction;
                    entity.SPEED = model.Speed;
                    entity.ALERT_TIME = model.AlertTime;
                    if (model.GpsTime != DateTime.MinValue)
                    {
                        entity.GPS_TIME = model.GpsTime;
                    }
                    entity.SUITE_STATUS = (short)model.SuiteStatus;
                    entity.STATUS = 1;//not handle
                    entity.GPS_VALID = model.GpsValid;
                    entity.CREATE_TIME = DateTime.Now.ToUniversalTime();
                    entity.CLIENT_ID = model.ClientId;
                    context.ALT_BUSINESS_ALERT.Add(entity);
                    context.SaveChanges();

                    scope.Complete();

                    ret = true;
                }
                catch (Exception ex)
                {
                    //LoggerManager.Logger.Error(ex);
                    LoggerManager.Logger.Error("********\r\n" + DateTime.Now + ":An exception occurred while business alert information into the database。\r\n" + "Method:Gsafety.PTMS.Alert.Repository.AddBusinessAlert;\r\n" + ex.ToString() + "\r\n********\r\n");
                }
            }

            return ret;
        }
    }
}
