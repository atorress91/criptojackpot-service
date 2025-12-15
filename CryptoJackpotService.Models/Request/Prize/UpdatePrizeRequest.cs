namespace CryptoJackpotService.Models.Request.Prize;

public class UpdatePrizeRequest
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;
    public decimal EstimatedValue { get; set; }
    public string MainImageUrl { get; set; } = null!;
    public List<string> AdditionalImageUrls { get; set; } = null!;
    public Dictionary<string, string> Specifications { get; set; } = null!;
    public decimal? CashAlternative { get; set; }
    public bool IsDeliverable { get; set; }
    public bool IsDigital { get; set; }
}