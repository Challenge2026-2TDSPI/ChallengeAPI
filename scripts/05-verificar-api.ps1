. "$PSScriptRoot/config.ps1"
$appHost = Invoke-Azure -Arguments @('webapp','show','--name',$WebAppName,'--resource-group',$ResourceGroup,'--query','defaultHostName','--output','tsv')
function Test-Endpoint {
    param([string]$Path, [switch]$Oracle)
    for ($attempt = 1; $attempt -le 12; $attempt++) {
        try {
            $response = Invoke-WebRequest -Uri "https://$appHost$Path" -UseBasicParsing -TimeoutSec 30
            if ($Oracle) {
                $health = $response.Content | ConvertFrom-Json
                if ($health.status -ne 'Healthy') { throw 'Oracle indisponivel.' }
            } else {
                $document = $response.Content | ConvertFrom-Json
                if (-not $document.openapi) { throw 'OpenAPI indisponivel.' }
            }
            return
        } catch {
            if ($attempt -eq 12) { throw "Falha ao verificar $Path. Consulte os logs do Web App." }
            Start-Sleep -Seconds 10
        }
    }
}
Test-Endpoint -Path '/swagger/v1/swagger.json'
Test-Endpoint -Path '/health' -Oracle
Write-Host "API e Oracle responderam. Swagger: https://$appHost/swagger"
