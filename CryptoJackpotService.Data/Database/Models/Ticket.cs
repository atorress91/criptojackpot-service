using CryptoJackpotService.Data.Database.Enum;

namespace CryptoJackpotService.Data.Database.Models;

public class Ticket : BaseEntity
{
    public Guid Id { get; set; }
    public Guid LotteryId { get; set; }
    public long UserId { get; set; }
    public decimal PurchaseAmount { get; set; }
    public DateTime PurchaseDate { get; set; }
    public TicketStatus Status { get; set; }
    public string TransactionId { get; set; } = null!;
    public bool IsGift { get; set; }
    public long? GiftRecipientId { get; set; }

    public virtual ICollection<LotteryNumber> SelectedNumbers { get; set; } = null!;
    
    public virtual Lottery Lottery { get; set; } = null!;
    public virtual User User { get; set; } = null!;
    public virtual User? GiftRecipient { get; set; } 
}