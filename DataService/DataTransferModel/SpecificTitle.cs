using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.DataTransferModel
{
    public class SpecificTitle
    {
        public string? TConst { get; set; }
        public string? Title { get; set; }
        public string? Year { get; set; }
        public IList<string>? Genre { get; set; }
        public int? Runtime { get; set; }
        public IList<DirectorListElement>? DirectorList { get; set; }
        public IList<ActorListElement>? ActorList { get; set; }
        public float? Rating { get; set; }

    }
}
