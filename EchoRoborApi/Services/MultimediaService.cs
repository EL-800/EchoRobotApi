using EchoRoborApi.Services.Interfaces;
using Microsoft.AspNetCore.Components.Web;
using System.Reflection.PortableExecutable;
using System.Security.Cryptography;
using System.Text;

namespace EchoRoborApi.Services
{
    public class MultimediaService : IMultimediaService
    {
        private readonly string _PathUser =@"AppData\Usuarios";
        private readonly string _PathPublication = @"AppData\Publicacion";

        public string EncryptName(string name)
        {
            byte[] nameBytes = Encoding.UTF8.GetBytes(name);
            MD5 md5 = MD5.Create();
            byte[] keyBytes = md5.ComputeHash(nameBytes);
            return BitConverter.ToString(keyBytes).Replace("-", "").ToLower();

        }
        public string UploadFile(IFormFile file, int id, string name, int place)
        {
            try
            {
                string ruta = place == 0? _PathUser : _PathPublication;

                int lastIndex = file.FileName.LastIndexOf(".");
                string extension = file.FileName.Substring(lastIndex);

                string rutaDocumento = Path.Combine(ruta, EncryptName(file.FileName + id + name));
                using (FileStream newFile = File.Create(rutaDocumento + extension))
                {
                    file.CopyTo(newFile);
                    newFile.Flush();
                }
                return rutaDocumento;
            }
            catch (Exception)
            {
                return null;
            };
        }
        public bool DeleteFile(string path)
        {
            if (File.Exists(path))
            {
                File.Delete(path);
                return true;
            }
            return false;
        }

        public bool isImage(IFormFile file)
        {
            string extension = Path.GetExtension(file.FileName).ToLower();

            if (extension.Equals(".jpg") || extension.Equals(".jpeg") || extension.Equals(".png")) return true;

            return false;
        }
        public bool isVideo(IFormFile file)
        {
            string extension = Path.GetExtension(file.FileName).ToLower();

            if (extension.Equals(".mp4") || extension.Equals(".m4a")) return true;

            return false;
        }
    }
}
