using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using Gsafety.Common.Localization;
using System.Linq.Expressions;
using Gsafety.Common.Logging;
using System.Reflection;
using System.Configuration;

namespace Gsafety.PTMS.Report.Repository
{
    public class ReportStatisticsRepository
    {
        #region Fields
        StringResourceReader reader = new StringResourceReader();
        OracleHelp oracleHelp = new OracleHelp();
        double dtemp = 0;


        #endregion
        #region ctor
        public ReportStatisticsRepository(ReportWhereInfo whereInfo)
        {
            WhereInfo = whereInfo;
            DateFormat = whereInfo.DataFromat;
            //System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo(WhereInfo.Language);
            System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo(WhereInfo.Language);
        }
        public ReportStatisticsRepository(string whereInfo)
        {
            WhereInfo = JsonHelper.FromJsonString<ReportWhereInfo>(whereInfo);
            DateFormat = WhereInfo.DataFromat;
            //System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo(WhereInfo.Language);
            System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo(WhereInfo.Language);
        }
        #endregion
        #region Attributes
        public ReportWhereInfo WhereInfo
        {
            get;
            set;
        }
        public string DateFormat { get; set; }
        public DataTable DataSource
        {
            get;
            set;
        }
        #endregion
        #region Vehicle
        #region Vehicle Alarm
        /// <summary>
        /// Vehicle Alarm
        /// </summary>
        /// <param name="whereInfo"></param>
        /// <returns></returns>
        public DataTable GetDataForAlarmReport()
        {
            try
            {
                string str = CreateWhereInfo(WhereInfo, "binfo.District_Code", "binfo.create_time");
                StringBuilder builder = new StringBuilder();
                builder.Append(" select t.createTime,sum(decode(t.Alarm_flag,'1',t.counts,null))T , sum(decode(t.Alarm_flag,'0',t.counts,null))F,sum(t.counts)ZONGSHUGG  From ");
                builder.Append(" (  select to_char(binfo.create_time,'yyyy-mm-dd') createTime, gong.Alarm_flag, count(*) counts  From  ECU911_DISPOSE gong  join  ALARM_RECORD binfo  on  gong.alarm_id=binfo.id ");
                builder.Append(str);
                builder.Append(" group by to_char(binfo.create_time,'yyyy-mm-dd'), gong.Alarm_flag  ");
                builder.Append(" order by createTime  )t   group by t.createTime order by t.createTime desc ");

                //builder.Append(" select ");
                //builder.Append(" a.*, ");
                //builder.Append(" (decode(a.F,null,0,a.F)+ decode(a.T,null,0,a.T)) ZONGSHUGG ");
                //builder.Append(" from ");
                //builder.Append(" ( ");
                //builder.Append(" select ");
                //builder.Append(" t.createTime,sum(decode(t.Alarm_flag,'1',t.counts,null))\"T\" , ");
                //builder.Append(" sum(decode(t.Alarm_flag,'0',t.counts,null))\"F\"  ");
                //builder.Append(" From ");
                //builder.Append(" ( ");
                //builder.Append(" select ");
                //builder.Append(" to_char(gong.DISPOSE_TIME,'yyyy-mm-dd') createTime,  ");
                //builder.Append(" gong.Alarm_flag, ");
                //builder.Append(" count(*) counts ");
                //builder.Append(" From ");
                //builder.Append(" ECU911_DISPOSE gong   ");
                //builder.Append(" join ");
                //builder.Append(" ALARM_RECORD binfo ");
                //builder.Append(" on ");
                //builder.Append(" gong.alarm_id=binfo.id ");
                //builder.Append(str);
                //builder.Append(" group by to_char(gong.DISPOSE_TIME,'yyyy-mm-dd'), ");
                //builder.Append(" gong.Alarm_flag  ");
                //builder.Append(" order by createTime  ");
                //builder.Append("  )t  ");
                //builder.Append("  group by t.createTime ");
                //builder.Append("  order by t.createTime desc) a ");

                DataTable dtGongGong = oracleHelp.ExecuteDataTable(builder.ToString());

                builder = new StringBuilder();
                builder.Append(" select a.*, (decode(a.JC1, null, 0, a.JC1) + decode(a.XF2, null, 0, a.XF2) +  decode(a.FX3, null, 0, a.FX3) + decode(a.JT4, null, 0, a.JT4) +  decode(a.YL5, null, 0, a.YL5) + decode(a.JD6, null, 0, a.JD6) +  decode(a.SZ7, null, 0, a.SZ7)) TrueZS911 ");
                builder.Append(" from (select t.createTime,  sum(decode(t.INCIDENT_TYPE, '1', t.counts, null)) \"JC1\",  sum(decode(t.INCIDENT_TYPE, '7', t.counts, null)) \"XF2\",  sum(decode(t.INCIDENT_TYPE, '2', t.counts, null)) \"FX3\", sum(decode(t.INCIDENT_TYPE, '3', t.counts, null)) \"JT4\",  sum(decode(t.INCIDENT_TYPE, '4', t.counts, null)) \"YL5\",  sum(decode(t.INCIDENT_TYPE, '5', t.counts, null)) \"JD6\",  sum(decode(t.INCIDENT_TYPE, '6', t.counts, null)) \"SZ7\",  sum(counts1) False911 ");
                builder.Append(" From (select to_char(binfo.create_time, 'yyyy-mm-dd') createTime,  jiu11.INCIDENT_TYPE, jiu11.alarm_flag, ");
                builder.Append("   count(case  when jiu11.alarm_flag = 1 then 1 else null end) counts,  count(case when jiu11.alarm_flag = 0 then 1 else null end) counts1  From ECU911_DISPOSE jiu11 join ALARM_DISPOSE gong on  jiu11.alarm_id = gong.alarm_id  and jiu11.alarm_flag in (0, 1)  join ALARM_RECORD binfo on binfo.id=jiu11.alarm_id ");
                builder.Append(str);
                builder.Append("   group by binfo.create_time,  jiu11.INCIDENT_TYPE,  jiu11.alarm_flag  order by jiu11.INCIDENT_TYPE) t  group by t.createTime order by t.createTime desc) a ");

                //builder.Append(" select ");
                //builder.Append(" a.*, ");
                //builder.Append(" (decode(a.JC1, null, 0, a.JC1) + decode(a.XF2, null, 0, a.XF2) + ");
                //builder.Append(" decode(a.FX3, null, 0, a.FX3) + decode(a.JT4, null, 0, a.JT4) + ");
                //builder.Append(" decode(a.YL5, null, 0, a.YL5) + decode(a.JD6, null, 0, a.JD6) + ");
                //builder.Append(" decode(a.SZ7, null, 0, a.SZ7)) TrueZS911 ");
                //builder.Append(" from (select t.createTime, ");
                //builder.Append(" sum(decode(t.INCIDENT_TYPE, '1', t.counts, null)) \"JC1\", ");
                //builder.Append(" sum(decode(t.INCIDENT_TYPE, '2', t.counts, null)) \"XF2\", ");
                //builder.Append(" sum(decode(t.INCIDENT_TYPE, '3', t.counts, null)) \"FX3\", ");
                //builder.Append(" sum(decode(t.INCIDENT_TYPE, '4', t.counts, null)) \"JT4\", ");
                //builder.Append(" sum(decode(t.INCIDENT_TYPE, '5', t.counts, null)) \"YL5\", ");
                //builder.Append(" sum(decode(t.INCIDENT_TYPE, '6', t.counts, null)) \"JD6\", ");
                //builder.Append(" sum(decode(t.INCIDENT_TYPE, '7', t.counts, null)) \"SZ7\", ");
                //builder.Append(" sum(counts1) False911 ");
                //builder.Append(" From (select to_char(gong.dispose_time, 'yyyy-mm-dd') createTime, ");
                //builder.Append(" jiu11.INCIDENT_TYPE, ");
                //builder.Append(" jiu11.alarm_flag, ");
                //builder.Append("   count(case ");
                //builder.Append(" when jiu11.alarm_flag = 1 then 1 else null end) counts,");
                //builder.Append("  count(case when jiu11.alarm_flag = 0 then 1 else null end) counts1 ");
                //builder.Append(" From ECU911_DISPOSE jiu11 join ALARM_DISPOSE gong on ");
                //builder.Append(" jiu11.alarm_id = gong.alarm_id ");
                //builder.Append(" and jiu11.alarm_flag in (0, 1) ");
                //builder.Append(" join ALARM_RECORD binfo on binfo.id=jiu11.alarm_id ");
                //builder.Append(str);
                //builder.Append("   group by gong.dispose_time, ");
                //builder.Append(" jiu11.INCIDENT_TYPE, ");
                //builder.Append("  jiu11.alarm_flag ");
                //builder.Append(" order by jiu11.INCIDENT_TYPE) t ");
                //builder.Append(" group by t.createTime ");
                //builder.Append(" order by t.createTime desc) a ");
                DataTable tb911 = oracleHelp.ExecuteDataTable(builder.ToString());
                DataSource = MergeDataTable(dtGongGong, tb911);

                FillZeroData(DataSource);

                return DataSource;
            }
            catch (Exception ex)
            {
                LoggerManager.Logger.Error(ex);
                throw;
            }
        }

        public DataTable GetDataForAlarmStatisticsReport()
        {
            try
            {
                StringBuilder builder = new StringBuilder();

                builder.Append("(select ALM_ALARM_RECORD.id,ALM_ALARM_RECORD.Vehicle_Id,ALM_ALARM_RECORD.District_Code,1 AlarmNum, alm_alarm_record.gps_time,alm_alarm_record.source, ALM_ALARM_DISPOSE.ALARM_FLAG, alm_911_dispose.is_transfer from ALM_ALARM_RECORD  left join ALM_ALARM_DISPOSE on ALM_ALARM_DISPOSE.Alarm_Id=alm_alarm_record.id left join alm_911_dispose on alm_911_dispose.alarm_id=alm_alarm_record.id where ALM_ALARM_RECORD.client_id='");
                builder.Append(WhereInfo.ClientID);
                builder.Append("' and GPS_TIME>='");
                builder.Append(WhereInfo.BeginTime.ToString("yyyy-MM-dd HH:mm:ss"));
                builder.Append("' and GPS_TIME<='");
                builder.Append(WhereInfo.EndTime.ToString("yyyy-MM-dd HH:mm:ss"));
                builder.Append("'");

                if (WhereInfo.City != null)
                {
                    builder.Append("  and district_code like '");
                    builder.Append(WhereInfo.City);
                    builder.Append("%'");
                }
                else
                {
                    if (WhereInfo.Province != null)
                    {
                        builder.Append("  and district_code like '");
                        builder.Append(WhereInfo.Province);
                        builder.Append("%'");

                    }
                }


                if (WhereInfo.AlarmSource.HasValue)
                {
                    builder.Append("  and Source =");
                    builder.Append(WhereInfo.AlarmSource.Value);
                }

                builder.Append(") t1");
                string innertext = builder.ToString();
                builder.Clear();

                builder.Append("select t1.id,t1.vehicle_id vehicleid ,t1.gps_time,substring(t1.District_code,1,2) province,t1.District_code,");

                builder.Append("ltrim(DATEPART(yyyy,GPS_TIME)) year, ltrim(DATEPART(yyyy,GPS_TIME))+'-'+ltrim(DATEPART(mm,GPS_TIME)) month,ltrim(DATEPART(yyyy,GPS_TIME))+'-'+ltrim(DATEPART(wk,GPS_TIME)) week, ltrim(DATEPART(yyyy,GPS_TIME))+'-'+ltrim(DATEPART(mm,GPS_TIME))+'-'+ltrim(DATEPART(dd,GPS_TIME)) day,");
                
                builder.Append("v.orgnization_id, v.vehicle_type vehicleType,t1.AlarmNum, t1.Source,(CASE when t1.is_transfer=1 then 1 else 0 END) is_transfer,(CASE when t1.ALARM_FLAG=1 then 1 else 0 END) TrueAlarm, (CASE when t1.ALARM_FLAG=0 then 1 else 0 END) FalseAlarm from");
                builder.Append(innertext);

                builder.Append(",bsc_vehicle v where  v.vehicle_id=t1.vehicle_id");

                if (!string.IsNullOrEmpty(WhereInfo.VehicleType))
                {
                    builder.Append("  and v.vehicle_type = '");
                    builder.Append(WhereInfo.VehicleType);
                    builder.Append("'");
                }

                builder.Append(" and v.orgnization_id in (");
                builder.Append(WhereInfo.OrgCode);
                builder.Append(") ");
                innertext = builder.ToString();
                builder.Clear();
                if (WhereInfo.GroupMode == 1)
                {
                    builder.Append("select year project,sum(AlarmNum) AlarmNum, sum(TrueAlarm) TrueAlarm,sum(FalseAlarm) FalseAlarm,sum(is_transfer) is_transfer from (");
                    builder.Append(innertext);
                    builder.Append(") t2 group by year");
                }
                else if (WhereInfo.GroupMode == 2)
                {
                    builder.Append("select month project,sum(AlarmNum) AlarmNum, sum(TrueAlarm) TrueAlarm,sum(FalseAlarm) FalseAlarm,sum(is_transfer) is_transfer from (");
                    builder.Append(innertext);
                    builder.Append(") t2 group by month");
                }
                else if (WhereInfo.GroupMode == 3)
                {
                    builder.Append("select week project,sum(AlarmNum) AlarmNum, sum(TrueAlarm) TrueAlarm,sum(FalseAlarm) FalseAlarm,sum(is_transfer) is_transfer from (");
                    builder.Append(innertext);
                    builder.Append(") t2 group by week");
                }
                else if (WhereInfo.GroupMode == 4)
                {
                    builder.Append("select day project,sum(AlarmNum) AlarmNum, sum(TrueAlarm) TrueAlarm,sum(FalseAlarm) FalseAlarm,sum(is_transfer) is_transfer from (");
                    builder.Append(innertext);
                    builder.Append(") t2 group by day");
                }
                else if (WhereInfo.GroupMode == 5)
                {
                    builder.Append("select name project,sum(AlarmNum) AlarmNum, sum(TrueAlarm) TrueAlarm,sum(FalseAlarm) FalseAlarm,sum(is_transfer) is_transfer from (");
                    builder.Append(innertext);
                    builder.Append(") t2 left join usr_organization on t2.orgnization_id=usr_organization.id group by name");
                }
                else if (WhereInfo.GroupMode == 6)
                {
                    builder.Append("select province,sum(AlarmNum) AlarmNum, sum(TrueAlarm) TrueAlarm,sum(FalseAlarm) FalseAlarm,sum(is_transfer) is_transfer from (");
                    builder.Append(innertext);
                    builder.Append(") t2 group by province");
                    innertext = builder.ToString();
                    builder.Clear();
                    builder.Append("select t3.*, d.name as Project from (");
                    builder.Append(innertext);
                    builder.Append(") t3 inner join bsc_district d on d.code=t3.province");
                }
                else if (WhereInfo.GroupMode == 7)
                {
                    builder.Append("select district_code ,sum(AlarmNum) AlarmNum, sum(TrueAlarm) TrueAlarm,sum(FalseAlarm) FalseAlarm,sum(is_transfer) is_transfer from (");
                    builder.Append(innertext);
                    builder.Append(") t2 group by district_code");
                    innertext = builder.ToString();
                    builder.Clear();
                    builder.Append("select t3.*, d.name as Project from (");
                    builder.Append(innertext);
                    builder.Append(") t3 inner join bsc_district d on d.code=t3.district_code");
                }

                else if (WhereInfo.GroupMode == 8)
                {
                    builder.Append("select vehicleid project,sum(AlarmNum) AlarmNum, sum(TrueAlarm) TrueAlarm,sum(FalseAlarm) FalseAlarm,sum(is_transfer) is_transfer from (");
                    builder.Append(innertext);
                    builder.Append(") t2 group by vehicleid");
                  
                }

                builder.Append(" order by Project");


                DataTable dtAlarm = oracleHelp.ExecuteDataTable(builder.ToString());

                return dtAlarm;
            }
            catch (Exception ex)
            {
                LoggerManager.Logger.Error(ex);
                throw;
            }
        }

        private void FillZeroData(DataTable table)
        {
            foreach (DataRow row in table.Rows)
            {
                for (int i = 0; i < table.Columns.Count; i++)
                {
                    if (row[i] == DBNull.Value)
                    {
                        row[i] = 0;
                    }
                }
            }
        }

        private DataTable RebuildDataTable(DataTable source, DateTime start, DateTime end, string columnname, string dataformat)
        {
            DateTime temp = start;
            while (temp < end)
            {
                string datavalue = temp.ToString(dataformat);
                bool found = false;
                foreach (DataRow row in source.Rows)
                {
                    if (row[columnname].ToString() == datavalue)
                    {
                        found = true;
                        break;
                    }
                }

                if (!found)
                {
                    DataRow row = source.NewRow();
                    row[columnname] = datavalue;
                    source.Rows.Add(row);
                }

                temp = temp.AddDays(1);
            }

            source.DefaultView.Sort = columnname + " desc";
            DataTable temptable = source.Copy();
            temptable.Rows.Clear();


            foreach (DataRowView item in source.DefaultView)
            {
                DataRow row = temptable.NewRow();
                for (int i = 0; i < source.Columns.Count; i++)
                {
                    row[i] = item.Row[i];
                }

                temptable.Rows.Add(row);
            }

            return temptable;
        }
        /// <summary>
        /// Merge
        /// </summary>
        /// <param name="dtGongGong"></param>
        /// <param name="dt911"></param>
        /// <returns></returns>
        private DataTable MergeDataTable(DataTable dtGongGong, DataTable dt911)
        {
            try
            {
                List<string> list = new List<string>();
                foreach (DataColumn column in dt911.Columns)
                {
                    if (column.ColumnName != "CREATETIME")
                    {
                        DataColumn column2 = new DataColumn
                        {
                            ColumnName = column.ColumnName,
                            DataType = column.DataType
                        };
                        dtGongGong.Columns.Add(column2);
                        list.Add(column.ColumnName);
                    }
                }
                string key = null;
                dt911.PrimaryKey = new DataColumn[] { dt911.Columns[0] };
                for (int i = 0; i < dtGongGong.Rows.Count; i++)
                {
                    key = dtGongGong.Rows[i][0].ToString();
                    DataRow row = dt911.Rows.Find(key);
                    if (row != null)
                    {
                        foreach (string item in list)
                        {
                            dtGongGong.Rows[i][item] = row[item];
                        }
                    }
                }

                DataTable table = RebuildDataTable(dtGongGong, WhereInfo.BeginTime, WhereInfo.EndTime, "createTime", "yyyy-MM-dd");

                DataRow tempRow = table.NewRow();
                tempRow[0] = reader.GetString("Rpt_Alarm_Count");


                table.Rows.Add(tempRow);
                foreach (DataColumn column in table.Columns)
                {
                    if (column.ColumnName != "CREATETIME")
                    {
                        tempRow[column] = table.Compute("Sum(" + column.ColumnName + ")", "true");
                    }
                }
                return table;
            }
            catch (Exception ex)
            {
                LoggerManager.Logger.Error(ex);
                throw;
            }
        }
        /// <summary>
        /// getChartData
        /// </summary>
        /// <param name="dtSource"></param>
        /// <returns></returns>
        public List<PieData> CreatePieDataForAlarmRpt()
        {
            try
            {
                DataRow row = DataSource.Rows[DataSource.Rows.Count - 1];
                PieData data = new PieData();
                //data.DateSource.Add(new PieData() { Name = reader.GetString("Rpt_Alarm_PublicFlase"), Val = string.IsNullOrEmpty(row["F"].ToString()) ? 0 : int.Parse(row["F"].ToString()) });
                //data.DateSource.Add(new PieData() { Name = reader.GetString("Rpt_Alarm_911False"), Val = string.IsNullOrEmpty(row["False911"].ToString()) ? 0 : int.Parse(row["False911"].ToString()) });
                data.DateSource.Add(new PieData() { Name = reader.GetString("Rpt_Alarm_911_Police"), Val = string.IsNullOrEmpty(row["JC1"].ToString()) ? 0 : int.Parse(row["JC1"].ToString()) });
                data.DateSource.Add(new PieData() { Name = reader.GetString("Rpt_Alarm_911_FireControl"), Val = string.IsNullOrEmpty(row["XF2"].ToString()) ? 0 : int.Parse(row["XF2"].ToString()) });
                data.DateSource.Add(new PieData() { Name = reader.GetString("Rpt_Alarm_911_Risk"), Val = string.IsNullOrEmpty(row["FX3"].ToString()) ? 0 : int.Parse(row["FX3"].ToString()) });
                data.DateSource.Add(new PieData() { Name = reader.GetString("Rpt_Alarm_911_Traffic"), Val = string.IsNullOrEmpty(row["JT4"].ToString()) ? 0 : int.Parse(row["JT4"].ToString()) });
                data.DateSource.Add(new PieData() { Name = reader.GetString("Rpt_Alarm_911_Medical"), Val = string.IsNullOrEmpty(row["YL5"].ToString()) ? 0 : int.Parse(row["YL5"].ToString()) });
                data.DateSource.Add(new PieData() { Name = reader.GetString("Rpt_Alarm_911_Army"), Val = string.IsNullOrEmpty(row["JD6"].ToString()) ? 0 : int.Parse(row["JD6"].ToString()) });
                data.DateSource.Add(new PieData() { Name = reader.GetString("Rpt_Alarm_911_Municipal"), Val = string.IsNullOrEmpty(row["SZ7"].ToString()) ? 0 : int.Parse(row["SZ7"].ToString()) });
                return data.DateSource;
            }
            catch (Exception)
            {
                throw;
            }

        }

        #endregion
        #region Vehicle Alert
        /// <summary>
        /// Get Data For VehicleAlert
        /// </summary>
        /// <returns></returns>
        public DataTable GetDataForVehicleAlertRpt()
        {
            try
            {
                string strWhereInfo = this.CreateWhereInfo(WhereInfo, "DISTRICT_CODE", "CREATE_TIME");
                StringBuilder builder = new StringBuilder();
                builder.Append(" SELECT a.*,  (  decode(a.COLJDZWL, null, 0, a.COLJDZWL) +  decode(a.COLCDZWL, null, 0, a.COLCDZWL) +  decode(a.COLPTCS, null, 0, a.COLPTCS)  +  decode(a.COLWLNCS, null, 0, a.COLWLNCS) +  decode(a.COLWLNDS, null, 0, a.COLWLNDS) +  decode(a.COLYCKM, null, 0, a.COLYCKM)  )  celZS  FROM ( ");
                builder.Append(" SELECT t.COLTIME,  sum(decode(t.alert_type, '11', t.counts, 0)) \"COLJDZWL\", sum(decode(t.alert_type, '12', t.counts, 0)) \"COLCDZWL\", sum(decode(t.alert_type, '13', t.counts, 0)) \"COLPTCS\",sum(decode(t.alert_type, '15', t.counts, 0)) \"COLWLNCS\",  sum(decode(t.alert_type, '16', t.counts, 0)) \"COLWLNDS\", sum(decode(t.alert_type, '18', t.counts, 0)) \"COLYCKM\"  from ");
                builder.Append(" ( SELECT  to_char(CREATE_TIME, 'yyyy-mm-dd') COLTIME,ALERT_TYPE, count(*) counts FROM BUSINESS_ALERT  ");
                builder.Append(strWhereInfo);
                if (!string.IsNullOrEmpty(strWhereInfo))
                {
                    builder.Append(" and (SUITE_STATUS =23 or SUITE_STATUS =24 or SUITE_STATUS = 25)  ");
                }
                builder.Append(" group by to_char(CREATE_TIME, 'yyyy-mm-dd'), ALERT_TYPE order by COLTIME  )t group by t.COLTIME order by t.COLTIME desc) a ");

                return DataSource = DataTableSum(oracleHelp.ExecuteDataTable(builder.ToString()), "COLTIME");
            }
            catch (Exception)
            {
                throw;
            }
        }
        public List<PieData> CreatePieDataForVehicleAlertRpt()
        {
            try
            {
                DataRow row = DataSource.Rows[DataSource.Rows.Count - 1];
                PieData data = new PieData();
                data.DateSource.Add(new PieData() { Name = reader.GetString("Rpt_VehicleAlert_JDZWL"), Val = string.IsNullOrEmpty(row["COLJDZWL"].ToString()) ? 0 : int.Parse(row["COLJDZWL"].ToString()) });
                data.DateSource.Add(new PieData() { Name = reader.GetString("Rpt_VehicleAlert_CDZWL"), Val = string.IsNullOrEmpty(row["COLCDZWL"].ToString()) ? 0 : int.Parse(row["COLCDZWL"].ToString()) });
                data.DateSource.Add(new PieData() { Name = reader.GetString("Rpt_VehicleAlert_PTCS"), Val = string.IsNullOrEmpty(row["COLPTCS"].ToString()) ? 0 : int.Parse(row["COLPTCS"].ToString()) });
                data.DateSource.Add(new PieData() { Name = reader.GetString("Rpt_VehicleAlert_WLNCS"), Val = string.IsNullOrEmpty(row["COLWLNCS"].ToString()) ? 0 : int.Parse(row["COLWLNCS"].ToString()) });
                data.DateSource.Add(new PieData() { Name = reader.GetString("Rpt_VehicleAlert_WLNDS"), Val = string.IsNullOrEmpty(row["COLWLNDS"].ToString()) ? 0 : int.Parse(row["COLWLNDS"].ToString()) });
                //data.DateSource.Add(new PieData() { Name = reader.GetString("Rpt_VehicleAlert_YCKM"), Val = string.IsNullOrEmpty(row["COLYCKM"].ToString()) ? 0 : int.Parse(row["COLYCKM"].ToString()) });
                return data.DateSource;
            }
            catch (Exception)
            {
                throw;
            }

        }
        #endregion
        #region OnLineTime
        /// <summary>
        /// OnLineTime
        /// </summary>
        /// <param name="whereInfo"></param>
        /// <returns></returns>
        public DataTable GetDataForVehicleOnTimeRpt()
        {
            try
            {
                //string strWhereInfo = this.CreateWhereInfo(WhereInfo, "t.district_code", "t.online_time");
                //StringBuilder builder = new StringBuilder();
                //builder.Append(" select t1.*, t2.COLONLINECOUNT ");
                //builder.Append("  from (select to_char(t.online_time, 'YYYY-MM-DD') COLTIME, ");
                //builder.Append("  trunc(avg(Distance), 2) DISTANCE, ");
                //builder.Append(" trunc(avg(t.online_timespan), 2) COLAVGTIMESPAN ");
                //builder.Append("  From vehicle_online_time_view t ");
                //builder.Append(strWhereInfo);
                //builder.Append(" group by to_char(t.online_time, 'YYYY-MM-DD') ");
                //builder.Append(" order by COLTIME desc) t1 ");
                //builder.Append(" join (select COLTIME, count(*) COLONLINECOUNT ");
                //builder.Append(" From (select distinct VEHICLE_ID, ");
                //builder.Append("  to_char(online_time, 'YYYY-MM-DD') COLTIME ");
                //builder.Append(" From vehicle_online_time_view ");
                //builder.Append(strWhereInfo);
                //builder.Append(" group by to_char(online_time, 'YYYY-MM-DD'), VEHICLE_ID) ");
                //builder.Append(" group by COLTIME) t2 ");
                //builder.Append(" on t1.coltime = t2.coltime ");
                #region Old
                string strWhereInfo = this.CreateWhereInfo(WhereInfo, "t.district_code", "t.online_time");
                StringBuilder builder = new StringBuilder();
                builder.Append(" select ");
                builder.Append(" to_char(t.online_time, 'dd/MM/yyyy') COLTIME, ");
                builder.Append(" count(to_char(online_time, 'dd/MM/yyyy')) COLONLINECOUNT, ");
                builder.Append(" trunc(avg(Distance),1) DISTANCE, ");
                builder.Append(" trunc(avg(t.online_timespan),2) COLAVGTIMESPAN");
                builder.Append(" From vehicle_online_time_view t ");
                builder.Append(strWhereInfo);
                builder.Append("  group by to_char(t.online_time, 'dd/MM/yyyy') ");
                builder.Append("  order by COLTIME desc ");
                #endregion
                DataSource = ConvertTimeForVehicleOnTimeRpt(oracleHelp.ExecuteDataTable(builder.ToString()));

                foreach (DataRow row in DataSource.Rows)
                {
                    if (row["COLAVGTIMESPAN"] != DBNull.Value)
                        row["COLAVGTIMESPAN"] = Math.Round(Convert.ToDouble(row["COLAVGTIMESPAN"]) / 3600, 3);
                }

                FillZeroData(DataSource);
                return DataSource;
            }
            catch (Exception)
            {

                throw;
            }
        }

        /// <summary>
        /// Convert
        /// </summary>
        /// <param name="dtTemp"></param>
        /// <returns></returns>
        private DataTable ConvertTimeForVehicleOnTimeRpt(DataTable dtTemp)
        {
            DataTable table = this.RebuildDataTable(dtTemp, WhereInfo.BeginTime, WhereInfo.EndTime, "COLTIME", "dd/MM/yyyy");

            return table;
        }
        #endregion
        #endregion
        #region Suite
        #region SuiteStatus
        /// <summary>
        /// get suite Data
        /// </summary>
        /// <param name="whereInfo"></param>
        /// <returns></returns>
        public DataTable GetDataForSuiteInfoRpt()
        {
            try
            {
                string strWhereInfo = this.CreateWhereInfo(WhereInfo, "a.district_code", "");

                StringBuilder builder = new StringBuilder();
                builder.Append(" select  to_char(a.status) statustype,count(*) toatl From SECURITY_SUITE_INFO a  ");
                builder.Append(" where a.status in(10,30,40) ");
                builder.Append(" group by a.status ");
                builder.Append(" union all ");
                builder.Append(" select  to_char(b.status) statustype,count(*) toatl From install_suite_info_view a ");
                builder.Append(" join SECURITY_SUITE_WORKING b on a.suite_info_id=b.suite_info_id  ");
                builder.Append(strWhereInfo);
                if (string.IsNullOrEmpty(strWhereInfo.Trim()))
                {
                    builder.Append(" where b.status in(22,23,24) ");
                }
                else
                {
                    builder.Append(" and b.status in(22,23,24) ");
                }
                builder.Append(" group by b.status ");
                return DataSource = CreateDataSourceForSuiteInfoRpt(oracleHelp.ExecuteDataTable(builder.ToString()));

            }
            catch (Exception)
            {

                throw;
            }
        }
        /// <summary>
        /// create suite data
        /// </summary>
        /// <param name="dtTemp"></param>
        /// <returns></returns>
        private DataTable CreateDataSourceForSuiteInfoRpt(DataTable dtTemp)
        {
            try
            {
                if (dtTemp.Rows.Count < 1)
                {
                    return null;
                }
                foreach (var item in dtTemp.Select("statustype in (99,20,25)"))
                {
                    dtTemp.Rows.Remove(item);
                }

                foreach (var item in new string[] { "23", "22", "24" })
                {
                    if (dtTemp.Select(string.Format("statustype in ({0})", item)).Count() < 1)
                    {
                        DataRow dr = dtTemp.NewRow();
                        dr["statustype"] = item;
                        dr["toatl"] = 0;
                        dtTemp.Rows.Add(dr);
                    }
                }

                DataRow tempRow = dtTemp.NewRow();
                tempRow[0] = reader.GetString("Summary");
                dtTemp.Rows.Add(tempRow);
                foreach (DataColumn column in dtTemp.Columns)
                {
                    if (column.ColumnName == "TOATL")
                    {
                        tempRow[column] = dtTemp.Compute("Sum(" + column.ColumnName + ")", "true");
                    }
                }


                dtTemp.Columns.Add(new DataColumn() { ColumnName = "percentage", DataType = typeof(double) });
                for (int i = 0; i < dtTemp.Rows.Count; i++)
                {
                    double d1 = 0, d2 = 0;
                    if (double.TryParse(dtTemp.Rows[dtTemp.Rows.Count - 1]["toatl"].ToString(), out d2) && d2 != 0)
                    {
                        double.TryParse(dtTemp.Rows[i]["toatl"].ToString(), out d1);
                        dtTemp.Rows[i]["percentage"] = Math.Round(d1 / d2, 3) * 100;
                    }
                    else
                    {
                        dtTemp.Rows[i]["percentage"] = 0;
                    }

                    if (i == dtTemp.Rows.Count - 1)
                        continue;
                    dtTemp.Rows[i]["statustype"] = GetStatus(int.Parse(dtTemp.Rows[i]["statustype"].ToString()));
                }
                return dtTemp;
            }
            catch (Exception ex)
            {
                LoggerManager.Logger.Error(ex);
                throw;
            }
        }

        public List<PieData> CreatePieDataForSuiteInfoRpt()
        {
            try
            {
                if (DataSource.Rows.Count < 1)
                {
                    return null;
                }
                DataTable dtTemp = DataSource.Copy();
                dtTemp.Rows.RemoveAt(DataSource.Rows.Count - 1);
                PieData data = new PieData();
                foreach (DataRow item in dtTemp.Rows)
                {
                    data.DateSource.Add(new PieData() { Name = item[0].ToString(), Val = string.IsNullOrEmpty(item[1].ToString()) ? 0 : int.Parse(item[1].ToString()) });
                }
                return data.DateSource;
            }
            catch (Exception ex)
            {
                LoggerManager.Logger.Error(ex);
                throw;
            }

        }
        public string GetStatus(int i)
        {
            switch (i)
            {
                case 10:
                    return reader.GetString("SUITE_SuiteInit");
                case 20:
                    return reader.GetString("SUITE_SuiteWork");
                case 22:
                    return reader.GetString("SUITE_SuiteTest");
                case 23:
                    return reader.GetString("SUITE_SuiteWork");
                case 24:
                    return reader.GetString("SUITE_SuiteUnusual");
                case 25:
                    return reader.GetString("SUITE_WaitMaintain");
                case 30:
                    return reader.GetString("SUITE_SuiteMaintain");
                case 40:
                    return reader.GetString("SUITE_SuiteScrap");
                case 99:
                    return reader.GetString("History");
                default:
                    return i.ToString();

            }
        }
        #endregion
        #region Execption Suite Rpt

        //public List<ExceptionSuiteModel> GetDataForExceptionSuiteRpt()
        //{
        //    try
        //    {
        //        Expression<Func<ExceptionSuiteModel, bool>> expression = e => true;

        //        if (WhereInfo.BeginTime != null && WhereInfo.EndTime != null)
        //        {
        //            if (WhereInfo.BeginTime == WhereInfo.EndTime)
        //            {
        //                WhereInfo.EndTime.AddDays(1);
        //            }
        //            expression = expression.And(c => c.FAULT_TIME >= WhereInfo.BeginTime && WhereInfo.EndTime <= WhereInfo.EndTime);
        //        }
        //        if (!string.IsNullOrEmpty(WhereInfo.Province))
        //        {
        //            if (!string.IsNullOrEmpty(WhereInfo.City))
        //            {
        //                expression = expression.And(c => c.DISTRICT_CODE.StartsWith(WhereInfo.City));
        //            }
        //            else
        //            {
        //                expression = expression.And(c => c.DISTRICT_CODE.StartsWith(WhereInfo.Province));
        //            }
        //        }
        //        else
        //        {
        //            if (!string.IsNullOrEmpty(WhereInfo.City))
        //            {
        //                expression = expression.And(c => c.DISTRICT_CODE.StartsWith(WhereInfo.City));
        //            }
        //        }

        //        Gsafety.PTMS.DBEntity.PTMSEntities db = new PTMSEntities();
        //        var query = from s in db.SECURITY_SUITE_WORKING
        //                    join v in db.VEHICLE on s.VEHICLE_ID equals v.VEHICLE_ID
        //                    where s.STATUS == 24
        //                    select new ExceptionSuiteModel
        //                    {
        //                        VEHICLE_ID = s.VEHICLE_ID,
        //                        MDVR_CORE_SN = s.MDVR_CORE_SN,
        //                        ABNORMAL_CAUSE = s.ABNORMAL_CAUSE,
        //                        SUITE_INFO_ID = s.SUITE_INFO_ID,
        //                        DISTRICT_CODE = v.DISTRICT_CODE,
        //                        FAULT_TIME = s.FAULT_TIME
        //                    };

        //        List<ExceptionSuiteModel> list = query.AsQueryable().Where(expression).ToList();
        //        string[] fiter = new string[] { ", " };
        //        foreach (var item in list)
        //        {
        //            if (string.IsNullOrEmpty(item.ABNORMAL_CAUSE))
        //            {
        //                continue;
        //            }
        //            string[] exceptions = item.ABNORMAL_CAUSE.Split(fiter, StringSplitOptions.RemoveEmptyEntries);
        //            string strException = "";
        //            if (exceptions.Count() > 0)
        //            {
        //                string[] values = exceptions.Distinct().ToArray();
        //                foreach (var exception in values)
        //                {
        //                    DeviceAlertConvert c = new DeviceAlertConvert();
        //                    strException += c.Convert(exception);
        //                    if (values.Last() != exception)
        //                        strException += ",";
        //                }
        //                item.ABNORMAL_CAUSE = strException;
        //            }
        //        }
        //        return list;
        //    }
        //    catch (Exception)
        //    {

        //        throw;
        //    }
        //}

        #endregion
        public DataTable GetDataForDeviceAlertRpt()
        {
            try
            {
                string strWhereInfo = this.CreateWhereInfo(WhereInfo, "DISTRICT_CODE", "CREATE_TIME");

                StringBuilder builder = new StringBuilder();
                builder.Append(" select ");
                builder.Append(" a.*, ");
                builder.Append(" ( ");
                builder.Append(" decode(a.COLCW, null, 0, a.COLCW) + ");
                builder.Append("  decode(a.COLGPSJSJGZ, null, 0, a.COLGPSJSJGZ) + ");
                builder.Append(" decode(a.COLSXTZD, null, 0, a.COLSXTZD) + ");
                builder.Append(" decode(a.COLSXTWXH, null, 0, a.COLSXTWXH) + ");
                builder.Append(" decode(a.COLMDVRSDCW, null, 0, a.COLMDVRSDCW) + ");
                builder.Append(" decode(a.COLSCMMCW, null, 0, a.COLSCMMCW) + ");
                builder.Append(" decode(a.COLDYYC, null, 0, a.COLDYYC) ");
                builder.Append(" )COLZS ");
                builder.Append(" from ");
                builder.Append(" ( ");
                builder.Append(" select ");
                builder.Append(" t.coltime, ");
                builder.Append(@" sum(decode(t.alert_type, 11, t.counts, 0)) ""COLCW"", ");
                builder.Append(@" sum(decode(t.alert_type, 12, t.counts, 0)) ""COLGPSJSJGZ"", ");
                builder.Append(@" sum(decode(t.alert_type, 10, t.counts, 0)) ""COLSXTZD"", ");
                builder.Append(@" sum(decode(t.alert_type, 14, t.counts, 0)) ""COLSXTWXH"", ");
                builder.Append(@" sum(decode(t.alert_type, 16, t.counts, 0)) ""COLMDVRSDCW"", ");
                builder.Append(@" sum(decode(t.alert_type, 17, t.counts, 0)) ""COLSCMMCW"", ");
                builder.Append(@" sum(decode(t.alert_type, 18, t.counts, 0)) ""COLDYYC"" ");
                builder.Append(" from ");
                builder.Append(" ( ");
                builder.Append(" select ");
                builder.Append(" to_char(CREATE_TIME, 'YYYY-MM-DD') coltime, ");
                builder.Append("  alert_type, ");
                builder.Append(" count(*) counts ");
                builder.Append(" from ");
                builder.Append(" DEVICE_ALERT ");
                builder.Append(strWhereInfo);
                builder.Append(" group by ");
                builder.Append(" to_char(CREATE_TIME, 'YYYY-MM-DD'), alert_type ");
                builder.Append(" ) ");
                builder.Append(" t ");
                builder.Append("  group by t.coltime ");
                builder.Append(" order by t.coltime desc ");
                builder.Append(" ) ");
                builder.Append(" a ");

                return DataSource = DataTableSum(oracleHelp.ExecuteDataTable(builder.ToString()), "COLTIME");
            }
            catch (Exception ex)
            {
                throw;
            }

        }

        public List<PieData> CreatePieDataForDeviceAlertRpt()
        {
            try
            {
                DataRow row = DataSource.Rows[DataSource.Rows.Count - 1];
                PieData data = new PieData();
                data.DateSource.Add(new PieData() { Name = reader.GetString("MAINTAIN_HightTemperature"), Val = string.IsNullOrEmpty(row["COLCW"].ToString()) ? 0 : int.Parse(row["COLCW"].ToString()) });
                data.DateSource.Add(new PieData() { Name = reader.GetString("LowTemperature"), Val = string.IsNullOrEmpty(row["COLSXTZD"].ToString()) ? 0 : int.Parse(row["COLSXTZD"].ToString()) });
                data.DateSource.Add(new PieData() { Name = reader.GetString("Rpt_Device_Alert_COLGPSJSJGZ"), Val = string.IsNullOrEmpty(row["COLGPSJSJGZ"].ToString()) ? 0 : int.Parse(row["COLGPSJSJGZ"].ToString()) });
                data.DateSource.Add(new PieData() { Name = reader.GetString("Rpt_Device_Alert_COLMDVRSDCW"), Val = string.IsNullOrEmpty(row["COLMDVRSDCW"].ToString()) ? 0 : int.Parse(row["COLMDVRSDCW"].ToString()) });
                data.DateSource.Add(new PieData() { Name = reader.GetString("Rpt_Device_Alert_COLSXTWXH"), Val = string.IsNullOrEmpty(row["COLSXTWXH"].ToString()) ? 0 : int.Parse(row["COLSXTWXH"].ToString()) });
                data.DateSource.Add(new PieData() { Name = reader.GetString("Rpt_Device_Alert_COLSCMMCW"), Val = string.IsNullOrEmpty(row["COLSCMMCW"].ToString()) ? 0 : int.Parse(row["COLSCMMCW"].ToString()) });
                data.DateSource.Add(new PieData() { Name = reader.GetString("Rpt_Device_Alert_COLDYYC"), Val = string.IsNullOrEmpty(row["COLDYYC"].ToString()) ? 0 : int.Parse(row["COLDYYC"].ToString()) });
                return data.DateSource;
            }
            catch (Exception ex)
            {
                LoggerManager.Logger.Error(ex);
                throw;
            }

        }
        #endregion
        #region SystemManagerRpt
        #region Internal_AccessRpt
        public DataTable GetDataForInternal_AccessRpt()
        {
            return null;
        }
        #endregion
        #region External_AccessRpt
        public DataTable GetDataForExternal_AccessRpt()
        {
            string strWhereInfo = this.CreateWhereInfo(WhereInfo, "", "VISIT_TIME");

            StringBuilder builder = new StringBuilder();
            builder.Append(" select ");
            builder.Append("  a.*, ");
            builder.Append(" ( ");
            builder.Append(" decode(a.COLRCGPS,null,0,a.COLRCGPS)+ ");
            builder.Append(" decode(a.COLLSGPS,null,0,a.COLLSGPS)+ ");
            builder.Append(" decode(a.COLYJBJGPS,null,0,a.COLYJBJGPS)+ ");
            builder.Append(" decode(a.COLSSSP,null,0,a.COLSSSP)+ ");
            builder.Append(" decode(a.COLLSSP,null,0,a.COLLSSP)+ ");
            builder.Append(" decode(a.COLWJXZ,null,0,a.COLWJXZ) ");
            builder.Append(" ) ");
            builder.Append(" colZS ");
            builder.Append(" from ");
            builder.Append(" ( ");
            builder.Append(" select ");
            builder.Append(" t.colTime, ");
            builder.Append(@" sum(decode(t.celType, 1, t.celCounts, 0)) ""COLRCGPS"", ");
            builder.Append(@" sum(decode(t.celType, 2, t.celCounts, 0)) ""COLLSGPS"", ");
            builder.Append(@" sum(decode(t.celType, 3, t.celCounts, 0)) ""COLYJBJGPS"", ");
            builder.Append(@" sum(decode(t.celType, 7, t.celCounts, 0)) ""COLSSSP"", ");
            builder.Append(@" sum(decode(t.celType, 9, t.celCounts, 0)) ""COLLSSP"", ");
            builder.Append(@" sum(decode(t.celType, 6, t.celCounts, 0)) ""COLWJXZ"" ");
            builder.Append(" From ");
            builder.Append(" ( ");
            builder.Append(" select to_char(VISIT_TIME, 'YYYY-MM-DD') colTime, ");
            builder.Append("  CONTENT_TYPE celType, ");
            builder.Append(" count(*) celCounts ");
            builder.Append(" from ");
            builder.Append(" VISIT_LOG ");
            builder.Append(strWhereInfo);
            builder.Append(" group by ");
            builder.Append(" to_char(VISIT_TIME, 'YYYY-MM-DD'), ");
            builder.Append(" CONTENT_TYPE ");
            builder.Append(" ) t ");
            builder.Append(" group by t.colTime");
            builder.Append(" order by t.colTime desc");
            builder.Append(" ) a ");
            return DataSource = DataTableSum(oracleHelp.ExecuteDataTable(builder.ToString()), "COLTIME");
        }

        public List<PieData> CreatePieDataForExternal_AccessRpt()
        {
            try
            {
                DataRow row = DataSource.Rows[DataSource.Rows.Count - 1];
                PieData data = new PieData();

                data.DateSource.Add(new PieData() { Name = reader.GetString("Rpt_External_Access_COLRCGPS"), Val = string.IsNullOrEmpty(row["COLRCGPS"].ToString()) ? 0 : int.Parse(row["COLRCGPS"].ToString()) });
                data.DateSource.Add(new PieData() { Name = reader.GetString("Rpt_External_Access_COLLSGPS"), Val = string.IsNullOrEmpty(row["COLLSGPS"].ToString()) ? 0 : int.Parse(row["COLLSGPS"].ToString()) });
                data.DateSource.Add(new PieData() { Name = reader.GetString("Rpt_External_Access_COLYJBJGPS"), Val = string.IsNullOrEmpty(row["COLYJBJGPS"].ToString()) ? 0 : int.Parse(row["COLYJBJGPS"].ToString()) });
                data.DateSource.Add(new PieData() { Name = reader.GetString("E8_VEN911"), Val = string.IsNullOrEmpty(row["COLSSSP"].ToString()) ? 0 : int.Parse(row["COLSSSP"].ToString()) });
                data.DateSource.Add(new PieData() { Name = reader.GetString("E8_ECU911"), Val = string.IsNullOrEmpty(row["COLLSSP"].ToString()) ? 0 : int.Parse(row["COLLSSP"].ToString()) });
                return data.DateSource;
            }
            catch (Exception ex)
            {
                LoggerManager.Logger.Error(ex);
                throw;
            }

        }

        #endregion
        #endregion
        #region Public Method

        public DataTable ConvertIntToTime(DataTable datasource, string celName)
        {
            try
            {
                string copyColName = celName + "Temp";
                DataColumn dcolCopy = new DataColumn(copyColName, typeof(string));
                if (datasource.Columns.Contains(celName))
                {
                    datasource.Columns.Add(dcolCopy);
                }
                else
                {
                    return datasource;
                }
                foreach (DataColumn item in datasource.Columns)
                {
                    if (item.ColumnName == celName.ToUpper())
                    {
                        foreach (DataRow itemRow in datasource.Rows)
                        {
                            double.TryParse(itemRow[item.ColumnName].ToString(), out dtemp);
                            TimeSpan span = TimeSpan.FromHours(dtemp);
                            string value = span.Hours.ToString() + ":" + span.Minutes.ToString("D2") + ":" + span.Seconds.ToString("D2");
                            itemRow[dcolCopy] = value;
                        }
                    }
                }
                return datasource;
            }
            catch (Exception ex)
            {
                LoggerManager.Logger.Error(ex);
                throw;
            }
        }
        /// <summary>
        ///  DataTable count
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="colunmName"></param>
        /// <returns></returns>
        private DataTable DataTableSum(DataTable dt, params string[] colunmName)
        {
            return DataTableCount("Sum", dt, colunmName);
        }


        private DataTable DataTableCount(string operation, DataTable dt, params string[] colunmName)
        {
            try
            {
                DataTable table = RebuildDataTable(dt, WhereInfo.BeginTime, WhereInfo.EndTime, colunmName[0], "yyyy-MM-dd");

                DataRow dr = table.NewRow();
                dr[0] = reader.GetString("Rpt_Alarm_Count");
                foreach (DataColumn item in table.Columns)
                {
                    if (colunmName.Count() > 0)
                    {
                        if (!colunmName.Contains(item.ColumnName))
                        {
                            dr[item] = table.Compute(string.Format("{1}({0})", item.ColumnName, operation), "true");
                        }
                    }
                    else
                    {
                        dr[item] = table.Compute(string.Format("{1}({0})", item.ColumnName, operation), "true");
                    }
                }
                table.Rows.Add(dr);

                FillZeroData(table);

                return table;
            }
            catch (Exception ex)
            {
                LoggerManager.Logger.Error(ex);
                throw;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="whereInfo"></param>
        /// <param name="celDistrict_Code"></param>
        /// <param name="celTime"></param>
        /// <returns></returns>
        private string CreateWhereInfo(ReportWhereInfo whereInfo, string celDistrict_Code, string celTime)
        {

            try
            {
                bool isUsingTime = false;
                string strWhereInfo = "";
                if (!string.IsNullOrEmpty(celTime))
                {
                    //if (whereInfo.BeginTime == whereInfo.EndTime)
                    //{
                    whereInfo.EndTime = whereInfo.EndTime.AddDays(1);
                    //}
                    strWhereInfo += string.Format("{2} >= to_date('{0}','{4}') and {3} < to_date('{1}','{4}') ", whereInfo.BeginTime.ToString(DateFormat), whereInfo.EndTime.ToString(DateFormat), celTime, celTime, DateFormat);
                    //LoggerManager.Logger.Error(strWhereInfo);
                    isUsingTime = true;
                }
                else
                {
                    isUsingTime = false;
                }
                if (string.IsNullOrEmpty(whereInfo.Province))
                {
                    //if (!whereInfo.ManageRegionCode.Equals("*"))
                    //{
                    //    if (isUsingTime)
                    //    {
                    //        strWhereInfo += "and ( ";
                    //    }
                    //    else
                    //    {
                    //        strWhereInfo += " ( ";
                    //    }
                    //    string[] strArray = whereInfo.ManageRegionCode.Split(new string[] { ", " }, StringSplitOptions.None);
                    //    for (int i = 0; i < strArray.Length; i++)
                    //    {
                    //        strWhereInfo = strWhereInfo + string.Format(" {1} like'{0}%' ", strArray[i], celDistrict_Code);
                    //        if (i != 0)
                    //        {
                    //            if ((strArray.Length - 1) == i)
                    //            {
                    //                strWhereInfo += " ) ";
                    //            }
                    //            else
                    //            {
                    //                strWhereInfo += " or ";
                    //            }
                    //        }
                    //        else if (strArray.Length == 1)
                    //        {
                    //            strWhereInfo += " ) ";
                    //        }
                    //        else
                    //        {
                    //            strWhereInfo += " or ";
                    //        }
                    //    }
                    //}
                }
                else
                {
                    if (isUsingTime)
                    {
                        strWhereInfo += "and ( ";
                    }
                    else
                    {
                        strWhereInfo += " ( ";
                    }
                    if (string.IsNullOrEmpty(whereInfo.City))
                    {
                        strWhereInfo += string.Format(" {1} like'{0}%' )", whereInfo.Province, celDistrict_Code);
                    }
                    else
                    {
                        strWhereInfo += string.Format(" {1} like'{0}%' )", whereInfo.City, celDistrict_Code);
                    }
                }
                if (!string.IsNullOrEmpty(strWhereInfo))
                {
                    strWhereInfo = " where " + strWhereInfo;
                }
                return strWhereInfo;

            }
            catch (Exception ex)
            {
                LoggerManager.Logger.Error(ex);
                throw;
            }
        }

        private DataTable Query(string strSql)
        {
            try
            {
                return oracleHelp.ExecuteDataTable(strSql);
            }
            catch (Exception ex)
            {
                LoggerManager.Logger.Error(ex);
                throw;
            }
        }
        #endregion

        public DataTable GetDataForBusinessAlertStatisticsReport()
        {
            try
            {
                StringBuilder builder = new StringBuilder();
                builder.Append("(select ALT_BUSINESS_ALERT.id,ALT_BUSINESS_ALERT.Vehicle_Id,ALT_BUSINESS_ALERT.District_Code,(CASE when ALT_BUSINESS_ALERT.ALERT_TYPE=0 or ALT_BUSINESS_ALERT.ALERT_TYPE=1 or ALT_BUSINESS_ALERT.ALERT_TYPE=2 or ALT_BUSINESS_ALERT.ALERT_TYPE=3 then 1 else 0 END) AlertNum,(CASE when ALT_BUSINESS_ALERT.ALERT_TYPE=0 then 1 else 0 END) SpeedAlert,(CASE when ALT_BUSINESS_ALERT.ALERT_TYPE=1 then 1 else 0 END) FenceAlert,(CASE when ALT_BUSINESS_ALERT.ALERT_TYPE=2 then 1 else 0 END) RouteAlert,(CASE when ALT_BUSINESS_ALERT.ALERT_TYPE=3 then 1 else 0 END) OffRouteAlert,ALT_BUSINESS_ALERT.gps_time from ALT_BUSINESS_ALERT where ALT_BUSINESS_ALERT.client_id='");
                builder.Append(WhereInfo.ClientID);
                builder.Append("' and GPS_TIME>='");
                builder.Append(WhereInfo.BeginTime.ToString("yyyy-MM-dd HH:mm:ss"));
                builder.Append("' and GPS_TIME<='");
                builder.Append(WhereInfo.EndTime.ToString("yyyy-MM-dd HH:mm:ss"));
                builder.Append("'");

                if (WhereInfo.City != null)
                {
                    builder.Append("  and district_code like '");
                    builder.Append(WhereInfo.City);
                    builder.Append("%'");
                }
                else
                {
                    if (WhereInfo.Province != null)
                    {
                        builder.Append("  and district_code like '");
                        builder.Append(WhereInfo.Province);
                        builder.Append("%'");

                    }
                }

                builder.Append(") t1,");
                string innertext = builder.ToString();
                builder.Clear();

                builder.Append("select t1.id,t1.vehicle_id vehicleid,t1.gps_time,substring(t1.District_code,1,2) province,t1.District_code,");

                builder.Append("ltrim(DATEPART(yyyy,GPS_TIME)) year, ltrim(DATEPART(yyyy,GPS_TIME))+'-'+ltrim(DATEPART(mm,GPS_TIME)) month,ltrim(DATEPART(yyyy,GPS_TIME))+'-'+ltrim(DATEPART(wk,GPS_TIME)) week, ltrim(DATEPART(yyyy,GPS_TIME))+'-'+ltrim(DATEPART(mm,GPS_TIME))+'-'+ltrim(DATEPART(dd,GPS_TIME)) day,");

                builder.Append(" v.orgnization_id, v.vehicle_type vehicleType,SpeedAlert,FenceAlert,RouteAlert,OffRouteAlert,AlertNum from ");
                builder.Append(innertext);
                builder.Append("bsc_vehicle v where v.vehicle_id=t1.vehicle_id ");

                if (!string.IsNullOrEmpty(WhereInfo.VehicleType))
                {
                    builder.Append("  and v.vehicle_type = '");
                    builder.Append(WhereInfo.VehicleType);
                    builder.Append("'");
                }

                builder.Append(" and v.orgnization_id in (");
                builder.Append(WhereInfo.OrgCode);
                builder.Append(") ");

                innertext = builder.ToString();
                builder.Clear();
                if (WhereInfo.GroupMode == 1)
                {
                    builder.Append("select year project,sum(AlertNum) AlertNum, sum(SpeedAlert) SpeedAlert,sum(FenceAlert) FenceAlert,sum(RouteAlert) RouteAlert,sum(OffRouteAlert) OffRouteAlert from (");
                    builder.Append(innertext);
                    builder.Append(") t2 group by year");
                }
                else if (WhereInfo.GroupMode == 2)
                {
                    builder.Append("select month project,sum(AlertNum) AlertNum, sum(SpeedAlert) SpeedAlert,sum(FenceAlert) FenceAlert,sum(RouteAlert) RouteAlert,sum(OffRouteAlert) OffRouteAlert from (");
                    builder.Append(innertext);
                    builder.Append(") t2 group by month");
                }
                else if (WhereInfo.GroupMode == 3)
                {
                    builder.Append("select week project,sum(AlertNum) AlertNum, sum(SpeedAlert) SpeedAlert,sum(FenceAlert) FenceAlert,sum(RouteAlert) RouteAlert,sum(OffRouteAlert) OffRouteAlert from (");
                    builder.Append(innertext);
                    builder.Append(") t2 group by week");
                }
                else if (WhereInfo.GroupMode == 4)
                {
                    builder.Append("select day project,sum(AlertNum) AlertNum, sum(SpeedAlert) SpeedAlert,sum(FenceAlert) FenceAlert,sum(RouteAlert) RouteAlert,sum(OffRouteAlert) OffRouteAlert from (");
                    builder.Append(innertext);
                    builder.Append(") t2 group by day");
                }
                else if (WhereInfo.GroupMode == 5)
                {
                    builder.Append("select name project,sum(AlertNum) AlertNum, sum(SpeedAlert) SpeedAlert,sum(FenceAlert) FenceAlert,sum(RouteAlert) RouteAlert,sum(OffRouteAlert) OffRouteAlert from (");
                    builder.Append(innertext);
                    builder.Append(") t2 left join usr_organization on t2.orgnization_id=usr_organization.id group by name");
                }
                else if (WhereInfo.GroupMode == 6)
                {
                    builder.Append("select province,sum(AlertNum) AlertNum, sum(SpeedAlert) SpeedAlert,sum(FenceAlert) FenceAlert,sum(RouteAlert) RouteAlert,sum(OffRouteAlert) OffRouteAlert from (");
                    builder.Append(innertext);
                    builder.Append(") t2 group by province");
                    innertext = builder.ToString();
                    builder.Clear();
                    builder.Append("select t3.*, d.name as Project from (");
                    builder.Append(innertext);
                    builder.Append(") t3 inner join bsc_district d on d.code=t3.province");
                }
                else if (WhereInfo.GroupMode == 7)
                {
                    builder.Append("select district_code,sum(AlertNum) AlertNum, sum(SpeedAlert) SpeedAlert,sum(FenceAlert) FenceAlert,sum(RouteAlert) RouteAlert,sum(OffRouteAlert) OffRouteAlert from (");
                    builder.Append(innertext);
                    builder.Append(") t2 group by district_code");
                    innertext = builder.ToString();
                    builder.Clear();
                    builder.Append("select t3.*, d.name as Project from (");
                    builder.Append(innertext);
                    builder.Append(") t3 inner join bsc_district d on d.code=t3.district_code");
                }
                else if (WhereInfo.GroupMode == 8)
                {
                    builder.Append("select vehicleid project,sum(AlertNum) AlertNum, sum(SpeedAlert) SpeedAlert,sum(FenceAlert) FenceAlert,sum(RouteAlert) RouteAlert,sum(OffRouteAlert) OffRouteAlert from (");
                    builder.Append(innertext);
                    builder.Append(") t2 group by vehicleid");
                }

                builder.Append(" order by Project");

                DataTable dtBusinessAlert = oracleHelp.ExecuteDataTable(builder.ToString());

                return dtBusinessAlert;
            }
            catch (Exception ex)
            {
                LoggerManager.Logger.Error(ex);
                throw;
            }
        }

        public DataTable GetDataForDeviceAlertStatisticsReport()
        {
            try
            {
                StringBuilder builder = new StringBuilder();
                builder.Append("(select ALT_DEVICE_ALERT.id,ALT_DEVICE_ALERT.Vehicle_Id,ALT_DEVICE_ALERT.District_Code,(CASE when ALT_DEVICE_ALERT.ALERT_TYPE=0 or ALT_DEVICE_ALERT.ALERT_TYPE=1 or ALT_DEVICE_ALERT.ALERT_TYPE=2 or ALT_DEVICE_ALERT.ALERT_TYPE=3 or ALT_DEVICE_ALERT.ALERT_TYPE=4 or ALT_DEVICE_ALERT.ALERT_TYPE=5 or ALT_DEVICE_ALERT.ALERT_TYPE=6 or ALT_DEVICE_ALERT.ALERT_TYPE=7 then 1 else 0 END) AlertNum,(CASE when ALT_DEVICE_ALERT.ALERT_TYPE=0 or ALT_DEVICE_ALERT.ALERT_TYPE=1 or ALT_DEVICE_ALERT.ALERT_TYPE=2 then 1 else 0 END) GNSSAlert,(CASE when ALT_DEVICE_ALERT.ALERT_TYPE=3 or ALT_DEVICE_ALERT.ALERT_TYPE=4 then 1 else 0 END) PowerAlert,(CASE when ALT_DEVICE_ALERT.ALERT_TYPE=5 then 1 else 0 END) LEDAlert,(CASE when ALT_DEVICE_ALERT.ALERT_TYPE=6 then 1 else 0 END) TTSAlert,(CASE when ALT_DEVICE_ALERT.ALERT_TYPE=7 then 1 else 0 END) CameraAlert,ALT_DEVICE_ALERT.gps_time from ALT_DEVICE_ALERT where ALT_DEVICE_ALERT.client_id='");
                builder.Append(WhereInfo.ClientID);
                builder.Append("' and GPS_TIME>='");
                builder.Append(WhereInfo.BeginTime.ToString("yyyy-MM-dd HH:mm:ss"));
                builder.Append("' and GPS_TIME<='");
                builder.Append(WhereInfo.EndTime.ToString("yyyy-MM-dd HH:mm:ss"));
                builder.Append("'");

                if (WhereInfo.City != null)
                {
                    builder.Append("  and district_code like '");
                    builder.Append(WhereInfo.City);
                    builder.Append("%'");
                }
                else
                {
                    if (WhereInfo.Province != null)
                    {
                        builder.Append("  and district_code like '");
                        builder.Append(WhereInfo.Province);
                        builder.Append("%'");

                    }
                }

                builder.Append(") t1,");
                string innertext = builder.ToString();
                builder.Clear();

                builder.Append("select t1.id,t1.vehicle_id vehicleid,t1.gps_time, substring(t1.District_code,1,2) province,t1.District_code,");

                builder.Append("ltrim(DATEPART(yyyy,GPS_TIME)) year, ltrim(DATEPART(yyyy,GPS_TIME))+'-'+ltrim(DATEPART(mm,GPS_TIME)) month,ltrim(DATEPART(yyyy,GPS_TIME))+'-'+ltrim(DATEPART(wk,GPS_TIME)) week, ltrim(DATEPART(yyyy,GPS_TIME))+'-'+ltrim(DATEPART(mm,GPS_TIME))+'-'+ltrim(DATEPART(dd,GPS_TIME)) day,");

                
                builder.Append(" v.orgnization_id, v.vehicle_type vehicleType,GNSSAlert,PowerAlert,LEDAlert,TTSAlert,CameraAlert,AlertNum from ");
                builder.Append(innertext);
                builder.Append("bsc_vehicle v where v.vehicle_id=t1.vehicle_id ");

                if (!string.IsNullOrEmpty(WhereInfo.VehicleType))
                {
                    builder.Append("  and v.vehicle_type = '");
                    builder.Append(WhereInfo.VehicleType);
                    builder.Append("'");
                }

                builder.Append(" and v.orgnization_id in (");
                builder.Append(WhereInfo.OrgCode);
                builder.Append(") ");

                innertext = builder.ToString();
                builder.Clear();
                if (WhereInfo.GroupMode == 1)
                {
                    builder.Append("select year project,sum(AlertNum) AlertNum, sum(GNSSAlert) GNSSAlert,sum(PowerAlert) PowerAlert,sum(LEDAlert) LEDAlert,sum(TTSAlert) TTSAlert,sum(CameraAlert) CameraAlert from (");
                    builder.Append(innertext);
                    builder.Append(") t2 group by year");
                }
                else if (WhereInfo.GroupMode == 2)
                {
                    builder.Append("select month project,sum(AlertNum) AlertNum, sum(GNSSAlert) GNSSAlert,sum(PowerAlert) PowerAlert,sum(LEDAlert) LEDAlert,sum(TTSAlert) TTSAlert,sum(CameraAlert) CameraAlert from (");
                    builder.Append(innertext);
                    builder.Append(") t2 group by month");
                }
                else if (WhereInfo.GroupMode == 3)
                {
                    builder.Append("select week project,sum(AlertNum) AlertNum, sum(GNSSAlert) GNSSAlert,sum(PowerAlert) PowerAlert,sum(LEDAlert) LEDAlert,sum(TTSAlert) TTSAlert,sum(CameraAlert) CameraAlert from  (");
                    builder.Append(innertext);
                    builder.Append(") t2 group by week");
                }
                else if (WhereInfo.GroupMode == 4)
                {
                    builder.Append("select day project,sum(AlertNum) AlertNum, sum(GNSSAlert) GNSSAlert,sum(PowerAlert) PowerAlert,sum(LEDAlert) LEDAlert,sum(TTSAlert) TTSAlert,sum(CameraAlert) CameraAlert from (");
                    builder.Append(innertext);
                    builder.Append(") t2 group by day");
                }
                else if (WhereInfo.GroupMode == 5)
                {
                    builder.Append("select name project,sum(AlertNum) AlertNum, sum(GNSSAlert) GNSSAlert,sum(PowerAlert) PowerAlert,sum(LEDAlert) LEDAlert,sum(TTSAlert) TTSAlert,sum(CameraAlert) CameraAlert from (");
                    builder.Append(innertext);
                    builder.Append(") t2 left join usr_organization on t2.orgnization_id=usr_organization.id group by name");
                }
                else if (WhereInfo.GroupMode == 6)
                {
                    builder.Append("select province,sum(AlertNum) AlertNum, sum(GNSSAlert) GNSSAlert,sum(PowerAlert) PowerAlert,sum(LEDAlert) LEDAlert,sum(TTSAlert) TTSAlert,sum(CameraAlert) CameraAlert from (");
                    builder.Append(innertext);
                    builder.Append(") t2 group by province");
                    innertext = builder.ToString();
                    builder.Clear();
                    builder.Append("select t3.*, d.name as Project from (");
                    builder.Append(innertext);
                    builder.Append(") t3 inner join bsc_district d on d.code=t3.province");
                }
                else if (WhereInfo.GroupMode == 7)
                {
                    builder.Append("select district_code,sum(AlertNum) AlertNum, sum(GNSSAlert) GNSSAlert,sum(PowerAlert) PowerAlert,sum(LEDAlert) LEDAlert,sum(TTSAlert) TTSAlert,sum(CameraAlert) CameraAlert from  (");
                    builder.Append(innertext);
                    builder.Append(") t2 group by district_code");
                    innertext = builder.ToString();
                    builder.Clear();
                    builder.Append("select t3.*, d.name as Project from (");
                    builder.Append(innertext);
                    builder.Append(") t3 inner join bsc_district d on d.code=t3.district_code");
                }
                else if (WhereInfo.GroupMode == 8)
                {
                    builder.Append("select vehicleid project,sum(AlertNum) AlertNum, sum(GNSSAlert) GNSSAlert,sum(PowerAlert) PowerAlert,sum(LEDAlert) LEDAlert,sum(TTSAlert) TTSAlert,sum(CameraAlert) CameraAlert from (");
                    builder.Append(innertext);
                    builder.Append(") t2 group by vehicleid");
                }

                builder.Append(" order by Project");

                DataTable dtBusinessAlert = oracleHelp.ExecuteDataTable(builder.ToString());

                return dtBusinessAlert;
            }
            catch (Exception ex)
            {
                LoggerManager.Logger.Error(ex);
                throw;
            }
        }

        public DataTable GetDataForOffLineStatisticsReport()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("select v.vehicle_id,v.owner,v.contact_phone,v.district_code, d.name as districtname,t.name as vehicletype,o.name as OrganizationName from bsc_vehicle v inner join usr_organization o on v.orgnization_id=o.id inner join bsc_vehicle_type t on v.vehicle_type=t.id inner join bsc_district d on v.district_code=d.code where v.vehicle_id not in ");
            builder.Append("((select g.vehicle_id from run_gps_online_record g where g.gps_time>=");
            builder.Append("'");
            builder.Append(WhereInfo.EndTime.ToString("yyyy-MM-dd HH:mm:ss"));
            builder.Append("'");
            builder.Append(" and g.gps_time<=");
            builder.Append("'");
            builder.Append(WhereInfo.BeginTime.ToString("yyyy-MM-dd HH:mm:ss"));
            builder.Append("'");
            builder.Append(") union (select s.vehicle_id from run_suite_online_record s where s.gps_time>=");
            builder.Append("'");
            builder.Append(WhereInfo.EndTime.ToString("yyyy-MM-dd HH:mm:ss"));
            builder.Append("'");
            builder.Append(" and s.gps_time<");
            builder.Append("'");
            builder.Append(WhereInfo.BeginTime.ToString("yyyy-MM-dd HH:mm:ss"));
            builder.Append("'");
            builder.Append("))");

            if (!string.IsNullOrEmpty(WhereInfo.City))
            {
                builder.Append(" and v.district_code like '");
                builder.Append(WhereInfo.City);
                builder.Append("%'");
            }
            else if (!string.IsNullOrEmpty(WhereInfo.Province))
            {
                builder.Append(" and v.district_code like '");
                builder.Append(WhereInfo.Province);
                builder.Append("%'");
            }

            if (!string.IsNullOrEmpty(WhereInfo.VehicleType))
            {
                builder.Append(" and v.vehicle_type = '");
                builder.Append(WhereInfo.VehicleType);
                builder.Append("'");
            }

            builder.Append(" and v.vehicle_id in ((select gd.vehicle_id from mtn_gps_installation_detail gd where gd.checkstep=4 and gd.valid=1) union( select sd.vehicle_id from mtn_installation_detail sd where sd.checkstep=7 and sd.valid=1))");

            builder.Append(" and v.orgnization_id in (");
            builder.Append(WhereInfo.OrgCode);
            builder.Append(") ");

            DataTable dtOffline = oracleHelp.ExecuteDataTable(builder.ToString());

            return dtOffline;
        }

        public DataTable GetHistoryTraceStatisticsReport()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("select v.vehicle_id,v.district_code, d.name as districtname,t.name as vehicletype,o.name as OrganizationName,vl.latitude as lat,vl.longitude as lon,vl.direction as dir,vl.speed as speed,vl.gps_time as gpstime from run_vehicle_location vl inner join  bsc_vehicle v  on v.vehicle_id = vl.vehicle_id inner join usr_organization o on v.orgnization_id=o.id inner join bsc_vehicle_type t on v.vehicle_type=t.id inner join bsc_district d on v.district_code=d.code where vl.gps_time >='");
           
            builder.Append(WhereInfo.BeginTime.ToString("yyyy-MM-dd HH:mm:ss"));
            builder.Append("'");           
            builder.Append(" and vl.gps_time<='");
            builder.Append(WhereInfo.EndTime.ToString("yyyy-MM-dd HH:mm:ss"));
            builder.Append("'");
           

            if (!string.IsNullOrEmpty(WhereInfo.City))
            {
                builder.Append(" and v.district_code like '");
                builder.Append(WhereInfo.City);
                builder.Append("%'");
            }
            else if (!string.IsNullOrEmpty(WhereInfo.Province))
            {
                builder.Append(" and v.district_code like '");
                builder.Append(WhereInfo.Province);
                builder.Append("%'");
            }

            if (!string.IsNullOrEmpty(WhereInfo.VehicleType))
            {
                builder.Append(" and v.vehicle_type = '");
                builder.Append(WhereInfo.VehicleType);
                builder.Append("'");
            }

            if (!string.IsNullOrEmpty(WhereInfo.VehicleId))
            {
                builder.Append(" and vl.vehicle_id = '");
                builder.Append(WhereInfo.VehicleId);
                builder.Append("'");
            }


           
            builder.Append(" and v.orgnization_id in (");
            builder.Append(WhereInfo.OrgCode);
            builder.Append(")  order by vl.gps_time desc");

            DataTable dtOffline = oracleHelp.ExecuteDataTable(builder.ToString());

            return dtOffline;
        }

        public DataTable GetDataForUserOnlineStatisticsReport()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("select * from LOG_ACCESS where LOGIN_TIME >='");
            builder.Append(WhereInfo.BeginTime.ToString("yyyy-MM-dd HH:mm:ss"));
            builder.Append("' and LOGIN_TIME<='");
            builder.Append(WhereInfo.EndTime.ToString("yyyy-MM-dd HH:mm:ss"));
            builder.Append("' order by LOGIN_TIME desc");

            DataTable dtOnline = oracleHelp.ExecuteDataTable(builder.ToString());

            return dtOnline;
        }

        public DataTable GetDataForVideoFlowStatisticsReport()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("select v.vehicle_id from bsc_vehicle v inner join usr_organization o on v.orgnization_id=o.id inner join bsc_vehicle_type t on v.vehicle_type=t.id inner join bsc_district d on v.district_code=d.code ");

            builder.Append(" and v.orgnization_id in (");
            builder.Append(WhereInfo.OrgCode);
            builder.Append(") ");

            if (!string.IsNullOrEmpty(WhereInfo.City))
            {
                builder.Append(" and v.district_code like '");
                builder.Append(WhereInfo.City);
                builder.Append("%'");
            }
            else if (!string.IsNullOrEmpty(WhereInfo.Province))
            {
                builder.Append(" and v.district_code like '");
                builder.Append(WhereInfo.Province);
                builder.Append("%'");
            }

            if (!string.IsNullOrEmpty(WhereInfo.VehicleType))
            {
                builder.Append(" and v.vehicle_type = '");
                builder.Append(WhereInfo.VehicleType);
                builder.Append("'");
            }

            string where = builder.ToString();
            builder.Clear();

            builder.Append("select DATEDIFF(Second,t.start_time,t.end_time) as TimeSpan,t.video_size as VideoSize,w.vehicle_id as VehicleID from MDI_ALARM_VIDEO t inner join run_suite_working w on t.mdvr_core_sn=w.mdvr_core_sn where t.start_time > ");
            builder.Append("'");
            builder.Append(WhereInfo.BeginTime.ToString("yyyy-MM-dd HH:mm:ss"));
            builder.Append("'");
            builder.Append(" and t.end_time<");
            builder.Append("'");
            builder.Append(WhereInfo.EndTime.ToString("yyyy-MM-dd HH:mm:ss"));
            builder.Append("'");
            builder.Append(" and w.vehicle_id in (");
            builder.Append(where);
            builder.Append(")");
            string alarmvideo = builder.ToString();
            builder.Clear();

            builder.Append("select  DATEDIFF(Second,d.start_time,d.end_time) as TimeSpan,d.SOURCE_DOWNLOAD_SIZE as VideoSize,w.vehicle_id as VehicleID from MDI_DOWNLOAD_VIDEO d inner join run_suite_working w on d.mdvr_core_sn=w.mdvr_core_sn where d.start_time > ");
            builder.Append("'");
            builder.Append(WhereInfo.BeginTime.ToString("yyyy-MM-dd HH:mm:ss"));
            builder.Append("'");
            builder.Append(" and d.end_time<");
            builder.Append("'");
            builder.Append(WhereInfo.EndTime.ToString("yyyy-MM-dd HH:mm:ss"));
            builder.Append("'");
            builder.Append(" and w.vehicle_id in (");
            builder.Append(where);
            builder.Append(")");
            string downloadvideo = builder.ToString();
            builder.Clear();

            builder.Append("select DATEDIFF(Second,l.start_time,l.end_time) as TimeSpan,l.video_size as VideoSize,w.vehicle_id as VehicleID from MDI_LIVE_VIDEO l inner join run_suite_working w on l.mdvr_core_sn=w.mdvr_core_sn where l.start_time > ");
            builder.Append("'");
            builder.Append(WhereInfo.BeginTime.ToString("yyyy-MM-dd HH:mm:ss"));
            builder.Append("'");
            builder.Append(" and l.end_time<");
            builder.Append("'");
            builder.Append(WhereInfo.EndTime.ToString("yyyy-MM-dd HH:mm:ss"));
            builder.Append("'");
            builder.Append(" and w.vehicle_id in (");
            builder.Append(where);
            builder.Append(")");
            string livevideo = builder.ToString();
            builder.Clear();

            string union = "((" + alarmvideo + ") union (" + downloadvideo + ") union (" + livevideo + ")))u";

            builder.Append("select u.VehicleID,sum(u.TimeSpan) as TimeSpan,sum(u.VideoSize) as VideoSize from (");
            builder.Append(union);
            builder.Append(" group by u.VehicleID");
            string group = "(" + builder.ToString() + ")g";
            builder.Clear();

            builder.Append("select v.vehicle_id, d.name as districtname,t.name as vehicletype,o.name as OrganizationName,g.VideoSize,g.TimeSpan from bsc_vehicle v inner join usr_organization o on v.orgnization_id=o.id inner join bsc_vehicle_type t on v.vehicle_type=t.id inner join bsc_district d on v.district_code=d.code inner join " + group);
            builder.Append(" on v.vehicle_id=g.VehicleID ");

            DataTable dtVideoFlow = oracleHelp.ExecuteDataTable(builder.ToString());

            return dtVideoFlow;
        }
    }
}

