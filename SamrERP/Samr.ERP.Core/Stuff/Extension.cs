using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Samr.ERP.Core.Models;
using Samr.ERP.Core.Models.ErrorModels;
using Mapper = AutoMapper.Mapper;
//using NLog;

namespace Samr.ERP.Core.Stuff
{
    public static class Extension
    {
     
        public static string ToStringX(this System.Enum enumerate)
        {
            
            var type = enumerate.GetType();
            var fieldInfo = type.GetField(enumerate.ToString());
            var attributes = (DescriptionAttribute[])fieldInfo.GetCustomAttributes(typeof(DescriptionAttribute), false);
            return (attributes.Length > 0) ? attributes[0].Description : enumerate.ToString();
        }
        public static List<String> Descriptions(this System.Enum enumerate)
        {
            var values = System.Enum.GetValues(enumerate.GetType());
            var descriptions = new List<String>(values.Length);

            foreach (var enumVal in values)
            {
                descriptions.Add(((System.Enum)enumVal).ToStringX());
            }

            return descriptions;
        }

        public static IEnumerable<ErrorModel> ToErrorModels(this IEnumerable<IdentityError> identityErrors)
        {
            return identityErrors.Select(p => new ErrorModel(p.Description));
        }

        /// <summary>
        /// if the object this method is called on is not null, runs the given function and returns the value.
        /// if the object is null, returns default(TResult)
        /// </summary>
        public static TResult IfNotNull<T, TResult>(this T target, Func<T, TResult> getValue)
        {
            if (target != null)
                return getValue(target);
            else
                return default(TResult);
        }

        public static string ToStringFormat(this DateTime target)
        {
            return target.ToString("dd-MM-yyyy");
        }

        public static string ToStringFormatCustom(this DateTime target)
        {
            return target.ToString("dd.MM.yyyy");
        }

        public static async Task<PagedList<T>> ToPagedListAsync<T>(this IQueryable<T> source, PagingOptions pagingOptions)
        {
            var count = await source.CountAsync();
            var items = await source.Skip((pagingOptions.Page - 1) * pagingOptions.PageSize).Take(pagingOptions.PageSize).ToListAsync();
            return new PagedList<T>(items, count, pagingOptions.Page,pagingOptions.PageSize);
        }

        public static async Task<PagedList<TDest>> ToMappedPagedListAsync<TSrc,TDest >(this IQueryable<TSrc> source, PagingOptions pagingOptions)
        {
            var paged = await source.ToPagedListAsync(pagingOptions);
            var mappedResult = paged.Items.MapTo<IEnumerable<TDest>>();
            return new PagedList<TDest>(mappedResult, paged.TotalCount, pagingOptions.Page, pagingOptions.PageSize);
        }

        public static TDest MapTo<TDest>(this object src)
        {
            
            return (TDest)Mapper.Map(src, src.GetType(), typeof(TDest));
        }
    }
}
