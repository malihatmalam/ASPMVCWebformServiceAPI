using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using MyRESTServices.BLL.DTOs;
using MyRESTServices.BLL.Interfaces;
using MyRESTServices.Helpers;
using MyRESTServices.ViewModels;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace MyRESTServices.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserBLL _userBLL;
        private readonly AppSettings _appSettings;

        public UsersController(IUserBLL userBLL, IOptions<AppSettings> appSettings)
        {
            _userBLL = userBLL;
            _appSettings = appSettings.Value;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserDTO>>> GetAllUsers()
        {
            var users = await _userBLL.GetAll();
            return Ok(users);
        }

        [HttpGet("{username}")]
        public async Task<ActionResult<UserDTO>> GetUserByUsername(string username)
        {
            try
            {
                var user = await _userBLL.GetByUsername(username);
                return Ok(user);
            }
            catch (ArgumentException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddUser(UserCreateDTO userCreateDTO)
        {
            try
            {
                await _userBLL.Insert(userCreateDTO);
                return Ok("User added successfully.");
            }
            catch (Exception ex)
            {
                return BadRequest($"Failed to add user: {ex.Message}");
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginViewModels loginData)
        {
            try
            {
                var user = await _userBLL.Login(loginData.Username, loginData.Password);
                if (user != null)
                {
                    List<Claim> claims = new List<Claim>();
                    claims.Add(new Claim(ClaimTypes.Name, user.Username));
                    foreach (var role in user.Roles)
                    {
                        claims.Add(new Claim(ClaimTypes.Role, role.RoleName));
                    }
                    var tokenHandler = new JwtSecurityTokenHandler();
                    var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
                    var tokenDescriptor = new SecurityTokenDescriptor
                    {
                        Subject = new ClaimsIdentity(claims),
                        Expires = DateTime.Now.AddHours(1),
                        SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key),
                            SecurityAlgorithms.HmacSha256Signature)
                    };
                    var token = tokenHandler.CreateToken(tokenDescriptor);
                    var userWithToken = new UserWithToken
                    {
                        Username = loginData.Username,
                        Password = loginData.Password,
                        Token = tokenHandler.WriteToken(token)
                    };
                    return Ok(userWithToken);
                }
                else
                {
                    return BadRequest("Invalid credentials");
                }

                return Ok(user);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // Endpoint untuk mengubah kata sandi
        [HttpPost("{username}/changepassword")]
        public async Task<IActionResult> ChangePassword(string username, string newpassword)
        {
            try
            {
                await _userBLL.ChangePassword(username, newpassword);
                return Ok("Password changed successfully.");
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }


    }
}