namespace CryptoJackpotService.Models.Request.LotteryNumber;

public class ReserveNumbersRequest
{
    public Guid TicketId { get; set; }
    public List<int> Numbers { get; set; } = [];
    public int Series { get; set; }
}

