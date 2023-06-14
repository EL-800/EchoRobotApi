using EchoRoborApi.Services.Interfaces;
using Firebase.Auth;
using Firebase.Auth.Providers;
using Firebase.Storage;
using System.Reflection.PortableExecutable;
using System.Security.Cryptography;
using System.Text;

namespace EchoRoborApi.Services
{
    public class MultimediaService : IMultimediaService
    {

        private readonly string _email = "echorobot@gmail.com";
        private readonly string _clave = "echorobot";
        private readonly string _ruta = "echorobot-1e60e.appspot.com";
        private readonly string _apiKey = "AIzaSyCo2pBTKQy8JHCF9IcJ2e4osZqGB-9H18I";

        public string EncryptName(string name)
        {
            byte[] nameBytes = Encoding.UTF8.GetBytes(name);
            MD5 md5 = MD5.Create();
            byte[] keyBytes = md5.ComputeHash(nameBytes);
            return BitConverter.ToString(keyBytes).Replace("-", "").ToLower();

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

        public async Task<string> UploadPhotoUserAsync(Stream file, string name)
        {
            var config = new FirebaseAuthConfig
            {
                ApiKey = _apiKey,
                AuthDomain = "localhost",
                Providers = new FirebaseAuthProvider[]
                {
                    new EmailProvider()
                }
            };
            var client = new FirebaseAuthClient(config);
            var userCredential = await client.SignInWithEmailAndPasswordAsync(_email, _clave);

            var cancellation = new CancellationTokenSource();

            var task = new FirebaseStorage(_ruta, new FirebaseStorageOptions
            {
                AuthTokenAsyncFactory = () => userCredential.User.GetIdTokenAsync(),
                ThrowOnCancel = true
            }).Child("UserPhotos").Child(name).PutAsync(file, cancellation.Token);

            var downloadUrl = await task;

            return downloadUrl;
        }
        public async Task<string> UploadFilePublicationAsync(Stream file, string name , int id)
        {
            var config = new FirebaseAuthConfig
            {
                ApiKey = _apiKey,
                AuthDomain = "localhost",
                Providers = new FirebaseAuthProvider[]
                {
                    new EmailProvider()
                }
            };
            var client = new FirebaseAuthClient(config);
            var userCredential = await client.SignInWithEmailAndPasswordAsync(_email, _clave);

            var cancellation = new CancellationTokenSource();

            var task = new FirebaseStorage(_ruta, new FirebaseStorageOptions
            {
                AuthTokenAsyncFactory = () => userCredential.User.GetIdTokenAsync(),
                ThrowOnCancel = true
            }).Child("Comunity").Child(id.ToString()).Child(EncryptName(name+id + new Random().Next(1000000))).PutAsync(file, cancellation.Token);

            var downloadUrl = await task;

            return downloadUrl;
        }
    }
}
