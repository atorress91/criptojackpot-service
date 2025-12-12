# Build stage
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copy solution and project files
COPY *.sln .
COPY CryptoJackpotService.Api/*.csproj CryptoJackpotService.Api/
COPY CryptoJackpotService.Core/*.csproj CryptoJackpotService.Core/
COPY CryptoJackpotService.Data/*.csproj CryptoJackpotService.Data/
COPY CryptoJackpotService.Ioc/*.csproj CryptoJackpotService.Ioc/
COPY CryptoJackpotService.Messaging/*.csproj CryptoJackpotService.Messaging/
COPY CryptoJackpotService.Models/*.csproj CryptoJackpotService.Models/
COPY CryptoJackpotService.Utility/*.csproj CryptoJackpotService.Utility/

# Restore dependencies
RUN dotnet restore CryptoJackpotService.Api/CryptoJackpotService.Api.csproj

# Copy everything else
COPY . .

# Build and publish
WORKDIR /src/CryptoJackpotService.Api
RUN dotnet publish -c Release -o /app/publish --no-restore

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime
WORKDIR /app

# Instalar curl para healthcheck
RUN apt-get update && apt-get install -y curl && rm -rf /var/lib/apt/lists/*

# Create non-root user for security
RUN adduser --disabled-password --gecos "" appuser && chown -R appuser /app
USER appuser

COPY --from=build /app/publish .

# Health check
HEALTHCHECK --interval=30s --timeout=3s --start-period=5s --retries=3 \
    CMD curl -f http://localhost:8080/health || exit 1

EXPOSE 8080
ENTRYPOINT ["dotnet", "CryptoJackpotService.Api.dll"]

