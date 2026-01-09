# Script PowerShell para build e push das imagens Docker
# Alunos System - Build and Push Script

param(
    [string]$DockerUser = "rafaeldev",
    [string]$Version = "latest",
    [switch]$SkipBuild,
    [switch]$SkipPush
)

$ErrorActionPreference = "Stop"

# Cores para output
function Write-ColorOutput {
    param(
        [string]$Message,
        [string]$Color = "White"
    )
    Write-Host $Message -ForegroundColor $Color
}

Write-ColorOutput "=== Build e Push das Imagens Alunos System ===" "Cyan"
Write-Host ""

# Configurações
$ApiImage = "$DockerUser/alunos-api"
$WebImage = "$DockerUser/alunos-web"
$ScriptDir = Split-Path -Parent $MyInvocation.MyCommand.Path
$ApiDir = $ScriptDir
$WebDir = Join-Path (Split-Path -Parent $ScriptDir) "alunos-web\frontend"

# Build API
if (-not $SkipBuild) {
    Write-ColorOutput "1. Building API image..." "Blue"
    Set-Location $ApiDir
    
    try {
        docker build -t "${ApiImage}:${Version}" .
        Write-ColorOutput "✓ API image built successfully" "Green"
    }
    catch {
        Write-ColorOutput "✗ Erro ao construir imagem da API" "Red"
        Write-ColorOutput $_.Exception.Message "Red"
        exit 1
    }
    Write-Host ""

    # Build Frontend
    Write-ColorOutput "2. Building Frontend image..." "Blue"
    Set-Location $WebDir
    
    try {
        docker build -t "${WebImage}:${Version}" .
        Write-ColorOutput "✓ Frontend image built successfully" "Green"
    }
    catch {
        Write-ColorOutput "✗ Erro ao construir imagem do Frontend" "Red"
        Write-ColorOutput $_.Exception.Message "Red"
        exit 1
    }
    Write-Host ""
}
else {
    Write-ColorOutput "Skipping build (use -SkipBuild:$false to build)" "Yellow"
    Write-Host ""
}

# Push imagens
if (-not $SkipPush) {
    # Verificar login no Docker
    Write-ColorOutput "Verificando autenticação Docker..." "Blue"
    try {
        $dockerInfo = docker info 2>&1
        if ($dockerInfo -match "Username") {
            Write-ColorOutput "✓ Docker login OK" "Green"
        }
        else {
            Write-ColorOutput "⚠ Execute 'docker login' primeiro" "Yellow"
            $login = Read-Host "Deseja fazer login agora? (s/n)"
            if ($login -eq "s") {
                docker login
            }
            else {
                Write-ColorOutput "Push cancelado" "Yellow"
                exit 0
            }
        }
    }
    catch {
        Write-ColorOutput "⚠ Não foi possível verificar status do Docker" "Yellow"
    }
    Write-Host ""

    # Push API
    Write-ColorOutput "3. Pushing API image to Docker Hub..." "Blue"
    try {
        docker push "${ApiImage}:${Version}"
        Write-ColorOutput "✓ API image pushed successfully" "Green"
    }
    catch {
        Write-ColorOutput "✗ Erro ao fazer push da imagem da API" "Red"
        Write-ColorOutput $_.Exception.Message "Red"
        exit 1
    }
    Write-Host ""

    # Push Frontend
    Write-ColorOutput "4. Pushing Frontend image to Docker Hub..." "Blue"
    try {
        docker push "${WebImage}:${Version}"
        Write-ColorOutput "✓ Frontend image pushed successfully" "Green"
    }
    catch {
        Write-ColorOutput "✗ Erro ao fazer push da imagem do Frontend" "Red"
        Write-ColorOutput $_.Exception.Message "Red"
        exit 1
    }
    Write-Host ""
}
else {
    Write-ColorOutput "Skipping push (use -SkipPush:$false to push)" "Yellow"
    Write-Host ""
}

# Resumo
Write-ColorOutput "=== Processo concluído com sucesso! ===" "Green"
Write-Host ""
Write-ColorOutput "Imagens disponíveis:" "Cyan"
Write-Host "  - ${ApiImage}:${Version}"
Write-Host "  - ${WebImage}:${Version}"
Write-Host ""
Write-ColorOutput "Para executar o sistema completo:" "Cyan"
Write-Host "  cd $ApiDir"
Write-Host "  docker-compose up -d"
Write-Host ""

# Voltar ao diretório original
Set-Location $ApiDir
