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
        public IActionResult GetActors(int page = 0, int pageSize = 20)
        {
            var Actors = _dataServicePersons.GetActors(page,pageSize);
            if (Actors == null)
            {
                return NotFound();
            }
            var ActorsModel = CreatePersonsModel(Actors);

            var total = _dataServicePersons.GetNumberOfActors();

            return Ok(PagingForActors(page,pageSize,total,ActorsModel));
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


        //PAGING 

        private string? CreateActorsLink(int page, int pageSize)
        {
            return _generator.GetUriByName(
                HttpContext,
                nameof(GetActors), new { page, pageSize});
        }

        private object PagingForActors<T>(int page, int pageSize, int total, IEnumerable<T> items)
        {
            pageSize = pageSize > MaxPageSize ? MaxPageSize : pageSize;

            var pages = (int)Math.Ceiling((double)total / (double)pageSize);
                      
            var first = total > 0
                ? CreateActorsLink(0, pageSize)
                : null;

            var prev = page > 0
                ? CreateActorsLink(page - 1, pageSize)
                : null;

            var current = CreateActorsLink(page, pageSize);

            var next = page < pages - 1
                ? CreateActorsLink(page + 1, pageSize)
                : null;

            var result = new
            {
                first,
                prev,
                next,
                current,
                total,
                pages,
                items
            };
            return result;
        }

    }
}
