using EmailContract.Data;
using EmailRepository;
using Gsafety.PTMS.Alert.Contract.Data;
using Gsafety.PTMS.Alert.Repository;
using Gsafety.PTMS.Base.Contract.Data;
using Gsafety.PTMS.BaseInformation.Contract.Data;
using Gsafety.PTMS.BaseInformation.Repository;
using Gsafety.Common.Logging;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Gsafety.PTMS.DBEntity;

namespace AutoEmail
{

    public class AutoEmailManager
    {
        /// <summary>
        /// 邮件服务开始
        /// </summary>
        public static void Start()
        {

            try
            {
                System.Timers.Timer t = new System.Timers.Timer();
                t.Interval = 3600000;//间隔时间为一小时
                //t.Interval = 120000;
                t.Elapsed += new System.Timers.ElapsedEventHandler(ChkSrv);//到达时间的时候执行事件； 
                t.AutoReset = true;//设置是执行一次（false）还是一直执行(true)； 
                t.Enabled = true;//是否执行System.Timers.Timer.Elapsed事件； 
                LoggerManager.Logger.Info("Email service starts successfully!");
            }
            catch (Exception ex)
            {
                LoggerManager.Logger.Error("Email service starts Failure!" + ex);
            }
        }

        /// <summary>
        /// 定时检查，并执行方法
        /// </summary>
        /// <param name="source"></param>
        /// <param name="e"></param>
        public static void ChkSrv(object source, System.Timers.ElapsedEventArgs e)
        {
            int intHour = DateTime.Now.Hour;

            DateTime date = DateTime.Now.AddDays(-1);
            DateTime beginDate = new DateTime(date.Year, date.Month, date.Day, 0, 0, 0);
            DateTime endDate = new DateTime(date.Year, date.Month, date.Day, 23, 59, 59);

            if (intHour == 1) ///定时设置,判断分时秒
            {
                try
                {
                    createEmail(beginDate, endDate);
                }
                catch (Exception ex)
                {

                }
            }
        }

        /// <summary>
        /// 邮件服务停止
        /// </summary>
        public static void Stop()
        {
            try
            {
                LoggerManager.Logger.Info("Email service stop successfully!");
            }
            catch (Exception ex)
            {
                LoggerManager.Logger.Error("Email service stop failure!" + ex);
            }
        }

        /// <summary>
        /// 获取告警信息
        /// </summary>
        private static List<EmailAlertInfo> getAlertInfo( DateTime begintime, DateTime endtime)
        {
            VehicleAlertRespository repository = new VehicleAlertRespository();

            var alert = repository.GetEmailAlertInfo(begintime, endtime);

            if (alert != null && alert.Count > 0)
            {
                //LoggerManager.Logger.Info(string.Format("add {0} data into the cache", alert.Count));
                return alert;
            }
            else
            {
                LoggerManager.Logger.Warn("Get AlertInfo is empty!");
                return null;
            }
        }

        /// <summary>
        /// 生成邮件
        /// </summary>
        private static void createEmail(DateTime begintime, DateTime endtime)
        {
            //    List<EmailAlertInfo> alertInfo = getAlertInfo(begintime, endtime);

            //    if (alertInfo != null && alertInfo.Count > 0)
            //    {
            //        alertInfo.GroupBy(x => x.CompanyId).ToList().ForEach(item =>
            //        {
            //            List<EmailAlertInfo> temp = item.ToList();

            //            if (temp.FirstOrDefault() != null && Regex.IsMatch(temp.FirstOrDefault().CompanyEmail, "\\w{1,}@\\w{1,}\\.\\w{1,}", RegexOptions.IgnoreCase))
            //            {
            //                string path = Export(temp, temp.FirstOrDefault().CompanyName);
            //                sendEmail(path, temp.FirstOrDefault().CompanyEmail);
            //            }
            //        });
            //    }
        }

        /// <summary>
        /// 发送邮件
        /// </summary>
        /// <param name="path">附件路径</param>
        /// <param name="addr">邮件地址</param>
        private static void sendEmail(string path, string addr)
        {
            EmailOperateRepository repository = new EmailOperateRepository();
            Email email = new Email();
            email.MailSubject = DateTime.Now.AddDays(-1).ToString("yyyyMMddhhmmss");
            email.IsbodyHtml = true;    //是否是HTML
            email.AttachmentsPath = new string[] { path };
            email.MailToArray = new string[] { addr };
            var alert = repository.SendEmail(email);
        }

        /// <summary>
        /// 导出Excel附件
        /// </summary>
        /// <param name="alertInfo">邮件内容</param>
        /// <param name="companyname"></param>
        /// <returns></returns>
        private static string Export(List<EmailAlertInfo> alertInfo, string companyname)
        {
            DataTable table = new DataTable();
            table.Columns.Add("VehicleId");
            table.Columns.Add("OverSpeedCount");
            table.Columns.Add("InFenceCount");

            DataRow FirstRow = table.NewRow();
            FirstRow[0] = "Placa";
            FirstRow[1] = "Contador de exceso de velocidad";
            FirstRow[2] = "Contador de ingreso y salida de la cerca";
            table.Rows.Add(FirstRow);

            alertInfo.ForEach(x =>
                {
                    DataRow ContentRow = table.NewRow();
                    ContentRow[0] = x.VehicleId;
                    ContentRow[1] = x.OverSpeedCount;
                    ContentRow[2] = x.InOutFenceCount;
                    table.Rows.Add(ContentRow);
                });

            List<DataTable> list = new List<DataTable>();
            list.Add(table);
            string addr = AppDomain.CurrentDomain.BaseDirectory + companyname + DateTime.Now.ToString("yyyyMMddhhmmss") + ".xlsx";
            //OpenXmlSDKExporter.Export(AppDomain.CurrentDomain.BaseDirectory + "\\excel.xlsx", lsit);
            OpenXmlSDKExporter.Export(addr, list);
            return addr;
        }
    }
}
