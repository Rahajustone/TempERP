﻿using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Samr.ERP.Core.Interfaces;
using Samr.ERP.Core.Models.ResponseModels;
using Samr.ERP.Core.ViewModels.Account;
using Samr.ERP.Infrastructure.Entities;
using Samr.ERP.WebApi.Infrastructure;
using AuthenticateResult = Samr.ERP.WebApi.Models.AuthenticateResult;


namespace Samr.ERP.WebApi.Controllers
{
    //[Authorize]
    //[EnableCors("AllowOrigin")]
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class AccountController : ApiController
    {
        private readonly IMapper _mapper;
        private readonly IUserService _userService;
        private readonly IAuthenticateService _authenticateService;


        public AccountController(
            IMapper mapper,
            IUserService userService,
            IAuthenticateService authenticateService
        )
        {
            _mapper = mapper;
            _userService = userService;
            _authenticateService = authenticateService;
        }
    
        [HttpPost]
        public async Task<BaseResponse<UserViewModel>> Register([FromBody] RegisterUserViewModel registerModel)
        {
            if (ModelState.IsValid)
            {
                var createdUserResponse = await _userService.CreateAsync(registerModel, registerModel.Password);

                return Response(createdUserResponse);

            }
            return Response(BaseResponse<UserViewModel>.Fail(null,null));

        }
        
        [HttpPost]
        //[AllowAnonymous]
        public async Task<BaseResponse<AuthenticateResult>> Login([FromBody]LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                //TODO:Amir need to complete
                var authenticateResponse = await _authenticateService.AuthenticateAsync(model);

                return Response(authenticateResponse);
                
            }
            return Response(BaseResponse<AuthenticateResult>.Fail(null, null));
        }

        [HttpGet]
        [Authorize]
        public async Task<UserViewModel> UserInfo()
        {
            var currentUser = await _userService.GetCurrentUserAsync(User);
            return _mapper.Map<UserViewModel>(currentUser);
        }

        [HttpGet]
        [Authorize]
        public IEnumerable<User> UserAll()
        {
            var users = _userService.GetAllUser();
            return users;
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<BaseResponse<string>> ResetPassword([FromBody] ResetPasswordViewModel resetPasswordModel)
        {
            if (ModelState.IsValid)
            {
                //TODO:Need to complete
                var resetPasswordResponse = await _userService.ResetPassword(resetPasswordModel);

                return Response(resetPasswordResponse);

            }
            return BaseResponse<string>.Fail(null,null);
        }
    }
}