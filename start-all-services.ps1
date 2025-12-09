# =============================================================================
# CryptoJackpot Service - Script de inicio completo
# =============================================================================

Write-Host ""
Write-Host "╔════════════════════════════════════════════════════════════════╗" -ForegroundColor Cyan
Write-Host "║     CryptoJackpot Service - Iniciando Infraestructura         ║" -ForegroundColor Cyan
Write-Host "╚════════════════════════════════════════════════════════════════╝" -ForegroundColor Cyan
Write-Host ""

# =============================================================================
# 1. Verificar Docker
# =============================================================================
Write-Host "🔍 [1/6] Verificando Docker..." -ForegroundColor Yellow
docker --version 2>&1 | Out-Null
if ($LASTEXITCODE -ne 0) {
    Write-Host "❌ ERROR: Docker no está instalado o no está corriendo" -ForegroundColor Red
    Write-Host "   Por favor, inicia Docker Desktop e intenta de nuevo" -ForegroundColor Red
    exit 1
}
Write-Host "✅ Docker está disponible" -ForegroundColor Green

# =============================================================================
# 2. Detener contenedores existentes (si hay)
# =============================================================================
Write-Host ""
Write-Host "🛑 [2/6] Limpiando contenedores existentes..." -ForegroundColor Yellow
$existingContainers = docker ps -a --filter "name=kafka" --filter "name=zookeeper" -q
if ($existingContainers) {
    Write-Host "   Deteniendo contenedores anteriores..." -ForegroundColor Gray
    docker-compose -f docker-compose.kafka.yml down 2>&1 | Out-Null
    Write-Host "✅ Contenedores anteriores detenidos" -ForegroundColor Green
} else {
    Write-Host "✅ No hay contenedores previos" -ForegroundColor Green
}

# =============================================================================
# 3. Iniciar Kafka y Zookeeper
# =============================================================================
Write-Host ""
Write-Host "🚀 [3/6] Iniciando Kafka y Zookeeper..." -ForegroundColor Yellow
docker-compose -f docker-compose.kafka.yml up -d

if ($LASTEXITCODE -eq 0) {
    Write-Host "✅ Contenedores iniciados correctamente" -ForegroundColor Green
} else {
    Write-Host "❌ ERROR: No se pudo iniciar Kafka" -ForegroundColor Red
    Write-Host "   Ejecuta: docker-compose -f docker-compose.kafka.yml logs" -ForegroundColor Red
    exit 1
}

# =============================================================================
# 4. Esperar a que Kafka esté listo
# =============================================================================
Write-Host ""
Write-Host "⏳ [4/6] Esperando a que Kafka esté listo..." -ForegroundColor Yellow
Write-Host "   Esto puede tomar 15-30 segundos..." -ForegroundColor Gray

$maxAttempts = 30
$attempt = 0
$kafkaReady = $false

while ($attempt -lt $maxAttempts -and -not $kafkaReady) {
    Start-Sleep -Seconds 1
    $attempt++
    
    # Verificar si Kafka responde
    $kafkaStatus = docker exec kafka kafka-broker-api-versions --bootstrap-server localhost:9092 2>&1
    if ($LASTEXITCODE -eq 0) {
        $kafkaReady = $true
        Write-Host "✅ Kafka está listo (después de $attempt segundos)" -ForegroundColor Green
    } else {
        Write-Host "   Intento $attempt/$maxAttempts..." -ForegroundColor Gray
    }
}

if (-not $kafkaReady) {
    Write-Host "⚠️  Kafka tardó más de lo esperado, pero continuando..." -ForegroundColor Yellow
}

# =============================================================================
# 5. Crear Topics necesarios
# =============================================================================
Write-Host ""
Write-Host "📝 [5/6] Creando topics de Kafka..." -ForegroundColor Yellow

$topics = @(
    "user-created-events",
    "password-reset-events"
)

foreach ($topicName in $topics) {
    Write-Host "   Creando topic: $topicName..." -ForegroundColor Gray
    docker exec kafka kafka-topics --create --if-not-exists --bootstrap-server localhost:9092 --replication-factor 1 --partitions 1 --topic $topicName 2>&1 | Out-Null
    if ($LASTEXITCODE -eq 0) {
        Write-Host "   ✅ $topicName" -ForegroundColor Green
    } else {
        Write-Host "   ⚠️  $topicName (puede que ya exista)" -ForegroundColor Yellow
    }
}

# Listar topics creados
Write-Host ""
Write-Host "   Topics disponibles:" -ForegroundColor Cyan
docker exec kafka kafka-topics --list --bootstrap-server localhost:9092

# =============================================================================
# 6. Verificar servicios
# =============================================================================
Write-Host ""
Write-Host "📊 [6/6] Estado de los servicios:" -ForegroundColor Yellow
Write-Host ""
docker ps --filter "name=kafka" --filter "name=zookeeper" --format "table {{.Names}}\t{{.Status}}\t{{.Ports}}"

# =============================================================================
# Información adicional
# =============================================================================
Write-Host ""
Write-Host "╔════════════════════════════════════════════════════════════════╗" -ForegroundColor Green
Write-Host "║                  ✅ Servicios Iniciados                        ║" -ForegroundColor Green
Write-Host "╚════════════════════════════════════════════════════════════════╝" -ForegroundColor Green
Write-Host ""
Write-Host "📍 Servicios disponibles:" -ForegroundColor Cyan
Write-Host "   • Kafka:       localhost:9092" -ForegroundColor White
Write-Host "   • Zookeeper:   localhost:2181" -ForegroundColor White
Write-Host "   • Kafka UI:    http://localhost:8080" -ForegroundColor White
Write-Host ""
Write-Host "🎯 Próximos pasos:" -ForegroundColor Cyan
Write-Host "   1. Abre Kafka UI: http://localhost:8080" -ForegroundColor White
Write-Host "   2. Inicia la API:" -ForegroundColor White
Write-Host "      cd CryptoJackpotService.Api" -ForegroundColor Gray
Write-Host "      dotnet run" -ForegroundColor Gray
Write-Host "   3. Inicia el Worker:" -ForegroundColor White
Write-Host "      cd CryptoJackpotService.Worker" -ForegroundColor Gray
Write-Host "      dotnet run" -ForegroundColor Gray
Write-Host ""
Write-Host "💡 Comandos útiles:" -ForegroundColor Cyan
Write-Host "   • Ver logs:        docker-compose -f docker-compose.kafka.yml logs -f" -ForegroundColor Gray
Write-Host "   • Detener todo:    docker-compose -f docker-compose.kafka.yml down" -ForegroundColor Gray
Write-Host "   • Ver tópicos:     docker exec -it kafka kafka-topics --list --bootstrap-server localhost:9092" -ForegroundColor Gray
Write-Host ""
Write-Host "🎉 ¡Todo listo! Puedes empezar a desarrollar" -ForegroundColor Green
Write-Host ""

