using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PG_Api.Models.Entity;
using PG_Api.Services;

namespace PG_Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class FoodController(FoodService _service) : ControllerBase
{
    [Authorize] //Solo usuarios autorizados pueden ver el controlador
    [HttpGet]
    [Route("GetAllFood")]
    public async Task<ActionResult> GetAll() {
        var response = await _service.GetAll();
        return Ok(response); 
    }

}
