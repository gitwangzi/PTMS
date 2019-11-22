using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gsafety.PTMS.Analysis.Helper
{
    public class UserRoute
    {
        public const string UserLogin = "App.User.UserLogin";
        public const string UserLogout = "App.User.UserLogout";
        public const string ForceLogout = "App.User.ForceLogout";
        public const string ForceMultiplyLogout = "App.User.ForceMultiplyLogout";
        public const string UserOnlineHeartBeat = "App.User.OnlineHeartbeat";
        public const string UpdateCache = "App.User.UpdateCache";
    }
}
