using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.DatabaseModel
{
    public class NameBasics
    {
        public string? NConst { get; set; }
        public string? PrimaryName { get; set; }
        public string? BirthYear { get; set; }
        public string? DeathYear { get; set; }
        public float? AVGNameRating { get; set; }
        public IList<KnownFor>? KnownFor { get; set; }
        public IList<TitlePrincipals>? TitlePrincipals { get; set; }
        public IList<Professions>? Professions { get; set; }
        public IList<Jobs>? Jobs { get; set; }
        public IList<Characters>? Characters { get; set; }
    }
}
