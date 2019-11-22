using Gsafety.PTMS.BaseInfo.Conditions.QueryFiler;
using Gsafety.PTMS.BaseInfo.Conditons.QueryFiler;
using Gsafety.PTMS.BaseInfo.MakerContions.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gsafety.PTMS.BaseInfo.Conditons.ValueAnalys
{
    /// <summary>
    /// District Value
    /// </summary>
    public class DistrictValue : AuthorityValue
    {
        public DistrictValue(short? userType, string regions, string province, string city)
            : base(userType, regions,province,city)
        { 
        }

        public override MakerContions.Items.CondtionItem ToConditonItem()
        {
            return ToItemByPropertyName(GlobalData.DISTRICT_CODE);
        }
    }
}
