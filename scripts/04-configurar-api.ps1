. "$PSScriptRoot/config.ps1"
function Get-Setting {
    param([string]$Name, [string]$Default = '', [switch]$Required)
    $value = [Environment]::GetEnvironmentVariable($Name)
    if (-not $value -or $value.StartsWith('$(')) { $value = $Default }
    if ($Required -and [string]::IsNullOrWhiteSpace($value)) {
        throw "Configure a variavel $Name no Azure DevOps."
    }
    return $value
}
$oracle = Get-Setting -Name 'ORACLE_CONNECTION' -Required
$jwt = Get-Setting -Name 'JWT_SECRET_KEY' -Required
if ([Text.Encoding]::UTF8.GetByteCount($jwt) -lt 32) {
    throw 'A chave JWT precisa ter pelo menos 32 bytes.'
}
$settings = @{
    ConnectionStrings__OracleConnection = $oracle
    Jwt__SecretKey = $jwt
    Jwt__Issuer = Get-Setting -Name 'JWT_ISSUER' -Default 'ChallengeAPI'
    Jwt__Audience = Get-Setting -Name 'JWT_AUDIENCE' -Default 'ChallengeAPI'
    ASPNETCORE_ENVIRONMENT = 'Production'
    SCM_DO_BUILD_DURING_DEPLOYMENT = 'false'
}
$mongo = Get-Setting -Name 'MONGO_CONNECTION'
if ($mongo) {
    $settings['MongoDbSettings__ConnectionString'] = $mongo
    $settings['MongoDbSettings__DatabaseName'] = Get-Setting -Name 'MONGO_DATABASE' -Required
}
$privateDir = Join-Path ([IO.Path]::GetTempPath()) ([Guid]::NewGuid().ToString('N'))
try {
    New-Item -ItemType Directory -Path $privateDir | Out-Null
    $identity = [Security.Principal.WindowsIdentity]::GetCurrent().User
    $acl = New-Object Security.AccessControl.DirectorySecurity
    $acl.SetAccessRuleProtection($true, $false)
    $acl.SetOwner($identity)
    $rule = New-Object Security.AccessControl.FileSystemAccessRule($identity, 'FullControl', 'ContainerInherit,ObjectInherit', 'None', 'Allow')
    $acl.AddAccessRule($rule)
    Set-Acl -LiteralPath $privateDir -AclObject $acl
    $settingsPath = Join-Path $privateDir 'settings.json'
    $json = ConvertTo-Json -InputObject $settings
    [IO.File]::WriteAllText($settingsPath, $json, (New-Object Text.UTF8Encoding($false)))
    & az webapp config appsettings set --name $WebAppName --resource-group $ResourceGroup --settings "@$settingsPath" --only-show-errors --output none 1>$null 2>$null
    if ($LASTEXITCODE -ne 0) { throw 'Falha ao salvar configuracoes da API.' }
} catch {
    throw 'Falha ao salvar configuracoes. Confira as variaveis e permissoes no Azure DevOps.'
} finally {
    if (Test-Path -LiteralPath $privateDir) {
        Remove-Item -LiteralPath $privateDir -Recurse -Force
    }
}
Write-Host 'Configuracoes da API salvas.'
