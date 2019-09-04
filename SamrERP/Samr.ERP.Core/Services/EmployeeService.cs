using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using Samr.ERP.Core.Enums;
using Samr.ERP.Core.Interfaces;
using Samr.ERP.Core.Models;
using Samr.ERP.Core.Models.ErrorModels;
using Samr.ERP.Core.Models.ResponseModels;
using Samr.ERP.Core.Staff;
using Samr.ERP.Core.ViewModels.Account;
using Samr.ERP.Core.ViewModels.Employee;
using Samr.ERP.Infrastructure.Data.Contracts;
using Samr.ERP.Infrastructure.Entities;
using Samr.ERP.Infrastructure.Extensions;
using Samr.ERP.Infrastructure.Providers;

namespace Samr.ERP.Core.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserService _userService;
        private readonly UserProvider _userProvider;
        private readonly IEmailSender _emailSender;
        private readonly ISMSSender _smsSender;
        private readonly IFileService _fileService;
        private readonly IActiveUserTokenService _activeUserTokenService;
        private readonly IMapper _mapper;

        public EmployeeService(
            IUnitOfWork unitOfWork,
            UserService userService,
            UserProvider userProvider,
            IEmailSender emailSender,
            ISMSSender smsSender,
            IFileService fileService,
            IActiveUserTokenService activeUserTokenService,
            IMapper mapper

            )
        {
            _unitOfWork = unitOfWork;
            _userService = userService;
            _userProvider = userProvider;
            _emailSender = emailSender;
            _smsSender = smsSender;
            _fileService = fileService;
            _activeUserTokenService = activeUserTokenService;
            _mapper = mapper;
        }

        private static IQueryable<Employee> FilterEmployeesQuery(FilterEmployeeViewModel filterEmployeeViewModel, IQueryable<Employee> query)
        {
            if (filterEmployeeViewModel.FullName != null)
            {
                var filterFullName = filterEmployeeViewModel.FullName.ToLower();

                query = query.Where(e =>
                    EF.Functions.Like(Extension.FullNameToString(e.LastName, e.FirstName, e.MiddleName).ToString(),
                        "%" + filterFullName + "%"));
            }

            if (filterEmployeeViewModel.DepartmentId != null)
                query = query.Where(e => e.Position.DepartmentId == filterEmployeeViewModel.DepartmentId);

            if (filterEmployeeViewModel.OnlyUsers)
            {
                query = query.Where(e => e.UserId != null);
            }

            return query;
        }

        public async Task<BaseDataResponse<GetEmployeeViewModel>> GetByIdAsync(Guid id)
        {
            var employee = await EmployeeById(id);
            
            if (employee == null)
                return BaseDataResponse<GetEmployeeViewModel>.NotFound(_mapper.Map<GetEmployeeViewModel>(employee));

            var employeeLog = await _unitOfWork.EmployeeLogs.GetDbSet()
                .Include(p => p.CreatedUser)
                .ThenInclude(p => p.Employee)
                .OrderByDescending( p => p.CreatedAt)
                .FirstOrDefaultAsync(p => p.EmployeeId == employee.Id);

            var vm = _mapper.Map<GetEmployeeViewModel>(employee);

            if (employeeLog != null)
            {
                vm.LastEditedAt = employeeLog.CreatedAt.ToStringCustomFormat();
            }

            return BaseDataResponse<GetEmployeeViewModel>.Success(vm);
        }

        public async Task<GetEmployeeCardTemplateViewModel> GetEmployeeCardByIdAsync(Guid id)
        {
            var employee = await EmployeeById(id);

            return _mapper.Map<GetEmployeeCardTemplateViewModel>(employee);
        }

        private async Task<Employee> EmployeeById(Guid id)
        {
            var employee = await _unitOfWork.Employees.GetDbSet()
                .Include(p => p.User)
                .Include(p => p.CreatedUser)
                    .ThenInclude( p => p.Employee)
                .Include(p => p.Position)
                .Include(p => p.Position.Department)
                .Include(p => p.Gender)
                .Include(p => p.EmployeeLockReason)
                .FirstOrDefaultAsync(p => p.Id == id);
            return employee;
        }

        public async Task<BaseDataResponse<PagedList<AllEmployeeViewModel>>> AllAsync(PagingOptions pagingOptions, FilterEmployeeViewModel filterEmployeeViewModel, SortRule sortRule)
        {
            var query = _unitOfWork.Employees
                .GetDbSet()
                .Include(p => p.CreatedUser)
                .Include(p => p.Position)
                .Include(p => p.Position.Department)
                .Include(p => p.LockUser)
                .Include(p => p.User)
                .Where(e => e.EmployeeLockReasonId == null);

            query = FilterEmployeesQuery(filterEmployeeViewModel, query.AsNoTracking());

            var queryVm = query.ProjectTo<AllEmployeeViewModel>();

            var orderedQuery = queryVm.OrderBy(sortRule, p => p.FullName);

            var pagedList = await orderedQuery.ToPagedListAsync(pagingOptions);

            foreach (var allEmployeeViewModel in pagedList.Items)
            {
                allEmployeeViewModel.PhotoPath =
                    FileService.GetDownloadAction(FileService.GetResizedPath(allEmployeeViewModel.PhotoPath));
            }
            //var pagedList = await query.ToMappedPagedListAsync<Employee, AllEmployeeViewModel>(pagingOptions);

            return BaseDataResponse<PagedList<AllEmployeeViewModel>>.Success(pagedList);
        }

        public async Task<BaseDataResponse<EditEmployeeViewModel>> CreateAsync(EditEmployeeViewModel editEmployeeViewModel)
        {


            var emailExist = _unitOfWork.Employees.Any(predicate: e =>
                e.Email.ToLower() == editEmployeeViewModel.Email.ToLower()
            );
            //TODO phone edit
            if (emailExist)
                return BaseDataResponse<EditEmployeeViewModel>.Fail(editEmployeeViewModel,
                    new ErrorModel(ErrorCode.EmailMustBeUnique));

            if (_unitOfWork.Employees.Any(p => p.Phone == editEmployeeViewModel.Phone))
                return BaseDataResponse<EditEmployeeViewModel>.Fail(editEmployeeViewModel,
                    new ErrorModel(ErrorCode.PhoneMustBeUnique));

            if (editEmployeeViewModel.DateOfBirth.AddYears(16) > DateTime.Now)
                return BaseDataResponse<EditEmployeeViewModel>.Fail(editEmployeeViewModel, new ErrorModel("invalid birthday"));


            var employee = _mapper.Map<Employee>(editEmployeeViewModel);

            if (editEmployeeViewModel.Photo != null)
            {
                employee.PhotoPath = await _fileService.UploadPhoto(FileService.EmployeePhotoFolderPath, editEmployeeViewModel.Photo, true);
            }
            _unitOfWork.Employees.Add(employee);
            await _unitOfWork.CommitAsync();

            return BaseDataResponse<EditEmployeeViewModel>.Success(_mapper.Map<EditEmployeeViewModel>(employee));
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

            var generateNewPassword = RandomGenerator.GenerateNewPassword();
            var createUserResult = await _userService.CreateAsync(user, generateNewPassword);

            if (!createUserResult.Succeeded)
                return BaseDataResponse<UserViewModel>.Fail(null, createUserResult.Errors.ToErrorModels());

            employee.UserId = user.Id;

            await _unitOfWork.CommitAsync();
            //await _emailSender.SendEmailToUserAsync(user, "User created", $"Account was created, your pass {generateNewPassword}", true);
            await _smsSender.SendSMSToUserAsync(user, $"Account was created, your pass {generateNewPassword}", true);
            return BaseDataResponse<UserViewModel>.Success(_mapper.Map<UserViewModel>(user));

        }

        public async Task<BaseDataResponse<EditEmployeeViewModel>> EditAsync(EditEmployeeViewModel editEmployeeViewModel)
        {
            BaseDataResponse<EditEmployeeViewModel> dataResponse;

            var employeExists = await _unitOfWork.Employees.GetDbSet().FirstOrDefaultAsync(e => e.Id == editEmployeeViewModel.Id);

            if (employeExists == null)
            {
                dataResponse = BaseDataResponse<EditEmployeeViewModel>.NotFound(editEmployeeViewModel, new ErrorModel(ErrorCode.EmailMustBeUnique));
            }
            else
            {

                var checkEmailUnique = await _unitOfWork.Employees
                    .GetDbSet()
                    .AnyAsync(e => e.Id != editEmployeeViewModel.Id
                                   && e.Email.ToLower() == editEmployeeViewModel.Email.ToLower());
                if (checkEmailUnique)
                {
                    return BaseDataResponse<EditEmployeeViewModel>.Fail(editEmployeeViewModel, new ErrorModel(ErrorCode.EmailMustBeUnique));
                }

                if (await _unitOfWork.Employees.AnyAsync(p => p.Id != editEmployeeViewModel.Id && p.Phone == editEmployeeViewModel.Phone))
                    return BaseDataResponse<EditEmployeeViewModel>.Fail(editEmployeeViewModel, new ErrorModel(ErrorCode.PhoneMustBeUnique));

                if (editEmployeeViewModel.DateOfBirth.AddYears(16) > DateTime.Now)
                    return BaseDataResponse<EditEmployeeViewModel>.Fail(editEmployeeViewModel, new ErrorModel("invalid birthday"));


                var existsUser = await _unitOfWork
                    .Employees
                    .GetDbSet()
                    .Where(p => p.Id == editEmployeeViewModel.Id && p.UserId != null)
                    .Select(p => p.User)
                    .FirstOrDefaultAsync();

                if (existsUser != null)
                {
                    existsUser.Email = editEmployeeViewModel.Email;

                    _unitOfWork.Users.Update(existsUser);
                }

                await AddToLog(employeExists);
                var employee = _mapper.Map<EditEmployeeViewModel, Employee>(editEmployeeViewModel, employeExists);
                if (editEmployeeViewModel.Photo != null)
                {
                    employee.PhotoPath = await _fileService.UploadPhoto(FileService.EmployeePhotoFolderPath, editEmployeeViewModel.Photo, true);
                }
                _unitOfWork.Employees.Update(employee);

                await _unitOfWork.CommitAsync();

                dataResponse = BaseDataResponse<EditEmployeeViewModel>.Success(_mapper.Map<EditEmployeeViewModel>(employee));
            }

            return dataResponse;
        }

        public async Task<BaseResponse> EditUserDetailsAsync(
            EditUserDetailsViewModel editUserDetailsView)
        {
            var userExists = await _unitOfWork.Users.ExistsAsync(_userProvider.CurrentUser.Id);

            if (!userExists)
                return BaseResponse.NotFound();

            var employee = await _unitOfWork.Employees.All().FirstOrDefaultAsync(x => x.UserId == _userProvider.CurrentUser.Id);

            employee.Email = editUserDetailsView.Email;
            employee.FactualAddress = editUserDetailsView.FactualAddress;

            var employeeEditResult = await EditAsync(_mapper.Map<EditEmployeeViewModel>(employee));
            return employeeEditResult.Meta.Success
                ? BaseResponse.Success()
                : new BaseResponse(employeeEditResult.Meta.StatusCode, employeeEditResult.Meta.Errors);
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

            _unitOfWork.Employees.Update(employee);

            if (employee.User != null)
            {
                _userService.LockUser(employee.User, GuidExtensions.FULL_GUID);
            }

            await _unitOfWork.CommitAsync();
            if (employee.User != null)
            {
                _activeUserTokenService.DeactivateTokenByUserId(employee.UserId.Value);

            }
            return BaseResponse.Success();
        }

        public async Task<BaseResponse> UnLockEmployeeAsync(Guid employeeId)
        {

            var employee = await _unitOfWork.Employees.GetDbSet()
                .Include(p => p.User)
                .FirstOrDefaultAsync(p => p.Id == employeeId);

            if (employee?.EmployeeLockReasonId == null) return BaseResponse.NotFound();


            employee.LockUserId = null;
            employee.EmployeeLockReasonId = null;
            employee.LockDate = null;

            if (employee.User != null)
            {
                _userService.UnlockUser(employee.User);
            }
            await _unitOfWork.CommitAsync();

            return BaseResponse.Success();
        }

        public async Task<BaseDataResponse<GetPassportDataEmployeeViewModel>> GetPassportDataAsync(Guid employeeId)
        {
            BaseDataResponse<GetPassportDataEmployeeViewModel> dataResponse;

            var passportDataAsync = await _unitOfWork.Employees.GetDbSet()
                .Include(p => p.Nationality)
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

        public async Task<BaseDataResponse<PagedList<AllLockEmployeeViewModel>>> GetAllLockedEmployeeAsync(PagingOptions pagingOptions, FilterEmployeeViewModel filterEmployeeViewModel, SortRule sortRule)
        {
            var query = _unitOfWork.Employees
                .GetDbSet()
                .Include(p => p.CreatedUser)
                .Include(p => p.Position)
                .Include(p => p.Position.Department)
                .Include(u => u.LockUser)
                .Include(p => p.EmployeeLockReason)
                .Where(e => e.EmployeeLockReasonId != null);

            query = FilterEmployeesQuery(filterEmployeeViewModel, query);

            var queryVm = query.ProjectTo<AllLockEmployeeViewModel>();

            var orderedQuery = queryVm.OrderBy(sortRule, p => p.FullName);


            var pagedList = await orderedQuery.ToPagedListAsync(pagingOptions);

            return BaseDataResponse<PagedList<AllLockEmployeeViewModel>>.Success(pagedList);
        }

        public async Task<BaseResponse> EditPassportDataAsync(EditPassportDataEmployeeViewModel editPassportDataEmployeeViewModel)
        {
            BaseResponse response;

            var existEmployee = await _unitOfWork.Employees
                .GetDbSet()
                .FirstOrDefaultAsync(e => e.Id == editPassportDataEmployeeViewModel.EmployeeId);

            if (existEmployee != null)
            {
                var passportNumberUnique = await _unitOfWork.Employees.GetDbSet()
                    .AnyAsync(e => e.Id != editPassportDataEmployeeViewModel.EmployeeId
                                   && e.PassportNumber.ToLower() == editPassportDataEmployeeViewModel.PassportNumber.ToLower());

                if (passportNumberUnique)
                {
                    response = BaseResponse.Fail(new ErrorModel(ErrorCode.PassportNumberMustBeUnique));
                }
                else
                {
                    await AddToLog(existEmployee);
                    var passportDataEmployee = _mapper.Map<EditPassportDataEmployeeViewModel, Employee>(editPassportDataEmployeeViewModel, existEmployee);

                    if (editPassportDataEmployeeViewModel.PassportScan != null)
                    {
                        passportDataEmployee.PassportScanPath = await _fileService.UploadPhoto(FileService.EmployeePassportScanFolderPath, editPassportDataEmployeeViewModel.PassportScan, true);
                    }

                    _unitOfWork.Employees.Update(passportDataEmployee);

                    await _unitOfWork.CommitAsync();

                    response = BaseResponse.Success();
                }
            }
            else
            {
                response = BaseResponse.NotFound();
            }

            return response;
        }

        public async Task<EmployeeInfoTokenViewModel> GetEmployeeInfo(Guid id)
        {
            var emp = await _unitOfWork.Employees.GetDbSet().Include(p => p.Position).FirstOrDefaultAsync(
                e => e.UserId == id);
            var vm = _mapper.Map<EmployeeInfoTokenViewModel>(emp);

            return vm;
        }
        public async Task<IList<ExportExcelViewModel>> ExportToExcelAsync(FilterEmployeeViewModel filterEmployeeViewModel, SortRule sortRule)
        {
            var query = _unitOfWork.Employees
                .GetDbSet()
                .Include(p => p.CreatedUser)
                .Include(p => p.Position)
                .Include(p => p.Position.Department)
                .Include(p => p.LockUser)
                .Include(p => p.User)
                .Where(e => e.EmployeeLockReasonId == null);

            query = FilterEmployeesQuery(filterEmployeeViewModel, query);

            var queryVm = query.ProjectTo<ExportExcelViewModel>();

            var orderedQuery = queryVm.OrderBy(sortRule, p => p.FullName);

            var all = await orderedQuery.ToListAsync();

            return _mapper.Map<IList<ExportExcelViewModel>>(all);
        }

        private async Task AddToLog(Employee employee, bool commit = false)
        {
            if (employee != null)
            {
                var employeeLog = _mapper.Map<EmployeeLog>(employee);

                _unitOfWork.EmployeeLogs.Add(employeeLog);
                if (commit) await _unitOfWork.CommitAsync();
            }
        }

        public async Task<BaseDataResponse<PagedList<EmployeeLogViewModel>>> GetAllLogAsync(Guid id, PagingOptions pagingOptions, SortRule sortRule)
        {
            var query = _unitOfWork.EmployeeLogs.GetDbSet()
                .Include(p => p.CreatedUser)
                .Include(p => p.Position)
                .Include(p => p.Position.Department)
                .Include(p => p.LockUser)
                .Include(p => p.User)
                .Where(e => e.EmployeeId == id);
            //.OrderByDescending(p => p.CreatedAt);

            var queryVm = query.ProjectTo<EmployeeLogViewModel>();

            var orderedQuery = queryVm.OrderBy(sortRule, p => p.FirstName);

            var pagedList = await orderedQuery.ToPagedListAsync(pagingOptions);

            foreach (var allEmployeeViewModel in pagedList.Items)
            {
                if (allEmployeeViewModel.PhotoPath != null)
                {
                    allEmployeeViewModel.PhotoPath =
                        FileService.GetDownloadAction(FileService.GetResizedPath(allEmployeeViewModel.PhotoPath));
                }
            }

            return BaseDataResponse<PagedList<EmployeeLogViewModel>>.Success(pagedList);
        }
    }
}