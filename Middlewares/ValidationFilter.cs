using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace ECommerce.Middlewares
{
    public class ValidationFilter : IAsyncActionFilter
    {
            public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
            {
                if (!context.ModelState.IsValid)
                {
                    var BadRequestMessage = context.ModelState.SelectMany(model => model.Value.Errors) // select many will flatten the list into single list of errors
                        .Select(errors => errors.ErrorMessage).ToList();
                    context.Result = new BadRequestObjectResult(BadRequestMessage);
                }
            await next(); // delegate to call the next actionmethod or filter
            }
    }
}
