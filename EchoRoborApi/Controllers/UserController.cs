using EchoRoborApi.Models;
using EchoRoborApi.Models.Request;
using EchoRoborApi.Models.Request.User;
using EchoRoborApi.Services.Interfaces;
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
        public IActionResult RegisterUser(UserRequest userRequest)
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

    }
}

