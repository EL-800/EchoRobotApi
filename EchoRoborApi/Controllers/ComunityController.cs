using Azure;
using EchoRoborApi.Models;
using EchoRoborApi.Models.Request.Comunity;
using EchoRoborApi.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Reflection.Metadata.Ecma335;

namespace EchoRoborApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ComunityController : ControllerBase
    {
        private readonly IComunityService _conmunity;
        public ComunityController(IComunityService comunity)
        {
            _conmunity = comunity;
        }

        [HttpPost("AddPublication")]
        public IActionResult Add([FromForm]PublicacionRequest request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState + "Datos incorrectos");

            var response = _conmunity.AddPublicacion(request);

            if (response.Exito == 0) return BadRequest(response);
            else return Ok(response);
        }
        [HttpDelete("DeletePublicacion")]
        public IActionResult DeletePublicacion([FromHeader] int id , int idUsuario)
        {
            
            var response = _conmunity.DeletePublicacion(id,idUsuario);

            if (response.Exito == 0) return BadRequest(response);
            else return Ok(response);
        }

        [HttpPut("UpdatePublicacion")]
        public IActionResult UpdatePublicacion([FromForm]EditPublicacion edit)
        {
            var response = _conmunity.EditPublicacion(edit);
            if (response.Exito == 0) return BadRequest(response);
            else return Ok(response);
        }

        [HttpPost("AddComentario")]
        public IActionResult AddComentario([FromBody] AddComentarioModel request)
        {
            throw new NotImplementedException();
        }
    }
}
