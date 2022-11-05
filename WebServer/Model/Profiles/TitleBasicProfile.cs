using AutoMapper;
using DataLayer.DatabaseModel;

namespace WebServer.Model.Profiles
{
    public class TitleBasicProfile : Profile
    {
        public TitleBasicProfile()
        {
            CreateMap<TitleBasic, TitleBasicModel>();

        }
    }
}
