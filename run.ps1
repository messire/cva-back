param (
    [Parameter(Position=0, Mandatory=$false)]
    [ValidateSet("up", "down", "restart", "build", "logs", "ps", "clean")]
    $Command = "up"
)

$EnvFile = ".env"
$BackCompose = "docker/docker-compose.back.yml"

if (-not (Test-Path $EnvFile)) {
    Write-Error "$EnvFile not found."
    exit 1
}

# Читаем Database__Type из .env
$DbType = ""
Get-Content $EnvFile | ForEach-Object {
    if ($_ -match "Database__Type=(.*)") {
        $DbType = $matches[1].Trim()
    }
}

# Определяем файл БД
$DbCompose = $null
if ($DbType -eq "Postgres") {
    $DbCompose = "docker/docker-compose.postgres.yml"
}
elseif ($DbType -eq "Mongo") {
    $DbCompose = "docker/docker-compose.mongo.yml"
}

Write-Host "Detected Database Type: $DbType" -ForegroundColor Cyan

# Проверка/создание сети
function Ensure-Network {
    $networkName = "docker-network-shared"
    $exists = docker network ls --filter "name=$networkName" -q
    if (-not $exists) {
        Write-Host "Creating network $networkName..."
        docker network create $networkName
    }
}

switch ($Command) {
    "up" {
        Ensure-Network
        if ($DbCompose) {
            Write-Host "Starting Database..." -ForegroundColor Yellow
            docker compose -f $DbCompose --env-file $EnvFile up -d
        }
        Write-Host "Starting Backend..." -ForegroundColor Yellow
        docker compose -f $BackCompose --env-file $EnvFile up -d --build
    }
    "down" {
        Write-Host "Stopping Backend..." -ForegroundColor Yellow
        docker compose -f $BackCompose down
        if ($DbCompose) {
            Write-Host "Stopping Database..." -ForegroundColor Yellow
            docker compose -f $DbCompose down
        }
    }
    "restart" {
        & $PSCommandPath down
        & $PSCommandPath up
    }
    "build" {
        docker compose -f $BackCompose build
    }
    "logs" {
        docker compose -f $BackCompose logs -f
    }
    "ps" {
        Write-Host "Backend Status:" -ForegroundColor Yellow
        docker compose -f $BackCompose ps
        if ($DbCompose) {
            Write-Host "`nDatabase Status:" -ForegroundColor Yellow
            docker compose -f $DbCompose ps
        }
    }
    "clean" {
        docker system prune -f
    }
}
