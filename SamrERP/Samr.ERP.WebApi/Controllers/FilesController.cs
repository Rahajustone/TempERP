using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Samr.ERP.Core.Interfaces;
using Samr.ERP.Core.Services;
using SixLabors.Shapes;

namespace Samr.ERP.WebApi.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    //[Authorize]
    public class FilesController :ControllerBase
    {
        private readonly IFileService _fileService;

        public FilesController(IFileService fileService)
        {
            _fileService = fileService;
        }

        [HttpGet]
        public IActionResult GetPhoto(string path)
        {
            var image = System.IO.File.OpenRead(FileService.GetFullPath(path));
            return File(image, "image/jpeg");
        }

        [HttpGet]
        public async Task<IActionResult> GetArchiveFile(string path)
        {
            var fileName = await _fileService.GetFileShortDescription(path);

            var file = System.IO.File.OpenRead(FileService.GetFullArchivePath(path));
            
            var fileExtension = System.IO.Path.GetExtension(path).ToLower();

            return File(file, FileService.GetMimeType(fileExtension), fileName+ fileExtension);
        }

    }
}