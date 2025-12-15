using CryptoJackpotService.Models.DTO.Prize;
using CryptoJackpotService.Models.Enums;

namespace CryptoJackpotService.Models.DTO.Lottery;

public class LotteryDto
{
    public Guid Id { get; set; }
    public string LotteryNo { get; set; } = null!;
    public string Title { get; set; } = null!;
    public string Description { get; set; } = null!;
    public int MinNumber { get; set; }
    public int MaxNumber { get; set; }
    public int TotalSeries { get; set; }
    public decimal TicketPrice { get; set; }
    public int MaxTickets { get; set; }
    public int SoldTickets { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public LotteryStatus Status { get; set; }
    public LotteryType Type { get; set; }
    public string Terms { get; set; } = null!;
    public bool HasAgeRestriction { get; set; }
    public int? MinimumAge { get; set; }
    public List<string> RestrictedCountries { get; set; } = new();
    public List<PrizeDto> Prizes { get; set; } = new();
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}

