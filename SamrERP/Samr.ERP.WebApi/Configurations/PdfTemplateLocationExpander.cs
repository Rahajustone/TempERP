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

            //replace the Views to MyViews..  
            //viewLocations = viewLocations.Select(s => s.Replace("Views", "MyViews"));
            return viewLocations.Select(s => s.Replace("Views", "Templates"));
            var pdfTemplateLocations = viewLocations.Select(s => s.Replace("Views", "Templates/PdfViews"));

            return viewLocations.Union(pdfTemplateLocations);
        }

        public void PopulateValues(ViewLocationExpanderContext context)
        {
            //nothing to do here.  
        }
    }

}
