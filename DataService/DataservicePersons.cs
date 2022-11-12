using DataLayer.DatabaseModel;
using DataLayer.DataTransferModel;
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

        public IList<Persons> GetActors()
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
                .Take(15).ToList();
            if (actors == null) return null;

            foreach (var actor in actors) {
                //remove spaces
                var inputTConst = actor.NConst?.RemoveSpaces();
                var inputBirthYear = actor.BirthYear?.RemoveSpaces();
                var inputDeathYear = actor.BirthYear?.RemoveSpaces();

                //get knownforLists
                actor.NConst = inputTConst;
                actor.KnownForMovies = GetKnownForMovies(inputTConst);
                actor.KnownForTvShows = GetKnownForTvShows(inputTConst);

                //replace empty values with meta message
                if (inputBirthYear == "") { actor.BirthYear = "No registered birth date"; }
                if (inputDeathYear == "") { actor.DeathYear = "No registered death date"; }
         
            }


            return actors;

        }

        //Helper functions 
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
