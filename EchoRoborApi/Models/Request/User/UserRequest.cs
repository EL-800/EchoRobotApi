using System.ComponentModel.DataAnnotations;

namespace EchoRoborApi.Models.Request.User
{
    public class UserRequest
    {
        [Required]
        public string? Nombre { get; set; }
        [Required]
        public string? Apellido { get; set; }
        [Required]
        public string? Email { get; set; }
        [Required]
        public string? Password { get; set; }

    }
}
