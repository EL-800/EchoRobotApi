using System.ComponentModel.DataAnnotations;

namespace EchoRoborApi.Models.Request.Comunity
{
    public class PublicacionRequest
    {
        [Required]
        public int IdAutor { get; set; }
        [Required]
        public string? Titulo { get; set; }
        [Required]
        public string? Descripcion { get; set; }

        [Required]
        public ICollection<IFormFile> Elementos { get; set; } = new List<IFormFile>();
        public IFormFile? Proyecto { get; set; }
    }
}
