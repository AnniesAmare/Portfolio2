using AutoMapper;
using DataLayer;
using DataLayer.DataServiceInterfaces;
using DataLayer.DataTransferModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Routing;
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

        [HttpGet("{username}", Name = nameof(GetUserRatings))]
        [Authorize]
        public IActionResult GetUserRatings(string username, int page = 0, int pageSize = 20)
        {            
            try
            { 
                var ratings = _dataServiceRatings.GetUserRatings(username, page, pageSize);
                if (ratings == null)
                {
                    return NotFound();
                }
                var ratingsModel = CreateUserRatingModel(ratings);

                var total = _dataServiceRatings.GetNumberOfUserRatings(username, page, pageSize);

                var url = _generator.GetUriByName
                    (HttpContext, nameof(UserRatingController.GetUserRatings),
                    new { page, pageSize });
                Console.WriteLine(url);

                return Ok(PagingForUserRatings(page, pageSize, total, ratingsModel));


            }
            catch
            {
                return Unauthorized();
            }
        }

        [HttpPost("create/{id}/{rating}", Name = nameof(CreateUserRating))]
        [Authorize]
        public IActionResult CreateUserRating(string id, int rating)
        {
            try
            {
                var username = GetUsername();
                var created = _dataServiceRatings.InsertUserRating(username,id, rating);     

                if (created == true)
                {
                    return Ok();
                }
                else { 
                    return BadRequest(); 
                }

                
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


        public IList<UserRatingModel> CreateUserRatingModel(IList<UserRatingElement> ratings)
        {
            var ratingModel = new List<UserRatingModel>();

            foreach (var rating in ratings)
            {
                var tConst = rating.TConst.RemoveSpaces();
                var model = _mapper.Map<UserRatingModel>(rating);

                model.Url = _generator.GetUriByName(HttpContext,
                            nameof(SpecificTitleController.GetTitleById),
                            new { id = tConst });

                ratingModel.Add(model);
            }

            return ratingModel;
        }

        [HttpDelete("delete/{id}", Name = nameof(DeleteUserRating))]
        [Authorize]
        public IActionResult DeleteUserRating(string id)
        {
            try
            {
                var username = GetUsername();
                var deleted = _dataServiceRatings.DeleteUserRatings(username, id);
                if (!deleted) return NotFound();
                return Ok();
            }
            catch
            {
                return Unauthorized();
            }
        }


        //Helper functions

        private string? CreateUserRatingLink(int page, int pageSize)
        {
            return _generator.GetUriByName(HttpContext, 
                nameof(GetUserRatings), 
                new { page, pageSize });
        }

        private object PagingForUserRatings<T>(int page, int pageSize, int total, IEnumerable<T> items)
        {
            pageSize = pageSize > MaxPageSize ? MaxPageSize : pageSize;

            var pages = (int)Math.Ceiling((double)total / (double)pageSize);

            var first = total > 0
                ? CreateUserRatingLink(0, pageSize)
                : null;

            var prev = page > 0
                ? CreateUserRatingLink(page - 1, pageSize)
                : null;

            var current = CreateUserRatingLink(page, pageSize);

            var next = page < pages - 1
                ? CreateUserRatingLink(page + 1, pageSize)
                : null;

            var result = new
            {
                first,
                prev,
                next,
                current,
                total,
                pages,
                items
            };
            return result;
        }

    }
}
