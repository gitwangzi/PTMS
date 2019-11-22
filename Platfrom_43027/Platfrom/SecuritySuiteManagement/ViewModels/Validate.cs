using Gsafety.Common.Controls;
using Gsafety.Common.Localization.Resource;
using Gsafety.PTMS.Share;
using System;
using System.Net;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace Gsafety.Ant.SecuritySuite.ViewModels
{
    internal class Validate
    {
        internal bool SearchConditionValid(DateTime sd, DateTime ed)
        {
            bool result = false;
            try
            {              
                if (sd.CompareTo(ed) > 0)
                {
                    string timeError = StringResource.ResourceManager.GetString("TimeError");
                    string msg = string.IsNullOrEmpty(timeError) ? "StrartTime can not be greater than EndTime !" : timeError;

                    string warning = StringResource.ResourceManager.GetString("Warning");
                    MessageBoxHelper.ShowDialog(msg, string.IsNullOrEmpty(warning) ? "Warning !" : warning, MessageDialogButton.Ok);
                    result = false;
                }
                else
                {
                    result = true;
                }              
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(), ex);
            }
            return result;
            
        }
    }
}
