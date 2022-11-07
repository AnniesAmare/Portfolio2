
using DataLayer.DatabaseModel;

using System.Collections.Generic;
using DataLayer.DataTransferModel;
using Microsoft.EntityFrameworkCore;
using System.IO;
using System.Runtime.CompilerServices;

namespace DataLayer
{
    public class DataService : IDataService 
    {
        public IList<TitleBasic> GetTitleBasics()
        {
            using var db = new PortfolioDBContext();
            return db.TitleBasics.ToList();
        }


        //SPECIFIC TITLE COMMANDS
        public SpecificTitle GetSpecificTitleByName(string name)
        {
            using var db = new PortfolioDBContext();
            var title = db.TitleBasics
                .Include(x => x.TitleRating)
                .Select(x => new SpecificTitle
                {
                    TConst = x.TConst,
                    Title = x.PrimaryTitle,
                    Runtime = x.RuntimeMinutes,
                    Year = x.StartYear,
                    Rating = x.TitleRating.AverageRating
                })
                .FirstOrDefault(x => x.Title == name);
            if (title == null) return null;
            var tConst = title.TConst;
            var inputTConst = tConst.RemoveSpaces();

            title.ActorList = GetActorsForSpecificTitle(inputTConst);
            title.DirectorList = GetDirectorsForSpecificTitle(inputTConst);
            title.Genre = GetGenreForSpecificTitle(inputTConst);

            return title;
        }

        public SpecificTitle GetSpecificTitle(string tConst)
        {
            using var db = new PortfolioDBContext();
            var inputTConst = tConst.RemoveSpaces();
            var title = db.TitleBasics
                .Include(x => x.TitleRating)
                .Select(x => new SpecificTitle
                {
                    TConst = x.TConst,
                    Title = x.PrimaryTitle,
                    Runtime = x.RuntimeMinutes,
                    Year = x.StartYear,
                    Rating = x.TitleRating.AverageRating
                })
                .FirstOrDefault(x => x.TConst == tConst);
            if (title == null) return null;
            title.ActorList = GetActorsForSpecificTitle(inputTConst);
            title.DirectorList = GetDirectorsForSpecificTitle(inputTConst);
            title.Genre = GetGenreForSpecificTitle(inputTConst);

            return title;
        }

        //Helper functions
        private IList<ActorListElement> GetActorsForSpecificTitle(string tConst)
        {
            using var db = new PortfolioDBContext();
            var actors = db.Characters
                .Include(x => x.NameBasic)
                .Where(x => x.TConst == tConst)
                .OrderBy(x => x.NameBasic.PrimaryName)
                .Select(x => new ActorListElement
                {
                    NConst = x.NConst.RemoveSpaces(),
                    Name = x.NameBasic.PrimaryName,
                    Character = x.TCharacter
                })
                .ToList();

            return actors;
        }

        private IList<DirectorListElement> GetDirectorsForSpecificTitle(string tConst)
        {
            using var db = new PortfolioDBContext();
            var directors = db.TitlePrincipals
                .Include(x => x.NameBasic)
                .Where(x => x.TConst == tConst)
                .Where(x => x.Category == "director")
                .OrderBy(x => x.NameBasic.PrimaryName)
                .Select(x => new DirectorListElement()
                {
                    NConst = x.NConst.RemoveSpaces(),
                    Name = x.NameBasic.PrimaryName
                })
                .ToList();

            return directors;
        }

        private IList<string> GetGenreForSpecificTitle(string tConst)
        {
            using var db = new PortfolioDBContext();
            var genres = db.Genres.Where(x => x.TConst == tConst).Select(x => x.TGenre).ToList();
            return genres;
        }

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

        private IList<string> GetProfessionsForSpecificPerson(string nConst)
        {
            using var db = new PortfolioDBContext();
            var professions = db.TitlePrincipals.Where(x => x.NConst == nConst).Select(x => x.Category).Distinct().ToList();
            return professions;
        }

        private IList<TitleListElement> GetKnownForListForSpecificPerson(string nConst)
        {
            using var db = new PortfolioDBContext();
            var knownForList = db.KnownFors
                .Include(x => x.TitleBasic)
                .Where(x => x.NConst == nConst)
                .Select(x => new TitleListElement
                {
                    TConst = x.TitleBasic.TConst.RemoveSpaces(),
                    Title = x.TitleBasic.PrimaryTitle
                })
                .Distinct().ToList();
            return knownForList;
        }
        
    }
}