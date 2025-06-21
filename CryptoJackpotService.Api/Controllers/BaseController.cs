using System.Net;
using CryptoJackpotService.Models.Resources;
using CryptoJackpotService.Models.Responses;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace CryptoJackpotService.Api.Controllers
{
    public class BaseController : ControllerBase
    {
        private IStringLocalizer<SharedResource>? _localizer;
        
        protected IStringLocalizer<SharedResource> Localizer =>
            _localizer ??= HttpContext.RequestServices.GetRequiredService<IStringLocalizer<SharedResource>>();

        protected IActionResult Success(object data)
            => Ok(new ServicesResponse
            {
                Success = true,
                Code = (int)HttpStatusCode.OK,
                Data = data
            });

        protected IActionResult Fail(string messageKey, HttpStatusCode statusCode = HttpStatusCode.BadRequest)
        {
            var localizedMessage = Localizer[messageKey];
            return StatusCode((int)statusCode, new ServicesResponse
            {
                Success = false,
                Code = (int)statusCode,
                Message = localizedMessage
            });
        }
        
        protected IActionResult FailWithParams(string messageKey, object[] parameters, HttpStatusCode statusCode = HttpStatusCode.BadRequest)
        {
            var localizedMessage = Localizer[messageKey, parameters];
            return StatusCode((int)statusCode, new ServicesResponse
            {
                Success = false,
                Code = (int)statusCode,
                Message = localizedMessage
            });
        }
        
        protected IActionResult SuccessPaginated<T>(
            IEnumerable<T> items,
            int totalItems,
            int pageNumber,
            int pageSize)
        {
            var totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);

            var paginatedResponse = new PaginatedResponse<T>
            {
                Items = items,
                TotalItems = totalItems,
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalPages = totalPages
            };

            return Ok(new ServicesResponse
            {
                Success = true,
                Code = (int)HttpStatusCode.OK,
                Data = paginatedResponse
            });
        }
    }
}