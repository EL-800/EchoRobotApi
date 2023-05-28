using System;
using System.Collections.Generic;

namespace EchoRoborApi.Models;

public partial class Comentario
{
    public int IdRespuesta { get; set; }

    public int? IdPublicacion { get; set; }

    public int? IdAutor { get; set; }

    public string? Descripcion { get; set; }

    public DateTime? FechaPublicacion { get; set; }

    public virtual Usuario? IdAutorNavigation { get; set; }

    public virtual Publicacion? IdPublicacionNavigation { get; set; }
}
