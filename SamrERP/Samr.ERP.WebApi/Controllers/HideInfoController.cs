using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Samr.ERP.Core.Interfaces;

namespace Samr.ERP.WebApi.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    //[Authorize]
    public class HideInfoController : ControllerBase
    {
        private readonly IFileService _fileService;

        public HideInfoController(IFileService fileService)
        {
            _fileService = fileService;
        }
        [HttpGet]
        public ActionResult<IEnumerable<string>> SecretInfo()
        {
            return Ok(Enumerable.Range(1, 5).Select(p => $"secret info {p}"));
        }

        //[HttpPost]
        //public ActionResult UploadFile(IFormFile formFile)
        //{
        //    _fileService.SaveFile("test\\test", formFile);
        //    return Ok();
        //}
        //[HttpPost]
        //public ActionResult UploadPhoto(IFormFile formFile)
        //{
        //    _fileService.UploadPhoto("Files\\Test", formFile,true);
        //    return Ok();
        //}
        [HttpGet]
        public ActionResult DateTimeNow()
        {
            return Ok(DateTime.Now.ToString("F"));
        }

        [HttpGet]
        public int ErrorByCode(int statusCode)
        {
            Response.StatusCode = statusCode;
            return 0;
        }
    }
    
    
}