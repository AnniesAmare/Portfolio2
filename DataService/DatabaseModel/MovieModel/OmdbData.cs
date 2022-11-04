using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.DatabaseModel
{
    public class OmdbData
    {
        public string? TConst { get; set; }
        public TitleBasics? TitleBasics { get; set; }
        public string? Poster { get; set; }
        public string? Plot { get; set; }

    }
}
