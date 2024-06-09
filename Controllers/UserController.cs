using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PG_Api.Models.Dtos;
using PG_Api.Services;

namespace PG_Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IAutorizacionService _autorizacionService;

        public UserController(IAutorizacionService autorizacionService)
        {
            _autorizacionService = autorizacionService;
        }

        [HttpPost]
        [Route("Autentication")]
        public async Task<IActionResult> Autentication([FromBody] AutorizacionRequest dataUser) {
            var resultAutorizacion = await _autorizacionService.returnToken(dataUser);

            if (resultAutorizacion == null)
            {
                return Unauthorized();
            }

            return Ok(resultAutorizacion);
        }
    }
}
