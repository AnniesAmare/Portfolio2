using AutoMapper;
using DataLayer;
using DataLayer.DataTransferModel;
using Microsoft.AspNetCore.Mvc;
using WebServer.Model;

namespace WebServer.Controllers
{
    [Route("api/person")]
    [ApiController]
    public class SpecificPersonController : BaseController
    {
        private readonly IDataserviceSpecificPerson _dataServiceSpecificPerson;

        public SpecificPersonController(IDataserviceSpecificPerson dataServiceSpecificPerson, LinkGenerator generator, IMapper mapper, IConfiguration configuration) : base(generator, mapper, configuration)
        {
            _dataServiceSpecificPerson = dataServiceSpecificPerson;
        }

        [HttpGet("{id}", Name = nameof(GetPersonById))]
        public IActionResult GetPersonById(string id)
        {
            var specificPerson = _dataServiceSpecificPerson.GetSpecificPerson(id);
            if (specificPerson == null)
            {
                return NotFound();
            }
            var specificPersonModel = CreateSpecificPersonModel(specificPerson);
            return Ok(specificPersonModel);
        }

        [HttpGet("name/{name}")]
        public IActionResult GetPersonByName(string name)
        {
            var specificPerson = _dataServiceSpecificPerson.GetSpecificPersonByName(name);
            if (specificPerson == null)
            {
                return NotFound();
            }
            var specificPersonModel = CreateSpecificPersonModel(specificPerson);
            return Ok(specificPersonModel);
        }

        public SpecificPersonModel CreateSpecificPersonModel(SpecificPerson person)
        {
            var model = _mapper.Map<SpecificPersonModel>(person);
            var knownForList = new List<TitleListElementModel>();
            
            foreach (var title in person.KnownForList)
            {
                var newTitle = new TitleListElementModel
                {
                    Title = title.Title,
                    Url = GenerateLink(nameof(SpecificTitleController.GetTitleById), new { id = title.TConst })
                };
                knownForList.Add(newTitle);
            }
            model.KnownForListWithUrl = knownForList;
            model.Bookmark = GenerateLink(nameof(BookmarksController.CreateBookmark), new { id = person.NConst.RemoveSpaces() });
            return model;
        }
        
    }
}

