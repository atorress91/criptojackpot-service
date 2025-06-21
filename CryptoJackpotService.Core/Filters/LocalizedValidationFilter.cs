using System.Net;
using CryptoJackpotService.Models.Resources;
using CryptoJackpotService.Models.Responses;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Localization;
using Microsoft.AspNetCore.Mvc;

namespace CryptoJackpotService.Core.Filters;

public class LocalizedValidationFilter : IActionFilter
{
    private readonly IStringLocalizer<SharedResource> _localizer;

    public LocalizedValidationFilter(IStringLocalizer<SharedResource> localizer)
    {
        _localizer = localizer;
    }

    public void OnActionExecuting(ActionExecutingContext context)
    {
        if (!context.ModelState.IsValid)
        {
            var errors = context.ModelState
                .Where(x => x.Value?.Errors.Count > 0)
                .ToDictionary(
                    kvp => kvp.Key,
                    kvp => kvp.Value!.Errors.Select(e => e.ErrorMessage).ToArray()
                );

            var response = new ServicesResponse
            {
                Success = false,
                Code = (int)HttpStatusCode.BadRequest,
                Message = _localizer[ValidationMessages.InvalidRequest],
                Data = errors
            };

            context.Result = new BadRequestObjectResult(response);
        }
    }

    public void OnActionExecuted(ActionExecutedContext context)
    {
        // No action needed
    }
}
