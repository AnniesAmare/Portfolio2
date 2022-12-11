using AutoMapper;
using DataLayer;
using DataLayer.DataServiceInterfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebServer.Model;

namespace WebServer.Controllers
{
    [Route("api")]
    [ApiController]
    public class WordCloudController : BaseController
    {
        private readonly IDataserviceWordCloud _dataserviceWordCloud;

        public WordCloudController(IDataserviceWordCloud dataserviceWordCloud, LinkGenerator generator, IMapper mapper, IConfiguration configuration) : base(generator, mapper, configuration)
        {
            _dataserviceWordCloud = dataserviceWordCloud;
        }


        [HttpGet("wordcloud/{word}", Name = nameof(WordCloud))]
        public IActionResult WordCloud(string? word)
        {
            var wordResultModel = new List<WordModel>();
            var wordResult = _dataserviceWordCloud.GetRelatedWordsForWord(word);
            if (wordResult == null)
            {
                return NotFound();
            }
            foreach (var wordObject in wordResult)
            {
                var wordObjectModel = _mapper.Map<WordModel>(wordObject);
                wordResultModel.Add(wordObjectModel);
            }
            return Ok(wordResultModel);
        }

        [HttpGet("person/wordcloud/{name}", Name = nameof(PersonWordCloud))]
        public IActionResult PersonWordCloud(string? name)
        {
            var wordResultModel = new List<WordModel>();
            var wordResult = _dataserviceWordCloud.GetRelatedWordsForName(name);
            if (wordResult == null)
            {
                return NotFound();
            }
            foreach (var wordObject in wordResult)
            {
                var wordObjectModel = _mapper.Map<WordModel>(wordObject);
                wordResultModel.Add(wordObjectModel);
            }
            return Ok(wordResultModel);
        }


    }
}
