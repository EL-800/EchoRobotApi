using EchoRoborApi.Models;
using EchoRoborApi.Models.Request.Comunity;
using EchoRoborApi.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.IdentityModel.Tokens;
using System.Xml.Linq;

namespace EchoRoborApi.Services
{
    
    public class ComunityService : IComunityService
    {

        private readonly EchoRobotContext _context = new EchoRobotContext();
        private readonly IMultimediaService _multimediaService = new MultimediaService();

        public ResponseModel AddComentario(AddComentarioModel request)
        {
            throw new NotImplementedException();
        }

        public ResponseModel AddPublicacion(PublicacionRequest request)
        {
            ResponseModel response = new ResponseModel();
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    Publicacion publicacion = new Publicacion();

                    publicacion.Titulo = request.Titulo;
                    publicacion.FechaPublicacion = DateTime.Now;
                    publicacion.Descripcion = request.Descripcion;
                    publicacion.IdAutor = request.IdAutor;

                    _context.Publicacions.Add(publicacion);
                    _context.SaveChanges();

                    foreach(var elemento in request.Elementos)
                    {
                        if (!_multimediaService.isImage(elemento) || _multimediaService.isVideo(elemento))
                            throw new Exception("Un documento no tiene el formato adecuado");

                        var multimedia = new Multimedia();

                        multimedia.IdPublicacion = publicacion.IdPublicacion;

                        var image = _multimediaService.UploadFile(elemento, publicacion.IdPublicacion, publicacion.Titulo, 1);
                        if (image == null) throw new Exception("Error al subir un archivo");

                        multimedia.Direccion = image;

                        _context.Multimedia.Add(multimedia);
                        _context.SaveChanges();
                    }
                    transaction.Commit();
                    response.Exito = 1;
                    response.Mensage = "Publicacion agrgada con exito";
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    response.Exito = 0;
                    response.Mensage = ex.Message;
                }
                return response;
            }
        }

        public ResponseModel DeletePublicacion(int id, int idUser)
        {
            var response = new ResponseModel();
            try
            {
                var currentPublicacion = _context.Publicacions.Where(d =>
                    d.IdPublicacion == id &&  d.IdAutor == idUser
                ).FirstOrDefault();

                if (currentPublicacion == null) throw new Exception("Publicacion no se pudo eliminar");

                _context.Publicacions.Remove(currentPublicacion);
                _context.SaveChangesAsync();

                response.Exito = 1;
                response.Mensage = $"Publicacion con id {id} ha sido eliminada";

            }
            catch (Exception ex)
            {
                response.Exito = 0;
                response.Mensage = ex.Message;
            }
            return response;
        }



        public ResponseModel EditPublicacion(EditPublicacion request)
        {
            var response = new ResponseModel();
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    var publication = _context.Publicacions.Find(request.IdPublicacion);
                    if (publication == null) throw new Exception("Publicacion no existe");

                    publication.Titulo = request.Titulo;
                    publication.Descripcion = request.Descripcion;

                    var imagenes = _context.Multimedia.Where(d => d.IdPublicacion == publication.IdPublicacion);

                    foreach (var imagen in imagenes)
                    {
                        _multimediaService.DeleteFile(imagen.Direccion);
                        _context.Multimedia.Remove(imagen);
                    }

                    foreach(var imagen in request.Elementos)
                    {
                        if (!_multimediaService.isImage(imagen) || _multimediaService.isVideo(imagen))
                            throw new Exception("Un documento no tiene el formato adecuado");

                        var multimedia = new Multimedia();

                        multimedia.IdPublicacion = publication.IdPublicacion;

                        var image = _multimediaService.UploadFile(imagen, publication.IdPublicacion, publication.Titulo, 1);
                        if (image == null) throw new Exception("Error al subir un archivo");

                        multimedia.Direccion = image;

                        _context.Multimedia.Add(multimedia);
                        _context.SaveChanges();
                    }

                    response.Exito = 1;
                    response.Mensage = "Publicacion actualizada con exito";
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    response.Mensage = ex.Message;
                }
            }
                return response;
        }
    }
}
