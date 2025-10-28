using AutoMapper;
using CryptoJackpotService.Core.Services.IServices;
using CryptoJackpotService.Data.Repositories.IRepositories;
using CryptoJackpotService.Models.DTO.Role;
using CryptoJackpotService.Models.Responses;

namespace CryptoJackpotService.Core.Services;

public class RoleService(IMapper mapper, IRoleRepository roleRepository) : BaseService(mapper), IRoleService
{
    public async Task<ResultResponse<IEnumerable<RoleDto>>> GetRolesAsync()
    {
        var roles = await roleRepository.GetAllRoles();
        var rolesDto = Mapper.Map<IEnumerable<RoleDto>>(roles);
        return ResultResponse<IEnumerable<RoleDto>>.Ok(rolesDto);
    }
}

