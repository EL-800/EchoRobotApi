using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace EchoRoborApi.Services.Interfaces
{
    public interface IMultimediaService
    {
        public bool DeleteFile(string path);
        public string EncryptName(string name);
        public bool isImage(IFormFile file);
        public bool isVideo(IFormFile file);

        public Task<string> UploadPhotoUserAsync(Stream file, string name);
    }
}
