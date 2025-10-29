# =============================================================================
# CryptoJackpot Service - Ver logs de Kafka
# =============================================================================

Write-Host ""
Write-Host "╔════════════════════════════════════════════════════════════════╗" -ForegroundColor Cyan
Write-Host "║          CryptoJackpot Service - Logs de Kafka                ║" -ForegroundColor Cyan
Write-Host "╚════════════════════════════════════════════════════════════════╝" -ForegroundColor Cyan
Write-Host ""
Write-Host "📋 Mostrando logs en tiempo real..." -ForegroundColor Yellow
Write-Host "   Presiona Ctrl+C para salir" -ForegroundColor Gray
Write-Host ""

docker-compose -f docker-compose.kafka.yml logs -f

