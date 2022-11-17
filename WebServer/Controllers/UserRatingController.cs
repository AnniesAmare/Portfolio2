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
    public class UserRatingController : BaseController
    {
        private readonly IDataserviceUserRatings _dataServiceRatings;

        public UserRatingController(IDataserviceUserRatings dataServiceRatings, LinkGenerator generator, IMapper mapper, IConfiguration configuration) : base(generator, mapper, configuration)
        {
            _dataServiceRatings = dataServiceRatings;
        }

        [HttpGet(Name = nameof(GetUserRatings))]
        [Authorize]
        public IActionResult GetUserRatings(int page = 0, int pageSize = 20)
        {
            try
            {
                var username = GetUsername();
                var ratings = _dataServiceRatings.GetUserRatings(username, page, pageSize);
                if (ratings == null)
                {
                    return NotFound();
                }
                var ratingsModel = CreateUserRatingModel(ratings);

                var total = _dataServiceRatings.GetNumberOfUserRatings(username, page, pageSize);

                return Ok(DefaultPagingModel(page, pageSize, total, ratingsModel, nameof(GetUserRatings)));
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

                if (!created)
                {
                    return BadRequest();
                }
                return Ok();
            }
            catch
            {
                return Unauthorized();
            }
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

    }
}
