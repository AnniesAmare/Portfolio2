
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
            var inputTConst = string.Concat(tConst.Where(c => !char.IsWhiteSpace(c)));

            title.ActorList = GetActorsForSpecificTitle(inputTConst);
            title.DirectorList = GetDirectorsForSpecificTitle(inputTConst);
            title.Genre = GetGenreForSpecificTitle(inputTConst);

            return title;
        }

        public SpecificTitle GetSpecificTitle(string tConst)
        {
            using var db = new PortfolioDBContext();
            var inputTConst = string.Concat(tConst.Where(c => !char.IsWhiteSpace(c)));
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
                    NConst = x.NConst,
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
                    NConst = x.NConst,
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