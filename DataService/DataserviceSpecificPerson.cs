using DataLayer.DataTransferModel;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer
{
    public class DataserviceSpecificPerson : IDataserviceSpecificPerson
    {
        //SPECIFIC PERSON COMMANDS
        public SpecificPerson GetSpecificPersonByName(string name)
        {
            using var db = new PortfolioDBContext();

            var person = db.NameBasics
                .Select(x => new SpecificPerson
                {
                    NConst = x.NConst,
                    Name = x.PrimaryName,
                    BirthYear = x.BirthYear,
                    DeathYear = x.DeathYear
                })
                .FirstOrDefault(x => x.Name == name);
            if (person == null) return null;

            var inputNConst = person.NConst.RemoveSpaces();
            person.ProfessionList = GetProfessionsForSpecificPerson(inputNConst);
            person.KnownForList = GetKnownForListForSpecificPerson(inputNConst);

            return person;
        }

        public SpecificPerson GetSpecificPerson(string nConst)
        {
            using var db = new PortfolioDBContext();
            var inputNConst = nConst.RemoveSpaces();

            var person = db.NameBasics
                .Select(x => new SpecificPerson
                {
                    NConst = x.NConst,
                    Name = x.PrimaryName,
                    BirthYear = x.BirthYear,
                    DeathYear = x.DeathYear
                })
                .FirstOrDefault(x => x.NConst == nConst);
            if (person == null) return null;


            person.ProfessionList = GetProfessionsForSpecificPerson(inputNConst);
            person.KnownForList = GetKnownForListForSpecificPerson(inputNConst);

            return person;
        }

        //Helper functions
        private IList<string> GetProfessionsForSpecificPerson(string nConst)
        {
            using var db = new PortfolioDBContext();
            var professions = db.TitlePrincipals.Where(x => x.NConst == nConst).Select(x => x.Category).Distinct().ToList();
            return professions;
        }

        public IList<TitleListElement> GetKnownForListForSpecificPerson(string nConst)
        {
            using var db = new PortfolioDBContext();
            var knownForList = db.KnownFors
                .Include(x => x.TitleBasic)
                .Where(x => x.NConst == nConst)
                .Select(x => new TitleListElement
                {
                    TConst = x.TitleBasic.TConst.RemoveSpaces(),
                    Title = x.TitleBasic.PrimaryTitle,
                    IsTvShow = x.TitleBasic.IsTvShow,
                    IsMovie = x.TitleBasic.IsMovie
                })
                .Distinct().ToList();
            return knownForList;
        }

    }
}
