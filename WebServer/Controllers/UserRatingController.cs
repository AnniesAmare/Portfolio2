using AutoMapper;
using DataLayer;
using DataLayer.DataServiceInterfaces;
using DataLayer.DataTransferModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Security.Claims;
using WebServer.Model;
using WebServer.Services;

namespace WebServer.Controllers
{
    [Route("api/user/ratings")]
    [ApiController]
    public class UserRatingController : Controller
    {
        private IDataserviceUserRatings _dataServiceRatings;
        private readonly LinkGenerator _generator;
        private readonly IMapper _mapper;
        private readonly Hashing _hashing;
        private readonly IConfiguration _configuration;

        private const int MaxPageSize = 30;

        public UserRatingController(IDataserviceUserRatings dataServiceRatings, LinkGenerator generator, IMapper mapper, Hashing hashing, IConfiguration configuration)
        {
            _dataServiceRatings = dataServiceRatings;
            _generator = generator;
            _mapper = mapper;
            _hashing = hashing;
            _configuration = configuration;
        }

        [HttpPost("create/{id}/{rating}", Name = nameof(CreateUserRating))]
        [Authorize]
        public IActionResult CreateUserRating(string id, int rating)
        {
            try
            {
                var username = GetUsername();
                var created = _dataServiceRatings.InsertUserRating(username,id, rating);

                //Console.WriteLine(created.TConst);
                //Console.WriteLine(created.Rating);
                //Console.WriteLine(created.Username);
                //Console.WriteLine(created.Title);
                if (created == false) return BadRequest();

                

                if (created == true)
                {
                    return Ok();
                }

                return BadRequest();
            }
            catch
            {
                return Unauthorized();
            }
        }

        public string? GetUsername()
        {
            return User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Name)!.Value;
        }


        //public UserRatingModel CreateUserRatingModel(UserRatingElement rating)
        //{
        //    var model = _mapper.Map<UserRatingModel>(rating);

        //    model.Url = _generator.GetUriByName(HttpContext,
        //                nameof(SpecificTitleController.GetTitleById),
        //                new { id = tvShow.TConst })

        //    var newRating = new TitleListElementModel
        //    {
        //        Title = tvShow.Title,
        //        Url = _generator.GetUriByName(HttpContext,
        //                nameof(SpecificTitleController.GetTitleById),
        //                new { id = tvShow.TConst })

        //    };

        //}

    }
}
