param(
    [Parameter(Mandatory=$true)]
    [string]$MigrationName
)

$ProjectPath = "..\Liam\Infrastructure\Locamart.Liam.Adapter.Masstransit\Locamart.Liam.Adapter.Masstransit.csproj"
$StartupProjectPath = ".\..\Locamart.Host\Locamart.Host.csproj"
$DbContext = "LocamartLiamAdapterMasstransitDbContext"

Write-Host (Resolve-Path $StartupProjectPath)

dotnet ef migrations add $MigrationName --project $ProjectPath --startup-project $StartupProjectPath --context $DbContext