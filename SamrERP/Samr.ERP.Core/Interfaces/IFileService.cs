using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Samr.ERP.Core.Interfaces
{
    public interface IFileService
    {
        Task<string> SaveFile(string folderPath,IFormFile file);
        Task<string> UploadPhoto(string folderPath, IFormFile file, bool createResizedClone);
        Task<string> GetFileShortDescription(string path);
    }
}