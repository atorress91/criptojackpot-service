namespace CryptoJackpotService.Data.Database.Models;

public class LotteryNumber : BaseEntity
{
    public Guid Id { get; set; }
    public Guid LotteryId { get; set; }
    public int Number { get; set; }
    public int Series { get; set; }
    public bool IsAvailable { get; set; }
    public Guid? TicketId { get; set; }

    public virtual Lottery Lottery { get; set; } = null!;
    public virtual Ticket? Ticket { get; set; }
}