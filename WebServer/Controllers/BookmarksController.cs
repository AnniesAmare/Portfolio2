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
        
        private const int MaxPageSize = 30;

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
        public IActionResult GetBookmarks(int page = 0, int pageSize = 20)
        {
            try
            {
                var username = GetUsername();
                var bookmarks = _dataServiceBookmarks.getBookmarks(username);

                if (bookmarks == null) return NotFound();

                var personBookmarksModel = bookmarks.Where(x => x.isPerson).Select(x => new BookmarksModel
                {
                    Name = x.Annotation,
                    Url = _generator.GetUriByName(HttpContext, nameof(SpecificPersonController.GetPersonById), new { id = x.Id })

                }).ToList();

                var titleBookmarksModel = bookmarks.Where(x => x.isTitle).Select(x => new BookmarksModel
                {
                    Name = x.Annotation,
                    Url = _generator.GetUriByName(HttpContext, nameof(SpecificTitleController.GetTitleById), new { id = x.Id })

                }).ToList();

                var total = _dataServiceBookmarks.GetNumberOfBookmarks(username);
                var allBookmarks = titleBookmarksModel.Concat(personBookmarksModel);
                var allBookmarksWithPaging = PagingForBookmarks(page, pageSize, total, allBookmarks);
               
                return Ok(allBookmarksWithPaging);
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

        private string? CreateBookmarksLink(int page, int pageSize)
        {
            return _generator.GetUriByName(HttpContext, nameof(GetBookmarks), new { page, pageSize });
        }

        private object PagingForBookmarks<T>(int page, int pageSize, int total, IEnumerable<T> items)
        {
            pageSize = pageSize > MaxPageSize ? MaxPageSize : pageSize;

            var pages = (int)Math.Ceiling((double)total / (double)pageSize);

            var first = total > 0
                ? CreateBookmarksLink(0, pageSize)
                : null;

            var prev = page > 0
                ? CreateBookmarksLink(page - 1, pageSize)
                : null;

            var current = CreateBookmarksLink(page, pageSize);

            var next = page < pages - 1
                ? CreateBookmarksLink(page + 1, pageSize)
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
