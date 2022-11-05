using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.DatabaseModel
{
    public class Genre
    {
        
        public string? TConst { get; set; }
        public TitleBasic? TitleBasic { get; set; }
        public string? TGenre { get; set; }
    }
}
