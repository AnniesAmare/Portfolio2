using AutoMapper;
using DataLayer;
using DataLayer.DataTransferModel;
using Microsoft.AspNetCore.Mvc;
using WebServer.Model;

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

        [HttpGet("{id}", Name = nameof(GetPersonById))]
        public IActionResult GetPersonById(string id)
        {
            var specificPerson = _dataService.GetSpecificPerson(id);
            if (specificPerson == null)
            {
                return NotFound();
            }
            var specificPersonModel = createSpecificPersonModel(specificPerson);
            return Ok(specificPersonModel);
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

        public SpecificPersonModel createSpecificPersonModel(SpecificPerson person)
        {
            var model = _mapper.Map<SpecificPersonModel>(person);
            var knownForList = new List<TitleListElementModel>();
            
            foreach (var title in person.KnownForList)
            {
                var newTitle = new TitleListElementModel
                {
                    Title = title.Title,
                    Url = _generator.GetUriByName(HttpContext, nameof(SpecificTitleController.GetTitleById), new { id = title.TConst })
                };
                knownForList.Add(newTitle);
            }
            model.KnownForListWithUrl = knownForList;

            return model;
        }


    }
}

