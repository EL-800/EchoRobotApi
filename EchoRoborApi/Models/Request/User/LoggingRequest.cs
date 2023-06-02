using System.ComponentModel.DataAnnotations;

namespace EchoRobotApi.Models.Request.User
{
    public class LoggingRequest
    {
        [Required]
        public string Email { get; set; } = "";
        [Required]
        public string Password { get; set; } = "";
    }
}
