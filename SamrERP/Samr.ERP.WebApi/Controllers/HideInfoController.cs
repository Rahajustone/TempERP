using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Samr.ERP.Core.Interfaces;
using Samr.ERP.WebApi.Hub;

namespace Samr.ERP.WebApi.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    //[Authorize]
    public class HideInfoController : ControllerBase
    {
        private readonly IFileService _fileService;
        private readonly IHubContext<NotificationHub> _hub;

        public HideInfoController(IFileService fileService, IHubContext<NotificationHub> hub)
        {
            _fileService = fileService;
            _hub = hub;
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

        [HttpGet]
        public IActionResult Get()
        {
            var timerManager = _hub.Clients.All.SendAsync("transferchartdata", "message recieve");

            return Ok(new { Message = "Request Completed" });
        }
    }
}
