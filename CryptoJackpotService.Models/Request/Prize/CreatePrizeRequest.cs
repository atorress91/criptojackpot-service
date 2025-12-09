using CryptoJackpotService.Models.Enums;

namespace CryptoJackpotService.Models.Request.Prize;

public class CreatePrizeRequest
{
    public Guid LotteryId { get; set; }
    public int Tier { get; set; }
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;
    public decimal EstimatedValue { get; set; }
    public PrizeType Type { get; set; }
    public string MainImageUrl { get; set; } = null!;
    public List<PrizeImageRequest> AdditionalImages { get; set; } = new();
    public Dictionary<string, string> Specifications { get; set; } = new();
    public decimal? CashAlternative { get; set; }
    public bool IsDeliverable { get; set; }
    public bool IsDigital { get; set; }
}


