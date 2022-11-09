using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AutoMapper;
using DataLayer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using WebServer.Model;
using WebServer.Services;

namespace WebServer.Controllers
{
    [Route("api/users")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private IDataService _dataService;
        private readonly LinkGenerator _generator;
        private readonly IMapper _mapper;
        private readonly Hashing _hashing;
        private readonly IConfiguration _configuration;

        public UsersController(IDataService dataService, LinkGenerator generator, IMapper mapper, Hashing hashing, IConfiguration configuration)
        {
            _dataService = dataService;
            _generator = generator;
            _mapper = mapper;
            _hashing = hashing;
            _configuration = configuration;
        }

        [HttpGet("{username}", Name = nameof(GetUser))]
        [Authorize]
        public IActionResult GetUser(string username)
        {
            try
            {
                var usernameValue = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Name).Value;
                var user = _dataService.GetUser(username);
                if (user == null)
                {
                    return NotFound();
                }

                var userModel = new UserModel
                {
                    Username = user.Username,
                    Email = user.Email,
                    Birthyear = user.BirthYear,
                    Password = "Cannot show password"
                };
                return Ok(userModel);
            }
            catch
            {
                return Unauthorized();
            }
        }

        //api/users?action=register (possibility) 
        [HttpPost("register")]
        public IActionResult Register(UserModel model)
        {
            if (_dataService.UserExists(model.Username)) return BadRequest();
            if (string.IsNullOrEmpty(model.Password)) return BadRequest();
            var hashResult = _hashing.Hash(model.Password);

            _dataService.CreateUser(model.Username, hashResult.hash, hashResult.salt, model.Email, model.Birthyear);
            return Ok();
        }

        [HttpPost("login")]
        public IActionResult Login(UserLoginModel model)
        {
            var user = _dataService.GetUser(model.Username);
            if (user == null) return BadRequest();
            if (!_hashing.Verify(model.Password, user.Password, user.Salt)) BadRequest();

            var claims = new List<Claim>{
                new Claim(ClaimTypes.Name, user.Username)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.GetSection("Auth:secret").Value));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);
            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: creds);

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);

            return Ok(new {user.Username, token = jwt});
        }


    }
}
