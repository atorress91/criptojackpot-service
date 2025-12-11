namespace CryptoJackpotService.Core.Helpers;

/// <summary>
/// Helper para clasificar excepciones de base de datos.
/// Centraliza la lógica de detección para mantener el middleware limpio.
/// </summary>
public static class DatabaseExceptionClassifier
{
    private static readonly string[] DuplicateKeyPatterns =
    [
        "23505",                        // PostgreSQL unique violation
        "2601", "2627",                 // SQL Server
        "1062",                         // MySQL
        "duplicate key",
        "unique constraint",
        "duplicate entry",
        "violates unique constraint",
        "UNIQUE constraint failed"      // SQLite
    ];

    private static readonly string[] TransientErrorPatterns =
    [
        "53300",                        // PostgreSQL: too many clients
        "53400",                        // PostgreSQL: configuration limit exceeded
        "57P01", "57P02", "57P03",      // PostgreSQL: shutdown/cannot connect
        "too many clients",
        "transient failure",
        "connection refused",
        "connection timed out",
        "pool exhausted",
        "cannot connect"
    ];

    public static bool IsDuplicateKeyException(Exception? exception)
    {
        return MatchesAnyPattern(exception, DuplicateKeyPatterns);
    }

    public static bool IsTransientException(Exception? exception)
    {
        return MatchesAnyPattern(exception, TransientErrorPatterns);
    }

    private static bool MatchesAnyPattern(Exception? exception, string[] patterns)
    {
        while (exception != null)
        {
            var message = exception.Message;
            if (patterns.Any(p => message.Contains(p, StringComparison.OrdinalIgnoreCase)))
                return true;

            exception = exception.InnerException;
        }
        return false;
    }
}

