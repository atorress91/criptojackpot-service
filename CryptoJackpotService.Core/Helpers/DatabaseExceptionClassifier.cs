using System.Reflection;

namespace CryptoJackpotService.Core.Helpers;

/// <summary>
/// Helper para clasificar excepciones de base de datos.
/// Centraliza la lógica de detección para mantener el middleware limpio.
/// Usa reflexión para detectar códigos de error sin depender de librerías específicas.
/// </summary>
public static class DatabaseExceptionClassifier
{
    // Códigos de error numéricos (independientes del idioma del servidor)
    private static readonly HashSet<string> DuplicateKeyCodes = new(StringComparer.OrdinalIgnoreCase)
    {
        "23505",                        // PostgreSQL: unique_violation
        "2601", "2627",                 // SQL Server: duplicate key
        "1062",                         // MySQL: duplicate entry
        "19"                            // SQLite: SQLITE_CONSTRAINT
    };

    private static readonly HashSet<string> TransientErrorCodes = new(StringComparer.OrdinalIgnoreCase)
    {
        "53300",                        // PostgreSQL: too_many_connections
        "53400",                        // PostgreSQL: configuration_limit_exceeded
        "57P01",                        // PostgreSQL: admin_shutdown
        "57P02",                        // PostgreSQL: crash_shutdown
        "57P03",                        // PostgreSQL: cannot_connect_now
        "08001", "08004", "08006",      // SQL Standard: connection exceptions
        "40001",                        // SQL Server: deadlock
        "-2",                           // SQL Server: timeout
        "1205"                          // SQL Server: deadlock victim
    };

    // Fallback: patrones de texto (solo si no se detecta código)
    private static readonly string[] DuplicateKeyTextPatterns =
    [
        "duplicate key",
        "unique constraint",
        "duplicate entry",
        "UNIQUE constraint failed"
    ];

    private static readonly string[] TransientTextPatterns =
    [
        "too many clients",
        "transient failure",
        "connection refused",
        "timeout expired",
        "pool exhausted"
    ];

    public static bool IsDuplicateKeyException(Exception? exception)
    {
        return MatchesErrorCodes(exception, DuplicateKeyCodes) 
            || MatchesTextPatterns(exception, DuplicateKeyTextPatterns);
    }

    public static bool IsTransientException(Exception? exception)
    {
        return MatchesErrorCodes(exception, TransientErrorCodes) 
            || MatchesTextPatterns(exception, TransientTextPatterns);
    }

    /// <summary>
    /// Busca códigos de error usando reflexión para no depender de librerías específicas.
    /// Detecta SqlState (PostgreSQL/Npgsql), Number (SQL Server), ErrorCode (genérico).
    /// </summary>
    private static bool MatchesErrorCodes(Exception? exception, HashSet<string> codes)
    {
        while (exception != null)
        {
            var errorCode = TryGetErrorCode(exception);
            if (errorCode != null && codes.Contains(errorCode))
                return true;

            exception = exception.InnerException;
        }
        return false;
    }

    private static string? TryGetErrorCode(Exception exception)
    {
        var type = exception.GetType();

        // PostgreSQL (Npgsql): SqlState
        var sqlState = type.GetProperty("SqlState", BindingFlags.Public | BindingFlags.Instance);
        if (sqlState?.GetValue(exception) is string state)
            return state;

        // SQL Server: Number
        var number = type.GetProperty("Number", BindingFlags.Public | BindingFlags.Instance);
        if (number?.GetValue(exception) is int num)
            return num.ToString();

        // MySQL: Number (también usa Number)
        // SQLite: ErrorCode
        var errorCode = type.GetProperty("ErrorCode", BindingFlags.Public | BindingFlags.Instance);
        if (errorCode?.GetValue(exception) is int code)
            return code.ToString();

        return null;
    }

    /// <summary>
    /// Fallback: búsqueda por texto del mensaje (menos confiable pero necesario para SQLite y casos edge).
    /// </summary>
    private static bool MatchesTextPatterns(Exception? exception, string[] patterns)
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

