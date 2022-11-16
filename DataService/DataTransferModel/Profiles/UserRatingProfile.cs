using AutoMapper;
using DataLayer.DatabaseModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.DataTransferModel.Profiles
{
    public class UserRatingProfile : Profile
    {

        public UserRatingProfile()
        {
            //CreateMap<UserRating, UserRatingElement>();
            //CreateMap<UserRating, UserRatingElement>()
            //    .ForMember(dst => dst.TConst, opt => opt.MapFrom(src => src.TConst));

            CreateMap<UserRating, UserRatingElement>();
            CreateMap<UserRating, UserRatingElement>()
                .ForMember(dst => dst.TConst, opt => opt.MapFrom(src => src.TConst));

        }
    }
}
