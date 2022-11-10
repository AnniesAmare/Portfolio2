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
        public Titles GetMovies() {
            using var db = new PortfolioDBContext();

            var titles = db.TitleBasics
                .Select(x => new Titles
                {
                    TConst = x.TConst,
                    Type = x.TitleType,
                    AiringDate = x.StartYear,
                    IsTvShow = x.IsTvShow,
                    IsEpisode = x.IsEpisode,
                    IsMovie = x.Movie
                   
                })
                .Take(100);
            if (titles == null) return null;

            var inputTConst = titles.TConst.RemoveSpaces();
         

            return titles;

        }

    }
}
