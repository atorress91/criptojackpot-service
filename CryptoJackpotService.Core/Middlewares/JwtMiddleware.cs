using System.Text;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text.Json;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace CryptoJackpotService.Core.Middlewares;
public class JwtMiddleware
{
    private readonly RequestDelegate _next;
    private readonly IConfiguration _configuration;

    public JwtMiddleware(RequestDelegate next, IConfiguration configuration)
    {
        _next = next;
        _configuration = configuration;
    }

    public async Task Invoke(HttpContext context)
    {
        var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
        var endpoint = context.GetEndpoint();
        
        if (endpoint?.Metadata.GetMetadata<IAllowAnonymous>() != null)
        {
            await _next(context);
            return;
        }
        
        if (token != null)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                
                var secretKey = _configuration["AppSettings:JwtSettings:SecretKey"];
                var issuer = _configuration["AppSettings:JwtSettings:Issuer"];
                var audience = _configuration["AppSettings:JwtSettings:Audience"];
                
                if (string.IsNullOrEmpty(secretKey))
                {
                    context.Response.StatusCode = 401;
                    await context.Response.WriteAsync("Invalid token configuration");
                    return;
                }

                var key = Encoding.UTF8.GetBytes(secretKey);
                var principal = tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = true,
                    ValidIssuer = issuer,
                    ValidateAudience = true,
                    ValidAudience = audience,
                    ClockSkew = TimeSpan.Zero
                }, out _
                    );
                
                context.User = principal;
            }
            catch (Exception)
            {
                context.Response.StatusCode = 401;
                await context.Response.WriteAsync(JsonSerializer.Serialize(new { message = "Invalid token" }));
                return;
            }
        }

        await _next(context);
    }
}