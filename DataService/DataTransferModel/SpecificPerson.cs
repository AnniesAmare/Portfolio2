using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.DataTransferModel
{
    public class SpecificPerson
    {
        public string NConst { get; set; }
        public string Name { get; set; }
        public string? BirthYear { get; set; }
        public string? DeathYear { get; set; }
        public IList<string> ProfessionList { get; set; }
        public IList<TitleListElement> KnownForList { get; set; }

    }
}
