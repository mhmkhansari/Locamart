param(
    [Parameter(Mandatory=$true)]
    [string]$MigrationName
)

$ProjectPath = "..\Liam\Infrastructure\Locamart.Liam.Adapter.Masstransit\Locamart.Liam.Adapter.Masstransit.csproj"
$StartupProjectPath = ".\..\Locamart.Host\Locamart.Host.csproj"
$DbContext = "LocamartLiamAdapterMasstransitDbContext"

dotnet ef database update $MigrationName --project $ProjectPath --startup-project $StartupProjectPath --context $DbContext