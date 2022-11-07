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
            var specificTitleModel = CreateSpecificTitleModel(specificTitle);
            return Ok(specificTitleModel);
        }

        [HttpGet("name/{search}")]
        public IActionResult GetTitleByName(string search)
        {
            var specificTitle = _dataService.GetSpecificTitleByName(search);
            if (specificTitle == null)
            {
                return NotFound();
            }
            var specificTitleModel = CreateSpecificTitleModel(specificTitle);
            return Ok(specificTitleModel);
        }


        public SpecificTitleModel CreateSpecificTitleModel(SpecificTitle title)
        {
            var model = _mapper.Map<SpecificTitleModel>(title);
            var directorList = new List<DirectorListElementModel>();
            var actorList = new List<ActorListElementModel>();

            model.DirectorListWithUrl = getDirectorListElementModels(title);
            model.ActorListWithUrl = getActorListElementModels(title);

            return model;
        }

        private List<DirectorListElementModel> getDirectorListElementModels(SpecificTitle title)
        {
            var directorList = new List<DirectorListElementModel>();
            foreach (var director in title.DirectorList)
            {
                var newDirector = new DirectorListElementModel
                {
                    Name = director.Name,
                    Url = _generator.GetUriByName(HttpContext, nameof(SpecificPersonController.GetPersonById), new { id = director.NConst })
                };
                directorList.Add(newDirector);
            }
            return directorList;
        }

        private List<ActorListElementModel> getActorListElementModels(SpecificTitle title)
        {
            var actorList = new List<ActorListElementModel>();
            foreach (var actor in title.ActorList)
            {
                var newActor = new ActorListElementModel
                {
                    Name = actor.Name,
                    Character = actor.Character,
                    Url = _generator.GetUriByName(HttpContext, nameof(SpecificPersonController.GetPersonById), new { id = actor.NConst })
                };
                actorList.Add(newActor);
            }
            return actorList;
        }

    }
}
