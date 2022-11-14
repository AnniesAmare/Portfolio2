using DataLayer.DatabaseModel;
using DataLayer.DataTransferModel;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer
{
    public class DataservicePersons : IDataservicePersons
    {

        public IList<Persons> GetActors(int page, int pageSize)
        {
            using var db = new PortfolioDBContext();
            var actors = db.NameBasics
                .Select(x => new Persons
                {
                    NConst = x.NConst,
                    Name = x.PrimaryName,
                    BirthYear = x.BirthYear,
                    DeathYear = x.DeathYear,
                    Popularity = x.AVGNameRating,
                    isActor = x.IsActor,
                })
                .Where(x => x.isActor == true)
                .OrderBy(x => x.Popularity)
                .Skip(page * pageSize)
                .Take(pageSize)
                .ToList();
            if (actors == null) return null;

            foreach (var actor in actors) {
                //remove spaces
                var inputTConst = actor.NConst?.RemoveSpaces();
                actor.NConst = inputTConst;
                var inputBirthYear = actor.BirthYear?.RemoveSpaces();
                var inputDeathYear = actor.BirthYear?.RemoveSpaces();

                //get knownforLists
                actor.NConst = inputTConst;
                actor.KnownForMovies = GetKnownForMovies(inputTConst);
                actor.KnownForTvShows = GetKnownForTvShows(inputTConst);

                //replace empty/null values
                if (inputBirthYear == "") { actor.BirthYear = "No registered birth date"; }
                if (inputDeathYear == "") { actor.DeathYear = "No registered death date"; }

            }


            return actors;

        }


        public IList<TitlePersons> GetCastByTitleId(string tConst, int page, int pageSize)
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
                .Where(x => x.isActor == true)
                .Where(x => x.isMovie == true)
                .Where(x => x.isTvShow == true)
                .OrderBy(x => x.Popularity)
                .Skip(page * pageSize)
                .Take(pageSize)
                .ToList();

            if (cast == null) return null;

            foreach (var person in cast)
            {
                //remove spaces
                var inputNConst = person.NConst?.RemoveSpaces();
                person.NConst = inputNConst;
                var inputTConst = person.TConst?.RemoveSpaces();
                person.TConst = inputTConst;

                //get Lists
                person.Characters = GetCharactersById(inputNConst);

            }

            return cast;
        }

        //Helper functions 
        public int GetNumberOfActors()
        {
            using var db = new PortfolioDBContext();
             
            return db.NameBasics
                .Select(x => new Persons { isActor = x.IsActor})
                .Where(x => x.isActor == true).Count();
        }

       public IList<string> GetCharactersById(string NConst)
        {
            using var db = new PortfolioDBContext();
            IList<string> chara = new List<string>();

            var characters = db.Characters
                .Include(x => x.NameBasic)
                .Where(x => x.NConst == NConst);
            foreach(var character in characters){chara.Add(character.TCharacter);}

            return chara;
        }

    
        public IList<TitleListElement> GetKnownForMovies(string nConst)
        {
            DataserviceSpecificPerson anInstance = new DataserviceSpecificPerson();
            var knownForTitles = anInstance.GetKnownForListForSpecificPerson(nConst);
            IList<TitleListElement> knownForMovies = new List<TitleListElement>();
            foreach (var knownForTitle in knownForTitles)
            {
                if (knownForTitle.IsMovie == true)
                {
                    knownForMovies.Add(knownForTitle);
                }
            }

            if (knownForMovies == null) return null;

            return knownForMovies;

        }

        public IList<TitleListElement> GetKnownForTvShows(string nConst)
        {
            DataserviceSpecificPerson anInstance = new DataserviceSpecificPerson();
            var knownForTitles = anInstance.GetKnownForListForSpecificPerson(nConst);
            IList<TitleListElement> knownForTvShows = new List<TitleListElement>();
            foreach (var knownForTitle in knownForTitles)
            {
                if (knownForTitle.IsTvShow == true)
                {
                    knownForTvShows.Add(knownForTitle);
                }
            }

            if (knownForTvShows == null) return null;

            return knownForTvShows;

        }
    }
}
