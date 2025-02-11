using CryptoJackpotService.Data.Database.Enum;

namespace CryptoJackpotService.Data.Database.Models;

public class Winner : BaseEntity
{
    public Guid Id { get; set; }
    public Guid LotteryId { get; set; }
    public Guid TicketId { get; set; }
    public long UserId { get; set; }
    public Guid PrizeTierId { get; set; }
    public WinnerStatus Status { get; set; }
    public DateTime WinDate { get; set; }
    public DateTime? ClaimDate { get; set; }
    public string DeliveryAddress { get; set; } = null!;
    public string DeliveryStatus { get; set; } = null!;
    public bool HasSelectedCashAlternative { get; set; }

    public virtual Lottery Lottery { get; set; } = null!;
    public virtual Ticket Ticket { get; set; } = null!;
    public virtual User User { get; set; } = null!;
    public virtual PrizeTier PrizeTier { get; set; } = null!;
}