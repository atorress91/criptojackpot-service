namespace CryptoJackpotService.Models.DTO.LotteryNumber;

public class LotteryNumberStatsDto
{
    public Guid LotteryId { get; set; }
    public int TotalNumbers { get; set; }
    public int SoldNumbers { get; set; }
    public int AvailableNumbers { get; set; }
    public decimal PercentageSold { get; set; }
}

