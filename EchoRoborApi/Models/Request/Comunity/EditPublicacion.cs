using System.ComponentModel.DataAnnotations;

namespace EchoRoborApi.Models.Request.Comunity
{
    public class EditPublicacion
    {
        public int IdPublicacion { get; set; }
        [Required]
        public int IdAutor { get; set; }
        [Required]
        public string Titulo { get; set; }
        [Required]
        public string Descripcion { get; set; }

        public ICollection<IFormFile> Elementos { get; set; } = new List<IFormFile>();
    }
}
