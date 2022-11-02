using AutoMapper;
using DataLayer.DatabaseModel;

namespace WebServer.Model.Profiles
{
    public class TitleBasicsProfile : Profile
    {
        public TitleBasicsProfile()
        {
            CreateMap<TitleBasics, TitleBasicsModel>();

        }
    }
}
