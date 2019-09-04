using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Samr.ERP.Core.Enums;
using Samr.ERP.Core.Interfaces;
using Samr.ERP.Core.Models.ErrorModels;
using Samr.ERP.Core.Models.ResponseModels;
using Samr.ERP.Core.Staff;
using Samr.ERP.Core.ViewModels.Account;
using Samr.ERP.Core.ViewModels.Common;
using Samr.ERP.Core.ViewModels.Employee;
using Samr.ERP.Infrastructure.Data.Contracts;
using Samr.ERP.Infrastructure.Entities;
using Samr.ERP.Infrastructure.Extensions;
using Samr.ERP.Infrastructure.Providers;

namespace Samr.ERP.Core.Services
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<User> _userManager;
        private readonly IEmailSender _emailSender;
        private readonly ISMSSender _smsSender;
        private readonly UserProvider _userProvider;
        private readonly IActiveUserTokenService _activeUserTokenService;
        private readonly IMapper _mapper;

        public UserService(
            IUnitOfWork unitOfWork,
            UserManager<User> userManager,
            IEmailSender emailSender,
            ISMSSender smsSender,
            UserProvider userProvider,
            IActiveUserTokenService activeUserTokenService,
            IMapper mapper
            )
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
            _emailSender = emailSender;
            _smsSender = smsSender;
            _userProvider = userProvider;
            _activeUserTokenService = activeUserTokenService;
            _mapper = mapper;

        }

        public async Task<IdentityResult> CreateAsync(User user, string password)
        {
            var identityResult = await _userManager.CreateAsync(user, password);

            return identityResult;
        }

        public async Task<User> GetByUserName(string userName)
        {
            var userResult = await _unitOfWork.Users.GetDbSet().FirstOrDefaultAsync(p => p.UserName == userName);
            return userResult;
        }

        public async Task<BaseDataResponse<UserViewModel>> GetByIdAsync(Guid id)
        {
            var userResult = await _unitOfWork.Users.GetDbSet()
                .Include(p => p.UserLockReason)
                    .ThenInclude( p => p.CreatedUser)
                        .ThenInclude( p => p.Employee)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (userResult == null) return BaseDataResponse<UserViewModel>.NotFound(null);
            return BaseDataResponse<UserViewModel>.Success(_mapper.Map<UserViewModel>(userResult)); ;
        }

        public bool HasUserValidRefreshToken(Guid userId, string refreshToken, string ipAddress)
        {
            return _unitOfWork.RefreshTokens.Any(p => p.UserId == userId && p.Active && p.RemoteIpAddress == ipAddress && p.Token == refreshToken);
        }

        public async Task AddRefreshToken(string token, Guid userId, string remoteIpAddress, double daysToExpire = 5)
        {
            var refreshToken = new RefreshToken()
            {
                Token = token,
                Expires = DateTime.Now.AddDays(daysToExpire),
                UserId = userId,
                RemoteIpAddress = remoteIpAddress
            };

            _unitOfWork.RefreshTokens.Add(refreshToken);
            await _unitOfWork.CommitAsync();
        }

        public async Task RemoveRefreshToken(Guid userId, string token)
        {
            var refreshToken = await _unitOfWork.RefreshTokens.All()
                .FirstOrDefaultAsync(p => p.UserId == userId && p.Token == token);

            if (refreshToken != null)
            {
                _unitOfWork.RefreshTokens.Delete(refreshToken);
                await _unitOfWork.CommitAsync();
            }
        }

        public async Task<BaseResponse> GenerateChangePasswordConfirmationCodeToCurrentUser()
        {
            var confirmCode = RandomGenerator.GenerateRandomNumber(1000, 9999);
            var user = _userProvider.CurrentUser;
            if (user == null)
            {
                return BaseResponse.Unauthorized(null);
            }

            user.ChangePasswordConfirmationCode = confirmCode;
            user.ChangePasswordConfirmationCodeExpires = DateTime.Now.AddMinutes(2);

            //await _emailSender.SendEmailToUserAsync(user, "Confirmation code",
            //    $"Change password confirmation code {confirmCode}", true);
            await _smsSender.SendSMSToUserAsync(user, 
                $"Change password confirmation code {confirmCode}", true);

            await _unitOfWork.CommitAsync();

            return BaseResponse.Success();
        }

        public async Task<User> GetByPhoneNumber(string phoneNumber)
        {
            var userResult = await _unitOfWork.Users.GetDbSet().FirstOrDefaultAsync(p => p.PhoneNumber == phoneNumber);
            return userResult;
        }

        public async Task<User> GetUserAsync(ClaimsPrincipal userPrincipal)
        {
            if (userPrincipal == null) return null;
            Guid.TryParse(userPrincipal.Claims.FirstOrDefault(c => c.Type == "id")?.Value, out Guid id);

            var user = await _unitOfWork.Users.GetDbSet().FirstOrDefaultAsync(p => p.Id == id); // await _userManager.GetUserAsync(userPrincipal);
            return user;
        }

        public IEnumerable<User> GetAllUser()
        {
            var users = _unitOfWork.Users.GetAll().ToList();
            return users;
        }

        public async Task<BaseDataResponse<IEnumerable<SelectListItemViewModel>>> UsersSelectListItemsAsync()
        {
            var users = await _unitOfWork.Users.GetDbSet()
                .Where(p => p.Id != _userProvider.CurrentUser.Id)
                .Where( p => p.Id != GuidExtensions.FULL_GUID)
                .Where(u => !(u.LockUserId.HasValue))
                .Include(e => e.Employee)
                .ToListAsync();

            var vm = _mapper.Map<IEnumerable<SelectListItemViewModel>>(users);

            return BaseDataResponse<IEnumerable<SelectListItemViewModel>>.Success(vm);
        }

        public async Task<BaseDataResponse<string>> ResetPasswordAsync(ResetPasswordViewModel resetPasswordModel)
        {
            var user = await GetByPhoneNumber(resetPasswordModel.PhoneNumber);
            if (user == null) return BaseDataResponse<string>.Fail("", new ErrorModel(ErrorCode.UserNotExists));

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);

            var generatedPass = RandomGenerator.GenerateNewPassword();
            var resetPasswordResult = await _userManager.ResetPasswordAsync(user, token, generatedPass);

            //await _emailSender.SendEmailToUserAsync(user, "Reset password", $"Your account pass was reset, new pass {generatedPass}", true);
            await _smsSender.SendSMSToUserAsync(user, $"Your account pass was reset, new pass {generatedPass}", true);

            if (!resetPasswordResult.Succeeded)
                return BaseDataResponse<string>.Fail(null, resetPasswordResult.Errors.Select(p => new ErrorModel()
                {
                    //Code = //TODO: надо доделать
                    Description = p.Description
                }).ToArray());

            return BaseDataResponse<string>.Success(generatedPass);
        }

        public async Task<BaseDataResponse<string>> ChangePasswordAsync(ChangePasswordViewModel viewModel)
        {
            BaseDataResponse<string> dataResponse;

            var user = _userProvider.CurrentUser;
            if (user == null) return BaseDataResponse<string>.NotFound("");

            //
            if (viewModel.SmsConfirmationCode == user.ChangePasswordConfirmationCode
                && user.ChangePasswordConfirmationCodeExpires >= DateTime.Now)
            {
                var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                var resetPasswordResult = await _userManager.ResetPasswordAsync(user, token, viewModel.Password);

                if (resetPasswordResult.Succeeded)
                    dataResponse = BaseDataResponse<string>.Success(null);
                else
                    dataResponse = BaseDataResponse<string>.Fail(null, resetPasswordResult.Errors.Select(p => new ErrorModel()
                    {
                        //Code = //TODO: надо доделать
                        Description = p.Description
                    }).ToArray());
            }
            else dataResponse = BaseDataResponse<string>.Fail(string.Empty, new ErrorModel("invalid confirmation code"));

            return dataResponse;
        }

        public async Task<BaseResponse> UserLockAsync(LockUserViewModel lockUserViewModel)
        {
            var userExists = await _unitOfWork
                .Users.GetDbSet()
                .FirstOrDefaultAsync(u => u.Id == lockUserViewModel.Id);

            var userLockReasonExists = await _unitOfWork
                .UserLockReasons.GetDbSet()
                .FirstOrDefaultAsync(u => u.Id == lockUserViewModel.UserLockReasonId);

            if (userExists == null
                || userLockReasonExists == null
                || userExists.UserLockReasonId != null)
                return BaseResponse.Fail();

            LockUser(userExists,lockUserViewModel.UserLockReasonId);

            _activeUserTokenService.DeactivateTokenByUserId(userExists.Id);


            await _unitOfWork.CommitAsync();

            return BaseResponse.Success();
        }

        public void LockUser(User user,Guid userLockReasonId)
        {
            user.UserLockReasonId = userLockReasonId;
            user.LockDate = DateTime.Now;
            user.LockUserId = _userProvider.CurrentUser.Id;
        }

        public void UnlockUser(User user)
        {
            user.UserLockReasonId = null;
            user.LockDate = null;
            user.LockUserId = null;
        }
        public async Task<BaseResponse> UserUnlockAsync(Guid id)
        {
            var userExists = await _unitOfWork
                .Users.GetDbSet()
                .Include(p=>p.Employee)
                .FirstOrDefaultAsync(u => u.Id == id);

            if (userExists?.UserLockReasonId == null)
                return BaseResponse.Fail();

            if (userExists.Employee.LockDate != null)
                return BaseResponse.Fail(new ErrorModel(ErrorCode.EmployeeLockedAndUserLocked));

            UnlockUser(userExists);
            
            await _unitOfWork.CommitAsync();

            return BaseResponse.Success();
        }

        public async Task<BaseDataResponse<GetEmployeeDataViewModel>> GetEmployeeDataAsync()
        {
            BaseDataResponse<GetEmployeeDataViewModel> dataResponse;

            var employee = await _unitOfWork.Employees.GetDbSet()
                .Include(p => p.User)
                .Include(p => p.CreatedUser)
                .Include(p => p.Position)
                .Include(p => p.Position.Department)
                .Include(p => p.Gender)
                .Include(p => p.EmployeeLockReason)
                .FirstOrDefaultAsync(p => p.UserId == _userProvider.CurrentUser.Id);

            if (employee == null)
            {
                dataResponse = BaseDataResponse<GetEmployeeDataViewModel>.NotFound(null);
            }
            else
            {
                dataResponse = BaseDataResponse<GetEmployeeDataViewModel>.Success(_mapper.Map<GetEmployeeDataViewModel>(employee));
            }

            return dataResponse;
        }

        #region Roles

        public async Task<BaseResponse> SetUserRolesAsync(SetUserRolesViewModel model)
        {
            var user = await _unitOfWork.Users.GetByIdAsync(model.UserId);
            if (user == null)
            {
                return BaseResponse.NotFound(new ErrorModel("user not found"));
            }

            var allRoles = await _unitOfWork.Roles.All().Select(p => p.Name).ToListAsync();

            var selectedRoles = allRoles.Intersect(model.RolesName).ToList();

            var userRoles = await _userManager.GetRolesAsync(user);

            var toRemove = allRoles.Except(selectedRoles).Intersect(userRoles).ToList();

            var toAdd = selectedRoles.Except(userRoles);
            //removing not selected roles
            var removeRoleResult = await _userManager.RemoveFromRolesAsync(user, toRemove);
            if (!removeRoleResult.Succeeded) return BaseResponse.Fail(removeRoleResult.Errors.ToErrorModels());

            //adding new roles
            var addRoleResult = await _userManager.AddToRolesAsync(user, toAdd);
            if (!addRoleResult.Succeeded) return BaseResponse.Fail(addRoleResult.Errors.ToErrorModels());

            _activeUserTokenService.DeactivateTokenByUserId(user.Id);
            return BaseResponse.Success();

        }

        public async Task ClearUserRefreshToken(Guid userId)
        {
            _unitOfWork.RefreshTokens.GetDbSet()
                .RemoveRange(_unitOfWork.RefreshTokens.All().Where(p => p.UserId == userId));

            await _unitOfWork.CommitAsync();
        }

        public async Task<BaseDataResponse<IEnumerable<GroupedUserRolesViewModel>>> GetUserRolesAsync(Guid id)
        {
            var user = await _unitOfWork.Users.GetByIdAsync(id);

            if (user == null)
                return BaseDataResponse<IEnumerable<GroupedUserRolesViewModel>>.NotFound(null);

            var userRoles = await _userManager.GetRolesAsync(user);


            var roles = await _unitOfWork.Roles.All()
                .GroupBy(r => r.Category)
                .Select(p => new GroupedUserRolesViewModel()
                {
                    GroupName = p.First().CategoryName,
                    Roles = p.Select(r => new UserRolesViewModel()
                    {
                        Name = r.Name,
                        Description = r.Description,
                        Selected = userRoles.Contains(r.Name)
                    })
                }).ToListAsync();


            return BaseDataResponse<IEnumerable<GroupedUserRolesViewModel>>.Success(roles);
        }

        #endregion
    }
}
