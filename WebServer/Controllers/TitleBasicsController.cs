using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using DataLayer.IDataService;
using System.Reflection.Emit;

namespace WebServer.Controllers
{
    [Route("api/test")]
    [ApiController]
    public class TitleBasicsController : ControllerBase
    {
        private IDataService _dataService;
        private readonly LinkGenerator _generator;
        private readonly IMapper _mapper;

    }

    public TitleBasicsController(IDataService dataService, LinkGenerator generator, IMapper mapper)
    {
        _dataService = dataService;
        _generator = generator;
        _mapper = mapper;
    }

    [HttpGet]
    public IActionResult GetTitleBasics()
    {
        var titlebasics =
        _dataService.GetTitleBasics().Select(x => CreateTitleBasicsModel(x));
        return Ok(titlebasics);
    }


    private TitleBasicsModel CreateTitleBasicsModel(TitleBasics titlebasics)
    {
        //maps a Category-object to a CategoryModel.
        var model = _mapper.Map<TitleBasicsModel>(titlebasics);
        model.Url = _generator.GetUriByName(HttpContext, nameof(GetTitleBasics), new { TitleBasics.TConst});
        return model;
    }
}
