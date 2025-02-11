namespace CryptoJackpotService.Data.Database.Enum;

public enum TransactionStatus
{
    Pending,
    Processing,
    Completed,
    Failed,
    Refunded,
    Cancelled
}