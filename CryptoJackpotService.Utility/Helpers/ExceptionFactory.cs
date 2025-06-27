using CryptoJackpotService.Models.Exceptions;

namespace CryptoJackpotService.Utility.Helpers;

public static class ExceptionFactory
{
    public static BadRequestException BadRequest(string message)
        => new BadRequestException(message);

    public static NotFoundException NotFound(string message)
        => new NotFoundException(message);
}



