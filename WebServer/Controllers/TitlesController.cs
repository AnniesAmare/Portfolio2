using AutoMapper;
using DataLayer;
using DataLayer.DataTransferModel;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Security.Claims;
using WebServer.Model;

namespace WebServer.Controllers
{
    [Route("api/titles")]
    [ApiController]
    public class TitlesController : Controller
    {
        private IDataserviceTitles _dataServiceTitles;
        private readonly LinkGenerator _generator;
        private readonly IMapper _mapper;

        public TitlesController(IDataserviceTitles dataServiceMovies, LinkGenerator generator, IMapper mapper)
        {
            _dataServiceTitles = dataServiceMovies;
            _generator = generator;
            _mapper = mapper;
        }

        //MOVIES 
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

        //TVSHOWS
        [HttpGet("tvshows")]
        public IActionResult GetTvShows()
        {
            var tvShows = _dataServiceTitles.GetTvShows();
            if (tvShows == null)
            {
                return NotFound();
            }
            var tvShowsModel = CreateTvShowsModel(tvShows);
            return Ok(tvShowsModel);
        }

        public IList<TvShowsModel> CreateTvShowsModel(IList<Titles> tvShows)
        {
            var tvShowsModel = new List<TvShowsModel>();

            foreach (var tvShow in tvShows)
            {
                var tvShowModelElement = _mapper.Map<TvShowsModel>(tvShow);
                tvShowsModel.Add(tvShowModelElement);
            }

            return tvShowsModel;
        }

        [HttpGet("tvshows/{id}", Name = nameof(GetTvShowsById))]
        public IActionResult GetTvShowsById(string id)
        {
            var tvShow = _dataServiceTitles.GetTvShowsById(id);

            if (tvShow == null)
            {
                return NotFound();
            }

            var tvShowModelElement = _mapper.Map<TvShowsModel>(tvShow);

            return Ok(tvShowModelElement);
        }

        





    }
}
