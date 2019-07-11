using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Samr.ERP.Core.Interfaces
{
    public interface IFileService
    {
        Task<string> SaveFile(string folderPath,IFormFile file);
    }
}