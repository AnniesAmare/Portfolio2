using AutoMapper;
using DataLayer.DataServiceInterfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebServer.Model;

namespace WebServer.Controllers
{
    [Route("api/user/bookmarks")]
    [ApiController]
    public class BookmarksController : BaseController
    {
        private readonly IDataserviceBookmarks _dataServiceBookmarks;

        public BookmarksController(IDataserviceBookmarks dataServiceBookmarks, LinkGenerator generator, IMapper mapper, IConfiguration configuration) : base(generator, mapper, configuration)
        {
            _dataServiceBookmarks = dataServiceBookmarks;
        }

        [HttpGet(Name = nameof(GetBookmarks))]
        [Authorize]
        public IActionResult GetBookmarks(int page = 0, int pageSize = 20)
        {
            try
            {
                var username = GetUsername();
                var bookmarks = _dataServiceBookmarks.GetBookmarks(username, page, pageSize);

                if (bookmarks == null) return NotFound();

                var personBookmarksModel = bookmarks.Where(x => x.isPerson).Select(x => new BookmarksModel
                {
                    Name = x.Annotation,
                    Url = GenerateLink(nameof(SpecificPersonController.GetPersonById), new { id = x.Id })

                }).ToList();

                var titleBookmarksModel = bookmarks.Where(x => x.isTitle).Select(x => new BookmarksModel
                {
                    Name = x.Annotation,
                    Url = GenerateLink(nameof(SpecificTitleController.GetTitleById), new { id = x.Id })

                }).ToList();

                var total = _dataServiceBookmarks.GetNumberOfBookmarks(username);
                var allBookmarks = titleBookmarksModel.Concat(personBookmarksModel);
                var allBookmarksWithPaging = DefaultPagingModel(page, pageSize, total, allBookmarks, nameof(GetBookmarks));
               
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
                var created = _dataServiceBookmarks.CreateBookmark(username, id, name);
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
                var bookmark = _dataServiceBookmarks.NameBookmark(username, id, name);
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
                var deleted = _dataServiceBookmarks.DeleteBookmark(username, id);
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
