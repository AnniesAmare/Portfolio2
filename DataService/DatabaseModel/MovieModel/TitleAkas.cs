using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.DatabaseModel
{
    public class TitleAkas
    {
        public string TConst { get; set; }
        public TitleBasics TitleBasics { get; set; }
        public int Ordering { get; set; }
        public string? Title { get; set; }
        public string? Region { get; set; }
        public bool? IsOriginalTitle { get; set; }

        public IList<Languages>? Languages { get; set; }
        public IList<Types>? Types { get; set; }
        public IList<Attributes>? Attributes { get; set; }

    }
}
