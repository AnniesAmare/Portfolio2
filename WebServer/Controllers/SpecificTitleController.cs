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
        private IDataserviceSpecificTitle _dataServiceSpecificTitle;
        private readonly LinkGenerator _generator;
        private readonly IMapper _mapper;

        public SpecificTitleController(IDataserviceSpecificTitle dataServiceSpecificTitle, LinkGenerator generator, IMapper mapper)
        {
            _dataServiceSpecificTitle = dataServiceSpecificTitle;
            _generator = generator;
            _mapper = mapper;
        }

        [HttpGet("{id}", Name = nameof(GetTitleById))]
        public IActionResult GetTitleById(string id)
        {
            var specificTitle = _dataServiceSpecificTitle.GetSpecificTitle(id);
            if (specificTitle == null)
            {
                return NotFound();
            }
            var specificTitleModel = CreateSpecificTitleModel(specificTitle);
            return Ok(specificTitleModel);
        }

        [HttpGet("crew/{id}", Name = nameof(GetTitleCrewById))]
        public IActionResult GetTitleCrewById(string id)
        {
            var TitleCrew = _dataServiceSpecificTitle.GetTitleCrewById(id);

            if (TitleCrew == null)
            {
                return NotFound();
            }

            var CrewModel = CreateTitleCastModel(TitleCrew);
            return Ok(CrewModel);

        }

        [HttpGet("cast/{id}", Name = nameof(GetTitleCastById))]
        public IActionResult GetTitleCastById(string id)
        {
            var TitleCast = _dataServiceSpecificTitle.GetTitleCastById(id);

            if (TitleCast == null)
            {
                return NotFound();
            }

            var CastModel = CreateTitleCastModel(TitleCast);
            return Ok(CastModel);

        }

        public TitlePersonsModel CreateTitleCastModel(IList<TitlePersons> cast)
        {
            var castModel = new TitlePersonsModel();
            var actors = new List<TitlePersonListModel>();

            string title = "";

            string tConst = "";


            foreach(var person in cast)
            {
                title = person.Title;
                tConst = person.TConst;
                var model = _mapper.Map<TitlePersonListModel>(person);

                model.PersonUrl = _generator.GetUriByName(HttpContext,
                      nameof(SpecificPersonController.GetPersonById),
                      new { id = person.NConst });

                actors.Add(model);
            }

            if (title == "") { title = "No current primary title name"; }

            castModel.TitleUrl = _generator.GetUriByName(HttpContext,
                       nameof(SpecificTitleController.GetTitleById),
                       new { id = tConst });

            castModel.Title = title;
            castModel.PersonsList = actors;

            return castModel;

        }




        [HttpGet("name/{search}")]
        public IActionResult GetTitleByName(string search)
        {
            var specificTitle = _dataServiceSpecificTitle.GetSpecificTitleByName(search);
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
