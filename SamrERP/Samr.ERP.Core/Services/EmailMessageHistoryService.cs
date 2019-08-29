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
using Samr.ERP.Infrastructure.Entities;

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

        public async Task<BaseDataResponse<PagedList<EmailMessageHistoryLogViewModel>>> GetAllLogAsync(PagingOptions pagingOptions, SortRule sortRule, FilterEmailMessageHistoryLogViewModel emailMessageHistoryLogFilterView)
        {
            var query = _unitOfWork.EmailMessageHistories.GetDbSet()
                .Include(p => p.EmailSetting)
                .Include(p => p.ReceiverUser)
                .Include(p => p.ReceiverEmail)
                .AsQueryable();

            query = FilterEmailMessageHistories(emailMessageHistoryLogFilterView, query);

            var queryVm = query.ProjectTo<EmailMessageHistoryLogViewModel>();

            var orderedQuery = queryVm.OrderBy(sortRule, p => p.CreatedAt);

            var pagedList = await orderedQuery.ToPagedListAsync(pagingOptions);

            return BaseDataResponse<PagedList<EmailMessageHistoryLogViewModel>>.Success(pagedList);
        }

        private static IQueryable<EmailMessageHistory> FilterEmailMessageHistories(
            FilterEmailMessageHistoryLogViewModel emailMessageHistoryLogFilterView, IQueryable<EmailMessageHistory> query)
        {
            if (emailMessageHistoryLogFilterView.FromDate != null)
            {
                var fromDate = Convert.ToDateTime(emailMessageHistoryLogFilterView.FromDate);
                query = query.Where(p => p.CreatedAt.Date >= fromDate);
            }

            if (emailMessageHistoryLogFilterView.ToDate != null)
            {
                var toDate = Convert.ToDateTime(emailMessageHistoryLogFilterView.ToDate);
                query = query.Where(p => p.CreatedAt.Date <= toDate);
            }

            if (emailMessageHistoryLogFilterView.ReceiverName != null)
                query = query.Where(p => EF.Functions.Like(Extension.FullNameToString(p.ReceiverUser.Employee.LastName,
                    p.ReceiverUser.Employee.FirstName, p.ReceiverUser.Employee.MiddleName).ToLower(),
                    "%" + emailMessageHistoryLogFilterView.ReceiverName.ToLower() + "%"));

            if (emailMessageHistoryLogFilterView.SenderEmail != null)
                query = query.Where(p => EF.Functions.Like(p.EmailSetting.Sender.ToLower(),
                    "%" + emailMessageHistoryLogFilterView.SenderEmail.ToLower() + "%"));

            return query;
        }
    }
}
