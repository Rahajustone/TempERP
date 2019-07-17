using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Samr.ERP.Core.Interfaces;
using Samr.ERP.Core.Models.ResponseModels;
using Samr.ERP.Core.Stuff;
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
        private readonly IEmployeeService _employeeService;
        private readonly IAuthenticateService _authenticateService;


        public AccountController(
            IMapper mapper,
            IUserService userService,
            IEmployeeService employeeService,
            IAuthenticateService authenticateService
        )
        {
            _mapper = mapper;
            _userService = userService;
            _employeeService = employeeService;
            _authenticateService = authenticateService;
        }

        [HttpPost]
        public async Task<BaseDataResponse<UserViewModel>> Register([FromBody] RegisterUserViewModel registerModel)
        {
            if (ModelState.IsValid)
            {
                var user = new User()
                {
                    PhoneNumber = registerModel.Phone,
                    Email = registerModel.Email,
                    UserName = registerModel.Phone
                };

                var createdUserResponse = await _userService.CreateAsync(user, registerModel.Password);
                if (createdUserResponse.Succeeded)
                {
                    return Response(BaseDataResponse<UserViewModel>.Success(_mapper.Map<UserViewModel>(user)));

                }
                return Response(BaseDataResponse<UserViewModel>.Fail(_mapper.Map<UserViewModel>(user),createdUserResponse.Errors.ToErrorModels()));
            }
            return Response(BaseDataResponse<UserViewModel>.Fail(null, null));

        }

        [HttpPost]
        //[AllowAnonymous]
        public async Task<BaseDataResponse<AuthenticateResult>> Login([FromBody]LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                //TODO:Amir need to complete
                var authenticateResponse = await _authenticateService.AuthenticateAsync(model);

                return Response(authenticateResponse);

            }
            return Response(BaseDataResponse<AuthenticateResult>.Fail(null, null));
        }

        [HttpPost()]
        public async Task<BaseDataResponse<AuthenticateResult>> RefreshToken([FromBody] ExchangeRefreshToken model)
        {
            if (ModelState.IsValid)
            {

                var authenticateResponse = await _authenticateService.RefreshTokenAsync(model);

                return Response(authenticateResponse);
            }

            return Response(BaseDataResponse<AuthenticateResult>.Fail(null));
        }

        [HttpGet]
        [Authorize]
        public async Task<UserViewModel> UserInfo()
        {
            var currentUser = await _userService.GetUserAsync(User);
            return _mapper.Map<UserViewModel>(currentUser);
        }
        [HttpGet("{id}")]
        [Authorize]
        public async Task<BaseDataResponse<UserViewModel>> GetById(Guid id)
        {
            var userResponse = await _userService.GetByIdAsync(id);
            return userResponse;
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
        public async Task<BaseDataResponse<string>> ResetPassword([FromBody] ResetPasswordViewModel resetPasswordModel)
        {
            if (ModelState.IsValid)
            {
                //TODO:Need to complete
                var resetPasswordResponse = await _userService.ResetPasswordAsync(resetPasswordModel);

                return Response(resetPasswordResponse);

            }
            return Response(BaseDataResponse<string>.Fail(null));
        }
        [HttpPost]
        public async Task<BaseDataResponse<string>> ChangePassword([FromBody] ChangePasswordViewModel changePasswordViewModel)
        {
            if (ModelState.IsValid)
            {
                //TODO:Need to complete
                var resetPasswordResponse = await _userService.ChangePasswordAsync(changePasswordViewModel);

                return Response(resetPasswordResponse);

            }
            return Response(BaseDataResponse<string>.Fail(null));
        }

        
        [HttpPost("{id}")]
        public async Task<BaseResponse> SendChangePasswordConfirmationCode(Guid id)
        {
            return Response(await _userService.GenerateChangePasswordConfirmationCodeToCurrentUser());
        }


        [HttpPost]
        public async Task<BaseResponse> EditUserDetails([FromBody] EditUserDetailsViewModel editUserDetailsView)
        {
            if (ModelState.IsValid)
            {
                var response = await _employeeService.EditUserDetailsAsync(editUserDetailsView);

                return Response(response);
            }
            return Response(BaseResponse.Fail());
        }

        [HttpPost]
        public async Task<BaseResponse> LockUser([FromBody] LockUserViewModel lockUserViewModel)
        {
            if (ModelState.IsValid)
            {
                var response = await _userService.UserLockAsync(lockUserViewModel);

                return Response(response);
            }
            return Response(BaseResponse.Fail());
        }

        [HttpPost("{id}")]
        [Authorize]
        public async Task<BaseResponse> UnlockUser(Guid id)
        {
            if (ModelState.IsValid)
            {
                var response = await _userService.UserUnlockAsync(id);

                return Response(response);
            }
            return Response(BaseResponse.Fail());
        }
    }
}