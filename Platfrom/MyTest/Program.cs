using Gs.PTMS.Service;
using Gsafety.PTMS.Alarm.Repository;
using Gsafety.PTMS.Alert.Repository;
using Gsafety.PTMS.Common.Data;
using Gsafety.PTMS.Common.Data.Enum;
using Gsafety.PTMS.DBEntity;
using Gsafety.PTMS.Monitor.Repository;
using Gsafety.PTMS.Report.Repository;
using Gsafety.PTMS.Traffic.Repository;
using GSafety.PTMS.PublicService.Repository;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MyTest
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            string code = "12-13";
            int index = code.LastIndexOf("-");
            string mycode = code.Substring(0, index);
        }
    }
}
