using EchoRoborApi.Models;
using EchoRoborApi.Models.Request.Comunity;

namespace EchoRoborApi.Services.Interfaces
{
    public interface IComunityService
    {
        public  Task<ResponseModel> AddPublicacion(PublicacionRequest request);
        public ResponseModel EditPublicacion(EditPublicacion request);
        public ResponseModel DeletePublicacion(int id , int idUser);

        public ResponseModel AddComentario(AddComentarioModel request);

        public Task<ResponseModel> ListTopPublication();
        public Task<ResponseModel> ListPublication();


    }
}
