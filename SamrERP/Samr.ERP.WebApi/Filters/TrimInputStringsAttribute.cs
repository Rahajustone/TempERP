﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Samr.ERP.WebApi.Filters
{
    public class TrimInputStringsAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            foreach (var arg in context.ActionArguments)
            {
                if (arg.Value is string)
                {
                    string val = arg.Value as string;
                    if (!string.IsNullOrEmpty(val))
                    {
                        context.ActionArguments[arg.Key] = val.Trim();
                    }

                    continue;
                }

                Type argType = arg.Value.GetType();
                if (!argType.IsClass)
                {
                    continue;
                }

                TrimAllStringsInObject(arg.Value, argType);
            }
        }

        private void TrimAllStringsInObject(object arg, Type argType)
        {
            var stringProperties = argType.GetProperties()
                .Where(p => p.PropertyType == typeof(string));

            foreach (var stringProperty in stringProperties)
            {
                string currentValue = stringProperty.GetValue(arg, null) as string;
                if (!string.IsNullOrEmpty(currentValue))
                {
                    stringProperty.SetValue(arg, currentValue.Trim(), null);
                }
            }
        }
    }
}
