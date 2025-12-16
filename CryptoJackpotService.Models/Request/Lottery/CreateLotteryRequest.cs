namespace CryptoJackpotService.Models.Request.Lottery;

public class CreateLotteryRequest
{
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public int MinNumber { get; set; }
    public int MaxNumber { get; set; }
    public int TotalSeries { get; set; }
    public decimal TicketPrice { get; set; }
    public int MaxTickets { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public int Status { get; set; }
    public int Type { get; set; }
    public string Terms { get; set; } = string.Empty;
    public bool HasAgeRestriction { get; set; }
    public int? MinimumAge { get; set; }
    public List<string> RestrictedCountries { get; set; } = new();
    public Guid? PrizeId { get; set; }
}