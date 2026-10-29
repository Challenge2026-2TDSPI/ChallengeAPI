. "$PSScriptRoot/config.ps1"
$planId = Invoke-Azure -Arguments @('appservice','plan','show','--name',$PlanName,'--resource-group',$ResourceGroup,'--query','id','--output','tsv')
$currentPlan = Invoke-Azure -Arguments @('webapp','list','--resource-group',$ResourceGroup,'--query',"[?name=='$WebAppName'].serverFarmId | [0]",'--output','tsv')
if (-not $currentPlan) {
    Invoke-Azure -Arguments @('webapp','create','--name',$WebAppName,'--resource-group',$ResourceGroup,'--plan',$planId,'--runtime','DOTNETCORE:8.0','--only-show-errors','--output','none')
} elseif ($currentPlan -ne $planId) {
    throw 'O Web App existente esta associado a outro plano.'
}
Invoke-Azure -Arguments @('webapp','config','set','--name',$WebAppName,'--resource-group',$ResourceGroup,'--linux-fx-version','DOTNETCORE|8.0','--startup-file','dotnet ChallengeAPI.dll','--min-tls-version','1.2','--only-show-errors','--output','none')
Invoke-Azure -Arguments @('webapp','update','--name',$WebAppName,'--resource-group',$ResourceGroup,'--https-only','true','--only-show-errors','--output','none')
Write-Host "Web App pronto: $WebAppName"
if ($env:TF_BUILD) {
    Write-Host "##vso[task.setvariable variable=webAppName;isOutput=true]$WebAppName"
}
