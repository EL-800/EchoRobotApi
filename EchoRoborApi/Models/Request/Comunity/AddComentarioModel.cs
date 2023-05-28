using System.ComponentModel.DataAnnotations;

namespace EchoRoborApi.Models.Request.Comunity
{
    public class AddComentarioModel
    {
        [Required]
        public int IdPublicacion { get; set; }
        [Required]
        public int IdAutor { get; set; }
        [Required]
        public string? Descripcion { get; set; }

    }
}
