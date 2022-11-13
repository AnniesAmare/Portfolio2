using AutoMapper;
using DataLayer.DataServiceInterfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Security.Claims;
using DataLayer;
using WebServer.Model;
using WebServer.Services;
using System.Xml.Linq;

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

        [HttpGet(Name = nameof(GetBookmarks))]
        [Authorize]
        public IActionResult GetBookmarks()
        {
            try
            {
                var username = GetUsername();
                var (titleBookmarks, personBookmarks) = _dataServiceBookmarks.getBookmarks(username);

                if (personBookmarks == null && titleBookmarks == null) return NotFound();

                var personBookmarksModel = personBookmarks.Select(x => new BookmarksModel
                {
                    Name = x.Annotation,
                    Url = _generator.GetUriByName(HttpContext, nameof(SpecificPersonController.GetPersonById), new { id = x.Id })

                }).ToList();

                var titleBookmarksModel = titleBookmarks.Select(x => new BookmarksModel
                {
                    Name = x.Annotation,
                    Url = _generator.GetUriByName(HttpContext, nameof(SpecificTitleController.GetTitleById), new { id = x.Id })

                }).ToList();
                
                return Ok(new { titleBookmarksModel , personBookmarksModel });
            }
            catch
            {
                return Unauthorized();
            }
            
        }


        [HttpPost("create/{id}/{name?}", Name = nameof(CreateBookmark))]
        [Authorize]
        public IActionResult CreateBookmark(string id, string? name)
        {
            try
            {
                var username = GetUsername();
                var created = _dataServiceBookmarks.createBookmark(username, id, name);
                if (!created) return BadRequest();
                return Ok();
            }
            catch
            {
                return Unauthorized();
            }
        }

        [HttpPut("rename/{id}/{name}", Name = nameof(RenameBookmark))]
        [Authorize]
        public IActionResult RenameBookmark(string id, string name)
        {
            try
            {
                var username = GetUsername();
                var bookmark = _dataServiceBookmarks.nameBookmark(username, id, name);
                if (bookmark == null) return NotFound();
                return Ok();
            }
            catch
            {
                return Unauthorized();
            }
        }

        [HttpDelete("delete/{id}", Name = nameof(DeleteBookmark))]
        [Authorize]
        public IActionResult DeleteBookmark(string id)
        {
            try
            {
                var username = GetUsername();
                var deleted = _dataServiceBookmarks.deleteBookmark(username, id);
                if (!deleted) return NotFound();
                return Ok();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return Unauthorized();
            }
        }

        public string? GetUsername()
        {
            return User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Name)!.Value;
        }

    }
}
