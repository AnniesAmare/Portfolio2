using AutoMapper;
using DataLayer;
using DataLayer.DataTransferModel;
using Microsoft.AspNetCore.Mvc;
using NpgsqlTypes;
using WebServer.Model;

namespace WebServer.Controllers
{
    [Route("api/title")]
    [ApiController]
    public class SpecificTitleController : ControllerBase
    {
        private IDataService _dataService;
        private readonly LinkGenerator _generator;
        private readonly IMapper _mapper;

        public SpecificTitleController(IDataService dataService, LinkGenerator generator, IMapper mapper)
        {
            _dataService = dataService;
            _generator = generator;
            _mapper = mapper;
        }

        [HttpGet("{id}", Name = nameof(GetTitleById))]
        public IActionResult GetTitleById(string id)
        {
            var specificTitle = _dataService.GetSpecificTitle(id);
            if (specificTitle == null)
            {
                return NotFound();
            }
            return Ok(specificTitle);
        }

        [HttpGet("name/{search}")]
        public IActionResult GetTitleByName(string search)
        {
            var specificTitle = _dataService.GetSpecificTitleByName(search);
            if (specificTitle == null)
            {
                return NotFound();
            }
            return Ok(specificTitle);
        }

    }
}
