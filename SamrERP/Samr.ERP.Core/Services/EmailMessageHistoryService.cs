using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using Samr.ERP.Core.Interfaces;
using Samr.ERP.Core.Models;
using Samr.ERP.Core.Models.ResponseModels;
using Samr.ERP.Core.Stuff;
using Samr.ERP.Core.ViewModels.Department;
using Samr.ERP.Core.ViewModels.EmailSetting;
using Samr.ERP.Infrastructure.Data.Contracts;

namespace Samr.ERP.Core.Services
{
    public class EmailMessageHistoryService : IEmailMessageHistoryService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public EmailMessageHistoryService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<BaseDataResponse<PagedList<EmailMessageHistoryLogViewModel>>> GetAllLogAsync(PagingOptions pagingOptions, SortRule sortRule)
        {
            var query = _unitOfWork.EmailMessageHistories.GetDbSet()
                .Include(p => p.EmailSetting)
                .Include(p => p.RecieverUser)
                .Include(p => p.RecieverEMail)
                .OrderByDescending(p => p.CreatedAt);

            var queryVm = query.ProjectTo<EmailMessageHistoryLogViewModel>();

            var orderedQuery = queryVm.OrderBy(sortRule, p => p.RecieverUser);

            var pagedList = await orderedQuery.ToPagedListAsync(pagingOptions);

            return BaseDataResponse<PagedList<EmailMessageHistoryLogViewModel>>.Success(pagedList);
        }
    }
}
