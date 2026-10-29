. "$PSScriptRoot/config.ps1"
$exists = Invoke-Azure -Arguments @('group','exists','--name',$ResourceGroup,'--output','tsv')
if ($exists -ne 'true') {
    Invoke-Azure -Arguments @('group','create','--name',$ResourceGroup,'--location',$Location,'--only-show-errors','--output','none')
}
Write-Host "Grupo pronto: $ResourceGroup"
