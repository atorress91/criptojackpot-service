using CryptoJackpotService.Data.Database.Enum;

namespace CryptoJackpotService.Data.Database.Models;

public class Invoice : BaseEntity
{
    public Guid Id { get; set; }
    public long UserId { get; set; }
    public string InvoiceNumber { get; set; } = null!;
    public DateTime InvoiceDate { get; set; }
    public decimal SubTotal { get; set; }
    public decimal Tax { get; set; }
    public decimal Total { get; set; }
    public string? Notes { get; set; }
    public InvoiceStatus Status { get; set; }
    
    public virtual User User { get; set; } = null!;
    public virtual ICollection<InvoiceDetail> Details { get; set; } = null!;
    public virtual Transaction Transaction { get; set; } = null!;
}