using DataLayer.DatabaseModel;
using DataLayer.DataTransferModel;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace DataLayer
{
    public class DataserviceTitles : IDataserviceTitles
    {
        public IList<Titles> GetMovies() {
            using var db = new PortfolioDBContext();

            var movies = db.TitleBasics
                .Select(x => new Titles
                {
                    TConst = x.TConst,
                    Type = x.TitleType,
                    Name = x.PrimaryTitle,
                    AiringDate = x.StartYear,
                    Rating = x.TitleRating.AverageRating,
                    IsTvShow = x.IsTvShow,
                    IsEpisode = x.IsEpisode,
                    IsMovie = x.IsMovie

                })
                .Where(x => x.IsMovie == true)
                .Take(10).ToList();
            if (movies == null) return null;

            
            foreach(var movie in movies)
            {
                var inputTConst = movie?.TConst?.RemoveSpaces();
                movie.TConst = inputTConst;

                movie.DirectorList = GetDirectorsForSpecificTitle(inputTConst);
            }
            
            return movies;
        }

        public IList<Titles> GetTvShows() {

            using var db = new PortfolioDBContext();

            var tvShows = db.TitleBasics
                .Select(x => new Titles
                {
                    TConst = x.TConst,
                    Type = x.TitleType,
                    Name = x.PrimaryTitle,
                    AiringDate = x.StartYear,
                    Rating = x.TitleRating.AverageRating,
                    IsTvShow = x.IsTvShow,
                    IsEpisode = x.IsEpisode,
                    IsMovie = x.IsMovie

                })
                .Where(x => x.IsTvShow == true)
                .Take(5).ToList();
            if (tvShows == null) return null;


            foreach (var tvShow in tvShows)
            {
                var inputTConst = tvShow?.TConst?.RemoveSpaces();
                tvShow.TConst = inputTConst;

                tvShow.DirectorList = GetDirectorsForSpecificTitle(inputTConst);
            }

            return tvShows;

        }


        //Helper method hijacked from DataserviceSpecificTitle
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
        

    }
}
