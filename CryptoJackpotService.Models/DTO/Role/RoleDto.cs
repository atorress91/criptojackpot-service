﻿namespace CryptoJackpotService.Models.DTO.Role;

public class RoleDto
{
    public long Id { get; set; }
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
}