using Azure;
using EchoRoborApi.Models;
using EchoRoborApi.Models.Request.Comunity;
using EchoRoborApi.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
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
        public async Task<IActionResult> Add([FromForm]PublicacionRequest request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState + "Datos incorrectos");

            var response =await _conmunity.AddPublicacion(request);

            if (response.Exito == 0) return BadRequest(response);
            else return Ok(response);
        }
        [HttpDelete("DeletePublicacion")]
        public IActionResult DeletePublicacion(int id )
        {
            
            var response = _conmunity.DeletePublicacion(id);

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
        public IActionResult AddComentario([FromForm] AddComentarioModel request)
        {
            var response = _conmunity.AddComentario(request);
            if (response.Exito == 0) return BadRequest(response);
            else return Ok(response);
        }

        [HttpGet("Publicaciones")]
        public async Task<IActionResult> ListarPublicaciones()
        {
            var response = await _conmunity.ListPublication();
            if (response.Exito == 0) return BadRequest(response);
            else return Ok(response);
        }

        [HttpGet("Publicacion")]
        public async Task<IActionResult> GetPublication(int id)
        {
            var response = await _conmunity.GetPublication(id);
            if (response.Exito == 0) return BadRequest(response);
            else return Ok(response);
        }
    }
}
