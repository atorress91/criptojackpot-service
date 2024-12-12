using System.Net;
using CryptoJackpotService.Models.Responses;
using Microsoft.AspNetCore.Mvc;

namespace CryptoJackpotService.Api.Controllers;

public class BaseController : ControllerBase
{
    protected static ServicesResponse Success(object data)
        => new()
        {
            Success = true,
            Code    = (int)HttpStatusCode.OK,
            Data    = data
        };

    protected static ServicesResponse Fail(string message)
        => new()
        {
            Success = false,
            Code    = (int)HttpStatusCode.BadRequest,
            Message = message
        };
    
    protected static ServicesResponse SuccessPaginated<T>(
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

        return Success(paginatedResponse);
    }
}