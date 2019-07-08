﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Samr.ERP.Core.Models.ResponseModels;
using Samr.ERP.Core.ViewModels.News;
using Samr.ERP.Core.ViewModels.News.Categories;

namespace Samr.ERP.Core.Interfaces
{
    public interface INewsService
    {
        Task<BaseDataResponse<EditNewsViewModel>> GetByIdAsync(Guid id);
        Task<BaseDataResponse<IEnumerable<EditNewsViewModel>>> GetAllAsync();
        Task<BaseDataResponse<EditNewsViewModel>> CreateAsync(EditNewsViewModel newsViewModel);
        Task<BaseDataResponse<EditNewsViewModel>> UpdateAsync(EditNewsViewModel newsViewModel);
    }
}