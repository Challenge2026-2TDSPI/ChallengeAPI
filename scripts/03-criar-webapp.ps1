. "$PSScriptRoot/config.ps1"
$planId = Invoke-Azure -Arguments @('appservice','plan','show','--name',$PlanName,'--resource-group',$ResourceGroup,'--query','id','--output','tsv')
$currentPlan = Invoke-Azure -Arguments @('webapp','list','--resource-group',$ResourceGroup,'--query',"[?name=='$WebAppName'].serverFarmId | [0]",'--output','tsv')
if (-not $currentPlan) {
    Invoke-Azure -Arguments @('webapp','create','--name',$WebAppName,'--resource-group',$ResourceGroup,'--plan',$planId,'--runtime','DOTNETCORE:8.0','--only-show-errors','--output','none')
} elseif ($currentPlan -ne $planId) {
    throw 'O Web App existente esta associado a outro plano.'
}
$configPath = [IO.Path]::GetTempFileName()
try {
    $siteConfig = @{
        linuxFxVersion = 'DOTNETCORE|8.0'
        appCommandLine = 'dotnet ChallengeAPI.dll'
        minTlsVersion = '1.2'
    } | ConvertTo-Json
    [IO.File]::WriteAllText($configPath, $siteConfig, (New-Object Text.UTF8Encoding($false)))
    Invoke-Azure -Arguments @('webapp','config','set','--name',$WebAppName,'--resource-group',$ResourceGroup,'--generic-configurations',"@$configPath",'--only-show-errors','--output','none')
} finally {
    Remove-Item -LiteralPath $configPath -Force
}
Invoke-Azure -Arguments @('webapp','update','--name',$WebAppName,'--resource-group',$ResourceGroup,'--https-only','true','--only-show-errors','--output','none')
Write-Host "Web App pronto: $WebAppName"
if ($env:TF_BUILD) {
    Write-Host "##vso[task.setvariable variable=webAppName;isOutput=true]$WebAppName"
}
