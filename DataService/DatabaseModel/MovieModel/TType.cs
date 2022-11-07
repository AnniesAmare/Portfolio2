using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.DatabaseModel
{
    public class TType
    {
        public string TConst { get; set; }
        public int Ordering { get; set; }
        public string Type { get; set; }


        public TitleAka? TitleAka { get; set; }

    }
}
