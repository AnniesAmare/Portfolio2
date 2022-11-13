using DataLayer.DatabaseModel;
using DataLayer.DataTransferModel;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace DataLayer
{
    public class DataserviceTitles : IDataserviceTitles
    {
        public IList<Titles> GetMovies(int page, int pageSize) {
            using var db = new PortfolioDBContext();

            var movies = db.TitleBasics
                .Select(x => new Titles
                {
                    TConst = x.TConst,
                    Name = x.PrimaryTitle,
                    AiringDate = x.StartYear,
                    Rating = x.TitleRating.AverageRating,
                    IsTvShow = x.IsTvShow,
                    IsEpisode = x.IsEpisode,
                    IsMovie = x.IsMovie

                })
                .Where(x => x.IsMovie == true)
                .Skip(page * pageSize)
                .Take(pageSize)
                .ToList();
            if (movies == null) return null;

            
            foreach(var movie in movies)
            {
                var inputTConst = movie?.TConst?.RemoveSpaces();
                movie.TConst = inputTConst;

                movie.DirectorList = GetDirectorsForSpecificTitle(inputTConst);
            }
            
            return movies;
        }

        public IList<Titles> GetTvShows(int page, int pageSize) {

            using var db = new PortfolioDBContext();

            var tvShows = db.TitleBasics
                .Select(x => new Titles
                {
                    TConst = x.TConst,
                    Name = x.PrimaryTitle,
                    AiringDate = x.StartYear,
                    Rating = x.TitleRating.AverageRating,
                    IsTvShow = x.IsTvShow,
                    IsEpisode = x.IsEpisode,
                    IsMovie = x.IsMovie

                })
                .Where(x => x.IsTvShow == true)
                .Skip(page * pageSize)
                .Take(pageSize)
                .ToList();
            if (tvShows == null) return null;


            foreach (var tvShow in tvShows)
            {               
                var inputTConst = tvShow?.TConst?.RemoveSpaces();
                tvShow.TConst = inputTConst;
                tvShow.TvShowContentList = GetTvShowListElements(inputTConst);
                tvShow.DirectorList = GetDirectorsForSpecificTitle(inputTConst);  
            }
            return tvShows;
        }

        public Titles GetTvShowById(string TConst)
        {

            using var db = new PortfolioDBContext();

            var tvShow = db.TitleBasics
                .Select(x => new Titles
                {
                    TConst = x.TConst,
                    Type = x.TitleType,
                    Name = x.PrimaryTitle,
                    AiringDate = x.StartYear,
                    Rating = x.TitleRating.AverageRating,
                    IsTvShow = x.IsTvShow
                })
                .FirstOrDefault(x => x.TConst == TConst);
            if (tvShow == null) return null;

            var inputTConst = tvShow?.TConst?.RemoveSpaces();
            tvShow.TConst = inputTConst;
            tvShow.DirectorList = GetDirectorsForSpecificTitle(inputTConst);
            tvShow.TvShowContentList = GetTvShowListElements(inputTConst);

            return tvShow;
        }

        //Helper functions
        public int GetNumberOfMovies()
        {
            using var db = new PortfolioDBContext();

            return db.TitleBasics
                .Select(x => new Titles{ IsMovie = x.IsMovie })
                .Where(x => x.IsMovie == true).Count();
        }

        public int GetNumberOfTvShows()
        {
            using var db = new PortfolioDBContext();

            return db.TitleBasics
                .Select(x => new Titles { IsTvShow = x.IsTvShow })
                .Where(x => x.IsTvShow == true).Count();
        }
        public IList<TvShowListElement> GetTvShowListElements(string parenTConst) {
            using var db = new PortfolioDBContext();

            var tvShowContentList = db.TitleEpisodes
                .Where(x => x.ParentTConst == parenTConst)
                .OrderBy(x => x.SeasonNumber)
                .Select(x => new TvShowListElement
                {
                    Season = x.SeasonNumber
                })
                .Distinct().ToList();

            foreach(var tvShowElement in tvShowContentList)
            {
                tvShowElement.Episodes 
                    = GetEpisodeListElements(parenTConst, tvShowElement.Season);   
            }

            if (tvShowContentList == null) return null;

            return tvShowContentList;
        }



        public IList<EpisodeListElement> GetEpisodeListElements(string parenTConst, int? seasonNum)
        {
            using var db = new PortfolioDBContext();            
            
            var episodes = db.TitleEpisodes
                .Where(x => x.ParentTConst == parenTConst)
                .Where(x => x.SeasonNumber == seasonNum)
                .OrderBy(x => x.EpisodeNumber)
                .Select(x => new EpisodeListElement
                {
                    TConst = x.TConst,
                    Episode = x.EpisodeNumber
                })
                .ToList();

            foreach (var episode in episodes)
            {
                var inputTConst = episode.TConst?.RemoveSpaces();
                episode.Name = getEpisodeName(inputTConst);
            }

            return episodes;
        }


        public string getEpisodeName(string TConst)
        {
            using var db = new PortfolioDBContext();

            var name = db.TitleBasics
                .Where(x => x.TConst == TConst)
                .Select(x => x.PrimaryTitle).FirstOrDefault();

            if (name == null) return null;

            return name;
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
