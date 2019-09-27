using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gsafety.PTMS.Media.Common.Loggers
{
    public interface ILogging
    {
        LoggerLevel Level { get; set; }

        void Debug(string message);

        void Info(string message);

        void Error(string message);

        void Exception(Exception ex);

        void Exception(string ex);
    }
}
