namespace EchoRoborApi.Models.Request.User
{
    public class UserPhotoRequest
    {
        public int IdUsuario { get; set; }
        public IFormFile File { get; set; }
    }
}
