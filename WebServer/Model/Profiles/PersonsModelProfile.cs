using AutoMapper;
using DataLayer.DataTransferModel;

namespace WebServer.Model.Profiles
{
    public class PersonsModelProfile : Profile
    {
        public PersonsModelProfile()
        {
            CreateMap<Persons, PersonListModel>();
            
        }
    }
}
