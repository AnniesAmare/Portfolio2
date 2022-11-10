using AutoMapper;
using DataLayer;
using DataLayer.DataTransferModel;
using Microsoft.AspNetCore.Mvc;
using System;
using WebServer.Model;

namespace WebServer.Controllers
{
    [Route("api/titles")]
    [ApiController]
    public class MoviesController : Controller
    {
        private IDataserviceTitles _dataServiceTitles;
        private readonly LinkGenerator _generator;
        private readonly IMapper _mapper;

        public MoviesController(IDataserviceTitles dataServiceMovies, LinkGenerator generator, IMapper mapper)
        {
            _dataServiceTitles = dataServiceMovies;
            _generator = generator;
            _mapper = mapper;
        }

        [HttpGet("movies")]
        public IActionResult GetMovie()
        {
            var movies = _dataServiceTitles.GetMovies();
            if (movies == null)
            {
                return NotFound();
            }
            var moviesModel = CreateMoviesModel(movies);
            return Ok(moviesModel);
        }

        public IList<MoviesModel> CreateMoviesModel(IList<Titles> movies)
        {
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
