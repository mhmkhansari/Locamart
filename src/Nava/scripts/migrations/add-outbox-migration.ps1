param(
    [Parameter(Mandatory=$true)]
    [string]$MigrationName
)

$ProjectPath = "..\Nava\Infrastructure\Locamart.Nava.Adapter.Masstransit\Locamart.Nava.Adapter.Masstransit.csproj"
$StartupProjectPath = ".\..\Locamart.Host\Locamart.Host.csproj"
$DbContext = "LocamartNavaAdapterMasstransitDbContext"

Write-Host (Resolve-Path $StartupProjectPath)

dotnet ef migrations add $MigrationName --project $ProjectPath --startup-project $StartupProjectPath --context $DbContext