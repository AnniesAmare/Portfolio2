using AutoMapper;
using DataLayer;
using DataLayer.DataServiceInterfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using WebServer.Services;

namespace WebServer.Controllers
{
    [Route("api/search")]
    [ApiController]
    public class SearchController : ControllerBase
    {
        private IDataserviceSearches _dataServiceSearches;
        private readonly LinkGenerator _generator;
        private readonly IMapper _mapper;
        private readonly Hashing _hashing;
        private readonly IConfiguration _configuration;

        private const int MaxPageSize = 30;

        public SearchController(IDataserviceSearches dataServiceSearches, LinkGenerator generator, IMapper mapper,
            Hashing hashing, IConfiguration configuration)
        {
            _dataServiceSearches = dataServiceSearches;
            _generator = generator;
            _mapper = mapper;
            _hashing = hashing;
            _configuration = configuration;
        }

        [HttpGet("actors/{search}", Name = nameof(SearchActors))]
        [Authorize]
        public IActionResult SearchActors(string? search, int page = 0, int pageSize = 20)
        {
            try
            {
                var username = GetUsername();
                var searchResult = _dataServiceSearches.GetSearchResultActors(username, search, page, pageSize);
                var searchResultPaging = PagingForSearch(page, pageSize, searchResult.total, searchResult.searchResult, search, nameof(SearchActors));
                return Ok(searchResultPaging);
            }
            catch
            {
                return Unauthorized();
            }
        }



        [HttpGet("titles/{search}", Name = nameof(SearchTitles))]
        [Authorize]
        public IActionResult SearchTitles(string? search, int page = 0, int pageSize = 20)
        {
            try
            {
                var username = GetUsername();
                var searchResult = _dataServiceSearches.GetSearchResultTitles(username, search, page, pageSize);
                var searchResultPaging = PagingForSearch(page, pageSize, searchResult.total, searchResult.searchResult, search, nameof(SearchTitles));
                return Ok(searchResultPaging);
            }
            catch
            {
                return Unauthorized();
            }
        }
        
        [HttpGet("genres/{search}", Name = nameof(SearchGenres))]
        [Authorize]
        public IActionResult SearchGenres(string? search, int page = 0, int pageSize = 20)
        {
            try
            {
                var username = GetUsername();
                var searchResult = _dataServiceSearches.GetSearchResultTitles(username, search, page, pageSize);
                var searchResultPaging = PagingForSearch(page, pageSize, searchResult.total, searchResult.searchResult, search, nameof(SearchGenres));
                return Ok(searchResultPaging);
            }
            catch
            {
                return Unauthorized();
            }
        }



        [HttpGet("history")]
        [Authorize]
        public IActionResult GetSearchHistory()
        {
            //method to get search history
            return Ok();
        }

        [HttpDelete("history")]
        [Authorize]
        public IActionResult DeleteSearchHistory()
        {
            try
            {
                var username = GetUsername();
                var deleted = _dataServiceSearches.ClearSearchHistory(username);
                if (!deleted)
                {
                    return NotFound();
                }
                return Ok();
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


        private object PagingForSearch<T>(int page, int pageSize, int total, IEnumerable<T> items, string search, string endpointName)
        {
            pageSize = pageSize > MaxPageSize ? MaxPageSize : pageSize;

            var pages = (int)Math.Ceiling((double)total / (double)pageSize);

            var first = total > 0
                ? _generator.GetUriByName(HttpContext, endpointName, new { page = 0, pageSize, search })
                : null;

            var prev = page > 0
                ? _generator.GetUriByName(HttpContext, endpointName, new { page = page - 1, pageSize, search })
                : null;

            var current = _generator.GetUriByName(HttpContext, endpointName, new { page, pageSize, search });

            var next = page < pages - 1
                ? _generator.GetUriByName(HttpContext, endpointName, new { page = page + 1, pageSize, search })
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
