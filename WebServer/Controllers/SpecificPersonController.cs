﻿using AutoMapper;
using DataLayer;
using DataLayer.DataTransferModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;
using WebServer.Model;

namespace WebServer.Controllers
{
    [Route("api/person")]
    [ApiController]
    public class SpecificPersonController : ControllerBase
    {
        private IDataserviceSpecificPerson _dataServiceSpecificPerson;
        private readonly LinkGenerator _generator;
        private readonly IMapper _mapper;

        public SpecificPersonController(IDataserviceSpecificPerson dataServiceSpecificPerson, LinkGenerator generator, IMapper mapper)
        {
            _dataServiceSpecificPerson = dataServiceSpecificPerson;
            _generator = generator;
            _mapper = mapper;
        }

        [HttpGet("{id}", Name = nameof(GetPersonById))]
        public IActionResult GetPersonById(string id)
        {
            var specificPerson = _dataServiceSpecificPerson.GetSpecificPerson(id);
            if (specificPerson == null)
            {
                return NotFound();
            }
            var specificPersonModel = CreateSpecificPersonModel(specificPerson);
            return Ok(specificPersonModel);
        }

        [HttpGet("name/{search}")]
        public IActionResult GetPersonByName(string search)
        {
            var specificPerson = _dataServiceSpecificPerson.GetSpecificPersonByName(search);
            if (specificPerson == null)
            {
                return NotFound();
            }
            var specificPersonModel = CreateSpecificPersonModel(specificPerson);
            return Ok(specificPersonModel);
        }

        public SpecificPersonModel CreateSpecificPersonModel(SpecificPerson person)
        {
            var model = _mapper.Map<SpecificPersonModel>(person);
            var knownForList = new List<TitleListElementModel>();
            
            foreach (var title in person.KnownForList)
            {
                var newTitle = new TitleListElementModel
                {
                    Title = title.Title,
                    Url = _generator.GetUriByName(HttpContext, nameof(SpecificTitleController.GetTitleById), new { id = title.TConst })
                };
                knownForList.Add(newTitle);
            }
            model.KnownForListWithUrl = knownForList;
            model.Bookmark = _generator.GetUriByName(HttpContext, nameof(BookmarksController.CreateBookmark), new { id = person.NConst.RemoveSpaces() });
            return model;
        }
        
    }
}

