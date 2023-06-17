namespace EchoRobotApi.Models
{
    public class UserResponse
    {
        public string Nombre { get; set; } = "";
        public string Apellido { get; set; } = "";
        public string Token { get; set; } = "";
        public int IdUsuario { get; set; }
        public string UrlFoto { get; set; } = "";
    }
}
