# =============================================================================
# CryptoJackpot Service - Detener todos los servicios
# =============================================================================

Write-Host ""
Write-Host "╔════════════════════════════════════════════════════════════════╗" -ForegroundColor Cyan
Write-Host "║        CryptoJackpot Service - Deteniendo Servicios           ║" -ForegroundColor Cyan
Write-Host "╚════════════════════════════════════════════════════════════════╝" -ForegroundColor Cyan
Write-Host ""

Write-Host "🛑 Deteniendo Kafka, Zookeeper y Kafka UI..." -ForegroundColor Yellow
docker-compose -f docker-compose.kafka.yml down

if ($LASTEXITCODE -eq 0) {
    Write-Host ""
    Write-Host "✅ Todos los servicios detenidos correctamente" -ForegroundColor Green
    Write-Host ""
    Write-Host "📊 Contenedores activos:" -ForegroundColor Cyan
    docker ps
} else {
    Write-Host ""
    Write-Host "⚠️  Hubo un problema al detener los servicios" -ForegroundColor Yellow
    Write-Host "   Intenta: docker-compose -f docker-compose.kafka.yml down -v" -ForegroundColor Gray
}

Write-Host ""

