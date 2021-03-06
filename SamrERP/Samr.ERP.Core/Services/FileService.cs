﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Samr.ERP.Core.Interfaces;
using Samr.ERP.Infrastructure.Data.Contracts;
using Samr.ERP.Infrastructure.Entities;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using SixLabors.Primitives;


namespace Samr.ERP.Core.Services
{
    public class FileService : IFileService
    {
        private const string ApiBaseUrl = "http://51.145.98.38:4445";
        private readonly IUnitOfWork _unitOfWork;
        private static string _filesPath;
        private static readonly int _resizeSize = 150;
        public static readonly Dictionary<string, string> AllowedExtensionsWithMiMeType = new Dictionary<string, string>()
        {
            {".jpeg", "image/jpeg"},
            {".jpg", "image/jpeg"},
            {".doc", "application/msword"},
            {".docx", "application/vnd.openxmlformats-officedocument.wordprocessingml.document"},
            {".xls", "application/vnd.ms-excel"},
            {".xlsx", "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"},
            {".pdf", "application/pdf"},
            {".csv", "text/csv"},
            {".ppt", "application/vnd.ms-powerpoint"},
            {".pptx", "application/vnd.openxmlformats-officedocument.presentationml.presentation"}
        };

        public static readonly string EmployeePhotoFolderPath = "Employees\\Photo";
        public static readonly string EmployeePassportScanFolderPath = "Employees\\PassportScan";
        public static readonly string NewsPhotoFolderPath = "News\\Photo";


        public static readonly string FileArchiveFolderPath = "FileArchiveFolder";

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

            var fileName = Guid.NewGuid().ToString();

            var filePath = System.IO.Path.Combine(directoryPath, fileName + fileExtension);
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }
            if (createResizedClone)
            {
                var resizedFileName = Path.Combine(_filesPath, folderPath, GetResizedName(filePath));

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

            return $"{Path.Combine(folderPath, fileName + fileExtension)}";
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

            return fileName;
        }

        public static string GetDownloadAction(string path)
        {
            if (string.IsNullOrEmpty(path)) return String.Empty;

            return ApiBaseUrl + "/api/files/getphoto?path=" + path;
        }

        public static string GetFileArchivePath(string path)
        {
            if (string.IsNullOrEmpty(path)) return String.Empty;

            return ApiBaseUrl + "/api/files/GetArchiveFile?path=" + path;
        }

        public async Task<string> GetFileShortDescription(string path)
        {
            var fileArchive = await _unitOfWork.FileArchives.GetDbSet()
                .FirstOrDefaultAsync(p => p.FilePath.ToLower() == path.ToLower());

            if (fileArchive == null)
            {
                return String.Empty;
            }

            var fileName = fileArchive.Title;

            return fileName;
        }

        public static string GetFullPath(string path)
        {
            return Path.Combine(_filesPath, path);
        }

        public static string GetFullArchivePath(string path)
        {
            return Path.Combine(_filesPath, FileArchiveFolderPath + "\\" + path);
        }

        private static void EnsureDirectoryCreated(string directoryPath)
        {
            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }
        }

        public static string GetResizedName(string filePath)
        {
            var fileExtension = Path.GetExtension(filePath);
            var fileName = Path.GetFileNameWithoutExtension(filePath);
            return $"{fileName}-{_resizeSize}x{_resizeSize}{fileExtension}";

        }

        public static string GetResizedPath(string filePath)
        {
            if (string.IsNullOrEmpty(filePath)) return filePath;
            var resizedName = GetResizedName(filePath);
            return $"{Path.Combine(Path.GetDirectoryName(filePath), resizedName)}";
        }

        private bool ExtensionAllowed(string extension)
        {
            return AllowedExtensionsWithMiMeType.Keys.Any(p => p.Equals(extension));
        }

        public static string GetMimeType(string fileExtension)
        {
            return AllowedExtensionsWithMiMeType[fileExtension];
        }
    }
}
