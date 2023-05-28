namespace EchoRoborApi.Models
{
    public class ResponseModel
    {
        public int Exito { get; set; }
        public string? Mensage { get; set; }

        public ResponseModel()
        {
            Exito = 0;
        }
    }
}
