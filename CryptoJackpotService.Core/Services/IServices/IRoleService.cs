using CryptoJackpotService.Models.DTO.Role;
using CryptoJackpotService.Models.Responses;

namespace CryptoJackpotService.Core.Services.IServices;

public interface IRoleService
{
    Task<ResultResponse<IEnumerable<RoleDto>>> GetRolesAsync();
}

