using Gsafety.PTMS.BaseInfo.MakerContions.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gsafety.PTMS.BaseInfo.Conditons.Interface
{
    /// <summary>
    /// Rule analysis
    /// </summary>
    public interface IValueAnaly
    {
        CondtionItem ToConditonItem();
    }
}
