using DataLayer.DataTransferModel;
using AutoMapper;

namespace WebServer.Model.Profiles
{
    public class TitlesModelProfile
    {
       public TitlesModelProfile()
        {
            CreateMap<Titles, MoviesModel>();
        }
    }
}
