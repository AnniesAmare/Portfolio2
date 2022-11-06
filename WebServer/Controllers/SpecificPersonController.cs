using AutoMapper;
using DataLayer;
using DataLayer.DataTransferModel;
using Microsoft.AspNetCore.Mvc;

namespace WebServer.Controllers
{
    [Route("api/person")]
    [ApiController]
    public class SpecificNameController : ControllerBase
    {
        private IDataService _dataService;
        private readonly LinkGenerator _generator;
        private readonly IMapper _mapper;

        public SpecificNameController(IDataService dataService, LinkGenerator generator, IMapper mapper)
        {
            _dataService = dataService;
            _generator = generator;
            _mapper = mapper;
        }

        [HttpGet("{id}")]
        public IActionResult GetPersonById(string id)
        {
            var specificPerson = _dataService.GetSpecificPerson(id);
            if (specificPerson == null)
            {
                return NotFound();
            }
            return Ok(specificPerson);
        }

        [HttpGet("name/{search}")]
        public IActionResult GetPersonByName(string search)
        {
            var specificPerson = _dataService.GetSpecificPersonByName(search);
            if (specificPerson == null)
            {
                return NotFound();
            }
            return Ok(specificPerson);
        }





    }
}

