using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Samr.ERP.Core.Interfaces;
using Samr.ERP.Core.Models;
using Samr.ERP.Core.Models.ResponseModels;
using Samr.ERP.Core.Services;
using Samr.ERP.Core.Stuff;
using Samr.ERP.Core.ViewModels.Handbook;
using Samr.ERP.Infrastructure.Entities;

namespace Samr.ERP.WebApi.Controllers
{
    [Route("api/[controller]/[action]")]
    [Authorize]
    [ApiController]
    public class HandbookController : ApiController
    {
        private readonly IHandbookService _handbook;

        public HandbookController(IHandbookService handbook)
        {
            _handbook = handbook;
        }

        [HttpGet]
        public async Task<BaseDataResponse<IEnumerable<HandbookViewModel>>> All()
        {
            var handbooks = await _handbook.GetAllAsync();
            return Response(handbooks);
        }
    }
}