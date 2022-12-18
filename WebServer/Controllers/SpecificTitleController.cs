using AutoMapper;
using AutoMapper.Configuration.Annotations;
using DataLayer;
using DataLayer.DataTransferModel;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using WebServer.Model;

namespace WebServer.Controllers
{
    [Route("api/title")]
    [ApiController]
    public class SpecificTitleController : BaseController
    {
        private readonly IDataserviceSpecificTitle _dataServiceSpecificTitle;

        public SpecificTitleController(IDataserviceSpecificTitle dataServiceSpecificTitle, LinkGenerator generator, IMapper mapper, IConfiguration configuration) : base(generator, mapper, configuration)
        {
            _dataServiceSpecificTitle = dataServiceSpecificTitle;
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

        [HttpGet("tvshow/{id}", Name = nameof(GetTvShowById))]
        public IActionResult GetTvShowById(string id)
        {
            var tvShow = _dataServiceSpecificTitle.GetTvShowById(id);

            if (tvShow == null)
            {
                return NotFound();
            }

            var tvShowContent = CreateTvShowModel(tvShow);

            //var tvShowModelElement = _mapper.Map<TvShowListElement>(tvShow);
            TvShowListModel tvShowModelElement = new TvShowListModel();
            tvShowModelElement.Name = tvShow.Name;
            tvShowModelElement.AiringDate = tvShow.AiringDate;
            tvShowModelElement.TvShowContentList = tvShowContent;
            tvShowModelElement.Rating = tvShow.Rating;
            tvShowModelElement.Url = 
                GenerateLink(nameof(GetTvShowById), new { id = tvShow.TConst });

            IList<DirectorListElementModel> directors = new List<DirectorListElementModel>();


            foreach (var director in tvShow.DirectorList)
            {
                DirectorListElementModel newDirector = new DirectorListElementModel();

                newDirector.Name = director.Name;

                newDirector.Url = GenerateLink(nameof(SpecificPersonController.GetPersonById), new { id = director.NConst });

                directors.Add(newDirector);
            }
            tvShowModelElement.DirectorListWithUrl = directors;

            tvShowModelElement.TvShowContentList = tvShowContent;

            return Ok(tvShowModelElement);
        }

        [HttpGet("episode/{id}", Name = nameof(GetEpisodeById))]
        public IActionResult GetEpisodeById(string id)
        {
            DataserviceTitles instance = new DataserviceTitles();
            var episode = instance.GetEpisodeById(id);

            if (episode == null)
            {
                return NotFound();
            }


            var episodeModel = _mapper.Map<EpisodeModel>(episode);
      
            episodeModel.URL = GenerateLink(nameof(GetEpisodeById), new { id = episode.TConst});

            return Ok(episodeModel);
        }




        public IList<TvShowModel> CreateTvShowModel(Titles tvShow)
        {

            IList<TvShowModel> tvShowContent = new List<TvShowModel>();

            IList<EpisodeModel> episodes = new List<EpisodeModel>();

            foreach (var season in tvShow.TvShowContentList)
            {
                var newSeason = new TvShowModel();

                newSeason.Season = season.Season;
                
                foreach (var episode in season.Episodes)
                {
                    var tConst = episode.TConst.RemoveSpaces();
                    var newEpisode = new EpisodeModel{

                        Episode = episode.Episode,
                        Name = episode.Name,
                        URL = GenerateLink(nameof(GetEpisodeById), new { id = tConst })
                    };

                    episodes.Add(newEpisode);

                }

                newSeason.Episodes = episodes;

                tvShowContent.Add(newSeason);
            }



            return tvShowContent;
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
            if (CrewModel.TitleUrl == null) { return BadRequest(); }
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
            if (CastModel.TitleUrl == null) { return BadRequest(); }
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

        [HttpGet("name/{name}")]
        public IActionResult GetTitleByName(string name)
        {
            var specificTitle = _dataServiceSpecificTitle.GetSpecificTitleByName(name);
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

            model.DirectorListWithUrl = getDirectorListElementModels(title);
            model.ActorListWithUrl = getActorListElementModels(title);
            model.Bookmark = GenerateLink(nameof(BookmarksController.CreateBookmark), new { id = title.TConst.RemoveSpaces() });

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
                    Url = GenerateLink(nameof(SpecificPersonController.GetPersonById), new { id = director.NConst })
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
                    Url = GenerateLink(nameof(SpecificPersonController.GetPersonById), new { id = actor.NConst })
                };
                actorList.Add(newActor);
            }
            return actorList;
        }

    }
}
