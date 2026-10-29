$ErrorActionPreference = 'Stop'
Set-StrictMode -Version Latest
$ResourceGroup = 'rg-challengeapi-sprint4'
$PlanName = 'plan-challengeapi-sprint4'
$WebAppName = 'clyvovet-api-rm562156'
$Location = 'brazilsouth'
$Sku = 'F1'

function Invoke-Azure {
    param([string[]]$Arguments)
    $result = & az @Arguments
    if ($LASTEXITCODE -ne 0) {
        throw 'Comando Azure CLI falhou. Confira o erro acima.'
    }
    return $result
}
