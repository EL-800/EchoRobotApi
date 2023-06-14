using Azure;
using EchoRoborApi.Models;
using EchoRoborApi.Models.Request.User;
using EchoRoborApi.Services.Interfaces;
using EchoRobotApi.Models;
using EchoRobotApi.Models.Common;
using EchoRobotApi.Models.Request.User;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace EchoRoborApi.Services
{
    public class UserService : IUserService
    {

        private readonly EchoRobotContext _context = new ();
        private readonly IMultimediaService _multimediaService = new MultimediaService();

        private readonly AppSettings _appSettings;

        public UserService(IOptions<AppSettings> appSettings)
        {
            _appSettings = appSettings.Value;
        }


        public ResponseModel AddUser(UserRequest userRequest)
        {
            ResponseModel response = new();
            try
            {
                Usuario user = new();
                user.Nombre = userRequest.Nombre!;
                user.Apellido = userRequest.Apellido!;
                user.Email = userRequest.Email!;
                user.Password = userRequest.Password!;

                _context.Add(user);
                _context.SaveChangesAsync();

                response.Exito = 1;
                response.Mensage = "Usuario ingresado correctamente";
            }
            catch (Exception ex)
            {
                response.Mensage = ex.Message;
                response.Exito = 0;
            }
            return response;
        }

        public ResponseModel DeleteUser(int id)
        {
            ResponseModel response = new ResponseModel();
            try
            {
                var currentlyUser = _context.Usuarios.Find(id);
                if (currentlyUser == null) throw new Exception("Usuario no existe");

                _context.Usuarios.Remove(currentlyUser);
                _context.SaveChangesAsync();

                response.Exito = 1;
                response.Mensage= "Usuario Eliminado correctamente";
            }
            catch (Exception ex)
            {
                response.Exito = 0;
                response.Mensage = ex.Message;
            }
            return response;
        }

        public ResponseModel EditUser(EditRequest edit)
        {
            ResponseModel response = new ();
            try
            {

                if (edit == null) throw new Exception("Error: Valores no enviados");

                var currentlyUser =  _context.Usuarios.Find(edit.IdUsuario);

                if (currentlyUser == null) throw new Exception("Usuario no encontrado");

                currentlyUser.IdUsuario = edit.IdUsuario;
                currentlyUser.Nombre = edit.Nombre!;
                currentlyUser.Apellido = edit.Apellido!;

                _context.Usuarios.Entry(currentlyUser).State = EntityState.Modified;
                _context.SaveChangesAsync();
                response.Exito = 1;
                response.Mensage = "Se ha modificado correctamante";
            }
            catch (Exception ex)
            {
                response.Exito = 0;
                response.Mensage = ex.Message;
            }
            return response;
        }

        public async Task<Usuario?> GetUser(int id)
        {
            return await _context.Usuarios.Where(d=>d.IdUsuario == id).FirstOrDefaultAsync();
        }

        public async Task<UserResponse> Logging(LoggingRequest request)
        {
            var user = await _context.Usuarios.Where(d => d.Email == request.Email && d.Password == request.Password).FirstOrDefaultAsync();
            var response = new UserResponse();

            if (user == null) return null;

            response.Nombre = user.Nombre;
            response.Token = GetToken(user);
            response.IdUsuario = user.IdUsuario;
            response.UrlFoto = user.Foto == null ? "" : user.Foto;

            return response;
        }

        public async Task<ResponseModel> UploadPhotoUser(UserPhotoRequest request)
        {
            ResponseModel response = new ResponseModel();
            if(!_multimediaService.isImage(request.File))
            {
                response.Mensage = "No tiene el formato adecuado";
                return response;
            }
            try
            {
                var usuario = await _context.Usuarios.FindAsync(request.IdUsuario);

                if (usuario == null) throw new Exception("El usuario no existe");

                if (!string.IsNullOrEmpty(usuario.Foto)) _multimediaService.DeleteFile(usuario.Foto);

                usuario.Foto = await _multimediaService.UploadPhotoUserAsync(request.File.OpenReadStream(), request.IdUsuario.ToString() + usuario.Nombre);

                if (usuario.Foto == null) throw new Exception("Error al subir el archivo");

                _context.Usuarios.Entry(usuario).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                response.Mensage = "Foto actualizada con exito";
                response.Exito = 1;
            }
            catch (Exception ex)
            {
                response.Mensage = ex.Message;
                response.Exito = 0;
            }
            return response;
        }
        private string GetToken(Usuario usuario)
        {
            var tokenHandler = new JwtSecurityTokenHandler();

            var llave = Encoding.ASCII.GetBytes(_appSettings.Secreto);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(
                    new Claim[]
                    {
                        new Claim(ClaimTypes.NameIdentifier, usuario.IdUsuario.ToString()),
                        new Claim(ClaimTypes.Email, usuario.Email.ToString())
                    }
                    ),
                Expires = DateTime.Now.AddDays(30),
                NotBefore = DateTime.Now,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(llave), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
