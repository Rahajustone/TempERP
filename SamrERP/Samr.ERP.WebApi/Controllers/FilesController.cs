using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Samr.ERP.Core.Interfaces;
using Samr.ERP.Core.Services;
using SixLabors.Shapes;
using Path = System.IO.Path;

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

        [HttpGet("{path}")]
        [ResponseCache(Duration = 547657)]
        public IActionResult GetPhoto(string path)
        {
            if (System.IO.File.Exists(FileService.GetFullPath(path)))
            {
                var image = System.IO.File.OpenRead(FileService.GetFullPath(path));
                return File(image, "image/jpeg");
            }

            return NotFound("Image not found.");
        }

        [HttpGet]
        public async Task<IActionResult> GetArchiveFile(string path)
        {
            if (System.IO.File.Exists(FileService.GetFullArchivePath(path)))
            {
                var fileName = await _fileService.GetFileShortDescription(path);

                var file = System.IO.File.OpenRead(FileService.GetFullArchivePath(path));

                var fileExtension = System.IO.Path.GetExtension(path).ToLower();

                return File(file, FileService.GetMimeType(fileExtension), fileName + fileExtension);
            }

            return NotFound("File not found.");
        }
    }
}