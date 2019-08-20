using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.Differencing;
using Samr.ERP.Core.Interfaces;
using Samr.ERP.Core.Models.ResponseModels;
using Samr.ERP.Core.ViewModels.EmailSetting;

namespace Samr.ERP.WebApi.Controllers
{
    [Route("api/[controller]/[action]")]
    [Authorize]
    [ApiController]
    public class EmailSettingController : ApiController
    {
        private readonly IEmailSettingService _emailSettingService;

        public EmailSettingController(
            IEmailSettingService emailSettingService
            )
        {
            _emailSettingService = emailSettingService;
        }

        [HttpPost]
        public async Task<BaseDataResponse<EmailSettingViewModel>> Create(EmailSettingViewModel emailSettingViewModel)
        {
            if (ModelState.IsValid)
            {
                var resposne = await _emailSettingService.CreateAsync(emailSettingViewModel);
                return Response(resposne);
            }
            return Response(BaseDataResponse<EmailSettingViewModel>.Fail(emailSettingViewModel));
        }

        [HttpGet]
        public BaseDataResponse<IEnumerable<EmailSettingViewModel>> GetAll()
        {
            return Response(_emailSettingService.GetAll());
        }

        [HttpGet("{id}")]
        public async Task<BaseDataResponse<EmailSettingViewModel>> GetById(Guid id)
        {
            return Response(await _emailSettingService.GetByIdAsync(id));
        }

        [HttpPost]
        public async Task<BaseDataResponse<EmailSettingViewModel>> Edit(EmailSettingViewModel emailSettingViewModel)
        {
            if (ModelState.IsValid)
            {
                var response = await _emailSettingService.EditAsync(emailSettingViewModel);

                return Response(response);
            }

            return Response(BaseDataResponse<EmailSettingViewModel>.Fail(emailSettingViewModel));
        }

        [HttpPost("{id}")]
        public async Task<BaseResponse> Delete(Guid id)
        {
            var response = await _emailSettingService.Delete(id);
            return Response(response);
        }
    }
}