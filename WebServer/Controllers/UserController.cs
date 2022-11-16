using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AutoMapper;
using DataLayer.DataServiceInterfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using WebServer.Model;
using WebServer.Services;
using DataLayer;

namespace WebServer.Controllers
{
    [Route("api/user")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private IDataserviceUsers _dataServiceUsers;
        private readonly LinkGenerator _generator;
        private readonly IMapper _mapper;
        private readonly Hashing _hashing;
        private readonly IConfiguration _configuration;

        public UserController(IDataserviceUsers dataServiceUsers, LinkGenerator generator, IMapper mapper, Hashing hashing, IConfiguration configuration)
        {
            _dataServiceUsers = dataServiceUsers;
            _generator = generator;
            _mapper = mapper;
            _hashing = hashing;
            _configuration = configuration;
        }

        [HttpGet]
        [Authorize]
        public IActionResult GetUser()
        {
            try
            {
                var username = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Name).Value;
                var user = _dataServiceUsers.GetUser(username);
                if (user == null)
                {
                    return NotFound();
                }

                var userModel = new UserModel
                {
                    Username = user.Username,
                    Email = user.Email,
                    Birthyear = user.BirthYear,
                    BookmarksUrl = _generator.GetUriByName(HttpContext, nameof(BookmarksController.GetBookmarks), new { })
                };
                return Ok(userModel);
            }
            catch
            {
                return Unauthorized();
            }
        }

        [HttpPost("register")]
        public IActionResult Register(UserRegisterModel registerModel)
        {
            //none of the input values must be empty or null
            if (registerModel.Username.IsNullOrEmpty()) return BadRequest();
            if (registerModel.Password.IsNullOrEmpty()) return BadRequest();
            if (registerModel.Email.IsNullOrEmpty()) return BadRequest();
            if (registerModel.Birthyear.IsNullOrEmpty()) return BadRequest();

            //username must be unique
            if (_dataServiceUsers.UserExists(registerModel.Username)) return BadRequest();

            //password must have a minimum length of 8.
            const int minimumPasswordLength = 8;
            if (registerModel.Password.Length < minimumPasswordLength) return BadRequest();

            //password is hashed
            var hashResult = _hashing.Hash(registerModel.Password);
            var user = _dataServiceUsers.CreateUser(registerModel.Username, hashResult.hash, hashResult.salt, registerModel.Email, registerModel.Birthyear);
            return Ok();
        }

        [HttpPost("login")]
        public IActionResult Login(UserLoginModel model)
        {
            var user = _dataServiceUsers.GetUser(model.Username);
            if (user == null) return BadRequest();
            if (!_hashing.Verify(model.Password, user.Password, user.Salt)) BadRequest();

            var jwt = GenerateJwtToken(user.Username);

            return Ok(new { user.Username, token = jwt });
        }


        [HttpPut("update")]
        [Authorize]
        public IActionResult UpdateUser(UserUpdateModel model)
        {
            try
            {
                var username = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Name)!.Value;
                var updated = _dataServiceUsers.UpdateUser(username, model.Email, model.Birthyear);
                if (!updated)
                {
                    return NotFound();
                }
                return Ok();
            }
            catch
            {
                return Unauthorized();
            }
        }

        [HttpDelete("delete")]
        [Authorize]
        public IActionResult DeleteUser()
        {
            try
            {
                var username = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Name).Value;
                var deleted = _dataServiceUsers.DeleteUser(username);
                if (!deleted)
                {
                    return NotFound();
                }
                return Ok();
            }
            catch
            {
                return Unauthorized();
            }
        }

        public string? GenerateJwtToken(string username)
        {
            var claims = new List<Claim>{
                new Claim(ClaimTypes.Name, username),
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.GetSection("Auth:secret").Value));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);
            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: creds);

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);
            return jwt;
        }

        //if time allows make check-functions for user-stuff, so we confirm the validity of the inputs.
        //email must contain an '@'
        //password should be X length and so on
        //birthyear should be a 4-char of numbers and so on.
    }
}