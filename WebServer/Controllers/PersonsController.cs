using AutoMapper;
using DataLayer;
using DataLayer.DataTransferModel;
using Microsoft.AspNetCore.Mvc;
using WebServer.Model;

namespace WebServer.Controllers
{
    [Route("api/persons")]
    [ApiController]
    public class PersonsController : Controller
    {
        private IDataservicePersons _dataServicePersons;
        private readonly LinkGenerator _generator;
        private readonly IMapper _mapper;

        private const int MaxPageSize = 30;

        public PersonsController(IDataservicePersons dataServicePersons, LinkGenerator generator, IMapper mapper)
        {
            _dataServicePersons = dataServicePersons;
            _generator = generator;
            _mapper = mapper;
        }

        [HttpGet("actors", Name = nameof(GetActors))]
        public IActionResult GetActors()
        {
            var Actors = _dataServicePersons.GetActors();
            if (Actors == null)
            {
                return NotFound();
            }
            var ActorsModel = CreatePersonsModel(Actors);
            return Ok(ActorsModel);
        }


        public IList<PersonsModel> CreatePersonsModel(IList<Persons> persons)
        {
            var personsModel = new List<PersonsModel>();
            var knownForMoviesList = new List<TitleListElementModel>();
            var knownForTvShowsList = new List<TitleListElementModel>();

            foreach (var person in persons)
            {
                var model = _mapper.Map<PersonsModel>(person);

                //make moviesListWithUrl
                foreach (var movie in person.KnownForMovies)
                {
                    var newTitle = new TitleListElementModel
                    {
                        Title = movie.Title,
                        Url = _generator.GetUriByName(HttpContext,
                        nameof(SpecificTitleController.GetTitleById),
                        new { id = movie.TConst })

                    };
                   knownForMoviesList.Add(newTitle);
                }

                //make tvShowListWithUrl
                foreach (var tvShow in person.KnownForTvShows)
                {
                    var newTitle = new TitleListElementModel
                    {
                        Title = tvShow.Title,
                        Url = _generator.GetUriByName(HttpContext,
                        nameof(SpecificTitleController.GetTitleById),
                        new { id = tvShow.TConst })

                    };
                    knownForTvShowsList.Add(newTitle);
                }

                model.KnownForMoviesWithUrl = knownForMoviesList;
                model.KnownForTvShowsWithUrl = knownForTvShowsList;

                personsModel.Add(model);                
            }

            return personsModel;
        }

    }
}
