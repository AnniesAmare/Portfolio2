using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.DatabaseModel
{
    public class BookmarkName
    {
        public string? Username { get; set; }
        public User? User { get; set; }
        public string? NConst { get; set; }
        public NameBasic? NameBasic { get; set; }
        public string? Annotation { get; set; }


    }
}
