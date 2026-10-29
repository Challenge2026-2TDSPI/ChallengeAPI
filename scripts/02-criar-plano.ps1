. "$PSScriptRoot/config.ps1"
if ($Sku -ne 'F1') { throw 'Somente o plano gratuito F1 e permitido.' }
$planId = Invoke-Azure -Arguments @('appservice','plan','list','--resource-group',$ResourceGroup,'--query',"[?name=='$PlanName'].id | [0]",'--output','tsv')
if ($planId) {
    $planUrl = 'https://management.azure.com' + $planId + '?api-version=2024-11-01'
    $plan = (Invoke-Azure -Arguments @('rest','--method','get','--url',$planUrl,'--query','{sku:sku.name,linux:properties.reserved}','--output','json')) | ConvertFrom-Json
    if ($plan.sku -ne 'F1' -or $plan.linux -ne $true) {
        throw 'O plano existente precisa ser Linux e F1. Nenhum plano pago sera usado.'
    }
} else {
    Invoke-Azure -Arguments @('appservice','plan','create','--name',$PlanName,'--resource-group',$ResourceGroup,'--location',$Location,'--is-linux','--sku',$Sku,'--only-show-errors','--output','none')
}
Write-Host "Plano pronto: $PlanName (F1)"
