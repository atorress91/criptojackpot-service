﻿namespace CryptoJackpotService.Data.Database.Custom;

public class UserReferralWithStats
{
    public DateTime RegisterDate { get; set; }
    public string UsedSecurityCode { get; set; } = null!;
    public string FullName { get; set; } = null!;
    public string Email { get; set; } = null!;
}