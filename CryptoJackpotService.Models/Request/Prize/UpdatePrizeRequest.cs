namespace CryptoJackpotService.Models.Request.Prize;

public class UpdatePrizeRequest
{
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;
    public decimal EstimatedValue { get; set; }
    public string MainImageUrl { get; set; } = null!;
    public List<string> AdditionalImageUrls { get; set; } = [];
    public Dictionary<string, string> Specifications { get; set; } = [];
    public decimal? CashAlternative { get; set; }
    public bool IsDeliverable { get; set; }
    public bool IsDigital { get; set; }
}