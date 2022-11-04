using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.DatabaseModel
{
    public class KnownFor
    {
        public string TConst { get; set; }
        public TitleBasics TitleBasics { get; set; }
        public string NConst { get; set; }
        public NameBasics NameBasics { get; set; }
    }
}
