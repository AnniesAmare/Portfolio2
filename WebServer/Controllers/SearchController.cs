using AutoMapper;
using DataLayer.DataServiceInterfaces;
using Microsoft.AspNetCore.Mvc;
using WebServer.Services;

namespace WebServer.Controllers
{
    [Route("api/user/bookmarks")]
    [ApiController]
    public class SearchController : ControllerBase
    {
        private IDataserviceSearches _dataServiceSearches;
        private readonly LinkGenerator _generator;
        private readonly IMapper _mapper;
        private readonly Hashing _hashing;
        private readonly IConfiguration _configuration;

        private const int MaxPageSize = 30;

        public SearchController(IDataserviceSearches dataServiceSearches, LinkGenerator generator, IMapper mapper, Hashing hashing, IConfiguration configuration)
        {
            _dataServiceSearches = dataServiceSearches;
            _generator = generator;
            _mapper = mapper;
            _hashing = hashing;
            _configuration = configuration;
        }
    
    }
}
