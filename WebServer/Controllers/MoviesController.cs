using AutoMapper;
using DataLayer;
using DataLayer.DataTransferModel;
using Microsoft.AspNetCore.Mvc;
using System;
using WebServer.Model;

namespace WebServer.Controllers
{
    [Route("api/movies")]
    [ApiController]
    public class MoviesController : Controller
    {
        private IDataserviceMovies _dataServiceMovies;
        private readonly LinkGenerator _generator;
        private readonly IMapper _mapper;

        public MoviesController(IDataserviceMovies dataServiceMovies, LinkGenerator generator, IMapper mapper)
        {
            _dataServiceMovies = dataServiceMovies;
            _generator = generator;
            _mapper = mapper;
        }

        [HttpGet]
        public IActionResult GetMovie()
        {
            var movies = _dataServiceMovies.GetMovies();
            if (movies == null)
            {
                return NotFound();
            }

            var moviesModel = CreateMoviesModel(movies);


            return Ok(moviesModel);
        }

        public IList<MoviesModel> CreateMoviesModel(IList<Titles> movies)
        {
            //var moviesModel = _mapper.Map<MoviesModel>(movies);

            var moviesModel = new List<MoviesModel>();

            foreach (var movie in movies)
            {
                var movieModelElement = _mapper.Map<MoviesModel>(movie);
                moviesModel.Add(movieModelElement);
            }

            return moviesModel;
        }
    }
}
