using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.DatabaseModel
{
    public class NameBasic
    {
        public string? NConst { get; set; }
        public string? PrimaryName { get; set; }
        public string? BirthYear { get; set; }
        public string? DeathYear { get; set; }
        public float? AVGNameRating { get; set; }
        public bool IsActor { get; set; }
        public IList<KnownFor>? KnownFor { get; set; }
        public IList<TitlePrincipal>? TitlePrincipal { get; set; }
        public IList<Profession>? Profession { get; set; }
        public IList<Job>? Job { get; set; }
        public IList<Character>? Character { get; set; }
    }
}
