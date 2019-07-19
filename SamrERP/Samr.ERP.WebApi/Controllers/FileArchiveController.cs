using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Samr.ERP.Core.Interfaces;
using Samr.ERP.Core.Models;
using Samr.ERP.Core.Models.ResponseModels;
using Samr.ERP.Core.Stuff;
using Samr.ERP.Core.ViewModels.Common;
using Samr.ERP.Core.ViewModels.FileArchive;

namespace Samr.ERP.WebApi.Controllers
{
    [Route("api/[controller]/[action]")]
    [Authorize]
    [ApiController]
    public class FileArchiveController : ApiController
    {
        private readonly IFileArchiveService _fileArchiveService;

        public FileArchiveController(IFileArchiveService fileArchiveService)
        {
            _fileArchiveService = fileArchiveService;
        }

        [HttpGet]
        public async Task<BaseDataResponse<PagedList<EditFileArchiveViewModel>>> All([FromQuery]PagingOptions pagingOptions, [FromQuery]FilterFileArchiveViewModel filterFileArchiveViewModel, [FromQuery] SortRule sortRule)
        {
            var fileArchives = await _fileArchiveService.GetAllAsync(pagingOptions, filterFileArchiveViewModel, sortRule);
            return Response(fileArchives);
        }

        [HttpGet("{id}")]
        public async Task<BaseDataResponse<EditFileArchiveViewModel>> Get(Guid id)
        {
            var fileArchives = await _fileArchiveService.GetByIdAsync(id);

            return Response(fileArchives);
        }

        [HttpPost]
        public async Task<BaseDataResponse<EditFileArchiveViewModel>> Create([FromForm] EditFileArchiveViewModel editFileArchiveViewModel)
        {
            if (ModelState.IsValid)
            {
                var result = await _fileArchiveService.CreateAsync(editFileArchiveViewModel);
                return Response(result);
            }

            return Response(BaseDataResponse<EditFileArchiveViewModel>.Fail(editFileArchiveViewModel, null));
        }

        [HttpPost]
        public async Task<BaseDataResponse<EditFileArchiveViewModel>> Edit([FromForm] EditFileArchiveViewModel editFileArchiveViewModel)
        {
            if (ModelState.IsValid)
            {
                var result = await _fileArchiveService.EditAsync(editFileArchiveViewModel);
                return Response(result);
            }

            return Response(BaseDataResponse<EditFileArchiveViewModel>.Fail(null, null));
        }
    }
}
