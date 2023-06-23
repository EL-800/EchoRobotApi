using System;
using System.Collections.Generic;

namespace EchoRobotApi.Models;

public partial class Multimedia
{
    public int IdArchivo { get; set; }

    public int? IdPublicacion { get; set; }

    public string? Direccion { get; set; }

    public virtual Publicacion? IdPublicacionNavigation { get; set; }
}
