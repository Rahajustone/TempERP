using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Samr.ERP.Core.Interfaces;
using Samr.ERP.Core.Models;
using Samr.ERP.Core.Models.ResponseModels;
using Samr.ERP.Core.Staff;
using Samr.ERP.Core.ViewModels.Common;
using Samr.ERP.Core.ViewModels.FileArchive;
using Samr.ERP.WebApi.Filters;

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
        public async Task<BaseDataResponse<PagedList<GetListFileArchiveViewModel>>> All([FromQuery]PagingOptions pagingOptions, [FromQuery]FilterFileArchiveViewModel filterFileArchiveViewModel, [FromQuery] SortRule sortRule)
        {
            var fileArchives = await _fileArchiveService.GetAllAsync(pagingOptions, filterFileArchiveViewModel, sortRule);
            return Response(fileArchives);
        }

        [HttpGet("{id}")]
        public async Task<BaseDataResponse<GetByIdFileArchiveViewModel>> Get(Guid id)
        {
            var fileArchives = await _fileArchiveService.GetByIdAsync(id);

            return Response(fileArchives);
        }

        [HttpPost]
        [TrimInputStrings]
        public async Task<BaseDataResponse<GetByIdFileArchiveViewModel>> Create([FromForm] CreateFileArchiveViewModel createFileArchiveViewModel)
        {
            if (ModelState.IsValid)
            {
                var result = await _fileArchiveService.CreateAsync(createFileArchiveViewModel);
                return Response(result);
            }

            return Response(BaseDataResponse<GetByIdFileArchiveViewModel>.Fail(null));
        }

        [HttpPost]
        [TrimInputStrings]
        public async Task<BaseDataResponse<GetByIdFileArchiveViewModel>> Edit([FromForm] EditFileArchiveViewModel editFileArchiveViewModel)
        {
            if (ModelState.IsValid)
            {
                var result = await _fileArchiveService.EditAsync(editFileArchiveViewModel);
                return Response(result);
            }

            return Response(BaseDataResponse<GetByIdFileArchiveViewModel>.Fail(null));
        }
    }
}
