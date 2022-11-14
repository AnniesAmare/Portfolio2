using AutoMapper;
using DataLayer.DataTransferModel;

namespace WebServer.Model.Profiles
{
    public class TitlePersonModelProfile : Profile
    {
        public TitlePersonModelProfile()
        {
            CreateMap<TitlePersons, TitlePersonListModel>();

        }
    }
}
