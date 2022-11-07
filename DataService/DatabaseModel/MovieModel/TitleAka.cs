using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.DatabaseModel
{
    public class TitleAka
    {
        public string TConst { get; set; }
        public TitleBasic TitleBasic { get; set; }
        public int Ordering { get; set; }
        public string? Title { get; set; }
        public string? Region { get; set; }
        public bool? IsOriginalTitle { get; set; }

        public Language? Language { get; set; }
        public TType? TType { get; set; }
        public Attributes? Attributes { get; set; }

    }
}
