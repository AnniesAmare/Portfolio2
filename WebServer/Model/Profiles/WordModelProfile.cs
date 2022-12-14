using AutoMapper;
using DataLayer.DatabaseModel.WordCloudModel;
using DataLayer.DataTransferModel;

namespace WebServer.Model.Profiles
{
    public class WordModelProfile : Profile
    {
        public WordModelProfile()
        {
            CreateMap<WordObject, WordModel>()
                .ForMember(dst => dst.Text, opt => opt.MapFrom(src => src.Word))
                .ForMember(dst => dst.Weight, opt => opt.MapFrom(src => src.Rank));
        }
    }
}