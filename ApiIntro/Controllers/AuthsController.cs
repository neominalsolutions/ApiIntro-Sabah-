using ApiIntro.Dtos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiIntro.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthsController : ControllerBase
    {

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginRequestDto request)
        {
            if (ModelState.IsValid)
            {
                return Ok("OK");
            }

          var allErrors = ModelState.Values.SelectMany(v => v.Errors).Select(x => x.ErrorMessage);
            // tüm hataları dil bazlı okuyup dönecek.
            return BadRequest(allErrors);
        }
    }
}
