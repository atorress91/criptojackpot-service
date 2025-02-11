using System.Net;
using CryptoJackpotService.Models.Responses;
using Microsoft.AspNetCore.Mvc;

namespace CryptoJackpotService.Api.Controllers;

public class BaseController : ControllerBase
{
    protected IActionResult Success(object data)
        => Ok(new ServicesResponse
        {
            Success = true,
            Code = (int)HttpStatusCode.OK,
            Data = data
        });

    protected IActionResult Fail(string message, HttpStatusCode statusCode = HttpStatusCode.BadRequest)
        => StatusCode((int)statusCode, new ServicesResponse
        {
            Success = false,
            Code = (int)statusCode,
            Message = message
        });
    
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