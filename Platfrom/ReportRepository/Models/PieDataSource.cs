using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gsafety.PTMS.Report.Repository
{
  
    public class PieData
    {
        public int Val { get; set; }
        public string Name { get; set; }
        private List<PieData> _dateSource = new List<PieData>();
        public List<PieData> DateSource
        {
            get
            {
                return _dateSource;
            }
        }

        public PieData()
        { }
        public PieData(string name, int value)
        {
            this.Val = value;
            this.Name = name;
        }
    }
}
