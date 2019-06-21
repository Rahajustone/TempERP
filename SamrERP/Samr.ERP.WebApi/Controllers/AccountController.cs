﻿using System.Collections.Generic;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Samr.ERP.Core.Interfaces;
using Samr.ERP.Core.Models.ResponseModels;
using Samr.ERP.Infrastructure.Entities;
using Samr.ERP.WebApi.Infrastructure;
using Samr.ERP.WebApi.ViewModels.Account;

namespace Samr.ERP.WebApi.Controllers
{
    //[Authorize]
    //[EnableCors("AllowOrigin")]
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class AccountController : ControllerBase
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
    
        [HttpPost()]
        public async Task<UserViewModel> Register([FromBody] RegisterUserViewModel registerModel)
        {
            var user = new User()
            {
                UserName = registerModel.Email,
                Email = registerModel.Email,
                PhoneNumber = registerModel.Phone
            };
            var createdUser = await _userService.CreateAsync(user, registerModel.Password);
            var vm = _mapper.Map<UserViewModel>(createdUser.Model);
            return vm;

        }
        
        [HttpPost]
        //[AllowAnonymous]
        //[EnableCors("AllowOrigin")]
        public async Task<ActionResult<AuthorizationResult>> Login([FromBody]LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                //TODO:Amir need to complete
                var isAuthenticatedResult = await _authenticateService.IsAuthenticated(model);

                return Ok(isAuthenticatedResult);
                
            }
            return BadRequest("validation error");
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
        public async Task<ActionResult<AuthorizationResult>> ForgotPassword([FromBody] ResetPasswordViewModel resetPasswordModel)
        {
            if (ModelState.IsValid)
            {
                //TODO:Need to complete
                var isAuthenticatedResult = await _authenticateService.ResetPassword(resetPasswordModel);

                return Ok(isAuthenticatedResult);

            }
            return BadRequest("validation error");
        }
    }
}