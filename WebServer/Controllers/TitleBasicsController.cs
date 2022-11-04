using AutoMapper;
using DataLayer;
using DataLayer.DatabaseModel;
using Microsoft.AspNetCore.Mvc;
using WebServer.Model;

namespace WebServer.Controllers
{
    [Route("api/test")]
    [ApiController]
    public class TitleBasicsController : ControllerBase
    {
        private IDataService _dataService;
        private readonly LinkGenerator _generator;
        private readonly IMapper _mapper;

        public TitleBasicsController(IDataService dataService, LinkGenerator generator, IMapper mapper)
        {
            _dataService = dataService;
            _generator = generator;
            _mapper = mapper;
        }
        /*
        [HttpGet]
        public IActionResult GetTitleBasics()
        {
            var titlebasics =
                _dataService.GetTitleBasics().First();
            return Ok(titlebasics);
        }
        */
        
        [HttpGet]
        public IActionResult GetTest()
        {
            var test =
                _dataService.GetTitleAkas().First();
            return Ok(test);
        }
        


    }
}
