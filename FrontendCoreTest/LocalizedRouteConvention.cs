using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationModels;

namespace FrontendCoreTest
{
    public class LocalizedRouteConvention : IApplicationModelConvention
    {
        public IEnumerable<AttributeRouteModel> GetLocalizedVersionsForARoute(string name)
        {
            if (string.IsNullOrWhiteSpace(name)) yield break;

            foreach (var lang in Languages.AllLanguages)
            {
                string template = $"{name}_{lang}";

                yield return new AttributeRouteModel(new LocalizedRouteAttribute(template)
                {
                    Name = template,
                    Culture = lang.TwoLetterISOLanguageName
                });
            }
        }

        public void Apply(ApplicationModel application)
        {
            foreach (var controller in application.Controllers)
            {
                foreach (var action in controller.Actions)
                {
                    Apply(action);
                }
            }
        }

        [SuppressMessage("ReSharper", "SimplifyLinqExpression")]
        private void Apply(ActionModel action)
        {
            var attributes = action.Attributes.OfType<RouteAttribute>().ToArray();
            if (!attributes.Any())
            {
                return;
            }

            foreach (var attribute in attributes)
            {
                action.Selectors.Clear();

                foreach (var localizedVersion in GetLocalizedVersionsForARoute(attribute.Template))
                {
                    if (!action.Selectors.Any(s => s.AttributeRouteModel.Template == localizedVersion.Template))
                    {
                        action.Selectors.Add(new SelectorModel
                        {
                            AttributeRouteModel = localizedVersion,
                            ActionConstraints =
                            {
                                new CultureActionConstraint
                                {
                                    Culture = ((LocalizedRouteAttribute) localizedVersion.Attribute).Culture
                                }
                            }
                        });
                    }
                }
            }
        }
    }
}