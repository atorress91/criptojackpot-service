# Script para resetear los offsets de Kafka al final (latest)
# Útil cuando hay mensajes viejos que no quieres reprocesar

Write-Host "=== Resetear Offsets de Kafka ===" -ForegroundColor Cyan
Write-Host ""

# Verificar que Kafka esté corriendo
Write-Host "1. Verificando que Kafka esté corriendo..." -ForegroundColor Yellow
$kafkaRunning = docker ps --filter "name=kafka" --format "{{.Names}}" | Select-String -Pattern "kafka"

if (-not $kafkaRunning) {
    Write-Host "❌ ERROR: Kafka no está corriendo" -ForegroundColor Red
    Write-Host "   Ejecuta: .\start-all-services.ps1" -ForegroundColor Yellow
    exit 1
}
Write-Host "✅ Kafka está corriendo" -ForegroundColor Green

# Listar consumer groups
Write-Host ""
Write-Host "2. Consumer groups actuales:" -ForegroundColor Yellow
docker exec kafka kafka-consumer-groups --bootstrap-server localhost:9092 --list

# Resetear offsets del consumer group principal
Write-Host ""
Write-Host "3. Reseteando offsets de 'cryptojackpot-consumer-group'..." -ForegroundColor Yellow
docker exec kafka kafka-consumer-groups --bootstrap-server localhost:9092 --group cryptojackpot-consumer-group --reset-offsets --to-latest --all-topics --execute

# Resetear offsets del consumer group de password reset
Write-Host ""
Write-Host "4. Reseteando offsets de 'cryptojackpot-consumer-group-password-reset'..." -ForegroundColor Yellow
docker exec kafka kafka-consumer-groups --bootstrap-server localhost:9092 --group cryptojackpot-consumer-group-password-reset --reset-offsets --to-latest --all-topics --execute

# Verificar estado final
Write-Host ""
Write-Host "5. Estado final de los consumer groups:" -ForegroundColor Yellow
Write-Host ""
Write-Host "   cryptojackpot-consumer-group:" -ForegroundColor Cyan
docker exec kafka kafka-consumer-groups --bootstrap-server localhost:9092 --group cryptojackpot-consumer-group --describe

Write-Host ""
Write-Host "   cryptojackpot-consumer-group-password-reset:" -ForegroundColor Cyan
docker exec kafka kafka-consumer-groups --bootstrap-server localhost:9092 --group cryptojackpot-consumer-group-password-reset --describe

Write-Host ""
Write-Host "✅ Offsets reseteados exitosamente!" -ForegroundColor Green
Write-Host ""
Write-Host "Ahora puedes reiniciar el Worker y solo procesará mensajes nuevos." -ForegroundColor Cyan

