using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc.Razor;

namespace Learns.Assembly.Loading {
    public class ViewLocationExpander : IViewLocationExpander
    {
        public IEnumerable<string> ExpandViewLocations(ViewLocationExpanderContext context, IEnumerable<string> viewLocations)
        {
            var customViews = new []{
                "/{2}/Views/{1}/{0}.cshtml"
            };
            return viewLocations.Union(customViews);
        }

        public void PopulateValues(ViewLocationExpanderContext context)
        {
            context.Values["test"] = nameof(ViewLocationExpander);
        }
    }
}