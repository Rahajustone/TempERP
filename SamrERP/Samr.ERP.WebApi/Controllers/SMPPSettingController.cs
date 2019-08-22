using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Samr.ERP.Core.Interfaces;
using Samr.ERP.Core.Models.ResponseModels;
using Samr.ERP.Core.ViewModels.EmailSetting;
using Samr.ERP.Core.ViewModels.SMPPSetting;

namespace Samr.ERP.WebApi.Controllers
{
    [Route("api/[controller]/[action]")]
    [Authorize]
    [ApiController]
    public class SMPPSettingController : ApiController
    {
        private readonly ISMPPSettingService _smppSettingService;
        private readonly IMapper _mapper;

        public SMPPSettingController(
            ISMPPSettingService smppSettingService,
            IMapper mapper
        )
        {
            _smppSettingService = smppSettingService;
            _mapper = mapper;
        }

        [HttpPost]
        public async Task<BaseDataResponse<SMPPSettingResponseViewModel>> Create(SMPPSettingViewModel smppSettingViewModel)
        {
            if (ModelState.IsValid)
            {
                var resposne = await _smppSettingService.CreateAsync(smppSettingViewModel);
                return Response(resposne);
            }
            return Response(BaseDataResponse<SMPPSettingResponseViewModel>.Fail(_mapper.Map<SMPPSettingResponseViewModel>(smppSettingViewModel)));
        }

        [HttpGet]
        public BaseDataResponse<IEnumerable<SMPPSettingResponseViewModel>> GetAll()
        {
            return Response(_smppSettingService.GetAllAsync());
        }

        [HttpGet("{id}")]
        public async Task<BaseDataResponse<SMPPSettingResponseViewModel>> GetById(Guid id)
        {
            return Response(await _smppSettingService.GetByIdAsync(id));
        }

        [HttpPost]
        public async Task<BaseDataResponse<SMPPSettingResponseViewModel>> Edit(SMPPSettingViewModel smppSettingViewModel)
        {
            if (ModelState.IsValid)
            {
                var response = await _smppSettingService.EditAsync(smppSettingViewModel);

                return Response(response);
            }

            return Response(BaseDataResponse<SMPPSettingResponseViewModel>.Fail(_mapper.Map<SMPPSettingResponseViewModel>(smppSettingViewModel)));
        }
    }
}
