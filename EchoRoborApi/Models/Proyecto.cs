using System;
using System.Collections.Generic;

namespace EchoRobotApi.Models;

public partial class Proyecto
{
    public int IdProyecto { get; set; }

    public string Archivo { get; set; } = null!;

    public int IdPublicacion { get; set; }

    public virtual Publicacion IdPublicacionNavigation { get; set; } = null!;
}
