using Notifications.Application.Exceptions;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Notifications.API.Filters
{
    public class RequestValidationAttributeFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.ModelState.IsValid)
            {
                var modelState = context.ModelState;
#pragma warning disable CS8602 // Dereference of a possibly null reference.
                var errors = modelState.Keys.SelectMany(key => modelState[key].Errors.Select(d => new ValidationFailure(key, d.ErrorMessage))).ToList();
#pragma warning restore CS8602 // Dereference of a possibly null reference.
                throw new ValidationException(errors);
            }
            base.OnActionExecuting(context);
        }
    }
}
