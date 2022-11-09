using AutoMapper;
using DataLayer.DataServiceInterfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Security.Claims;
using DataLayer;
using WebServer.Model;
using WebServer.Services;

namespace WebServer.Controllers
{
    [Route("api/user/bookmarks")]
    [ApiController]
    public class BookmarksController : ControllerBase
    {
        private IDataserviceBookmarks _dataServiceBookmarks;
        private readonly LinkGenerator _generator;
        private readonly IMapper _mapper;
        private readonly Hashing _hashing;
        private readonly IConfiguration _configuration;

        public BookmarksController(IDataserviceBookmarks dataServiceBookmarks, LinkGenerator generator, IMapper mapper, Hashing hashing, IConfiguration configuration)
        {
            _dataServiceBookmarks = dataServiceBookmarks;
            _generator = generator;
            _mapper = mapper;
            _hashing = hashing;
            _configuration = configuration;
        }

        [HttpGet]
        [Authorize]
        public IActionResult GetBookmarks()
        {
            try
            {
                var username = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Name).Value;
                var bookmarks = _dataServiceBookmarks.getBookmarks(username);
                var personBookmarks = bookmarks.personBookmarks;
                var titleBookmarks = bookmarks.titleBookmarks;

                if (personBookmarks == null && titleBookmarks == null)
                {
                    return NotFound();
                }

                var personBookmarksModel = personBookmarks.Select(x => new BookmarksModel
                {
                    Name = x.Annotation,
                    Url = _generator.GetUriByName(HttpContext, nameof(SpecificPersonController.GetPersonById), new { id = x.Id.RemoveSpaces() })

                }).ToList();

                var titleBookmarksModel = titleBookmarks.Select(x => new BookmarksModel
                {
                    Name = x.Annotation,
                    Url = _generator.GetUriByName(HttpContext, nameof(SpecificTitleController.GetTitleById), new { id = x.Id.RemoveSpaces() })

                }).ToList();
                
                return Ok(new { titleBookmarksModel , personBookmarksModel });
            }
            catch
            {
                return Unauthorized();
            }
        }



    }
}
