namespace CryptoJackpotService.Models.DTO.LotteryNumber;

public class LotteryNumberDto
{
    public Guid Id { get; set; }
    public Guid LotteryId { get; set; }
    public int Number { get; set; }
    public int Series { get; set; }
    public bool IsAvailable { get; set; }
}

