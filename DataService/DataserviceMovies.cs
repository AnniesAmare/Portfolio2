using DataLayer.DataTransferModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace DataLayer
{
    public class DataserviceMovies : IDataserviceMovies
    {
        public IList<Titles> GetMovies() {
            using var db = new PortfolioDBContext();

            var movies = db.TitleBasics
                .Select(x => new Titles
                {
                    TConst = x.TConst,
                    Type = x.TitleType,
                    AiringDate = x.StartYear,
                    IsTvShow = x.IsTvShow,
                    IsEpisode = x.IsEpisode,
                    IsMovie = x.IsMovie
                   
                })
                .Where(x => x.IsMovie)
                .Take(100).ToList();
            if (movies == null) return null;

            foreach(var movie in movies)
            {
                var inputTConst = movie?.TConst?.RemoveSpaces();
                movie.TConst = inputTConst;
            }      
            return movies;
        }

    }
}
