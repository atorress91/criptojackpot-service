# Script para iniciar Kafka y los servicios de CryptoJackpot

Write-Host "=== CryptoJackpot Service - Kafka Setup ===" -ForegroundColor Green

# Verificar Docker
Write-Host "`n1. Verificando Docker..." -ForegroundColor Yellow
docker --version
if ($LASTEXITCODE -ne 0) {
    Write-Host "ERROR: Docker no está instalado o no está en el PATH" -ForegroundColor Red
    exit 1
}

# Iniciar Kafka
Write-Host "`n2. Iniciando Kafka..." -ForegroundColor Yellow
docker-compose -f docker-compose.kafka.yml up -d

if ($LASTEXITCODE -eq 0) {
    Write-Host "✓ Kafka iniciado correctamente" -ForegroundColor Green
} else {
    Write-Host "ERROR: No se pudo iniciar Kafka" -ForegroundColor Red
    exit 1
}

# Esperar a que Kafka esté listo
Write-Host "`n3. Esperando a que Kafka esté listo..." -ForegroundColor Yellow
Start-Sleep -Seconds 10

# Verificar servicios
Write-Host "`n4. Verificando servicios Docker..." -ForegroundColor Yellow
docker ps --filter "name=kafka" --filter "name=zookeeper" --format "table {{.Names}}\t{{.Status}}\t{{.Ports}}"

# Crear topic si no existe
Write-Host "`n5. Creando topic 'user-events'..." -ForegroundColor Yellow
docker exec kafka kafka-topics --create --topic user-events --bootstrap-server localhost:9092 --partitions 3 --replication-factor 1 --if-not-exists 2>$null

if ($LASTEXITCODE -eq 0) {
    Write-Host "✓ Topic 'user-events' creado/verificado" -ForegroundColor Green
}

Write-Host "`n=== Setup Completo ===" -ForegroundColor Green
Write-Host "`nServicios disponibles:" -ForegroundColor Cyan
Write-Host "  - Kafka: localhost:9092" -ForegroundColor White
Write-Host "  - Kafka UI: http://localhost:8080" -ForegroundColor White
Write-Host "`nPara iniciar los servicios:" -ForegroundColor Cyan
Write-Host "  Terminal 1: cd CryptoJackpotService.Api ; dotnet run" -ForegroundColor White
Write-Host "  Terminal 2: cd CryptoJackpotService.Worker ; dotnet run" -ForegroundColor White
Write-Host "`nPara detener Kafka:" -ForegroundColor Cyan
Write-Host "  docker-compose -f docker-compose.kafka.yml down" -ForegroundColor White

