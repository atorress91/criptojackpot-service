namespace CryptoJackpotService.Data.Database.Models;

public class PrizeTier : BaseEntity
{
    public Guid Id { get; set; }
    public Guid LotteryId { get; set; }
    public Guid PrizeId { get; set; }
    public int Tier { get; set; }
    public int NumberOfWinners { get; set; }

    public virtual Lottery Lottery { get; set; } = null!;
    public virtual Prize Prize { get; set; } = null!;
}