using System.Globalization;
using Microsoft.AspNetCore.Mvc.ActionConstraints;

namespace FrontendCoreTest
{
    public class CultureActionConstraint : IActionConstraint
    {
        public string Culture { get; set; }

        public int Order => 0;

        public bool Accept(ActionConstraintContext context)
        {
            return CultureInfo.CurrentCulture.TwoLetterISOLanguageName == Culture;
        }
    }
}