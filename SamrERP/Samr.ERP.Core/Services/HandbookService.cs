using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Samr.ERP.Core.Interfaces;
using Samr.ERP.Core.Models;
using Samr.ERP.Core.Models.ErrorModels;
using Samr.ERP.Core.Models.ResponseModels;
using Samr.ERP.Core.Stuff;
using Samr.ERP.Core.ViewModels.Handbook;
using Samr.ERP.Infrastructure.Data.Contracts;
using Samr.ERP.Infrastructure.Entities;

namespace Samr.ERP.Core.Services
{
    public class HandbookService : IHandbookService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public HandbookService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        private DbSet<Handbook> GetQuery()
        {
            return _unitOfWork.Handbooks.GetDbSet();
        }

        public async Task<BaseDataResponse<IEnumerable<HandbookViewModel>>> GetAllAsync()
        {
            var handbooks = await GetQuery().ToListAsync();

            var vm = _mapper.Map<IEnumerable<HandbookViewModel>>(handbooks);
            
            return BaseDataResponse<IEnumerable<HandbookViewModel>>.Success(vm);
        }

        public async  Task<bool> ChangeStatus(string name, Guid userId)
        {
            var existsHandbook = await GetQuery().FirstOrDefaultAsync( p => p.Name.ToLower() == name.ToLower());
            if (existsHandbook == null)
            {
                return false;
            }

            existsHandbook.LastModifiedAt = DateTime.Now;
            existsHandbook.LastModifiedUserId = userId;
            existsHandbook.LastModifiedUserFullName = await GetLastModifiedUserFullName(userId);

            await _unitOfWork.CommitAsync();

            return true;
        }

        public async Task<string> GetLastModifiedUserFullName(Guid Id)
        {
            var employeeExists = await _unitOfWork.Employees.GetDbSet().FirstOrDefaultAsync(e => e.UserId == Id);

            return employeeExists.FullName();
        }


    }
}
