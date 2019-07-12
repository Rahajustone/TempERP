using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Samr.ERP.Core.Enums;
using Samr.ERP.Core.Models;


namespace Samr.ERP.Core.Stuff
{
    public static class QueryableExtensions
    {
        //public static IQueryable<T> OrderBy<T>(this IQueryable<T> source, SortOptions sortOptions)
        //{
        //    if (string.IsNullOrEmpty(sortOptions?.Sort)) return source;

        //    var expression = source.Expression;
            
        //    var parameter = Expression.Parameter(typeof(T), "x");
        //    var selector = Expression.PropertyOrField(parameter, sortOptions.Sort);
        //    var method = string.Equals(sortOptions.Sort, "desc", StringComparison.OrdinalIgnoreCase)
        //        ? "OrderByDescending"
        //        : "OrderBy";
        //    expression = Expression.Call(typeof(Queryable), method,
        //        new Type[] { source.ElementType, selector.Type },
        //        expression, Expression.Quote(Expression.Lambda(selector, parameter)));


        //    return source.Provider.CreateQuery<T>(expression);
        //}
        public static IQueryable<T> OrderBy<T>(this IQueryable<T> query, SortRule rule)
        {
            if (string.IsNullOrEmpty(rule?.SortProperty)) return query;

            Type type = typeof(T);
            ParameterExpression pe = Expression.Parameter(type, "obj");

            System.Reflection.PropertyInfo propInfo = type.GetProperty(rule.SortProperty);

            //var expr = Expression.Property(pe, propInfo);

            var expr = Expression.MakeMemberAccess(pe, propInfo);
            var orderByExpression = Expression.Lambda(expr, pe);

            MethodCallExpression orderByCallExpression = Expression.Call(typeof(System.Linq.Queryable), rule.SortDir == SortDirection.Ascending ? "OrderBy" : "OrderByDescending",
                new Type[] { type, propInfo.PropertyType }, query.Expression, orderByExpression);

            return query.Provider.CreateQuery<T>(orderByCallExpression);
        }

        public static IQueryable<TSource> OrderBy<TSource, TKey>(this IQueryable<TSource> query, SortRule rule, Expression<Func<TSource, TKey>> keySelector)
        {
            if (string.IsNullOrEmpty(rule?.SortProperty))
                return query.OrderBy(keySelector);

            return query.OrderBy(rule);
        }

        public static IQueryable<TSource> OrderByThenBy<TSource, TKey>(this IQueryable<TSource> query, SortRule rule, params Expression<Func<TSource, TKey>>[] keySelector)
        {
            if (string.IsNullOrEmpty(rule?.SortProperty) && keySelector.Any())
            {
                var sorted = query.OrderBy(keySelector.First());
                foreach (var expression in keySelector.Skip(1))
                {
                    sorted = sorted.ThenBy(expression);
                }
                return sorted;
            }
            return query.OrderBy(rule);
        }
    }
}
