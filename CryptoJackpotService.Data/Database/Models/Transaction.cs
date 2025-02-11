using CryptoJackpotService.Data.Database.Enum;

namespace CryptoJackpotService.Data.Database.Models;

public class Transaction : BaseEntity
{
    public Guid Id { get; set; }
    public long UserId { get; set; }
    public Guid InvoiceId { get; set; }
    public string TransactionNumber { get; set; } = null!;
    public decimal Amount { get; set; }
    public string Currency { get; set; } = null!;
    public TransactionType Type { get; set; }
    public TransactionStatus Status { get; set; }
    public string? PaymentMethod { get; set; }
    public string? PaymentProvider { get; set; }
    public string? ProviderTransactionId { get; set; }
    public string? ErrorCode { get; set; }
    public string? ErrorMessage { get; set; }
    
    public virtual User User { get; set; } = null!;
    public virtual Invoice Invoice { get; set; } = null!;
}