using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.DatabaseModel
{
    public class Job
    {
        public string TConst { get; set; }
        public TitleBasic TitleBasic { get; set; }
        public string NConst { get; set; }
        public NameBasic NameBasic { get; set; }
        public string TJob { get; set; }
    }
}
