using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Samr.ERP.Core.Interfaces;
using Samr.ERP.Core.Models.ErrorModels;
using Samr.ERP.Core.Models.ResponseModels;
using Samr.ERP.Core.Stuff;
using Samr.ERP.Core.ViewModels.Account;
using Samr.ERP.Core.ViewModels.Employee;
using Samr.ERP.Core.ViewModels.Handbook;
using Samr.ERP.Infrastructure.Data;
using Samr.ERP.Infrastructure.Data.Contracts;
using Samr.ERP.Infrastructure.Entities;
using Samr.ERP.Infrastructure.Providers;

namespace Samr.ERP.Core.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserService _userService;
        private readonly UserProvider _userProvider;
        private readonly IMapper _mapper;
        private readonly IUploadFileService _file;

        public EmployeeService(
            IUnitOfWork unitOfWork,
            IUserService userService,
            UserProvider userProvider,
            IMapper mapper,
            IUploadFileService file
            )
        {
            _unitOfWork = unitOfWork;
            _userService = userService;
            _userProvider = userProvider;
            _mapper = mapper;
            _file = file;
        }

        public async Task<BaseDataResponse<GetEmployeeViewModel>> GetByIdAsync(Guid id)
        {
            BaseDataResponse<GetEmployeeViewModel> dataResponse;

            var employee = await _unitOfWork.Employees.GetDbSet()
                .Include(p => p.CreatedUser)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (employee == null)
            {
                dataResponse = BaseDataResponse<GetEmployeeViewModel>.NotFound(null);
            }
            else
            {
                dataResponse = BaseDataResponse<GetEmployeeViewModel>.Success(_mapper.Map<GetEmployeeViewModel>(employee));
            }

            return dataResponse;
        }

        public async Task<BaseDataResponse<IEnumerable<AllEmployeeViewModel>>> AllAsync()
        {
            var employee = await _unitOfWork.Employees
                .GetDbSet()
                .Include(p => p.CreatedUser)
                .Include(p => p.Position)
                .Include(p => p.Position.Department)
                .Include(l => l.EmployeeLockReason)
                .Include(u => u.LockUser)
                .ToListAsync();

            var vm = _mapper.Map<IEnumerable<AllEmployeeViewModel>>(employee);

            return BaseDataResponse<IEnumerable<AllEmployeeViewModel>>.Success(vm);
        }

        public async Task<BaseDataResponse<EditEmployeeViewModel>> CreateAsync(EditEmployeeViewModel editEmployeeViewModel)
        {
            BaseDataResponse<EditEmployeeViewModel> dataResponse;

            // TODO Raha
            //var filePathName = await _file.StorePhoto("wwwroot/employers", filePath);
            //dataResponse = BaseDataResponse<EditEmployeeViewModel>.Success(editEmployeeViewModel);

            var employeeExists = _unitOfWork.Employees.Any(predicate: e =>
                e.Phone.ToLower() == editEmployeeViewModel.Phone.ToLower() &&
                e.PassportNumber.ToLower() == editEmployeeViewModel.PassportNumber.ToLower());

            if (employeeExists)
            {
                dataResponse = BaseDataResponse<EditEmployeeViewModel>.Fail(editEmployeeViewModel, new ErrorModel("Phone number or Passport already exist"));

            }
            else
            {

                var employee = _mapper.Map<Employee>(editEmployeeViewModel);
                _unitOfWork.Employees.Add(employee);
                await _unitOfWork.CommitAsync();

                dataResponse = BaseDataResponse<EditEmployeeViewModel>.Success(_mapper.Map<EditEmployeeViewModel>(employee));
            }

            return dataResponse;
        }

        public async Task<BaseDataResponse<UserViewModel>> CreateUserForEmployee(Guid employeeId)
        {
            var employee = await _unitOfWork.Employees.GetByIdAsync(employeeId);

            if (employee == null)
                return BaseDataResponse<UserViewModel>.NotFound(null);

            var user = new User()
            {
                UserName = employee.Phone,
                Email = employee.Email,
                PhoneNumber = employee.Phone
            };

            var createUserResult = await _userService.CreateAsync(user, PasswordGenerator.GenerateNewPassword());

            if (!createUserResult.Succeeded)
                return BaseDataResponse<UserViewModel>.Fail(null, createUserResult.Errors.ToErrorModels());

            employee.UserId = user.Id;

            await _unitOfWork.CommitAsync();

            return BaseDataResponse<UserViewModel>.Success(_mapper.Map<UserViewModel>(user));

        }

        public async Task<BaseDataResponse<EditEmployeeViewModel>> UpdateAsync(EditEmployeeViewModel editEmployeeViewModel)
        {
            BaseDataResponse<EditEmployeeViewModel> dataResponse;

            var employeExists = await _unitOfWork.Employees.ExistsAsync(editEmployeeViewModel.Id);

            if (!employeExists)
            {
                dataResponse = BaseDataResponse<EditEmployeeViewModel>.NotFound(editEmployeeViewModel, new ErrorModel("Not found employee"));
            }
            else
            {
                var existsUser = await _unitOfWork.Employees.GetDbSet()
                    .Where(p => p.Id == editEmployeeViewModel.Id && p.UserId != null)
                    .Select(p => p.User)
                    .FirstOrDefaultAsync();
                if (existsUser != null)
                {
                    existsUser.Email = editEmployeeViewModel.Email;

                    _unitOfWork.Users.Update(existsUser);
                }

                var employee = _mapper.Map<Employee>(editEmployeeViewModel);

                _unitOfWork.Employees.Update(employee);

                await _unitOfWork.CommitAsync();

                dataResponse = BaseDataResponse<EditEmployeeViewModel>.Success(_mapper.Map<EditEmployeeViewModel>(employee));
            }

            return dataResponse;
        }

        public async Task<BaseResponse> EditUserDetailsAsync(
            EditUserDetailsViewModel editUserDetailsView)
        {
            var userExists = await _unitOfWork.Users.ExistsAsync(editUserDetailsView.UserId);

            if (!userExists)
                return BaseResponse.NotFound();

            var employee = await _unitOfWork.Employees.All().FirstOrDefaultAsync(x => x.UserId == editUserDetailsView.UserId);

            employee.Email = editUserDetailsView.Email;
            employee.AddressFact = editUserDetailsView.AddressFact;

            await UpdateAsync(_mapper.Map<EditEmployeeViewModel>(employee));

            return BaseResponse.Success();
        }

        public async Task<BaseResponse> LockEmployeeAsync(LockEmployeeViewModel lockEmployeeViewModel)
        {
            var employee = await _unitOfWork.Employees.GetDbSet()
                .Include(p => p.User)
                .FirstOrDefaultAsync(p => p.Id == lockEmployeeViewModel.EmployeeId);

            var employeeLockReason =
                await _unitOfWork.EmployeeLockReasons.GetByIdAsync(lockEmployeeViewModel.EmployeeLockReasonId);

            if (employee == null
                || employeeLockReason == null
                || !employeeLockReason.IsActive
                || employee.EmployeeLockReasonId != null) return BaseResponse.NotFound();

            employee.LockUserId = _userProvider.CurrentUser.Id;
            employee.EmployeeLockReasonId = employeeLockReason.Id;
            employee.LockDate = DateTime.Now;

            await _unitOfWork.CommitAsync();

            return BaseResponse.Success();
        }

        public async Task<BaseResponse> UnLockEmployeeAsync(Guid employeeId)
        {
            var employee = await _unitOfWork.Employees.GetDbSet()
                .Include(p => p.User)
                .FirstOrDefaultAsync(p => p.Id == employeeId);

            if (employee == null || employee.EmployeeLockReasonId != null) return BaseResponse.NotFound();

            employee.LockUserId = null;
            employee.EmployeeLockReasonId = null;
            employee.LockDate = null;

            await _unitOfWork.CommitAsync();

            return BaseResponse.Success();
        }

        public async Task<BaseDataResponse<GetPassportDataEmployeeViewModel>> GetPassportDataAsync(Guid employeeId)
        {
            BaseDataResponse<GetPassportDataEmployeeViewModel> dataResponse;

            var passportDataAsync = await _unitOfWork.Employees.GetDbSet()
                .Include(p => p.PassportNationality)
                .FirstOrDefaultAsync(p => p.Id == employeeId);
            if (passportDataAsync == null)
            {
                dataResponse = BaseDataResponse<GetPassportDataEmployeeViewModel>.NotFound(null, new ErrorModel("Not found employee"));
            }
            else
            {
                dataResponse = BaseDataResponse<GetPassportDataEmployeeViewModel>.Success(_mapper.Map<GetPassportDataEmployeeViewModel>(passportDataAsync));
            }

            return dataResponse;
        }
    }
}
