using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.DatabaseModel
{
    public class Review
    {
        public string Username { get; set; }
        public User User { get; set; }
        public string TConst { get; set; }
        public TitleBasics TitleBasics { get; set; }
        public DateOnly Date { get; set; }
        public string Content { get; set; }
        public bool IsSpoiler { get; set; }
    }
}
