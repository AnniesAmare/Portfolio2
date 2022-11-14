using DataLayer.DatabaseModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.DataTransferModel
{
    public class TitlePersons
    {
        public string? TConst { get; set; }
        public string? NConst { get; set; }
        public string? Title { get; set; }
        public string? Name { get; set; }
        public string ProductionRole { get; set; }
        public IList<CharacterListElement> CharacterList { get; set; }
        public float? Popularity { get; set; }
        public bool isActor { get; set; }
        public bool isMovie { get; set; }
        public bool isTvShow { get; set; }
    }
}
