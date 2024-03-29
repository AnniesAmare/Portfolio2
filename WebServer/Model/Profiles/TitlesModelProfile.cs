﻿using DataLayer.DataTransferModel;
using AutoMapper;
using System.Runtime.CompilerServices;

namespace WebServer.Model.Profiles
{
    public class TitlesModelProfile : Profile
    {
       public TitlesModelProfile()
        {
            CreateMap<Titles, MovieListModel>();
            CreateMap<Titles, TvShowListModel>();
            CreateMap<Titles, TvShowModel>();
        }
    }
}
