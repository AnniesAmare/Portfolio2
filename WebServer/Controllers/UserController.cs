using AutoMapper;
using DataLayer;
using DataLayer.DataServiceInterfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebServer.Model;
using WebServer.Services;

namespace WebServer.Controllers
{
    [Route("api/user")]
    [ApiController]
    public class UserController : BaseController
    {
        private readonly IDataserviceUsers _dataServiceUsers;
        private readonly Hashing _hashing;

        public UserController(IDataserviceUsers dataServiceUsers, LinkGenerator generator, IMapper mapper, Hashing hashing, IConfiguration configuration) : base(generator, mapper, configuration)
        {
            _dataServiceUsers = dataServiceUsers;
            _hashing = hashing;
        }

        [HttpGet]
        [Authorize]
        public IActionResult GetUser()
        {
            try
            {
                var username = GetUsername();
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
                    BookmarksUrl = GenerateLink(nameof(BookmarksController.GetBookmarks), new { })
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
            
            var created = _dataServiceUsers.CreateUser(registerModel.Username, hashResult.hash, hashResult.salt, registerModel.Email, registerModel.Birthyear);
            if (!created) return BadRequest();
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
                var username = GetUsername();
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
                var username = GetUsername();
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
    }
}