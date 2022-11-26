using AutoMapper;
using DataLayer;
using DataLayer.DataTransferModel;
using Microsoft.AspNetCore.Mvc;
using WebServer.Model;

namespace WebServer.Controllers
{
    [Route("api/titles")]
    [ApiController]
    public class TitlesController : BaseController
    {
        private readonly IDataserviceTitles _dataServiceTitles;

        public TitlesController(IDataserviceTitles dataServiceMovies, LinkGenerator generator, IMapper mapper, IConfiguration configuration) : base(generator, mapper, configuration)
        {
            _dataServiceTitles = dataServiceMovies;
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

            return Ok(DefaultPagingModel(page, pageSize, total, moviesModel, nameof(GetMovies)));
        }

        public IList<MovieListModel> CreateMoviesModel(IList<Titles> movies)
        {
            var moviesModel = new List<MovieListModel>();

            foreach (var movie in movies)
            {
                var movieModelElement = _mapper.Map<MovieListModel>(movie);
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

            return Ok(DefaultPagingModel(page, pageSize, total, tvShowsModel, nameof(GetTvShows)));
        }

        public IList<TvShowListModel> CreateTvShowsModel(IList<Titles> tvShows)
        {
            var tvShowsModel = new List<TvShowListModel>();

            foreach (var tvShow in tvShows)
            {

                var tvShowContent = CreateTvShowModel(tvShow);

                //var tvShowModelElement = _mapper.Map<TvShowListElement>(tvShow);
                TvShowListModel tvShowModelElement = new TvShowListModel();
                tvShowModelElement.Name = tvShow.Name;
                tvShowModelElement.AiringDate = tvShow.AiringDate;
                tvShowModelElement.TvShowContentList = tvShowContent;
                tvShowModelElement.Rating = tvShow.Rating;
                tvShowModelElement.Url =
                    GenerateLink(nameof(SpecificTitleController.GetTvShowById), new { id = tvShow.TConst });

                tvShowModelElement.TvShowContentList = tvShowContent;
                //var tvShowModelElement = _mapper.Map<TvShowListModel>(tvShow);

                tvShowModelElement.Url = _generator.GetUriByName(HttpContext,
                        nameof(SpecificTitleController.GetTvShowById),
                        new { id = tvShow.TConst });

                tvShowsModel.Add(tvShowModelElement);
            }

            return tvShowsModel;
        }


        public IList<TvShowModel> CreateTvShowModel(Titles tvShow)
        {

            IList<TvShowModel> tvShowContent = new List<TvShowModel>();

            IList<EpisodeModel> episodes = new List<EpisodeModel>();
            IList<DirectorListElementModel> directors = new List<DirectorListElementModel>();


            foreach (var director in tvShow.DirectorList)
            {
                DirectorListElementModel newDirector = new DirectorListElementModel();

                newDirector.Name = director.Name;

                newDirector.Url = GenerateLink(nameof(SpecificPersonController.GetPersonById), new { id = director.NConst });

                directors.Add(newDirector);
            }

            foreach (var season in tvShow.TvShowContentList)
            {
                var newSeason = new TvShowModel();

                newSeason.Season = season.Season;

                foreach (var episode in season.Episodes)
                {
                    var tConst = episode.TConst.RemoveSpaces();
                    var newEpisode = new EpisodeModel
                    {

                        Episode = episode.Episode,
                        Name = episode.Name,
                        URL = GenerateLink(nameof(SpecificTitleController.GetEpisodeById), new { id = tConst })
                    };

                    episodes.Add(newEpisode);

                }

                newSeason.Episodes = episodes;

                tvShowContent.Add(newSeason);
            }



            return tvShowContent;
        }

    }
}
