namespace CryptoJackpotService.Data.Database.Models;

public class InvoiceDetail : BaseEntity
{
    public Guid Id { get; set; }
    public Guid InvoiceId { get; set; }
    public Guid TicketId { get; set; }
    public decimal UnitPrice { get; set; }
    public int Quantity { get; set; }
    public decimal SubTotal { get; set; }
    public decimal Tax { get; set; }
    public decimal Total { get; set; }
    
    public virtual Invoice Invoice { get; set; } = null!;
    public virtual Ticket Ticket { get; set; } = null!;
}