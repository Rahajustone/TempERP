using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Samr.ERP.Core.Interfaces;
using Samr.ERP.Core.Models.ResponseModels;
using Samr.ERP.Infrastructure.Data.Contracts;

namespace Samr.ERP.Core.Services
{
    public class UploadFileService : IUploadFileService
    {
        public async Task<string> StorePhoto(string uploadFilePath, IFormFile file)
        {
            var fileExtension = Path.GetExtension(file.FileName).ToLower();
            if (fileExtension != ".jpg" || fileExtension != ".jpeg" || fileExtension != ".gif" ||
                fileExtension != ".png")
                return "Selected file type not allowed!";

            if (!Directory.Exists(uploadFilePath))
            {
                Directory.CreateDirectory(uploadFilePath);
            }
            var fileName = DateTime.Now.ToString("yyyyMMddHHmmss") + fileExtension;

            var filePath = Path.Combine(uploadFilePath, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            return filePath;
        }
    }
}
