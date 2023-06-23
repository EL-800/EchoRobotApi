using Azure;
using EchoRoborApi.Models;
using EchoRoborApi.Models.Request.Comunity;
using EchoRoborApi.Services.Interfaces;
using EchoRobotApi.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.IdentityModel.Tokens;
using System.Linq;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace EchoRoborApi.Services
{
    
    public class ComunityService : IComunityService
    {

        private readonly EchoRobotContext _context = new EchoRobotContext();
        private readonly IMultimediaService _multimediaService = new MultimediaService();

        public ResponseModel AddComentario(AddComentarioModel request)
        {
            var response = new ResponseModel();
            try
            {
                var comentario = new Comentario();
                comentario.Descripcion = request.Descripcion;
                comentario.IdPublicacion = request.IdPublicacion;
                comentario.IdAutor = request.IdAutor;
                comentario.FechaPublicacion = DateTime.Now;
                _context.Comentarios.Add(comentario);
                _context.SaveChanges();

                response.Exito = 1;
                response.Data = "Comentario agregado con exito";
            }
            catch (Exception ex)
            {

                response.Exito = 0;
                response.Mensage = ex.Message;
            }
            return response; 
        }

        public async Task<ResponseModel> ListPublication()
        {
            var response = new ResponseModel();
            try
            {
                int registrosCount = 10;
                var list = await (from p in _context.Publicacions
                                  select
                                  new {
                                      idPublicacion = p.IdPublicacion,
                                      titulo = p.Titulo,
                                      descripcion = p.Descripcion,
                                      fechaPublicacion = p.FechaPublicacion,
                                      numComentarios = _context.Comentarios.Where(e => e.IdPublicacion == p.IdPublicacion).Count(),
                                      nombreAutor = _context.Usuarios.Where(e => e.IdUsuario == p.IdAutor).FirstOrDefault().Nombre,
                                      foto = _context.Multimedia.Where(e=>e.IdPublicacion == p.IdPublicacion).First().Direccion,
                                  })
                                  .Take(registrosCount).ToListAsync();
                response.Exito = 1;
                response.Mensage = "";
                response.Data = list;
            }
            catch (Exception ex)
            {

                response.Exito =0;
                response.Mensage = ex.Message;
            }
            return response;
        }
        public async Task<ResponseModel> ListTopPublication()
        {
            var response = new ResponseModel();
            try
            {
                var list = "";
                response.Exito = 1;
                response.Mensage = "";
                response.Data = list;
            }
            catch (Exception ex)
            {

                response.Exito = 0;
                response.Mensage = ex.Message;
            }
            return response;
        }


        public async Task<ResponseModel> AddPublicacion(PublicacionRequest request)
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

                        //var image = _multimediaService.UploadFile(elemento, publicacion.IdPublicacion, publicacion.Titulo, 1);
                        var image = await _multimediaService.UploadFilePublicationAsync(elemento.OpenReadStream(), elemento.Name, publicacion.IdPublicacion);
                        
                        if (image == null) throw new Exception("Error al subir un archivo");

                        multimedia.Direccion = image;

                        _context.Multimedia.Add(multimedia);
                        _context.SaveChanges();
                    }

                    
                    

                    XmlDocument xmlDocument = new XmlDocument();
                    using (var reader = new StreamReader(request.Proyecto.OpenReadStream(), System.Text.Encoding.UTF8, true))
                    {
                        // Carga el contenido del archivo en el XmlDocument
                        xmlDocument.Load(reader);
                    }
                    var proyecto = new Proyecto();
                    proyecto.IdPublicacion = publicacion.IdPublicacion;
                    proyecto.Archivo = xmlDocument.InnerXml;

                    _context.Proyectos.Add(proyecto);
                    _context.SaveChanges();

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

        public ResponseModel DeletePublicacion(int id)
        {
            var response = new ResponseModel();
            try
            {
                var currentPublicacion = _context.Publicacions.Where(d =>
                    d.IdPublicacion == id
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

                        //var image = _multimediaService.UploadFile(imagen, publication.IdPublicacion, publication.Titulo, 1);
                        var image = "";
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

        public async Task<ResponseModel> GetPublication(int id)
        {
            var response = new ResponseModel();

            try
            {/*
                var publication = await _context.Publicacions.Where(x => x.IdPublicacion == id).FirstOrDefaultAsync();
                if (publication == null) throw new Exception("Not found");
                var multimedia = (from d in _context.Multimedia where d.IdPublicacion == publication.IdPublicacion select d.Direccion);
                var comentarios = (from d in _context.Comentarios
                                  where d.IdPublicacion == publication.IdPublicacion
                                  select new
                                  {
                                      autor = (from e in _context.Usuarios where e.IdUsuario == d.IdAutor select e.Nombre),
                                      fecha = d.FechaPublicacion,
                                      comentario = d.Descripcion
                                  }).ToList();
                publication.Comentarios = comentarios;*/

                var publicacion = await (from p in _context.Publicacions
                                         where p.IdPublicacion == id
                                         select new
                                         {
                                             idPublicacion = p.IdPublicacion,
                                             titulo = p.Titulo,
                                             nombre = (from a in _context.Usuarios where a.IdUsuario == p.IdAutor select a.Nombre).FirstOrDefault(),
                                             apellido = (from a in _context.Usuarios where a.IdUsuario == p.IdAutor select a.Apellido).FirstOrDefault(),
                                             foto = (from a in _context.Usuarios where a.IdUsuario == p.IdAutor select a.Foto).FirstOrDefault(),
                                             fecha = p.FechaPublicacion,
                                             descripcion = p.Descripcion,
                                             idAutor = p.IdAutor,
                                             proyecto = (from file in _context.Proyectos where file.IdPublicacion == p.IdPublicacion select file.Archivo).First(),
                                             multimedia = (from m in _context.Multimedia
                                                           where m.IdPublicacion == p.IdPublicacion select m.Direccion).ToList(),
                                             comentarios = (from c in _context.Comentarios
                                                            where c.IdPublicacion == p.IdPublicacion
                                                            select new
                                                            {
                                                                nombre = (from u in _context.Usuarios where u.IdUsuario == c.IdAutor select u.Nombre).FirstOrDefault(),
                                                                apellido = (from u in _context.Usuarios where u.IdUsuario == c.IdAutor select u.Apellido).FirstOrDefault(),
                                                                foto = (from u in _context.Usuarios where u.IdUsuario == c.IdAutor select u.Foto).FirstOrDefault(),
                                                                fecha = c.FechaPublicacion,
                                                                comentario = c.Descripcion
                                                            }).ToList()
                                         }).FirstOrDefaultAsync();

                response.Exito = 1;
                response.Data = publicacion;
            }
            catch (Exception ex)
            {
                response.Exito = 0;
                response.Mensage = ex.Message;
            }
            return response; 
        }
    }
}
