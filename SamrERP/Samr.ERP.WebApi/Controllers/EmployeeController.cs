using System;
using System.Data;
using System.IO;
using System.Threading.Tasks;
using DinkToPdf;
using DinkToPdf.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using Samr.ERP.Core.Auth;
using Samr.ERP.Core.Interfaces;
using Samr.ERP.Core.Models;
using Samr.ERP.Core.Models.ResponseModels;
using Samr.ERP.Core.Services;
using Samr.ERP.Core.Stuff;
using Samr.ERP.Core.ViewModels.Account;
using Samr.ERP.Core.ViewModels.Employee;
using Samr.ERP.WebApi.Services;

namespace Samr.ERP.WebApi.Controllers
{
    [Route("api/[controller]/[action]")]
    [Authorize()]
    [ApiController]
    public class EmployeeController : ApiController
    {
        private readonly IEmployeeService _employeeService;
        private readonly IHtmlTemplateXService _htmlTemplateXService;
        private readonly PdfConverterService _pdfConverterService;

        public EmployeeController(
            IEmployeeService employeeService,
            IHtmlTemplateXService htmlTemplateXService,
            PdfConverterService pdfConverterService,
            IHostingEnvironment host)
        {
            _employeeService = employeeService;
            _htmlTemplateXService = htmlTemplateXService;
            _pdfConverterService = pdfConverterService;
        }

        [HttpGet]
        [Authorize(Roles = Roles.EmployeeAll)]
        public async Task<BaseDataResponse<PagedList<AllEmployeeViewModel>>> All([FromQuery]PagingOptions pagingOptions, [FromQuery]FilterEmployeeViewModel filterEmployeeViewModel, [FromQuery] SortRule sortRule)
        {
            var employee = await _employeeService.AllAsync(pagingOptions, filterEmployeeViewModel, sortRule);
            return Response(employee);
        }

        [HttpGet]
        [Authorize(Roles = Roles.EmployeeAll)]
        public async Task<BaseDataResponse<PagedList<AllLockEmployeeViewModel>>> AllLockedEmployees([FromQuery]PagingOptions pagingOptions, [FromQuery]FilterEmployeeViewModel filterEmployeeViewModel, [FromQuery]SortRule sortRule)
        {
            var employee = await _employeeService.GetAllLockedEmployeeAsync(pagingOptions, filterEmployeeViewModel, sortRule);

            return Response(employee);
        }

        [HttpGet("{id}")]
        [Authorize(Roles = Roles.EmployeeDetails)]
        public async Task<BaseDataResponse<GetEmployeeViewModel>> Get(Guid id)
        {
            var employee = await _employeeService.GetByIdAsync(id);

            return Response(employee);
        }

        [HttpPost]
        [Authorize(Roles = Roles.EmployeeCreate)]
        public async Task<BaseDataResponse<EditEmployeeViewModel>> Create([FromForm] EditEmployeeViewModel editEmployeeViewModel)
        {
            if (ModelState.IsValid)
            {

                var employeeResult = await _employeeService.CreateAsync(editEmployeeViewModel);

                return Response(employeeResult);
            }

            return Response(BaseDataResponse<EditEmployeeViewModel>.Fail(editEmployeeViewModel, null));

        }

        [HttpPost]
        [Authorize(Roles = Roles.EmployeeEdit)]
        public async Task<BaseDataResponse<EditEmployeeViewModel>> Edit(
            [FromForm] EditEmployeeViewModel editEmployeeViewModel)
        {
            if (ModelState.IsValid)
            {
                var employeeResult = await _employeeService.EditAsync(editEmployeeViewModel);
                return Response(employeeResult);
            }

            return Response(BaseDataResponse<EditEmployeeViewModel>.Fail(editEmployeeViewModel, null));
        }

        [HttpPost]
        [Authorize(Roles = Roles.EmployeeEdit)]
        public async Task<BaseDataResponse<UserViewModel>> CreateUser([FromBody] Guid employeeId)
        {
            var createdUserResponse = await _employeeService.CreateUserForEmployee(employeeId);

            return Response(createdUserResponse);
        }

        [HttpPost]
        [Authorize(Roles = Roles.EmployeeEdit)]
        public async Task<BaseResponse> LockEmployee([FromBody] LockEmployeeViewModel lockEmployeeViewModel)
        {
            if (ModelState.IsValid)
            {
                var response = await _employeeService.LockEmployeeAsync(lockEmployeeViewModel);
                return Response(response);
            }
            return Response(BaseResponse.Fail());
        }

        [HttpPost("{id}")]
        [Authorize(Roles = Roles.EmployeeEdit)]
        public async Task<BaseResponse> UnlockEmployee(Guid id)
        {
            if (ModelState.IsValid)
            {
                var response = await _employeeService.UnLockEmployeeAsync(id);
                return Response(response);
            }
            return Response(BaseResponse.Fail());
        }

        [HttpGet("{id}")]
        [Authorize(Roles = Roles.EmployeeDetails)]
        public async Task<BaseDataResponse<GetPassportDataEmployeeViewModel>> GetPassportData(Guid id)
        {
            var passportData = await _employeeService.GetPassportDataAsync(id);

            return Response(passportData);
        }

        [HttpPost]
        [Authorize(Roles = Roles.EmployeeEdit)]
        public async Task<BaseResponse> EditPassportData([FromForm]
            EditPassportDataEmployeeViewModel editPassportDataEmployeeViewModel)
        {
            if (ModelState.IsValid)
            {
                var response = await _employeeService.EditPassportDataAsync(editPassportDataEmployeeViewModel);

                return Response(response);
            }

            return Response(BaseResponse.Fail());
        }

        [HttpGet]
        [Authorize(Roles = Roles.EmployeeAll)]
        public async Task<IActionResult> ExportExcel([FromQuery]FilterEmployeeViewModel filterEmployeeViewModel, [FromQuery] SortRule sortRule)
        {
            var dataTable = new DataTable();
            dataTable.Columns.Add("ФИО", typeof(string));
            dataTable.Columns.Add("Подразделение", typeof(string));
            dataTable.Columns.Add("Должность", typeof(string));
            dataTable.Columns.Add("Номер Телефона", typeof(string));
            dataTable.Columns.Add("Эл. адрес", typeof(string));
            dataTable.Columns.Add("Пользователь", typeof(string));

            var employees = await _employeeService.ExportToExcelAsync(filterEmployeeViewModel, sortRule);

            foreach (var employee in employees)
            {
                var row = dataTable.NewRow();
                row["ФИО"] = employee.FullName;
                row["Подразделение"] = employee.DepartmentName;
                row["Должность"] = employee.PositionName;
                row["Номер Телефона"] = employee.Phone;
                row["Эл. адрес"] = employee.Email;
                row["Пользователь"] = employee.HasAccount;
                dataTable.Rows.Add(row);
            }

            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("Список_сотрудников");
                worksheet.Cells["A1"].LoadFromDataTable(dataTable, PrintHeaders: true).Style.Border.Top.Style = ExcelBorderStyle.Thin;
                worksheet.Cells["A1"].LoadFromDataTable(dataTable, PrintHeaders: true).Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                worksheet.Cells["A1"].LoadFromDataTable(dataTable, PrintHeaders: true).Style.Border.Left.Style = ExcelBorderStyle.Thin;
                worksheet.Cells["A1"].LoadFromDataTable(dataTable, PrintHeaders: true).Style.Border.Right.Style = ExcelBorderStyle.Thin;

                worksheet.Row(1).Style.Font.Bold = true;

                for (var col = 1; col < dataTable.Columns.Count + 1; col++)
                {
                    worksheet.Column(col).AutoFit();
                }

                var fileName = $"Список_сотрудников({DateTime.Now.ToString("dd_MM_yyyy")})";
                return File(package.GetAsByteArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
            }
        }

        [HttpGet("{id}")]
        [Authorize(Roles = Roles.EmployeeDetails)]
        public async Task<IActionResult> DownloadEmployeeCard(Guid id)
        {
            var employeeCardViewModel = await _employeeService.GetEmployeeCardByIdAsync(id);

            if (employeeCardViewModel != null)
            {
                //string photoPath = FileService.GetFullPath(employeeCardViewModel.PhotoPath);

                //employeeCardViewModel.PhotoPath = @"data:image/png;base64," +
                //                              Convert.ToBase64String(System.IO.File.ReadAllBytes(FileService.GetFullPath("sauron.jpg")));

                var html = await _htmlTemplateXService.RenderTemplateAsync("Employee/EmployeeCardTemplate",
                        employeeCardViewModel);


                var fileName = $"employeeCard({DateTime.Now.ToString("dd_MM_yyyy")})";

                var pdfBytes = _pdfConverterService.ConvertToPdf(html);
                return File(pdfBytes, "application/pdf", fileName);


            }

            return NotFound("employee not found");
        }


    }
}
