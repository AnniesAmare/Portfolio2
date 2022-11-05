using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.DatabaseModel
{
    public class Attributes
    {
        
        public string TConst { get; set; }
        public int Ordering { get; set; }
        public string Attribute { get; set; }
        public TitleAkas? TitleAkas { get; set; }
    }
}
