using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyRESTServices.BLL.Interfaces;
using MyRESTServices.Data.Interfaces;

namespace MyRESTServices.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RolesController : ControllerBase
    {
        private readonly IRoleBLL _roleBLL;

        public RolesController(IRoleBLL roleBLL)
        {
            _roleBLL = roleBLL;
        }

        [HttpPost("AddUserToRole")]
        public async Task<IActionResult> AddUserToRoles(string username, int roleId) {

            try
            {
                await _roleBLL.AddUserToRole(username, roleId);
                return Ok();
            }
            catch (Exception ex)
            {
                throw new ArgumentException(ex.Message);
            }
        }
    }
}
