using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using Samr.ERP.Core.Interfaces;
using Samr.ERP.Core.Models;
using Samr.ERP.Core.Models.ErrorModels;
using Samr.ERP.Core.Models.ResponseModels;
using Samr.ERP.Core.Stuff;
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
        private readonly IFileService _fileService;
        private readonly IMapper _mapper;

        public EmployeeService(
            IUnitOfWork unitOfWork,
            UserService userService,
            UserProvider userProvider,
            IEmailSender emailSender,
            IFileService fileService,
            IMapper mapper

            )
        {
            _unitOfWork = unitOfWork;
            _userService = userService;
            _userProvider = userProvider;
            _emailSender = emailSender;
            _fileService = fileService;
            _mapper = mapper;
        }

        private static IQueryable<Employee> FilterEmployeesQuery(FilterEmployeeViewModel filterEmployeeViewModel, IQueryable<Employee> query)
        {
            if (filterEmployeeViewModel.FullName != null)
            {
                var filterFullName = filterEmployeeViewModel.FullName.ToLower();

                query = query.Where(e => EF.Functions.Like(e.FirstName.ToLower(), "%" + filterFullName + "%")
                                         || EF.Functions.Like(e.LastName, "%" + filterFullName + "%")
                                         || (!string.IsNullOrWhiteSpace(e.MiddleName) &
                                             EF.Functions.Like(e.MiddleName, "%" + filterFullName + "%")
                                         ));
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
            BaseDataResponse<GetEmployeeViewModel> dataResponse;

            var employee = await EmployeeById(id);

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
            BaseDataResponse<EditEmployeeViewModel> dataResponse;

            var employeeExists = _unitOfWork.Employees.Any(predicate: e =>
                e.Phone.ToLower() == editEmployeeViewModel.Phone.ToLower() ||
                e.Email.ToLower() == editEmployeeViewModel.Email.ToLower()
            );

            if (employeeExists)
            {
                dataResponse = BaseDataResponse<EditEmployeeViewModel>.Fail(editEmployeeViewModel, new ErrorModel("Phone number already exists"));

            }
            else
            {
                var employee = _mapper.Map<Employee>(editEmployeeViewModel);

                if (editEmployeeViewModel.Photo != null)
                {
                    employee.PhotoPath = await _fileService.UploadPhoto(FileService.EmployeePhotoFolderPath, editEmployeeViewModel.Photo, true);
                }
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

            var generateNewPassword = RandomGenerator.GenerateNewPassword();
            var createUserResult = await _userService.CreateAsync(user, generateNewPassword);

            if (!createUserResult.Succeeded)
                return BaseDataResponse<UserViewModel>.Fail(null, createUserResult.Errors.ToErrorModels());

            employee.UserId = user.Id;

            await _unitOfWork.CommitAsync();
            await _emailSender.SendEmailToEmployeeAsync(user, "User created", $"Account was created, your pass {generateNewPassword}");

            return BaseDataResponse<UserViewModel>.Success(_mapper.Map<UserViewModel>(user));

        }

        public async Task<BaseDataResponse<EditEmployeeViewModel>> EditAsync(EditEmployeeViewModel editEmployeeViewModel)
        {
            BaseDataResponse<EditEmployeeViewModel> dataResponse;

            var employeExists = await _unitOfWork.Employees.GetDbSet().FirstOrDefaultAsync(e => e.Id == editEmployeeViewModel.Id);

            if (employeExists == null)
            {
                dataResponse = BaseDataResponse<EditEmployeeViewModel>.NotFound(editEmployeeViewModel, new ErrorModel("Not found employee"));
            }
            else
            {
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

                var checkEmailUnique = await _unitOfWork.Employees
                    .GetDbSet()
                    .AnyAsync(e => e.Id != editEmployeeViewModel.Id
                                   && e.Phone.ToLower() == editEmployeeViewModel.Phone.ToLower()
                                   && e.Email.ToLower() == editEmployeeViewModel.Email.ToLower());
                if (checkEmailUnique)
                {
                    dataResponse = BaseDataResponse<EditEmployeeViewModel>.Fail(editEmployeeViewModel, new ErrorModel("Duplicate phone number and Email!"));
                }
                else
                {
                    var employee = _mapper.Map<EditEmployeeViewModel, Employee>(editEmployeeViewModel, employeExists);
                    if (editEmployeeViewModel.Photo != null)
                    {
                        employee.PhotoPath = await _fileService.UploadPhoto(FileService.EmployeePhotoFolderPath, editEmployeeViewModel.Photo, true);
                    }
                    _unitOfWork.Employees.Update(employee);

                    await _unitOfWork.CommitAsync();

                    dataResponse = BaseDataResponse<EditEmployeeViewModel>.Success(_mapper.Map<EditEmployeeViewModel>(employee));
                }
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

            await EditAsync(_mapper.Map<EditEmployeeViewModel>(employee));

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

            _userService.LockUser(employee.User,GuidExtensions.FULL_GUID);

            await _unitOfWork.CommitAsync();

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

            _userService.UnlockUser(employee.User);
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
                    response = BaseResponse.Fail(new ErrorModel("Passport number must be unique"));
                }
                else
                {
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
            var emp = await _unitOfWork.Employees.GetDbSet().Include( p => p.Position).FirstOrDefaultAsync(
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

            var orderedQuery = queryVm.OrderBy(sortRule, p => p.FullName );

            var all = await orderedQuery.ToListAsync();

            return _mapper.Map<IList<ExportExcelViewModel>>(all);
        }
    }
}