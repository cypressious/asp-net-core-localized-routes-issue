using Microsoft.AspNetCore.Mvc;

namespace FrontendCoreTest
{
    public class LocalizedRouteAttribute : RouteAttribute
    {
        public LocalizedRouteAttribute(string template) : base(template)
        {
        }

        public string Culture { get; set; } = "de";
    }
}