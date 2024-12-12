using System.ComponentModel.DataAnnotations;
using CryptoJackpotService.Data.Database.Enum;

namespace CryptoJackpotService.Data.Database.Models;

public class RolePermission : BaseEntity
{
    public long RoleId { get; set; }
    public long PermissionId { get; set; }
    public PermissionLevel AccessLevel { get; set; }
    public Role Role { get; set; } = null!;
    public Permission Permission { get; set; } = null!;
}