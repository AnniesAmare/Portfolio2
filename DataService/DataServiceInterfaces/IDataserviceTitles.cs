using DataLayer.DatabaseModel;
using DataLayer.DataTransferModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer
{
    public interface IDataserviceTitles
    {
        IList<Titles> GetMovies();
        IList<Titles> GetTvShows();
        Titles GetTvShowsById(string TConst);
    }
}
