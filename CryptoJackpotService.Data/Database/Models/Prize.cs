using CryptoJackpotService.Data.Database.Enum;

namespace CryptoJackpotService.Data.Database.Models;

/*Premios*/
public class Prize : BaseEntity
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;
    public decimal EstimatedValue { get; set; }
    public PrizeType Type { get; set; }
    public string MainImageUrl { get; set; } = null!;
    public List<PrizeImage> AdditionalImages { get; set; } = null!;
    public Dictionary<string, string> Specifications { get; set; } = null!;
    public decimal? CashAlternative { get; set; }
    public bool IsDeliverable { get; set; }
    public bool IsDigital { get; set; }
}