﻿using DataLayer.DatabaseModel;
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
        IList<Titles> GetMovies(int page, int pageSize);
        IList<Titles> GetTvShows(int page, int pageSize);
        public int GetNumberOfMovies();
        public int GetNumberOfTvShows();
        EpisodeListElement GetEpisodeById(string id);
    }
}
