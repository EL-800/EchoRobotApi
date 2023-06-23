using EchoRoborApi.Models;
using EchoRoborApi.Models.Request;
using EchoRoborApi.Models.Request.User;
using EchoRoborApi.Services.Interfaces;
using EchoRobotApi.Models;
using EchoRobotApi.Models.Request.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EchoRoborApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        public UserController(IUserService service)
        {
            _userService = service;
        }

        [HttpPost()]
        public IActionResult RegisterUser([FromForm]UserRequest userRequest)
        {
            if (!ModelState.IsValid) return BadRequest("Datos incorrectos");

            var response = _userService.AddUser(userRequest);

            if (response.Exito == 0) return BadRequest(response);
            else return Ok(response);
        }

        [HttpPut]
        public IActionResult EditUser(EditRequest edit)
        {
            if (!ModelState.IsValid) return BadRequest("Datos incorrectos");

            var response = _userService.EditUser(edit);

            if (response.Exito == 0) return BadRequest(response);
            else return Ok(response);
        }
        [HttpDelete]
        public IActionResult DeleteUser([FromHeader] int id)
        {
            //cambio
            var response = _userService.DeleteUser(id);
            if (response.Exito == 0) return BadRequest(response);
            else return Ok(response);
        }


        [HttpPut("uploadphoto")]
        public async Task<IActionResult> UploadPhoto([FromForm] UserPhotoRequest request)
        {

            var response = await _userService.UploadPhotoUser(request);

            if (response.Exito == 1) return Ok(response);

            return BadRequest(response);
        }

        [HttpPost("logging")]
        public async Task<IActionResult> Logging([FromForm]LoggingRequest request)
        {
            var response = new ResponseModel();
            var userResponse = await _userService.Logging(request);

            if (userResponse == null)
            {
                response.Exito = 0;
                response.Mensage = "Credenciales incorrectas";
                return Unauthorized(response);
            }

            response.Exito = 1;
            response.Mensage = "Inicio de sesion correcto";
            response.Data = userResponse;
            return Ok(response);
        }

        [HttpGet("ListUser")]
        [Authorize]
        public IActionResult GetUsuario()
        {

            using (var context = new EchoRobotContext())
            {
                var usuario = context.Usuarios.OrderByDescending(d => d.IdUsuario).ToList();
                return Ok(usuario);
            }
        }

    } 
}

