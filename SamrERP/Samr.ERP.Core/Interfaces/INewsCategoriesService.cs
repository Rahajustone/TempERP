using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Samr.ERP.Core.Models.ResponseModels;
using Samr.ERP.Core.ViewModels.News.Categories;

namespace Samr.ERP.Core.Interfaces
{
    public interface INewsCategoriesService
    {
        Task<BaseDataResponse<NewsCategoriesViewModel>> GetByIdAsync(Guid id);
        Task<BaseDataResponse<IEnumerable<NewsCategoriesViewModel>>> GetAllAsync();
        Task<BaseDataResponse<NewsCategoriesViewModel>> CreateAsync(NewsCategoriesViewModel newsCategoriesViewModel);
        Task<BaseDataResponse<NewsCategoriesViewModel>> UpdateAsync(NewsCategoriesViewModel newsCategoriesViewModel);
    }
}
