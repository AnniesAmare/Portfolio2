using AutoMapper;
using DataLayer;
using DataLayer.DataServiceInterfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using WebServer.Model;
using WebServer.Services;

namespace WebServer.Controllers
{
    [Route("api")]
    [ApiController]
    public class SearchController : BaseController
    {
        private readonly IDataserviceSearches _dataServiceSearches;

        public SearchController(IDataserviceSearches dataServiceSearches, LinkGenerator generator, IMapper mapper, IConfiguration configuration): base(generator, mapper, configuration)
        {
            _dataServiceSearches = dataServiceSearches;
        }

        [HttpGet("search/actors/{search}", Name = nameof(SearchActors))]
        [Authorize]
        public IActionResult SearchActors(string? search, int page = 0, int pageSize = 20)
        {
            try
            {
                var username = GetUsername();
                var searchResult = _dataServiceSearches.GetSearchResultActors(username, search, page, pageSize);
                var total = searchResult.total;
                var searchResultModel = searchResult.searchResult
                    .Select(x => new PersonSearchModel
                    {
                        Name = x.PrimaryName,
                        Url = GenerateLink(nameof(SpecificPersonController.GetPersonById), new { id = x.NConst.RemoveSpaces() })
                    });
                var searchResultPaging = SearchPagingModel(page, pageSize, total, searchResultModel, search, nameof(SearchActors));
                return Ok(searchResultPaging);
            }
            catch
            {
                return Unauthorized();
            }
        }



        [HttpGet("search/titles/{search}", Name = nameof(SearchTitles))]
        [Authorize]
        public IActionResult SearchTitles(string? search, int page = 0, int pageSize = 20)
        {
            try
            {
                var username = GetUsername();
                var searchResult = _dataServiceSearches.GetSearchResultTitles(username, search, page, pageSize);
                var total = searchResult.total;
                var searchResultModel = searchResult.searchResult
                    .Select(x => new TitleSearchModel
                    {
                        Title = x.PrimaryTitle,
                        Rank = x.Rank,
                        Url = GenerateLink(nameof(SpecificTitleController.GetTitleById), new { id = x.TConst.RemoveSpaces() })
                    });
                var searchResultPaging = SearchPagingModel(page, pageSize, total,searchResultModel, search, nameof(SearchTitles));
                return Ok(searchResultPaging);
            }
            catch
            {
                return Unauthorized();
            }
        }
        
        [HttpGet("search/genres/{search}", Name = nameof(SearchGenres))]
        [Authorize]
        public IActionResult SearchGenres(string? search, int page = 0, int pageSize = 20)
        {
            try
            {
                var username = GetUsername();
                var searchResult = _dataServiceSearches.GetSearchResultGenres(username, search, page, pageSize);
                var total = searchResult.total;
                var searchResultModel = searchResult.searchResult
                    .Select(x => new TitleSearchModel
                    {
                        Title = x.PrimaryTitle,
                        Rank = x.Rank,
                        Url = GenerateLink(nameof(SpecificTitleController.GetTitleById), new { id = x.TConst.RemoveSpaces() })
                    });
                var searchResultPaging = SearchPagingModel(page, pageSize, total, searchResultModel, search, nameof(SearchGenres));
                return Ok(searchResultPaging);
            }
            catch
            {
                return Unauthorized();
            }
        }



        [HttpGet("user/history", Name = nameof(GetSearchHistory))]
        [Authorize]
        public IActionResult GetSearchHistory(int page = 0, int pageSize = 20)
        {
            try
            {
                var username = GetUsername();
                var searchHistory = _dataServiceSearches.GetSearchHistory(username);
                var total = searchHistory.Count();
                var searchHistoryList = new List<SearchHistoryListElementModel>();
                foreach (var search in searchHistory)
                {
                    var newSearch = new SearchHistoryListElementModel
                    {
                        Date = search.Date.Date.ToString(),
                        Content = search.Content,
                    };
                    if (search.Category.Contains("Genres"))
                    {
                        newSearch.Url = GenerateLink(nameof(SearchGenres),
                            new { search = search.Content });
                    }
                    if (search.Category.Contains("Titles"))
                    {
                        newSearch.Url = GenerateLink(nameof(SearchTitles),
                            new { search = search.Content });
                    }
                    if (search.Category.Contains("Actors"))
                    {
                        newSearch.Url = GenerateLink(nameof(SearchActors),
                            new { search = search.Content });
                    }
                    searchHistoryList.Add(newSearch);
                }

                var searchHistoryListWithPaging =
                    DefaultPagingModel(page, pageSize, total, searchHistoryList, nameof(GetSearchHistory));
                return Ok(searchHistoryListWithPaging);
            }
            catch
            {
                return Unauthorized();
            }
        }

        [HttpDelete("user/history/delete")]
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

        public object SearchPagingModel<T>(int page, int pageSize, int total, IEnumerable<T> items, string search, string endpointName)
        {
            pageSize = pageSize > MaxPageSize ? MaxPageSize : pageSize;

            var pages = (int)Math.Ceiling((double)total / (double)pageSize);

            var first = total > 0
                ? GenerateLink(endpointName, new { page = 0, pageSize, search })
                : null;

            var prev = page > 0
                ? GenerateLink(endpointName, new { page = page - 1, pageSize, search })
                : null;

            var current = GenerateLink(endpointName, new { page, pageSize, search });

            var next = page < pages - 1
                ? GenerateLink(endpointName, new { page = page + 1, pageSize, search })
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
