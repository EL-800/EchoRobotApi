﻿using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace EchoRoborApi.Services.Interfaces
{
    public interface IMultimediaService
    {
        public string UploadFile(IFormFile file, int id, string name,int place);
        public bool DeleteFile(string path);
        public string EncryptName(string name);
        public bool isImage(IFormFile file);
        public bool isVideo(IFormFile file);
    }
}
