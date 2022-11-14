using DataLayer.DataTransferModel;
using Microsoft.EntityFrameworkCore;
using System.Runtime.CompilerServices;

namespace DataLayer
{
    public class DataserviceSpecificTitle : IDataserviceSpecificTitle
    {
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

        public IList<TitlePersons> GetTitleCastById(string tConst)
        {
            using var db = new PortfolioDBContext();
            var cast = db.TitlePrincipals
                .Include(x => x.TitleBasic)
                .Include(x => x.NameBasic)
                .Select(x => new TitlePersons
                {
                    TConst = x.TConst,
                    NConst = x.NConst,
                    Title = x.TitleBasic.PrimaryTitle,
                    Name = x.NameBasic.PrimaryName,
                    ProductionRole = x.Category,
                    Popularity = x.NameBasic.AVGNameRating,
                    isActor = x.NameBasic.IsActor,
                    isMovie = x.TitleBasic.IsMovie,
                    isTvShow = x.TitleBasic.IsTvShow

                })
                .Where(x => x.TConst == tConst)
                .Where(x => x.isTvShow == true || x.isMovie == true)
                .Where(x => x.isActor == true)
                .Where(x => x.ProductionRole == "actor" || x.ProductionRole == "actress")
                .OrderBy(x => x.Popularity)
                .ToList();

            if (cast == null) return null;

            foreach (var person in cast)
            {
                //remove spaces
                var inputNConst = person.NConst?.RemoveSpaces();
                person.NConst = inputNConst;
                var inputTConst = person.TConst?.RemoveSpaces();
                person.TConst = inputTConst;

                //get List
                person.Characters = GetCharactersById(person);

            }

            return cast;
        }


        //Helper functions
        public IList<string> GetCharactersById(TitlePersons person)
        {
            using var db = new PortfolioDBContext();
            IList<string> chara = new List<string>();
            var tConst = person.TConst;
            var nConst = person.NConst;
            
            var characters = db.Characters
                .Where(x => x.NConst == nConst)
                .Where(x => x.TConst == tConst);
            
            foreach (var character in characters) { chara.Add(character.TCharacter); }

            return chara;
        }

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


    }
}

