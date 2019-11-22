﻿

//------------------------------------------------------------------------------
// <auto-generated>
//    此代码是根据模板生成的。
//
//    手动更改此文件可能会导致应用程序中发生异常行为。
//    如果重新生成代码，则将覆盖对此文件的手动更改。
// </auto-generated>
//------------------------------------------------------------------------------


namespace Gsafety.PTMS.DBEntity
{

using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;


public partial class PTMSEntities : DbContext
{
    public PTMSEntities()
        : base("name=PTMSEntities")
    {

    }

    protected override void OnModelCreating(DbModelBuilder modelBuilder)
    {
        throw new UnintentionalCodeFirstException();
    }


    public DbSet<ALM_911_DISPOSE> ALM_911_DISPOSE { get; set; }

    public DbSet<ALM_ALARM_DISPOSE> ALM_ALARM_DISPOSE { get; set; }

    public DbSet<ALM_ALARM_NOTE> ALM_ALARM_NOTE { get; set; }

    public DbSet<ALM_ALARM_RECORD> ALM_ALARM_RECORD { get; set; }

    public DbSet<ALT_BUSINESS_ALERT> ALT_BUSINESS_ALERT { get; set; }

    public DbSet<ALT_BUSINESS_ALERT_HANDLE> ALT_BUSINESS_ALERT_HANDLE { get; set; }

    public DbSet<ALT_DEVICE_ALERT> ALT_DEVICE_ALERT { get; set; }

    public DbSet<ALT_DEVICE_ALERT_CHECK> ALT_DEVICE_ALERT_CHECK { get; set; }

    public DbSet<BSC_CHAUFFEUR> BSC_CHAUFFEUR { get; set; }

    public DbSet<BSC_DEV_GPS> BSC_DEV_GPS { get; set; }

    public DbSet<BSC_DEV_SUITE> BSC_DEV_SUITE { get; set; }

    public DbSet<BSC_DEV_SUITE_PART> BSC_DEV_SUITE_PART { get; set; }

    public DbSet<BSC_DISTRICT> BSC_DISTRICT { get; set; }

    public DbSet<BSC_GEO_POI> BSC_GEO_POI { get; set; }

    public DbSet<BSC_INSTALLATION_STAFF> BSC_INSTALLATION_STAFF { get; set; }

    public DbSet<BSC_ORDER_CLIENT> BSC_ORDER_CLIENT { get; set; }

    public DbSet<BSC_SETUP_STATION> BSC_SETUP_STATION { get; set; }

    public DbSet<BSC_SETUPSTATION_USER> BSC_SETUPSTATION_USER { get; set; }

    public DbSet<BSC_VEHICLE> BSC_VEHICLE { get; set; }

    public DbSet<BSC_VEHICLE_CHAUFFEUR> BSC_VEHICLE_CHAUFFEUR { get; set; }

    public DbSet<BSC_VEHICLE_SPEEDCOLOR> BSC_VEHICLE_SPEEDCOLOR { get; set; }

    public DbSet<BSC_VEHICLE_TYPE> BSC_VEHICLE_TYPE { get; set; }

    public DbSet<CFG_APP_CONFIG> CFG_APP_CONFIG { get; set; }

    public DbSet<CFG_TRANSFER_MAPPING> CFG_TRANSFER_MAPPING { get; set; }

    public DbSet<LOG_ACCESS> LOG_ACCESS { get; set; }

    public DbSet<LOG_DATA> LOG_DATA { get; set; }

    public DbSet<LOG_MANAGER> LOG_MANAGER { get; set; }

    public DbSet<LOG_OPERATE> LOG_OPERATE { get; set; }

    public DbSet<LOG_VIDEO> LOG_VIDEO { get; set; }

    public DbSet<MDI_ALARM_VIDEO> MDI_ALARM_VIDEO { get; set; }

    public DbSet<MDI_DOWNLOAD_VIDEO> MDI_DOWNLOAD_VIDEO { get; set; }

    public DbSet<MDI_LIVE_VIDEO> MDI_LIVE_VIDEO { get; set; }

    public DbSet<MDI_PHOTOGRAPH> MDI_PHOTOGRAPH { get; set; }

    public DbSet<MTN_GPS_INSTALLATION_DETAIL> MTN_GPS_INSTALLATION_DETAIL { get; set; }

    public DbSet<MTN_INSTALLATION_AUDIT> MTN_INSTALLATION_AUDIT { get; set; }

    public DbSet<MTN_INSTALLATION_DETAIL> MTN_INSTALLATION_DETAIL { get; set; }

    public DbSet<MTN_MAINTAIN_APPLICATION> MTN_MAINTAIN_APPLICATION { get; set; }

    public DbSet<MTN_MAINTAIN_RECORD> MTN_MAINTAIN_RECORD { get; set; }

    public DbSet<MTN_MDVR_REGISTER> MTN_MDVR_REGISTER { get; set; }

    public DbSet<MTN_PART_AUDIT> MTN_PART_AUDIT { get; set; }

    public DbSet<MTN_VIDEO_UPLOAD_COMMAND> MTN_VIDEO_UPLOAD_COMMAND { get; set; }

    public DbSet<RUN_APP_MESSAGE> RUN_APP_MESSAGE { get; set; }

    public DbSet<RUN_APP_ONLINE_RECORD> RUN_APP_ONLINE_RECORD { get; set; }

    public DbSet<RUN_APPMESSAGE_VEHICLE> RUN_APPMESSAGE_VEHICLE { get; set; }

    public DbSet<RUN_FOUND_REGISTRY> RUN_FOUND_REGISTRY { get; set; }

    public DbSet<RUN_GPS_ONLINE_RECORD> RUN_GPS_ONLINE_RECORD { get; set; }

    public DbSet<RUN_GPS_STATUS_CHANGE> RUN_GPS_STATUS_CHANGE { get; set; }

    public DbSet<RUN_GPS_WORKING> RUN_GPS_WORKING { get; set; }

    public DbSet<RUN_LOST_REGISTRY> RUN_LOST_REGISTRY { get; set; }

    public DbSet<RUN_MDVR_MESSAGE> RUN_MDVR_MESSAGE { get; set; }

    public DbSet<RUN_MDVRMESSAGE_VEHICLE> RUN_MDVRMESSAGE_VEHICLE { get; set; }

    public DbSet<RUN_MOBILE_WORKING> RUN_MOBILE_WORKING { get; set; }

    public DbSet<RUN_MONITOR_GROUP> RUN_MONITOR_GROUP { get; set; }

    public DbSet<RUN_MONITOR_GROUP_VEHICLE> RUN_MONITOR_GROUP_VEHICLE { get; set; }

    public DbSet<RUN_SUITE_ONLINE_RECORD> RUN_SUITE_ONLINE_RECORD { get; set; }

    public DbSet<RUN_SUITE_STATUS_CHANGE> RUN_SUITE_STATUS_CHANGE { get; set; }

    public DbSet<RUN_SUITE_WORKING> RUN_SUITE_WORKING { get; set; }

    public DbSet<RUN_TAKEPICTURE> RUN_TAKEPICTURE { get; set; }

    public DbSet<RUN_USER_ONLINE> RUN_USER_ONLINE { get; set; }

    public DbSet<RUN_VEHICLE_LOCATION> RUN_VEHICLE_LOCATION { get; set; }

    public DbSet<RUN_VEHICLE_ONLINE_TIME> RUN_VEHICLE_ONLINE_TIME { get; set; }

    public DbSet<RUN_VIDEO_QUERY> RUN_VIDEO_QUERY { get; set; }

    public DbSet<TRF_COMMAND_PARAM> TRF_COMMAND_PARAM { get; set; }

    public DbSet<TRF_COMMAND_VEHICLE> TRF_COMMAND_VEHICLE { get; set; }

    public DbSet<TRF_FENCE> TRF_FENCE { get; set; }

    public DbSet<TRF_FENCE_QUEUE> TRF_FENCE_QUEUE { get; set; }

    public DbSet<TRF_ROUTE> TRF_ROUTE { get; set; }

    public DbSet<TRF_ROUTE_QUEUE> TRF_ROUTE_QUEUE { get; set; }

    public DbSet<TRF_SOFTWARE_VERSION> TRF_SOFTWARE_VERSION { get; set; }

    public DbSet<TRF_UPGRADE_RECORD> TRF_UPGRADE_RECORD { get; set; }

    public DbSet<USR_DEPARTMENT> USR_DEPARTMENT { get; set; }

    public DbSet<USR_FUNC_ITEM> USR_FUNC_ITEM { get; set; }

    public DbSet<USR_GUSER> USR_GUSER { get; set; }

    public DbSet<USR_ORGANIZATION> USR_ORGANIZATION { get; set; }

    public DbSet<USR_ORGANIZATION_USER> USR_ORGANIZATION_USER { get; set; }

    public DbSet<USR_ROLE> USR_ROLE { get; set; }

    public DbSet<USR_ROLE_FUNCS> USR_ROLE_FUNCS { get; set; }

    public DbSet<ALARM_UNHANDLED_VIEW> ALARM_UNHANDLED_VIEW { get; set; }

    public DbSet<BSC_GPS_VIEW> BSC_GPS_VIEW { get; set; }

    public DbSet<BSC_SUITE_VIEW> BSC_SUITE_VIEW { get; set; }

    public DbSet<BUSINESS_ALERT_UNHANDLED_VIEW> BUSINESS_ALERT_UNHANDLED_VIEW { get; set; }

    public DbSet<BUSINESS_ALERT_VIEW> BUSINESS_ALERT_VIEW { get; set; }

    public DbSet<DEVICEALERT_VECHILESER_VIEW> DEVICEALERT_VECHILESER_VIEW { get; set; }

    public DbSet<DISTRICT_LEVEL_VIEW> DISTRICT_LEVEL_VIEW { get; set; }

    public DbSet<INSTALL_STATISTICS_VIEW> INSTALL_STATISTICS_VIEW { get; set; }

    public DbSet<INSTALL_SUITE_INFO_VIEW> INSTALL_SUITE_INFO_VIEW { get; set; }

    public DbSet<INSTALLATION_VIEW> INSTALLATION_VIEW { get; set; }

    public DbSet<MNT_MAINTAINRECORD_UNFINISHED> MNT_MAINTAINRECORD_UNFINISHED { get; set; }

    public DbSet<STATUS_CHANGING_VIEW> STATUS_CHANGING_VIEW { get; set; }

    public DbSet<SUITE_SOFTWARE_VERSION_VIEW> SUITE_SOFTWARE_VERSION_VIEW { get; set; }

    public DbSet<SUITE_UPGRADE_RECORD_VIEW> SUITE_UPGRADE_RECORD_VIEW { get; set; }

    public DbSet<SUITE_VIEW> SUITE_VIEW { get; set; }

    public DbSet<TRF_FENCEQUEUE_DELETED> TRF_FENCEQUEUE_DELETED { get; set; }

    public DbSet<TRF_FENCEQUEUE_DELETEFENCEID> TRF_FENCEQUEUE_DELETEFENCEID { get; set; }

    public DbSet<TRF_FENCEQUEUE_INVALIDID> TRF_FENCEQUEUE_INVALIDID { get; set; }

    public DbSet<TRF_FENCEQUEUE_ONVEHICLE> TRF_FENCEQUEUE_ONVEHICLE { get; set; }

    public DbSet<TRF_FENCEQUEUE_SUCCEEDID> TRF_FENCEQUEUE_SUCCEEDID { get; set; }

    public DbSet<TRF_FENCEQUEUE_TOVEHICLE> TRF_FENCEQUEUE_TOVEHICLE { get; set; }

    public DbSet<TRF_ROUTEQUEUE_DELETED> TRF_ROUTEQUEUE_DELETED { get; set; }

    public DbSet<TRF_ROUTEQUEUE_DELETEID> TRF_ROUTEQUEUE_DELETEID { get; set; }

    public DbSet<TRF_ROUTEQUEUE_INVALIDID> TRF_ROUTEQUEUE_INVALIDID { get; set; }

    public DbSet<TRF_ROUTEQUEUE_ONVEHCILE> TRF_ROUTEQUEUE_ONVEHCILE { get; set; }

    public DbSet<TRF_ROUTEQUEUE_SUCCEEDID> TRF_ROUTEQUEUE_SUCCEEDID { get; set; }

    public DbSet<TRF_ROUTEQUEUE_TOVEHICLE> TRF_ROUTEQUEUE_TOVEHICLE { get; set; }

    public DbSet<VEHICLE_ONLINE_TIME_VIEW> VEHICLE_ONLINE_TIME_VIEW { get; set; }

    public DbSet<VEHICLE_STATUS_VIEW> VEHICLE_STATUS_VIEW { get; set; }

    public DbSet<VEHICLE_WORKING_VIEW> VEHICLE_WORKING_VIEW { get; set; }

    public DbSet<VEHICLEGPS_VIEW> VEHICLEGPS_VIEW { get; set; }

    public DbSet<ALM_ALARM_MAIL> ALM_ALARM_MAIL { get; set; }

    public DbSet<LOG_ERROR> LOG_ERROR { get; set; }

}

}

