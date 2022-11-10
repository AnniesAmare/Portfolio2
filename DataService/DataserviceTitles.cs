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
                .Take(20).ToList();
            if (tvShows == null) return null;


            foreach (var tvShow in tvShows)
            {
                var inputTConst = tvShow?.TConst?.RemoveSpaces();
                tvShow.TConst = inputTConst;

                tvShow.DirectorList = GetDirectorsForSpecificTitle(inputTConst);
                tvShow.TvShowContentList = GetTvShowListElements(inputTConst);
            }

            return tvShows;

        }

        
        public IList<TvShowListElement> GetTvShowListElements(string parenTConst) {
            using var db = new PortfolioDBContext();

            //var seasonList = new List<TvShowListElement>();

            var tvShowContentList = db.TitleEpisodes
                .Where(x => x.ParentTConst == parenTConst)
                .OrderBy(x => x.SeasonNumber)
                .Select(x => new TvShowListElement
                {
                    Season = x.SeasonNumber
                })
                .Distinct().ToList();

            foreach(var element in tvShowContentList)
            {
                element.Episodes = GetEpisodeListElements(parenTConst);
                
            }


            if (tvShowContentList == null) return null;

            return tvShowContentList;
        }



        public IList<EpisodeListElement> GetEpisodeListElements(string parenTConst)
        {
            using var db = new PortfolioDBContext();

            var episodes = db.TitleEpisodes
              .Where(x => x.ParentTConst == parenTConst)
              .OrderBy(x => x.EpisodeNumber)
              .Select(x => new EpisodeListElement
              {
                  TConst = x.TConst,
                  Episode = x.EpisodeNumber
              })
              .ToList();



            //foreach (var episode in episodes)
            //{
            //    var epNames = db.TitleBasics.Select(x = episod)


            //   .Where(x => x.TConst = episode.TConst);


            //}


            return episodes;
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
