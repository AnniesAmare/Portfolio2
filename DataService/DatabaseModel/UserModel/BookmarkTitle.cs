using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.DatabaseModel
{
    public class BookmarkTitle
    {
        public string Username { get; set; }
        public User User { get; set; }
        public string TConst { get; set; }
        public TitleBasic TitleBasic { get; set; }
        public string Annotation { get; set; }
    }
}
