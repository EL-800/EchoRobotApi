using System;
using System.Collections.Generic;

namespace EchoRobotApi.Models;

public partial class Publicacion
{
    public int IdPublicacion { get; set; }

    public int IdAutor { get; set; }

    public string Titulo { get; set; } = null!;

    public string Descripcion { get; set; } = null!;

    public DateTime FechaPublicacion { get; set; }

    public virtual ICollection<Comentario> Comentarios { get; } = new List<Comentario>();

    public virtual Usuario IdAutorNavigation { get; set; } = null!;

    public virtual ICollection<Multimedia> Multimedia { get; } = new List<Multimedia>();

    public virtual ICollection<Proyecto> Proyectos { get; } = new List<Proyecto>();
}
