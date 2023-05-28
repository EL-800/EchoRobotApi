﻿using Azure;
using EchoRoborApi.Models;
using EchoRoborApi.Models.Request.User;
using EchoRoborApi.Services.Interfaces;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.EntityFrameworkCore;

namespace EchoRoborApi.Services
{
    public class UserService : IUserService
    {

        private readonly EchoRobotContext _context = new ();
        private readonly IMultimediaService _multimediaService = new MultimediaService();
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

                usuario.Foto = _multimediaService.UploadFile(request.File, usuario.IdUsuario, usuario.Nombre,0);

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

    }
}
