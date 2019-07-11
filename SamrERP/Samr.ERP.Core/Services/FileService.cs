using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Samr.ERP.Core.Interfaces;
using Samr.ERP.Infrastructure.Data.Contracts;

namespace Samr.ERP.Core.Services
{
    public class FileService:IFileService
    {
        public  static readonly string[] AllowedExtensions = {
            ".jpg",
            ".jpeg",
            ".doc",
            ".docx",
            ".xls",
            ".xlsx",
            ".pdf",
            ".csv",
            ".ppt",
            ".pptx"
        };
        public FileService(
            IUnitOfWork unitOfWork

            )
        {
            
        }

        public async Task<string> SaveFile(string folderPath,IFormFile file)
        {

            throw new Exception();
        }
    }
}
