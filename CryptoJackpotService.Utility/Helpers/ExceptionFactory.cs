using CryptoJackpotService.Models.Exceptions;

namespace CryptoJackpotService.Utility.Helpers;

public static class ExceptionFactory
{
    public static BadRequestException BadRequest(string msg)
        => new BadRequestException(msg);

    public static NotFoundException NotFound(string msg)
        => new NotFoundException(msg);

    public static InternalServerException InternalError(string msg)
        => new InternalServerException(msg);
}



