using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.DataTransferModel
{
    public class Titles
    {
        public string? TConst { get; set; }
        public string? Type { get; set; }
        public string? Name { get; set; }
        public string? AiringDate { get; set; }
        public bool IsTvShow { get; set; }
        public bool IsEpisode { get; set; }
        public bool IsMovie { get; set; }

    }
}


//distinct() or take(25) -query limitation