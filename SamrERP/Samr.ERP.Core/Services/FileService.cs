using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Samr.ERP.Core.Interfaces;
using Samr.ERP.Infrastructure.Data.Contracts;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using SixLabors.Primitives;


namespace Samr.ERP.Core.Services
{
    public class FileService : IFileService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly string _filesPath;
        private readonly int _resizeSize = 200;
        public static readonly string[] AllowedExtensions = {
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
            _unitOfWork = unitOfWork;
            _filesPath = System.IO.Path.Combine(Directory.GetCurrentDirectory(), "Files");
        }

        public async Task<string> UploadPhoto(string folderPath, IFormFile file, bool createResizedClone)
        {
            var fileExtension = System.IO.Path.GetExtension(file.FileName).ToLower();
            if (!ExtensionAllowed(fileExtension))
                return null;

            var directoryPath = Path.Combine(_filesPath, folderPath);
            EnsureDirectoryCreated(directoryPath);

            var fileName = Guid.NewGuid().ToString() ;

            var filePath = System.IO.Path.Combine(directoryPath, fileName + fileExtension);
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            if (createResizedClone)
            {
                var resizedFileName = Path.Combine(_filesPath, folderPath, $"{fileName}-{_resizeSize}x{_resizeSize}{fileExtension}");

                var image = Image.Load(File.OpenRead(filePath));

                image.Mutate(p =>
                {
                    p.Resize(new ResizeOptions
                    {
                        Size = new Size(_resizeSize, _resizeSize),
                        Mode = ResizeMode.Max
                    });

                });

                image.Save(resizedFileName);

            }

            return filePath;

        }

        public async Task<string> SaveFile(string folderPath, IFormFile file)
        {
            var fileExtension = System.IO.Path.GetExtension(file.FileName).ToLower();
            if (!ExtensionAllowed(fileExtension))
                return null;

            var directoryPath = Path.Combine(_filesPath, folderPath);

            EnsureDirectoryCreated(directoryPath);
            var fileName = Guid.NewGuid() + fileExtension;

            var filePath = System.IO.Path.Combine(directoryPath, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            return filePath;
        }

        private static void EnsureDirectoryCreated(string directoryPath)
        {
            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }
        }

        private bool ExtensionAllowed(string extension)
        {
            return AllowedExtensions.Any(p => p.Equals(extension));
        }
    }
}
