using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Razor;

namespace Samr.ERP.WebApi.Configurations
{
 
    /// <summary>  
    /// My view location expander  
    /// </summary>  
    public class PdfTemplateLocationExpander : IViewLocationExpander
    {

        public IEnumerable<string> ExpandViewLocations(ViewLocationExpanderContext context,
            IEnumerable<string> viewLocations)
        {
            return viewLocations.Select(s => s.Replace("Views", "Templates"));

        }

        public void PopulateValues(ViewLocationExpanderContext context)
        {
            //nothing to do here.  
        }
    }

}
