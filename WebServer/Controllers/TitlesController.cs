﻿using AutoMapper;
using DataLayer;
using DataLayer.DataTransferModel;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Numerics;
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

        private const int MaxPageSize = 30;

        public TitlesController(IDataserviceTitles dataServiceMovies, LinkGenerator generator, IMapper mapper)
        {
            _dataServiceTitles = dataServiceMovies;
            _generator = generator;
            _mapper = mapper;
        }

        //MOVIES 
        [HttpGet("movies", Name = nameof(GetMovies))]
        public IActionResult GetMovies(int page = 0, int pageSize = 20)
        {
            var movies = _dataServiceTitles.GetMovies(page, pageSize);
            if (movies == null)
            {
                return NotFound();
            }
            var moviesModel = CreateMoviesModel(movies);

            var total = _dataServiceTitles.GetNumberOfMovies();

            return Ok(PagingForMovies(page, pageSize, total, moviesModel));
        }

        public IList<MoviesModel> CreateMoviesModel(IList<Titles> movies)
        {
            var moviesModel = new List<MoviesModel>();

            foreach (var movie in movies)
            {
                var movieModelElement = _mapper.Map<MoviesModel>(movie);
                movieModelElement.Url = _generator.GetUriByName(HttpContext,
                        nameof(SpecificTitleController.GetTitleById),
                        new { id = movie.TConst });

                moviesModel.Add(movieModelElement);
            }

            return moviesModel;
        }

        //TVSHOWS
        [HttpGet("tvshows", Name = nameof(GetTvShows))]
        public IActionResult GetTvShows(int page = 0, int pageSize = 20)
        {
            var tvShows = _dataServiceTitles.GetTvShows(page, pageSize);
            if (tvShows == null)
            {
                return NotFound();
            }
            var tvShowsModel = CreateTvShowsModel(tvShows);

            var total = _dataServiceTitles.GetNumberOfTvShows();

            return Ok(PagingForTvShows(page, pageSize, total, tvShowsModel));
        }

        public IList<TvShowsModel> CreateTvShowsModel(IList<Titles> tvShows)
        {
            var tvShowsModel = new List<TvShowsModel>();

            foreach (var tvShow in tvShows)
            {
                var tvShowModelElement = _mapper.Map<TvShowsModel>(tvShow);

                tvShowModelElement.Url = _generator.GetUriByName(HttpContext,
                        nameof(TitlesController.GetTvShowById),
                        new { id = tvShow.TConst });

                tvShowsModel.Add(tvShowModelElement);
            }

            return tvShowsModel;
        }

        [HttpGet("tvshows/{id}", Name = nameof(GetTvShowById))]
        public IActionResult GetTvShowById(string id)
        {
            var tvShow = _dataServiceTitles.GetTvShowById(id);

            if (tvShow == null)
            {
                return NotFound();
            }

            var tvShowModelElement = _mapper.Map<TvShowsModel>(tvShow);

            return Ok(tvShowModelElement);
        }


        //PAGING 


        //movie paging
        private string? CreateMoviesLink(int page, int pageSize)
        {
            return _generator.GetUriByName(
                HttpContext,
                nameof(GetMovies), new { page, pageSize });
        }

        private object PagingForMovies<T>(int page, int pageSize, int total, IEnumerable<T> items)
        {
            pageSize = pageSize > MaxPageSize ? MaxPageSize : pageSize;

            var pages = (int)Math.Ceiling((double)total / (double)pageSize);

            var first = total > 0
                ? CreateMoviesLink(0, pageSize)
                : null;

            var prev = page > 0
                ? CreateMoviesLink(page - 1, pageSize)
                : null;

            var current = CreateMoviesLink(page, pageSize);

            var next = page < pages - 1
                ? CreateMoviesLink(page + 1, pageSize)
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

        //tvshow paging
        private string? CreateTvShowsLink(int page, int pageSize)
        {
            return _generator.GetUriByName(
                HttpContext,
                nameof(GetTvShows), new { page, pageSize });
        }

        private object PagingForTvShows<T>(int page, int pageSize, int total, IEnumerable<T> items)
        {
            pageSize = pageSize > MaxPageSize ? MaxPageSize : pageSize;

            var pages = (int)Math.Ceiling((double)total / (double)pageSize);

            var first = total > 0
                ? CreateTvShowsLink(0, pageSize)
                : null;

            var prev = page > 0
                ? CreateTvShowsLink(page - 1, pageSize)
                : null;

            var current = CreateTvShowsLink(page, pageSize);

            var next = page < pages - 1
                ? CreateTvShowsLink(page + 1, pageSize)
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