using System.Globalization;
using Microsoft.AspNetCore.Http;

namespace CryptoJackpotService.Utility.Helpers;

public static class CultureHelper
{
    public static string GetCurrentCulture(HttpContext context)
    {
        return CultureInfo.CurrentCulture.TwoLetterISOLanguageName;
    }

    public static bool IsSpanish(HttpContext context)
    {
        return GetCurrentCulture(context) == "es";
    }

    public static bool IsEnglish(HttpContext context)
    {
        return GetCurrentCulture(context) == "en";
    }
}