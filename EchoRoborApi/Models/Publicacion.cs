using System;
using System.Collections.Generic;

namespace EchoRoborApi.Models;

public partial class Publicacion
{
    public int IdPublicacion { get; set; }

    public int? IdAutor { get; set; }

    public string? Titulo { get; set; }

    public string? Descripcion { get; set; }

    public DateTime? FechaPublicacion { get; set; }

    public virtual ICollection<Comentario> Comentarios { get; } = new List<Comentario>();

    public virtual Usuario? IdAutorNavigation { get; set; }

    public virtual ICollection<Multimedia> Multimedia { get; } = new List<Multimedia>();
}
