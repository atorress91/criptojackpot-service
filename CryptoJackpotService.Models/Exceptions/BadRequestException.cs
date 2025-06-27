using System.Net;
namespace CryptoJackpotService.Models.Exceptions;

public class BadRequestException : BaseException
{
    public BadRequestException(string message) : base(message, HttpStatusCode.BadRequest)
    {
    }
}