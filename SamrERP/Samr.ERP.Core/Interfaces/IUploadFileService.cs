using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Samr.ERP.Core.Interfaces
{
    public interface IUploadFileService
    {
        Task<string> StorePhoto(string uploadFilePath, IFormFile file);
    }
}
