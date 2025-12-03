# Build stage
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copy solution and project files
COPY *.sln .
COPY CryptoJackpotService.Worker/*.csproj CryptoJackpotService.Worker/
COPY CryptoJackpotService.Core/*.csproj CryptoJackpotService.Core/
COPY CryptoJackpotService.Data/*.csproj CryptoJackpotService.Data/
COPY CryptoJackpotService.Ioc/*.csproj CryptoJackpotService.Ioc/
COPY CryptoJackpotService.Messaging/*.csproj CryptoJackpotService.Messaging/
COPY CryptoJackpotService.Models/*.csproj CryptoJackpotService.Models/
COPY CryptoJackpotService.Utility/*.csproj CryptoJackpotService.Utility/

# Restore dependencies
RUN dotnet restore CryptoJackpotService.Worker/CryptoJackpotService.Worker.csproj

# Copy everything else
COPY . .

# Build and publish
WORKDIR /src/CryptoJackpotService.Worker
RUN dotnet publish -c Release -o /app/publish --no-restore

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime
WORKDIR /app

# Create non-root user for security
RUN adduser --disabled-password --gecos "" appuser && chown -R appuser /app
USER appuser

COPY --from=build /app/publish .

ENTRYPOINT ["dotnet", "CryptoJackpotService.Worker.dll"]

