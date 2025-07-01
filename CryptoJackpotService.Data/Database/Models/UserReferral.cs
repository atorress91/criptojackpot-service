﻿namespace CryptoJackpotService.Data.Database.Models;

public class UserReferral : BaseEntity
{
    public long Id { get; set; }
    public long ReferrerId { get; set; }
    public long ReferredId { get; set; }
    public string UsedSecurityCode { get; set; } = null!;

    // Navegación: Usuario que hizo el referido
    public User Referrer { get; set; } = null!;

    // Navegación: Usuario que fue referido
    public User Referred { get; set; } = null!;
}