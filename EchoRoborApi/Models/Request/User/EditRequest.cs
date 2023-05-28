using System.ComponentModel.DataAnnotations;

namespace EchoRoborApi.Models.Request.User
{
    public class EditRequest
    {
        [Required]
        public int IdUsuario { get; set; }
        [Required]
        public string? Nombre { get; set; }
        [Required]
        public string? Apellido { get; set; }

    }
}
