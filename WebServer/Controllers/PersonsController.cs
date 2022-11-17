using AutoMapper;
using DataLayer;
using DataLayer.DataTransferModel;
using Microsoft.AspNetCore.Mvc;
using WebServer.Model;

namespace WebServer.Controllers
{
    [Route("api/persons")]
    [ApiController]
    public class PersonsController : BaseController
    {
        private readonly IDataservicePersons _dataServicePersons;

        public PersonsController(IDataservicePersons dataServicePersons, LinkGenerator generator, IMapper mapper, IConfiguration configuration) : base(generator, mapper, configuration)
        {
            _dataServicePersons = dataServicePersons;
        }

        [HttpGet("actors", Name = nameof(GetActors))]
        public IActionResult GetActors(int page = 0, int pageSize = 20)
        {
            var Actors = _dataServicePersons.GetActors(page,pageSize);
            if (Actors == null)
            {
                return NotFound();
            }
            var ActorsModel = CreatePersonsModel(Actors);

            var total = _dataServicePersons.GetNumberOfActors();

            return Ok(DefaultPagingModel(page,pageSize,total,ActorsModel, nameof(GetActors)));
        }

        
        public IList<PersonListModel> CreatePersonsModel(IList<Persons> persons)
        {
            var personsModel = new List<PersonListModel>();
            var knownForMoviesList = new List<TitleListElementModel>();
            var knownForTvShowsList = new List<TitleListElementModel>();

            foreach (var person in persons)
            {
                var model = _mapper.Map<PersonListModel>(person);
                
                model.PersonUrl = GenerateLink(
                       nameof(SpecificPersonController.GetPersonById),
                       new { id = person.NConst });

                //make moviesListWithUrl
                foreach (var movie in person.KnownForMovies)
                {
                    var newTitle = new TitleListElementModel
                    {
                        Title = movie.Title,
                        Url = GenerateLink(nameof(SpecificTitleController.GetTitleById), new { id = movie.TConst })

                    };
                   knownForMoviesList.Add(newTitle);
                }

                //make tvShowListWithUrl
                foreach (var tvShow in person.KnownForTvShows)
                {
                    var newTitle = new TitleListElementModel
                    {
                        Title = tvShow.Title,
                        Url = GenerateLink(nameof(SpecificTitleController.GetTitleById), new { id = tvShow.TConst })

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
